using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.NovosNegocios
{
    /// <summary>
    /// Enum de prioridades
    /// </summary>
    public enum CsPrioridade
    {
        /// <summary>
        /// Opção "Alta"
        /// </summary>
        [Description("Alta")]
        Alta,
        /// <summary>
        /// Media
        /// </summary>
        [Description("Média")]
        Media,
        /// <summary>
        /// Baixa
        /// </summary>
        [Description("Baixa")]
        Baixa
    }
}
