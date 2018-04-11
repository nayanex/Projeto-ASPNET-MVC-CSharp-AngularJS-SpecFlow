using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Resource
    {
        public Resource()
        {
            this.ResourceResources_EventEvents = new List<ResourceResources_EventEvents>();
        }

        public System.Guid Oid { get; set; }
        public Nullable<int> Color { get; set; }
        public string Caption { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<ResourceResources_EventEvents> ResourceResources_EventEvents { get; set; }
    }
}
