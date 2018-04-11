using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.DAOs.Custos
{
	public class TipoProjetoDao
	{
		/// <summary>
		/// Recupera do Banco de Dados o tipo de Projeto com um determinado Id.
		/// </summary>
		/// <param name="tipoProjetoId">Id do tipo de Projeto a recuperar.</param>
		/// <returns>Tipo de Projeto.</returns>
		public static TipoProjeto GetTipoProjetoPorId(int tipoProjetoId)
		{
			TipoProjeto tipoProjeto;

			using (var _db = new WexDb())
			{
				tipoProjeto = (from tp in _db.TiposProjetos.Include("ClasseProjeto")
							   where tp.TipoProjetoId == tipoProjetoId
							   select tp).First();
			}

			return tipoProjeto;
		}
	}
}
