using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Execucao
{
    /// <summary>
    /// Classe Enum da situação do ciclo
    /// </summary>
    public enum CsSituacaoCicloDomain
    {
        /// <summary>
        /// Não Planejado
        /// </summary>
        [Description("Não Planejado")]
        NaoPlanejado,
        /// <summary>
        /// Concluido
        /// </summary>
        [Description("Concluído")]
        Concluido,
        /// <summary>
        /// Em andamento
        /// </summary>
        [Description("Em Andamento")]
        EmAndamento,
        /// <summary>
        /// Planejado
        /// </summary>
        [Description("Planejado")]
        Planejado,
        /// <summary>
        /// Em atraso
        /// </summary>
        [Description("Em Atraso")]
        EmAtraso,
        /// <summary>
        /// Cancelado
        /// </summary>
        [Description("Cancelado")]
        Cancelado
    }
}
