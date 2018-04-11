using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Geral
{
    /// <summary>
    /// Enum de tipos de documento
    /// </summary>
    public enum CsTipoDocumento
    {
        /// <summary>
        /// Opção "Solicitação de Orçamento"
        /// </summary>
        [Description("Solicitação de Orçamento")]
        SolicitacaoOrcamento,
        /// <summary>
        /// Opção "Proposta Técnica"
        /// </summary>
        [Description("Proposta Técnica")]
        PropostaTecnica,
        /// <summary>
        /// Opção "Proposta Financeira"
        /// </summary>
        [Description("Proposta Financeira")]
        PropostaFinanceira
    }
}
