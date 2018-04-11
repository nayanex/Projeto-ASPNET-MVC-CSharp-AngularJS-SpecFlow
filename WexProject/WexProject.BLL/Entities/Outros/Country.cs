using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Country
    {
        public Country()
        {
            this.Addresses = new List<Address>();
            this.Pais = new List<Pai>();
        }

        public System.Guid Oid { get; set; }
        public string Name { get; set; }
        public string PhoneCode { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual XPObjectType XPObjectType { get; set; }
        public virtual ICollection<Pai> Pais { get; set; }
    }
}
