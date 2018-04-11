using System;
using System.Collections.Generic;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;

namespace WexProject.BLL.Shared.DTOs.Analise.Custos
{
    public class AnaliseCriticaDto
    {
        public String Nome { get; set; }
        public Decimal CustoTotal { get; set; }
        public ExtratoProjetoDto ExtratoDoProjeto { get; set; }
        public ExtratoFinanceiroDto ExtratoFinanceiro { get; set; }
    }

    public class ExtratoProjetoDto
    {
        public Dictionary<string, List<Object>> Anos { get; set; }
        public OrcamentoDespesas Total { get; set; }
    }

    public class OrcamentoDespesas
    {
        public Decimal OrcamentoAprovadoAdministracao { get; set; }
        public Decimal OrcamentoAprovadoDesenvolvimento { get; set; }
        public Custo DespesasDesenvolvimento { get; set; }
		public Custo DespesasAdministrativas { get; set; }
		public Custo ResultadoMensal { get; set; }
		public Custo Acumulado { get; set; }
    }

    public class ExtratoFinanceiroDto
    {
        public Dictionary<string, List<Object>> Anos { get; set; }
        public DespesasAportes Total { get; set; }
    }

    public class DespesasAportes
    {
        public Decimal AportePlanejado { get; set; }
        public Custo AporteRealizado { get; set; }
        public Custo DespesasReaisAdministrativas { get; set; }
        public Custo DespesasReaisDesenvolvimento { get; set; }
        public Custo Acumulado { get; set; }
    }
}
