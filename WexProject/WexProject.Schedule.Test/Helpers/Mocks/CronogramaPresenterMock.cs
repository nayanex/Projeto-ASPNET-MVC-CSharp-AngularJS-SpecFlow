using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Schedule.Library.Presenters;
using WexProject.MultiAccess.Library.Components;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    /// <summary>
    /// Classe que simula o comportamento do CronogramaPresenter com propriedades publicas para auxilio em testes unitários
    /// </summary>
    public class CronogramaPresenterMock : CronogramaPresenter
    {

        /// <summary>
        /// Propriedade WexMultiAccessClient publicado para testes
        /// </summary>
        public IWexMultiAccessClient Cliente { get { return accessClient; } }

        /// <summary>
        /// Propriedade View publicado para testes
        /// </summary>
        public ICronogramaView View { get { return CronogramaView; } }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<Guid, bool> TarefaRespostaAutorizacaoPendente { get { return tarefaRespostaAutorizacaoPendente; } }

        /// <summary>
        /// Propriedade responsável por armazenar as tarefas que estão aguardando a autorização publicado para testes unitários
        /// </summary>
        public Dictionary<Guid, bool> AutorizacaoTarefas { get { return autorizacaoTarefas; } }

        /// <summary>
        /// Gerenciadador de comandos pode executar comandos
        /// </summary>
        public bool PodeExecutar 
        { 
            get { return gerenciadorComandos.PodeExecutar ;}
            set { gerenciadorComandos.PodeExecutar = value; } 
        }

        /// <summary>
        /// Inicializar presenter e seus atributos
        /// </summary>
        /// <param name="view">CronogramaView a ser gerenciado pelo presenter</param>
        /// <param name="client">WexMultiAccessClient que encontra-se conectado ao servidor</param>
        public CronogramaPresenterMock( ICronogramaView view, IWexMultiAccessClient client )
            : base( view, client )
        {
        }
    }
}
