using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class ModuleInfo
    {
        public int ID { get; set; }
        public string Version { get; set; }
        public string Name { get; set; }
        public string AssemblyFileName { get; set; }
        public Nullable<bool> IsMain { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
    }
}
