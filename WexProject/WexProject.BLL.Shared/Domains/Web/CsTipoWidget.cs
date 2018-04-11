using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WexProject.BLL.Shared.Domains.Web
{
    public enum CsTipoWidget
    {
        /// <summary>
        /// Categoria Projetos
        /// </summary>
        [Description("Escopo vs Completude")]
        EscopoCompletude = 0,

        /// <summary>
        /// Categoria Rh
        /// </summary>
        [Description("Estimado vs Realizado")]
        EstimadoRealizado = 1,


    }
}
