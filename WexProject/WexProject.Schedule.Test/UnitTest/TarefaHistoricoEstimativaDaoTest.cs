using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Extensions.EnumExtension;
using WexProject.Schedule.Test.Builders;
using WexProject.Schedule.Test.Fixtures.Factory;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaHistoricoEstimativaDaoTest : BaseEntityFrameworkTest
    {
        #region Atributos

        /// <summary>
        /// lista de tarefas impactadas (Dependência do BO de criação das entidades CronogramaTarefa)
        /// </summary>
        private List<CronogramaTarefa> _tarefasImpactadas;

        /// <summary>
        /// data de realização de uma alteração (Dependência do BO de uma ação sobre as entidades de CronogramaTarefa)
        /// </summary>
        private DateTime _dataAcao;

        private List<CronogramaTarefa> _cronogramaTarefas;

        #endregion

        #region Propriedades

        public Cronograma Cronograma { get; set; }

        public List<SituacaoPlanejamento> SituacoesPlanejamento { get; set; }

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Método auxiliar para os testes para edição da data de um cronograma
        /// </summary>
        /// <param name="dataInicio">data de inicio do cronograma</param>
        /// <param name="dataFinal">data de término do cronograma</param>
        /// <param name="cronograma">cronograma a ser editado</param>
        private void EditarDataCronograma( DateTime dataInicio, DateTime dataFinal, Cronograma cronograma )
        {
            CronogramaBo.EditarCronograma( new CronogramaDto
            {
                DtInicio = dataInicio,
                DtFinal = dataFinal,
                Oid = cronograma.Oid,
                OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento,
                TxDescricao = cronograma.TxDescricao
            } );
        }

        /// <summary>
        /// Método para auxliar na edição das datas do cronograma
        /// </summary>
        /// <param name="dataInicio"></param>
        /// <param name="dataTermino"></param>
        private void AlterarDataInicioEDataFinalCronograma( DateTime dataInicio, DateTime dataTermino )
        {
            EditarDataCronograma( dataInicio, dataTermino, Cronograma );
            contexto.SaveChanges();
        }

        /// <summary>
        /// Método auxiliar para criar tarefas no cronograma
        /// </summary>
        private void CriarTarefasNoCronograma()
        {
            _cronogramaTarefas = new List<CronogramaTarefa>();

            for(var i = 0; i < 5; i++)
            {
                _cronogramaTarefas.Add( CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, "Tarefa 01",
                    SituacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ),
                    DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao,
                    "Criar método", 3 ) );
            }
        }

        #endregion

        [TestInitialize]
        public void Inicializar()
        {
            SituacoesPlanejamento = new List<SituacaoPlanejamento>
            {
                CronogramaFactoryEntity.CriarSituacaoPlanejamento(contexto, "Não Iniciado",
                    CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim),
                CronogramaFactoryEntity.CriarSituacaoPlanejamento(contexto, "Em Andamento"),
                CronogramaFactoryEntity.CriarSituacaoPlanejamento(contexto, "Cancelado",
                    CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento),
                CronogramaFactoryEntity.CriarSituacaoPlanejamento(contexto, "Impedimento",
                    CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Impedimento),
                CronogramaFactoryEntity.CriarSituacaoPlanejamento(contexto, "Pronto", CsTipoSituacaoPlanejamento.Ativo,
                    CsTipoPlanejamento.Encerramento)
            };
            Cronograma = CronogramaBo.CriarCronogramaPadrao();
            _tarefasImpactadas = new List<CronogramaTarefa>();
            _dataAcao = new DateTime();

            CriarTarefasNoCronograma();
        }

        [TestMethod]
        public void DeveRetornarNuloQuandoNaoHouverHistoricoCadastradoParaUmaTarefaEmDeterminadaData()
        {
            DateTime dataInicio = DateTime.Parse( "05/05/2014" );

            var oidTarefa = Guid.NewGuid();
            var historicoPesquisado = TarefaHistoricoEstimativaDao.ConsultarHistoricoEstimativaPorOidTarefaEData( contexto, oidTarefa, dataInicio );

            Assert.IsNull( historicoPesquisado, "Não deveria ter retornado o objeto do histórico criado" );
        }

        [TestMethod]
        public void DeveCriarUmHistoricoQuandoNaoPossuirHistoricoRestanteParaDataEspecificada()
        {
            DateTime dataInicio = DateTime.Parse("05/05/2014");
            AlterarDataInicioEDataFinalCronograma( dataInicio, DateTime.Parse( "09/05/2014" ) );

            CronogramaTarefa cronogramaTarefa = _cronogramaTarefas.FirstOrDefault();

            const int nbHorasRestantes = 0;

            var tarefaEstimativa = new TarefaHistoricoEstimativaBuilder().Data( dataInicio ).HorasRestantes( nbHorasRestantes ).OidTarefa( cronogramaTarefa.OidTarefa ).Criar();
            var novoHistorico = TarefaHistoricoEstimativaDao.SalvarHistorico( tarefaEstimativa );

            Assert.IsNotNull( novoHistorico, "Deveria ter retornado o objeto do histórico criado" );
            Assert.AreEqual( dataInicio.Date, novoHistorico.DtPlanejado.Date, string.Format( "Deveria ter criado um histórico para a data {0}", dataInicio.Date ) );
            Assert.AreEqual( cronogramaTarefa.OidTarefa, novoHistorico.OidTarefa, string.Format( "Deveria ter criado um histórico para a tarefa de oid {0}", cronogramaTarefa.OidTarefa ) );
            Assert.AreEqual( nbHorasRestantes, novoHistorico.NbHoraRestante, string.Format( "Deveria ter criado um histórico para a data {0}", dataInicio.Date ) );
        }

        [TestMethod]
        public void NaoDeveSalvarQuandoOHistoricoDeEstimativaForNulo()
        {
            var historicoEstimativa = TarefaHistoricoEstimativaDao.SalvarHistorico( null );
            Assert.IsNull( historicoEstimativa, "Não deveria possuir histórico de estimativa" );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void DeveDispararExceptionQuandoContextoNulo()
        {
            TarefaHistoricoEstimativaDao.ConsultarHistoricoEstimativaPorOidTarefaEData(null, Guid.NewGuid(), DateTime.MinValue);
        }

        [TestMethod]
        public void DeveDispararExceptionQuandoTentarSalvarHistoricoParaUmaTarefaQueNaoExiste()
        {
            var historicoEstimativa =
                new TarefaHistoricoEstimativaBuilder()
                .Data("05/05/2014")
                    .HorasRestantes(0)
                    .OidTarefa(Guid.NewGuid())
                    .Criar();
            historicoEstimativa = TarefaHistoricoEstimativaDao.SalvarHistorico( historicoEstimativa );
            Assert.IsNull(historicoEstimativa,"Não deveria  criar um histórico pois não há tarefa para ser referenciada com o Oid");
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void DeveDispararExceptionQuandoOidTipoDoOidNaoEstiverCorreto()
        {
            TarefaHistoricoEstimativaDao.ConsultarHistoricoEstimativaPorOidTarefaEData( contexto, new Guid(), DateTime.MinValue );
        }
    }
}
