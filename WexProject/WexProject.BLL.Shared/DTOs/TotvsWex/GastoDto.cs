using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.DTOs.TotvsWex
{
    //Objeto que será trafegado pelo serviço REST /gastosrelacionados
    public class NotaFiscalDto
    {
        public int GastoId { get; set; }
        public int CentroDeCustoId { get; set; }
        public int CentroDeCustoCodigo { get; set; }
        public string CentroDeCustoDesc { get; set; }
        public int? RubricaId { get; set; }
        public string Descricao { get; set; }
        public string HistoricoLancamento { get; set; }
        public string Justificativa { get; set; }
        public DateTime? Data { get; set; }
        public Decimal Valor { get; set; }
    }
}
