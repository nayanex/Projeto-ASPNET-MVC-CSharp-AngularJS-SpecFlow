using System;

namespace WexProject.BLL.Shared.DTOs.Custos
{
    public class CustoProjetoDto : CustoValoresDto
    {
        public Guid IdProjeto { get; set; }
        public String NomeProjeto { get; set; }
    }
}