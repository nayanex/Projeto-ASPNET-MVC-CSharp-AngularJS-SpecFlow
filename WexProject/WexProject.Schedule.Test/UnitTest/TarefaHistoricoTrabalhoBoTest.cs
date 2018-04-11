using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Exceptions.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Extensions.Entities;
using WexProject.BLL;
using System.Data.Entity;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaHistoricoTrabalhoBoTest : BaseEntityFrameworkTest
    {
        /// <summary>
        /// Cenário: Quando um colaborador gerar um histórico e linha de base da tarefa não estiver salva.
        /// Expectativa: Deverá salvar a linha de base da tarefa e o histórico para aquela tarefa.
        /// </summary>
        [TestMethod]
        public void CriarHistoricoTarefaQuandoTarefaNaoPossuirHistoricoELinhaDeBaseTarefaEstiverFalsaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins" );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 5 );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalho historicoCriado = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( tarefa.Oid );

            Tarefa tarefaResultado = TarefaDao.ConsultarTarefaPorOid( tarefa.Oid );

            Assert.IsNotNull( historicoCriado, "Deveria ter criado um histórico pra tarefa" );
            Assert.AreEqual( tarefaResultado.Oid, historicoCriado.OidTarefa, "Deveria ser o mesmo Oid pois foi criado um histórico pra determinada tarefa" );
            Assert.AreEqual( true, tarefaResultado.CsLinhaBaseSalva, "A linha de base da tarefa deveria estar true, pois quando salvar o primeiro histórico deve salvar a linha de base." );
        }

        /// <summary>
        /// Cenário: Quando uma tarefa já possuir um histórico e outro colaborador gerar um histórico para a mesma tarefa
        /// Expectativa: A tarefa deverá possuir 2 históricos salvos e 2 colaboradores responsáveis e o histórico mais atual deve ser do colaborador2
        /// </summary>
        [TestMethod]
        public void CriarHistoricoTarefaQuandoTarefaJaPossuirHistoricoEOutroColaboradorGerarHistoricoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName,
                                         (string)"Criar método", responsaveis, 5 );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            //colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador2.Usuario.UserName, new TimeSpan( 1, 0, 0 ), DateTime.Now, new TimeSpan( 13, 0, 0 ), new TimeSpan( 14, 0, 0 ), "comentário historico 2", new TimeSpan( 1, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalho historicoCriado = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( tarefa.Oid );

            const int NUM_DE_HISTORICOS = 2;
            const int NUM_DE_RESPONSAVEIS = 2;

            List<string> responsaveisTarefa = TarefaDao.ConsultarTarefaPorOid( tarefa.Oid ).TxResponsaveis.Split( ',' ).ToList();

            Assert.AreEqual( NUM_DE_HISTORICOS, TarefaDao.ConsultarTarefaPorOid( tarefa.Oid, o => o.TarefaHistoricoTrabalhos ).TarefaHistoricoTrabalhos.Count, "Deveria possuir 2 históricos, pois foram criados 2 históricos." );
            Assert.AreEqual( NUM_DE_RESPONSAVEIS, responsaveisTarefa.Count, "Deveria possuir 2 responsáveis pela tarefa, pois 2 colaboradores criaram históricos diferentes para a tarefa." );
            Assert.AreEqual( new TimeSpan( 1, 0, 0 ), historicoCriado.HoraRestante, "Deveria resta apenas 1 hora, pois o histórico atual foi salvo com esses dados." );
            Assert.AreEqual( colaborador2.Oid, historicoCriado.OidColaborador, "Deveria ser o mesmo colaborador, pois foi ele o último a criar um histórico para a tarefa." );
            Assert.AreEqual( true, TarefaDao.ConsultarTarefaPorOid( tarefa.Oid ).CsLinhaBaseSalva, "A linha de base da tarefa deveria estar true, pois quando salvar o primeiro histórico deve salvar a linha de base." );
        }

        /// <summary>
        /// Cenário: Quando uma tarefa já possuir um histórico e outro colaborador gerar um histórico para a mesma tarefa, mas não realizar nenhuma hora para aquela tarefa (apenas mudar as horas restantes).
        /// Expectativa: A tarefa deverá possuir 2 históricos salvos e 1 colaborador responsável.
        /// </summary>
        [TestMethod]
        public void CriarHistoricoTarefaQuandoTarefaJaPossuirHistoricoEOutroColaboradorGerarHistoricoMasNaoAlterarHorasRealizadoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName,
                                         (string)"Criar método", responsaveis, 5 );

            tarefa = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa.Oid );
            //Salvando linha base tarefa
            tarefa.CsLinhaBaseSalva = true;

            contexto.Entry<Tarefa>( tarefa ).State = System.Data.EntityState.Modified;
            contexto.SaveChanges();

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            //colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            //Aumentando 1 hora na estimativa restante
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador2.Usuario.UserName, new TimeSpan( 0, 0, 0 ), DateTime.Now, new TimeSpan( 15, 0, 0 ), new TimeSpan( 15, 0, 0 ), "comentário historico 2", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalho historicoCriado = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( tarefa.Oid );

            const int NUM_DE_HISTORICOS = 2;
            const int NUM_DE_RESPONSAVEIS = 1;

            List<string> responsaveisTarefa = tarefa.TxResponsaveis.Split( ',' ).ToList();

            Assert.AreEqual( NUM_DE_HISTORICOS, TarefaDao.ConsultarTarefaPorOid( tarefa.Oid, o => o.TarefaHistoricoTrabalhos ).TarefaHistoricoTrabalhos.Count, "Deveria possuir 2 históricos, pois foram criados 2 históricos." );
            Assert.AreEqual( NUM_DE_RESPONSAVEIS, responsaveisTarefa.Count, "Deveria possuir 1 responsáveis pela tarefa, pois 2 colaboradores criaram históricos diferentes para a tarefa," +
            " mas apenas 1 realizou horas na tarefa." );
            Assert.AreEqual( new TimeSpan( 3, 0, 0 ), historicoCriado.HoraRestante, "Deveria restar apenas 3 horas, pois o histórico atual foi salvo com esses dados." );
            Assert.AreEqual( colaborador2.Oid, historicoCriado.OidColaborador, "Deveria ser o mesmo colaborador, pois foi ele o último a criar um histórico para a tarefa." );
            Assert.AreEqual( true, tarefa.CsLinhaBaseSalva, "A linha de base da tarefa deveria estar true, pois quando salvar o primeiro histórico deve salvar a linha de base." );
        }

        [TestMethod]
        public void DeveRetornarOHorarioInicialDoPrimeiroDiaUtilDoColaboradorQuandoForCadastradaAPrimeiraEstimativaDeEsforcoRealizadoDoColaboradorSemanaTrabalhoPadrao()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            DateTime dataSolicitacao = new DateTime( 2013, 08, 14 );
            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( new DateTime( 2013, 08, 14 ), inicializadorEstimativa.DataEstimativa, "Considerando a semana de trabalho padrão deveria indicar o dia atual como inicio" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando a semana de trabalho padrão deveria indicar 8:00 como hora inicial" );
        }

        [TestMethod]
        public void DeveRetornarOHorarioInicialDoPrimeiroDiaUtilDoColaboradorQuandoForCadastradaAPrimeiraEstimativaDeEsforcoRealizadoDoColaboradorSemanaTrabalhoCostumizada()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );

            contexto.SaveChanges();
            #endregion

            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            SemanaTrabalhoBo.AdicionarDiaDeTrabalho( semanaTrabalho, DayOfWeek.Monday );
            SemanaTrabalhoBo.AdicionarDiaDeTrabalho( semanaTrabalho, DayOfWeek.Friday );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Friday, "10:15", "11:45" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Friday, "13:15", "14:45" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "10:30", "13:00" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "15:00", "19:00" );
            TarefaHistoricoTrabalhoDao.SemanaTrabalho = semanaTrabalho;
            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 14 );
            DateTime dataEsperada = DateUtil.ConsultarDataDoProximoDiaDaSemana( DayOfWeek.Friday, dataSolicitacao );
            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao, semanaTrabalho );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataEsperada, inicializadorEstimativa.DataEstimativa, "Considerando a data atual como 14/08/2013 (Quarta-Feira) e os periodos de trabalho do colaborador como Segunda e Sexta-Feira," +
                " a data de estimativa deveria refletir a data calculada da próxima sexta-feira" );
            Assert.AreEqual( new TimeSpan( 10, 15, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando a semana de trabalho padrão deveria indicar 8:00 como hora inicial" );
        }

        [TestMethod]
        public void DeveRetornarAHoraDeEstimativaInicialNoMesmoDiaQuandoEstiverDentroDaCargaHoraria()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 14 );
            DateTime dataRealizado = dataSolicitacao.Date;
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 10, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataSolicitacao.Date, inicializadorEstimativa.DataEstimativa, "Considerando a dataRealizado como mais recente, deve retorna-la como mais atual" );
            Assert.AreEqual( new TimeSpan( 12, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando os historicos cadastradas o horario final mais recente é de 12:00 da tarefa1" );
        }

        [TestMethod]
        public void DeveRetornarAHoraDeEstimativaInicialNoMesmoDiaQuandoEstiverDentroDaCargaHorariaComDiasDiferentes()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 15 );
            DateTime dataRealizado = dataSolicitacao.Date;
            DateTime dataRealizadoMaisAtual = new DateTime( 2013, 08, 15 );
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizadoMaisAtual, new TimeSpan( 10, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            List<TarefaHistoricoTrabalho> historicos = contexto.TarefaHistoricoTrabalho.ToList();

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataRealizadoMaisAtual, inicializadorEstimativa.DataEstimativa, "Considerando a dataRealizadoMaisAtual como mais recente, deve retorna-la como mais atual" );
            Assert.AreEqual( new TimeSpan( 11, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando os historicos cadastradas o horario final mais recente é de 11:00 da tarefa2" );
        }

        [TestMethod]
        public void DeveRetornarAHoraInicialEDataInicioNoMesmoDiaQuandoODiaEmQueForEstimadoForIgualAoDiaAtual()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion


            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 14 );
            DateTime dataRealizado = dataSolicitacao.Date;
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 15, 0, 0 ), new TimeSpan( 18, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            List<TarefaHistoricoTrabalho> historicos = contexto.TarefaHistoricoTrabalho.ToList();

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataSolicitacao.Date, inicializadorEstimativa.DataEstimativa, "Considerando a dataRealizadoMaisAtual como mais recente, deve retorna-la como mais atual" );
            Assert.AreEqual( new TimeSpan( 18, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando os historicos cadastradas o horario final mais recente é de 11:00 da tarefa2" );
        }

        [TestMethod]
        public void DeveRetornarAHoraInicialEDataInicioNoProximoDiaQuandoODiaEmQueForEstimadoForIgualAoDiaAtualIniciaraNoLimiteDoDia()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion


            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 14 );
            DateTime dataRealizado = dataSolicitacao.Date;
            TimeSpan meiaNoite = new TimeSpan( 24, 0, 0 );
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 19, 0, 0 ), meiaNoite, "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 15, 0, 0 ), new TimeSpan( 18, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( new DateTime( 2013, 08, 15 ), inicializadorEstimativa.DataEstimativa, "Considerando a dataRealizadoMaisAtual como mais recente, deve retorna-la como mais atual" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando os historicos cadastradas o horario final mais recente é de 11:00 da tarefa2" );
        }

        [TestMethod]
        public void DeveRetornarAHoraInicialEDataInicioNoProximoDiaQuandoForaDaCargaHorariaEODiaEmQueForEstimadoForDiferenteDoDiaAtual()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 15 );
            DateTime dataRealizado = new DateTime( 2013, 08, 14 );
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 15, 0, 0 ), new TimeSpan( 18, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            List<TarefaHistoricoTrabalho> historicos = contexto.TarefaHistoricoTrabalho.ToList();

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataSolicitacao.Date, inicializadorEstimativa.DataEstimativa, "Considerando a dataRealizadoMaisAtual como mais recente, deve retorna-la como mais atual" );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando os historicos cadastradas o horario final mais recente é de 11:00 da tarefa2" );
        }

        [TestMethod]
        public void DeveRetornarAHoraInicialEDataInicioNoProximoDiaQuandoForaDaCargaHorariaEODiaEmQueForEstimadoForDiferenteDoDiaAtualComSemanaCostumizada()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            SemanaTrabalhoBo.AdicionarDiaDeTrabalho( semanaTrabalho, DayOfWeek.Monday );
            SemanaTrabalhoBo.AdicionarDiaDeTrabalho( semanaTrabalho, DayOfWeek.Friday );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Friday, "10:15", "11:45" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Friday, "13:15", "19:45" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "10:30", "13:00" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "15:00", "19:00" );
            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 2, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataSolicitacao = new DateTime( 2013, 08, 15 );
            DateTime dataEsperada = DateUtil.ConsultarDataDoProximoDiaDaSemana( DayOfWeek.Friday, dataSolicitacao );
            DateTime dataRealizado = new DateTime( 2013, 08, 14 );
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 15, 0, 0 ), new TimeSpan( 18, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            InicializadorEstimativaDto inicializadorEstimativa = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaDto( colaborador1.Usuario.UserName, dataSolicitacao, semanaTrabalho );
            Assert.IsNotNull( inicializadorEstimativa, "Deveria ter calculado Inicializador de estimativa" );
            Assert.AreEqual( dataEsperada, inicializadorEstimativa.DataEstimativa, "Considerando o cenário a e a semana de trabalho costumizada a data selecionada deve ser a primeira sexta após a data realizado" );
            Assert.AreEqual( new TimeSpan( 10, 15, 0 ), inicializadorEstimativa.HoraInicialEstimativa, "Considerando o cenário deve ser estimada como hora inicial o horario inicial do periodo de trabalho da primeira sexta após a data realizado" );
        }

        [TestMethod]
        public void DeveSelecionarOProximoDiaUtilDaSemanaDeTrabalhoPadrao()
        {
            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            semanaTrabalho = SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory();
            //mock da data atual
            DateTime dataSolicitacao = new DateTime( 2013, 08, 15 );
            DateTime dataSelecionada = dataSolicitacao;
            DateTime dataEsperada = new DateTime( 2013, 08, 16 );
            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "Deveria ter calculado o próximo dia util como {0}({1})", dataEsperada, dataEsperada.DayOfWeek ) );
        }

        [TestMethod]
        public void DeveRetornarOMesmoDiaQuandoASemanaDeTrabalhoForNulaNaoPossuirDiasDeTrabalhoOuNaoPossuirDiasComPeriodosDeTrabalhoCadastrados()
        {
            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            //mock da data atual
            DateTime dataSolicitacao = new DateTime( 2013, 08, 15 );
            DateTime dataSelecionada = dataSolicitacao;
            DateTime dataEsperada = new DateTime( 2013, 08, 15 );
            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "Não deveria ter alterado o valor pois a semana de trabalho não contem dias de trabalho", dataEsperada, dataEsperada.DayOfWeek ) );

            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( null, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "Não deveria ter alterado o valor pois a semana de trabalho não contem dias de trabalho", dataEsperada, dataEsperada.DayOfWeek ) );

            SemanaTrabalhoBo.AdicionarDiaDeTrabalho( semanaTrabalho, DayOfWeek.Monday );
            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "Não deveria ter alterado o valor pois a semana de trabalho não contem dias de trabalho", dataEsperada, dataEsperada.DayOfWeek ) );
        }

        [TestMethod]
        public void DeveRetornarOProximoDiaUtilDaSemanaDeTrabalhoCostumizada()
        {
            SemanaTrabalho semanaTrabalho = new SemanaTrabalho();
            //mock da data atual
            DateTime dataSolicitacao = new DateTime( 2013, 08, 12 );
            DateTime dataSelecionada = dataSolicitacao;
            DateTime dataEsperadaSextaFeira = new DateTime( 2013, 08, 16 );
            DateTime dataEsperadaProximaSegunda = new DateTime( 2013, 08, 19 );
            DateTime dataEsperadaProximaSexta = new DateTime( 2013, 08, 23 );
            //Cenário cadastrado apenas Segunda e Sexta
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "8:45", "12:00" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Friday, "13:00", "17:00" );

            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperadaSextaFeira, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperadaSextaFeira, dataEsperadaSextaFeira.DayOfWeek ) );

            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperadaProximaSegunda, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperadaProximaSegunda, dataEsperadaProximaSegunda.DayOfWeek ) );


            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperadaProximaSexta, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperadaProximaSexta, dataEsperadaProximaSexta.DayOfWeek ) );

            semanaTrabalho = new SemanaTrabalho();
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Monday, "8:00", "12:00" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Tuesday, "8:00", "12:00" );
            SemanaTrabalhoBo.AdicionarPeriodoDeTrabalho( semanaTrabalho, DayOfWeek.Saturday, "8:00", "12:00" );

            dataSolicitacao = new DateTime( 2013, 08, 12 );
            dataSelecionada = dataSolicitacao;
            DateTime dataEsperada = new DateTime( 2013, 08, 13 );

            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperada, dataEsperada.DayOfWeek ) );

            dataEsperada = new DateTime( 2013, 08, 17 );
            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperada, dataEsperada.DayOfWeek ) );

            dataEsperada = new DateTime( 2013, 08, 19 );
            dataSelecionada = TarefaHistoricoTrabalhoBo.SelecionarDataProximoDiaUtil( semanaTrabalho, dataSelecionada );
            Assert.AreEqual( dataEsperada, dataSelecionada, string.Format( "O dia selecionado deveria ser {0} - {1}", dataEsperada, dataEsperada.DayOfWeek ) );

        }

        [TestMethod]
        public void DeveRetornarOHorarioDeInicioParaADataSelecionadaQuandoNaoHouver()
        {
            #region Construção Cenário
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                            CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                            CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 15 ),
                TxDescricao = "WexCronograma1",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma1 );
            contexto.SaveChanges();

            Cronograma cronograma2 = new Cronograma()
            {
                DtInicio = new DateTime( 2013, 02, 1 ),
                DtFinal = new DateTime( 2013, 02, 16 ),
                TxDescricao = "WexCronograma2",
                SituacaoPlanejamento = situacaoPlanejamentoEmAndamento
            };
            contexto.Cronograma.Add( cronograma2 );
            contexto.SaveChanges();

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            Tarefa tarefa1 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Cria método Cadastro",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 8,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa1 );
            contexto.SaveChanges();

            Tarefa tarefa2 = new Tarefa()
            {
                DtInicio = new DateTime( 2013, 02, 25 ),
                CsLinhaBaseSalva = true,
                TxDescricao = "Criar método Exclusão",
                TxResponsaveis = colaborador1.NomeCompleto,
                NbEstimativaInicial = 4,
                SituacaoPlanejamento = situacaoPlanejamentoNaoIniciado,
                AtualizadoPor = colaborador1
            };
            contexto.Tarefa.Add( tarefa2 );
            contexto.SaveChanges();
            #endregion

            CronogramaTarefa cronogramaTarefa = new CronogramaTarefa() { Cronograma = cronograma1, NbID = 1, Tarefa = tarefa1 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa );
            contexto.SaveChanges();
            CronogramaTarefa cronogramaTarefa2 = new CronogramaTarefa() { Cronograma = cronograma2, NbID = 1, Tarefa = tarefa2 };
            contexto.CronogramaTarefa.Add( cronogramaTarefa2 );
            contexto.SaveChanges();
            DateTime dataRealizado = DateTime.Now;
            InicializadorEstimativaDto inicializador = TarefaHistoricoTrabalhoBo.SelecionarInicializadorEstimativaColaboradorParaDataEspecifica( colaborador1.Usuario.UserName, dataRealizado.Date );
            Assert.AreEqual( DateTime.Now.Date, inicializador.DataEstimativa );
            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), inicializador.HoraInicialEstimativa );
        }

        /// <summary>
        /// Cenário: Quando uma tarefa já possuir um histórico e outro colaborador gerar um histórico para a mesma tarefa
        /// Expectativa: A tarefa deverá possuir 2 históricos salvos e 2 colaboradores responsáveis e o histórico mais atual deve ser do colaborador2
        /// </summary>
        [TestMethod]
        public void DeveCriarHistoricoDeEstimativaQuandoAEstimativaRestanteForAtualizadaAtravesDoHistoricoDeTrabalho()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName,
                                         (string)"Criar método", responsaveis, 5 );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            //colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador2.Usuario.UserName, new TimeSpan( 1, 0, 0 ), DateTime.Now, new TimeSpan( 13, 0, 0 ), new TimeSpan( 14, 0, 0 ), "comentário historico 2", new TimeSpan( 1, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalho historicoCriado = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( tarefa.Oid );

            var horas = contexto.TarefaHistoricoEstimativa.Where( o => o.OidTarefa == tarefa.Oid ).OrderByDescending( o => o.DtPlanejado ).ToList();
            var historicoEstimativa = contexto.TarefaHistoricoEstimativa.OrderByDescending( o => o.DtPlanejado ).FirstOrDefault( o => o.Tarefa.Oid == tarefa.Oid );
            Assert.IsNotNull( historicoEstimativa, "Deveria ter criado o histórico de estimativa ao atualizar a tarefa" );

            tarefa = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa.Oid );
            Assert.AreEqual( tarefa.NbEstimativaRestante, historicoEstimativa.NbHoraRestante );
        }

    }
}
