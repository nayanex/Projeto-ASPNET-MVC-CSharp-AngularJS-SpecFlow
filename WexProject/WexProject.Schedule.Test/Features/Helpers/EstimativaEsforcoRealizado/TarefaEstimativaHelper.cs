using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Test.Features.Helpers.EstimativaEsforcoRealizado
{
    public class TarefaEstimativaHelper
    {
        /// <summary>
        /// Descrição da tarefa
        /// </summary>
        public string Descricao { get; set; }
        /// <summary>
        /// Horas estimadas inicialmente para a tarefa
        /// </summary>
        public short EstimativaInicial { get; set; }

        /// <summary>
        /// Horas restantes (A serem consumidas)
        /// </summary>
        public string Restante { get; set; }

        /// <summary>
        /// Horas já consumidas como esforço
        /// </summary>
        public string Realizado { get; set; }

    }
}
