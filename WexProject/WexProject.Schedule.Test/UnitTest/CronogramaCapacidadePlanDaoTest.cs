using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Exceptions.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Schedule.Test.Fixtures.Factory;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaCapacidadePlanDaoTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void CriarCapacidadeDePlanejamentoParaOCronogramaAtualQuandoNaoHouverCapacidadeCadastradaParaODiaAtual()
        {
            var situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true );
            var cronograma = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime( 2013, 11, 1 ), new DateTime( 2013, 11, 10 ), true );
            var capacidadePlan = new CronogramaCapacidadePlan()
            {
                Cronograma = cronograma,
                DtDia = new DateTime( 2013, 11, 1 ),
                HorasCapacidade = TimeSpan.Zero,
                HorasDiaAnterior = TimeSpan.Zero,
                HorasPlanejadas = TimeSpan.Zero
            };
            CronogramaCapacidadePlanDao.CriarCapacidadePlanejamento( contexto, cronograma, capacidadePlan );
            var capacidadePlanEsperada = CronogramaCapacidadePlanDao.ConsultarCronogramaCapacidadePlanPorOid( contexto, capacidadePlan.Oid );
            Assert.IsTrue( capacidadePlan.Equals( capacidadePlanEsperada ), "Deveria possuir os mesmos valores" );
        }

        [TestMethod]
        [ExpectedException( typeof( CronogramaCapacidadePlanDataJaCadastradaException ) )]
        public void CriarCapacidadeDePlanejamentoParaOCronogramaAtualQuandoHouverCapacidadeCadastradaParaODiaAtual()
        {
            var situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true );
            var cronograma = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime( 2013, 11, 1 ), new DateTime( 2013, 11, 10 ), true );
            var capacidadePlan = new CronogramaCapacidadePlan()
            {
                Cronograma = cronograma,
                DtDia = new DateTime( 2013, 11, 1 ),
                HorasCapacidade = TimeSpan.Zero,
                HorasDiaAnterior = TimeSpan.Zero,
                HorasPlanejadas = TimeSpan.Zero
            };
            CronogramaCapacidadePlanDao.CriarCapacidadePlanejamento( contexto, cronograma, capacidadePlan );


            var segundaCapacidadePlan = new CronogramaCapacidadePlan()
            {
                Cronograma = cronograma,
                DtDia = new DateTime( 2013, 11, 1 ),
                HorasCapacidade = TimeSpan.Zero,
                HorasDiaAnterior = TimeSpan.Zero,
                HorasPlanejadas = TimeSpan.Zero
            };

            CronogramaCapacidadePlanDao.CriarCapacidadePlanejamento( contexto, cronograma, segundaCapacidadePlan );
        }

        [TestMethod]
        [ExpectedException( typeof( CapacidadePlanejamentoForaDoPeriodoCronogramaException ) )]
        public void CriarCapacidadeDePlanejamentoParaOCronogramaAtualQuandoADataForAnteriorADataDeInicioDoCronograma()
        {
            var situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true );
            var cronograma = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime( 2013, 11, 1 ), new DateTime( 2013, 11, 10 ), true );
            var capacidadePlan = new CronogramaCapacidadePlan()
            {
                Cronograma = cronograma,
                DtDia = new DateTime( 2013, 10, 11 ),
                HorasCapacidade = TimeSpan.Zero,
                HorasDiaAnterior = TimeSpan.Zero,
                HorasPlanejadas = TimeSpan.Zero
            };
            CronogramaCapacidadePlanDao.CriarCapacidadePlanejamento( contexto, cronograma, capacidadePlan );
        }

        [TestMethod]
        [ExpectedException( typeof( CapacidadePlanejamentoForaDoPeriodoCronogramaException ) )]
        public void CriarCapacidadeDePlanejamentoParaOCronogramaAtualQuandoADataForPosteriorADataDeTerminoDoCronograma()
        {
            var situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true );
            var cronograma = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime( 2013, 11, 1 ), new DateTime( 2013, 11, 10 ), true );
            var capacidadePlan = new CronogramaCapacidadePlan()
            {
                Cronograma = cronograma,
                DtDia = new DateTime( 2013, 11, 11 ),
                HorasCapacidade = TimeSpan.Zero,
                HorasDiaAnterior = TimeSpan.Zero,
                HorasPlanejadas = TimeSpan.Zero
            };
            CronogramaCapacidadePlanDao.CriarCapacidadePlanejamento( contexto, cronograma, capacidadePlan );
        }
    }
}
