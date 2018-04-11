using System;
using System.Collections.Generic;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;
using WexProject.BLL.Shared.DTOs.Custos;
namespace WexProject.BLL.DAOs.Analise.Custos
{
	public interface IGeralDao
	{
		Dictionary<ProjetoDto, List<CustoRubricaDto>> GetCustosProjetos();
		Dictionary<TipoRubricaDto, List<CustoRubricaDto>> GetCustosRubricas(Guid projetoOid);
	}
}
