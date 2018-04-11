using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class SituacaoPlanejamentoBoTest : BaseEntityFrameworkTest
    {
        /// <summary>
        /// Método auxiliar para criar uma instancia de situação planejamento
        /// </summary>
        /// <param name="descricao">descrição da situação planejamento</param>
        /// <param name="tipoSituacao">tipo de situação</param>
        /// <param name="ativo">estado da situação planejamento</param>
        /// <param name="padrao">verificar se a situação é padrão</param>
        /// <returns></returns>
        public static SituacaoPlanejamento CriarSituacaoPlanejamento( string descricao,
            CsTipoPlanejamento tipoSituacao, CsTipoSituacaoPlanejamento ativo, CsPadraoSistema padrao )
        {
            return new SituacaoPlanejamento() { TxDescricao = descricao, CsTipo = tipoSituacao, CsPadrao = padrao, CsSituacao = ativo };
        }

        [TestMethod]
        public void DeveRetirarUmaSituacaoQueEPadraoParaQueNaoSejaPadraoQuandoSolicitado()
        {
            #region Criação do Cenário

            //Criando uma situação planejamento padrão
            CriarSituacaoPlanejamento( "Não iniciado", CsTipoPlanejamento.Planejamento, CsTipoSituacaoPlanejamento.Ativo, CsPadraoSistema.Sim );

            #endregion

            #region Testando regra

            SituacaoPlanejamentoBO.RetirarSituacaoPlanejamentoPadrao( contexto );

            var situacaoPlanejamentoConsultada = SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto );

            #endregion

            #region Validação

            Assert.IsNull( situacaoPlanejamentoConsultada, "Não deve existir uma SituaçãoPlanejamento padrão, pois foi retirada." );

            #endregion
        }
    }
}
