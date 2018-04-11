using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.NovosNegocios;

namespace WexProject.BLL.Entities.RH
{
    public partial class ColaboradorUltimoFiltro
    {
        #region Construtor

        public ColaboradorUltimoFiltro()
        {
            Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public System.Guid Oid { get; set; }

        public Nullable<System.Guid> OidLastSituacaoFilterSeot { get; set; }

        public Nullable<System.Guid> OidLastUsuarioFilterSeot { get; set; }

        public Nullable<int> LastPeriodoFilterPlanejamentoFerias { get; set; }

        public Nullable<int> LastSituacaoFilterPlanejamentoFerias { get; set; }

        public Nullable<int> OptimisticLockField { get; set; }

        public string LastSuperiorImediatoFilterPlanejamentoFerias { get; set; }

        public string LastSituacaoFeriasFilterPlanejamentoFerias { get; set; }

        [ForeignKey( "EmpresaInstituicao" )]
        public Nullable<System.Guid> OidLastEmpresaInstituicaoSEOT { get; set; }

        [ForeignKey( "TipoSolicitacao" )]
        public Nullable<System.Guid> OidLastTipoSolicitacaoSEOT { get; set; }

        public Nullable<System.Guid> OidUltimoProjetoSelecionado { get; set; }

        #endregion

        
        public virtual ICollection<Colaborador> Colaboradors { get; set; }
        public virtual EmpresaInstituicao EmpresaInstituicao { get; set; }
        public virtual TipoSolicitacao TipoSolicitacao { get; set; }
    }
}
