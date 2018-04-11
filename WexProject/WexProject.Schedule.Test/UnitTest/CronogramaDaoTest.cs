using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Schedule.Test.Fixtures.Factory;
using System.Linq;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaDaoTest : BaseEntityFrameworkTest
    {
        private List<SituacaoPlanejamento> situacoesPlanejamento { get; set; }

        private List<CronogramaTarefa> tarefasImpactadas;

        private DateTime dataAcao;

        [TestInitialize]
        public void CriarContexto()
        {
            situacoesPlanejamento = new List<SituacaoPlanejamento>();
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Cancelado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Impedimento", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Impedimento, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Pronto", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Encerramento, CsPadraoSistema.Não, true ) );
        }

        [TestMethod]
        public void DeveRetornarOSomatorioDeHorasPlanejadasDoCronogramaQuandoNaoHouverTarefasTest()
        {
            //TODO: MIGRAR TESTE PARA TarefaHistoricoEstimativaDao

            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao();

            double horasPlanejadas = TarefaHistoricoEstimativaDao.ConsultarTotalHorasPlanejadasCronograma( cronograma.Oid );

            Assert.AreEqual( 0, horasPlanejadas, "Deve ter 0 horas planejadas, pois não existem tarefas cadastradas." );
        }

        [TestMethod]
        public void DeveRetornarOSomatorioDeHorasPlanejadasDoCronogramaQuandoHouverTarefasTest()
        {
            //TODO: MIGRAR TESTE PARA TarefaHistoricoEstimativaDao

            Cronograma cronograma = CronogramaBo.CriarCronogramaPadrao();

            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid,"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out tarefasImpactadas, ref dataAcao,"Criar método", 3 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid,"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out tarefasImpactadas, ref dataAcao,"Criar método", 3 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid,"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out tarefasImpactadas, ref dataAcao,"Criar método", 3 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid,"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out tarefasImpactadas, ref dataAcao,"Criar método", 3 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid,"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out tarefasImpactadas, ref dataAcao,"Criar método", 3 );

            double horasPlanejadas = TarefaHistoricoEstimativaDao.ConsultarTotalHorasPlanejadasCronograma( cronograma.Oid );

            Assert.AreEqual( 15, horasPlanejadas.ToTimeSpan().Hours, "Deve ter 0 horas planejadas, pois não existem tarefas cadastradas." );
        }
    }
}
