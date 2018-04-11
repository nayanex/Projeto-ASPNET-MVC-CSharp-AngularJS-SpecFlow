using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WexProject.BLL.Entities.Outros
{
    public partial class UserUsers_RoleRoles
    {

        public UserUsers_RoleRoles()
        {
            OID = Guid.NewGuid();
        }
        [ForeignKey("Role")]
        public Nullable<System.Guid> OidRole { get; set; }
        [ForeignKey( "User" )]
        public Nullable<System.Guid> OidUser { get; set; }
        public System.Guid OID { get; set; }
        public Nullable<int> OptimisticLockField { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
