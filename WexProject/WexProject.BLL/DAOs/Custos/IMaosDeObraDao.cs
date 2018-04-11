using System;
using System.Collections.Generic;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.DAOs.Custos
{
	public interface IMaosDeObraDao
	{
		LoteMaoDeObra ConsultarLote(int centroCustoId, int ano, int mes);
		void ConsultarMaosDeObra(int centroCustoId, int rubricaMesId, out LoteMaoDeObra lote, out List<MaoDeObra> maosDeObra);
		int SalvarLote(LoteMaoDeObra lote);
		int SalvarMaoDeObra(MaoDeObra maoDeObra);
	}
}
