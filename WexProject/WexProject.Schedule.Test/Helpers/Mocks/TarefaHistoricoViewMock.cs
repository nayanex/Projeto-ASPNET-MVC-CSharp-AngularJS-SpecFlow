using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Schedule.Library.Views.Interfaces;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    public class TarefaHistoricoViewMock : ITarefaHistoricoView
    {
        public string NbHoraRealizado { get; set; }

        public DateTime DtRealizado { get; set; }

        public string NbHoraInicial { get; set; }

        public string NbHoraFinal { get; set; }

        public string TxComentario { get; set; }

        public string NbHoraRestante { get; set; }

        public Guid OidSituacaoPlanejamento { get; set; }

        public string TxJustificativaDeReducao { get; set; }

        public void AlterarEstadoAtivacaoPainelJustificativa(bool ativo)
        {
            //Caso haja necessidade em algum teste unitário, implementar essa funcionalidade
            //método criado para satisfazer a implementação da interface
        }
        public virtual void NotificarMensagem( string titulo, string mensagem )
        {
            //Implementar caso haja necessidade nos testes unitários
        }


        public void Desabilitar()
        {
            //Implementar caso haja necessidade nos testes unitários
        }

        public void Habilitar()
        {
            //Implementar caso haja necessidade nos testes unitários
        }
    }
}
