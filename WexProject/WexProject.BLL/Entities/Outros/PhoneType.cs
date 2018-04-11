using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class PhoneType
    {
        public System.Guid Oid { get; set; }
        public string TypeName { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public Nullable<int> GCRecord { get; set; }
    }
}
