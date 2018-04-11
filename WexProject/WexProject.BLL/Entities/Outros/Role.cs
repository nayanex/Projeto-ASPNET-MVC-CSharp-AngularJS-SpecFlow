using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Role
    {
        public Role()
        {
            this.UserUsers_RoleRoles = new List<UserUsers_RoleRoles>();
        }

        public System.Guid Oid { get; set; }
        public virtual RoleBase RoleBase { get; set; }
        public virtual ICollection<UserUsers_RoleRoles> UserUsers_RoleRoles { get; set; }
    }
}
