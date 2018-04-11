using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
	/// <summary>
	/// Dto para a criação de uma nova tarefa
	/// </summary>
	public class TarefaCriacaoDto
	{
		#region Propriedades

		/// <summary>
		/// Oid do cronograma
		/// </summary>
		public Guid OidCronograma { get; set; }

		/// <summary>
		/// Login do usuário que está criando a tarefa
		/// </summary>
		public string AutorCriacao { get; set; }

		/// <summary>
		/// Descrição da tarefa
		/// </summary>
		public string TxDescricaoTarefa { get; set; }

		/// <summary>
		/// Estimativa inicial
		/// </summary>
		public Int16 NbEstimativaInicial { get; set; }

		/// <summary>
		/// Observações da tarefa
		/// </summary>
		public string TxObservacaoTarefa { get; set; }

		public DateTime DtInicio { get; set; }

		public string TxResponsaveis { get; set; }

		public string OidSituacaoPlanejamentoTarefa { get; set; }

		public short NbIdReferencia { get; set; }
		#endregion
	}
}
