using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.Xpo;

namespace WexProject.BLL.Shared.Domains.Planejamento
{
    /// <summary>
    /// Guarda o enum das opções de tipos para o planejamento.
    /// </summary>
    public enum CsTipoPlanejamento
    {
        /// <summary>
        /// Encerramento
        /// </summary>
        [Description("Encerramento")] 
        [Custom("Caption","Encerramento")]
        Encerramento,

        /// <summary>
        /// Execucção
        /// </summary>
        [Description("Execução")]
        [Custom("Caption", "Execução")]
        Execução,

        /// <summary>
        /// Impedimento
        /// </summary>
        [Description("Impedimento")]
        [Custom("Caption", "Impedimento")]
        Impedimento,

        /// <summary>
        /// Planejamento
        /// </summary>
        [Description("Planejamento")]
        [Custom("Caption", "Planejamento")]
        Planejamento,

        /// <summary>
        /// Cancelamento
        /// </summary>
        [Description("Cancelamento")]
        [Custom("Caption", "Cancelamento")]
        Cancelamento
    }
}