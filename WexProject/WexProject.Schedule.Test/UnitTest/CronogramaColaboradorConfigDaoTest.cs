using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaColaboradorConfigDaoTest : BaseEntityFrameworkTest
    {
		[TestInitialize]
		public void ConstruirContextoPadraoTest()
		{
			SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto , "Não Iniciado" , CsTipoSituacaoPlanejamento.Ativo , CsTipoPlanejamento.Planejamento , CsPadraoSistema.Sim , true );
		}

        [TestMethod]
        public void ConsultarCronogramaColaboradorConfigQuandoColaboradorOuCronogramaNaoExistirTest()
        {
            CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( contexto, "gabriel.matos", Guid.NewGuid() );
            Assert.IsNull( config );
        }

        [TestMethod]
        public void ConsultarColaboradorCronogramaConfigQuandoColaboradorOuCronogramaNaoExistirTest()
        {
            CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( contexto, "gabriel.matos", Guid.NewGuid() );
            Assert.IsNull( config, "Não deveria ter criado um configuração, quando o colaborador ou cronograma não existirem" );
        }

        [TestMethod]
        public void ConsultarCronogramaColaboradorConfigTest()
        {
            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );
            string[] logins = new string[] { "gabriel.matos", "anderson.lins", "adriava.silva", "amanda.silva", "ana.marques" };
            List<Colaborador> colaboradores = new List<Colaborador>();
            Dictionary<string, string> dicionarioConfigs = new Dictionary<string, string>();
            string cor = null;
            for(int i = 0; i < logins.Length; i++)
            {
                colaboradores.Add( ColaboradorFactoryEntity.CriarColaborador( contexto, logins[i], true ) );
                cor = CronogramaColaboradorConfigBo.EscolherCorColaborador( logins[i], cronograma.Oid );
                dicionarioConfigs.Add( logins[i], cor );
            }

            List<CronogramaColaboradorConfig> configs = CronogramaColaboradorConfigDao.ConsultarCronogramaColaboradorConfig( logins.ToList(), cronograma.Oid );
            Dictionary<string, string> resultadoListagemConfigs = configs.ToDictionary( o => o.Colaborador.Usuario.UserName, o => o.Cor.ToString() );
            CollectionAssert.AreEquivalent( dicionarioConfigs, resultadoListagemConfigs );
        }
    }
}
