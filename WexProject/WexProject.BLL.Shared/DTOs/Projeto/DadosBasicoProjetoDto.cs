using System;
using System.Collections.Generic;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Rh;

namespace WexProject.BLL.Shared.DTOs.Projeto
{
    public class DadosBasicoProjetoDto
    {
        public Guid IdProjeto { get; set; }
        public string Nome { get; set; }
        public DateTime? InicioPlanejado { get; set; }
        public DateTime? InicioReal { get; set; }
        public DateTime? TerminoReal { get; set; }
        public ColaboradorDto Gerente { get; set; }
        public List<ClienteDto> Clientes { get; set; }
        public DadosBasicoProjetoDto ProjetoMacro { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
        public CsProjetoSituacaoDomain Situacao { get; set; }
        public bool HasProjetosMicros { get; set; }
        public bool IsMacro { get; set; }
    }
}