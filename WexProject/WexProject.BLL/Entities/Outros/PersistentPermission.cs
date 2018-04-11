using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class PersistentPermission
    {
        public System.Guid Oid { get; set; }
        public string SerializedPermission { get; set; }
        public Nullable<System.Guid> Role { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual RoleBase RoleBase { get; set; }
    }
}
