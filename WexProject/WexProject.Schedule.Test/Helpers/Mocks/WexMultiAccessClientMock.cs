using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.MultiAccess.Library;
using System.ComponentModel;
using System.Net.Sockets;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Libs;
using WexProject.MultiAccess.Library.Interfaces;
using WexProject.Schedule.Test.Stubs.Redes;
using WexProject.Schedule.Test.Stubs.MultiAccess;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    public class WexMultiAccessClientMock : WexMultiAccessClient
    {
        #region Construtores
        /// <summary>
        /// Construtor para Inicialização do component
        /// </summary>
        public WexMultiAccessClientMock()
            : base()
        {
            InicializarVariaveis();
        }
        /// <summary>
        /// Construtor para Inicialização do component
        /// </summary>
        /// <param name="container">Container de inicialização do component</param>
        public WexMultiAccessClientMock(IContainer container)
            : base(container)
        {
            InicializarVariaveis();
        }

        public void InicializarVariaveis() 
        {
            MensagensRecebidas = new List<MensagemDto>();
            contagemTiposMensagemDtoRecebidas = new Dictionary<int, int>();
            MensagensEnviadas = new List<MensagemDto>();
            TcpAdapterStubAtivo = false;
        }
        #endregion

        #region Atributos

        /// <summary>
        /// Propriedade para Auxilio em Testes Unitários e BDD para armazenamento de todas mensagens recebidas pelo WexClient
        /// </summary>
        public List<MensagemDto> MensagensRecebidas { get; set; }

        /// <summary>
        /// Verificar se deve desabilitar o o factory do TcpAdapter
        /// </summary>
        public bool TcpAdapterStubAtivo { get; set; }

        /// <summary>
        /// Publicar para testes o tcpAdapter
        /// </summary>
        public ITcpAdapter TcpAdapter { get { return tcpAdapter; } }

        /// <summary>
        /// Propriedade para auxilio em testes unitários que armazena mensagens que estão sendo comunicadas
        /// </summary>
        public List<MensagemDto> MensagensEnviadas { get; set; }

        /// <summary>
        /// Propriedade para auxilio em teste unitários e bdd para armazenar a contagem das mensagens recebidas por tipo
        /// </summary>
        public Dictionary<int,int> contagemTiposMensagemDtoRecebidas { get; set; }

        #endregion

        #region Métodos Sobrescritos

        /// <summary>
        /// Método sobrescrito para teste em BDD, acrescentando informação  do login do usuário proprietário do MultiAcessClient vigente.
        /// </summary>
        /// <param name="mensagem">Mensagem do Tipo FinalizarExclusaoTarefa</param>
        public override void AcionarEventoAoTarefaSerExcluida(MensagemDto mensagem)
        {
            base.AcionarEventoAoTarefaSerExcluida(mensagem);
        }

        /// <summary>
        /// Criar uma instância  ITcpAdapter para utilização nos testes
        /// </summary>
        /// <returns></returns>
        protected override ITcpAdapter TcpAdapterFactory()
        {
            if(TcpAdapterStubAtivo)
            {
                TcpAdapterStub stub = new TcpAdapterStub(this);
                return stub;
            }
            return base.TcpAdapterFactory();
        }

        /// <summary>
        /// Método sobrescrito para teste em BDD, acrescentando informação  do login do usuário proprietário do MultiAcessClient vigente.
        /// </summary>
        /// <param name="mensagem"></param>
        public override void AcionarEventoAoEdicaoTarefaSerRecusada(MensagemDto mensagem)
        {
            mensagem.Propriedades.Add(Constantes.LOGIN_WEX_CLIENT,Login);
            base.AcionarEventoAoEdicaoTarefaSerRecusada(mensagem);
        }

        /// <summary>
        ///  Método sobrescrito para teste em BDD, acrescentando informação  do login do usuário proprietário do MultiAcessClient vigente.
        /// </summary>
        /// <param name="mensagem"></param>
        public override void AcionarEventoAoUsuarioDesconectar(MensagemDto mensagem)
        {
            mensagem.Propriedades.Add(Constantes.LOGIN_WEX_CLIENT ,Login);
            base.AcionarEventoAoUsuarioDesconectar(mensagem);
        }

        /// <summary>
        ///  Método sobrescrito para teste em BDD, acrescentando informação  do login do usuário proprietário do MultiAcessClient vigente.
        /// </summary>
        /// <param name="mensagem"></param>
        public override void AcionarEventoAoIniciarEdicaoTarefa(MensagemDto mensagem)
        {
            mensagem.Propriedades.Add(Constantes.LOGIN_WEX_CLIENT ,Login);
            base.AcionarEventoAoIniciarEdicaoTarefa(mensagem);
        }

        /// <summary>
        ///  Método sobrescrito para teste em BDD, acrescentando informação  do login do usuário proprietário do MultiAcessClient vigente.
        /// </summary>
        /// <param name="mensagem"></param>
        public override void AcionarEventoExecutarExclusaoTarefa(MensagemDto mensagem)
        {
            base.AcionarEventoExecutarExclusaoTarefa(mensagem);
        }

        /// <summary>
        /// Método Sobrescrito para Auxilio em testes unitários interceptando objetos MensagemDto que antes do disparo dos eventos
        /// </summary>
        /// <param name="objeto">MensagemDto Recebida na Comunicação</param>
        protected override void ProcessarMensagemEvento(MensagemDto objeto)
        {
            lock(MensagensRecebidas)
            {
                MensagensRecebidas.Add( objeto );
                if(!contagemTiposMensagemDtoRecebidas.ContainsKey( (int)objeto.Tipo ))
                    contagemTiposMensagemDtoRecebidas.Add( (int)objeto.Tipo, 1 );
                else
                    contagemTiposMensagemDtoRecebidas[(int)objeto.Tipo]++;
                base.ProcessarMensagemEvento( objeto ); 
            }
        }

        /// <summary>
        /// Método auxiliar em testes unitários em BDD, adicionando o login do colaborador que receber a mensagem em seu WexMultiAccesssClient
        /// </summary>
        /// <param name="mensagem">MensagemDto do Tipo EdicaoTarefaFinalizada</param>
        public override void AcionarEventoAoSerFinalizadaEdicaoTarefaPorOutroUsuario(MensagemDto mensagem)
        {
            mensagem.Propriedades.Add(Constantes.LOGIN_WEX_CLIENT ,Login);
            base.AcionarEventoAoSerFinalizadaEdicaoTarefaPorOutroUsuario(mensagem);
        }

        public override void AcionarEventoAoNovaTarefaSerCriada(MensagemDto mensagem)
        {
            mensagem.Propriedades.Add(Constantes.LOGIN_WEX_CLIENT ,Login);
            base.AcionarEventoAoNovaTarefaSerCriada(mensagem);
        }

        /// <summary>
        /// Método utilizado para publicar para testes a chamada do método ProcessarMensagemEvento
        /// </summary>
        /// <param name="mensagem"></param>
        public void ProcessarMensagemEventoParaTeste(MensagemDto mensagem) 
        {
           ProcessarMensagemEvento( mensagem );
        }

        /// <summary>
        /// Método utilazado para mock o envio das mensagens armazenando uma copia delas para
        /// </summary>
        /// <param name="mensagem"></param>
        protected override void RnEnviarMensagem( MensagemDto mensagem )
        {
            MensagensEnviadas.Add( mensagem );
            base.RnEnviarMensagem( mensagem );
        }
        #endregion
    }
}
