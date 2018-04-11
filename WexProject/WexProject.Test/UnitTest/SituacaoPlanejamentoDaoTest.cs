using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class SituacaoPlanejamentoDaoTest : BaseEntityFrameworkTest
    {

        /// <summary>
        /// Método auxiliar para criar uma instancia de situação planejamento
        /// </summary>
        /// <param name="descricao">descrição da situação planejamento</param>
        /// <param name="tipoSituacao">tipo de situação</param>
        /// <param name="ativo">estado da situação planejamento</param>
        /// <param name="padrao">verificar se a situação é padrão</param>
        /// <returns></returns>
        public static SituacaoPlanejamento CriarSituacaoPlanejamento(string descricao,
            CsTipoPlanejamento tipoSituacao,CsTipoSituacaoPlanejamento ativo,CsPadraoSistema padrao)
        {
            return new SituacaoPlanejamento() { TxDescricao = descricao, CsTipo = tipoSituacao, CsPadrao = padrao, CsSituacao = ativo }; 
        }

        [TestMethod]
        public void DeveCriarEArmazenarUmaSituacaoPlanejamento() 
        {
            const string txDescricao = "Não iniciado";
            Guid fixedOid = Guid.NewGuid();
            var situacaoPlanejamento = new SituacaoPlanejamento();
            situacaoPlanejamento.Oid = fixedOid;
            situacaoPlanejamento.TxDescricao = txDescricao;
            situacaoPlanejamento.CsPadrao = CsPadraoSistema.Sim;
            situacaoPlanejamento.CsSituacao = CsTipoSituacaoPlanejamento.Ativo;
            situacaoPlanejamento.CsTipo = CsTipoPlanejamento.Planejamento;
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );
            Assert.AreEqual( 1, contexto.SituacaoPlanejamento.Local.Count,"Deveria conter uma nova situação planejamento" );
            Assert.AreEqual( txDescricao, contexto.SituacaoPlanejamento.Find( fixedOid ).TxDescricao, string.Format( "Deveria ter encontrado uma situação com a seguinte descrição {0}", txDescricao ) );
        }

        [TestMethod]
        public void DeveConsultarASituacaoPlanejamentoERetornarNullQuandoNaoHouverSituacoesPlanejamentoCadastradas() 
        {
            var situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
            Assert.IsNull( situacaoPlanejamento,"Não deveria ter localizado nenhuma situação planejamento" );
        }

        [TestMethod]
        public void DeveConsultarASituacaoPlanejamentoPadraoCadastrada() 
        {
            const string txDescricao = "Não iniciado";
            var situacaoPlanejamento = CriarSituacaoPlanejamento( txDescricao, CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );
            Guid oidSituacaoPlanejamento = situacaoPlanejamento.Oid;
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            var situacaoPlanejamentoEsperada = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
            Assert.IsNotNull( situacaoPlanejamentoEsperada,"Deveria ter encontrado a situação planejamento padrão" );
            Assert.AreEqual( txDescricao, situacaoPlanejamentoEsperada.TxDescricao, string.Format( "A situação planejamento esperada deveria ser {0}", txDescricao ) );
            Assert.AreEqual( oidSituacaoPlanejamento, situacaoPlanejamentoEsperada.Oid, string.Format( "O oid da situação planejamento padrão deveria ser {0}", oidSituacaoPlanejamento ) );
        }

        [TestMethod]
        public void DeveConsultarASituacaoPlanejamentoPadraoQuandoHouveremOutrasSituacoesPlanejamentoCadastradas() 
        {
            //Criando varias situações planejamento
            var situacaoPlanejamento = CriarSituacaoPlanejamento("Não iniciado",CsTipoPlanejamento.Planejamento,CsTipoSituacaoPlanejamento.Ativo,CsPadraoSistema.Não);
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Pronto", CsTipoPlanejamento.Encerramento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Impedido", CsTipoPlanejamento.Impedimento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            //Criando a situação planejamento padrão
            const string txDescricaoSituacaoPadrao = "Em Andamento";
            CsTipoPlanejamento tipoPlanejamentoPadrao = CsTipoPlanejamento.Execução;
            situacaoPlanejamento = CriarSituacaoPlanejamento( txDescricaoSituacaoPadrao, CsTipoPlanejamento.Execução, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );
            Guid oidSituacaoPlanejamentoPadrao = situacaoPlanejamento.Oid;
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            //Pesquisando a situação planejamento padrão
            var situacaoPlanejamentoEsperada = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
            Assert.IsNotNull( situacaoPlanejamentoEsperada, "Deveria ter encontrado a situação planejamento padrão" );
            Assert.AreEqual( txDescricaoSituacaoPadrao, situacaoPlanejamentoEsperada.TxDescricao, string.Format( "A situação planejamento esperada deveria ser {0}", txDescricaoSituacaoPadrao ) );
            Assert.AreEqual( oidSituacaoPlanejamentoPadrao, situacaoPlanejamentoEsperada.Oid, string.Format( "O oid da situação planejamento padrão deveria ser {0}", oidSituacaoPlanejamentoPadrao ) );
            Assert.AreEqual( tipoPlanejamentoPadrao, situacaoPlanejamentoEsperada.CsTipo, string.Format( "O tipo de planejamento padrão deveria ser {0}", tipoPlanejamentoPadrao.ToString() ) );
        }

        [TestMethod]
        public void DeveConsultarSituacaoPlanejamentoPadraoQuandoNaoHouveremSituacoesAtivas() 
        {
            //Criando varias situações planejamento
            var situacaoPlanejamento = CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Pronto", CsTipoPlanejamento.Encerramento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Impedido", CsTipoPlanejamento.Impedimento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            //Pesquisando a situação planejamento padrão
            var situacaoPlanejamentoEsperada = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
            Assert.IsNull( situacaoPlanejamentoEsperada, "Não deveria ter encontrado a situação planejamento padrão pois não existem situações planejamento ativas" );
            Assert.AreEqual( 4, contexto.SituacaoPlanejamento.Local.Count,"Deveriam estar cadastradas 4 situações planejamento" );
        }

        [TestMethod]
        public void DeveConsultarSituacaoPlanejamentoPadraoQuandoNaoHouveremSituacoesPadraoCadastradas()
        {
            //Criando varias situações planejamento
            var situacaoPlanejamento = CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Pronto", CsTipoPlanejamento.Encerramento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            situacaoPlanejamento = CriarSituacaoPlanejamento( "Impedido", CsTipoPlanejamento.Impedimento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            //Pesquisando a situação planejamento padrão
            var situacaoPlanejamentoEsperada = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );
            Assert.IsNull( situacaoPlanejamentoEsperada, "Não deveria ter encontrado a situação planejamento padrão pois não existem situações planejamento ativas" );
            Assert.AreEqual( 4, contexto.SituacaoPlanejamento.Local.Count, "Deveriam estar cadastradas 4 situações planejamento" );
        }

        [TestMethod]
        public void NaoDevePermitirSalvarMaisQueUmaSituacaoPlanejamentoPadrao() 
        {
            //Criando uma situação planejamento padrão
            var situacaoPlanejamento = CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );
            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );
        }

        [TestMethod]
        public void DeveConsultarUmaSituacaoPlanejamentoPorOidQuandoExistirUmaSituacaoPlanejamentoCadastrada()
        {
            #region Criação do Cenário

                //Criando uma situação planejamento padrão
                var situacaoPlanejamento = CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );

                SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            #endregion

            #region Testando regra

                var situacaoPlanejamentoConsultada = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid );

            #endregion

            #region Validação

                Assert.IsNotNull( situacaoPlanejamento, "Não deve ser nulo, pois foi criada uma SituaçãoPlanejamento." );
                Assert.AreEqual( situacaoPlanejamento.Oid, situacaoPlanejamentoConsultada.Oid );

            #endregion
        }

        [TestMethod]
        public void DeveConsultarUmaSituacaoPorTipoQuandoExistirUmaSituacaoCadastradaComOMesmoTipo()
        {
            #region Criação do Cenário

            //Criando uma situação planejamento padrão
            var situacaoPlanejamento = CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );

            SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacaoPlanejamento );

            #endregion

            #region Testando regra

            var situacaoPlanejamentoConsultada = SituacaoPlanejamentoDAO.ConsultarSituacaoPorTipo( CsTipoPlanejamento.Planejamento );

            #endregion

            #region Validação

            Assert.AreEqual( situacaoPlanejamento.Oid, situacaoPlanejamentoConsultada.Oid, "Devem ser iguais, pois são do mesmo Oid." );

            #endregion
        }

        [TestMethod]
        public void DeveConsultarSituacoesPlanejamentoAtivasQuandoHouverSituacoesAtivasEInativasCadastradas()
        {
            #region Criação do Cenário

            List<SituacaoPlanejamento> situacoesPlanejamento = new List<SituacaoPlanejamento>();

            //Criando uma situação planejamento padrão
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim ) );
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Em Andamento", CsTipoPlanejamento.Execução, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não ) );
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não ) );

            for(int i = 0; i < situacoesPlanejamento.Count; i++)
                SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacoesPlanejamento[i] );

            #endregion

            #region Testando regra

            List<SituacaoPlanejamento> situacoesPlanejamentoConsultadas = SituacaoPlanejamentoDAO.ConsultarSituacoesAtivas();

            #endregion

            #region Validação

            Assert.AreEqual( 2, situacoesPlanejamentoConsultadas.Count, "Deve possuir 2 situações, pois foram criadas 2 ativas." );

            #endregion
        }

        [TestMethod]
        public void DeveConsultarSituacoesPlanejamentoInativasQuandoHouverSituacoesInativasCadastradasEAtivas()
        {
            #region Criação do Cenário

            List<SituacaoPlanejamento> situacoesPlanejamento = new List<SituacaoPlanejamento>();

            //Criando uma situação planejamento padrão
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim ) );
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Em Andamento", CsTipoPlanejamento.Execução, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Não ) );
            situacoesPlanejamento.Add( CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento, CsTipoSituacaoPlanejamento.Inativo, CsPadraoSistema.Não ) );

            for(int i = 0; i < situacoesPlanejamento.Count; i++)
                SituacaoPlanejamentoDAO.CriarSituacaoPlanejamento( contexto, situacoesPlanejamento[i] );

            #endregion

            #region Testando regra

            List<SituacaoPlanejamento> situacoesPlanejamentoConsultadas = SituacaoPlanejamentoDAO.ConsultarSituacoesInativas();

            #endregion

            #region Validação

            Assert.AreEqual( 1, situacoesPlanejamentoConsultadas.Count, "Deve possuir 1 situação inativa, pois foi criado somente uma." );

            #endregion
        }
    }
}
