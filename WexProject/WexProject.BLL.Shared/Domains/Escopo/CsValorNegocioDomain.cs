using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Escopo
{
    /// <summary>
    /// Enumerado com os valores para Sim e Não.
    /// </summary>
    public enum CsValorNegocioDomain
    {
        #region Attributes
        #endregion

        #region Properties
        /// <summary>
        /// Mandatorio
        /// </summary>
        [Description("Mandatorio")] Mandatorio,
        /// <summary>
        /// Importante
        /// </summary>
        [Description("Importante")] Importante,
        /// <summary>
        /// Desejável
        /// </summary>
        [Description("Desejável")] Desejavel
    }
    #endregion
}
