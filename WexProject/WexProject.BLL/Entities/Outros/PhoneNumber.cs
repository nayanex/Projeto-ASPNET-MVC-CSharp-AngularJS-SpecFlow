using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class PhoneNumber
    {
        public System.Guid Oid { get; set; }
        public string Number { get; set; }
        public Nullable<System.Guid> Party { get; set; }
        public string PhoneType { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
        public virtual Party Party1 { get; set; }
    }
}
