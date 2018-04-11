using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.Domains.Planejamento
{
    public enum CsMotivoReordenacaoEspecifico
    {
        #region Motivos Específicos

        /// <summary>
        /// Reordenação por Motivo de Criação Ou Exclusão
        /// </summary>
        [Description( "Criação Ou Exclusão" )]
        CriacaoOuExclusao,

        /// <summary>
        /// Reordenação por Motivo de Movimentação para Cima
        /// </summary>
        [Description( "Movimentação para Cima" )]
        MovimentacaoParaCima,

        /// <summary>
        /// Reordenação por Motivo de Movimentação para Cima
        /// </summary>
        [Description( "Movimentação para Baixo" )]
        MovimentacaoParaBaixo,

        /// <summary>
        /// Motivo não especificado (estado antes de ser validado)
        /// </summary>
        [Description("Não Especificado")]
        NaoEspecificado

        #endregion
    }
}
