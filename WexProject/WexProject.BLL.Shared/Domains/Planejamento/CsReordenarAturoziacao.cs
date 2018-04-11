using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Shared.Domains.Planejamento
{
    public enum CsReordenarAutorizacao
    {
        [Description("Autorizado Reordenar.")]
        Autorizado,

        [Description( "Não Autorizado Reordenar." )]
        NaoAutorizado,

        [Description( "Não Autorizado Reordenar, Mas Pode Prosseguir com a reordenação sequente." )]
        NaoAutorizadoMasPodeProsseguirOutraReordenacao,

        [Description( "Não Autorizado Reordenar Por Movimentação se tornar inválida." )]
        NaoAutorizadoPorMovimentacaoInvalida,

        [Description( "Não Autorizado Reordenar Por Semaforos Inválidos." )]
        NaoAutorizadoPorSemaforosInvalidos
    }
}
