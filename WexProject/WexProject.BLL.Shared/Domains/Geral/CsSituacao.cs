using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Geral
{
    /// <summary>
    /// Enum de situação
    /// </summary>
    public enum CsSituacao
    {
        /// <summary>
        /// Opção "Ativo"
        /// </summary>
        [Description("Ativo")]
        Ativo,

        /// <summary>
        /// Opção "Inativo"
        /// </summary>
        [Description("Inativo")]
        Inativo
    }
}
