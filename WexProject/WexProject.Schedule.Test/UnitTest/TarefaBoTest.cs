using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Library.Libs.DataHora;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Planejamento;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaBoTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void ExcluirTarefaQuandoExistirTarefaParaExcluir()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( tarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();
            oidTarefasExcluidas = TarefaBo.ExcluirTarefa( contexto, oidTarefas );

            Tarefa tarefaExcluida = TarefaDao.ConsultarTarefaPorOid( tarefa.Oid );

            Assert.AreEqual( 1, oidTarefasExcluidas.Count, "Deve ter excluido uma tarefa" );
            Assert.AreEqual( tarefa.Oid, oidTarefasExcluidas[0], "Deve ter excluido a tarefa com este oid" );
            Assert.IsNull( tarefaExcluida, "Deveria ser nulo, pois a tarefa deveria ter sido excluida." );
        }

        [TestMethod]
        public void ExcluirTarefaQuandoNaoExistirTarefaParaExcluir()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa( (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( tarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();
            TarefaBo.ExcluirTarefa( contexto, oidTarefas );

            oidTarefasExcluidas = TarefaBo.ExcluirTarefa( contexto, oidTarefas );

            Tarefa tarefaExcluida = TarefaDao.ConsultarTarefaPorOid( tarefa.Oid );

            Assert.AreEqual( 0, oidTarefasExcluidas.Count, "Não deve existir nenhuma tarefa na lista, pois nenhuma tarefa foi deletada (pois a tarefa já estava deletada)" );
            Assert.IsNull( tarefaExcluida, "Deveria ser nulo, pois a tarefa deveria ter sido excluida." );
        }

        [TestMethod]
        public void ExcluirTarefaQuandoExistirHistoricoELogAlteracao()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa(  (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            TarefaHistoricoTrabalho tarefaHistorico = new TarefaHistoricoTrabalho();
            tarefaHistorico.OidTarefa = tarefa.Oid;
            tarefaHistorico.OidColaborador = colaborador1.Oid;
            tarefaHistorico.DtRealizado = DateTime.Now;
            tarefaHistorico.HoraFinal = new TimeSpan( 10, 0, 0 );
            tarefaHistorico.HoraInicio = new TimeSpan( 5, 0, 0 );
            tarefaHistorico.HoraRealizado = new TimeSpan( 4, 0, 0 );
            tarefaHistorico.HoraRestante = new TimeSpan( 6, 0, 0 );
            tarefaHistorico.TxComentario = "Comentario";

            contexto.TarefaHistoricoTrabalho.Add( tarefaHistorico );
            contexto.SaveChanges();

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( tarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();
            oidTarefasExcluidas = TarefaBo.ExcluirTarefa( contexto, oidTarefas );

            Tarefa tarefaExcluida = TarefaDao.ConsultarTarefaPorOid( tarefa.Oid );

            TarefaHistoricoTrabalho tarefaHistoricoExcluido = contexto.TarefaHistoricoTrabalho.FirstOrDefault( o => o.OidTarefa == tarefa.Oid );

            TarefaLogAlteracao tarefaLogExcluido = contexto.TarefaLogAlteracao.FirstOrDefault( o => o.OidTarefa == tarefa.Oid );

            Assert.AreEqual( 1, oidTarefasExcluidas.Count, "Deve ter excluido uma tarefa" );
            Assert.AreEqual( tarefa.Oid, oidTarefasExcluidas[0], "Deve ter excluido a tarefa com este oid" );
            Assert.IsNull( tarefaExcluida, "Deveria ser nulo, pois a tarefa deveria ter sido excluida." );
            Assert.IsNotNull( tarefaHistoricoExcluido, "Não deveria ser nulo, pois a tarefa deveria ter sido excluida, mas o historico permanece." );
            Assert.IsNotNull( tarefaLogExcluido, "Não deveria ser nulo, pois a tarefa deveria ter sido excluida, mas o Log permanece." );
        }

        [TestMethod]
        public void EditarTarefaQuandoLinhaBaseNaoEstiverSalvaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao( contexto );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dataInicio = DateTime.Now;

            List<CronogramaTarefa> tarefaImpactadas;

            DateTime dataHoraAcao = new DateTime();

            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronogramaPadrao1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dataInicio, responsaveis, colaborador1.Usuario.UserName, out tarefaImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            //busca tarefa criada
            CronogramaTarefa tarefaCriada = contexto.CronogramaTarefa.FirstOrDefault( o => o.Oid == novaTarefa.Oid );

            //Alterando responsaveis
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "pedro.lins", true );
            colaborador2.Usuario.UserName = "Pedro";

            Colaborador colaborador3 = ColaboradorFactoryEntity.CriarColaborador( contexto, "joao.lins", true );
            colaborador3.Usuario.UserName = "Joao";

            string responsaveisAlterado = String.Format( "{0},{1}", colaborador2.NomeCompleto, colaborador3.NomeCompleto );

            //cria situação planejamento alterada
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento,
                                                        CsPadraoSistema.Sim, true );

            string txDescricaoAlterada = "Tarefa 01 alterada";
            string txObservacaoAlterada = "Criar método alterado";
            Int16 EstimativaInicialHoraAlterada = 4;

            TarefaBo.EditarTarefa( tarefaCriada.Oid.ToString(), txDescricaoAlterada, situacaoPlanejamento2.Oid.ToString(), colaborador1.Usuario.UserName,
                                  txObservacaoAlterada, responsaveisAlterado, EstimativaInicialHoraAlterada, new TimeSpan( 0 ), new TimeSpan( 0 ), false, dataInicio );

            //busca tarefa criada
            CronogramaTarefa tarefaAlterada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefaCriada.Oid, o => o.Tarefa.SituacaoPlanejamento, o => o.Tarefa.TarefaResponsaveis );

            DateUtil.CurrentDateTime = (DateTime)tarefaAlterada.Tarefa.DtAtualizadoEm;

            Assert.AreEqual( DateUtil.CurrentDateTime, tarefaAlterada.Tarefa.DtAtualizadoEm, "As datas devem ser iguais." );
            Assert.AreEqual( txDescricaoAlterada, tarefaAlterada.Tarefa.TxDescricao, "Devem ser as mesmas, pois a descrição foi alterada." );
            Assert.AreEqual( txObservacaoAlterada, tarefaAlterada.Tarefa.TxObservacao, "Devem ser as mesmas, pois a data foi alterada." );
            Assert.AreEqual( EstimativaInicialHoraAlterada, tarefaAlterada.Tarefa.NbEstimativaInicial, "" );
            Assert.AreEqual( situacaoPlanejamento2.Oid, tarefaAlterada.Tarefa.OidSituacaoPlanejamento, "" );
            Assert.AreEqual( responsaveisAlterado, tarefaAlterada.Tarefa.TxResponsaveis );
        }

        [TestMethod]
        public void EditarTarefaQuandoSalvarLinhaBaseTarefaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao(contexto);

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dataInicio = DateTime.Now;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronogramaPadrao1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dataInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            //busca tarefa criada
            CronogramaTarefa tarefaCriada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid, o => o.Tarefa.SituacaoPlanejamento, o => o.Cronograma, o => o.Tarefa.AtualizadoPor.Usuario.Person, o => o.Tarefa );
            
            bool salvarLinhaBaseTarefa = true;
            
            TimeSpan estimativaRestante = TarefaDao.ConsultarTarefaPorOid( (Guid)tarefaCriada.OidTarefa ).EstimativaRestanteHora;

            string responsaveisTarefaEdicao = colaborador1.NomeCompleto;

            TarefaBo.EditarTarefa( tarefaCriada.Oid.ToString(), tarefaCriada.Tarefa.TxDescricao, tarefaCriada.Tarefa.SituacaoPlanejamento.Oid.ToString(), colaborador1.Usuario.UserName,
                                  tarefaCriada.Tarefa.TxObservacao, responsaveisTarefaEdicao, tarefaCriada.Tarefa.NbEstimativaInicial, estimativaRestante, new TimeSpan( 0 ), salvarLinhaBaseTarefa, dataInicio );

            //busca tarefa criada
            CronogramaTarefa tarefaAlterada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefaCriada.Oid, o => o.Tarefa.SituacaoPlanejamento, o => o.Cronograma, o => o.Tarefa.AtualizadoPor.Usuario.Person, o => o.Tarefa.LogsAlteracao, o => o.Tarefa.TarefaHistoricoTrabalhos, o => o.Tarefa.TarefaResponsaveis );

            Assert.AreEqual( salvarLinhaBaseTarefa, tarefaAlterada.Tarefa.CsLinhaBaseSalva, "Deveria ter salvo a linha de base" );
            Assert.AreEqual( estimativaRestante, tarefaAlterada.Tarefa.NbEstimativaRestante.ToTimeSpan(), "Deveria ter mudado a estimativa restando, pois salvou a linha de base" );
        }

        [TestMethod]
        public void EditarTarefaQuandoAlterarHorasRestantesEHorasRealizadoTarefaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao(contexto);


            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dataInicio = DateTime.Now;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronogramaPadrao1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dataInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            //busca tarefa criada
            CronogramaTarefa tarefaCriada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid, o => o.Tarefa.SituacaoPlanejamento );

            //Salvando a linha de base
            bool salvarLinhaBaseTarefa = true;
            TimeSpan estimativaRestante = tarefaCriada.Tarefa.EstimativaInicialHora;

            string responsaveisAlterado = colaborador1.NomeCompleto;

            TarefaBo.EditarTarefa( tarefaCriada.Oid.ToString(), tarefaCriada.Tarefa.TxDescricao, tarefaCriada.Tarefa.SituacaoPlanejamento.Oid.ToString(), colaborador1.Usuario.UserName,
                                  tarefaCriada.Tarefa.TxObservacao, responsaveisAlterado, tarefaCriada.Tarefa.NbEstimativaInicial, estimativaRestante, new TimeSpan( 0 ), salvarLinhaBaseTarefa, dataInicio );

            //busca tarefa criada
            CronogramaTarefa tarefaAlterada = contexto.CronogramaTarefa.FirstOrDefault( o => o.Oid == tarefaCriada.Oid );

            //Alterando Estimativa Restante da tarefa
            TimeSpan estimativaRestanteAlterada = new TimeSpan( 1, 0, 0 );
            TimeSpan horasRealizado = new TimeSpan( 2, 0, 0 );

            TarefaBo.EditarTarefa( tarefaCriada.Oid.ToString(), tarefaCriada.Tarefa.TxDescricao, tarefaCriada.Tarefa.SituacaoPlanejamento.Oid.ToString(), colaborador1.Usuario.UserName,
                                  tarefaCriada.Tarefa.TxObservacao, responsaveisAlterado, tarefaCriada.Tarefa.NbEstimativaInicial, estimativaRestanteAlterada, horasRealizado, salvarLinhaBaseTarefa , DateTime.Now);

            //busca tarefa criada
            CronogramaTarefa tarefaAlteradaHorario = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefaCriada.Oid, o => o.Tarefa.SituacaoPlanejamento );

            Assert.AreEqual( new TimeSpan( 3, 0, 0 ), tarefaCriada.Tarefa.EstimativaInicialHora, "Deve ser a mesma estimativa inicial." );
            Assert.AreEqual( estimativaRestanteAlterada, tarefaAlteradaHorario.Tarefa.EstimativaRestanteHora, "Deve ter alterado a estimativa restante." );
            Assert.AreEqual( horasRealizado, tarefaAlteradaHorario.Tarefa.EstimativaRealizadoHora, "Deve ter alterado as horas realizado." );
        }

        [TestMethod]
        public void EditarTarefaQuandoOutroUsuarioAlterarTarefaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao(contexto);

            
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dataInicio = DateTime.Now;
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronogramaPadrao1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dataInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            //busca tarefa criada
            CronogramaTarefa tarefaCriada = contexto.CronogramaTarefa.FirstOrDefault( o => o.Oid == novaTarefa.Oid );

            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "pedro.lins", true );

            Colaborador colaborador3 = ColaboradorFactoryEntity.CriarColaborador( contexto, "joao.lins", true );
            
            List<Colaborador> responsaveisAlterados = new List<Colaborador>();
            responsaveisAlterados.Add( colaborador2 );
            responsaveisAlterados.Add( colaborador3 );

            //cria situação planejamento alterada
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador4 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string txDescricaoAlterada = "Tarefa 01 alterada";
            DateTime dataInicioAlterada = new DateTime( 2013, 12, 10 );
            string txObservacaoAlterada = "Criar método alterado";
            Int16 EstimativaInicialHoraAlterada = 4;

            string responsaveisAlterado = colaborador1.NomeCompleto;

            TarefaBo.EditarTarefa( tarefaCriada.Oid.ToString(), txDescricaoAlterada, situacaoPlanejamento2.Oid.ToString(), colaborador2.Usuario.UserName,
                                  txObservacaoAlterada, responsaveisAlterado, EstimativaInicialHoraAlterada, new TimeSpan( 0 ), new TimeSpan( 0 ), false ,DateTime.Now);

            //busca tarefa criada
            CronogramaTarefa tarefaAlterada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( tarefaCriada.Oid, o => o.Tarefa.SituacaoPlanejamento );

            DateUtil.CurrentDateTime = (DateTime)tarefaAlterada.Tarefa.DtAtualizadoEm;

            Assert.AreEqual( DateUtil.CurrentDateTime, tarefaAlterada.Tarefa.DtAtualizadoEm, "As datas devem ser iguais." );
            Assert.AreEqual( txDescricaoAlterada, tarefaAlterada.Tarefa.TxDescricao, "Devem ser as mesmas, pois a descrição foi alterada." );
            Assert.AreEqual( txObservacaoAlterada, tarefaAlterada.Tarefa.TxObservacao, "Devem ser as mesmas, pois a data foi alterada." );
            Assert.AreEqual( EstimativaInicialHoraAlterada, tarefaAlterada.Tarefa.NbEstimativaInicial, "Devem ser iguais, pois foi alterada" );
            Assert.AreEqual( situacaoPlanejamento2.Oid, tarefaAlterada.Tarefa.OidSituacaoPlanejamento, "Devem ser iguais, pois foi alterada" );
            Assert.AreEqual( colaborador2.Oid, tarefaAlterada.Tarefa.OidAtualizadoPor, "Deve ser o mesmo, pois foi outro usuário que alterou a tarefa" );
        }

        [TestMethod]
        public void CriarTarefaQuandoCriarLogPrimeiraVezTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa(  (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            DateUtil.CurrentDateTime = (DateTime)tarefa.DtAtualizadoEm;

            Assert.AreEqual( DateUtil.CurrentDateTime, tarefa.DtAtualizadoEm );
            Assert.AreEqual( colaborador1.NomeCompleto, TarefaDao.ConsultarTarefaPorOid( tarefa.Oid, o => o.AtualizadoPor.Usuario.Person ).AtualizadoPor.NomeCompleto );
        }

        [TestMethod]
        public void CriarTarefaQuandoLogSofrerAlteracaoPorOutroUsuarioTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();


            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            //cria tarefa
            Tarefa tarefa = new Tarefa();

            tarefa = TarefaBo.CriarTarefa(  (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            tarefa.TxDescricao = "Tarefa Alterada";
            tarefa.DtAtualizadoEm = DateUtil.ConsultarDataHoraAtual();
            tarefa.AtualizadoPor = colaborador2;

            contexto.SaveChanges();

            DateUtil.CurrentDateTime = (DateTime)tarefa.DtAtualizadoEm;

            Assert.AreEqual( "Tarefa Alterada", tarefa.TxDescricao );
            Assert.AreEqual( DateUtil.CurrentDateTime, tarefa.DtAtualizadoEm );
            Assert.AreEqual( colaborador2, tarefa.AtualizadoPor );
        }

        /// <summary>
        /// Método para testar se uma tarefa está sendo criada com apenas 1 responsável.
        /// </summary>
        [TestMethod]
        public void CriarTarefaQuandoHouverUmResponsavelApenasTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();
            tarefa = TarefaBo.CriarTarefa(  (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3);

            //busca tarefa criada
            Tarefa tarefaCriada = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa.Oid );
            
            //verifica se existe
            Assert.IsNotNull( tarefaCriada, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa.Oid, tarefaCriada.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );
        }

        /// <summary>
        /// Método para testar se uma tarefa está sendo criada com mais de 1 responsável.
        /// </summary>
        [TestMethod]
        public void CriarTarefaQuandoHouverUmOuMaisResponsaveisTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            
            

            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "pedro.lins", true );
            colaborador2.Usuario.UserName = "Pedro";

            Colaborador colaborador3 = ColaboradorFactoryEntity.CriarColaborador( contexto, "joao.lins", true );
            colaborador3.Usuario.UserName = "Joao";

            string responsaveis = String.Format( "{0},{1},{2}", colaborador1.NomeCompleto, colaborador2.NomeCompleto, colaborador3.NomeCompleto );

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa = new Tarefa();
            tarefa = TarefaBo.CriarTarefa(  (string)"Tarefa 01", situacaoPlanejamento, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método", responsaveis, 3 );

            //busca tarefa criada
            Tarefa tarefaCriada = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa.Oid );

            //verifica se existe
            Assert.IsNotNull( tarefaCriada, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa.Oid, tarefaCriada.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            //verifica se os responsáveis são os mesmos.
            Assert.AreEqual( responsaveis, tarefaCriada.TxResponsaveis, "Deveria ser a mesma string com os mesmos colaboradores." );
        }

        /// <summary>
        /// Método para testar se uma tarefa está sendo criada com a Situação Planejamento Padrão.
        /// </summary>
        [TestMethod]
        public void CriarTarefaQuandoSituacaoPlanejamentoForPadraoTest()
        {
            //Situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "pedro.lins", true );
            
            colaborador2.Usuario.UserName = "pedro.alves";

            string responsaveis = String.Format( "{0},{1}", colaborador1.NomeCompleto, colaborador2.NomeCompleto );

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa1 = new Tarefa();
            tarefa1 = TarefaBo.CriarTarefa( "Tarefa 01", situacaoPlanejamento1, dtInicio, colaborador1.Usuario.UserName, "Criar método1", responsaveis, 3 );

            //busca tarefa criada
            Tarefa tarefaCriada1 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa1.Oid );

            //verifica se existe
            Assert.IsNotNull( tarefaCriada1, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa1.Oid, tarefaCriada1.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            //verifica se os responsáveis são os mesmos.
            Assert.AreEqual( "Anderson Lins,Pedro Lins", tarefa1.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );

            //Verifica Situação Planejamento
            Assert.AreEqual( situacaoPlanejamento1.Oid, tarefa1.OidSituacaoPlanejamento );
            Assert.AreEqual( SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto ).Oid, tarefa1.OidSituacaoPlanejamento );
        }

        /// <summary>
        /// Método para testar se uma tarefa está sendo criada com a Situação Planejamento que foi passada por parâmetro.
        /// </summary>
        [TestMethod]
        public void CriarTarefaQuandoSituacaoPlanejamentoNaoForPadraoTest()
        {
            //Situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Não, true );

            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Impedimento, CsPadraoSistema.Não, true );

            SituacaoPlanejamento situacaoPlanejamento3 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S3", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Não, true );

            SituacaoPlanejamento situacaoPlanejamento4 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S4", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento, CsPadraoSistema.Não, true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "pedro.lins", true );
            
            colaborador2.Usuario.UserName = "pedro.alves";
            
            Colaborador colaborador3 = ColaboradorFactoryEntity.CriarColaborador( contexto, "joao.lins", true );
            
            colaborador3.Usuario.UserName = "joao.pereira";
            
            string responsaveis = String.Format( "{0},{1},{2}", colaborador1.NomeCompleto, colaborador2.NomeCompleto, colaborador3.NomeCompleto );

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa1 = new Tarefa();
            tarefa1 = TarefaBo.CriarTarefa(  "Tarefa 01", situacaoPlanejamento1, dtInicio, colaborador1.Usuario.UserName, "Criar método1",
                                            responsaveis, 3 );

            Tarefa tarefa2 = new Tarefa();
            tarefa2 = TarefaBo.CriarTarefa(  "Tarefa 02", situacaoPlanejamento2, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método2",
                                            responsaveis, 3 );

            Tarefa tarefa3 = new Tarefa();
            tarefa3 = TarefaBo.CriarTarefa(  "Tarefa 03", situacaoPlanejamento3, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método3",
                                            responsaveis, 3 );

            Tarefa tarefa4 = new Tarefa();
            tarefa4 = TarefaBo.CriarTarefa(  (string)"Tarefa 04", situacaoPlanejamento4, dtInicio, colaborador1.Usuario.UserName, (string)"Criar método4",
                                            responsaveis, 3 );

            //busca tarefa criada
            Tarefa tarefaCriada1 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa1.Oid );
            Tarefa tarefaCriada2 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa2.Oid );
            Tarefa tarefaCriada3 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa3.Oid );
            Tarefa tarefaCriada4 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa4.Oid );

            //verifica se existe
            Assert.IsNotNull( tarefaCriada1, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa1.Oid, tarefaCriada1.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            Assert.IsNotNull( tarefaCriada2, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa2.Oid, tarefaCriada2.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            Assert.IsNotNull( tarefaCriada3, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa3.Oid, tarefaCriada3.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            Assert.IsNotNull( tarefaCriada4, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa4.Oid, tarefaCriada4.Oid, "Deveria ter criado uma tarefa com os mesmos dados" );

            //verifica se os responsáveis são os mesmos.
            Assert.AreEqual( "Anderson Lins,Pedro Lins,Joao Lins", tarefa1.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );
            Assert.AreEqual( "Anderson Lins,Pedro Lins,Joao Lins", tarefa2.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );
            Assert.AreEqual( "Anderson Lins,Pedro Lins,Joao Lins", tarefa3.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );
            Assert.AreEqual( "Anderson Lins,Pedro Lins,Joao Lins", tarefa4.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );

            //Verifica Situação Planejamento
            Assert.AreEqual( situacaoPlanejamento1.Oid, tarefa1.OidSituacaoPlanejamento );
            Assert.AreEqual( situacaoPlanejamento2.Oid, tarefa2.OidSituacaoPlanejamento );
            Assert.AreEqual( situacaoPlanejamento3.Oid, tarefa3.OidSituacaoPlanejamento );
            Assert.AreEqual( situacaoPlanejamento4.Oid, tarefa4.OidSituacaoPlanejamento );
        }

        /// <summary>
        /// Método para testar se uma tarefa está sendo criada com a Situação Planejamento que foi passada por parâmetro.
        /// </summary>
        [TestMethod]
        [ExpectedException( typeof( ArgumentException ) )]
        public void CriarTarefaQuandoSituacaoPlanejamentoPadraoNaoExistirTest()
        {
            //Situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Não, true );

            
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            
            string responsaveis = String.Format( "{0}", colaborador1.NomeCompleto );

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa1 = new Tarefa();
            tarefa1 = TarefaBo.CriarTarefa(  "Tarefa 01", null, dtInicio, colaborador1.Usuario.UserName, "Criar método1", responsaveis, 3 );

            //busca tarefa criada
            Tarefa tarefaCriada1 = contexto.Tarefa.FirstOrDefault( o => o.Oid == tarefa1.Oid );

            //verifica se existe
            Assert.IsNotNull( tarefaCriada1, "Deveria retornar uma tarefa." );
            Assert.AreEqual( tarefa1, tarefaCriada1, "Deveria ter criado uma tarefa com os mesmos dados" );
            //verifica se os responsáveis são os mesmos.
            Assert.AreEqual( "Anderson,Pedro,Joao", tarefa1.TxResponsaveis, "Deveria retornar 3 responsáveis cadastrados na tarefa." );

            //Verifica Situação Planejamento
            Assert.AreEqual( situacaoPlanejamento1, tarefa1.SituacaoPlanejamento );
        }

        /// <summary>
        /// Cenário: Quando salvar um histórico de tarefa.
        /// Expectativa: Deverá atualizar a estimativa restante da tarefa
        ///              Deverá atualizar a situação planejamento da tarefa
        /// </summary>
        [TestMethod]
        public void AtualizarDadosTarefaQuandoJáExistirColaboradorNaListaDeResponsaveisTest()
        {
            //Cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                                                                     CsTipoSituacaoPlanejamento.Ativo,
                                                                                                     CsTipoPlanejamento.Execução,
                                                                                                     CsPadraoSistema.Não, true );

            //Cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                                                                     CsTipoSituacaoPlanejamento.Ativo,
                                                                                                     CsTipoPlanejamento.Execução,
                                                                                                     CsPadraoSistema.Não, true );
            
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );
            
            
            string responsaveis = String.Format( "{0}", colaborador1.NomeCompleto );
            

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa1 = new Tarefa();
            tarefa1 = TarefaBo.CriarTarefa(  "Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName, "Criar método1", responsaveis, 5 );

            //busca tarefa criada
            Tarefa tarefaCriada = TarefaDao.ConsultarTarefaPorOid( tarefa1.Oid, o => o.SituacaoPlanejamento );

            //simulando passagem de parâmetros ao criar um histórico.
            TarefaBo.AtualizarDadosTarefa( tarefaCriada.Oid, colaborador1, situacaoPlanejamentoEmAndamento, new TimeSpan( 3, 0, 0 ), new TimeSpan( 2, 0, 0 ), tarefaCriada.CsLinhaBaseSalva );

            Tarefa tarefaAlterada = TarefaDao.ConsultarTarefaPorOid( tarefaCriada.Oid, o => o.SituacaoPlanejamento );
            List<string> responsaveisTarefa = new List<string>( tarefaAlterada.TxResponsaveis.Split( ',' ) );

            //Responsável Tarefa
            Assert.AreEqual( 1, responsaveisTarefa.Count, "Deveria ter um responsável, pois foi adicionado apenas 1 colaborador pela tarefa." );
            Assert.AreEqual( colaborador1.NomeCompleto, responsaveisTarefa.First(), "Deveriam ser iguais, pois é o único cadastrado." );

            //Situacao Planejamento
            Assert.AreEqual( tarefaAlterada.OidSituacaoPlanejamento, situacaoPlanejamentoEmAndamento.Oid, "Deveria ser o mesmo Oid, pois a tarefa foi alterada para Em Andamento." );

            //Estimativas
            Assert.AreEqual( new TimeSpan( 2, 0, 0 ), tarefaAlterada.NbEstimativaRestante.ToTimeSpan(), "Deveriam ser iguais, pois a tarefa possui 5 horas de estimativa inicial e foram realizadas 3 horas, então sobram 2" );
            Assert.AreEqual( new TimeSpan( 3, 0, 0 ), tarefaAlterada.NbRealizado.ToTimeSpan(), "Deveria ser 3 horas, pois foram realizadas 3 horas na tarefa e o historico foi salvo." );
        }

        /// <summary>
        /// Cenário: Quando salvar um histórico de tarefa.
        /// Expectativa: Deverá atualizar a estimativa restante da tarefa
        ///              Deverá atualizar a situação planejamento da tarefa
        ///              Deverá atualizar a lista de responsáveis pela tarefa
        /// </summary>
        [TestMethod]
        public void AtualizarDadosTarefaQuandoNaoExistirColaboradorNaListaDeResponsaveisTest()
        {
            //Cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                                                                     CsTipoSituacaoPlanejamento.Ativo,
                                                                                                     CsTipoPlanejamento.Execução,
                                                                                                     CsPadraoSistema.Não, true );

            //Cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                                                                     CsTipoSituacaoPlanejamento.Ativo,
                                                                                                     CsTipoPlanejamento.Execução,
                                                                                                     CsPadraoSistema.Não, true );
            
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = String.Format( "{0}", colaborador1.NomeCompleto );

            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            //cria tarefa
            Tarefa tarefa1 = new Tarefa();
            tarefa1 = TarefaBo.CriarTarefa(  "Tarefa 01", situacaoPlanejamentoNaoIniciado, dtInicio, colaborador1.Usuario.UserName, "Criar método1", responsaveis, 5 );

            //busca tarefa criada
            Tarefa tarefaCriada = TarefaDao.ConsultarTarefaPorOid( tarefa1.Oid, o => o.SituacaoPlanejamento );

            //simulando passagem de parâmetros ao criar um histórico, mas agora com um colaborador que nao estava associado à tarefa antes.
            TarefaBo.AtualizarDadosTarefa( tarefaCriada.Oid, colaborador2, situacaoPlanejamentoEmAndamento, new TimeSpan( 3, 0, 0 ), new TimeSpan( 2, 0, 0 ), tarefaCriada.CsLinhaBaseSalva );

            Tarefa tarefaAlterada = TarefaDao.ConsultarTarefaPorOid( tarefaCriada.Oid, o => o.SituacaoPlanejamento );
            List<string> responsaveisTarefa = new List<string>( tarefaAlterada.TxResponsaveis.Split( ',' ) );

            Assert.AreEqual( 2, responsaveisTarefa.Count, "Deveria ter 2 responsáveis, pois um foi adicionado ao criar tarefa e o outro adicionado ao acionar o método AdicionarResponsavelTarefa." );
            Assert.IsTrue( responsaveisTarefa.Contains( colaborador2.NomeCompleto ), "Deveriam ser iguais, pois foi adicionado à lista de responsáveis." );

            //Situacao Planejamento
            Assert.AreEqual( tarefaAlterada.OidSituacaoPlanejamento, situacaoPlanejamentoEmAndamento.Oid, "Deveria ser o mesmo Oid, pois a tarefa foi alterada para Em Andamento." );

            //Estimativas
            Assert.AreEqual( new TimeSpan( 2, 0, 0 ), tarefaAlterada.NbEstimativaRestante.ToTimeSpan(), "Deveriam ser iguais, pois a tarefa possui 5 horas de estimativa inicial e foram realizadas 3 horas, então sobram 2" );
            Assert.AreEqual( new TimeSpan( 3, 0, 0 ), tarefaAlterada.NbRealizado.ToTimeSpan(), "Deveria ser 3 horas, pois foram realizadas 3 horas na tarefa e o historico foi salvo." );
        }

        [TestMethod]
        public void DeveCriarUmRespectivoItemDeTrabalhoParaAsTarefasQueForemCriadas()
        {
            Assert.Inconclusive( "Falta correção de ItemDeTrabalho" );

            Cronograma cronograma = new Cronograma();
            cronograma.TxDescricao = "Cronograma1";
            contexto.Cronograma.Add( cronograma );

            SituacaoPlanejamento situacao = new SituacaoPlanejamento();
            situacao.CsTipo = CsTipoPlanejamento.Planejamento;
            situacao.TxDescricao = "Nao iniciado";
            contexto.SituacaoPlanejamento.Add( situacao );

            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            contexto.SaveChanges();

            DateTime dat2 = DateTime.Now;
            List<CronogramaTarefa> tarefasList = new List<CronogramaTarefa>();
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, "T1", situacao, DateTime.Now, "", "gabriel.matos", out tarefasList, ref dat2, "" );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, "T2", situacao, DateTime.Now, "", "gabriel.matos", out tarefasList, ref dat2, "" );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, "T3", situacao, DateTime.Now, "", "gabriel.matos", out tarefasList, ref dat2, "" );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, "T4", situacao, DateTime.Now, "", "gabriel.matos", out tarefasList, ref dat2, "" );
            List<Tarefa> tarefas = contexto.Tarefa.ToList();

            //List<ItemDeTrabalho> itens = new XPCollection<ItemDeTrabalho>( contexto ).ToList();
            //List<Guid> oidTarefas = new List<Guid>( tarefas.Select( o => o.Oid ) );
            //List<Guid> itensTrabalhoOid = new List<Guid>( itens.Select( o => o.Oid ) );
            //CollectionAssert.AreEquivalent( itensTrabalhoOid, oidTarefas, "Itens de trabalho deveria conter o oid de todas as tarefas criadas" );
        }
    }
}
