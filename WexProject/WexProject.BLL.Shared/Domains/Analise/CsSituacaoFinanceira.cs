using System;
using System.ComponentModel;

namespace WexProject.BLL.Shared.Domains.Analise
{
    /// <summary>
    /// Classe Enum da situação financeira de um projeto
    /// </summary>
    public enum CsSituacaoFinanceira
    {
        [Description("Positivo")]
        Positivo,
        [Description("Atenção")]
        Atencao,
        [Description("Crítico")]
        Critico,
    }
}
