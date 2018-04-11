using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WexProject.BLL.Models.Custos
{
    public class NotaFiscal
    {
        [Key]
        public int Id { get; set; }

        public DateTime? Data { get; set; }
        public string Lote { get; set; }
        public string SubLote { get; set; }
        public string Documento { get; set; }
        public string Linha { get; set; }
        public string Descricao { get; set; }
        public string HistoricoLancamento { get; set; }
        public string Justificativa { get; set; }
        public int ChaveImportacao { get; set; }
        public Decimal Valor { get; set; }

        [ForeignKey("CentroCusto")]
        public int CentroDeCustoId { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        [ForeignKey("Rubrica")]
        public int? RubricaId { get; set; }
        public virtual Rubrica Rubrica { get; set; }
    }
}