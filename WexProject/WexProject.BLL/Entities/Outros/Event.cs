using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Event
    {
        public Event()
        {
            this.Event1 = new List<Event>();
            this.ResourceResources_EventEvents = new List<ResourceResources_EventEvents>();
        }

        public System.Guid Oid { get; set; }
        public string ResourceIds { get; set; }
        public Nullable<System.Guid> RecurrencePattern { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> StartOn { get; set; }
        public Nullable<System.DateTime> EndOn { get; set; }
        public Nullable<bool> AllDay { get; set; }
        public string Location { get; set; }
        public Nullable<int> Label { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> Type { get; set; }
        public string RecurrenceInfoXml { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual ICollection<Event> Event1 { get; set; }
        public virtual Event Event2 { get; set; }
        public virtual ICollection<ResourceResources_EventEvents> ResourceResources_EventEvents { get; set; }
    }
}
