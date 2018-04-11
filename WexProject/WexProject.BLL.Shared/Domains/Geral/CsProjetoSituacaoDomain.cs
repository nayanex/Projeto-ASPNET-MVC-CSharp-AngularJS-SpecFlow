using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Geral
{
    /// <summary>
    /// Enum da situação do projeto
    /// </summary>
    public enum CsProjetoSituacaoDomain
    {

        /// <summary>
        /// Opção "Não Iniciado"
        /// </summary>
        [Description("Em Andamento")]
        EmAndamento,
        /// <summary>
        /// Opção "Concluido"
        /// </summary>
        [Description("Concluído")]
        Concluido,
        /// <summary>
        /// Opção "Em atraso"
        /// </summary>
        [Description("Em Atraso")]
        EmAtraso,
        /// <summary>
        /// Opção "Cancelado"
        /// </summary>
        [Description("Cancelado")]
        Cancelado
    }
}

