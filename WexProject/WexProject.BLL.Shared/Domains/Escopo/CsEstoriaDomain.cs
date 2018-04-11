using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Escopo
{
    /// <summary>
    /// Classe de EstoriaDomain
    /// </summary>
    public enum CsEstoriaDomain
    {

        #region Properties
        /// <summary>
        /// Opção "Não Iniciado"
        /// </summary>
        [Description("Não Iniciado")] NaoIniciado,
        /// <summary>
        /// Opção Em análise
        /// </summary>
        [Description("Em Analise")] EmAnalise,
        /// <summary>
        /// Opção Replanejado
        /// </summary>
        [Description("Replanejado")] Replanejado,
        /// <summary>
        /// Opção em Desenvolvimento
        /// </summary>
        [Description("Em Desenvolvimento")] EmDesenv,
        /// <summary>
        /// Opção Pronto
        /// </summary>
        [Description("Pronto")] Pronto,
        /// <summary>
        /// Opção Cancelado
        /// </summary>
        [Description("Cancelado")] Cancelado
    }
    #endregion
}
