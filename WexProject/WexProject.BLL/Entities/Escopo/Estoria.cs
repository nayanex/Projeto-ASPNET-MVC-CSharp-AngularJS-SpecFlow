using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Qualidade;

namespace WexProject.BLL.Entities.Escopo
{
    public partial class Estoria
    {
        public Estoria()
        {
            Oid = Guid.NewGuid();
        }

        #region Propriedades

        public System.Guid Oid { get; set; }

        public string TxID { get; set; }

        public string TxTitulo { get; set; }

        public Nullable<int> CsTipo { get; set; }

        public string TxGostariaDe { get; set; }

        public string TxEntaoPoderei { get; set; }

        public Nullable<int> CsValorNegocio { get; set; }

        public string TxReferencias { get; set; }

        public string TxDuvidas { get; set; }

        public Nullable<decimal> NbPrioridade { get; set; }

        public Nullable<double> NbTamanho { get; set; }

        public string TxPremissas { get; set; }

        public Nullable<int> CsSituacao { get; set; }

        public Nullable<int> OptimisticLockField { get; set; }

        public Nullable<int> GCRecord { get; set; }

        public string TxAnotacoes { get; set; }

        public Nullable<bool> salvandoPrioridades { get; set; }

        public Nullable<bool> CsEmAnalise { get; set; }

        #endregion

        #region Foreign Keys

        [Column( "Modulo" ), ForeignKey( "Modulo" )]
        public Nullable<System.Guid> OidModulo { get; set; }

        [Column( "ProjetoParteInteressada" ), ForeignKey( "ProjetoParteInteressada" )]
        public Nullable<System.Guid> OidProjetoParteInteressada { get; set; }

        [Column( "ComoUmBeneficiado" ), ForeignKey( "Beneficiado" )]
        public Nullable<System.Guid> OidBeneficiado { get; set; }

        [Column( "EstoriaPai" ), ForeignKey( "EstoriaPai" )]
        public Nullable<System.Guid> OidEstoriaPai { get; set; }

        [Column( "Ciclo" ), ForeignKey( "CicloDesenv" )]
        public Nullable<System.Guid> OidCiclo { get; set; }

        #endregion

        #region Propriedades Navegacionais

        public virtual Beneficiado Beneficiado { get; set; }

        public virtual ICollection<CasoTeste> CasoTestes { get; set; }

        public virtual CicloDesenv CicloDesenv { get; set; }

        public virtual ICollection<CicloDesenvEstoria> CicloDesenvEstorias { get; set; }

        public virtual ICollection<Estoria> Estorias { get; set; }

        public virtual Estoria EstoriaPai { get; set; }

        public virtual Modulo Modulo { get; set; }

        public virtual ProjetoParteInteressada ProjetoParteInteressada { get; set; }

        public virtual ICollection<EstoriaCasoTeste> EstoriaCasoTestes { get; set; }

        #endregion

       
       
       
       
    }
}
