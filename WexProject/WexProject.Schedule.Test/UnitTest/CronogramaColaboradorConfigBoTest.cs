using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaColaboradorConfigBoTest : BaseEntityFrameworkTest
    {
		[TestInitialize]
		public void ConstruirContextoPadraoTest()
		{
			SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto , "Não Iniciado" , CsTipoSituacaoPlanejamento.Ativo , CsTipoPlanejamento.Planejamento , CsPadraoSistema.Sim , true );
		}

        [TestMethod]
        public void CriarColaboradorCronogramaConfigTest()
        {
            ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );

            CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.SalvarCronogramaColaboradorConfig( contexto, "gabriel.matos", cronograma.Oid );

            Assert.IsNotNull( config, "Deveria ter sido criado o CronogramaColaborador Config para o Colabordor atual" );

            CronogramaColaboradorConfig config2 = CronogramaColaboradorConfigDao.SalvarCronogramaColaboradorConfig( contexto, "gabriel.matos", cronograma.Oid );

            Assert.AreEqual( config.Oid, config2.Oid, "Deveria possuir as mesmas configurações" );
        }

        [TestMethod]
        public void SelecionarCorColaboradorQuandoNaoHouverNaoExistirOColaboradorOuCronogramaTest()
        {
            string cor = CronogramaColaboradorConfigBo.EscolherCorColaborador( "gabriel.matos", Guid.NewGuid() );
            Assert.IsNull( cor, "Não deveria ter criado uma cor pois não existem o colaborador nem o cronograma" );
        }

        [TestMethod]
        public void SelecionarCorColaboradorQuandoHouverColaboradorECronogramaMasNaoHouverCronogramaColaboradorConfigTest()
        {
            ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );
            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );
            string cor = CronogramaColaboradorConfigBo.EscolherCorColaborador( "gabriel.matos", cronograma.Oid );
            Assert.IsNotNull( cor, "Deveria ter criado uma cor para o colaborador" );
            string corSelecionada = CronogramaColaboradorConfigBo.EscolherCorColaborador( "gabriel.matos", cronograma.Oid );
            Assert.AreEqual( cor, corSelecionada, "A cor selecionada deveria ser a mesma cor que foi escolhida da primeira vez" );
        }

        [TestMethod]
        public void SelecionarCorColaboradorTest()
        {
            ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );
            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );
            CronogramaColaboradorConfig config = CronogramaColaboradorConfigDao.SalvarCronogramaColaboradorConfig( contexto, "gabriel.matos", cronograma.Oid );
            string cor = CronogramaColaboradorConfigBo.EscolherCorColaborador( "gabriel.matos", cronograma.Oid );
            Assert.IsNotNull( cor, "Deveria ter selecionado uma cor para o colaborador" );
            string cor2 = CronogramaColaboradorConfigBo.EscolherCorColaborador( "gabriel.matos", cronograma.Oid );
            Assert.AreEqual( cor, cor2, "Ao ser chamado pela segunda vez, deveria retornar a mesma cor escolhida que foi escolhida na primeira vez" );
        }
    }
}
