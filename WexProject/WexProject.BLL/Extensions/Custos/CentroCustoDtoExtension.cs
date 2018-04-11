using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;

namespace WexProject.BLL.Extensions.Custos
{
    internal static class CentroCustoDtoExtension
    {
        public static CentroCusto FromDto (this CentroCustoDto centroCustoDto)
        {
            return new CentroCusto()
            {
                CentroCustoId = centroCustoDto.IdCentroCusto,
                Codigo = centroCustoDto.Codigo,
                Nome = centroCustoDto.Nome
            };
        }

        public static CentroCustoDto ToDto (this CentroCusto centroCusto) 
        {
            return new CentroCustoDto()
            {
                IdCentroCusto = centroCusto.CentroCustoId,
                Codigo = centroCusto.Codigo,
                Nome = centroCusto.Nome
            };
        }
    }
}
