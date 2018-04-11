using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Libs;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library;
using System.Net.Sockets;
using System.Net;
using WexProject.MultiAccess.Library.Domains;
using WexProject.Library.Libs;
using WexProject.Library.Libs.Test;
using Moq;
using Newtonsoft.Json;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Library.Libs.DataHora;
using System.Threading;
using WexProject.Schedule.Test.Stubs.Redes;
using WexProject.Schedule.Test.Stubs.MultiAccess;
using WexProject.MultiAccess.Library.Components;


namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class MultiAccessClientTest
    {
        /// <summary>
        /// Atributo para fins de teste simulando oidTarefa e NbId para testes de comunicação
        /// </summary>
        Dictionary<string, Int16> tarefasGrid;

        const string ipServidor = "127.0.0.1";
        const int porta = 8000;

        /// <summary>
        /// Método para inicializae as tarefas adicionadas no grid (oidTarefa,NbId)
        /// </summary>
        public void InicializarTarefasGrid()
        {
            tarefasGrid = new Dictionary<string, short>();
            tarefasGrid.Add( "T1", 1 );
            tarefasGrid.Add( "T2", 2 );
            tarefasGrid.Add( "T3", 3 );
            tarefasGrid.Add( "T4", 4 );
            tarefasGrid.Add( "T5", 5 );
            tarefasGrid.Add( "T6", 6 );
            tarefasGrid.Add( "T7", 7 );
            tarefasGrid.Add( "T8", 8 );
            tarefasGrid.Add( "T9", 9 );
            tarefasGrid.Add( "T10", 10 );
        }

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma coleção de tarefas impactadas
        /// </summary>
        public const string DATAHORA_ACAO = "dataHoraAcao";

        /// <summary>
        /// Constante indice hashtable propriedades da MensagemDto
        /// </summary>
        const string OIDCRONOGRAMA = "oidCronograma";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de uma tarefa
        /// </summary>
        const string OIDTAREFA = "oidTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o nbId (no grid) de uma tarefa
        /// </summary>
        const string IDTAREFA = "nbIdTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar a descrição de uma nova tarefa
        /// </summary>
        const string DESCRICAOTAREFA = "txDescricaoTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de um usuário.
        /// </summary>
        const string OIDUSUARIO = "oidUsuario";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de um usuário.
        /// </summary>
        const string LOGIN = "login";


        [TestMethod]
        public void RnProcessarEventosDispararEventoAoReceberMensagemDeServidorDesconectando()
        {
            int contador = 0;
            string oidCronograma = Guid.NewGuid().ToString();
            TcpAdapterPool poolTcp = new TcpAdapterPool();

            //Criando a mensagem de desconexão do servidor
            MensagemDto mensagemTemporaria = Mensagem.RnCriarMensagemServidorDesconectando( "Servidor Efetuando Processo de Desligamento!" );
            string mensagemJson = JsonConvert.SerializeObject( mensagemTemporaria );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            WexMultiAccessClientMock client = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                Porta = porta,
                Login = "gabriel.matos",
                OidCronograma = oidCronograma,
                TcpAdapterStubAtivo = true
            };

            client.Conectar();
            poolTcp.AceitarConexao( client );
            client.AoServidorDesconectar += ( mensagem ) => { contador++; };

            poolTcp.ServerBroadCast( mensagemJson );

            ControleDeEsperaUtil.AguardarAte( () => { return contador > 0; } );
            Assert.AreEqual( 1, contador, "O contador adicionado ao Evento AoServidorDesconectar deveria ter sido contado 1 vez" );
            client.RnDesconectar();
        }

        [TestMethod]
        public void RnComunicarCriacaoNovaTarefaQuandoEnviarMensagemNovaTarefaTest()
        {
            int contador = 0;
            TcpAdapterPool poolTcp = new TcpAdapterPool();
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                OidCronograma = "C1",
                Porta = porta,
                Login = "gabriel.matos",
                TcpAdapterStubAtivo = true
            };

            WexMultiAccessClientMock cliente2 = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                OidCronograma = "C1",
                Porta = porta,
                Login = "anderson.lins",
                TcpAdapterStubAtivo = true
            };

            cliente.Conectar();
            poolTcp.AceitarConexao( cliente );

            cliente2.Conectar();
            poolTcp.AceitarConexao( cliente2 );

            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            cliente2.AoServidorDesconectar += ( mensagemDto ) => { };
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T2", 3 );
            tarefasImpactadas.Add( "T3", 4 );
            tarefasImpactadas.Add( "T4", 5 );
            tarefasImpactadas.Add( "T5", 6 );
            tarefasImpactadas.Add( "T6", 7 );
            tarefasImpactadas.Add( "T7", 8 );



            DateUtil.CurrentDateTime = DateTime.Now;
            cliente.RnComunicarNovaTarefaCriada( "T1", "C1", null, DateUtil.CurrentDateTime );
            int codigoTipoMensagem = (int)CsTipoMensagem.NovaTarefaCriada;
            ControleDeEsperaUtil.AguardarAte( () => { return cliente2.contagemTiposMensagemDtoRecebidas.ContainsKey( codigoTipoMensagem ) && cliente2.contagemTiposMensagemDtoRecebidas[codigoTipoMensagem] > 0; } );

            MensagemDto mensagemRecebida = cliente2.MensagensRecebidas.FirstOrDefault( o => o.Tipo.Equals( CsTipoMensagem.NovaTarefaCriada ) );

            MensagemDto objetoMensagemEsperado = Mensagem.RnCriarMensagemNovaTarefaCriada( "T1", cliente.Login, "C1", null, DateUtil.CurrentDateTime );

            Assert.AreEqual( objetoMensagemEsperado.Propriedades[OIDTAREFA], mensagemRecebida.Propriedades[OIDTAREFA] );
            Assert.AreEqual( objetoMensagemEsperado.Propriedades[LOGIN], mensagemRecebida.Propriedades[LOGIN] );
            Assert.AreEqual( objetoMensagemEsperado.Propriedades[OIDCRONOGRAMA], mensagemRecebida.Propriedades[OIDCRONOGRAMA] );
            Assert.AreEqual( objetoMensagemEsperado.Propriedades[DATAHORA_ACAO], mensagemRecebida.Propriedades[DATAHORA_ACAO] );

        }

        [TestMethod]
        public void RnComunicarInicioEdicaoTarefaTest()
        {
            bool foiInformado = false;

            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };
            TcpAdapterPool pool = new TcpAdapterPool();
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            pool.AceitarConexao( cliente );
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };

            WexMultiAccessClientMock cliente2 = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };
            cliente2.EnderecoIp = ipServidor;
            cliente2.OidCronograma = "C1";
            cliente2.Porta = porta;
            cliente2.Login = "Pedro";
            cliente2.Conectar();
            pool.AceitarConexao( cliente2 );
            cliente2.AoServidorDesconectar += ( mensagemDto ) => { };
            cliente2.AoIniciarEdicaoTarefa += ( mensagemDto ) =>
            {
                if(mensagemDto != null && mensagemDto.Tipo.Equals( CsTipoMensagem.InicioEdicaoTarefa ))
                {
                    foiInformado = true;
                }
            };

            cliente.RnComunicarInicioEdicaoTarefa( "T1" );
            ControleDeEsperaUtil.AguardarAte( () => { return foiInformado; } );

            //Recebendo a mensagem de que iniciou edição de uma tarefa
            MensagemDto objetoMensagemEsperado = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", cliente.Login, "C1" );
            objetoMensagemEsperado.Propriedades.Add( Constantes.LOGIN_WEX_CLIENT, cliente2.Login );
            MensagemDto objetoMensagem = cliente2.MensagensRecebidas.FirstOrDefault( o => o.Tipo.Equals( CsTipoMensagem.InicioEdicaoTarefa ) );
            Assert.IsNotNull( objetoMensagem, "Deveria ter recebido a mensagem esperada" );
            CollectionAssert.AreEquivalent( objetoMensagemEsperado.Propriedades, objetoMensagem.Propriedades );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosProcessarMensagemInicioEdicaoTarefaTest()
        {
            bool mensagemRecebida = false;
            TcpAdapterPool pool = new TcpAdapterPool();

            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };
            #region Preenchendo informações do cliente
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            pool.AceitarConexao( cliente );
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            #endregion

            WexMultiAccessClientMock cliente2 = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };
            #region Preenchendo informações cliente2
            cliente2.EnderecoIp = ipServidor;
            cliente2.OidCronograma = "C1";
            cliente2.Porta = porta;
            cliente2.Login = "Pedro";
            cliente2.Conectar();
            pool.AceitarConexao( cliente2 );
            cliente2.AoServidorDesconectar += ( mensagemDto ) => { };
            cliente2.AoIniciarEdicaoTarefa += ( mensagemDto ) =>
            {
                if(mensagemDto != null && mensagemDto.Tipo.Equals( CsTipoMensagem.InicioEdicaoTarefa ))
                {
                    mensagemRecebida = true;
                }
            };
            #endregion

            cliente.RnComunicarInicioEdicaoTarefa( "T1" );
            ControleDeEsperaUtil.AguardarAte( () => { return mensagemRecebida; } );

            //Criando uma réplica do que deverá ser recebido pelo cliente2
            MensagemDto objetoMensagemEsperado = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", cliente.Login, "C1" );
            objetoMensagemEsperado.Propriedades.Add( Constantes.LOGIN_WEX_CLIENT, cliente2.Login );


            MensagemDto objetoMensagem = cliente2.MensagensRecebidas.FirstOrDefault( o => o.Tipo.Equals( CsTipoMensagem.InicioEdicaoTarefa ) );
            Assert.IsNotNull( objetoMensagem, string.Format( "{0} deveria ser avisado que uma tarefa entrou em edição.", cliente2.Login ) );
            CollectionAssert.AreEquivalent( objetoMensagemEsperado.Propriedades, objetoMensagem.Propriedades, "Deveria ter recebido as informações esperadas." );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosRecusarTarefa()
        {
            int contador = 0;
            TcpAdapterPool pool = new TcpAdapterPool();
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };

            #region Preenchendo informações do cliente
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            pool.AceitarConexao( cliente );
            cliente.AoSerRecusadaEdicaoTarefa += ( mensagem ) => { contador++; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            #endregion

            //Criando a mensagem de recusa
            MensagemDto recusaDeTarefa = Mensagem.RnCriarMensagemRecusarEdicaoTarefa( "T1", "Marcos", "C1" );
            string mensagemJson = JsonConvert.SerializeObject( recusaDeTarefa );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );

            //Enviando a mensagem de recusa para o cliente
            pool.EnviarMensagemPara( cliente, mensagemJson );

            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );

            Assert.AreEqual( 1, contador, "Deveria ter siso disparado o evento AoEdicaoTarefaSerRecusada" );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosFinalizarEdicaoTarefa()
        {
            int contador = 0;
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { TcpAdapterStubAtivo = true };
            TcpAdapterPool pool = new TcpAdapterPool();

            #region Preenchendo informações do cliente
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            pool.AceitarConexao( cliente );
            cliente.AoSerFinalizadaEdicaoTarefaPorOutroUsuario += ( mensagem ) => { contador++; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            #endregion

            //Criando a mensagem de fim de edição de uma tarefa por outro colaborador
            MensagemDto mensagemFimDeEdicao = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( "T1", "Marcos", "C1" );
            string mensagemJson = JsonConvert.SerializeObject( mensagemFimDeEdicao );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );

            pool.EnviarMensagemPara( cliente, mensagemJson );

            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );
            Assert.AreEqual( 1, contador, "Deveria ter sido disparado o evento AoEdicaoTarefaSerRecusada" );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosReceberMensagensComStringDeProtecao()
        {
            int contador = 0;
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { Porta = porta, EnderecoIp = ipServidor, Login = "Joao", OidCronograma = "C1", TcpAdapterStubAtivo = true };
            TcpAdapterPool pool = new TcpAdapterPool();
            // mensagemDto que serão preenchidas no disparo dos eventos do client
            MensagemDto mensagemAutenticacaoEsperada = new MensagemDto();
            MensagemDto mensagemNovaTarefaCriadaEsperada = new MensagemDto();
            MensagemDto mensagemInicioEdicaoTarefaCriada = new MensagemDto();

            //eventos que deverão preencher as mensagensDto anteriores para comparação posterior
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) => { contador++; mensagemAutenticacaoEsperada = mensagem; };
            cliente.AoSerCriadaNovaTarefa += ( mensagem ) => { contador++; mensagemNovaTarefaCriadaEsperada = mensagem; };
            cliente.AoIniciarEdicaoTarefa += ( mensagem ) => { contador++; mensagemInicioEdicaoTarefaCriada = mensagem; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            cliente.Conectar();
            pool.AceitarConexao( cliente );

            //dicionário de tarefas impactadas
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T2", 1 );
            tarefasImpactadas.Add( "T3", 2 );
            tarefasImpactadas.Add( "T4", 3 );
            tarefasImpactadas.Add( "T5", 4 );

            //mensagens que serão comunicadas ao client e deverão ser recebidas com sucesso no client
            MensagemDto autenticacaoEnviada, novaTarefaCriadaEnviada, inicioEdicaoTarefaEnviada;

            //criando mensagem de conexão aceita e retornando a mensagem ao client
            autenticacaoEnviada = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { "Joao", "Pedro" }, "C1", null );
            string json = JsonConvert.SerializeObject( autenticacaoEnviada );
            json = TcpUtil.AdicionarStringProtecaoDeIntegridade( json );

            //criando mensagem de  inicio de edição de uma tarefa e comunicando ao client
            inicioEdicaoTarefaEnviada = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Joao", "C1" );
            string json2 = JsonConvert.SerializeObject( inicioEdicaoTarefaEnviada );
            json2 = TcpUtil.AdicionarStringProtecaoDeIntegridade( json2 );

            DateUtil.CurrentDateTime = DateTime.Now;

            //criando mensagem de criação de uma nova tarefa
            novaTarefaCriadaEnviada = Mensagem.RnCriarMensagemNovaTarefaCriada( "T2", "Gabriel", "C2", tarefasImpactadas, DateUtil.CurrentDateTime );
            string json3 = JsonConvert.SerializeObject( novaTarefaCriadaEnviada );
            json3 = TcpUtil.AdicionarStringProtecaoDeIntegridade( json3 );

            //unindo as mensagens em uma unica para testar o envio e o recebimento de mensagens coladas
            json = json + json2 + json3;

            pool.EnviarMensagemPara( cliente, json );
            //aguardar até que os 3 eventos do client sejam disparados ao receber e tratar as mensagens enviadas
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 3; } );
            Assert.AreEqual( 3, contador, "Deveria ter contado 3 pois foram é a quantidade de mensagens acumuladas no tcp" );

            //asserts para a mensagem de autenticação

            Assert.AreEqual( mensagemAutenticacaoEsperada.Tipo, autenticacaoEnviada.Tipo, "A mensagem recebida de autenticação deveria ser do mesmo tipo" );
            Assert.AreEqual( (string)mensagemAutenticacaoEsperada.Propriedades[Constantes.OIDCRONOGRAMA], (string)autenticacaoEnviada.Propriedades["oidCronograma"], "Deveria possuir o mesmo" +
                " cronograma nas mensagens de autenticação" );
            CollectionAssert.AreEquivalent( (string[])mensagemAutenticacaoEsperada.Propriedades[Constantes.USUARIOS], (string[])autenticacaoEnviada.Propriedades[Constantes.USUARIOS], "Deveria possuir a mesma lista de usuários online" );

            //asserts para mensagem de criação de nova tarefa

            Assert.AreEqual( mensagemNovaTarefaCriadaEsperada.Tipo, novaTarefaCriadaEnviada.Tipo, "A mensagem recebida de nova tarefa criada deveria ser do mesmo tipo" );
            Assert.AreEqual( (string)mensagemNovaTarefaCriadaEsperada.Propriedades[Constantes.OIDCRONOGRAMA], (string)novaTarefaCriadaEnviada.Propriedades[Constantes.OIDCRONOGRAMA], "A mensagem recebida de nova tarefa criada deveria ser do mesmo tipo" );
            CollectionAssert.AreEquivalent( (Dictionary<string, Int16>)mensagemNovaTarefaCriadaEsperada.Propriedades[Constantes.TAREFAS_IMPACTADAS], (Dictionary<string, Int16>)novaTarefaCriadaEnviada.Propriedades[Constantes.TAREFAS_IMPACTADAS], "Deveria possuir as tarefas impactadas com a mesma ordenacao" );
        }

        [TestMethod]
        public void RnComunicarFimEdicaoTarefaTest()
        {
            int contador = 0;
            TcpAdapterPool pool = new TcpAdapterPool();
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                OidCronograma = "C1",
                Porta = porta,
                Login = "Joao",
                TcpAdapterStubAtivo = true
            };

            WexMultiAccessClientMock cliente2 = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                OidCronograma = "C1",
                Porta = porta,
                Login = "Pedro",
                TcpAdapterStubAtivo = true
            };

            cliente.Conectar();
            pool.AceitarConexao( cliente );

            cliente2.Conectar();
            pool.AceitarConexao( cliente2 );

            cliente2.AoServidorDesconectar += ( mensagem ) => { };
            cliente2.AoSerFinalizadaEdicaoTarefaPorOutroUsuario += ( mensagem ) => { contador++; };
            cliente.AoServidorDesconectar += ( mensagem ) => { };



            cliente.RnComunicarFimEdicaoTarefa( "T1" );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );

            MensagemDto mensagemDto = cliente2.MensagensRecebidas.FirstOrDefault( o => o.Tipo.Equals( CsTipoMensagem.EdicaoTarefaFinalizada ) );

            Assert.AreEqual( CsTipoMensagem.EdicaoTarefaFinalizada, mensagemDto.Tipo, "A mensagem recebida veio com tipo diferente do original" );
            Assert.IsTrue( mensagemDto.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ), "Deveria possuir joao na sua lista de propriedades" );
            Assert.AreEqual( "T1", (string)mensagemDto.Propriedades[Constantes.OIDTAREFA] );
            Assert.AreEqual( "C1", (string)mensagemDto.Propriedades[OIDCRONOGRAMA] );
        }

        [TestMethod]
        public void MultiAccessClientComunicarManagerSobreFimEdicaoTarefa()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8086;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();

            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { EnderecoIp = ipServidor, Porta = porta, Login = "Joao", OidCronograma = "C1" };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            cliente.Conectar();

            TcpClient conexaoTcpEmServidor = servidor.AcceptTcpClient();
            //efetuando a leitura da primeira mensagem do cliente mensagemd e autenticação
            TcpUtil.ReceberMensagemTcp( conexaoTcpEmServidor );
            ///Comunicando o fim da tarefa
            cliente.RnComunicarFimEdicaoTarefa( "T2" );
            string mensagemJson = TcpUtil.ReceberMensagemTcp( conexaoTcpEmServidor );
            mensagemJson = TcpUtil.RemoverStringProtecaoDeIntegridade( mensagemJson );
            MensagemDto mensagem = Mensagem.DeserializarMensagemDto( mensagemJson );

            Assert.AreEqual( cliente.OidCronograma, (string)mensagem.Propriedades[Constantes.OIDCRONOGRAMA], "Deveria ser o mesmo cronograma em que esta conectado o client" );
            Assert.IsTrue( mensagem.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ), "Deveria conter o login do colaborador que esta editou a tarefa" );
            Assert.AreEqual( "T2", (string)mensagem.Propriedades[Constantes.OIDTAREFA], "Deveria ser a mesma tarefa comunicada na mensagem de fim de edição" );
            cliente.RnDesconectar();
            servidor.Stop();

        }

        [TestMethod]
        public void RnComunicarInicioExclusaoTarefaTest()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8086;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock()
            {
                EnderecoIp = ipServidor,
                OidCronograma = "C1",
                Porta = porta,
                Login = "Joao"
            };
            cliente.AoServidorDesconectar += ( mensagem ) => { };
            cliente.Conectar();
            TcpClient tcpEmServidor = servidor.AcceptTcpClient();
            //interceptando  a mensagem de conexão
            TcpUtil.ReceberMensagemTcp( tcpEmServidor );
            cliente.RnComunicarInicioExclusaoTarefa( new string[] { "T1", "T2", "T3" } );
            string resposta = TcpUtil.ReceberMensagemTcp( tcpEmServidor );
            resposta = TcpUtil.RemoverStringProtecaoDeIntegridade( resposta );
            MensagemDto mensagemDto = Mensagem.DeserializarMensagemDto( resposta );
            Assert.AreEqual( CsTipoMensagem.ExclusaoTarefaIniciada, mensagemDto.Tipo, "A mensagem recebida veio com tipo diferente do original" );
            Assert.IsTrue( mensagemDto.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ), "A mensagem deveria conter o usuário que solicitou a exclusão" );
            Assert.AreEqual( "Joao", (string)mensagemDto.Propriedades[Constantes.AUTOR_ACAO], "O usuário deveria ser o Joao" );
            Assert.IsTrue( mensagemDto.Propriedades.ContainsKey( Constantes.TAREFAS ), "A mensagem deveria conter tarefas" );
            CollectionAssert.AreEquivalent( new string[] { "T1", "T2", "T3" }, (string[])mensagemDto.Propriedades["tarefas"], "A mensagem recebida deveria possuir as mesmas tarefas que foram enviadas" );
        }

        [TestMethod]
        public void RnProcessarEventosRecebendoMensagemDoTipoEdicaoTarefaAutorizada()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8088;
            int contador = 0;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            cliente.AoSerAutorizadaEdicaoTarefa += ( mensagem ) => { contador++; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };

            TcpClient tcpEmServidor = servidor.AcceptTcpClient();
            //interceptando  a mensagem de conexão
            TcpUtil.ReceberMensagemTcp( tcpEmServidor );

            MensagemDto mensagemEdicaoAutorizada = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada( "Joao", "C1", "T1" );
            string mensagemJson = JsonConvert.SerializeObject( mensagemEdicaoAutorizada );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            TcpUtil.EnviarMensagemTcp( mensagemJson, tcpEmServidor );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );
            Assert.AreEqual( 1, contador, "Deveria ter siso disparado o evento AoEdicaoTarefaSerRecusada" );
            MensagemDto mensagemEsperada = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.EdicaoTarefaAutorizada ).First();
            Assert.AreEqual( "C1", (string)mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA], "A mensagem recebida deveria ser do cronograma indicado" );
            Assert.AreEqual( "T1", (string)mensagemEsperada.Propriedades[Constantes.OIDTAREFA], "O oid da tarefa da mensagem recebida deveria ser o passado na criação da mensagem" );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosRecebendoMensagemDoTipoMovimentacaoPosicaoTarefa()
        {
            DateUtil.CurrentDateTime = DateTime.Now;
            const string ipServidor = "127.0.0.1";
            const int porta = 8089;
            int contador = 0;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            cliente.AoOcorrerMovimentacaoPosicaoTarefa += ( mensagemDto ) => { contador++; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            TcpClient tcpEmServidor = servidor.AcceptTcpClient();
            //interceptando  a mensagem de conexão
            TcpUtil.ReceberMensagemTcp( tcpEmServidor );
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T3", 3 );
            tarefasImpactadas.Add( "T4", 4 );
            MensagemDto mensagem = Mensagem.RnCriarMensagemMovimentacaoTarefa( 2, 4, "T2", tarefasImpactadas, "Joao", "C1", DateUtil.CurrentDateTime );
            string mensagemJson = JsonConvert.SerializeObject( mensagem );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            //enviando mensagem ao cliente
            TcpUtil.EnviarMensagemTcp( mensagemJson, tcpEmServidor );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );
            Assert.AreEqual( 1, contador, "Deveria ter sido acionado o evento AoOcorrerMovimentacaoPosicaoTarefa" );
            MensagemDto mensagemEsperada = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.MovimentacaoPosicaoTarefa ).FirstOrDefault();
            Assert.IsNotNull( mensagemEsperada, "Deveria ter sido recebida uma mensagem do tipo MovimentacaoPosicaoTarefa" );
            CollectionAssert.AreEqual( tarefasImpactadas, (Dictionary<string, Int16>)mensagemEsperada.Propriedades[Constantes.TAREFAS_IMPACTADAS] );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosRecebendoMensagemDoTipoExclusaoTarefaPermitida()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8090;
            int contador = 0;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();
            MensagemDto mensagemEsperada = new MensagemDto();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            cliente.ExecutarExclusaoTarefa += ( mensagemDto ) => { contador++; mensagemEsperada = mensagemDto; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            TcpClient tcpEmServidor = servidor.AcceptTcpClient();
            //interceptando  a mensagem de conexão
            TcpUtil.ReceberMensagemTcp( tcpEmServidor );
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T3", 3 );
            tarefasImpactadas.Add( "T4", 4 );
            //MensagemDto mensagem = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada(2 ,4 ,"T2","Joao" ,"C1");
            MensagemDto mensagem = Mensagem.RnCriarMensagemEfetuarExclusaoTarefas( new string[] { "T1", "T2" }, new string[] { }, "C1", "Joao" );
            string mensagemJson = JsonConvert.SerializeObject( mensagem );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            //enviando mensagem ao cliente
            TcpUtil.EnviarMensagemTcp( mensagemJson, tcpEmServidor );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );
            Assert.AreEqual( 1, contador, "Deveria ter sido acionado o evento ExecutarExclusaoTarefa" );
            //MensagemDto mensagemEsperada = cliente.mensagensDtoEvento.Where(o => o.Tipo == CsTipoMensagem.ExclusaoTarefaPermitida).FirstOrDefault();
            Assert.IsNotNull( mensagemEsperada, "Deveria ter sido recebida uma mensagem do tipo ExecutarExclusaoTarefa" );
            CollectionAssert.AreEqual( new string[] { "T1", "T2" }, (string[])mensagemEsperada.Propriedades[Constantes.TAREFAS] );
            Assert.AreEqual( "C1", (string)mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA] );
            Assert.AreEqual( "Joao", (string)mensagemEsperada.Propriedades[Constantes.AUTOR_ACAO] );
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnProcessarEventosRecebendoMensagemDoTipoExclusaoTarefaFinalizada()
        {
            DateUtil.CurrentDateTime = DateTime.Now;
            const string ipServidor = "127.0.0.1";
            const int porta = 8091;
            bool disparoEvento = false;
            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.EnderecoIp = ipServidor;
            cliente.OidCronograma = "C1";
            cliente.Porta = porta;
            cliente.Login = "Joao";
            cliente.Conectar();
            cliente.AoSerExcluidaTarefaPorOutroUsuario += ( mensagemDto ) => { disparoEvento = true; };
            cliente.AoServidorDesconectar += ( mensagemDto ) => { };
            TcpClient tcpEmServidor = servidor.AcceptTcpClient();
            //interceptando  a mensagem de conexão
            TcpUtil.ReceberMensagemTcp( tcpEmServidor );
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T3", 3 );
            tarefasImpactadas.Add( "T4", 4 );
            MensagemDto mensagem = Mensagem.RnCriarMensagemComunicarExclusaoTarefaConcluida( new string[] { "T1", "T5" }, tarefasImpactadas, "C1", "Joao", DateUtil.CurrentDateTime, new string[] { "T2" } );
            string mensagemJson = JsonConvert.SerializeObject( mensagem );
            mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade( mensagemJson );
            //enviando mensagem ao cliente
            TcpUtil.EnviarMensagemTcp( mensagemJson, tcpEmServidor );
            ControleDeEsperaUtil.AguardarAte( () => { return disparoEvento; } );
            Assert.IsTrue( disparoEvento, "Deveria ter sido acionado o evento AoSerExcluidaTarefaPorOutroUsuario" );
            MensagemDto mensagemEsperada = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada ).FirstOrDefault();
            Assert.IsNotNull( mensagemEsperada, "Deveria ter sido recebida uma mensagem do tipo AoSerExcluidaTarefaPorOutroUsuario" );
            CollectionAssert.AreEqual( tarefasImpactadas, (Dictionary<string, Int16>)mensagemEsperada.Propriedades[Constantes.TAREFAS_IMPACTADAS] );
            CollectionAssert.AreEqual( new string[] { "T1", "T5" }, (string[])mensagemEsperada.Propriedades[Constantes.TAREFAS] );
            CollectionAssert.AreEqual( new string[] { "T2" }, (string[])mensagemEsperada.Propriedades[Constantes.TAREFAS_NAO_EXCLUIDAS] );
            Assert.AreEqual( "C1", (string)mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA] );
            Assert.AreEqual( "Joao", (string)mensagemEsperada.Propriedades[Constantes.AUTOR_ACAO] );
            Assert.AreEqual( DateUtil.CurrentDateTime, (DateTime)mensagemEsperada.Propriedades[Constantes.DATAHORA_ACAO] );
            cliente.RnDesconectar();

        }

        [TestMethod]
        public void RnComunicarFimExclusaoTarefaConcluidaTest()
        {
            DateUtil.CurrentDateTime = DateTime.Now;

            //inicializar o Dicionário tarefasGrid
            const string ipServidor = "127.0.0.1";
            const int porta = 8092;

            TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ), porta );
            servidor.Start();

            WexMultiAccessClientMock client = new WexMultiAccessClientMock();
            client.Porta = porta;
            client.EnderecoIp = ipServidor;
            client.Login = "Joao";
            client.OidCronograma = "C1";
            client.Conectar();
            client.AoServidorDesconectar += ( mensagemDto ) => { };
            TcpClient conexaoClienteNoServidor = servidor.AcceptTcpClient();
            //efetuar a leitura da autenticacao do client
            TcpUtil.ReceberMensagemTcp( conexaoClienteNoServidor );
            //representar as tarefas no grid
            Dictionary<string, Int16> tarefasGrid = new Dictionary<string, short>();
            //preencher as tarefas no grid
            //representar as tarefas excluidas
            string[] tarefasExcluidas = new string[] { "T1", "T2" };

            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            tarefasImpactadas.Add( "T3", 1 );
            tarefasImpactadas.Add( "T4", 2 );
            client.RnComunicarFimExclusaoTarefaConcluida( tarefasExcluidas, tarefasImpactadas, new string[] { }, DateUtil.CurrentDateTime );
            string msgJson = TcpUtil.ReceberMensagemTcp( conexaoClienteNoServidor );
            msgJson = TcpUtil.RemoverStringProtecaoDeIntegridade( msgJson );
            MensagemDto mensagemRecebida = Mensagem.DeserializarMensagemDto( msgJson );

            Assert.AreEqual( CsTipoMensagem.ExclusaoTarefaFinalizada, mensagemRecebida.Tipo, string.Format( "A mensagem deveria ser do tipo {0}", CsTipoMensagem.ExclusaoTarefaFinalizada ) );
            Assert.AreEqual( "C1", (string)mensagemRecebida.Propriedades[Constantes.OIDCRONOGRAMA], "O cronograma da mensagem deveria ser C1" );
            Assert.AreEqual( "Joao", (string)mensagemRecebida.Propriedades[Constantes.AUTOR_ACAO], "O login do colaborador que comunicou a exclusão da mensagem deveria ser Joao" );
            CollectionAssert.AreEquivalent( tarefasImpactadas, (Dictionary<string, Int16>)mensagemRecebida.Propriedades[Constantes.TAREFAS_IMPACTADAS], "Deveria possuir as " +
                "mesmas tarefas na ordem indicada da reordenação" );

            CollectionAssert.AreEquivalent( new string[] { }, (string[])mensagemRecebida.Propriedades[Constantes.TAREFAS_NAO_EXCLUIDAS] );
        }

        [TestMethod]
        public void RnComunicarAlteracaoNomeCronogramaTest()
        {
			//inicializar o Dicionário tarefasGrid
			const string ipServidor = "127.0.0.1";
			const int porta = 8093;

			TcpListener servidor = new TcpListener( IPAddress.Parse( ipServidor ) , porta );
			servidor.Start();

			WexMultiAccessClientMock client = new WexMultiAccessClientMock();
			client.Porta = porta;
			client.EnderecoIp = ipServidor;
			client.Login = "Joao";
			client.OidCronograma = "C1";
			client.Conectar();
			client.AoServidorDesconectar += ( mensagemDto ) => { };
			TcpClient conexaoClienteNoServidor = servidor.AcceptTcpClient();
			//efetuar a leitura da autenticacao do client
			TcpUtil.ReceberMensagemTcp( conexaoClienteNoServidor );
			client.RnComunicarAlteracaoDadosCronograma();
			ControleDeEsperaUtil.AguardarAte( () => { return conexaoClienteNoServidor.Available > 0; } );
			string mensagemJson = TcpUtil.ReceberMensagemTcp( conexaoClienteNoServidor );
			mensagemJson = TcpUtil.RemoverStringProtecaoDeIntegridade( mensagemJson );
			MensagemDto mensagemRecebida = JsonConvert.DeserializeObject<MensagemDto>( mensagemJson );
			Assert.AreEqual( client.Login , (string)mensagemRecebida.Propriedades[Constantes.AUTOR_ACAO] , "O nome do autor da ação deveria corresponder ao nome esperado" );
			Assert.AreEqual( client.OidCronograma , (string)mensagemRecebida.Propriedades[Constantes.OIDCRONOGRAMA] , "O oid do cronograma atual deveria corresponder ao oid atual" );
        }
    }
}
