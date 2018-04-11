using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Planejamento;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaHistoricoTrabalhoDaoTest : BaseEntityFrameworkTest
    {
        /// <summary>
        /// Cenário: Quando buscar a última hora trabalhada por um colaborador em um determinado dia e o colaborador ainda não tiver realizado nenhuma atividade no dia.
        /// Expectativa: Deve vir o horário padrão (08:00:00 horas)
        /// </summary>
        [TestMethod]
        public void ConsultarHorarioUltimaTarefaDiaColaboradorQuandoColaboradorNaoTiverRealizadoTarefaNoDia()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador1
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //colaborador2
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName,
                                         (string)"Criar método", responsaveis, 5 );

            //Salvando linha base tarefa
            tarefa.CsLinhaBaseSalva = true;

			contexto.Entry( tarefa ).State = System.Data.EntityState.Modified;
            contexto.SaveChanges();

            //Criar um histórico associando o colaborador1
            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TimeSpan nbHoraFinal = TarefaHistoricoTrabalhoDao.ConsultarHorarioUltimaTarefaDiaColaborador( contexto, colaborador2.Usuario.UserName, DateTime.Now );

            Assert.AreEqual( new TimeSpan( 8, 0, 0 ), nbHoraFinal, "Deveria ser o mesmo horário, pois o colaborador 2 não realizou nenhuma tarefa no dia." );
        }

        /// <summary>
        /// Cenário: Quando buscar a última hora trabalhada por um colaborador em um determinado dia e o colaborador ainda não tiver realizado nenhuma atividade no dia.
        /// Expectativa: Deve vir o horário padrão (08:00:00 horas)
        /// </summary>
        [TestMethod]
        public void ConsultarHorarioUltimaTarefaDiaColaboradorQuandoColaboradorTiverRealizadoTarefaNoDia()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            //colaborador1
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName,
                                         (string)"Criar método", responsaveis, 5 );

            //Salvando linha base tarefa
            tarefa.CsLinhaBaseSalva = true;
            contexto.Entry<Tarefa>( tarefa ).State = System.Data.EntityState.Modified;
            contexto.SaveChanges();

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador1.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            //colaborador2
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa.Oid, colaborador2.Usuario.UserName, new TimeSpan( 1, 0, 0 ), DateTime.Now, new TimeSpan( 13, 0, 0 ), new TimeSpan( 14, 0, 0 ), "comentário historico 2", new TimeSpan( 1, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TimeSpan nbHoraFinal = TarefaHistoricoTrabalhoDao.ConsultarHorarioUltimaTarefaDiaColaborador( contexto, colaborador1.Usuario.UserName, DateTime.Now );

            Assert.AreEqual( new TimeSpan( 11, 0, 0 ), nbHoraFinal, "Deveria ser o mesmo horário, pois o colaborador 1 realizou uma tarefa de 8 às 11 no dia." );
        }

        [TestMethod]
        public void ConsultarHorarioUltimaTarefaDiaColaboradorQuandoColaboradorTiverRealizadoTarefasEmDiferentesCronogramas()
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

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

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

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), DateTime.Now, new TimeSpan( 10, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TimeSpan nbHoraFinal = TarefaHistoricoTrabalhoDao.ConsultarHorarioUltimaTarefaDiaColaborador( contexto, colaborador1.Usuario.UserName, DateTime.Now );

            Assert.AreEqual( new TimeSpan( 11, 0, 0 ), nbHoraFinal, "A hora final deveria ser igual a 11:00 pois o colaborador investiu esforço realizado " +
                " em outra tarefa independente do cronograma em que está lançando a hora realizada" );
        }

        [TestMethod]
        public void DevePesquisarADataEHoraDaUltimaHoraTrabalhadaPeloColaborador()
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

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 2, 0, 0 ), dataRealizado, new TimeSpan( 8, 0, 0 ), new TimeSpan( 10, 0, 0 ), "comentário", new TimeSpan( 6, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa1.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 11, 0, 0 ), new TimeSpan( 12, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( tarefa2.Oid, colaborador1.Usuario.UserName, new TimeSpan( 1, 0, 0 ), dataRealizado, new TimeSpan( 10, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 3, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalhoDto historicoTrabalhoMaisRecente = TarefaHistoricoTrabalhoBo.ConsultarUltimoEsforcoRealizadoColaboradorDto( contexto, colaborador1.Usuario.UserName );
            Assert.IsNotNull( historicoTrabalhoMaisRecente, "Deveria ter encontrado pois foi criado histórico para tarefa" );
            Assert.AreEqual( dataRealizado, historicoTrabalhoMaisRecente.DtRealizado, "Deveria ser a mesma data realizada" );
            Assert.AreEqual( new TimeSpan( 12, 0, 0 ), historicoTrabalhoMaisRecente.NbHoraFinal, "O horário final deveria ser igual ao horário final do ultimo esforço do colaborador" );
        }

        [TestMethod]
        public void DeveRetornarUmaDiariaDeOitoAsDezoitoQuandoODiaAtualNaoPossuirPeriodosDeTrabalhoCadastrados()
        {
            SemanaTrabalho semana = SemanaTrabalhoBo.SemanaTrabalhoPadraoFactory();
            DiaTrabalhoDto diaDto = TarefaHistoricoTrabalhoDao.ConsultarDiaAtualDeTrabalhoDto( DayOfWeek.Saturday, semana );
            Assert.AreEqual( 1, diaDto.PeriodosTrabalho.Count, "So deveria conter 1 periodo de 8:00 as 18:00" );
            Assert.AreEqual( "8:00", diaDto.PeriodosTrabalho[0].HoraInicial, "O horario inicial deveria ser as 8:00" );
            Assert.AreEqual( "18:00", diaDto.PeriodosTrabalho[0].HoraFinal, "O horario inicial deveria ser as 8:00" );
        }
    }
}
