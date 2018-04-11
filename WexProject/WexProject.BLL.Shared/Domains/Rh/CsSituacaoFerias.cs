using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Rh
{
    /// <summary>
    /// Enum de situação das férias
    /// </summary>
    public enum CsSituacaoFerias
    {
        /// <summary>
        /// Opção Planejado
        /// </summary>
        [Description("Planejado")]
        Planejado = 0,

        /// <summary>
        /// Opção em atraso
        /// </summary>
        [Description("Em Atraso")]
        EmAtraso = 1,

        /// <summary>
        /// Opção Realizado
        /// </summary>
        [Description("Realizado")]
        Realizado = 2,

        /// <summary>
        /// Opção Vendida
        /// </summary>
        [Description("Vendida")]
        Vendida = 3
    }
}