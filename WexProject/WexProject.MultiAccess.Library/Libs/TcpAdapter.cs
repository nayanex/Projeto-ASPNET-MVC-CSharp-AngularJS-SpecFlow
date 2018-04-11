using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WexProject.MultiAccess.Library.Interfaces;

namespace WexProject.MultiAccess.Library.Libs
{
    public class TcpAdapter : ITcpAdapter
    {
        /// <summary>
        /// Cliente tcp de comunicação via rede
        /// </summary>
        private TcpClient tcp;

        public TcpAdapter()
        {
        }

        /// <summary>
        /// Verifica se a conexão tcp atual é valida
        /// </summary>
        /// <returns></returns>
        public bool ValidarConexao()
        {
            return TcpUtil.ConexaoTcpValida(tcp);
        }

        /// <summary>
        /// Efetua o envio de uma mensagem via a instância de tcp atual
        /// </summary>
        /// <param name="mensagem"></param>
        public void EnviarMensagem( string mensagem )
        {
            TcpUtil.EnviarMensagemTcp( mensagem, tcp );
        }
        
        /// <summary>
        /// Efetua a leitura de uma mensagem via tcp
        /// </summary>
        /// <returns></returns>
        public string EfetuarLeitura()
        {
            return TcpUtil.ReceberMensagemTcp( tcp );
        }

        /// <summary>
        /// Efetuar a liberação do recurso tcp
        /// </summary>
        public void Dispose()
        {
            if(tcp == null)
                return;

            if(ValidarConexao())
            {
                tcp.Client.Shutdown( SocketShutdown.Both );
                tcp.Close();
                tcp = null;
            }
        }

        /// <summary>
        /// Efetua a desconexão do tcp
        /// </summary>
        public void Desconectar()
        {
            Dispose();
        }

        /// <summary>
        /// Verificar se está conectado
        /// </summary>
        public bool Conectado
        {
            get 
            { 
                return tcp != null && tcp.Connected; 
            }

            set{}
        }

        /// <summary>
        /// Efetua a conexão do cliente tcp
        /// </summary>
        /// <param name="enderecoIp"></param>
        /// <param name="porta"></param>
        /// <returns></returns>
        public bool Conectar( string enderecoIp, int porta )
        {
            if(string.IsNullOrEmpty( enderecoIp ))
                throw new ArgumentException( string.Format( "Deveria possuir um ip válido. valor atual {0}" ,enderecoIp) );

            try
            {
                if(!ValidarConexao())
                {
                    tcp = new TcpClient();
                    tcp.Connect( IPAddress.Parse( enderecoIp ), porta );
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna verdadeiro se existir dados para leitura 
        /// </summary>
        /// <returns></returns>
        public bool PossuiDadosParaLeitura()
        {
            return TcpUtil.ConexaoTcpValida( tcp ) && tcp.GetStream().DataAvailable;
        }
    }
}
