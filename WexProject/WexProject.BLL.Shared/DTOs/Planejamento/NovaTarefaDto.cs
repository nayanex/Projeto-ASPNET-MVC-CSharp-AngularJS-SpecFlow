using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    /// <summary>
    /// Classe utilizada na deserialização do json para criação de uma nova tarefa
    /// </summary>
    public class NovaTarefaDto
    {
        /// <summary>
        /// Oid do cronograma que contém a nova tarefa
        /// </summary>
        public Guid OidCronograma { get; set; }

        /// <summary>
        /// Descricao da tarefa
        /// </summary>
        public string TxDescricao { get; set; }

        /// <summary>
        /// Oid da situação de planejamento da tarefa
        /// </summary>
        public Guid OidSituacaoPlanejamento { get; set; }

        /// <summary>
        /// Data de inicio de realização da tarefa
        /// </summary>
        public DateTime DtInicio { get; set; }

        /// <summary>
        /// Nome dos responsaveis pela tarefa
        /// </summary>
        public string TxResponsaveis { get; set; }

        /// <summary>
        /// Login do colaborador que está criando a tarefa
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Texto de observação da tarefa
        /// </summary>
        public string TxObservacao { get; set; }

        /// <summary>
        /// Quantidade de estimativa inicial para criação da tarefa
        /// </summary>
        public Int16 NbEstimativaInicial { get; set; }

        /// <summary>
        /// NbId da tarefa em foco quando foi iniciado o processo de criação da tarefa
        /// </summary>
        public Int16 NbIdReferencia { get; set; }
    }
}
