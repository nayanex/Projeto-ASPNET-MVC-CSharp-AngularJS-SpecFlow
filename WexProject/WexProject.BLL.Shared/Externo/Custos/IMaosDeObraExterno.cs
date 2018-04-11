using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.Shared.Externo.Custos
{
	/// <summary>
	/// Interface para importar dados de Mão de Obra de fonte externa.
	/// </summary>
	public interface IMaosDeObraExterno
	{
		/// <summary>
		/// Consulta todas as Mãos de Obra relacionadas a um Centro de Custo.
		/// </summary>
		/// <param name="centroCusto">Código do Centro de Custo.</param>
		/// <returns>Lista de Mãos de Obra.</returns>
		List<MaoDeObraDto> ConsultarMaosDeObra(int centroCusto, int ano, int mes);

        /// <summary>
        /// Consulta a ultima versao da importacao realizada pelo Totvs
        /// </summary>
        /// <param name="centroCustoCodigo"></param>
        /// <param name="ano"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        int ConsultarCodigoImportacao(int centroCustoCodigo, int ano, int mes);

	}
}
