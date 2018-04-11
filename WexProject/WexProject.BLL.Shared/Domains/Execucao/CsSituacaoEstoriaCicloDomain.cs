using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Execucao
{
    /// <summary>
    /// Enum da situação dos itens do ciclo
    /// </summary>
    public enum CsSituacaoEstoriaCicloDomain
    {
        /// <summary>
        /// Não iniciado
        /// </summary>
        [Description("Não Iniciado")]
        NaoIniciado,
        /// <summary>
        /// Em desenvolvimento
        /// </summary>
        [Description("Em Desenvolvimento")]
        EmDesenv,
        /// <summary>
        /// Pronto
        /// </summary>
        [Description("Pronto")]
        Pronto,
        /// <summary>
        /// Replanejado
        /// </summary>
        [Description("Replanejado")]
        Replanejado
    }
}