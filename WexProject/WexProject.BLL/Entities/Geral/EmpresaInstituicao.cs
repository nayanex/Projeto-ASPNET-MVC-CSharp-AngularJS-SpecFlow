using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.NovosNegocios;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Geral
{
    public partial class EmpresaInstituicao
    {
        public EmpresaInstituicao()
        {
            this.ColaboradorUltimoFiltroes = new List<ColaboradorUltimoFiltro>();
            this.ParteInteressadas = new List<ParteInteressada>();
            this.Projetoes = new List<Projeto>();
            this.ProjetoClientes = new List<ProjetoCliente>();
            this.SolicitacaoOrcamentoes = new List<SolicitacaoOrcamento>();
            this.Solicitantes = new List<Solicitante>();
        }

        public System.Guid Oid { get; set; }
        public string TxNome { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public string TxSigla { get; set; }
        public string TxEmail { get; set; }
        public string TxFoneFax { get; set; }
        public Nullable<System.Guid> Pais { get; set; }
        public virtual ICollection<ColaboradorUltimoFiltro> ColaboradorUltimoFiltroes { get; set; }
        public virtual Pai Pai { get; set; }
        public virtual ICollection<ParteInteressada> ParteInteressadas { get; set; }
        public virtual ICollection<Projeto> Projetoes { get; set; }
        public virtual ICollection<ProjetoCliente> ProjetoClientes { get; set; }
        public virtual ICollection<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
        public virtual ICollection<Solicitante> Solicitantes { get; set; }
    }
}
