using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.Entities.Geral
{
    public partial class Projeto
    {
        public Projeto()
        {
            Oid = Guid.NewGuid();
            NbCicloTotalPlan = 0;
            NbCicloDiasIntervalo = 1;
            NbCicloDuracaoDiasPlan = 10;
            ProjetoClientes = new List<ProjetoCliente>();
        }

        public System.Guid Oid { get; set; }

        public string TxNome { get; set; }
            
		public Nullable<int> TipoProjetoId { get; set; }
        public Nullable<System.Guid> EmpresaInstituicao { get; set; }
        public Nullable<int> NbTamanhoTotal { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<System.DateTime> DtInicioPlan { get; set; }
        public Nullable<System.DateTime> DtInicioReal { get; set; }
        public Nullable<System.DateTime> DtTerminoPlan { get; set; }
        public Nullable<decimal> NbCicloTotalPlan { get; set; }
        public Nullable<decimal> NbCicloDuracaoDiasPlan { get; set; }
        public Nullable<decimal> NbCicloDiasIntervalo { get; set; }
        public Nullable<System.DateTime> oldDate { get; set; }
        public Nullable<System.DateTime> DtTerminoReal { get; set; }
        public Nullable<System.Guid> UltimoFiltro { get; set; }
        public Nullable<decimal> NbRitmoTime { get; set; }
        public decimal NbValor { get; set; }
		public CsProjetoSituacaoDomain CsSituacaoProjeto { get; set; }

        [Column("ProjetoMacroOid"), ForeignKey("ProjetoMacro")]
        public Nullable<System.Guid> ProjetoMacroOid { get; set; }
        public virtual Projeto ProjetoMacro { get; set; }

        [Column("GerenteOid"), ForeignKey("Gerente")]
        public Nullable<System.Guid> GerenteOid { get; set; }
        public virtual Colaborador Gerente { get; set; }

        [Column("CentroCustoId"), ForeignKey("CentroCusto")]
        public Nullable<int> CentroCustoId { get; set; }
        public virtual CentroCusto CentroCusto { get; set; }

        public virtual ICollection<Aditivo> Aditivos { get; set; }
		public virtual TipoProjeto TipoProjeto { get; set; }
        public virtual ICollection<CicloDesenv> CicloDesenvs { get; set; }
        public virtual Colaborador Colaborador { get; set; }
        public virtual EmpresaInstituicao EmpresaInstituicao1 { get; set; }
        public virtual ICollection<Modulo> Moduloes { get; set; }
        public virtual ICollection<ParteInteressada> ParteInteressadas { get; set; }
        public virtual ProjetoUltimoFiltro ProjetoUltimoFiltro { get; set; }
        public virtual ICollection<ProjetoCliente> ProjetoClientes { get; set; }
        public virtual ICollection<ProjetoColaboradorConfig> ProjetoColaboradorConfigs { get; set; }
        public virtual ICollection<ProjetoParteInteressada> ProjetoParteInteressadas { get; set; }
        public virtual ICollection<ProjetoUltimoFiltro> ProjetoUltimoFiltroes { get; set; }

		public virtual ICollection<Projeto> ProjetosMicros { get; set; }
    }
}
