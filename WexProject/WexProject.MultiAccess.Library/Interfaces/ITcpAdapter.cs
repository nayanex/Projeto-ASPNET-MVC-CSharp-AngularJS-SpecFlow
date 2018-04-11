using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.MultiAccess.Library.Interfaces
{
    public interface ITcpAdapter
    {
        /// <summary>
        /// Retornar se o cliente tcp está conectado
        /// </summary>
        bool Conectado { get; set; }

        /// <summary>
        /// Retornar se a conexão tcp é válida
        /// </summary>
        /// <returns></returns>
        bool ValidarConexao();

        /// <summary>
        /// Efetuar o envio de uma mensagem tcp
        /// </summary>
        /// <param name="mensagem"></param>
        void EnviarMensagem( string mensagem );

        /// <summary>
        /// Efetuar a leitura de uma mensagem
        /// </summary>
        /// <returns></returns>
        string EfetuarLeitura();

        /// <summary>
        /// Liberar uma conexão tcp
        /// </summary>
        void Dispose();

        /// <summary>
        /// Efetuar a conexão do cliente tcp
        /// </summary>
        /// <param name="enderecoIp">endereço de ip a ser conectado</param>
        /// <param name="porta">porta tcp a ser conectada</param>
        /// <returns></returns>
        bool Conectar(string enderecoIp,int porta);

        /// <summary>
        /// Efetuar a desconexão da instância tcp
        /// </summary>
        void Desconectar();

        /// <summary>
        /// Verificar se existem dados para serem lidos
        /// </summary>
        /// <returns></returns>
        bool PossuiDadosParaLeitura();
    }
}
