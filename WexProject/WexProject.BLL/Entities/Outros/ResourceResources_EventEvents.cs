using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class ResourceResources_EventEvents
    {
        public Nullable<System.Guid> Events { get; set; }
        public Nullable<System.Guid> Resources { get; set; }
        public System.Guid OID { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual Event Event { get; set; }
        public virtual Resource Resource { get; set; }
    }
}
