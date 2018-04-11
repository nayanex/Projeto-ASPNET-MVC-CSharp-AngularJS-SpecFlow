using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.NovosNegocios;
using WexProject.BLL.Entities.Qualidade;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.Entities.Outros
{
    public partial class User
    {
        #region Construtor

        public User()
        {
            this.Oid = Guid.NewGuid();
        }

        #endregion

        #region Propriedades

        public System.Guid Oid { get; set; }
        public string StoredPassword { get; set; }
        public string UserName { get; set; }
        public Nullable<bool> ChangePasswordOnFirstLogon { get; set; }
        public Nullable<bool> IsActive { get; set; }

        #endregion

        #region Relacionamentos e ForeignKeys

        public ICollection<CasoTeste> CasoTestes { get; set; }
        public Person Person { get; set; }
        public ICollection<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
        public ICollection<UserUsers_RoleRoles> UserUsers_RoleRoles { get; set; }
        public ICollection<Colaborador> Colaboradors { get; set; }

        #endregion
    }
}
