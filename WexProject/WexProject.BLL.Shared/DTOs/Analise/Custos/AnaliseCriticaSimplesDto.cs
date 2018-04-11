using System;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos
{
    public class AnaliseCriticaSimplesDto
    {
        public Guid Oid { get; set; }
        public String Nome { get; set; }
        public Decimal Projecao { get; set; }
		public Decimal FluxoCaixa { get; set; }
        public int TempoPlanejado { get; set; }
        public int TempoConsumido { get; set; }
        public Decimal OrcamentoPrevisto { get; set; }
        public Decimal OrcamentoConsumido { get; set; }
        public String SituacaoFinanceira { get; set; }
        public String Status { get; set; }
        public double Porcentagem { get; set; }
    }
}
