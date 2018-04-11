using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace WexProject.BLL.Shared.Domains.Planejamento
{
    /// <summary>
    /// Enum que guarda os valores para tipos de situações no planejamento
    /// </summary>
    public enum CsTipoSituacaoPlanejamento
    {
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
}