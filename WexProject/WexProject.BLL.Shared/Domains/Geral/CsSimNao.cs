using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Geral
{
    /// <summary>
    /// Enum de Sim e Não
    /// </summary>
    public enum CsSimNao
    {
        /// <summary>
        /// Não
        /// </summary>
        [Description("Não")]
        Não = 0,

        /// <summary>
        /// Sim
        /// </summary>
        [Description("Sim")]
        Sim = 1
    }
}
