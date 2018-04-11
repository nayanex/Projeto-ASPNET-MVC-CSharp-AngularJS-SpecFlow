using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Entities.Generico
{
    public abstract class Entidade
    {
        public Guid Oid { get; set; }

        public Entidade()
        {
            Oid = Guid.NewGuid();
        }
    }
}
