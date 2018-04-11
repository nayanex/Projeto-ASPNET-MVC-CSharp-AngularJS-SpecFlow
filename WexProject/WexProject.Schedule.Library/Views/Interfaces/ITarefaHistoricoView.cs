using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WexProject.Schedule.Library.Views.Interfaces
{
    public interface ITarefaHistoricoView
    {
        #region Propriedades

        string NbHoraRealizado { get; set; }

        DateTime DtRealizado { get; set; }

        string NbHoraInicial { get; set; }

        string NbHoraFinal { get; set; }

        string TxComentario { get; set; }

        string NbHoraRestante { get; set; }

        Guid OidSituacaoPlanejamento { get; set; }

        string TxJustificativaDeReducao { get; set; }
        #endregion

        #region Métodos

        /// <summary>
        /// Método Responsável por alterar a visibilidade do campo justificativa de redução
        /// </summary>
        /// <param name="ativo">valor booleano para indicar a visibilidade do campo</param>
        void AlterarEstadoAtivacaoPainelJustificativa(bool ativo);

        /// <summary>
        /// Método responsável por notifica uma mensagem de alerta
        /// </summary>
        /// <param name="titulo">titulo da mensagem</param>
        /// <param name="mensagem">mensagem</param>
        /// <param name="icone"></param>
        void NotificarMensagem( string titulo, string mensagem);

        void Desabilitar();

        void Habilitar();

        #endregion
    }
}
