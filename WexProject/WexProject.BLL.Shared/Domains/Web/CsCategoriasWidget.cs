using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.Domains.Web
{
    public enum CsCategoriasWidget
    {
        /// <summary>
        /// Categoria Desenvolvedor
        /// </summary>
        [Description("Desenvolvedor")]
        Desenvolvedor = 0,

        /// <summary>
        /// Categoria Rh
        /// </summary>
        [Description("Rh")]
        Rh = 1,
        
        /// <summary>
        /// Categoria Designer
        /// </summary>
        [Description("Designer")]
        Designer = 2,

        /// <summary>
        /// Categoria Qualidade
        /// </summary>
        [Description("Qualidade")]
        Qualidade = 3,

        /// <summary>
        /// Categoria Gerente
        /// </summary>
        [Description("Gerente")]
        Gerente = 4,

        /// <summary>
        /// Categoria Lider
        /// </summary>
        [Description("Lider")]
        Lider = 5,

        /// <summary>
        /// Categoria Custom
        /// </summary>
        [Description("Custom")]
        Custom = 6
    }
}
