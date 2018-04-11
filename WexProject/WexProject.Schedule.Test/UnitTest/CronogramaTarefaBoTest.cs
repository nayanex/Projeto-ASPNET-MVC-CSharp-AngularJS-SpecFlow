using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using System.Collections;
using System.Threading;
using WexProject.Library.Libs.Test;
using WexProject.BLL.Models;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Contexto;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaTarefaBoTest : BaseEntityFrameworkTest
    {
        #region Constantes
        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma coleção de tarefas impactadas
        /// </summary>
        const string TAREFAS_IMPACTADAS = "TarefasImpactadas";
        #endregion

        #region Objetos Auxiliares para o Teste

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct Objetos
        {
            public Guid oidCronograma;
            public string txDescricaoTarefa;
            public SituacaoPlanejamento situacaoPlanejamento;
            public string login;
            public Guid oidTarefaSelecionada;
            public short nbIDdestino;
        }

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct ObjetosReordenarPorBlocoAoMoverTarefas
        {
            public CronogramaTarefa tarefaParaMover;
            public string txDescricaoTarefa;
            public short nbIDdestino;
            public List<CronogramaTarefa> tarefasParaReordenar;
            public Guid oidCronograma;
        }

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct ObjetosReordenarPorBlocoAoExcluirTarefas
        {
            public CronogramaTarefa tarefaReferenciaParaReordenacao;
            public string txDescricaoTarefa;
            public List<CronogramaTarefa> tarefasParaReordenar;
        }

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct ObjetosReordenarPorBlocoAoCriarTarefas
        {
            public CronogramaTarefa tarefaReferenciaParaReordenacao;
            public List<CronogramaTarefa> tarefasParaReordenar;
            public CronogramaTarefa novaTarefaCriada;
        }

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct ObjetosReordenarPorBlocoAoExcluirTarefasCompleto
        {
            public List<Guid> oidsParaExcluir;
            public Guid oidCronograma;
        }

        /// <summary>
        /// Struct para ser usado na passagem de parâmetros para as threads
        /// </summary>
        public struct ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao
        {
            public Guid oidCronograma;
            public string txDescricao;
            public SituacaoPlanejamento situacaoPlanejamento;
            public short NbIDReferencia;
        }

        #endregion

        #region Métodos Auxiliares Para o Teste

        /// <summary>
        /// Método que é acessado pelas threads quando ocorrer a ação de mover tarefas
        /// </summary>
        /// <param name="objeto">Objeto contendo todos os parametros necessários para a chamada do método</param>
        public void ReordenarPorBlocoAoMoverTarefas( object objeto )
        {
            ObjetosReordenarPorBlocoAoMoverTarefas objetosReordenarPorBloco = (ObjetosReordenarPorBlocoAoMoverTarefas)objeto;

            DateTime dataHoraAcao = new DateTime();
            short nbIdAtualizadoTarefaMovida = 0;

            if(objetosReordenarPorBloco.oidCronograma != null)
            {
                CronogramaTarefaBo.ReordenarTarefas( objetosReordenarPorBloco.tarefaParaMover.Oid, objetosReordenarPorBloco.nbIDdestino, ref nbIdAtualizadoTarefaMovida, ref dataHoraAcao, ref objetosReordenarPorBloco.oidCronograma );
            }
            else
            {
                CronogramaTarefaBo.RecalcularPorBloco( objetosReordenarPorBloco.tarefaParaMover, objetosReordenarPorBloco.tarefasParaReordenar, ref dataHoraAcao, true, true, objetosReordenarPorBloco.nbIDdestino );
            }
        }

        /// <summary>
        /// Método acessado pelas threads quando ocorrer a ação de excluir tarefas
        /// </summary>
        /// <param name="objeto">Objeto contendo todos os parametros necessários para a chamada do método</param>
        public void ReordenarPorBlocoAoExcluirTarefas( object objeto )
        {
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosReordenarPorBloco = (ObjetosReordenarPorBlocoAoExcluirTarefas)objeto;

            DateTime dataHoraAcao = new DateTime();

            CronogramaTarefaBo.RecalcularPorBloco( objetosReordenarPorBloco.tarefaReferenciaParaReordenacao, objetosReordenarPorBloco.tarefasParaReordenar, ref dataHoraAcao, true );
        }

        /// <summary>
        /// Método acessado pelas threads quando ocorrer a ação de criar tarefas
        /// </summary>
        /// <param name="objeto">Objeto contendo todos os parametros necessários para a chamada do método</param>
        public void ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao( object objeto )
        {
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosReordenarPorBloco = (ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao)objeto;

            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas = new List<CronogramaTarefa>();

            CronogramaTarefaBo.CriarCronogramaTarefa( objetosReordenarPorBloco.oidCronograma, objetosReordenarPorBloco.txDescricao, objetosReordenarPorBloco.situacaoPlanejamento, new DateTime(), "", "anderson.lins", out tarefasImpactadas, ref dataHoraAcao, "", 3, objetosReordenarPorBloco.NbIDReferencia );
        }

        public void ReordenarPorBlocoAoExcluirTarefasCompleto( object objeto )
        {
            ObjetosReordenarPorBlocoAoExcluirTarefasCompleto objetosReordenarPorBloco = (ObjetosReordenarPorBlocoAoExcluirTarefasCompleto)objeto;

            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            CronogramaTarefaBo.ExcluirCronogramaTarefas( objetosReordenarPorBloco.oidsParaExcluir, objetosReordenarPorBloco.oidCronograma, ref tarefasImpactadas, ref tarefasNaoExcluidas, ref dataHoraAcao );
        }

        /// <summary>
        /// Método acessado pelas threads quando ocorrer a ação de criar tarefas
        /// </summary>
        /// <param name="objeto">Objeto contendo todos os parametros necessários para a chamada do método</param>
        public void ReordenarPorBlocoAoCriarTarefas( object objeto )
        {
            ObjetosReordenarPorBlocoAoCriarTarefas objetosReordenarPorBloco = (ObjetosReordenarPorBlocoAoCriarTarefas)objeto;

            DateTime dataHoraAcao = new DateTime();

            CronogramaTarefaBo.RecalcularPorBloco( objetosReordenarPorBloco.novaTarefaCriada, objetosReordenarPorBloco.tarefasParaReordenar, ref dataHoraAcao, true, true, 0, objetosReordenarPorBloco.novaTarefaCriada );
        }

        #endregion

        /// <summary>
        /// Cenário: Excluir uma tarefa quando existem várias tarefas.
        /// Expectativa: A reordenação deve ocorrer.
        /// </summary>
        [TestMethod]
        public void ExcluirTarefasQuandoExistiremVariasTarefasEForNecessarioReordenar()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefa tarefaExcluir = cronogramaTarefas.FirstOrDefault( o => o.NbID == 3 );

            List<Guid> oidTarefas = new List<Guid>();
            List<Guid> oidTarefasExcluidas = new List<Guid>();
            List<CronogramaTarefa> tarefasExcluidas = new List<CronogramaTarefa>();

            oidTarefas.Add( tarefaExcluir.Oid );

            List<CronogramaTarefa> tarefasImpactadasPelaExclusao = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            tarefasExcluidas = CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefas, cronograma1.Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            oidTarefasExcluidas = tarefasExcluidas.Select( o => o.Oid ).ToList();

            Assert.AreEqual( 1, oidTarefasExcluidas.Count, "Deveria ter excluido 1 tarefas" );
            Assert.AreEqual( 2, tarefasImpactadasPelaExclusao.Count, "Deveria ter reordenado 2 tarefas" );
            Assert.AreEqual( tarefaExcluir.Oid, oidTarefasExcluidas[0], "Deveria ter excluido a tarefa com o mesmo Oid" );

            CronogramaTarefa tarefa4 = contexto.CronogramaTarefa.FirstOrDefault( o => o.Tarefa.TxDescricao == "Tarefa 04" );
            CronogramaTarefa tarefa5 = contexto.CronogramaTarefa.FirstOrDefault( o => o.Tarefa.TxDescricao == "Tarefa 05" );

            Assert.AreEqual( 3, tarefa4.NbID, "Deveria ter reordenado o NbId, pois uma tarefa foi excluida." );
            Assert.AreEqual( 4, tarefa5.NbID, "Deveria ter reordenado o NbId, pois uma tarefa foi excluida." );
        }

        [TestMethod]
        public void ExcluirTarefasQuandoExistirTarefaParaExcluir()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();

            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( novaTarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();

            List<CronogramaTarefa> tarefasImpactadasPelaExclusao = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            List<CronogramaTarefa> tarefasExcluidas = CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefas, cronograma1.Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            oidTarefasExcluidas = tarefasExcluidas.Select( o => o.Oid ).ToList();

            CronogramaTarefa tarefaExcluida = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid );

            Assert.AreEqual( 1, oidTarefasExcluidas.Count, "Deve ter excluido uma tarefa" );
            Assert.AreEqual( novaTarefa.Oid, oidTarefasExcluidas[0], "Deve ter excluido a tarefa com este oid" );
            Assert.IsNull( tarefaExcluida, "Deveria ser nulo, pois a tarefa deveria ter sido excluida." );
        }

        [TestMethod]
        public void ExcluirTarefasQuandoNaoExistirTarefaParaExcluir()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( novaTarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();
            Hashtable tarefas = new Hashtable();

            List<CronogramaTarefa> tarefasImpactadasPelaExclusao = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefas, cronograma1.Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            List<CronogramaTarefa> tarefasExcluidas = CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefas, cronograma1.Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            CronogramaTarefa tarefaExcluida = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid );

            Assert.AreEqual( 0, tarefasExcluidas.Count, "Não deve existir nenhuma tarefa na lista, pois nenhuma tarefa foi deletada (pois a tarefa já estava deletada)" );
            Assert.IsNull( tarefaExcluida, "Deveria ser nulo, pois a tarefa deveria ter sido excluida." );
        }

        [TestMethod]
        public void ExcluirTarefasQuandoExistirHistoricoELogAlteracao()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid, o => o.Tarefa );

            TarefaHistoricoTrabalho tarefaHistorico = new TarefaHistoricoTrabalho();
            tarefaHistorico.OidTarefa = cronogramaTarefa.Tarefa.Oid;
            tarefaHistorico.Colaborador = colaborador1;
            tarefaHistorico.DtRealizado = DateTime.Now;
            tarefaHistorico.HoraFinal = new TimeSpan( 10, 0, 0 );
            tarefaHistorico.HoraInicio = new TimeSpan( 5, 0, 0 );
            tarefaHistorico.HoraRealizado = new TimeSpan( 4, 0, 0 );
            tarefaHistorico.HoraRestante = new TimeSpan( 6, 0, 0 );
            tarefaHistorico.TxComentario = "Comentario";

            contexto.TarefaHistoricoTrabalho.Add( tarefaHistorico );
            contexto.SaveChanges();

            List<Guid> oidTarefas = new List<Guid>();
            oidTarefas.Add( cronogramaTarefa.Oid );

            List<Guid> oidTarefasExcluidas = new List<Guid>();

            List<CronogramaTarefa> tarefasImpactadasPelaExclusao = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            List<CronogramaTarefa> tarefas = new List<CronogramaTarefa>();
            tarefas = CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefas, cronograma1.Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            oidTarefasExcluidas = tarefas.Select( o => o.Oid ).ToList();

            CronogramaTarefa tarefaExcluida = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( cronogramaTarefa.Oid, o => o.Tarefa );

            TarefaHistoricoTrabalho tarefaHistoricoExcluido = contexto.TarefaHistoricoTrabalho.FirstOrDefault( o => o.Tarefa.Oid == cronogramaTarefa.Tarefa.Oid );

            TarefaLogAlteracao tarefaLogExcluido = contexto.TarefaLogAlteracao.FirstOrDefault( o => o.Tarefa.Oid == cronogramaTarefa.Tarefa.Oid );

            Assert.AreEqual( 0, oidTarefasExcluidas.Count, "Não deveria ter excluido uma tarefa, pois ela possui histórico" );
            Assert.IsNotNull( tarefaExcluida, "Não deveria ser nulo, pois a tarefa não deveria ter sido excluida, pois possui histó." );
        }

        /*
         * Cenário: Quando a primeira tarefa de um cronograma for criada.        
         * Espectativas: Deverá criar um ID para tarefa; 
         *               Deverá armazenar na hash estática de tarefas como sendo o último ID.
         *               Criar um segundo cronograma e verificar se hash esta sendo atualizada independentemente
         */
        [TestMethod]
        public void AtribuirIDQuandoPrimeiraTarefaCronogramaForCriadaTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );
            Cronograma cronograma2 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 02", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.OidCronograma = cronograma1.Oid;
            novaTarefa2.OidCronograma = cronograma2.Oid;

            //Atribui um Id inexistente para tarefa 
            CronogramaTarefaBo.AtribuirId( novaTarefa1, 0 );

            //Atribui um Id inexistente para tarefa
            CronogramaTarefaBo.AtribuirId( novaTarefa2, 0 );

            //Verifica se os ids são o 1 e se estão adicionando na hash independentes de cronograma.
            Assert.AreEqual( 1, novaTarefa1.NbID, "Deveria ser o primeiro nbID no cronograma 1" );
            Assert.AreEqual( 1, novaTarefa2.NbID, "Deveria ser o primeiro nbID no cronograma 2" );
            Assert.AreEqual( (Int16)1, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma1.Oid.ToString()], "Deveria ter armazenado na Hash como último ID do cronograma 1" );
            Assert.AreEqual( (Int16)1, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma2.Oid.ToString()], "Deveria ter armazenado na Hash como último ID do cronograma 2" );
        }

        /*
         *   Cenário: Quando uma tarefa for criada na última posição.
         *   Espectativas: Deverá verificar qual o último ID, 
         *                 criar o próximo ID a partir do último 
         *                 deverá armazenar na hash estática de tarefas como sendo o último ID.
         */
        [TestMethod]
        public void AtribuirIDQuandoUmaTarefaNaUltimaPosicaoForCriadaTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );
            Cronograma cronograma2 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 02", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa 1 para cronograma 1 e 2
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 2 );
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma2.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 2 );

            //busca lista de tarefas do cronograma 1
            List<CronogramaTarefa> lstTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            //criar 2 tarefa do cronograma 2
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método2", 2, lstTarefas[0].NbID );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.OidCronograma = cronograma1.Oid;
            lstTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            //Atribui um Id inexistente para tarefa 
            CronogramaTarefaBo.AtribuirId( novaTarefa1, lstTarefas.Last<CronogramaTarefa>().NbID );

            Assert.AreEqual( 3, novaTarefa1.NbID, "O último ID deveria ser 3" );
            Assert.AreEqual( (Int16)3, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma1.Oid.ToString()], "Deveria ter armazenado o último ID na Hash do cronograma 1" );
            Assert.AreEqual( (Int16)1, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma2.Oid.ToString()], "Deveria ter armazenado o último ID na Hash do cronograma 2" );
        }

        /*  Cenário: Quando uma tarefa for criada na no meio de outras tarefas.
         *  Espectativas: 
         *                deverá adicionar a tarefa na lista no lugar da tarefa destino
         *                incrementar os ID das tarefas afetadas
         *                
         */
        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaForCriadaNoMeioTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa1 );

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa2 );

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa3 );

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa4 );

            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa5 );

            CronogramaTarefa novaTarefa6 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa6.Cronograma = cronograma1;
            novaTarefa6.NbID = 6;
            novaTarefa6.Tarefa.TxDescricao = "Tarefa 06";
            novaTarefa6.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa6.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa1.Cronograma.Oid.ToString(), novaTarefa6.NbID );
            contexto.CronogramaTarefa.Add( novaTarefa6 );


            CronogramaTarefa novaTarefa7 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa7.Cronograma = cronograma1;
            novaTarefa7.NbID = 2;
            novaTarefa7.Tarefa.TxDescricao = "Tarefa 07";
            novaTarefa7.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa7.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            contexto.SaveChanges();

            List<CronogramaTarefa> listaTarefas = CronogramaTarefaDao.ConsultarTarefasImpactadas( novaTarefa7.Cronograma.Oid, novaTarefa7.NbID );

            contexto.CronogramaTarefa.Add( novaTarefa7 );
            contexto.SaveChanges();

            DateTime dataHoraDaAcao = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.RecalcularPorBloco( novaTarefa2, listaTarefas, ref dataHoraDaAcao, true, false, 0, novaTarefa7 );

            //Faz uma cópia da lista, a patir da pesquisa da lista através do cronograma da tarefa selecionada.
            List<CronogramaTarefa> resultadoListaTarefas = new List<CronogramaTarefa>( CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( novaTarefa1.Cronograma.Oid, o => o.Tarefa ) );

            Assert.IsNotNull( tarefasImpactadas );
            Assert.AreEqual( (Int16)1, resultadoListaTarefas[0].NbID );

            Assert.AreEqual( (Int16)2, resultadoListaTarefas[1].NbID );
            Assert.AreEqual( "Tarefa 07", resultadoListaTarefas[1].Tarefa.TxDescricao );

            Assert.AreEqual( (Int16)3, resultadoListaTarefas[2].NbID );
            Assert.AreEqual( (Int16)4, resultadoListaTarefas[3].NbID );
            Assert.AreEqual( (Int16)5, resultadoListaTarefas[4].NbID );
            Assert.AreEqual( (Int16)6, resultadoListaTarefas[5].NbID );
            Assert.AreEqual( (Int16)7, resultadoListaTarefas[6].NbID );
        }

        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaForExcluidaNoInicioOuMeio()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa1 );

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa2 );

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa3 );

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa4 );


            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;
            contexto.CronogramaTarefa.Add( novaTarefa5 );

            contexto.SaveChanges();

            //excluindo tarefa
            contexto.CronogramaTarefa.Remove( novaTarefa3 );
            contexto.SaveChanges();

            short nbIDexcluido = novaTarefa3.NbID;
            short nbIDValido = short.Parse( ( (int)novaTarefa3.NbID - 1 ).ToString() );

            List<CronogramaTarefa> lstTarefas = CronogramaTarefaDao.ConsultarTarefasImpactadas( novaTarefa1.Cronograma.Oid, nbIDexcluido, 0 );

            CronogramaTarefa tarefaSelecionada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( novaTarefa1.Cronograma.Oid, nbIDValido );

            DateTime dataHoraDaAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.RecalcularPorBloco( tarefaSelecionada, lstTarefas, ref dataHoraDaAcao, true );

            Dictionary<string, string> tarefasImpactadasValidacao = new Dictionary<string, string>();
            tarefasImpactadasValidacao.Add( novaTarefa4.Oid.ToString(), ( 3 ).ToString() );
            tarefasImpactadasValidacao.Add( novaTarefa5.Oid.ToString(), ( 4 ).ToString() );

            Assert.AreEqual( (short)3, CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa4.Oid ).NbID );
            Assert.AreEqual( (short)4, CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa5.Oid ).NbID );
        }

        /*  Cenário: Quando uma tarefa ja existente for movida para cima.
         *  Espectativas: 
         *                Deverá alterar o ID da tarefa origem para o ID destino 
         *                Deverá atualizar o ID das tarefas entre a posicao anterior da tarefa movida 
         *                ate a posicao destino da tarefa movida 
         */
        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaJaExistenteForMovidaParaCimaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa6 = new CronogramaTarefa() { Tarefa = new Tarefa() };
            novaTarefa6.Cronograma = cronograma1;
            novaTarefa6.NbID = 6;
            CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa1.Cronograma.Oid.ToString(), novaTarefa6.NbID );
            novaTarefa6.Tarefa.TxDescricao = "Tarefa 06";
            novaTarefa6.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa6.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.CronogramaTarefa.Add( novaTarefa6 );
            contexto.SaveChanges();

            short nbIDAtualizadoTarefaMovida = 0;

            DateTime dataHoraAcao = new DateTime();
            Guid oidCronograma = new Guid();
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( novaTarefa5.Oid, novaTarefa2.NbID, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );

            //Faz uma cópia da lista, a patir da pesquisa da lista através do cronograma da tarefa selecionada.
            List<CronogramaTarefa> resultadolstTarefas = new List<CronogramaTarefa>( CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( novaTarefa1.Cronograma.Oid, o => o.Tarefa ) );

            List<CronogramaTarefa> lstTarefasImpactadasTest = new List<CronogramaTarefa>() { novaTarefa2, novaTarefa3, novaTarefa4 };

            Assert.IsNotNull( tarefasImpactadas );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[0].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[1].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[2].Oid ).FirstOrDefault() );
            Assert.AreEqual( 3, tarefasImpactadas.Count );

            Assert.AreEqual( (Int16)1, resultadolstTarefas[0].NbID );

            Assert.AreEqual( (Int16)2, resultadolstTarefas[1].NbID );
            Assert.AreEqual( "Tarefa 05", resultadolstTarefas[1].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa5.Oid, resultadolstTarefas[1].Oid );

            Assert.AreEqual( (Int16)3, resultadolstTarefas[2].NbID );
            Assert.AreEqual( "Tarefa 02", resultadolstTarefas[2].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa2.Oid, resultadolstTarefas[2].Oid );

            Assert.AreEqual( (Int16)4, resultadolstTarefas[3].NbID );
            Assert.AreEqual( (Int16)5, resultadolstTarefas[4].NbID );
            Assert.AreEqual( (Int16)6, resultadolstTarefas[5].NbID );
        }

        /*
            Cenário: Quando uma tarefa ja existente for movida para cima na primeira posicao.
            Espectativas: 
         *                Deverá alterar o ID da tarefa origem para o ID destino 
         *                Deverá atualizar o ID das tarefas entre a posicao anterior da tarefa movida 
         *                ate a posicao destino da tarefa movida 
         */
        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaJaExistenteForMovidaParaCimaParaPrimeiraPosicaoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa6 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa6.Cronograma = cronograma1;
            novaTarefa6.NbID = 6;
            CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa1.Cronograma.Oid.ToString(), novaTarefa6.NbID );
            novaTarefa6.Tarefa.TxDescricao = "Tarefa 06";
            novaTarefa6.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa6.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.CronogramaTarefa.Add( novaTarefa6 );
            contexto.SaveChanges();

            short nbIDAtualizadoTarefaMovida = 0;
            DateTime dataHoraAcao = new DateTime();
            Guid oidCronograma = new Guid();
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( novaTarefa5.Oid, novaTarefa1.NbID, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );

            //Faz uma cópia da lista, a patir da pesquisa da lista através do cronograma da tarefa selecionada.
            List<CronogramaTarefa> resultadolstTarefas = new List<CronogramaTarefa>( CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( novaTarefa1.Cronograma.Oid, o => o.Tarefa ) );

            List<CronogramaTarefa> lstTarefasImpactadasTest = new List<CronogramaTarefa>() { novaTarefa1, novaTarefa2, novaTarefa3, novaTarefa4 };

            Assert.IsNotNull( tarefasImpactadas );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[0].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[1].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[2].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[3].Oid ).FirstOrDefault() );
            Assert.AreEqual( 4, tarefasImpactadas.Count );

            Assert.AreEqual( (Int16)1, resultadolstTarefas[0].NbID );
            Assert.AreEqual( "Tarefa 05", resultadolstTarefas[0].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa5.Oid, resultadolstTarefas[0].Oid );

            Assert.AreEqual( (Int16)2, resultadolstTarefas[1].NbID );
            Assert.AreEqual( "Tarefa 01", resultadolstTarefas[1].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa1.Oid, resultadolstTarefas[1].Oid );

            Assert.AreEqual( (Int16)3, resultadolstTarefas[2].NbID );
            Assert.AreEqual( (Int16)4, resultadolstTarefas[3].NbID );
            Assert.AreEqual( (Int16)5, resultadolstTarefas[4].NbID );
            Assert.AreEqual( (Int16)6, resultadolstTarefas[5].NbID );
        }

        /*
            Cenário: Quando uma tarefa pre-existente for movida para baixo
            Espectativas: 
         *                Deverá alterar o ID da tarefa origem para o ID destino 
         *                Deverá atualizar o ID das tarefas entre a posicao anterior da tarefa movida 
         *                ate a posicao destino da tarefa movida.
         */
        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaForMovidaParaBaixoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa6 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa6.Cronograma = cronograma1;
            novaTarefa6.NbID = 6;
            CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa1.Cronograma.Oid.ToString(), novaTarefa6.NbID );
            novaTarefa6.Tarefa.TxDescricao = "Tarefa 06";
            novaTarefa6.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa6.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.CronogramaTarefa.Add( novaTarefa6 );
            contexto.SaveChanges();

            short nbIDAtualizadoTarefaMovida = 0;
            DateTime dataHoraAcao = new DateTime();
            Guid oidCronograma = new Guid();
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( novaTarefa1.Oid, novaTarefa4.NbID, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );

            //Faz uma cópia da lista, a patir da pesquisa da lista através do cronograma da tarefa selecionada.
            List<CronogramaTarefa> resultadolstTarefas = new List<CronogramaTarefa>( CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( novaTarefa1.Cronograma.Oid, o => o.Tarefa ) );

            List<CronogramaTarefa> lstTarefasImpactadasTest = new List<CronogramaTarefa>() { novaTarefa2, novaTarefa3, novaTarefa4 };

            Assert.IsNotNull( tarefasImpactadas );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[0].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[1].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[2].Oid ).FirstOrDefault() );
            Assert.AreEqual( 3, tarefasImpactadas.Count );

            Assert.AreEqual( (Int16)1, resultadolstTarefas[0].NbID );
            Assert.AreEqual( "Tarefa 02", resultadolstTarefas[0].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa2.Oid, resultadolstTarefas[0].Oid );

            Assert.AreEqual( (Int16)2, resultadolstTarefas[1].NbID );
            Assert.AreEqual( "Tarefa 03", resultadolstTarefas[1].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa3.Oid, resultadolstTarefas[1].Oid );

            Assert.AreEqual( (Int16)3, resultadolstTarefas[2].NbID );
            Assert.AreEqual( "Tarefa 04", resultadolstTarefas[2].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa4.Oid, resultadolstTarefas[2].Oid );

            Assert.AreEqual( (Int16)4, resultadolstTarefas[3].NbID );
            Assert.AreEqual( "Tarefa 01", resultadolstTarefas[3].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa1.Oid, resultadolstTarefas[3].Oid );

            Assert.AreEqual( (Int16)5, resultadolstTarefas[4].NbID );
            Assert.AreEqual( "Tarefa 05", resultadolstTarefas[4].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa5.Oid, resultadolstTarefas[4].Oid );
        }

        /*
            Cenário: Quando uma tarefa pre-existente for movida para baixo na ultima posicao
            Espectativas: 
         *                Deverá alterar o ID da tarefa origem para o ID destino 
         *                Deverá atualizar o ID das tarefas entre a posicao anterior da tarefa movida 
         *                ate a posicao destino da tarefa movida.
         */
        [TestMethod]
        public void ReordenarIDQuandoUmaTarefaForMovidaParaBaixoParaUltimaPosicaoTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //nova tarefa
            CronogramaTarefa novaTarefa1 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa1.Cronograma = cronograma1;
            novaTarefa1.NbID = 1;
            novaTarefa1.Tarefa.TxDescricao = "Tarefa 01";
            novaTarefa1.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa1.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa2 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa2.Cronograma = cronograma1;
            novaTarefa2.NbID = 2;
            novaTarefa2.Tarefa.TxDescricao = "Tarefa 02";
            novaTarefa2.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa2.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa3 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa3.Cronograma = cronograma1;
            novaTarefa3.NbID = 3;
            novaTarefa3.Tarefa.TxDescricao = "Tarefa 03";
            novaTarefa3.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa3.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa4 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa4.Cronograma = cronograma1;
            novaTarefa4.NbID = 4;
            novaTarefa4.Tarefa.TxDescricao = "Tarefa 04";
            novaTarefa4.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa4.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa5 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa5.Cronograma = cronograma1;
            novaTarefa5.NbID = 5;
            novaTarefa5.Tarefa.TxDescricao = "Tarefa 05";
            novaTarefa5.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa5.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            CronogramaTarefa novaTarefa6 = new CronogramaTarefa() { Tarefa = new Tarefa() };

            novaTarefa6.Cronograma = cronograma1;
            novaTarefa6.NbID = 6;
            CronogramaTarefaBo.maiorNbIDPorCronograma.Add( novaTarefa1.Cronograma.Oid.ToString(), novaTarefa6.NbID );
            novaTarefa6.Tarefa.TxDescricao = "Tarefa 06";
            novaTarefa6.Tarefa.AtualizadoPor = colaborador1;
            novaTarefa6.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            contexto.CronogramaTarefa.Add( novaTarefa1 );
            contexto.CronogramaTarefa.Add( novaTarefa2 );
            contexto.CronogramaTarefa.Add( novaTarefa3 );
            contexto.CronogramaTarefa.Add( novaTarefa4 );
            contexto.CronogramaTarefa.Add( novaTarefa5 );
            contexto.CronogramaTarefa.Add( novaTarefa6 );
            contexto.SaveChanges();

            short nbIDAtualizadoTarefaMovida = 0;
            DateTime dataHoraAcao = new DateTime();
            Guid oidCronograma = new Guid();
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaBo.ReordenarTarefas( novaTarefa1.Oid, novaTarefa6.NbID, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );

            //Faz uma cópia da lista, a patir da pesquisa da lista através do cronograma da tarefa selecionada.
            List<CronogramaTarefa> resultadolstTarefas = new List<CronogramaTarefa>( CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( novaTarefa1.Cronograma.Oid, o => o.Tarefa ) );

            List<CronogramaTarefa> lstTarefasImpactadasTest = new List<CronogramaTarefa>() { novaTarefa2, novaTarefa3, novaTarefa4, novaTarefa5, novaTarefa6 };

            Assert.IsNotNull( tarefasImpactadas );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[0].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[1].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[2].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[3].Oid ).FirstOrDefault() );
            Assert.IsNotNull( tarefasImpactadas.Where( o => o.Oid == lstTarefasImpactadasTest[4].Oid ).FirstOrDefault() );
            Assert.AreEqual( 5, tarefasImpactadas.Count );

            Assert.AreEqual( (Int16)1, resultadolstTarefas[0].NbID );
            Assert.AreEqual( "Tarefa 02", resultadolstTarefas[0].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa2.Oid, resultadolstTarefas[0].Oid );

            Assert.AreEqual( (Int16)2, resultadolstTarefas[1].NbID );
            Assert.AreEqual( "Tarefa 03", resultadolstTarefas[1].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa3.Oid, resultadolstTarefas[1].Oid );

            Assert.AreEqual( (Int16)3, resultadolstTarefas[2].NbID );
            Assert.AreEqual( "Tarefa 04", resultadolstTarefas[2].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa4.Oid, resultadolstTarefas[2].Oid );

            Assert.AreEqual( (Int16)4, resultadolstTarefas[3].NbID );
            Assert.AreEqual( "Tarefa 05", resultadolstTarefas[3].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa5.Oid, resultadolstTarefas[3].Oid );

            Assert.AreEqual( (Int16)5, resultadolstTarefas[4].NbID );
            Assert.AreEqual( "Tarefa 06", resultadolstTarefas[4].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa6.Oid, resultadolstTarefas[4].Oid );

            Assert.AreEqual( (Int16)6, resultadolstTarefas[5].NbID );
            Assert.AreEqual( "Tarefa 01", resultadolstTarefas[5].Tarefa.TxDescricao );
            Assert.AreEqual( novaTarefa1.Oid, resultadolstTarefas[5].Oid );
        }

        /*
            Cenário: Quando uma tarefa for criada no meio do cronograma
            Espectativas: 
         *                Deverá armazenar a nova tarefa no lugar da tarefa selecionada assim como seu ID  
         *                Deverá reordenar as tarefas que estão no intervalo inicio e final do cronograma
         *                Deverá armazenar na hash estática de tarefas qual o último ID atual
         *                Deverá retornar a lista tarefas impactadas que sera retornada pelo metodo de reordenacao
         */
        [TestMethod]
        public void IncluirTarefaNoMeioDoOutrasTarefasTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );
            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();

            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            List<CronogramaTarefa> lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método2", 3, lstCronoTarefa[0].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método3", 3, lstCronoTarefa[1].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 04", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método4", 3, lstCronoTarefa[1].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            List<CronogramaTarefa> CronoTarefasOrdenada = lstCronoTarefa.OrderBy( o => o.NbID ).ToList();

            Assert.AreEqual( "Tarefa 04", CronoTarefasOrdenada[1].Tarefa.TxDescricao );
            Assert.AreEqual( 4, lstCronoTarefa.Count );
            Assert.AreEqual( (Int16)4, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma1.Oid.ToString()] );
            Assert.AreEqual( 3, CronoTarefasOrdenada[2].NbID );
            Assert.AreEqual( 4, CronoTarefasOrdenada[3].NbID );
        }

        /*
            Cenário: Quando uma tarefa for criada no meio do cronograma
            Espectativas: 
         *                Deverá armazenar a nova tarefa com o ID da tarefa selecionada + 1
         *                Deverá reordenar as tarefas que estão no intervalo inicio e final do cronograma
         *                Deverá armazenar na hash estática de tarefas qual o último ID atual
         *                Deverá retornar a lista tarefas impactadas que sera retornada pelo metodo de reordenacao
         */
        [TestMethod]
        public void IncluirTarefaComAUltimaTarefaSelecionadaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );
            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            List<CronogramaTarefa> lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método2", 3, lstCronoTarefa[0].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método3", 3, lstCronoTarefa[1].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 04", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método4", 3, lstCronoTarefa[2].NbID );

            lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            List<CronogramaTarefa> dicTarefasImpactadas = new List<CronogramaTarefa>();
            dicTarefasImpactadas = tarefasImpactadas;

            Assert.AreEqual( 0, dicTarefasImpactadas.Count );
            Guid oidUltimaTarefa = lstCronoTarefa.Last().Oid;
            CronogramaTarefa ultimaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidUltimaTarefa, o => o.Tarefa );
            Assert.AreEqual( "Tarefa 04", ultimaTarefa.Tarefa.TxDescricao );
            Assert.AreEqual( 4, lstCronoTarefa.Count );
            Assert.AreEqual( (Int16)4, CronogramaTarefaBo.maiorNbIDPorCronograma[cronograma1.Oid.ToString()] );
        }

        /// <summary>
        /// Método responsável por verificar se Tarefa está sendo incluida em Cronograma e salvando.
        /// </summary>
        [TestMethod]
        public void IncluirTarefaQuandoCriarTarefaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );
            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método", 3, 0 );

            List<CronogramaTarefa> lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            Assert.AreEqual( 1, lstCronoTarefa.Count );
            Assert.AreEqual( "Tarefa 01", lstCronoTarefa[0].Tarefa.TxDescricao );
        }

        public void IncluirTarefa( object dadosTarefa )
        {
            Hashtable dados = dadosTarefa as Hashtable;
            WexDb contexto = dados["session"] as WexDb;
            Guid oidCronograma = (Guid)dados["oidCronograma"];
            string descricao = dados["txDescricao"].ToString();
            SituacaoPlanejamento situacao = dados["situacaoPlanejamento"] as SituacaoPlanejamento;
            string login = dados["login"].ToString();
            Int16 estimativa = 0;
            short nbIDReferencia = (short)dados["nbIDReferencia"];

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            CronogramaTarefaBo.CriarCronogramaTarefa( oidCronograma, descricao, situacao, DateTime.Now, "", login, out tarefasImpactadas, ref dataHoraAcao, "", estimativa, nbIDReferencia );
        }

        [TestMethod]
        public void IncluirTarefaQuandoSofrerAlgumaReordenacao()
        {
            //Cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefa c1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento, DateTime.Now, "", colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método", 3, 0 );
            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefa c2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento, DateTime.Now, "", colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método", 3, 0 );
            //cria tarefa a partir da IncluirTarefa em CronogramaTarefa
            CronogramaTarefa c3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento, DateTime.Now, "", colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método", 3, 0 );

            //Criando tarefa com referencia da primeira Tarefa
            CronogramaTarefa c4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 04", situacaoPlanejamento, DateTime.Now, "", colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método", 3, c1.NbID );

            List<CronogramaTarefa> lstCronoTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.AreEqual( 4, lstCronoTarefa.Count, "Deveria ter criado 4 tarefas." );
            Assert.AreEqual( 1, lstCronoTarefa[0].NbID, "Deveria ter o nbID 1, pois foi a última tarefa criada tendo como referência a posição 1" );
            Assert.AreEqual( 2, lstCronoTarefa[1].NbID, "Deveria ter o nbID 2, pois sofreu reordenação. " );
            Assert.AreEqual( 3, lstCronoTarefa[2].NbID, "Deveria ter o nbID 3, pois sofreu reordenação. " );
            Assert.AreEqual( 4, lstCronoTarefa[3].NbID, "Deveria ter o nbID 4, pois sofreu reordenação. " );
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea da T1 para T6 e T5 para T1, ocorrendo primeiro a thread de T1 para T6 e depois T4 para T1" )]
        public void ReordenarPorBlocoQuandoOcorrerMovimentacaoSimultaneamenteDeTarefasContendoAMesmaTarefaReferenciaTest()
        {
            #region Contexto

            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t1;
            objetosUsuario2.tarefaParaMover = t5;

            objetosUsuario1.nbIDdestino = t6.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );


            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T1 para posicao 6",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T5 para posicao 1",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 5000 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[0].Oid, "Deveria ser a tarefa T5, pois ela foi movimentada para a primeira posição." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter NBID 1, pois ela foi movimentada para a primeira posição." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[1].Oid, "Deveria ser a tarefa T2, pois ela foi sofreu movimentação duas vezes ficando assim na 2 posicao." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter NBID 2, pois ela foi sofreu movimentação duas vezes ficando assim na 2 posicao." );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[2].Oid, "Deveria ser a tarefa T3, pois ela foi sofreu movimentação duas vezes ficando assim na 3 posicao." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter NBID 3, pois ela foi sofreu movimentação duas vezes ficando assim na 3 posicao." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[3].Oid, "Deveria ser a tarefa T4, pois ela foi sofreu movimentação duas vezes ficando assim na 4 posicao." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter NBID 4, pois ela foi sofreu movimentação duas vezes ficando assim na 4 posicao." );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[4].Oid, "Deveria ser a tarefa T6, pois ela foi sofreu movimentação duas vezes ficando assim na 5 posicao." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter NBID 5, pois ela foi sofreu movimentação duas vezes ficando assim na 5 posicao." );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[5].Oid, "Deveria ser a tarefa T1, pois ela foi foi movimentada para a posicao 6." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter NBID 6, pois ela foi sofreu movimentação duas vezes ficando assim na 6 posicao." );
        }

        [TestMethod]
        [Description( "Quando ocorrer a mesma movimentação duas vezes. Exemplo: Usuário 1: Move T1 para 6 posição. Usuário 2: Move T1 para posição 6" )]
        public void OcorrerMovimentacaoIgualDuasVezesTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t1;
            objetosUsuario2.tarefaParaMover = t1;

            objetosUsuario1.nbIDdestino = t6.NbID;
            objetosUsuario2.nbIDdestino = t6.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T1 para posicao 6",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T1 para posicao 6",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 500 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[0].Oid, "Deveria ser a tarefa T2, pois ela sofreu reordenação ficando na posicao 1." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter NBID 1, pois ela sofreu reordenação ficando na posicao 1." );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[1].Oid, "Deveria ser a tarefa T3, pois ela sofreu reordenação ficando na posicao 2." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter NBID 2, pois ela sofreu reordenação ficando na posicao 2." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[2].Oid, "Deveria ser a tarefa T4, pois ela sofreu reordenação ficando na posicao 3." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter NBID 3, pois ela sofreu reordenação ficando na posicao 3." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[3].Oid, "Deveria ser a tarefa T5, pois ela sofreu reordenação ficando na posicao 4." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter NBID 4, pois ela sofreu reordenação ficando na posicao 4." );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[4].Oid, "Deveria ser a tarefa T6, pois ela sofreu reordenação ficando na posicao 5." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter NBID 5, pois ela sofreu reordenação ficando na posicao 5." );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[5].Oid, "Deveria ser a tarefa T1, pois ela foi foi movimentada para a posicao 6." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter NBID 6, pois ela foi foi movimentada para a posicao 6." );
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentacao pra cimA e uma movimentação pra baixo simultaneamente." )]
        public void MovimentacaoPraCimaEMovimentacaoPraBaixoSimultaneamenteTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t2;
            objetosUsuario2.tarefaParaMover = t3;

            objetosUsuario1.nbIDdestino = t4.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T2 para posicao 4",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T3 para posicao 1",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 200 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 120 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[0].Oid, "Deveria ser a tarefa T3, pois ela foi movimentada para posicao 1." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter NBID 1, pois ela foi movimentada para posicao 1." );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[1].Oid, "Deveria ser a tarefa T1, pois ela sofreu reordenação ficando na posicao 2." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter NBID 2, pois ela sofreu reordenação ficando na posicao 2." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[2].Oid, "Deveria ser a tarefa T4, pois ela sofreu reordenação ficando na posicao 3." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter NBID 3, pois ela sofreu reordenação ficando na posicao 3." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[3].Oid, "Deveria ser a tarefa T2, pois ela foi movimentada para posicao 4." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter NBID 4, pois ela foi movimentada para posicao 4." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[4].Oid, "Deveria ser a tarefa T5, pois ela não sofreu reordenação ficando na posicao 5." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter NBID 5, pois ela não sofreu movimentacao, ficando na posicao 5." );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[5].Oid, "Deveria ser a tarefa T6, pois ela não sofreu reordenação ficando na posicao 6." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter NBID 6, pois ela não sofreu movimentacao, ficando na posicao 6." );
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para BAIXO utilizando a mesma tarefa referencia (T1) e movendo para posições diferentes ( T3 e T6)" +
            "Usuário1: Mover T1 para T6" +
            "Usuário2: Mover T1 para T3" )]
        public void MovimentarParaBaixoMesmaTarefaSelecionadaParaPosicoesDiferentesTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t1;
            objetosUsuario2.tarefaParaMover = t1;

            objetosUsuario1.nbIDdestino = t6.NbID;
            objetosUsuario2.nbIDdestino = t3.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T1 para posicao 6",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T1 para posicao 3",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 500 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[5].Oid );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID );
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para CIMA utilizando a mesma tarefa referencia (T1) e movendo para posições diferentes ( T3 e T6)" +
            "Usuário1: Mover T6 para T1" +
            "Usuário2: Mover T6 para T3" )]
        public void MovimentarParaCimaMesmaTarefaSelecionadaParaPosicoesDiferentesTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t6;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t3.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T6 para posicao 3",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 500 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 20 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[2].Oid );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID );
        }

        [TestMethod]
        [Description( "" )]
        public void MovimentarTarefasParaCimaSimultaneamenteDeTarefasDistintasParaPosicoesDistintasTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t5;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t2.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T6 para posicao 3",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 450 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[0].Oid );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[1].Oid );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID );
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para CIMA utilizando a tarefas distintas e movendo para posições iguais e que semáforos criados não sejam reaproveitados" +
            "Usuário1: Mover T6 para T1" +
            "Usuário2: Mover T3 para T1" )]
        public void MovimentarTarefasDistintasParaCimaParaPosicoesIguaisSemReaproveitarSemaforosCriadosQuandoUsuario1ExecutaPrimeiroQueUsuario2Test()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t3;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T3 para posicao 1",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 300 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[0].Oid, "Deveria ser T3, pois foi movimentada após a primeira movimentação." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois foi movimentada para a primeira posição" );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[1].Oid, "Deveria ser T6, pois foi movimentada e logo depois sofreu reordenação por causa da 2ª movimentação." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois foi movimentada para a primeira posição e logo depois sofreu reordenação por causa da 2ª movimentação" );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[2].Oid, "Deveria ser T1, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[3].Oid, "Deveria ser T2, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[4].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[5].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois sofreu reordenação por causa das movimentações." );

            //elimina sessões
            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para CIMA utilizando a tarefas distintas e movendo para posições iguais e que semáforos criados não sejam reaproveitados" +
            "Usuário1: Mover T6 para T1" +
            "Usuário2: Mover T3 para T1" )]
        public void MovimentarTarefasDistintasParaCimaParaPosicoesIguaisSemReaproveitarSemaforosCriadosQuandoUsuario2ExecutarPrimeiroQueUsuario1Test()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            //debug aqui
            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t3;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T3 para posicao 1",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 5000 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[0].Oid, "Deveria ser T6, pois foi movimentada após a primeira movimentação." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois foi movimentada para a primeira posição" );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[1].Oid, "Deveria ser T3, pois foi movimentada e logo depois sofreu reordenação por causa da 2ª movimentação." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois foi movimentada para a primeira posição e logo depois sofreu reordenação por causa da 2ª movimentação" );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[2].Oid, "Deveria ser T1, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[3].Oid, "Deveria ser T2, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[4].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[5].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois sofreu reordenação por causa das movimentações." );

            //elimina sessões


            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para CIMA utilizando a tarefas distintas e movendo para posições iguais e que semáforos criados sejam reaproveitados" +
            "Usuário1: Mover T6 para T1" +
            "Usuário2: Mover T3 para T1" )]
        public void MovimentarTarefasDistintasParaCimaParaPosicoesIguaisReaproveitandoSemaforosCriadosQuandoUsuario1ExecutarPrimeiroQueUsuario2Test()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t3;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T3 para posicao 1",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 1000 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[0].Oid, "Deveria ser T3, pois foi movimentada após a primeira movimentação." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois foi movimentada para a primeira posição" );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[1].Oid, "Deveria ser T6, pois foi movimentada e logo depois sofreu reordenação por causa da 2ª movimentação." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois foi movimentada para a primeira posição e logo depois sofreu reordenação por causa da 2ª movimentação" );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[2].Oid, "Deveria ser T1, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[3].Oid, "Deveria ser T2, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[4].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[5].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois sofreu reordenação por causa das movimentações." );

            //elimina sessões
            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "Quando ocorrer uma movimentação simultanea para CIMA utilizando a tarefas distintas e movendo para posições iguais e que semáforos criados sejam reaproveitados" +
            "Usuário1: Mover T6 para T1" +
            "Usuário2: Mover T3 para T1" )]
        public void MovimentarTarefasDistintasParaCimaParaPosicoesIguaisReaproveitandoSemaforosCriadosQuandoUsuario2ExecutarPrimeiroQueUsuario1Test()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.tarefaParaMover = t6;
            objetosUsuario2.tarefaParaMover = t3;

            objetosUsuario1.nbIDdestino = t1.NbID;
            objetosUsuario2.nbIDdestino = t1.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T6 para posicao 1",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T3 para posicao 1",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 1000 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[0].Oid, "Deveria ser T6, pois foi movimentada após a primeira movimentação." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois foi movimentada para a primeira posição" );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[1].Oid, "Deveria ser T3, pois foi movimentada e logo depois sofreu reordenação por causa da 2ª movimentação." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois foi movimentada para a primeira posição e logo depois sofreu reordenação por causa da 2ª movimentação" );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[2].Oid, "Deveria ser T1, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[3].Oid, "Deveria ser T2, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[4].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[5].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois sofreu reordenação por causa das movimentações." );

            //elimina sessões
            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "Movimentar simultaneamente para BAIXO tarefas distintas para posições distintas, sem reaproveitar semáforos e a THREAD USUARIO 1 executada primeiro do que a THREAD do USUARIO 2" +
            "Usuário1: Mover T1 para T5" +
            "Usuário2: Mover T2 para T6" )]
        public void MoverTarefasDistintasParaBaixoParaPosicoesDistintasSemReaproveitarSemaforosThread1ExecutaPrimeiroThread2Test()
        {
            #region Contexto
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t1;
            objetosUsuario2.tarefaParaMover = t2;

            objetosUsuario1.nbIDdestino = t5.NbID;
            objetosUsuario2.nbIDdestino = t6.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T1 para posicao 5",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T2 para posicao 6",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 300 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;
            }, 50 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[0].Oid, "Deveria ser T3, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[1].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[2].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[3].Oid, "Deveria ser T1, pois foi movimentada e logo depois sofreu reordenação por causa da 2ª movimentação." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois foi movimentada para a quinta posição e logo depois sofreu reordenação por causa da 2ª movimentação" );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[4].Oid, "Deveria ser T6, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[5].Oid, "Deveria ser T2, pois foi movimentada após a primeira movimentação." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois foi movimentada após a primeira movimentação." );
        }

        [TestMethod]
        [Description( "Erro: de lista vazia"
                       + "Situação: Ocorrer movimentação T2 para T5 e T1 para T4 e a THREAD de T1 para T4 reaproveita o semáforo do T2 para T5 e cria um semáforo de 1-1"
                       + "porém não encontra nenhuma tarefa para ser reordenada pois T1 foi movimentada para a posição 4." )]
        public void MoverTarefasEAsTarefasParaReordenarNaoExistiremEmUmDeterminadoMomentoTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas();
            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaParaMover = t2;
            objetosUsuario2.tarefaParaMover = t1;

            objetosUsuario1.nbIDdestino = t5.NbID;
            objetosUsuario2.nbIDdestino = t4.NbID;

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario1.tarefaParaMover.NbID, objetosUsuario1.nbIDdestino );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson, movendo T2 para posicao 5",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Highest,
                Name = "Thread Gabriel, movendo T1 para posicao 4",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 700 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t3.Oid, resultadoReordenacao[0].Oid, "Deveria ser T3, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[1].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[2].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t1.Oid, resultadoReordenacao[3].Oid, "Deveria ser T1, pois foi movimentada para esta posição." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois foi movimentada para a quarta posição." );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[4].Oid, "Deveria ser T2, pois foi movimentada para esta posição." );
            Assert.AreEqual( short.Parse( "5" ), resultadoReordenacao[4].NbID, "Deveria ter o ID 5, pois foi movimentada para esta posição." );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[5].Oid, "Deveria ser T6, pois não sofreu alteração por causa das movimentações." );
            Assert.AreEqual( short.Parse( "6" ), resultadoReordenacao[5].NbID, "Deveria ter o ID 6, pois não sofreu alteração por causa das movimentações." );

            //elimina sessões
            _contexto2.Dispose();
        }


        [TestMethod]
        [Description( "Cenário: Ocorre exclusão T1 e T3, as duas threads enxergam que as tarefas foram excluidas."
                      + "A thread T1 executa primeiro e reordena todas, quando a thread de t3 executa (sem reaproveitar os semáforos criados) ela não precisa mais reordenar, pois a thread de T1 já reordenou todas." )]
        public void QuandoExcluirT1eT3ASegundaThreadDeveSerAExclusaoDeT3ENaoDeveReordenarPoisNaoPrecisaMaisTest()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoExcluirTarefas();
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoExcluirTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid, Tarefa = new Tarefa() };
            objetosUsuario2.tarefaReferenciaParaReordenacao = t2;

            List<CronogramaTarefa> tarefas = contexto.CronogramaTarefa.ToList();
            tarefas.FirstOrDefault( o => o.NbID == 3 ).CsExcluido = true;
            tarefas.FirstOrDefault( o => o.NbID == 1 ).CsExcluido = true;

            contexto.SaveChanges();

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID, 0 );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t3.NbID, 0 );

            Thread thread1 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 450 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 50 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[0].Oid, "Deveria ser T2, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t4.Oid, resultadoReordenacao[1].Oid, "Deveria ser T4, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t5.Oid, resultadoReordenacao[2].Oid, "Deveria ser T5, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( t6.Oid, resultadoReordenacao[3].Oid, "Deveria ser T6, pois sofreu reordenação por causa das movimentações." );
            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

            //elimina sessões
            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "Cenário: Ocorre exclusão T1 e T3, as duas threads enxergam que as tarefas foram excluidas."
                      + "A thread T1 executa primeiro e reordena todas, quando a thread de t3 executa (reaproveitando os semáforos criados) ela não precisa mais reordenar, pois a thread de T1 já reordenou todas." )]
        public void QuandoExcluirT1eT3ASegundaThreadReaproveitarOsSemaforosCriadosNaoDeveReordenarPoisNaoPrecisaTest()
        {
            #region Contexto
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoExcluirTarefas();
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoExcluirTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid };
            objetosUsuario2.tarefaReferenciaParaReordenacao = t2;

            t1.Cronograma = null;
            t1.Tarefa = null;
            t1.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t1 );

            t3.Cronograma = null;
            t3.Tarefa = null;
            t3.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t3 );

            contexto.SaveChanges();

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t3.NbID );

            Thread thread1 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 600 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );

        }

        [TestMethod]
        [Description( "Cenário: Ocorre exclusão T3 e T1, as duas threads enxergam que as tarefas foram excluidas."
                      + "A thread T3 executa primeiro e reordena algumas, quando a thread de T1 executa (reaproveitando os semáforos criados) ela precisa reordenar até antes da T3, pois a thread de T3 já reordenou todas." )]
        public void QuandoExcluirT3eT1ASegundaThreadNaoDeveReordenarTodasApenasAlgumasTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoExcluirTarefas();
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoExcluirTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid };
            objetosUsuario2.tarefaReferenciaParaReordenacao = t2;

            t1.Cronograma = null;
            t1.Tarefa = null;
            t1.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t1 );

            t3.Cronograma = null;
            t3.Tarefa = null;
            t3.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t3 );

            List<CronogramaTarefa> cronogramaTarefasExcluidas1 = new List<CronogramaTarefa>();
            List<CronogramaTarefa> cronogramaTarefasExcluidas2 = new List<CronogramaTarefa>();
            cronogramaTarefasExcluidas1.Add( t1 );
            cronogramaTarefasExcluidas2.Add( t3 );

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID, 0 );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t3.NbID, 0 );

            objetosUsuario1.tarefasParaReordenar = objetosUsuario1.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas1 ).ToList();
            objetosUsuario2.tarefasParaReordenar = objetosUsuario2.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas2 ).ToList();


            Thread thread1 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 500 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );
        }

        [TestMethod]
        [Description( "Cenário: Ocorre exclusão T3 e T1, as duas threads enxergam que as tarefas foram excluidas."
                      + "A thread T3 executa primeiro e reordena algumas, quando a thread de T1 executa (sem reaproveitar os semáforos criados) ela precisa reordenar até antes da T3, pois a thread de T3 já reordenou todas." )]
        public void QuandoExcluirT3eT1ASegundaThreadReaproveitarOsSemaforosCriadosNaoDeveReordenarTodasApenasAlgumasTest()
        {
            #region Contexto
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoExcluirTarefas();
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoExcluirTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            objetosUsuario1.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid };
            objetosUsuario2.tarefaReferenciaParaReordenacao = t2;

            t1.Cronograma = null;
            t1.Tarefa = null;
            t1.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t1 );

            t3.Cronograma = null;
            t3.Tarefa = null;
            t3.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t3 );

            contexto.SaveChanges();

            List<CronogramaTarefa> cronogramaTarefasExcluidas1 = new List<CronogramaTarefa>();
            List<CronogramaTarefa> cronogramaTarefasExcluidas2 = new List<CronogramaTarefa>();
            cronogramaTarefasExcluidas1.Add( t1 );
            cronogramaTarefasExcluidas2.Add( t3 );

            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID );
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t3.NbID );

            objetosUsuario1.tarefasParaReordenar = objetosUsuario1.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas1 ).ToList();
            objetosUsuario2.tarefasParaReordenar = objetosUsuario2.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas2 ).ToList();


            Thread thread1 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 6000 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "3" ), resultadoReordenacao[2].NbID, "Deveria ter o ID 3, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "4" ), resultadoReordenacao[3].NbID, "Deveria ter o ID 4, pois sofreu reordenação por causa das movimentações." );
        }

        [TestMethod]
        [Description( " Cenário: Ocorre exclusão T1 T3 e T2 T4, as duas threads enxergam a exclusao das tarefas."
                     + "  " )]
        public void QuandoExcluirT1T3eT2T5ASegundaThreadNaoReaproveitaSemaforosDeveReordenarTarefasCorretamenteTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoExcluirTarefas();
            ObjetosReordenarPorBlocoAoExcluirTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoExcluirTarefas();

            objetosUsuario1.txDescricaoTarefa = "Tarefa Anderson";
            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";

            t1.Cronograma = null;
            t1.Tarefa = null;
            t1.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t1 );

            t3.Cronograma = null;
            t3.Tarefa = null;
            t3.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t3 );

            contexto.SaveChanges();

            objetosUsuario1.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid };
            objetosUsuario1.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID );

            t2.Cronograma = null;
            t2.Tarefa = null;
            t2.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t2 );

            t4.Cronograma = null;
            t4.Tarefa = null;
            t4.CsExcluido = true;
            contexto.CronogramaTarefa.Attach( t4 );

            contexto.SaveChanges();

            objetosUsuario2.tarefaReferenciaParaReordenacao = new CronogramaTarefa() { NbID = 0, OidCronograma = cronograma1.Oid };
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t5.NbID );

            List<CronogramaTarefa> cronogramaTarefasExcluidas1 = new List<CronogramaTarefa>();
            List<CronogramaTarefa> cronogramaTarefasExcluidas2 = new List<CronogramaTarefa>();

            cronogramaTarefasExcluidas1.Add( t1 );
            cronogramaTarefasExcluidas1.Add( t3 );

            cronogramaTarefasExcluidas2.Add( t2 );
            cronogramaTarefasExcluidas2.Add( t4 );

            objetosUsuario1.tarefasParaReordenar = objetosUsuario1.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas1 ).ToList();
            objetosUsuario2.tarefasParaReordenar = objetosUsuario2.tarefasParaReordenar.Concat( cronogramaTarefasExcluidas2 ).ToList();

            Thread thread1 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoExcluirTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario2 );
            Thread.Sleep( 5000 );
            thread2.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;

            }, 10 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );

            Assert.AreEqual( short.Parse( "1" ), resultadoReordenacao[0].NbID, "Deveria ter o ID 1, pois sofreu reordenação por causa das movimentações." );

            Assert.AreEqual( short.Parse( "2" ), resultadoReordenacao[1].NbID, "Deveria ter o ID 2, pois sofreu reordenação por causa das movimentações." );
        }

        [TestMethod]
        [Description( "" )]
        public void QuandoMoverECriarTarefasAoMesmoTempoDeveraNaoDuplicarNbIDsNoBancoDeDados()
        {
            #region Contexto
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region Criando tarefa


            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario1 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao();

            objetosUsuario1.txDescricao = "Tarefa Anderson";
            objetosUsuario1.NbIDReferencia = 1;
            objetosUsuario1.oidCronograma = cronograma1.Oid;
            objetosUsuario1.situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid );


            #endregion

            #region Movendo Tarefa


            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";
            objetosUsuario2.tarefaParaMover = t2;
            objetosUsuario2.nbIDdestino = t6.NbID;
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );


            #endregion

            Thread thread1 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 600 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                        thread2.ThreadState == ThreadState.Stopped;

            }, 120 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.AreEqual( t2.Oid, resultadoReordenacao[5].Oid );
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoCriarTarefaPosicao1EMoverUltimaTarefaParaSegunda()
        {
            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region Criando tarefa


            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario1 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao();

            objetosUsuario1.NbIDReferencia = 1;
            objetosUsuario1.oidCronograma = cronograma1.Oid;
            objetosUsuario1.situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid );


            #endregion

            #region Movendo Tarefa


            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";
            objetosUsuario2.tarefaParaMover = t6;
            objetosUsuario2.nbIDdestino = t2.NbID;
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );


            #endregion

            Thread thread1 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread1.Start( objetosUsuario1 );

            Thread.Sleep( 100 );
            thread2.Start( objetosUsuario2 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                        thread2.ThreadState == ThreadState.Stopped;

            }, 120 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );
            Assert.AreEqual( t6.Oid, resultadoReordenacao[1].Oid );
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoCriarTarefaPosicao1EMoverTarefa4ParaUltimaPosicaoTest()
        {
            //Assert.Inconclusive( "Método só passa se debugar linha por linha." );

            #region Contexto
            WexDb _contexto2 = ContextFactoryManager.CriarWexDb();

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //cria colaborador
            Colaborador colaborador2 = ColaboradorFactoryEntity.CriarColaborador( contexto, "gabriel.matos", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas;

            DateTime dataHoraAcao = new DateTime();

            //cria tarefa
            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region Criando tarefa


            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario1 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao();
            objetosUsuario1.txDescricao = "Tarefa Anderson";
            objetosUsuario1.NbIDReferencia = 1;
            objetosUsuario1.oidCronograma = cronograma1.Oid;
            objetosUsuario1.situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid );


            #endregion

            #region Movendo Tarefa


            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas();

            objetosUsuario2.txDescricaoTarefa = "Tarefa Gabriel";
            objetosUsuario2.tarefaParaMover = t4;
            objetosUsuario2.nbIDdestino = t6.NbID;
            objetosUsuario2.tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, objetosUsuario2.tarefaParaMover.NbID, objetosUsuario2.nbIDdestino );


            #endregion

            Thread thread1 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Anderson",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread Gabriel",
            };

            thread2.Start( objetosUsuario2 );
            Thread.Sleep( 100 );
            thread1.Start( objetosUsuario1 );

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                        thread2.ThreadState == ThreadState.Stopped;

            }, 120 );

            List<CronogramaTarefa> resultadoReordenacao = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid );

            Assert.IsNotNull( resultadoReordenacao );
            Assert.AreEqual( t4.Oid, resultadoReordenacao[6].Oid );

            //elimina sessões
            _contexto2.Dispose();
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoHouveremVariasAcoesDeCriacaoDeTarefasNoCronogramaTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region TarefasParaUsarNasThreads

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario1 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N1",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario2 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N2",
                NbIDReferencia = 3,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario3 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N3",
                NbIDReferencia = 3,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario4 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N4",
                NbIDReferencia = 2,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario5 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N5",
                NbIDReferencia = 5,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario6 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N6",
                NbIDReferencia = 8,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario7 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N7",
                NbIDReferencia = 11,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario8 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N8",
                NbIDReferencia = 10,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario9 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N9",
                NbIDReferencia = 7,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario10 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N10",
                NbIDReferencia = 3,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario11 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N11",
                NbIDReferencia = 17,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario12 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N12",
                NbIDReferencia = 15,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario13 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N13",
                NbIDReferencia = 3,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario14 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N14",
                NbIDReferencia = 11,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario15 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N15",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario16 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N16",
                NbIDReferencia = 20,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario17 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N17",
                NbIDReferencia = 15,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario18 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N18",
                NbIDReferencia = 7,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario19 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N19",
                NbIDReferencia = 5,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };
            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario20 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N20",
                NbIDReferencia = 2,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            #endregion

            #region ThreadsParaUsarNoTeste

            Thread thread1 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread3 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread4 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread5 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread6 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread7 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread8 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread9 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread10 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread11 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread12 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread13 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread14 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread15 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread16 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread17 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread18 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread19 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread20 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            #endregion

            thread3.Start( objetosUsuario3 );
            Thread.Sleep( 100 );
            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 100 );
            thread5.Start( objetosUsuario5 );
            Thread.Sleep( 100 );
            thread2.Start( objetosUsuario2 );
            Thread.Sleep( 100 );
            thread4.Start( objetosUsuario4 );
            Thread.Sleep( 100 );
            thread8.Start( objetosUsuario8 );
            Thread.Sleep( 100 );
            thread6.Start( objetosUsuario6 );
            Thread.Sleep( 100 );
            thread7.Start( objetosUsuario7 );
            Thread.Sleep( 100 );
            thread10.Start( objetosUsuario10 );
            Thread.Sleep( 100 );
            thread9.Start( objetosUsuario9 );
            //Thread.Sleep( 100 );
            //thread12.Start( objetosUsuario11 );
            //Thread.Sleep( 100 );
            //thread13.Start( objetosUsuario12);
            //Thread.Sleep( 100 );
            //thread14.Start( objetosUsuario13 );
            //Thread.Sleep( 100 );
            //thread15.Start( objetosUsuario14 );
            //Thread.Sleep( 100 );
            //thread16.Start( objetosUsuario15);
            //Thread.Sleep( 100 );
            //thread17.Start( objetosUsuario16 );
            //Thread.Sleep( 100 );
            //thread18.Start( objetosUsuario17);
            //Thread.Sleep( 100 );
            //thread19.Start( objetosUsuario18);
            //Thread.Sleep( 100 );
            //thread20.Start( objetosUsuario19);

            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                    thread2.ThreadState == ThreadState.Stopped &&
                    thread3.ThreadState == ThreadState.Stopped &&
                    thread4.ThreadState == ThreadState.Stopped &&
                    thread5.ThreadState == ThreadState.Stopped &&
                    thread6.ThreadState == ThreadState.Stopped &&
                    thread7.ThreadState == ThreadState.Stopped &&
                    thread8.ThreadState == ThreadState.Stopped &&
                    thread9.ThreadState == ThreadState.Stopped &&
                    thread10.ThreadState == ThreadState.Stopped &&
                    thread11.ThreadState == ThreadState.Stopped;
                //thread12.ThreadState == ThreadState.Stopped &&
                //thread13.ThreadState == ThreadState.Stopped &&
                //thread14.ThreadState == ThreadState.Stopped &&
                //thread15.ThreadState == ThreadState.Stopped &&
                //thread16.ThreadState == ThreadState.Stopped &&
                //thread17.ThreadState == ThreadState.Stopped &&
                //thread18.ThreadState == ThreadState.Stopped &&
                //thread19.ThreadState == ThreadState.Stopped &&
                //thread20.ThreadState == ThreadState.Stopped;

            }, 120 );

            #region Assert

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            for(int i = 0; i < cronogramaTarefas.Count; i++)
            {
                var validacao = i + 1;
                Assert.AreEqual( validacao, cronogramaTarefas[i].NbID, String.Format( "Esperavasse o nbId {0} e recebeu {1}, problemas de reordenação.", validacao, cronogramaTarefas[i].NbID ) );
            }

            Assert.IsNotNull( cronogramaTarefas );

            #endregion
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoCriarTarefasSimultaneamenteNaPrimeiraPosicaoTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region TarefasParaUsarNasThreads

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario1 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N1",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario2 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N2",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario3 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N3",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario4 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N4",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao objetosUsuario5 = new ObjetosReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao()
            {
                txDescricao = "N5",
                NbIDReferencia = 1,
                oidCronograma = cronograma1.Oid,
                situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( situacaoPlanejamento.Oid )
            };

            #endregion

            #region ThreadsParaUsarNoTeste

            Thread thread1 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread3 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread4 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            Thread thread5 = new Thread( ReordenarPorBlocoAoCriarTarefasDesdeInicioDoCicloDeCriacao )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread",
            };

            #endregion

            thread1.Start( objetosUsuario1 );
            //Thread.Sleep( 200 );
            thread2.Start( objetosUsuario2 );
            //Thread.Sleep( 200 );
            thread3.Start( objetosUsuario3 );
            //Thread.Sleep( 200 );
            thread4.Start( objetosUsuario4 );
            //Thread.Sleep( 200 );
            thread5.Start( objetosUsuario5 );


            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                    thread2.ThreadState == ThreadState.Stopped &&
                    thread3.ThreadState == ThreadState.Stopped &&
                    thread4.ThreadState == ThreadState.Stopped &&
                    thread5.ThreadState == ThreadState.Stopped;
            }, 60 );

            #region Assert

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            for(int i = 0; i < cronogramaTarefas.Count; i++)
            {
                var validacao = i + 1;
                Assert.AreEqual( validacao, cronogramaTarefas[i].NbID, String.Format( "Esperavasse o nbId {0} e recebeu {1}, problemas de reordenação.", validacao, cronogramaTarefas[i].NbID ) );
            }

            #endregion
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoHouveremVariasAcoesDeMovimentacaoDeTarefasNoCronogramaTest()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region TarefasParaUsarNasThreads

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t6.Tarefa.TxDescricao,
                nbIDdestino = 3,
                tarefaParaMover = t6,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, 3, t6.NbID ),
                oidCronograma = cronograma1.Oid
            };

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t5.Tarefa.TxDescricao,
                nbIDdestino = 1,
                tarefaParaMover = t5,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, 1, t5.NbID ),
                oidCronograma = cronograma1.Oid
            };

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario3 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t6.Tarefa.TxDescricao,
                nbIDdestino = 5,
                tarefaParaMover = t6,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, 5, t6.NbID ),
                oidCronograma = cronograma1.Oid
            };

            #endregion

            #region ThreadsParaUsarNoTeste

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 6 para 3",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 5 para 1",
            };

            Thread thread3 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 6 para 5",
            };

            #endregion

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 100 );
            thread2.Start( objetosUsuario2 );
            Thread.Sleep( 100 );
            thread3.Start( objetosUsuario3 );


            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped &&
                       thread3.ThreadState == ThreadState.Stopped;
            }, 120 );

            #region Assert

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            for(int i = 0; i < cronogramaTarefas.Count; i++)
            {
                var validacao = i + 1;
                Assert.AreEqual( validacao, cronogramaTarefas[i].NbID, String.Format( "Esperavasse o nbId {0} e recebeu {1}, problemas de reordenação.", validacao, cronogramaTarefas[i].NbID ) );
            }

            #endregion
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoHouverMovimentacaoTarefa1ParaPosicao4ETarefa4ParaPosicao3Test()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region TarefasParaUsarNasThreads

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t1.Tarefa.TxDescricao,
                nbIDdestino = 4,
                tarefaParaMover = t1,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID, 4 ),
                oidCronograma = cronograma1.Oid
            };

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t4.Tarefa.TxDescricao,
                nbIDdestino = 3,
                tarefaParaMover = t4,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, 3, t4.NbID ),
                oidCronograma = cronograma1.Oid
            };

            #endregion

            #region ThreadsParaUsarNoTeste

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 1 para 4",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 4 para 3",
            };

            #endregion

            thread1.Start( objetosUsuario1 );
            Thread.Sleep( 200 );
            thread2.Start( objetosUsuario2 );


            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;
            }, 120 );

            #region Assert

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            for(int i = 0; i < cronogramaTarefas.Count; i++)
            {
                var validacao = i + 1;
                Assert.AreEqual( validacao, cronogramaTarefas[i].NbID, String.Format( "Esperavasse o nbId {0} e recebeu {1}, problemas de reordenação.", validacao, cronogramaTarefas[i].NbID ) );
            }

            #endregion
        }

        [TestMethod]
        [Description( "" )]
        public void DeveReordenarCorretamenteQuandoHouverMovimentacaoTarefa4ParaPosicao3ETarefa1ParaPosicao4Test()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            #region TarefasParaUsarNasThreads

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario1 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t1.Tarefa.TxDescricao,
                nbIDdestino = 4,
                tarefaParaMover = t1,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, t1.NbID, 4 ),
                oidCronograma = cronograma1.Oid
            };

            ObjetosReordenarPorBlocoAoMoverTarefas objetosUsuario2 = new ObjetosReordenarPorBlocoAoMoverTarefas()
            {
                txDescricaoTarefa = t4.Tarefa.TxDescricao,
                nbIDdestino = 3,
                tarefaParaMover = t4,
                tarefasParaReordenar = CronogramaTarefaDao.ConsultarTarefasImpactadas( cronograma1.Oid, 3, t4.NbID ),
                oidCronograma = cronograma1.Oid
            };

            #endregion

            #region ThreadsParaUsarNoTeste

            Thread thread1 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 1 para 4",
            };

            Thread thread2 = new Thread( ReordenarPorBlocoAoMoverTarefas )
            {
                Priority = ThreadPriority.Normal,
                Name = "Thread 4 para 3",
            };

            #endregion

            thread2.Start( objetosUsuario2 );
            Thread.Sleep( 200 );
            thread1.Start( objetosUsuario1 );


            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return thread1.ThreadState == ThreadState.Stopped &&
                       thread2.ThreadState == ThreadState.Stopped;
            }, 120 );

            #region Assert

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronograma1.Oid, o => o.Tarefa );

            for(int i = 0; i < cronogramaTarefas.Count; i++)
            {
                var validacao = i + 1;
                Assert.AreEqual( validacao, cronogramaTarefas[i].NbID, String.Format( "Esperavasse o nbId {0} e recebeu {1}, problemas de reordenação.", validacao, cronogramaTarefas[i].NbID ) );
            }

            #endregion
        }

        [TestMethod]
        public void DeveCriarTarefaHistoricoEstimativaQuandoUmaTarefaForCriadaNoCronograma()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            List<CronogramaTarefa> cronogramaTarefas = new List<CronogramaTarefa>() { t1, t2, t3, t4, t5, t6 };

            List<Guid> guids = cronogramaTarefas.Select( o => o.OidTarefa ).ToList();

            guids.ForEach( o => Assert.IsTrue( contexto.TarefaHistoricoEstimativa.Any( x => x.OidTarefa == o ) ) );
        }

        [TestMethod]
        public void DeveCriarTarefaHistoricoEstimativaQuandoUmaTarefaForHoraRestanteForEditada()
        {
            #region Contexto

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, DateTime.Now, DateTime.Now.AddDays( 2 ), true );

            //colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;
            DateTime dtInicio = new DateTime();
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;
            DateUtil.CurrentDateTime = dtInicio;

            CronogramaTarefa t1 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t2 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 02", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t3 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 03", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t4 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 04", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t5 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 05", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );
            CronogramaTarefa t6 = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, (string)"Tarefa 06", situacaoPlanejamento, dtInicio, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, (string)"Criar método", 3, 0 );

            #endregion

            TarefaBo.EditarTarefa( t6.Oid.ToString(), t6.Tarefa.TxDescricao, t6.Tarefa.OidSituacaoPlanejamento.Value.ToString(), colaborador1.Usuario.UserName, "", "", t6.Tarefa.NbEstimativaInicial, ConversorTimeSpan.ConverterHoraInteiraParaTimeSpan( 8 ), ConversorTimeSpan.ConverterHoraInteiraParaTimeSpan( 0 ), true, DateTime.Now );

            var obj = TarefaHistoricoEstimativaDao.ConsultarHistoricoEstimativaPorOidTarefaEData( contexto, t6.OidTarefa, DateTime.Now );
            var historicos = TarefaHistoricoEstimativaDao.ConsultarHistoricosEstimativa( contexto ).ToList();
            Assert.AreEqual( 2, contexto.TarefaHistoricoEstimativa.Count( o => o.OidTarefa == t6.OidTarefa ), "Deve ter 2 históricos" );
            Assert.IsNotNull( obj );
        }
    }
}
