using System;
using System.Collections.Generic;

namespace WexProject.BLL.Entities.Outros
{
    public partial class Person
    {
        public Person()
        {
            this.Oid = Guid.NewGuid();
        }

        public System.Guid Oid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Email { get; set; }
        public Party Party { get; set; }
        public User User { get; set; }
    }
}
