using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.BOs.Custos
{
	public class ClasseProjetoBo
	{
		public static List<ClasseProjetoDto> ListaClassesProjeto()
		{
			var classesProjeto = ClasseProjetoDao.ListaClassesProjeto();

			var classesProjetoDto = (from cp in classesProjeto
									 select new ClasseProjetoDto()
									 {
										 Classe = cp.ClasseProjetoId,
										 Nome = cp.TxNome
									 }).ToList();

			return classesProjetoDto;
		}
	}
}
