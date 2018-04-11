using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class RoleBase
    {
        public RoleBase()
        {
            this.PersistentPermissions = new List<PersistentPermission>();
        }

        public System.Guid Oid { get; set; }
        public string Name { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public virtual ICollection<PersistentPermission> PersistentPermissions { get; set; }
        public virtual Role Role { get; set; }
        public virtual XPObjectType XPObjectType { get; set; }
    }
}
