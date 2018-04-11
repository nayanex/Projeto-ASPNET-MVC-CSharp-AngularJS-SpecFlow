using System;

namespace WexProject.BLL.Shared.DTOs.Custos
{
    public class CustoTipoRubricaDto : CustoValoresDto
    {
        public int TipoRubricaId { get; set; }
        public String Nome { get; set; }
    }
}