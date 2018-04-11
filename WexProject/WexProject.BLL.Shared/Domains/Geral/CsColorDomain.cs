using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Geral
{
    /// <summary>
    /// Domain do tipo de Cor.
    /// </summary>
    public enum CsColorDomain
    {
        /// <summary>
        /// Cor Custom
        /// </summary>
        [Description("Custom")]
        Custom,
        /// <summary>
        /// Color web
        /// </summary>
        [Description("Web")]
        web,
        /// <summary>
        /// Color sistema
        /// </summary>
        [Description("System")]
        System
    }
}
