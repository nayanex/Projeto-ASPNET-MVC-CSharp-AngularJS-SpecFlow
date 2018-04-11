using System.Collections.Generic;

namespace WexProject.BLL.Shared.DTOs.Custos
{
    public class CustosRubricasDto
    {
        public List<CustoTipoRubricaDto> TiposRubricas { get; set; }
        public CustoValoresDto Total { get; set; }
    }
}