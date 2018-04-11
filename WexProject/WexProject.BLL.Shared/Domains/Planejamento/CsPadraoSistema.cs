using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;


namespace WexProject.BLL.Shared.Domains.Planejamento
{
    /// <summary>
    /// Guarda o enum das opções de tipos para o planejamento
    /// </summary>
    public enum CsPadraoSistema
    {
        /// <summary>
        /// Não
        /// </summary>
        [Description("Não")]
        [Custom("Caption", "Não")]
        Não,

        /// <summary>
        /// Sim
        /// </summary>
        [Description("Sim")] 
        [Custom("Caption", "Sim")]
        Sim
    }
}