using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Interfaces;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Test.Stubs.Redes;

namespace WexProject.Schedule.Test.Stubs.MultiAccess
{
    public class TcpAdapterPool
    {
        /// <summary>
        /// Armazenar o pool de conexões
        /// </summary>
        private Dictionary<string, ITcpAdapter> conexoes;

        public TcpAdapterPool()
        {
            conexoes = new Dictionary<string, ITcpAdapter>();
        }

        /// <summary>
        /// Efetuar o registro do cliente no pool
        /// </summary>
        /// <param name="cliente">cliente a ser adicionado no pool de conexões</param>
        public void AceitarConexao( WexMultiAccessClientMock cliente )
        {
            lock(conexoes)
            {
                string key = CriarChave( cliente );
                if(!string.IsNullOrEmpty( key ) && !conexoes.ContainsKey( key ))
                {
                    conexoes.Add( key, cliente.TcpAdapter );
                    var stub = ConverterParaTcpAdapterStub( cliente.TcpAdapter );
                    stub.TcpPool = this;
                }
            }
        }

        /// <summary>
        /// Efetuar broadcast de uma mensagem para outros clientes
        /// </summary>
        /// <param name="mensagem">mensagem a ser comunicada aos outros clientes</param>
        /// <param name="cliente">cliente que está enviando a mensagem</param>
        public void EnviarMensagem( string mensagem, WexMultiAccessClientMock cliente )
        {
            lock(conexoes)
            {
                string key = CriarChave( cliente );
                foreach(var conexao in conexoes)
                {
                    if(conexao.Key.Equals( key ))
                        continue;

                    if(!string.IsNullOrEmpty( mensagem ))
                    {
                        TcpAdapterStub stub = conexao.Value as TcpAdapterStub;
                        stub.Mensagem += mensagem;
                    }
                }
            }
        }

        /// <summary>
        /// Método para simular uma mensagem do servidor direcionada para um cliente
        /// </summary>
        /// <param name="cliente">cliente que irá receber a mensagem</param>
        /// <param name="mensagem">mensagem json a ser enviada</param>
        public void EnviarMensagemPara( WexMultiAccessClientMock cliente, string mensagem )
        {
            lock(conexoes)
            {
                var stub = GetTcpAdapterStub( cliente );
                if(stub != null)
                {
                    stub.Mensagem += mensagem;
                }
            }
        }

        /// <summary>
        /// Remover um cliente do pool de conexões
        /// </summary>
        /// <param name="cliente">cliente a ser removido</param>
        public void RemoverConexao( WexMultiAccessClientMock cliente )
        {
            lock(conexoes)
            {
                string key = CriarChave( cliente );
                if(!string.IsNullOrEmpty( key ) && conexoes.ContainsKey( key ))
                {
                    conexoes.Remove( key );
                }
            }
        }



        /// <summary>
        /// Efetuar uma mensagem broadcast para os clientes armazenados no pool de conexões
        /// </summary>
        /// <param name="mensagem">mensagem json a ser enviada</param>
        public void ServerBroadCast( string mensagem )
        {
            lock(conexoes)
            {
                foreach(var conexao in conexoes)
                {
                    var stub = ConverterParaTcpAdapterStub( conexao.Value );
                    if(stub != null)
                        stub.Mensagem += mensagem;
                }
            }
        }

        #region Métodos privados
        /// <summary>
        /// Retornar o cast para stub
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        private TcpAdapterStub ConverterParaTcpAdapterStub( ITcpAdapter adapter )
        {
            return adapter as TcpAdapterStub;
        }

        /// <summary>
        /// Retornar o TcpAdapterStub do cliente
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        private TcpAdapterStub GetTcpAdapterStub( WexMultiAccessClientMock cliente )
        {
            lock(conexoes)
            {
                string key = CriarChave( cliente );
                if(conexoes != null && conexoes.ContainsKey( key ))
                {
                    var stub = ConverterParaTcpAdapterStub( conexoes[key] );
                    return stub;
                }
            }
            return null;
        }

        /// <summary>
        /// Criar uma chave para identificação de uma conexão
        /// </summary>
        /// <param name="cliente"></param>
        /// <returns></returns>
        private string CriarChave( WexMultiAccessClientMock cliente )
        {
            if(cliente == null || string.IsNullOrWhiteSpace( cliente.Login ) || string.IsNullOrEmpty( cliente.OidCronograma ))
                return null;

            return string.Format( "{0}_{1}", cliente.Login, cliente.OidCronograma );
        }
        #endregion

    }
}
