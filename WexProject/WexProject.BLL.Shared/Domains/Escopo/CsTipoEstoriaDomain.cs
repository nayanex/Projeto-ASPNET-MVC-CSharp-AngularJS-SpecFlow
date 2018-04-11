using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Escopo
{
    /// <summary>
    /// Enumerado com os valores escopo encontrado, solicitação de mudança e novas ideias.
    /// </summary>
    public enum CsTipoEstoriaDomain
    {

        #region Attributes       
        #endregion

        #region Properties
        /// <summary>
        /// Escopo Encontrado
        /// </summary>
        [Description("Escopo Contratado")] EscopoContratado,
        /// <summary>
        /// Escopo para solicitação de alguma mudança
        /// </summary>
        [Description("Solicitação de Mudança")] SolicitacaoMudanca
    }
    #endregion
}
