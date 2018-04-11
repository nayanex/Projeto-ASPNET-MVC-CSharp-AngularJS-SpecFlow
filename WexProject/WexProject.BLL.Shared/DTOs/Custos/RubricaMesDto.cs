using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Shared.DTOs.Custos
{
    /*
    * OBS:
    *     Na descrição de despesas por rúbricas na tela de "Custos" => Aba "Despesas Reais" => "Rúbricas"
    *     Encontram-se os seguintes títulos de linha por mês: "Planejado", "Previsões", "Real"
    *     Neste arquivo Dto (RubricaMesDto) os campos equivalentes aos títulos acima são:
    *     "Planejado" == "Planejado"
    *     "Previsões" == "Replanejado"
    *     "Real" == "Gasto"
    */
    public class RubricaMesDto
    {
        public int RubricaMesId { get; set; }
        public int RubricaId { get; set; }
        public String Classe { get; set; }
        public CsMesDomain Mes { get; set; }
        public int Ano { get; set; }
        public Boolean PossuiGastosRelacionados { get; set; }
        public Decimal? Planejado { get; set; }
        public Decimal? Gasto { get; set; }
        public Decimal? Replanejado { get; set; }
    }

    public class RubricaAnoDto
    {
        public int Ano { get; set; }
        public ICollection<RubricaMesDto> Meses { get; set; }
    }
}

