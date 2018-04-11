using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Party
    {
        public Party()
        {
            this.Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<System.Guid> Address1 { get; set; }
        public Nullable<System.Guid> Address2 { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public Nullable<int> ObjectType { get; set; }
        public virtual Address Address { get; set; }
        public virtual Address Address3 { get; set; }
        public virtual XPObjectType XPObjectType { get; set; }
        public Person Person { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
