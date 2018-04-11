using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Rh
{
    /// <summary>
    /// Enum de situação das férias
    /// </summary>
    public enum CsSituacaoFeriasPlanejamento
    {
        /// <summary>
        /// Opção Planejado
        /// </summary>
        [Description("Planejado")]
        Planejado = 0,

        /// <summary>
        /// Opção Realizado
        /// </summary>
        [Description("Realizado")]
        Realizado = 1
    }
}