using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.Domains.Planejamento
{
    public enum CsMotivoReordenacaoGeral
    {
        /// <summary>
        /// Reordenação por Motivo de Criação
        /// </summary>
        [Description("Criação")]
        Criacao,

        /// <summary>
        /// Reordenação por Motivo de Exclusão
        /// </summary>
        [Description( "Exclusão" )]
        Exclusao,

        /// <summary>
        /// Reordenação por Motivo de Movimentação
        /// </summary>
        [Description( "Movimentação" )]
        Movimentacao,
    }
}
