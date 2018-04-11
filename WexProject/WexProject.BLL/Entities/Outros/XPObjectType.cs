using System;
using System.Collections.Generic;
using WexProject.BLL.Entities.Geral;

namespace WexProject.BLL.Entities.Outros
{
    public partial class XPObjectType
    {
        public XPObjectType()
        {
            this.ConfiguracaoDocumentoSituacaoEmails = new List<ConfiguracaoDocumentoSituacaoEmail>();
            this.Countries = new List<Country>();
            //this.ItemDeTrabalhoes = new List<ItemDeTrabalho>();
            this.Notes = new List<Note>();
            this.Parties = new List<Party>();
            this.RoleBases = new List<RoleBase>();
        }

        public int OID { get; set; }
        public string TypeName { get; set; }
        public string AssemblyName { get; set; }
        public virtual ICollection<ConfiguracaoDocumentoSituacaoEmail> ConfiguracaoDocumentoSituacaoEmails { get; set; }
        public virtual ICollection<Country> Countries { get; set; }
        //public virtual ICollection<ItemDeTrabalho> ItemDeTrabalhoes { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Party> Parties { get; set; }
        public virtual ICollection<RoleBase> RoleBases { get; set; }
    }
}
