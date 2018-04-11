using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Test.Fixtures.Factory;
using System.Linq;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Library.Libs.Extensions.EnumExtension;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class GraficoBurndownBOTest : BaseEntityFrameworkTest
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

        #endregion

        #region Propriedades

        public Cronograma Cronograma { get; set; }

        public List<SituacaoPlanejamento> situacoesPlanejamento { get; set; }

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
        /// Método para auxiliar no assert de comparação dos dados de um CronogramaSerieBurndownDto
        /// </summary>
        /// <param name="esperado">dto com os dados esperados</param>
        /// <param name="dadosAtuais">dto com os dados atuais</param>
        /// <returns></returns>
        public bool CompararDadosGrafico( BurndownDadosDto esperado, BurndownDadosDto dadosAtuais )
        {
            return dadosAtuais != null && esperado.Dia.Date.Equals( dadosAtuais.Dia.Date ) && esperado.QtdeHoras.GetValueOrDefault().Equals( dadosAtuais.QtdeHoras.GetValueOrDefault() ) && esperado.CsTipo.Equals( dadosAtuais.CsTipo );
        }

        private void AdicionarCalendario( string descricao, CsCalendarioDomain tipoCalendario, CsVigenciaDomain tipoVigencia, DateTime dataInicio, DateTime? dataTermino = null )
        {
            //TODO: REVER SE DEVERÁ SER PASSADO PARA O CalendarioBo
            Calendario calendario = new Calendario()
            {
                CsCalendario = tipoCalendario.ToInt(),
                CsVigencia = tipoVigencia.ToInt(),
                DtInicio = dataInicio.Date,
                Periodo = dataInicio.Date,
                Oid = Guid.NewGuid(),
                TxDescricao = descricao,
                CsSituacao = CsSituacaoDomain.Ativo.ToInt()
            };

            switch(tipoVigencia)
            {
                case CsVigenciaDomain.PorDiaMes:
                    calendario.CsMes = dataInicio.Month;
                    calendario.NbDia = dataInicio.Day;
                    break;
                case CsVigenciaDomain.PorDiaMesAno:
                    break;
                case CsVigenciaDomain.PorPeriodo:
                    if(!dataTermino.HasValue)
                        throw new ArgumentException( "Deveria ter preenchido a data de término do período." );
                    calendario.DtTermino = dataTermino.Value.Date;
                    break;
                default:
                    break;
            }
            contexto.Calendarios.Add( calendario );
            contexto.SaveChanges();
        }

        #endregion

        [TestInitialize]
        public void Inicializar()
        {
            situacoesPlanejamento = new List<SituacaoPlanejamento>();
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Cancelado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Impedimento", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Impedimento, CsPadraoSistema.Não, true ) );
            situacoesPlanejamento.Add( CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Pronto", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Encerramento, CsPadraoSistema.Não, true ) );
            Cronograma = CronogramaBo.CriarCronogramaPadrao();
            _tarefasImpactadas = new List<CronogramaTarefa>();
            _dataAcao = new DateTime();
        }

        [TestMethod]
        public void DeveCalcularDadosDoGraficoQuandoNaoExistirTarefasNemFeriadosESemFinaisDeSemanaNoCronogramaTest()
        {
            EditarDataCronograma( new DateTime( 2014, 05, 05 ), new DateTime( 2014, 05, 09 ), Cronograma );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), CsTipo = CsTipoBurndown.Planejado, } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), CsTipo = CsTipoBurndown.Planejado, } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), CsTipo = CsTipoBurndown.Planejado, } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), CsTipo = CsTipoBurndown.Planejado, } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), CsTipo = CsTipoBurndown.Planejado, } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );

            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoGraficoQuandoOCronogramaPossuirApenasUmDiaDeDuracao()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/05" );
            EditarDataCronograma( new DateTime( 2014, 05, 05 ), new DateTime( 2014, 05, 06 ), Cronograma );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), CsTipo = CsTipoBurndown.Realizado } );
			dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014 , 05 , 06 ) , CsTipo = CsTipoBurndown.Planejado } );
			dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014 , 05 , 06 ) , CsTipo = CsTipoBurndown.Realizado } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );

            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoGraficoQuandoOCronogramaPossuirApenasUmDiaDeDuracaoEForFimDeSemana()
        {
            EditarDataCronograma( new DateTime( 2014, 05, 03 ), new DateTime( 2014, 05, 04 ), Cronograma );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );

            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.AreEqual( 0, dadosGrafico.DadosBurndown.Count, "Não deveria conter" );
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaConsiderandoFolgaPorPeriodoNoCalendario()
        {
            DateTime dataInicio = DateTime.Parse( "01/05/2014" ), dataTermino = DateTime.Parse( "07/05/2014" );
            DateUtil.CurrentDateTime = dataInicio.AddDays( -1 );

            AdicionarCalendario( "Dia do trabalhador e ponto facultativo", CsCalendarioDomain.Folga, CsVigenciaDomain.PorPeriodo, DateTime.Parse( "01/05/2014" ), DateTime.Parse( "02/05/2014" ) );

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 7.5, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaConsiderandoFolgaPorDiaEspecificoNoCalendario()
        {
            DateTime dataInicio = DateTime.Parse( "01/05/2014" ), dataTermino = DateTime.Parse( "06/05/2014" );

            DateUtil.CurrentDateTime = dataInicio.AddDays( -1 );

            AdicionarCalendario( "Dia do trabalhador", CsCalendarioDomain.Folga, CsVigenciaDomain.PorDiaMesAno, DateTime.Parse( "01/05/2014" ) );

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 02 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 7.5, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );


            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaConsiderandoDiaTrabalhoPorPeriodoNoCalendario()
        {
            DateTime dataInicio = DateTime.Parse( "01/05/2014" ), dataTermino = DateTime.Parse( "05/05/2014" );

            DateUtil.CurrentDateTime = dataInicio.AddDays( -1 );

            AdicionarCalendario( "Trabalho fim de semana compensar banco de horas", CsCalendarioDomain.Trabalho, CsVigenciaDomain.PorPeriodo, DateTime.Parse( "03/05/2014" ), DateTime.Parse( "04/05/2014" ) );

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 01 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 02 ), QtdeHoras = 11.25, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 03 ), QtdeHoras = 7.5, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 04 ), QtdeHoras = 3.75, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaConsiderandoDiaDeTrabalhoPorDiaEspecificoNoCalendario()
        {
            DateTime dataInicio = DateTime.Parse( "02/05/2014" ), dataTermino = DateTime.Parse( "05/05/2014" );

            DateUtil.CurrentDateTime = dataInicio.AddDays( -1 );

            AdicionarCalendario( "Trabalho no domingo", CsCalendarioDomain.Trabalho, CsVigenciaDomain.PorDiaMesAno, DateTime.Parse( "04/05/2014" ) );

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 3, 0 );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 02 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 04 ), QtdeHoras = 7.5, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaConsiderandoFolgaPorDiaMesNoCalendario()
        {
            DateTime dataInicio = DateTime.Parse( "01/05/2014" ), dataTermino = DateTime.Parse( "05/05/2014" );
            DateUtil.CurrentDateTime = dataInicio;
            AdicionarCalendario( "Folga", CsCalendarioDomain.Folga, CsVigenciaDomain.PorDiaMes, DateTime.Parse( "02/05/2014" ) );

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 5, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 5, 0 );

            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 01 ), QtdeHoras = 10, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 01 ), QtdeHoras = 10, CsTipo = CsTipoBurndown.Realizado } );


            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }

        [TestMethod]
        public void DeveCalcularDadosDoPlanejadoQuandoExistirTarefasNoCronogramaTest()
        {
            DateTime dataInicio = DateTime.Parse( "05/05/2014" ), dataTermino = DateTime.Parse( "09/05/2014" );
            DateUtil.CurrentDateTime = dataInicio;

            EditarDataCronograma( dataInicio, dataTermino, Cronograma );

            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 01", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 4, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 02", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 4, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 03", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 4, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 04", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 4, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( Cronograma.Oid, (string)"Tarefa 05", situacoesPlanejamento.FirstOrDefault( o => o.CsTipo == CsTipoPlanejamento.Planejamento ), DateTime.MinValue, "", colaboradorPadrao.Usuario.UserName, out _tarefasImpactadas, ref _dataAcao, (string)"Criar método", 4, 0 );


            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 20, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 10, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 5, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 0, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 20, CsTipo = CsTipoBurndown.Realizado } );

            var dadosGrafico = GraficoBurndownBO.Instancia.CalcularDadosGraficoBurndown( Cronograma.Oid );
            Assert.IsNotNull( dadosGrafico, "Deveria ter retornado o resultado com os dias planejados do cronograma" );
            Assert.IsTrue( dadosGrafico.DadosBurndown.Count > 0, "Deveria conter os dias planejados da sprint" );
            foreach(var dadosAtuais in dadosGrafico.DadosBurndown)
            {
                Assert.IsTrue( dadosEsperadosGrafico.Any( o => CompararDadosGrafico( o, dadosAtuais ) ), "O resultado deveria conter os resultados para o dia esperado." );
            }
        }


        //[TestMethod]
        //public void DeveCalcularODesvioQuandoNaoExistirDadosPlanejadosParaOBurndown()
        //{
        //    Assert.AreEqual( 0, GraficoBurndownBO.CalcularDesvio( new List<BurndownDadosDto>() ) );
        //}

        [TestMethod]
        public void DeveCalcularODesvioQuandoExistiremEstimativasPlanejadasENaoPossuirEstimativasRealizadas()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/05" );
            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 12, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 6, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Realizado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 3, CsTipo = CsTipoBurndown.Planejado } );

            Assert.AreEqual( 0, GraficoBurndownBO.CalcularDesvio( dadosEsperadosGrafico ),"Deveria estar em dia." );
        }

        [TestMethod]
        public void DeveCalcularODesvioQuandoExistiremEstimativasPlanejadasENaoPossuirEstimativasRealizadasParaOSegundoDia()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/06" );
            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 12, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 6, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 3, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Realizado } );

            Assert.AreEqual( -3, GraficoBurndownBO.CalcularDesvio( dadosEsperadosGrafico ) ,"Deveria estar 3 horas atrasado");
        }

        [TestMethod]
        public void DeveCalcularODesvioQuandoExistiremEstimativasPlanejadasENaoPossuirEstimativasRealizadasParaOTerceiroDia()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/07" );
            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 12, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 6, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 3, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Realizado } );

            Assert.AreEqual( -6, GraficoBurndownBO.CalcularDesvio( dadosEsperadosGrafico ) , "Deveria estar 6 horas atrasado" );
        }

        [TestMethod]
        public void DeveCalcularODesvioQuandoExistiremEstimativasPlanejadasPossuirEstimativasRealizadasAdiantado()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/06" );
            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 12, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 6, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 3, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Realizado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Realizado } );

            Assert.AreEqual( 3, GraficoBurndownBO.CalcularDesvio( dadosEsperadosGrafico ),"Deveria estar 3 horas adiantado" );
        }

        [TestMethod]
        public void DeveCalcularODesvioQuandoExistiremEstimativasPlanejadasPossuirEstimativasRealizadasAtrasado()
        {
            DateUtil.CurrentDateTime = DateTime.Parse( "2014/05/07" );
            var dadosEsperadosGrafico = new List<BurndownDadosDto>();
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 12, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 07 ), QtdeHoras = 9, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 08 ), QtdeHoras = 6, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 09 ), QtdeHoras = 3, CsTipo = CsTipoBurndown.Planejado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 05 ), QtdeHoras = 15, CsTipo = CsTipoBurndown.Realizado } );
            dadosEsperadosGrafico.Add( new BurndownDadosDto { Dia = new DateTime( 2014, 05, 06 ), QtdeHoras = 10, CsTipo = CsTipoBurndown.Realizado } );

            Assert.AreEqual( -1, GraficoBurndownBO.CalcularDesvio( dadosEsperadosGrafico ), "Deveria estar 1 hora atrasado" );
        }
    }
}
