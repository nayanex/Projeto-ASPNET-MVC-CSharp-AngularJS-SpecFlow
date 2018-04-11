using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Schedule.Library.Domains
{
	/// <summary>
	/// Classe responsável por identificar os tipos de retorno referentes as ações que ocorreram após ocorrer uma alteração de uma Situação Planejamento 
	/// </summary>
	public enum CsSituacaoPlanejamentoTipoRetorno
	{
		ConsumiuHoras,
		NaoConsumiuHoras,
		SituacaoPlanejamentoRecusada
	}
}
