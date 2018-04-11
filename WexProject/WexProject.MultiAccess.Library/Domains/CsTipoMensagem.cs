using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.MultiAccess.Library.Domains
{   
    /// <summary>
    /// Tipos de MensagemDto
    /// </summary>
    public enum CsTipoMensagem
    {
        NovosUsuariosConectados,
        ConexaoRecusadaServidor,
        UsuarioDesconectado,
        ServidorDesconectando,
        ConexaoEfetuadaComSucesso,
        NovaTarefaCriada,
        InicioEdicaoTarefa,
        EdicaoTarefaRecusada,
        EdicaoTarefaFinalizada,
        ExclusaoTarefaIniciada,
        ExclusaoTarefaPermitida,
        ExclusaoTarefaFinalizada,
        EdicaoTarefaAutorizada,
        MovimentacaoPosicaoTarefa,
        DadosCronogramaAlterados,
        InicioEdicaoNomeCronograma,
        EdicaoNomeCronogramaRecusada,
        EdicaoNomeCronogramaPermitida
    }
}
