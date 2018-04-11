using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.MultiAccess.Library.Libs;
using System.Net.Sockets;
using WexProject.MultiAccess.Library.Dtos;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    /// <summary>
    /// Classe Mock Para Criação do cenário de teste
    /// - Test08 : Ao iniciar Deve parar suas threads para o 
    /// </summary>
    public class ConexaoClienteMock : ConexaoCliente 
    {
        /// <summary>
        /// Responsável por iniciar a thread de escuta de um cliente especifico, e receber a fila de eventos a processar
        /// </summary>
        /// <param name="login">login do colaborador</param>
        /// <param name="tcp">Socket de conexão com o socket do colaborador</param>
        /// <param name="fila">referencia a fila para ordenação da execuçao do processamento de mensagens</param>
        public ConexaoClienteMock(string login, TcpClient tcp, Queue<MensagemDto> fila)
            :base(login,tcp,fila)
        {
        }
  
       /// <summary>
        /// Propriedade Publicada para testes que obtem o status de escrita da threadProcessarEscrita
        /// </summary>
        public bool PermissaoDeEscrita { get { return statusEscrita; } set { statusEscrita = value; } }
        /// <summary>
        /// Propriedade Publicada para testes que obtem o status de leitura da threadProcessarLeitura
        /// </summary>
        public bool PermissaoDeLeitura { get { return statusLeitura; } set { statusLeitura = value; } }
     
    }
}
