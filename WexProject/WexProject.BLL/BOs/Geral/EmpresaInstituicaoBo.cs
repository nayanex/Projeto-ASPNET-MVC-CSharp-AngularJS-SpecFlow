using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Extensions.Custos;

namespace WexProject.BLL.BOs.Geral
{
	public class EmpresaInstituicaoBo
	{
		private static EmpresaInstituicaoBo instancia;

		public static EmpresaInstituicaoBo Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new EmpresaInstituicaoBo();
				}

				return instancia;
			}
		}

		private EmpresaInstituicaoBo()
		{
		}

		/// <summary>
		/// Lista todas Empresas/Instituições.
		/// </summary>
		/// <returns>Lista de DTOs de EmpresaInstituição.</returns>
		public List<EmpresaInstituicaoDto> ListarEmpresasInstituicoes()
		{
			var empresasInstituicoesDto = (from ei in EmpresaInstituicaoDAO.Instancia.ListarEmpresasInstituicoes()
										   select ei.ToDto())
										   .ToList();

			return empresasInstituicoesDto;
		}
	}
}
