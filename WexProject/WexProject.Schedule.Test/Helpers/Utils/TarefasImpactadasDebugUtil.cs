﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Diagnostics;

namespace WexProject.Schedule.Test.Helpers.Utils
{
    public class TarefasImpactadasDebugUtil
    {
        /// <summary>
        /// Método responsável por exibir em Debug o resultado da movimentação
        /// </summary>
        /// <param name="retornoMovimentacao"></param>
        public static void ExibirLogTarefaMovida( TarefasMovidasDto retornoMovimentacao, List<CronogramaTarefaDto> tarefas, string alias = "" )
        {
            Debug.WriteLine( string.Format( "\n***** Resultado Movimentação {0} *****\n", alias ) );
            Debug.WriteLine( "Data e hora ação:{0}", retornoMovimentacao.DataHoraAcao.TimeOfDay );
            Debug.WriteLine( "Oid Tarefa Movida: {0}", retornoMovimentacao.OidCronogramaTarefaMovida );
            CronogramaTarefaDto tarefa = tarefas.FirstOrDefault( o => o.OidCronogramaTarefa == retornoMovimentacao.OidCronogramaTarefaMovida );
            if(tarefa != null)
                Debug.WriteLine( "\nTarefa {0} movida para posição {1}", tarefa.TxDescricaoTarefa, retornoMovimentacao.NbIDTarefaMovida );
            else
                Debug.WriteLine( "A tarefa correspondente ao Oid:{0} não foi encontrada na lista!" );
            ExibirLogTarefasImpactadas( retornoMovimentacao.TarefasImpactadas, tarefas, retornoMovimentacao.DataHoraAcao );
            Debug.WriteLine( "\n*******************************************" );
        }

        /// <summary>
        /// Método responsável por exibir um log na janela de output sobre as tarefas que estão sendo solicitadas  atualização
        /// </summary>
        /// <param name="impactadas"></param>
        /// <param name="tarefas"></param>
        /// <param name="horaAcao"></param>
        public static void ExibirLogTarefasImpactadas( Dictionary<string, short> impactadas, List<CronogramaTarefaDto> tarefas, DateTime horaAcao )
        {
            Debug.WriteLine( string.Format( "\nTarefas Impactadas ({0}):", horaAcao.TimeOfDay ) );
            Dictionary<Guid, CronogramaTarefaDto> dicionarioImpactadas = tarefas.Where( o => impactadas.Keys.Contains( o.OidCronogramaTarefa.ToString() ) ).ToDictionary( o => o.OidCronogramaTarefa, o => o );
            if(dicionarioImpactadas.Count > 0)
            {
                foreach(var item in dicionarioImpactadas)
                {
                    Debug.WriteLine( "\t{0} - ID Atual:{1} - ID Novo:{2}", item.Value.TxDescricaoTarefa, item.Value.NbID, impactadas[item.Key.ToString()] );
                }
            }
            else
            {
                Debug.WriteLine( "Não houveram tarefas impactadas" );
            }
        }

        public static void ExibirLogAtualizacaoTarefasImpactadas( Dictionary<string, short> tarefasImpactadas, List<CronogramaTarefaDto> tarefas )
        {
            Debug.WriteLine( "\t Tarefas Atualizadas:" );
            if(tarefas == null)
                return;
            tarefasImpactadas = new Dictionary<string, short>( tarefasImpactadas.OrderBy( o => o.Value ).ToDictionary( o => o.Key, o => o.Value ) );
            CronogramaTarefaDto tarefa;
            foreach(var item in tarefasImpactadas)
            {
                tarefa = tarefas.FirstOrDefault( o => o.OidCronogramaTarefa.ToString() == item.Key );
                if(tarefa != null)
                {
                    Debug.WriteLine( string.Format( "\t{0} - ID {1} -> ID {2} : Oid:{3}", tarefa.TxDescricaoTarefa, tarefa.NbID, item.Value, item.Key ) );
                }
            }
        }

        public static void ExibirOrdemTarefas( List<CronogramaTarefaDto> tarefas, string alias = "" )
        {
            if(string.IsNullOrEmpty( alias ))
                alias = "Ordem atual:";
            Debug.WriteLine( string.Format( "******* {0} ********", alias ) );
            if(tarefas != null && tarefas.Count > 0)
            {
                foreach(var item in tarefas)
                {
                    Debug.WriteLine( "{0} - ID:{1} | Oid:{2}", item.TxDescricaoTarefa, item.NbID, item.OidCronogramaTarefa );
                }
            }
            else
                Debug.WriteLine( "Não há tarefas na lista" );
            Debug.WriteLine( "*******************" );
        }

    }
}
