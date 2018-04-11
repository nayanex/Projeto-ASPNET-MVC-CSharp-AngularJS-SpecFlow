using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.NovosNegocios;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.NovosNegocios;

using DevExpress.Persistent.Validation;
using System.Text.RegularExpressions;
using DevExpress.Persistent.BaseImpl;
using WexProject.BLL.DAOs;
using WexProject.Library.Libs.Xaf;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes unitários para a classe SolicitacaoOrcamento
    /// </summary>
    [TestClass]
    public class SolicitacaoOrcamentoTest : BaseTest
    {
        /// <summary>
        /// Passos para testar a classe SolicitacaoOrcamento
        /// </summary>
        [TestMethod]
        public void SolicitacaoOrcamentoTest_001()
        {
            #region Passo 1

            DateTime dtPrazo = DateTime.Now.AddDays(1);
            string emailCliente = "novocliente@email.com";

            // Colaborador
            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now,
            "nome@fpf.br", "Solicitacao", "Orcamento", "Historico", "nome.completo");

            // Situações
            ConfiguracaoDocumentoSituacao situacaoNaoIniciado = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Não Iniciado", "Green", CsColorDomain.System, true);
            ConfiguracaoDocumentoSituacao situacaoEmAndamento = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Em Andamento", "Red", CsColorDomain.System, true);
            ConfiguracaoDocumentoSituacao situacaoConcluido = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Concluído", "Blue", CsColorDomain.System, true);

            // Cliente
            EmpresaInstituicao cliente = EmpresaInstituicaoFactory.Criar(SessionTest, "Novo Cliente", "FPF",
            emailCliente, "0000-0000", true);

            // Solicitação de Orçamento
            SolicitacaoOrcamento solicitacao = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
                situacaoNaoIniciado, CsPrioridade.Alta, "Teste1", dtPrazo, cliente);

            solicitacao.Reload();

            // Dados do cliente
            Assert.AreEqual(cliente.TxNome, solicitacao.TxContatoCliente,
            "O nome do cliente deveria ser o mesmo contato na Solicitação de Orçamento");
            Assert.AreEqual(cliente.TxEmail, solicitacao.TxEmailContatoCliente,
            "O email do cliente deveria ser o mesmo email do contato na Solicitação de Orçamento");
            Assert.AreEqual(cliente.TxFoneFax, solicitacao.TxFone,
            "O fone do cliente deveria ser o mesmo fone do contato na Solicitação de Orçamento");

            // Emails
            Assert.AreEqual(string.Empty, solicitacao.TxCc,
            "A quantidade de emails para envio com cópia deveria ser 0, pois nenhum email foi cadastrado.");
            Assert.AreEqual(string.Empty, solicitacao.TxCco,
            "A quantidade de emails para envio com cópia oculta deveria ser 0, pois nenhum email foi cadastrado.");

            #endregion

            #region Passo 2

            solicitacao.TxCc = "usuario@fpf.br"; // Adicionando email para envio
            solicitacao.Save(); // Persistindo

            // Verificação de envio de email
            Assert.AreEqual(true, solicitacao._EmailEnviado,
            "O email deveria ter sido enviado");

            // Verificação das datas
            Assert.AreEqual(solicitacao.DtEmissao.Date, solicitacao.DtConclusao.Date,
            "A data de emissão deveria ser a mesma de conclusão");

            // Verificação do código gerado
            Assert.AreEqual("FPF.01/2012", solicitacao.TxCodigo,
            "O código deveria estar de acordo com o padrão");

            // Verificação da quantidade de dias gastos
            Assert.AreEqual(1, solicitacao._DiasGastos,
            "A quantidade de dias gastos deveria ser 1.");

            #endregion

            #region Passo 3

            // Data prazo inválida
            solicitacao.DtPrazo = DateTime.Now.AddDays(-1);
            Assert.AreEqual(ValidationState.Valid, ValidationUtil.GetRuleState(solicitacao,
            "ValidarDtPrazo", DefaultContexts.Save));

            solicitacao.DtPrazo = dtPrazo; // Retornando à data anterior

            #endregion

            #region Passo 4

            // Email inválido
            solicitacao.TxEmailContatoCliente = "email";

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(solicitacao,
            "ValidarEmailContatoCliente", DefaultContexts.Save));

            solicitacao.TxEmailContatoCliente = emailCliente; // Retornando ao email anterior

            #endregion

            #region Passo 5

            // Alteração dos dados do cliente
            solicitacao.TxContatoCliente = "New Client";
            solicitacao.TxEmailContatoCliente = "newclient@email.com";
            solicitacao.TxFone = "1111-1111";

            solicitacao.Save();

            Assert.AreEqual("New Client", cliente.TxNome,
            "O nome deveria ter mudado no objeto de EmpresaInstituicao");

            Assert.AreEqual("newclient@email.com", cliente.TxEmail,
            "O email deveria ter mudado no objeto de EmpresaInstituicao");

            Assert.AreEqual("1111-1111", cliente.TxFoneFax,
            "O fone/fax deveria ter mudado no objeto de EmpresaInstituicao");

            #endregion

            #region Passo 6
            
            // user@fpf.br - Cc
            ConfiguracaoDocumentoSituacaoEmailCc emailCcNaoIniciado =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCc(SessionTest, "user@fpf.br",
            situacaoNaoIniciado, true);

            // user@fpf.br - Cco
            ConfiguracaoDocumentoSituacaoEmailCco emailCcoNaoIniciado =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCco(SessionTest, "user@fpf.br",
            situacaoNaoIniciado, true);

            // user2@fpf.br - Cc
            ConfiguracaoDocumentoSituacaoEmailCc emailCcEmAndamento =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCc(SessionTest, "user2@fpf.br",
            situacaoEmAndamento, true);

            // user2@fpf.br - Cco
            ConfiguracaoDocumentoSituacaoEmailCco emailCcoEmAndamento =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCco(SessionTest, "user2@fpf.br",
            situacaoEmAndamento, true);

            // user3@fpf.br - Cc
            ConfiguracaoDocumentoSituacaoEmailCc emailCcConcluido =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCc(SessionTest, "user3@fpf.br",
            situacaoConcluido, true);

            // user3@fpf.br - Cco
            ConfiguracaoDocumentoSituacaoEmailCco emailCcoConcluido =
            ConfiguracaoDocumentoSituacaoEmailFactory.CriarEmailCco(SessionTest, "user3@fpf.br",
            situacaoConcluido, true);

            situacaoNaoIniciado.ComCopia.Add(emailCcNaoIniciado);
            situacaoNaoIniciado.ComCopiaOculta.Add(emailCcoNaoIniciado);

            situacaoEmAndamento.ComCopia.Add(emailCcEmAndamento);
            situacaoEmAndamento.ComCopiaOculta.Add(emailCcoEmAndamento);

            situacaoConcluido.ComCopia.Add(emailCcConcluido);
            situacaoConcluido.ComCopiaOculta.Add(emailCcoConcluido);

            solicitacao.Situacao = situacaoEmAndamento; // Simular troca de situação
            solicitacao.Situacao = situacaoNaoIniciado; // Selecionando a situação "Não Iniciado"

            // Com Cópia
            Assert.AreEqual(1, solicitacao.TxCc.Split(';').Length, "Deveria ter sido carregado apenas 1 email");

            // Com Cópia Oculta
            Assert.AreEqual(string.Empty, solicitacao.TxCco, "Deveria ter sido carregado nenhum email");

            #endregion

            #region Passo 7

            solicitacao.Situacao = situacaoEmAndamento; // Selecionando a situação "Em Andamento"

            // Com Cópia
            /* Assert.AreEqual(1, solicitacao.ComCopia.Count, "Deveria ter sido carregado apenas 1 email");
             Assert.AreEqual(situacaoEmAndamento.ComCopia[0].TxEmail, solicitacao.ComCopia[0].TxEmail,
             "O email carregado deveria ser o mesmo email da situação");

             // Com Cópia Oculta
             Assert.AreEqual(1, solicitacao.ComCopiaOculta.Count, "Deveria ter sido carregado apenas 1 email");
             Assert.AreEqual(situacaoEmAndamento.ComCopiaOculta[0].TxEmail, solicitacao.ComCopiaOculta[0].TxEmail,
             "O email carregado deveria ser o mesmo email da situação");

            #endregion

            #region Passo 8

            solicitacao.Situacao = situacaoConcluido; // Selecionando a situação "Concluído"

            // Com Cópia
            Assert.AreEqual(1, solicitacao.ComCopia.Count, "Deveria ter sido carregado apenas 1 email");
            Assert.AreEqual(situacaoConcluido.ComCopia[0].TxEmail, solicitacao.ComCopia[0].TxEmail,
            "O email carregado deveria ser o mesmo email da situação");

            // Com Cópia Oculta
            Assert.AreEqual(1, solicitacao.ComCopiaOculta.Count, "Deveria ter sido carregado apenas 1 email");
            Assert.AreEqual(situacaoConcluido.ComCopiaOculta[0].TxEmail, solicitacao.ComCopiaOculta[0].TxEmail,
            "O email carregado deveria ser o mesmo email da situação");

            // Persistindo
            solicitacao.Save();

            // Verificação de envio de email
            Assert.AreEqual(true, solicitacao._EmailEnviado,
            "O email deveria ter sido enviado");

            #endregion

            #region Passo 9

            // Solicitação de Orçamento nova
            SolicitacaoOrcamento solicitacao02 = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoNaoIniciado, CsPrioridade.Alta, "Teste1", dtPrazo, cliente);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(solicitacao02,
            "SolicitacaoOrcamento_TxTitulo_Unique", DefaultContexts.Save));

            #endregion

            #region Passo 10

            solicitacao02 = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoNaoIniciado, CsPrioridade.Alta, "Teste2", dtPrazo, cliente);

            // Novas Solicitações de Orçamento
            SolicitacaoOrcamento solicitacao03 = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoNaoIniciado, CsPrioridade.Alta, "Teste3", dtPrazo, cliente);

            SolicitacaoOrcamento solicitacao04 = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoNaoIniciado, CsPrioridade.Alta, "Teste4", dtPrazo, cliente);

            // Persistência
            solicitacao02.Save();
            solicitacao03.Save();
            solicitacao04.Save();

            // Verificação dos códigos gerados
            Assert.AreEqual("FPF.02/2011", solicitacao02.TxCodigo,
            "O código deveria estar de acordo com o padrão");

            Assert.AreEqual("FPF.03/2011", solicitacao03.TxCodigo,
            "O código deveria estar de acordo com o padrão");

            Assert.AreEqual("FPF.04/2011", solicitacao04.TxCodigo,
            "O código deveria estar de acordo com o padrão");*/

            #endregion
        }


        /// <summary>
        /// Passos para testar editar e salvar solicitação orçamento.
        /// </summary>
        [TestMethod]
        public void SolicitacaoEditarSalvarOrcamentlo()
        {

            //Criar uma solicitação de orçamento com a situação "Em revisão técnica"
            //Inicio     
            //Passo 1
            DateTime dtPrazo = DateTime.Now;
            string data = dtPrazo.ToString("dd/MM/yyyy");
            string emailCliente = "testeEditarSalver@email.com";

            // Colaborador        
            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now,
            "nome@fpf.br", "Solicitacao", "Orcamento", "Historico", "nome.completo");

            // Situações           
            ConfiguracaoDocumentoSituacao situacaoEmrevisaoTecnica = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Em Revisão Técnica", "black", CsColorDomain.System, true);

            EmpresaInstituicao cliente = EmpresaInstituicaoFactory.Criar(SessionTest, "Novo Cliente", "FPF",
                emailCliente, "0000-0000", true);

            //histórico da solicitação de orçamento
            // Solicitação de orçamento
            SolicitacaoOrcamento solicitacao = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
                situacaoEmrevisaoTecnica, CsPrioridade.Alta, "TesteEditarSalvarSolicitação", dtPrazo, cliente);

            solicitacao.Save();
            solicitacao.Reload();

            // Dados do cliente
            //solicitacao.Cliente = cliente;
            string texto = solicitacao._TextoEmail;
            solicitacao.SolicitacaoOrcamentoHistoricos[0].Comentario = "Teste cadastrar solicitação";

            Assert.AreEqual("Em Revisão Técnica", solicitacao.Situacao.TxDescricao, "Visualizar no grind situação foi alterada");
            //testar e-mail
            Assert.IsTrue((new Regex(String.Format(".<td>{0:dd/MM/yyyy}.", dtPrazo))).IsMatch(texto),
                "O email deveria conter a data de criação do histórico");

            Assert.IsTrue((new Regex(".<td>Em Revisão Técnica</td>.")).IsMatch(texto),
            "O email deveria conter a situação em revisão técnica");
            //testa Histórico
            Assert.AreEqual("Em Revisão Técnica", solicitacao.SolicitacaoOrcamentoHistoricos[0].Situacoes.TxDescricao,
            "A situação deveria aparecer no histórico");

            Assert.AreEqual(dtPrazo.ToString("dd/MM/yyyy"), solicitacao.SolicitacaoOrcamentoHistoricos[0].DataHora.ToString("dd/MM/yyyy"),
            "A data atual deveria aparecer no histórico");

            Assert.IsNotNull(solicitacao.SolicitacaoOrcamentoHistoricos[0].Comentario,
            "O comentário deveria aparecer no histórico");

            //passo 2
            //Editar a solicitação de orçamento da pré-condição.
            ConfiguracaoDocumentoSituacao situacaoEntregue = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Entregue", "blue", CsColorDomain.System, true);

            solicitacao.Situacao = situacaoEntregue;
            solicitacao.TxObservacao = "Solicitação foi mudada para entregue";

            solicitacao.Save();
            string texto2 = solicitacao._TextoEmail;
            Assert.AreEqual("Entregue", solicitacao.Situacao.TxDescricao, "Situação deve ter sido alterada para entregue");
            //testa Histórico, com situação = entregue
            Assert.AreEqual("Entregue", solicitacao.SolicitacaoOrcamentoHistoricos[0].Situacoes.TxDescricao,
            "A situação entregue deveria aparecer no histórico");

            Assert.AreEqual(dtPrazo.ToString("dd/MM/yyyy"), solicitacao.SolicitacaoOrcamentoHistoricos[0].DataHora.ToString("dd/MM/yyyy"),
            "A data atual deveria aparecer no histórico");

            Assert.IsNotNull(solicitacao.SolicitacaoOrcamentoHistoricos[0].Comentario,
            "O comentário deveria aparecer no histórico");

            //testar e-mail, com situação = entregue
            Assert.IsTrue((new Regex(String.Format(".<td>{0:dd/MM/yyyy}.", dtPrazo))).IsMatch(texto2),
                "O email deveria conter a data de criação do histórico");
        }

        /// <summary>
        /// método SolicitacaoComentarioOrcamentlo
        /// </summary>
        [TestMethod]
        public void SolicitacaoComentarioOrcamentlo()
        {
            //Criar uma solicitação de orçamento com a situação "Em revisão técnica"
            //Inicio
            //Passo 1
            DateTime dtPrazo = DateTime.Now;
            string data = dtPrazo.ToString("dd/MM/yyyy");
            string emailCliente = "testeEditarSalver@email.com";
            // Colaborador
            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now,
            "nome@fpf.br", "Solicitacao", "Orcamento", "Historico", "nome.completo");
            // Situações
            ConfiguracaoDocumentoSituacao situacaoEmrevisaoTecnica = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Em Revisão Técnica", "black", CsColorDomain.System, true);

            EmpresaInstituicao cliente = EmpresaInstituicaoFactory.Criar(SessionTest, "Novo Cliente", "FPF",
            emailCliente, "0000-0000", true);
            //histórico da solicitação de orçamento
            //Solicitação de orçamento
            SolicitacaoOrcamento solicitacao = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoEmrevisaoTecnica, CsPrioridade.Alta, "TesteEditarSalvarSolicitação", dtPrazo, cliente);
            solicitacao.Save();
            
            //Inicio
            //Testar editar e salvar uma solicitação de orçamento sem alterar a situação
            //CT_9.01.02 - Testar editar e salvar uma solicitação de orçamento sem alterar a situação
            //Inicio
            //Alterar nome do cliente
            solicitacao.Reload();
            solicitacao.Cliente.TxNome = "Nome do cliente alterado";
            solicitacao.Save();
            
            Assert.AreEqual("Em Revisão Técnica", solicitacao.SolicitacaoOrcamentoHistoricos[0].Situacoes.TxDescricao,
                "A situação em revisão técnica deveria aparecer no histórico");

            ConfiguracaoDocumentoSituacao situacaoEntregue = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
                SessionTest, "Entregue", "blue", CsColorDomain.System, true);

            solicitacao.Reload();
            solicitacao.Situacao = situacaoEntregue;
            solicitacao.Save();

            Assert.AreEqual("Entregue", solicitacao.SolicitacaoOrcamentoHistoricos[0].Situacoes.TxDescricao,
            "A situação Entregue deveria aparecer no histórico");

            Assert.IsFalse(string.IsNullOrEmpty(solicitacao.SolicitacaoOrcamentoHistoricos[0].Comentario),
            "O comentário deveria aparecer no histórico");
        }
        
        /// <summary>
        /// método TestarSEOTUsuario
        /// </summary>
        [TestMethod]
        public void TestarSEOTUsuario()
        {
            //Criar uma solicitação de orçamento com a situação "Em revisão técnica"
            //Inicio
            //Passo 1
            DateTime dtPrazo = DateTime.Now;
            string data = dtPrazo.ToString("dd/MM/yyyy");
            string emailCliente = "testeEditarSalver@email.com";
            // Colaborador
            // Criando objetos de usuários que acessarão os cronnogramas

            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now,
            "nome@fpf.br", "Solicitacao", "Orcamento", "Historico", "nome.completo");

            User usuario01 = ColaboradorFactory.CriarUsuario( SessionTest, "nome.completo", "Nome", "Completo",
                "nome@fpf.br", true);

            // Situações
            ConfiguracaoDocumentoSituacao situacaoEmrevisaoTecnica = ConfiguracaoDocumentoSituacaoFactory.CriarConfiguracaoDocumentoSituacao(
            SessionTest, "Em Revisão Técnica", "black", CsColorDomain.System, true);

            EmpresaInstituicao cliente = EmpresaInstituicaoFactory.Criar(SessionTest, "Novo Cliente", "FPF",
            emailCliente, "0000-0000", true);
            //histórico da solicitação de orçamento
            //Solicitação de orçamento
            SolicitacaoOrcamento solicitacao = SolicitacaoOrcamentoFactory.CriarSolicitacaoOrcamento(SessionTest, colaborador,
            situacaoEmrevisaoTecnica, CsPrioridade.Alta, "TesteEditarSalvarSolicitação", dtPrazo, cliente);

            solicitacao.Save();
            solicitacao.Reload();

            solicitacao.Cliente.TxNome = "Nome do cliente alterado";
            solicitacao.Save();

            UsuarioDAO.CurrentUser = colaborador.Usuario;

            Colaborador.RnSalvarUsuarioUltimaSEOT(SessionTest, UsuarioDAO.CurrentUser.Oid, colaborador);

            solicitacao.Save();

            Assert.AreEqual(UsuarioDAO.CurrentUser.Oid, colaborador.ColaboradorUltimoFiltro.LastUsuarioFilterSeot, "O colaborador selecionado é igual a sua última seleção");
        }

        [TestMethod]
        public void TestarCriacaoListaEmailsCCoCC()
        {
        }
    }
}
