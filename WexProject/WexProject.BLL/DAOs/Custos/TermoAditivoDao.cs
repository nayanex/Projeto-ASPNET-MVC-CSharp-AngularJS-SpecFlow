using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Models;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.DAOs.Custos
{
	public class TermoAditivoDao
	{
		private static TermoAditivoDao instancia;

		public static TermoAditivoDao Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new TermoAditivoDao();
				}

				return instancia;
			}
		}

		private TermoAditivoDao()
		{
		}

		public int SalvarTermoAditivo(TermoAditivo termoAditivo)
		{
			using (var _db = new WexDb())
			{
				if (termoAditivo.TermoAditivoId == 0)
				{
					_db.TermosAditivos.Add(termoAditivo);
				}
				else
				{
					_db.Entry(termoAditivo).State = EntityState.Modified;
				}

				_db.SaveChanges();
			}

			return termoAditivo.TermoAditivoId;
		}

		public List<TermoAditivo> ListarTermoAditivo()
		{
			List<TermoAditivo> termosAditivos;

			using (var _db = new WexDb())
			{
				termosAditivos = _db.TermosAditivos
								.Include("Patrocinador")
								.Include("Projetos")
								.ToList();
			}

			return termosAditivos;
		}

		public TermoAditivo ConsultarTermoAditivoPorId(int termoAditivoId)
		{
			TermoAditivo termoAditivo;

			using (var _db = new WexDb())
			{
				termoAditivo = _db.TermosAditivos
								.Include("Projetos")
								.FirstOrDefault(ta => ta.TermoAditivoId == termoAditivoId);
			}

			return termoAditivo;
		}

		public int ExcluirTermoAditivo(TermoAditivo termoAditivo)
		{
			using (var _db = new WexDb())
			{
				_db.Entry(termoAditivo).State = EntityState.Deleted;

				_db.SaveChanges();
			}

			return termoAditivo.TermoAditivoId;
		}
	}
}
