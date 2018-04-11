using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.DAOs.Custos
{
	public class ClasseProjetoDao
	{
		/// <summary>
		/// Recupera do Banco de Dados as Classes de Projeto.
		/// </summary>
		/// <returns>Lista de Classes de Projetos.</returns>
		public static List<ClasseProjeto> ListaClassesProjeto()
		{
			List<ClasseProjeto> classesProjeto;

			using (var _db = new WexDb())
			{
				classesProjeto = (from cp in _db.ClassesProjetos
								  select cp).ToList();
			}

			return classesProjeto;
		}

		/// <summary>
		/// Recupera do Banco de Dados a Classe de Projeto com um determinado Id.
		/// </summary>
		/// <param name="classeProjetoId">Id da Classe de Projeto.</param>
		/// <returns>Classe de Projeto.</returns>
		public static ClasseProjeto GetClasseProjetoPorId(int classeProjetoId)
		{
			ClasseProjeto classeProjeto;

			using (var _db = new WexDb())
			{
				classeProjeto = (from cp in _db.ClassesProjetos
								 where cp.ClasseProjetoId == classeProjetoId
								 select cp).First();
			}

			return classeProjeto;
		}

       
	}
}
