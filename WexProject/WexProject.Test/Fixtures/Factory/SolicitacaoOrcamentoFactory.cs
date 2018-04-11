using System;
using WexProject.BLL.Models.NovosNegocios;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.NovosNegocios;


namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe SolicitacaoOrcamento
    /// </summary>
    public class SolicitacaoOrcamentoFactory : BaseFactory
    {
        /// <summary>
        /// Cria um objeto de SolicitacaoOrcamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="responsavel">Responsável pela Solicitação</param>
        /// <param name="situacao">Situação da Solicitação</param>
        /// <param name="prioridade">Prioridade da Solicitação</param>
        /// <param name="titulo">Título da Solicitação</param>
        /// <param name="prazo">Prazo da Solicitação</param>
        /// <param name="cliente">nome do cliente</param>
        /// <param name="tipo">tipo de solicitação</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de SolicitacaoOrcamento</returns>
        public static SolicitacaoOrcamento CriarSolicitacaoOrcamento(Session session, Colaborador responsavel, ConfiguracaoDocumentoSituacao situacao, CsPrioridade prioridade, string titulo, DateTime prazo, EmpresaInstituicao cliente, TipoSolicitacao tipo, bool save = false)
        {
            SolicitacaoOrcamento solicitacao = new SolicitacaoOrcamento(session);

            solicitacao.Cliente = cliente;
            solicitacao.Responsavel = responsavel;
            solicitacao.Situacao = situacao;
            solicitacao.CsPrioridade = prioridade;
            solicitacao.TxTitulo = titulo;
            solicitacao.DtPrazo = prazo;
            solicitacao.TipoSolicitacao = tipo;

            if (cliente != null)
                solicitacao.Cliente = cliente;

            if (save)
                solicitacao.Save();

            return solicitacao;
        }

        /// <summary>
        /// Alterar um objeto de SolicitacaoOrcamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="solicitacao">Solicitação de Orçamento</param>
        /// <param name="responsavel">Responsável pela Solicitação</param>
        /// <param name="situacao">Situação da Solicitação</param>
        /// <param name="prioridade">Prioridade da Solicitação</param>
        /// <param name="titulo">Título da Solicitação</param>
        /// <param name="prazo">Prazo da Solicitação</param>
        /// <param name="cliente">nome do cliente</param>
        /// <param name="tipo">tipo de solicitação</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de SolicitacaoOrcamento</returns>
        public static SolicitacaoOrcamento AlterarSolicitacaoOrcamento(Session session, SolicitacaoOrcamento solicitacao, Colaborador responsavel, ConfiguracaoDocumentoSituacao situacao, CsPrioridade prioridade, string titulo, DateTime prazo, EmpresaInstituicao cliente, TipoSolicitacao tipo, bool save = false)
        {
            solicitacao.Cliente = cliente;
            solicitacao.Responsavel = responsavel;
            solicitacao.Situacao = situacao;
            solicitacao.CsPrioridade = prioridade;
            solicitacao.TxTitulo = titulo;
            solicitacao.DtPrazo = prazo;
            solicitacao.TipoSolicitacao = tipo;

            if (save)
                solicitacao.Save();

            return solicitacao;
        }

        /// <summary>
        /// Cria um objeto de SolicitacaoOrcamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="responsavel">Responsável pela Solicitação</param>
        /// <param name="situacao">Situação da Solicitação</param>
        /// <param name="prioridade">Prioridade da Solicitação</param>
        /// <param name="titulo">Título da Solicitação</param>
        /// <param name="prazo">Prazo da Solicitação</param>
        /// <param name="cliente">nome do cliente</param>
        /// <param name="save">Indica se é para salvar ou não</param>
        /// <returns>Objeto de SolicitacaoOrcamento</returns>
        public static SolicitacaoOrcamento CriarSolicitacaoOrcamento(Session session, Colaborador responsavel, ConfiguracaoDocumentoSituacao situacao, CsPrioridade prioridade, string titulo, DateTime prazo, EmpresaInstituicao cliente, bool save = false)
        {
            SolicitacaoOrcamento solicitacao = new SolicitacaoOrcamento(session);

            solicitacao.Cliente = cliente;
            solicitacao.Responsavel = responsavel;
            solicitacao.Situacao = situacao;
            solicitacao.CsPrioridade = prioridade;
            solicitacao.TxTitulo = titulo;
            solicitacao.DtPrazo = prazo;

            if (cliente != null)
                solicitacao.Cliente = cliente;

            if (save)
                solicitacao.Save();

            return solicitacao;
        }
    }
}
