using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;


namespace WexProject.BLL.Shared.Domains.Planejamento
{
    /// <summary>
    /// Guarda o enum das opções de atalhos
    /// </summary>
    public enum CsAtalhosSituacao
    {
        /// <summary>
        /// Ctrl + 1
        /// </summary>
        [Description("Ctrl + 1")]
        [Custom("Caption", "Ctrl + 1")]
        Ctrl1,
        /// <summary>
        /// Ctrl + 2
        /// </summary>
        [Description("Ctrl + 2")]
        [Custom("Caption", "Ctrl + 2")]
        Ctrl2,
        /// <summary>
        /// Ctrl + 3
        /// </summary>
        [Description("Ctrl + 3")]
        [Custom("Caption", "Ctrl + 3")]
        Ctrl3,
        /// <summary>
        /// Ctrl + 4
        /// </summary>
        [Description("Ctrl + 4")]
        [Custom("Caption", "Ctrl + 4")]
        Ctrl4,
        /// <summary>
        /// Ctrl + 5
        /// </summary>
        [Description("Ctrl + 5")]
        [Custom("Caption", "Ctrl + 5")]
        Ctrl5,
        /// <summary>
        /// Ctrl + 1
        /// </summary>
        [Description("Ctrl + 6")]
        [Custom("Caption", "Ctrl + 6")]
        Ctrl6,
        /// <summary>
        /// Ctrl + 7
        /// </summary>
        [Description("Ctrl+ 7")]
        [Custom("Caption", "Ctrl + 7")]
        Ctrl7,
        /// <summary>
        /// Ctrl + 8
        /// </summary>
        [Description("Ctrl + 8")]
        [Custom("Caption", "Ctrl + 8")]
        Ctrl8,
        /// <summary>
        /// Ctrl + 9
        /// </summary>
        [Description("Ctrl + 9")]
        [Custom("Caption", "Ctrl + 9")]
        Ctrl9,
        /// <summary>
        /// Ctrl + 0
        /// </summary>
        [Description("Ctrl + 0")]
        [Custom("Caption", "Ctrl + 0")]
        Ctrl0
    }
}
