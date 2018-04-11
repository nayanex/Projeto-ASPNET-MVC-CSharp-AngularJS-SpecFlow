using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.Domains.Web
{
    public enum CsTiposDeFiltro
    {
        /// <summary>
        /// Categoria Projetos
        /// </summary>
        [Description("Chave Estrangeira")]
        PorChaveEstrangeira = 0,

        [Description("Chave Estrangeira 2")]
        PorChaveEstrangeira2 = 1
    }
}
