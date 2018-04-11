using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Address
    {
        public Address()
        {
            this.Parties = new List<Party>();
            this.Parties1 = new List<Party>();
        }

        public System.Guid Oid { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string ZipPostal { get; set; }
        public Nullable<System.Guid> Country { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual Country Country1 { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
        public virtual ICollection<Party> Parties1 { get; set; }
    }
}
