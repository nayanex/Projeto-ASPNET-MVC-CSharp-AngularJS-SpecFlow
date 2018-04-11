using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Qualidade
{
    /// <summary>
    /// Mostrando os possiveis tipos de resultados para um caso de teste.
    /// </summary>
    public enum CsTiposResultadosDomain
    {
        #region Attributes
        #endregion

        #region Properties
        /// <summary>
        /// Declaração da choice RN
        /// </summary>
        [Description("RN")] RN,
        /// <summary>
        /// Declaração da choice RT
        /// </summary>
        [Description("RT")] RT
    }
    #endregion
}
