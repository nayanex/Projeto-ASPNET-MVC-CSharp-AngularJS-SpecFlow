using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Libs.GerenciadorComandos;
using System.ComponentModel;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Test;
using System.Threading;
using Moq;
using Moq.Protected;
using WexProject.Schedule.Library.Libs.CrontroleMovimentacao;
using WexProject.Schedule.Library.Helpers;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class GerenciadorComandosTest
    {
        /// <summary>
        /// Constantes máxima de tarefas a serem criadas
        /// </summary>
        private const int QUANTIDADES_TAREFAS = 10;

        private delegate int ExecucaoAssincronaHandler();
        /// <summary>
        /// Contador de NbIds
        /// </summary>
        private Int16 nbIdContador;
        /// <summary>
        /// Oid do cronograma
        /// </summary>
        private Guid oidCronograma;

        /// <summary>
        /// Gerenciador de comandos para teste
        /// </summary>
        private GerenciadorComandos gerenciador;

        /// <summary>
        /// lista de tarefas utilizadas no gerenciador de comandos
        /// </summary>
        private BindingList<CronogramaTarefaGridItem> tarefas;

        [TestInitialize]
        public void Inicializar() 
        {
            nbIdContador = 0;
            oidCronograma = Guid.NewGuid();
            tarefas = new BindingList<CronogramaTarefaGridItem>();
            gerenciador = new GerenciadorComandos( tarefas );
            CriarTarefas();
        }

        /// <summary>
        /// Método responsável por criar as tarefas
        /// </summary>
        private void CriarTarefas() 
        {
            for(int i = 0; i < QUANTIDADES_TAREFAS; i++)
            {
                tarefas.Add( CriarTarefaDtoPadrao() );
            }
        }

        /// <summary>
        /// Método responsável por criar uma tarefa dto padrão
        /// </summary>
        /// <returns></returns>
        public CronogramaTarefaGridItem CriarTarefaDtoPadrao() 
        {
            nbIdContador++;
            Random aleatorio = new Random();
            int min, max;

            min = aleatorio.Next( 0, 5 );
            max = aleatorio.Next( 6, 10 );
            
            while(( 2 * min ) >= max) 
            {
                min = aleatorio.Next(0,5);
            }
            CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem() 
            {
                OidCronograma = oidCronograma,
                NbEstimativaInicial = (Int16)max,
                NbEstimativaRestante = min.ToHoursTimeSpan().Ticks,
                NbID = nbIdContador,
                NbRealizado =  max.ToHoursTimeSpan().Ticks - min.ToHoursTimeSpan().Ticks,
                OidCronogramaTarefa = Guid.NewGuid(),
                OidTarefa = Guid.NewGuid(),
                TxAtualizadoPor = "Gabriel Matos",
                TxDescricaoTarefa = string.Format( "Tarefa {0}",nbIdContador ),
                DtInicio = DateTime.Now,
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now.AddHours(1),
                DtHoraConsulta = DateTime.Now
            };
            return tarefa;
        }

        public CronogramaTarefaGridItem ClonarTarefa(CronogramaTarefaGridItem tarefa) 
        {
            CronogramaTarefaGridItem clone = new CronogramaTarefaGridItem()
            {
                OidCronograma = oidCronograma,
                NbEstimativaInicial = tarefa.NbEstimativaInicial,
                NbEstimativaRestante = tarefa.NbEstimativaRestante,
                NbID = tarefa.NbID,
                NbRealizado = tarefa.NbRealizado,
                OidCronogramaTarefa = tarefa.OidCronogramaTarefa,
                OidTarefa = tarefa.OidTarefa,
                TxAtualizadoPor = "Gabriel Matos",
                TxDescricaoTarefa = tarefa.TxDescricaoTarefa,
                DtInicio = tarefa.DtInicio,
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = tarefa.DtAtualizadoEm
            };

            return clone;
        }
        /// <summary>
        /// Método responsável por criar horas 
        /// </summary>
        /// <param name="horas"> quantidade de horas</param>
        /// <param name="minutos">quantidade de minutos</param>
        /// <param name="segundos"quantidade de segundos></param>
        /// <returns></returns>
        public static TimeSpan CriarTimeSpan(int horas,int minutos = 0,int segundos = 0) 
        {
            if(horas < 1)
                horas = 3;
            return new TimeSpan( horas, minutos, segundos );
        }

        [TestMethod]
        public void DeveManterAsOrdensDeAtualizacaoDasTarefasImpactadas()
        {
            /*
              tarefa 1 movida para posicao 7   * tarefa 2 movida para posicao 9
             * Impactadas                      *Impactadas
             * * * * * *                       * * * * * *
             * 2 -> 1  *                       * 3 -> 1  *
             * 3 -> 2  *                       * 4 -> 2  *
             * 4 -> 3  *                       * 5 -> 3  *
             * 5 -> 4  *                       * 6 -> 4  *
             * 6 -> 5  *                       * 7 -> 5  *
             * 7 -> 6  *                       * 1 -> 6  *
             * 1 -> 7  *                       * 8 -> 7  *
             * * * * * *                       * 9 -> 8  *
             * 8 -> 8                          * 2 -> 9  *
             * 9 -> 9                          * * * * * *
             * 10 -> 10                        * 10 -> 10
             */

            Mock<GerenciadorComandos> gerenciadorMock = new Mock<GerenciadorComandos>( tarefas ) { CallBase = true };
            gerenciadorMock.Setup( o => o.CriarTarefaMovida( It.IsAny<Guid>(), It.IsAny<Int16>(),
                It.IsAny<Int16>(), It.IsAny<Dictionary<string, short>>() ) ).Callback( () =>
                {
                    if(Thread.CurrentThread.Name == "execucao1" ) 
                    {
                        Thread.Sleep( 400 );
                    }
                } ).Returns<Guid,Int16,Int16,Dictionary<string, short>>( (oidCronogramaTarefaMovida,posicaoInicial,posicaoFinal,tarefasImpactadas) => {

                    TarefaMovida t = new TarefaMovida( oidCronogramaTarefaMovida, posicaoInicial, posicaoFinal, tarefasImpactadas );
                    return t;
                } );
            gerenciador = gerenciadorMock.Object;

            CronogramaTarefaGridItem tarefa1 = tarefas[0];
            CronogramaTarefaGridItem tarefa2 = tarefas[1];
            DateTime dataAtualizacao1 = DateTime.Now;
            DateTime dataAtualizacao2 = DateTime.Now.AddSeconds(1);
            CronogramaTarefaGridItem tarefaFinal1 = tarefas[6];
            CronogramaTarefaGridItem tarefaFinal2 = tarefas[8];
            Dictionary<string, short> impactadas1 = new Dictionary<string, short>();
            Dictionary<string, short> impactadas2 = new Dictionary<string, short>();
            List<CronogramaTarefaGridItem> copiaListaOriginal = new List<CronogramaTarefaGridItem>( tarefas.Select(o=> ClonarTarefa(o)) );
            List<CronogramaTarefaGridItem> tarefasOrdemEsperada = new List<CronogramaTarefaGridItem>( tarefas.Select( o => ClonarTarefa( o ) ) );
            tarefasOrdemEsperada[0].NbID = 6;
            tarefasOrdemEsperada[1].NbID = 9;
            tarefasOrdemEsperada[2].NbID = 1;
            tarefasOrdemEsperada[3].NbID = 2;
            tarefasOrdemEsperada[4].NbID = 3;
            tarefasOrdemEsperada[5].NbID = 4;
            tarefasOrdemEsperada[6].NbID = 5;
            tarefasOrdemEsperada[7].NbID = 7;
            tarefasOrdemEsperada[8].NbID = 8;
            tarefasOrdemEsperada[9].NbID = 10;

            // tarefa 1 movida para posicao 7
            impactadas1.Add( tarefas[1].OidCronogramaTarefa.ToString(), (short)tarefas[0].NbID );
            impactadas1.Add( tarefas[2].OidCronogramaTarefa.ToString(), (short)tarefas[1].NbID );
            impactadas1.Add( tarefas[3].OidCronogramaTarefa.ToString(), (short)tarefas[2].NbID );
            impactadas1.Add( tarefas[4].OidCronogramaTarefa.ToString(), (short)tarefas[3].NbID );
            impactadas1.Add( tarefas[5].OidCronogramaTarefa.ToString(), (short)tarefas[4].NbID );
            impactadas1.Add( tarefas[6].OidCronogramaTarefa.ToString(), (short)tarefas[5].NbID );
            impactadas1.Add( tarefas[0].OidCronogramaTarefa.ToString(), (short)tarefas[6].NbID );

            //tarefa 2 movida para posicao 9
            impactadas2.Add( tarefas[2].OidCronogramaTarefa.ToString(), (short)tarefas[0].NbID );
            impactadas2.Add( tarefas[3].OidCronogramaTarefa.ToString(), (short)tarefas[1].NbID );
            impactadas2.Add( tarefas[4].OidCronogramaTarefa.ToString(), (short)tarefas[2].NbID );
            impactadas2.Add( tarefas[5].OidCronogramaTarefa.ToString(), (short)tarefas[3].NbID );
            impactadas2.Add( tarefas[6].OidCronogramaTarefa.ToString(), (short)tarefas[4].NbID );
            impactadas2.Add( tarefas[0].OidCronogramaTarefa.ToString(), (short)tarefas[5].NbID );
            impactadas2.Add( tarefas[7].OidCronogramaTarefa.ToString(), (short)tarefas[6].NbID );
            impactadas2.Add( tarefas[8].OidCronogramaTarefa.ToString(), (short)tarefas[7].NbID );
            impactadas2.Add( tarefas[1].OidCronogramaTarefa.ToString(), (short)tarefas[8].NbID );

            Thread t1 = new Thread( () =>
            {
                gerenciador.CriarComandoMovimentarTarefa( tarefa1.OidCronogramaTarefa, (short)tarefa1.NbID, (short)tarefaFinal1.NbID, impactadas1, dataAtualizacao1 );
            } ) { Name = "execucao1" };

            Thread t2 = new Thread( () =>
            {
                gerenciador.CriarComandoMovimentarTarefa( tarefa2.OidCronogramaTarefa, (short)tarefa1.NbID, (short)tarefaFinal2.NbID, impactadas2, dataAtualizacao2 );
            } ) { Name = "execucao2" };
            t1.Start();
            Thread.Sleep( 100 );
            t2.Start();
            gerenciador.CriarComandoMovimentarTarefa( tarefa1.OidCronogramaTarefa, (short)tarefa1.NbID, (short)tarefaFinal1.NbID, impactadas1, dataAtualizacao1 );
            gerenciador.CriarComandoMovimentarTarefa( tarefa2.OidCronogramaTarefa, (short)tarefa2.NbID, (short)tarefaFinal2.NbID, impactadas2, dataAtualizacao2 );
           
            Dictionary<string, short> ordenacaoEsperada = tarefasOrdemEsperada.ToDictionary( o => o.OidCronogramaTarefa.ToString(), o => (short)o.NbID );

            gerenciador.ExecutarComandosPendentes();
            Dictionary<string, short> ordenacaoAtualTarefas = tarefas.ToDictionary( o => o.OidCronogramaTarefa.ToString(), o => (short)o.NbID );
            int contagemNBids = ordenacaoAtualTarefas.Select( o => o.Value ).Distinct().Count();
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return contagemNBids == tarefas.Count && (t1.ThreadState == ThreadState.Stopped && t2.ThreadState == ThreadState.Stopped);
            }, 3 );
            Assert.AreEqual( tarefas.Count, contagemNBids ,"Não deveria possuir nenhum numero repetido");
            CollectionAssert.AreEquivalent(ordenacaoEsperada,ordenacaoAtualTarefas,"As tarefas deveriam se encontrar na ordem esperada");
        }

        [TestMethod]
        public void TestarGerenciadorDeComandos() 
        {
            DateTime data = DateTime.Now.AddSeconds(2);
            List<CronogramaTarefaGridItem> tarefasAtualizadas = new List<CronogramaTarefaGridItem>(tarefas);
            tarefasAtualizadas.ForEach( o => gerenciador.GerenciadorTarefasImpactadas.AplicarDataAtualizacao( o.OidCronogramaTarefa, o.DtHoraConsulta ) );
            tarefasAtualizadas.ForEach(o=>o.DtHoraConsulta = data);
            tarefasAtualizadas = new List<CronogramaTarefaGridItem>( tarefasAtualizadas.Take( 5 ) );
            gerenciador.CriarComandoAtualizarTarefas( tarefasAtualizadas );
            DateTime dataEsperada = DateTime.Now.AddSeconds(3);
            tarefasAtualizadas[0].DtHoraConsulta = dataEsperada;
            string oidCronogramaTarefaAtualizada = tarefasAtualizadas[0].OidCronogramaTarefa.ToString();
            gerenciador.GerenciadorTarefasImpactadas.AplicarDataAtualizacao( tarefasAtualizadas[0].OidCronogramaTarefa, tarefasAtualizadas[0].DtHoraConsulta );
            gerenciador.ExecutarComandosPendentes();
            DateTime dataFinalAtualizada = gerenciador.GerenciadorTarefasImpactadas.TarefasAtualizadas[oidCronogramaTarefaAtualizada];
            Assert.AreEqual(dataEsperada,dataFinalAtualizada,"Deveria ser a mesma data");


        }
    }
}
