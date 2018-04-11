using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace WexProject.MultiAccess.Library.Libs
{
    /// <summary>
    /// Classe Utilitária para envio e recebimento de mensagens via Conexão Tcp
    /// </summary>
    public class TcpUtil
    {
        /// <summary>
        /// Stream de Comunicação entre sockets
        /// </summary>
        static NetworkStream stream;

        /// <summary>
        /// Enviar uma mensagem string para um determinado tcp
        /// </summary>
        /// <param name="msg">Mensagem de texto</param>
        /// <param name="tcp">Conexão Tcp destinatária</param>
        public static void EnviarMensagemTcp( string msg, TcpClient tcp )
        {
            lock(tcp)
            {
                if(ConexaoTcpValida( tcp ) && msg != null && msg != string.Empty)
                {
                    stream = tcp.GetStream();
                    if(stream.CanWrite)
                    {
                        byte[] sendbytes = Encoding.ASCII.GetBytes( msg );
                        stream.Write( sendbytes, 0, sendbytes.Length );
                        stream.Flush();
                    }
                } 
            }
        }

        /// <summary>
        /// Receber uma de texto mensagem via conexão tcp
        /// </summary>
        /// <param name="tcp">Tcp que espera a mensagem</param>
        /// <returns>Retorna a mensagem string recebida</returns>
        public static string ReceberMensagemTcp( TcpClient tcp )
        {
            //Debug.WriteLine(string.Format("\nTcpUtil (Leitura) - {0} ",Thread.CurrentThread.Name));
            if(!ConexaoTcpValida( tcp ))
                return string.Empty;
            try
            {
                lock(tcp)
                {
                    stream = tcp.GetStream();
                    if(stream.CanRead && stream.DataAvailable)
                    {
                        byte[] bytes = new byte[tcp.ReceiveBufferSize];
                        //leitura do mensagem
                        stream.Read( bytes, 0, tcp.ReceiveBufferSize );
                        string mensagem = Encoding.ASCII.GetString( bytes );
                        if(mensagem != null && mensagem != string.Empty)
                        {
                            mensagem = mensagem.TrimEnd( new char[] { '\0' } );
                            // Debug.WriteLine(string.Format("TcpUtil efetuou leitura! {0}\n Mensagem Json :{1}",Thread.CurrentThread.Name,mensagem));
                            return mensagem;
                        }
                    } 
                }
            }
            catch(Exception excessao)
            {
                Debug.WriteLine( string.Format( "{0}: lançou uma excessão :{1} \n StackTrace{2}", Thread.CurrentThread.Name, excessao.Message, excessao.StackTrace ) );
            }
            return string.Empty;
        }

        /// <summary>
        /// Adicionar um string de proteção a mensagem para verificar a integridade
        /// da mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem a Ser Protegida</param>
        /// <returns>Mensagem com string de integridade</returns>
        public static string AdicionarStringProtecaoDeIntegridade( string mensagem )
        {
            mensagem = mensagem.Replace( "\n", "*EnT3R*" );
            mensagem = String.Format( "{0}\n", mensagem );
            return mensagem;
        }

        /// <summary>
        /// Remover a string de integridade da mensagem
        /// </summary>
        /// <param name="mensagem">Mensagem com string de Integridade</param>
        /// <returns>Mensagem sem a string de Integridade da mensagem</returns>
        public static string RemoverStringProtecaoDeIntegridade( string mensagem )
        {
            mensagem = mensagem.Replace( "\n", "" );
            mensagem = mensagem.Replace( "*EnT3R*", "\n" );
            return mensagem;
        }

        /// <summary>
        /// Responsável por verificar validade de uma conexão tcp
        /// </summary>
        /// <param name="TcpCliente">Conexão tcp a ser validada</param>
        /// <returns></returns>
        public static bool ConexaoTcpValida( TcpClient TcpCliente )
        {
            try
            {
                if(TcpCliente != null && TcpCliente.Client != null && TcpCliente.Client.Connected)
                {
                    // Detect if client disconnected
                    if(TcpCliente.Client.Poll( 0, SelectMode.SelectRead ))
                    {
                        byte[] buff = new byte[1];
                        return TcpCliente.Client.Receive( buff, SocketFlags.Peek ) != 0;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine( "Exception: {0}, Mensagem: {1}, StackTrace: {2}", e.InnerException, e.Message, e.StackTrace );
                return false;
            }
        }
    }
}
