using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.RH
{
    public partial class ModalidadeFeria
    {
        public ModalidadeFeria()
        {
            this.FeriasPlanejamentoes = new List<FeriasPlanejamento>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<byte> NbDias { get; set; }
        public Nullable<bool> PodeVender { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> CsSituacao { get; set; }
        public virtual ICollection<FeriasPlanejamento> FeriasPlanejamentoes { get; set; }
    }
}
