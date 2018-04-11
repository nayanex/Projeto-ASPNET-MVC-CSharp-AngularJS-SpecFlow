using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Execucao
{

    /// <summary>
    /// Classe para as choices de status do motivo de cancelamento
    /// </summary>
    public enum CsStatusMotivoCancelamento
    {
        #region Attributes
        #endregion

        #region Properties
        /// <summary>
        /// Ativo
        /// </summary>
        [Description("Ativo")]
        Ativo,
        /// <summary>
        /// Inativo
        /// </summary>
        [Description("Inativo")]
        Inativo
    }
    #endregion
}
