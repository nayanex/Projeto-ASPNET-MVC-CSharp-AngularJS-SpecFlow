using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.MultiAccess.Library.Interfaces;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Test.Stubs.MultiAccess;

namespace WexProject.Schedule.Test.Stubs.Redes
{
    public class TcpAdapterStub : ITcpAdapter
    {
        /// <summary>
        /// Cliente proprietário do Tcp Adapter
        /// </summary>
        private WexMultiAccessClientMock cliente;

        /// <summary>
        /// Armazenar se está conectado
        /// </summary>
        public bool Conectado { get; set; }

        /// <summary>
        /// Armazenar se é valido
        /// </summary>
        public bool Valido { get; set; }

        /// <summary>
        /// Armazenar o gerenciador de comunicação
        /// </summary>
        private TcpAdapterPool tcpPool;

        /// <summary>
        /// Efetuar get e set do gerenciador de comunicação
        /// </summary>
        public TcpAdapterPool TcpPool
        {
            get { return tcpPool; }
            set 
            {
                tcpPool = value;
            }
        }

        /// <summary>
        /// Armazenar a mensagem escrita
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Criar uma instância stub Adapter
        /// </summary>
        /// <param name="valido">Informação se o stub será considerado uma instância válida ou não</param>
        public TcpAdapterStub(WexMultiAccessClientMock pai)
        {
            cliente = pai;
            Valido = true;
        }

        /// <summary>
        /// Retornar se está conectado
        /// </summary>
        /// <returns></returns>
        public bool ValidarConexao()
        {
            return Conectado;
        }

        /// <summary>
        /// Efetuar o envio da mensagem via pool
        /// </summary>
        /// <param name="mensagem"></param>
        public void EnviarMensagem( string mensagem )
        {
            if(!Conectado)
                return;

            if(tcpPool != null)
                tcpPool.EnviarMensagem( mensagem, cliente );
        }

        /// <summary>
        /// Efetuar a leitura da mensagem recebida
        /// </summary>
        /// <returns></returns>
        public string EfetuarLeitura()
        {
            if(Mensagem != null)
            {
                string mensagem = Mensagem;
                Mensagem = null;
                return mensagem;
            }
            return Mensagem;
        }

        /// <summary>
        /// Efetuar a liberação do cliente do pool
        /// </summary>
        public void Dispose()
        {
            Valido = false;
            Conectado = false;
            if(tcpPool != null)
                tcpPool.RemoverConexao( cliente );
        }

        /// <summary>
        /// Efetuar a conexão
        /// </summary>
        /// <param name="enderecoIp"></param>
        /// <param name="porta"></param>
        /// <returns></returns>
        public bool Conectar( string enderecoIp, int porta )
        {
            if(Valido)
                Conectado = true;
            return Conectado;
        }

        /// <summary>
        /// Efetuar a desconexão
        /// </summary>
        public void Desconectar()
        {
            Dispose();
        }

        /// <summary>
        /// Validar se existe dados para leitura
        /// </summary>
        /// <returns></returns>
        public bool PossuiDadosParaLeitura()
        {
            return Valido && Conectado && !string.IsNullOrWhiteSpace( Mensagem );
        }
    }
}
