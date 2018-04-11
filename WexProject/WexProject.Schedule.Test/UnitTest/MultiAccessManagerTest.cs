using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Domains;
using WexProject.MultiAccess.Library.Libs;
using System.Diagnostics;
using Moq;
using WexProject.Library.Libs.Test;
using Moq.Protected;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Test.Helpers.Domains;
using System.Threading;
using WexProject.Library.Libs.Extensions.Test;
namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class MultiAccessManagerTest
    {
        [TestMethod]
        [ExpectedException( typeof( ArgumentNullException ), "Deveria ter levantado a excecao argumento nulo." )]
        public void ConectarQuandoConexaoForNulaTest01()
        {
            WexMultiAccessManager manager = new WexMultiAccessManager() { EnderecoIp = null, Porta = 8000 };
            manager.Conectar();
        }


        [TestMethod]
        public void RnReceberConexaoQuandoCronogramaNaoExistirTest02()
        {
            WexMultiAccessManagerConexaoClientMock manager = new WexMultiAccessManagerConexaoClientMock() { EnderecoIp = "127.0.0.1", Porta = 8000 };
            manager.Conectar();
            TcpClient tcp = new TcpClient();
            const string oidCronograma = "C1";
            const string login = "gabriel.matos";
            tcp.Connect( IPAddress.Parse( "127.0.0.1" ), 8000 );
            manager.RnAceitarConexao( oidCronograma, login, tcp );
            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramas = manager.CronogramasConectados;
            Assert.IsNotNull( cronogramas[oidCronograma], "Cronogramas não deve ser nulo após o recebimento da conexão" );
            Assert.IsTrue( cronogramas[oidCronograma].ContainsKey( login ), "Deveria encontrar o login inserido em algum indice do Dicionario de cronogramas" );
            Assert.AreEqual( tcp, cronogramas[oidCronograma][login].TcpCliente, "Deveria ter encontrado o mesmo Objeto TcpClient" );
            manager.Desconectar();
            tcp.Close();
        }


        [TestMethod]
        public void RnReceberConexaoQuandoMaisDeUmUsuarioConectarAoMesmoCronogramaTest03()
        {
            const int porta = 8015;
            WexMultiAccessManagerConexaoClientMock manager = new WexMultiAccessManagerConexaoClientMock() { EnderecoIp = "127.0.0.1", Porta = porta };
            manager.Conectar();
            // Primeiro Colaborador
            TcpClient tcp = new TcpClient();
            const string oidCronograma = "C1";
            const string login = "gabriel.matos";
            tcp.Connect( IPAddress.Parse( "127.0.0.1" ), porta );
            manager.RnAceitarConexao( oidCronograma, login, tcp );

            //Segundo Colaborador
            TcpClient tcp2 = new TcpClient();
            const string login2 = "anderson.lins";
            tcp2.Connect( IPAddress.Parse( "127.0.0.1" ), porta );
            manager.RnAceitarConexao( oidCronograma, login2, tcp2 );

            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramas = manager.CronogramasConectados;
            //Colaborador 1
            Assert.IsNotNull( cronogramas[oidCronograma], "Cronogramas não deve ser nulo após o recebimento da conexão" );
            Assert.IsTrue( cronogramas[oidCronograma].ContainsKey( login ), "Deveria encontrar o login inserido em algum indice do Dicionario de cronogramas" );
            Assert.AreEqual( tcp, cronogramas[oidCronograma][login].TcpCliente, "Deveria ter encontrado o mesmo Objeto TcpClient" );
            //Colaborador 2
            Assert.IsNotNull( cronogramas[oidCronograma], "Cronogramas não deve ser nulo após o recebimento da conexão" );
            Assert.IsTrue( cronogramas[oidCronograma].ContainsKey( login2 ), "Deveria encontrar o login inserido em algum indice do Dicionario de cronogramas" );
            Assert.AreEqual( tcp2, cronogramas[oidCronograma][login2].TcpCliente, "Deveria ter encontrado o mesmo Objeto TcpClient" );
            tcp.Close();
            tcp2.Close();
            cronogramas[oidCronograma][login].TcpCliente.Close();
            cronogramas[oidCronograma][login2].TcpCliente.Close();
            manager.Desconectar();
        }


        [TestMethod]
        public void RnReceberConexarQuandoUsuarioSeConectarAMaisDeUmCronogramaTest04()
        {
            const int porta = 8002;
            WexMultiAccessManagerConexaoClientMock manager = new WexMultiAccessManagerConexaoClientMock() { EnderecoIp = "127.0.0.1", Porta = porta };
            manager.Conectar();

            //Colaborador
            TcpClient tcp = new TcpClient();
            const string login = "gabriel.matos";
            tcp.Connect( IPAddress.Parse( "127.0.0.1" ), porta );
            //Cronograma C1
            const string oidCronograma = "C1";
            manager.RnAceitarConexao( oidCronograma, login, tcp );
            //Cronograma C2
            const string oidCronograma2 = "C2";
            manager.RnAceitarConexao( oidCronograma2, login, tcp );

            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramas = manager.CronogramasConectados;

            //Colaborador no Cronograma 1
            Assert.IsNotNull( cronogramas[oidCronograma], "Cronogramas não deve ser nulo após o recebimento da conexão" );
            Assert.IsTrue( cronogramas[oidCronograma].ContainsKey( login ), "Deveria encontrar o login inserido em algum indice do Dicionario do cronograma C1" );
            Assert.AreEqual( tcp, cronogramas[oidCronograma][login].TcpCliente, "Deveria ter encontrado o mesmo Objeto TcpClient No Dicionário do Cronograma C1" );

            //Colaborador no Cronograma 
            Assert.IsNotNull( cronogramas[oidCronograma2], "Cronogramas não deve ser nulo após o recebimento da conexão" );
            Assert.IsTrue( cronogramas[oidCronograma2].ContainsKey( login ), "Deveria encontrar o login inserido em algum indice do Dicionario do cronograma C2" );
            Assert.AreEqual( tcp, cronogramas[oidCronograma2][login].TcpCliente, "Deveria ter encontrado o mesmo Objeto TcpClient No Dicionário do Cronograma C2" );

            tcp.Close();
            manager.Desconectar();

        }


        [TestMethod]
        public void RnReceberConexarQuandoMesmoUsuarioSeConectarAoMesmoCronogramaDuasVezesComTcpDiferenteTest05()
        {
            #region Cenário
            const int porta = 8003;
            WexMultiAccessManagerConexaoClientMock manager = new WexMultiAccessManagerConexaoClientMock() { EnderecoIp = "127.0.0.1", Porta = porta };
            manager.Conectar();

            //Colaborador
            TcpClient tcp = new TcpClient();
            const string login = "gabriel.matos";
            const string oidCronograma = "C1";
            tcp.Connect( IPAddress.Parse( "127.0.0.1" ), porta );
            //TCP 1
            manager.RnAceitarConexao( oidCronograma, login, tcp );
            //TCP 2
            TcpClient tcpNovo = new TcpClient();
            tcpNovo.Connect( IPAddress.Parse( "127.0.0.1" ), porta );
            manager.RnAceitarConexao( oidCronograma, login, tcpNovo );

            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramas = manager.CronogramasConectados;

            #endregion

            #region Testes
            //Verificar se a conexão ultima tcp passada é igual a conexão atual do cliente
            Assert.AreEqual( tcpNovo, cronogramas[oidCronograma][login].TcpCliente );
            // Verificar se a conexão antiga é não é a mesma conexão atual
            Assert.AreNotEqual( tcp, tcpNovo );
            #endregion

            #region Finalizando Conexões
            tcp.Close();
            tcpNovo.Close();
            cronogramas[oidCronograma][login].TcpCliente.Close();
            manager.Desconectar();
            #endregion

        }


        [TestMethod]
        public void RnAguardarIdentificaoQuandoOClienteResponderTest06()
        {
            /*
             Estimativa de  Teste:
             * Aguardar identificação deve receber a conexão efetuar seus procedimentos e incluir na hash a conexão aceita
             * Cenário: Aguardar Identificação vai receber um tcp que possui uma mensagem de novo usuário conectado
             *  - instanciar um manager
             *  - conectar o WexMultiAccessManager
             *  - instanciar um WexMultiAccessClient
             *  - conectar WexMultiAccessClient no manager
             *  - chamar WexMultiAccessManager.AguardarIdentificação
             * Resultado estimado:
             *  - Deve receber a conexão e  aceitar a identificação
             * Testes:
             *  - Verificar se cronogramasConectados possuem o colaborador identificado em sua hash
             *   
             */

            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Setup( o => o.ManterAtendimento() );
            managerMock.Setup( o => o.RnProcessarEventos() );
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8004;
            manager.Conectar();

            //Configurar Cliente
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { Porta = manager.Porta, EnderecoIp = manager.EnderecoIp, Login = "Joao", OidCronograma = "C1" };
            cliente.Conectar();
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            cliente.AoUsuarioDesconectar += ( mensagem ) => { };
            cliente.AoServidorDesconectar += ( mensagem ) => { };
            //Manager recebendo o tcp escutado
            TcpClient tcp = manager.ConexaoAtiva.AcceptTcpClient();
            manager.RnAguardarIdentificacao( tcp );

            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login ), "O cliente deveria existir na hash de usuariosConectados" );
            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login, cliente.OidCronograma ), "O cliente deveria existir na hash de cronogramas conectados C1" );
            manager.Desconectar();
            cliente.RnDesconectar();
            tcp.Close();
        }


        [TestMethod]
        public void RnAguardarIdentificaoQuandoOClienteResponderComOEventoErradoTest06()
        {
            /*
             *Estimativa de teste:
             * O manager aceita a conexão aguarda identificação porém recebe outra mensagem
             * que não seja de identificação de novo usuário e deve rejeitar enviando uma mensagem
             * de AoRecusarConexaoServidor
             * Cenário:
             *  - Instanciar e Conectar o Manager
             *  - Mockar IniciaAtendimento e ManterAtendimento
             *  - Conectar um tcp e enviar uma mensagem de outro tipo que não seja de conectarNovoUsuario
             *  - AguardarIdentificação recebendo um tcp que receberá a mensagem
             *  - o tcp deve receber de volta uma mensagem de recusaConexãoServidor
             */

            int contarRecusa = 0;
            Mock<WexMultiAccessManager> managerMock = new Mock<WexMultiAccessManager>() { CallBase = true };
            //MockandO RnComunicarRecusaConexao com um contador caso ele execute o envio de recusa
            managerMock.Protected().Setup( "RnComunicarRecusaConexao", ItExpr.IsAny<string>(), ItExpr.IsAny<TcpClient>() ).Callback( () =>
            {
                contarRecusa++;
            } );
            //Mock ManterAtendimento Evitar a threadManterAtendimento
            managerMock.Setup( o => o.ManterAtendimento() );
            managerMock.Setup( o => o.RnProcessarEventos() );
            WexMultiAccessManager manager = managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8005;
            manager.Conectar();
            // manager.TempoMaximoAguardarIdentificacao = 50;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            //mockando método AcionarEventoAoServidorRecusarConexao evitar disparo do evento
            clientMock.Protected().Setup( "AcionarEventoAoServidorRecusarConexao", ( ItExpr.IsAny<MensagemDto>() ) );
            clientMock.Setup( o => o.RnEnviarMensagemIdentificao() );
            //clientMock.Protected().Setup( "RnProcessarEventos" );
            // instanciando o cliente com Método mockado
            WexMultiAccessClientMock cliente = clientMock.Object;
            //suprimindo o evento com método que não faz nada
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            cliente.AoUsuarioDesconectar += ( mensagem ) => { };
            cliente.OidCronograma = "C1";
            cliente.Login = "Joao";
            cliente.Porta = manager.Porta;
            cliente.EnderecoIp = manager.EnderecoIp;
            // cliente.Conectar();
            TcpClient tcpCliente = new TcpClient();
            tcpCliente.Connect( IPAddress.Parse( "127.0.0.1" ), manager.Porta );
            TcpClient tcp = manager.ConexaoAtiva.AcceptTcpClient();
            //cliente.TcpCliente = tcpCliente;
            manager.RnReceberConexao( tcp );
            TcpUtil.EnviarMensagemTcp( Mensagem.Serializar( Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { cliente.Login }, cliente.OidCronograma ) ), tcpCliente );
            Assert.AreEqual( 1, contarRecusa, "Evento de Recusa de Conexão Deveria ser acionado após" );
            managerMock.Protected().Verify( "RnComunicarRecusaConexao", Times.Once(), ItExpr.IsAny<string>(), ItExpr.IsAny<TcpClient>() );
            tcp.Close();
            cliente.RnDesconectar();
            manager.Desconectar();
        }


        [TestMethod]
        public void RnReceberConexaoQuandoOClienteNaoResponderAntesDoTempoLimiteTest07()
        {
            #region Cenário
            int contadorDeRecusasConexao = 0;
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            // Mockando método ManterAtendimento do MultiAccessManager para que o método não execute nada
            //managerMock.Setup( o => o.ManterAtendimento() );
            managerMock.Setup( o => o.IniciaAtendimento() );
            managerMock.Setup( o => o.RnProcessarEventos() );
            managerMock.Protected().Setup( "RnComunicarRecusaConexao", ItExpr.IsAny<string>(), ItExpr.IsAny<TcpClient>() ).Callback( () => { contadorDeRecusasConexao++; } );
            //Passando Ao manager a instância mockada
            WexMultiAccessManagerMock manager = (WexMultiAccessManagerMock)managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8006;
            manager.Conectar();
            //Recriando um AccessClient mockando o Evento de recusa do servidor
            Mock<WexMultiAccessClient> clienteMock = new Mock<WexMultiAccessClient>() { CallBase = true };
            clienteMock.Protected().Setup( "AcionarEventoAoServidorRecusarConexao", ( ItExpr.IsAny<MensagemDto>() ) );
            WexMultiAccessClient accessCliente = clienteMock.Object;
            accessCliente.Login = "Joao";
            accessCliente.OidCronograma = "C1";
            accessCliente.EnderecoIp = manager.EnderecoIp;
            accessCliente.Porta = manager.Porta;
            accessCliente.Conectar();
            accessCliente.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            manager.TempoMaximoAguardarIdentificacao = 1;
            manager.RnAguardarConexao();

            ControleDeEsperaUtil.AguardarAte( () => { return contadorDeRecusasConexao > 0; } );
            #endregion

            Assert.AreEqual( 1, contadorDeRecusasConexao, "Deveria recusar ao menos 1 vez a tentativa de conexao" );
            Assert.IsFalse( manager.ColaboradorConectado( accessCliente.Login ), "Usuário não deveria estar conectado!" );
            Assert.AreEqual( 0, manager.UsuariosConectados.Count, "Não deve possuir usuário conectado!" );

            #region Finalizando Conexões

            accessCliente.RnDesconectar();
            manager.Desconectar();
            #endregion
        }


        [TestMethod]
        public void RnAguardarConexaoQuandoReceberMultiplasConexoesEnfileirarMensagensDosClientesTest08()
        {
            const int managerPorta = 8007;
            int contadorRecusas = 0;
            Mock<WexMultiAccessManagerConexaoClientMock> managerMock = new Mock<WexMultiAccessManagerConexaoClientMock>() { CallBase = true };
            managerMock.Setup( o => o.ManterAtendimento() );
            managerMock.Protected().Setup( "RnComunicarRecusaConexao", ItExpr.IsAny<string>(), ItExpr.IsAny<TcpClient>() ).Callback( () => { contadorRecusas++; } );
            WexMultiAccessManagerConexaoClientMock manager = managerMock.Object;
            manager.TempoMaximoAguardarIdentificacao = 3000;
            manager.EstadoConexaoCliente = CsEstadoConexaoCliente.Leitura_E_Escrita_Desativadas;
            manager.StatusServidor = false;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = managerPorta;
            manager.Conectar();
            List<Hashtable> usuariosOnlineHash = null;
            Hashtable informacoes;
            manager.AoEnviarConfirmacaoConexao += ( usuarios, cronograma ) =>
            {
                if(usuariosOnlineHash == null)
                    usuariosOnlineHash = new List<Hashtable>();
                informacoes = new Hashtable();
                informacoes.Add( "cronograma", cronograma );
                informacoes.Add( "usuarios", usuarios );
                usuariosOnlineHash.Add( informacoes );
            };
            string[] listaOnlineMaria = new string[] { "Joao" };
            string[] listaOnlineJose = new string[] { "Joao", "Maria" };

            //Instanciar três Clients
            WexMultiAccessClient clientC1Joao = new WexMultiAccessClient() { EnderecoIp = manager.EnderecoIp, Porta = manager.Porta, OidCronograma = "C1", Login = "Joao" };
            WexMultiAccessClient clientC2Pedro = new WexMultiAccessClient() { EnderecoIp = manager.EnderecoIp, Porta = manager.Porta, OidCronograma = "C2", Login = "Pedro" };
            WexMultiAccessClient clientC1Maria = new WexMultiAccessClient() { EnderecoIp = manager.EnderecoIp, Porta = manager.Porta, OidCronograma = "C1", Login = "Maria" };
            WexMultiAccessClient clientC1Jose = new WexMultiAccessClient() { EnderecoIp = manager.EnderecoIp, Porta = manager.Porta, OidCronograma = "C1", Login = "Jose" };
            clientC1Joao.AoConectarNovoUsuario += ( Mensagem, login ) => { };
            clientC1Joao.AoUsuarioDesconectar += ( mensagem ) => { };
            clientC1Joao.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            clientC2Pedro.AoConectarNovoUsuario += ( Mensagem, login ) => { };
            clientC2Pedro.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            clientC2Pedro.AoUsuarioDesconectar += ( mensagem ) => { };
            clientC1Maria.AoConectarNovoUsuario += ( Mensagem, login ) => { };
            clientC1Maria.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            clientC1Maria.AoUsuarioDesconectar += ( mensagem ) => { };
            clientC1Jose.AoConectarNovoUsuario += ( Mensagem, login ) => { };
            clientC1Jose.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            clientC1Jose.AoUsuarioDesconectar += ( mensagem ) => { };

            //Efetuar Conexão dos Clients
            //Client 1
            clientC1Joao.Conectar();
            manager.RnAguardarConexao();
            //Client 2

            clientC2Pedro.Conectar();
            manager.RnAguardarConexao();

            //Client3
            clientC1Maria.Conectar();
            manager.RnAguardarConexao();

            //Client4
            clientC1Jose.Conectar();
            manager.RnAguardarConexao();

            string[] listaEsperada1 = (string[])usuariosOnlineHash.ElementAt( 2 )["usuarios"];
            string[] listaEsperada2 = (string[])usuariosOnlineHash.ElementAt( 3 )["usuarios"];
            Assert.IsTrue( listaOnlineMaria.SequenceEqual( listaEsperada1 ), "Maria Deveria possuir a mesma lista de usuarios online no momento (Somente o Joao Online)" );
            Assert.IsTrue( listaOnlineJose.SequenceEqual( listaEsperada2 ), "Deveria possuir a mesmas lista de usuarios online no momento (Joao e Maria Online)" );
            //Client1
            //Aguardar o client1 estar conectado no cronograma
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( clientC1Joao.Login, clientC1Joao.OidCronograma ); }, 5 );
            //Testar se o client1 foi Conectado no Cronograma Determinado
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Joao.Login, clientC1Joao.OidCronograma ), string.Format( "O colaborador {0} deveria estar conectado no cronograma {1}", clientC1Joao.Login, clientC1Joao.OidCronograma ) );
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Joao.Login ), string.Format( "O usuario de login {0} deveria ser encontrado na hash de usuarios conectados", clientC1Joao.Login ) );
            //Client2
            //Aguardar o client2 estar conectado no cronograma
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( clientC2Pedro.Login, clientC2Pedro.OidCronograma ); }, 5 );

            //Testar se o client1 foi Conectado no Cronograma Determinado
            Assert.IsTrue( manager.ColaboradorConectado( clientC2Pedro.Login, clientC2Pedro.OidCronograma ), string.Format( "O colaborador {0} deveria estar conectado no cronograma {1}", clientC2Pedro.Login, clientC2Pedro.OidCronograma ) );
            Assert.IsTrue( manager.ColaboradorConectado( clientC2Pedro.Login ), string.Format( "O usuario de login {0} deveria ser encontrado na hash de usuarios conectados", clientC2Pedro.Login ) );

            //Client3
            //Aguardar o client3 estar conectado no cronograma
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( clientC1Maria.Login, clientC1Maria.OidCronograma ); }, 5 );
            //Testar se o client1 foi Conectado no Cronograma Determinado
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Maria.Login, clientC1Maria.OidCronograma ), string.Format( "O colaborador {0} deveria estar conectado no cronograma {1}", clientC1Maria.Login, clientC1Maria.OidCronograma ) );
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Maria.Login ), string.Format( "O usuario de login {0} deveria ser encontrado na hash de usuarios conectados", clientC1Maria.Login ) );

            //Client4
            //Aguardar o client4 estar conectado no cronograma
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( clientC1Jose.Login, clientC1Jose.OidCronograma ); }, 5 );
            //Testar se o client1 foi Conectado no Cronograma Determinado
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Jose.Login, clientC1Jose.OidCronograma ), string.Format( "O colaborador {0} deveria estar conectado no cronograma {1}", clientC1Jose.Login, clientC1Jose.OidCronograma ) );
            //Testar numero atual de clientes conectados
            Assert.AreEqual( 4, manager.UsuariosConectados.Count, "Deveria conter a mesma quantidade de colaboradores diferentes que efetuaram conexão" );
            //Testar numero atual de clientes conectados
            Assert.IsTrue( manager.ColaboradorConectado( clientC1Jose.Login ), string.Format( "O usuario de login {0} deveria ser encontrado na hash de usuarios conectados", clientC1Jose.Login ) );


            manager.ListarMensagensDeEscritaConexoesCliente();
            //Testar se há 3 mensagens na fila de escrita da conexãoCliente do Joao 
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados["C1"]["Joao"].FilaEscrita.Count == 3; } );
            Assert.AreEqual( 3, manager.CronogramasConectados["C1"]["Joao"].FilaEscrita.Count, "Deveria haver 3 mensagens na FilaEscrita da conexão do João, incluindo ele mesmo pois, ele sera removido somente antes de transmitir a comunicao." );
            //Testar se há 2 mensagens na fila de escrita da conexãoCliente da Maria
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados["C1"]["Maria"].FilaEscrita.Count >= 1; } );
            Assert.AreEqual( 2, manager.CronogramasConectados["C1"]["Maria"].FilaEscrita.Count, "Deveria haver 2 mensagens pois Maria entrou depois de João" );
            //Testar se há 1 mensagem na fila de escrita da conexãoCliente do Pedro
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados["C2"]["Pedro"].FilaEscrita.Count == 3; } );
            Assert.AreEqual( 1, manager.CronogramasConectados["C2"]["Pedro"].FilaEscrita.Count, "Deveria haver 1 mensagem na FilaEscrita da conexão do Pedro" );
            //Testar se há 1 mensagem na fila de escrita da conexãoCliente do Pedro 
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados["C1"]["Jose"].FilaEscrita.Count == 3; } );
            Assert.AreEqual( 1, manager.CronogramasConectados["C1"]["Jose"].FilaEscrita.Count, "Deveria haver 1 mensagem na FilaEscrita da conexão do Jose" );
            Assert.AreEqual( 0, contadorRecusas, "Não deveria ter recusado nenhuma conexão" );
            clientC1Joao.RnDesconectar();
            clientC2Pedro.RnDesconectar();
            clientC1Maria.RnDesconectar();
            clientC1Jose.RnDesconectar();
            manager.Desconectar();
            Debug.WriteLine( "Fim do teste" );
        }


        [TestMethod]
        public void RnAguardarConexaoQuandoOMesmoUsuarioSeConectarMaisDeUmaVezTest09()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Setup( o => o.RnProcessarEventos() );
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8008;
            manager.Conectar();

            //Configurar Cliente
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { Porta = manager.Porta, EnderecoIp = manager.EnderecoIp, Login = "Joao", OidCronograma = "C1" };
            cliente.Conectar();
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            cliente.AoServidorDesconectar += ( mensagem ) => { };
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( cliente.Login ); } );
            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login ), "O cliente deveria existir na hash de usuariosConectados" );
            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login, cliente.OidCronograma ), "O cliente deveria existir na hash de cronogramas conectados C1" );
            cliente.Conectar();
            //aguardar 2 segundos
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados[cliente.OidCronograma].Count == 1; } );
            Assert.AreEqual( 1, manager.CronogramasConectados[cliente.OidCronograma].Count, "Deveria ter somente um usuário na hash do cronograma C1" );
            manager.Desconectar();
            cliente.RnDesconectar();
        }

        [TestMethod]
        public void RnAguardarConexaoQuandoOMesmoUsuarioSeConectarMaisDeUmaVezTest()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Setup( o => o.RnProcessarEventos() );
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8008;
            manager.Conectar();

            //Configurar Cliente
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { Porta = manager.Porta, EnderecoIp = manager.EnderecoIp, Login = "Joao", OidCronograma = "C1" };
            cliente.Conectar();
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) => { };
            cliente.AoServidorDesconectar += ( mensagem ) => { };
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( cliente.Login ); } );
            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login ), "O cliente deveria existir na hash de usuariosConectados" );
            Assert.IsTrue( manager.ColaboradorConectado( cliente.Login, cliente.OidCronograma ), "O cliente deveria existir na hash de cronogramas conectados C1" );
            cliente.Conectar();
            //aguardar 2 segundos
            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados[cliente.OidCronograma].Count == 1; } );
            Assert.AreEqual( 1, manager.CronogramasConectados[cliente.OidCronograma].Count, "Deveria ter somente um usuário na hash do cronograma C1" );
            cliente.RnDesconectar();
            manager.Desconectar();
        }

        [TestMethod]
        public void RnAguardarConexaoQuandoVariosUsuariosFicaremOnlineTest10()
        {
            #region Cenário
            const int tempoDeTeste = 3000;
            const int managerPorta = 8009;
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock()
            {
                EnderecoIp = "127.0.0.1",
                Porta = managerPorta,
                TempoMaximoAguardarIdentificacao = tempoDeTeste
            };
            manager.Conectar();
            Mock<WexMultiAccessClient> clientMock1 = new Mock<WexMultiAccessClient>() { CallBase = true };
            clientMock1.Protected().Setup( "RnProcessarEventos" );

            Mock<WexMultiAccessClient> clientMock2 = new Mock<WexMultiAccessClient>() { CallBase = true };
            clientMock2.Protected().Setup( "RnProcessarEventos" );

            Mock<WexMultiAccessClient> clientMock3 = new Mock<WexMultiAccessClient>() { CallBase = true };
            clientMock3.Protected().Setup( "RnProcessarEventos" );

            Mock<WexMultiAccessClient> clientMock4 = new Mock<WexMultiAccessClient>() { CallBase = true };
            clientMock4.Protected().Setup( "RnProcessarEventos" );

            Mock<WexMultiAccessClient> clientMock5 = new Mock<WexMultiAccessClient>() { CallBase = true };
            clientMock5.Protected().Setup( "RnProcessarEventos" );

            WexMultiAccessClient client1 = clientMock1.Object;
            WexMultiAccessClient client2 = clientMock2.Object;
            WexMultiAccessClient client3 = clientMock3.Object;
            WexMultiAccessClient client4 = clientMock4.Object;
            WexMultiAccessClient client5 = clientMock5.Object;
            //Configurar Clients para Servidor
            //Client1 Config
            client1.EnderecoIp = manager.EnderecoIp;
            client1.Porta = manager.Porta;
            client1.OidCronograma = "C1";
            client1.Login = "Joao";
            client1.AoSerAutenticadoComSucesso += ( mensagem ) => { };

            //Client 2 Config
            client2.EnderecoIp = manager.EnderecoIp;
            client2.Porta = manager.Porta;
            client2.OidCronograma = "C2";
            client2.Login = "Pedro";
            client2.AoSerAutenticadoComSucesso += ( mensagem ) => { };

            //Client 3 Config
            client3.EnderecoIp = manager.EnderecoIp;
            client3.Porta = manager.Porta;
            client3.OidCronograma = "C1";
            client3.Login = "Maria";
            client3.AoSerAutenticadoComSucesso += ( mensagem ) => { };

            //Client 4 Config
            client4.EnderecoIp = manager.EnderecoIp;
            client4.Porta = manager.Porta;
            client4.OidCronograma = "C1";
            client4.Login = "Joana";
            client4.AoSerAutenticadoComSucesso += ( mensagem ) => { };

            //Client 5 Config
            client5.EnderecoIp = manager.EnderecoIp;
            client5.Porta = manager.Porta;
            client5.OidCronograma = "C1";
            client5.Login = "Jandira";
            client5.AoSerAutenticadoComSucesso += ( mensagem ) => { };

            WexMultiAccessClient[] clients = new WexMultiAccessClient[] { client1, client2, client3, client4, client5 };

            for(int i = 0; i < clients.Length; i++)
            {
                //Efetuar Conexão dos Clients
                clients[i].Conectar();
            }
            List<string> listaLogins = new List<string>( clients.Select( o => o.Login ) );
            ControleDeEsperaUtil.AguardarAte( () => { return manager.UsuariosConectados.Count == clients.Length; } );
            #endregion

            #region Testes
            Assert.AreEqual( 5, manager.UsuariosConectados.Count, "Deveria conter 5 usuarios conectados!" );
            Assert.AreEqual( 2, manager.CronogramasConectados.Count, "Deveria conter 2 cronogramas com usarios conectados" );
            List<string> listaLoginsConectados = manager.UsuariosConectados.Select( o => o.Key ).ToList();
            CollectionAssert.AreEquivalent( listaLogins, listaLoginsConectados, "Deveriam conter os mesmos nomes" );
            #endregion

            #region Finalizando Conexões
            client1.RnDesconectar();
            client2.RnDesconectar();
            client3.RnDesconectar();
            client4.RnDesconectar();
            client5.RnDesconectar();
            manager.Desconectar();
            #endregion
        }


        [TestMethod]
        public void RnComunicarUsuarioNovoConectadoQuandoVariosUsuariosConectaremTest11()
        {
            #region Estimativa do Teste
            /*
             * Estimativa do teste:
             * - É esperado que ao termino do teste ele tenha enfileirado corretamente as mensagens na FilaProcessamento
             * Cenário:
             * - Dado que 3 Pessoas se conectaram, ele deve enfileirar as 3 mensagens na FilaProcessamento
             * Resultado estimado:
             * - Que estejam contidas 2 mensagens na FilaProcessamento
             * - Que a mensagem possua as propriedades: 'oidCronograma' e 'login' preenchidos corretamente e que o tipo da mensagem seja o Tipo determinado
             */
            #endregion

            #region Cenário
            const string oidCronograma = "C1";
            string[] colaboradores = new string[] { "Joao", "Pedro", "Paulo" };
            MensagemDto objetoMensagem;
            Queue<MensagemDto> filaEsperada = new Queue<MensagemDto>();
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();
            foreach(var colaborador in colaboradores)
            {
                manager.RnComunicarUsuarioNovoConectado( colaborador, oidCronograma );
                objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( new string[] { colaborador }, oidCronograma );
                filaEsperada.Enqueue( objetoMensagem );
            }
            #endregion

            Assert.ReferenceEquals( filaEsperada, manager.FilaProcessamento );
        }


        [TestMethod]
        public void RnResumirMensangensQuandoMaisDeUmColaboradorSeConectarSimultaneamenteEmUmCronogramaTest12()
        {
            #region Estimativa do Teste
            /*
             * Acontecimento Estimado:
             * Neste teste haverá 3 colaboradores se conectando ao cronograma, deverá ser feita um sintetização das mensagens de todos usuários
             * que se conectaram.
             * Cenário:
             * - Conectar os usuários:'Joao','Pedro' e'Paulo' no crograma 'C1'
             * - Enfileirar os Eventos de NovosUsuáriosConectados
             * - Acionar RnResumirMensagens para sintetizar a fila de eventos NovoUsuariosConectados
             * Resultado Estimado:
             * - Receber 3 objetos MensagemDto e retornar Apenas 1 objeto MensagemDto(Sintetizado)             
             */
            #endregion

            const string oidCronograma = "C1";
            string[] colaboradores = new string[] { "Joao", "Pedro", "Paulo", "Julia" };
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();
            MensagemDto objetoMensagem, objetoMensagemEsperado;
            List<MensagemDto> listaMensagens = new List<MensagemDto>();

            for(int i = 0; i < colaboradores.Length; i++)
            {
                objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( new string[] { colaboradores[i] }, oidCronograma );
                listaMensagens.Add( objetoMensagem );
            }

            List<MensagemDto> listaResumida = manager.RnResumirMensagensPublicado( listaMensagens );

            //Objeto esperado
            objetoMensagemEsperado = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaboradores, oidCronograma );

            Assert.AreEqual( 1, listaResumida.Count, "Deveria por uma unica MensagemDto sintetizada" );
            MensagemDto objetoAtual = listaResumida.First();

            Assert.AreEqual( objetoMensagemEsperado.Propriedades["oidCronograma"], objetoAtual.Propriedades["oidCronograma"], "O cronograma deveria ser o mesmo" );
            Assert.IsTrue( colaboradores.SequenceEqual( (String[])objetoAtual.Propriedades["usuarios"] ), "Deveria ter  unificado os usuaarios em uma unica mensagem" );
            Assert.AreEqual( 1, listaResumida.Count, "Deveria ter resumido todas as mensagens em apenas 1 pois todas possuem o mesmo tipo e cronograma." );

        }


        [TestMethod]
        public void RnResumirMensangensQuandoMaisDeUmColaboradorSeConectarSimultaneamenteEmMaisDeUmCronogramaTest13()
        {
            #region Estimativa do Teste
            /*
             * Acontecimento Estimado:
             * Neste teste haverá 3 colaboradores se conectando ao cronograma 'C1' e 3 colaboradores se conectando ao Cronograma 'C2', deverá ser feita um sintetização das mensagens de todos usuários
             * que se conectaram.
             * Cenário:
             * - Conectar os usuários:'Joao','Pedro' e'Paulo' no crograma 'C1'
             * - Enfileirar os Eventos de NovosUsuáriosConectados
             * - Acionar RnResumirMensagens para sintetizar a fila de eventos NovoUsuariosConectados
             * Resultado Estimado:
             * - Receber 3 objetos MensagemDto e retornar Apenas 1 objeto MensagemDto(Sintetizado)             
             */
            #endregion

            #region Cenário
            const string OIDCRONOGRAMA = "oidCronograma";
            const string oidCronograma = "C1";
            string[] colaboradores = new string[] { "Joao", "Pedro", "Paulo", "Julia" };
            const string oidCronograma2 = "C2";
            string[] colaboradores2 = new string[] { "Mario", "Paula", "Andreza", "Adriana" };
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();
            Hashtable propriedadesMensagem = new Hashtable();
            MensagemDto objetoMensagem, objetoMensagemEsperado;
            List<MensagemDto> listaMensagens = new List<MensagemDto>();
            List<MensagemDto> listaMensagensEsperada = new List<MensagemDto>();

            //Criando Mensagens para o cronograma C1
            foreach(string colaborador in colaboradores)
            {
                objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaborador, oidCronograma );
                listaMensagens.Add( objetoMensagem );
            }
            //Criando Mensagens usuários conectados para o cronograma C2
            foreach(string colaborador in colaboradores2)
            {
                objetoMensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaborador, oidCronograma2 );
                listaMensagens.Add( objetoMensagem );
            }
            //Criando 2 Mensagens usuários desconectados para o cronograma C1 
            objetoMensagem = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Joao" }, oidCronograma );
            listaMensagens.Add( objetoMensagem );
            objetoMensagem = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Pedro" }, oidCronograma );
            listaMensagens.Add( objetoMensagem );
            //Criando 2 Mensagens usuários desconectados para o cronograma C2 
            objetoMensagem = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Mario" }, oidCronograma2 );
            listaMensagens.Add( objetoMensagem );
            objetoMensagem = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Paula" }, oidCronograma2 );
            listaMensagens.Add( objetoMensagem );


            //Recebendo a lista de mensagens sintetizadas
            List<MensagemDto> listaResumida = manager.RnResumirMensagensPublicado( listaMensagens );
            //Configurando MensagemDto sintetizada para o cronograma C1
            objetoMensagemEsperado = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaboradores, oidCronograma );
            listaMensagensEsperada.Add( objetoMensagemEsperado );
            //Configurando MensagemDto sintetizada para o cronograma 
            objetoMensagemEsperado = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaboradores2, oidCronograma2 );
            listaMensagensEsperada.Add( objetoMensagemEsperado );
            objetoMensagemEsperado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Joao", "Pedro" }, oidCronograma );
            listaMensagensEsperada.Add( objetoMensagemEsperado );
            objetoMensagemEsperado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Mario", "Paula" }, oidCronograma );
            listaMensagensEsperada.Add( objetoMensagemEsperado );
            #endregion

            #region Testes
            //Teste o Numero de mensagens resumidas que retornaram
            Assert.AreEqual( listaMensagensEsperada.Count, listaResumida.Count, "Deveria possuir a mesma quantidade de mensagens da lista esperada" );
            //recebendo primeira mensagem da lista resumida
            MensagemDto objetoAtual = listaResumida.First();
            //recebendo primera mensagem da lista esperada
            objetoMensagemEsperado = listaMensagensEsperada.First();
            //Testando se iguais o cronograma indicado na mensagem
            Assert.AreEqual( objetoMensagemEsperado.Propriedades[OIDCRONOGRAMA], objetoAtual.Propriedades[OIDCRONOGRAMA], "O cronograma deveria ser o mesmo" );
            //checando se a sequencia de usuários do vetor de usuarios do cronograma C1 é a mesma sequencia retornada na mensagem da lista resumida
            Assert.IsTrue( colaboradores.SequenceEqual( (String[])objetoAtual.Propriedades["usuarios"] ), "Deveria ter  unificado os usuarios em uma unica mensagem" );
            Assert.AreEqual( 4, listaResumida.Count, "Deveria ter resumido todas as mensagens em apenas 4 sintetizadas por tipo de mensagem e cronograma" );
            #endregion

        }

        [TestMethod]
        public void RnProcessarEventosEnfileirarEventosRecebidosTest14()
        {
            #region Estimativa do Teste
            /*
         * Acontecimento Estimado:
         * O RnProcessarEventos deve receber as mensagens e repassar para a Conexão cliente devida.
         * Cenário:
         *  - Fila preenchida com várias Mensagens
         *  - Instanciar de 2 ConexoesCliente para receber as suas especificas mensagens
         *  - Acionar RnProcessarEventos para preencher a filaLeitura de ConexaoCliente
         * Resultado Esperado:
         *  - RnProcessar eventos deve enfileirar em cada uma ConexaoCliente suas respectivas mensagens resumidas
         *  - Cada ConexaoCliente deve ter uma fila de mensagens com as respectivas mensagens
         */
            #endregion

            const string oidCronograma = "C1";
            string[] colaboradores = new string[] { "Joao", "Pedro", "Paulo", "Julia" };
            const string oidCronograma2 = "C2";
            string[] colaboradores2 = new string[] { "Marcos" };

            WexMultiAccessManagerConexaoClientMock manager = new WexMultiAccessManagerConexaoClientMock()
            {
                EnderecoIp = "127.0.0.1",
                Porta = 8020,
                EstadoConexaoCliente = CsEstadoConexaoCliente.Leitura_E_Escrita_Desativadas
            };

            manager.Conectar();
            //Criando um dicionario de  colaboradores conectados para C1 e C2
            TcpClient tcp = new TcpClient();
            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramasTemporarios = new Dictionary<string, Dictionary<string, ConexaoCliente>>();
            Dictionary<string, ConexaoCliente> colaboradoresConectadosTemporario1 = new Dictionary<string, ConexaoCliente>();
            Dictionary<string, ConexaoCliente> colaboradoresConectadosTemporario2 = new Dictionary<string, ConexaoCliente>();
            ConexaoCliente conexaoTemporaria;
            //preenchendo colaboradoresConectadosTemporarios - C1
            foreach(string colaborador in colaboradores)
            {
                conexaoTemporaria = new ConexaoClienteMock( colaborador, tcp, manager.FilaProcessamento ) { PermissaoDeEscrita = false, PermissaoDeLeitura = false };
                // conexaoTemporaria.OidCronograma = "C1";
                colaboradoresConectadosTemporario1.Add( colaborador, conexaoTemporaria );
            }
            //adicionando o CronogramaConectados C1 com seus colaboradores
            manager.CronogramasConectados.Add( oidCronograma, colaboradoresConectadosTemporario1 );
            //limpando colaboradoresConectadosTemporario
            colaboradoresConectadosTemporario2 = new Dictionary<string, ConexaoCliente>();
            //preenchendo colaboradoresConectadosTemporario - C2
            foreach(string colaborador in colaboradores2)
            {
                conexaoTemporaria = new ConexaoClienteMock( colaborador, tcp, manager.FilaProcessamento ) { PermissaoDeEscrita = false, PermissaoDeLeitura = false };
                colaboradoresConectadosTemporario2.Add( colaborador, conexaoTemporaria );
            }
            //adicionando ao CronogramaConectados C2 com seus colaboradores
            manager.CronogramasConectados.Add( oidCronograma2, colaboradoresConectadosTemporario2 );

            foreach(var cronograma in manager.CronogramasConectados)
            {
                foreach(var colaborador in cronograma.Value)
                {
                    manager.RnComunicarUsuarioNovoConectado( colaborador.Key, cronograma.Key );
                }
            }

            int cont;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                cont = ( from c in manager.CronogramasConectados
                         from conexoes in c.Value.Values
                         where conexoes.FilaEscrita.Count > 0
                         select conexoes.FilaEscrita.Count ).Sum();
                return cont == 5;

            }, 10 );

            //Primeira Mensagem
            MensagemDto objetoMensagem1 = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaboradores, oidCronograma );
            //Segunda Mensagem
            MensagemDto objetoMensagem2 = Mensagem.RnCriarMensagemNovoUsuarioConectado( colaboradores, oidCronograma );

            /*
             Estimativa funcional do RnProcessarEventos:
             * Deve enfileirar em cada ConexãoCliente a Lista de eventos de seus Determinado Cronograma
             */
            foreach(var cronograma in manager.CronogramasConectados)
            {
                foreach(var colaborador in cronograma.Value)
                {
                    Assert.IsTrue( colaborador.Value.FilaEscrita.Count > 0,
                    string.Format( "O colaborador {0} Deveria ter algum evento em sua fila de escrita", colaborador.Key ) );
                }
            }
            manager.ListarMensagensDeEscritaConexoesCliente();
            var cronograma1 = manager.CronogramasConectados["C1"];

            foreach(var colaborador in cronograma1)
            {
                Assert.ReferenceEquals( objetoMensagem1, colaborador.Value.FilaEscrita.Peek() );
            }
            manager.Desconectar();
            tcp.Close();
        }


        [TestMethod]
        public void VerificarTerminoDeConexaoNoConexaoClienteTest15()
        {
            Mock<WexMultiAccessManagerConexaoClientMock> managerMock = new Mock<WexMultiAccessManagerConexaoClientMock>() { CallBase = true };
            managerMock.Setup( o => o.ManterAtendimento() );
            WexMultiAccessManagerConexaoClientMock manager = managerMock.Object;
            manager.EnderecoIp = "127.0.0.1";
            manager.Porta = 8011;
            manager.Conectar();

            Mock<WexMultiAccessClientMock> clienteMock1 = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            clienteMock1.Protected().Setup( "RnProcessarEventos" );

            Mock<WexMultiAccessClientMock> clienteMock2 = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            clienteMock2.Protected().Setup( "RnProcessarEventos" );
            ;

            Mock<WexMultiAccessClientMock> clienteMock3 = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            clienteMock3.Protected().Setup( "RnProcessarEventos" );

            WexMultiAccessClientMock cliente1 = clienteMock1.Object;
            WexMultiAccessClientMock cliente2 = clienteMock2.Object;
            WexMultiAccessClientMock cliente3 = clienteMock3.Object;

            cliente1.Login = "Joao";
            cliente1.Porta = manager.Porta;
            cliente1.EnderecoIp = manager.EnderecoIp;
            cliente1.OidCronograma = "C1";
            cliente1.Conectar();

            manager.RnAguardarConexao();

            cliente2.Login = "Pedro";
            cliente2.Porta = manager.Porta;
            cliente2.EnderecoIp = manager.EnderecoIp;
            cliente2.OidCronograma = "C1";
            cliente2.Conectar();

            manager.RnAguardarConexao();

            cliente3.Login = "Jose";
            cliente3.Porta = manager.Porta;
            cliente3.EnderecoIp = manager.EnderecoIp;
            cliente3.OidCronograma = "C1";
            cliente3.Conectar();

            manager.RnAguardarConexao();

            ControleDeEsperaUtil.AguardarAte( () => { return manager.CronogramasConectados["C1"].Count == 3; } );

            Assert.IsTrue( manager.CronogramasConectados.ContainsKey( "C1" ), "Deveria ter preenchido a hash de cronogramas conectados." );
            Assert.AreEqual( 3, manager.CronogramasConectados["C1"].Count, "Deveria possuir 3 usuários conectados" );

            cliente1.RnDesconectar();
            cliente2.RnDesconectar();
            cliente3.RnDesconectar();
            manager.Desconectar();

            foreach(var cronograma in manager.CronogramasConectados)
            {
                foreach(var conexao in cronograma.Value)
                {
                    Assert.IsFalse( conexao.Value.TcpCliente.Connected, string.Format( "Deveria estar desconectado a conexao do cliente {0} ", conexao.Value.LoginCliente ) );
                }
            }


        }


        [TestMethod]
        public void RnRemoverUsuarioDesconectadoDoCronogramaDeUsuariosConectadosQuandoEstiverMaisDeUmCronograma()
        {
            Mock<WexMultiAccessManagerConexaoClientMock> managerMock = new Mock<WexMultiAccessManagerConexaoClientMock>() { CallBase = true };
            managerMock.Setup( o => o.ManterAtendimento() );
            managerMock.Setup( o => o.RnProcessarEventos() );
            WexMultiAccessManagerConexaoClientMock manager = managerMock.Object;
            manager.StatusServidor = false;
            const string colaborador1 = "Joao";
            const string colaborador2 = "Pedro";
            const string oidCronograma = "C1";
            const string oidCronograma2 = "C2";
            const string oidCronograma3 = "C3";

            //Conectando Joao nos cronogramas C1 e C3
            List<string> listaTemporaria = new List<string>();
            listaTemporaria.Add( oidCronograma );
            listaTemporaria.Add( oidCronograma3 );
            manager.PreencherHashUsuariosConectados( colaborador1, listaTemporaria );

            //Conectando Pedro nos cronogramas C2 e C3
            listaTemporaria = new List<string>();
            listaTemporaria.Add( oidCronograma2 );
            listaTemporaria.Add( oidCronograma3 );
            manager.PreencherHashUsuariosConectados( colaborador2, listaTemporaria );


            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramasConectados = new Dictionary<string, Dictionary<string, ConexaoCliente>>();
            Dictionary<string, ConexaoCliente> listaUsuariosEmCronograma = new Dictionary<string, ConexaoCliente>();
            //Conectando Joao no cronograma C1
            listaUsuariosEmCronograma.Add( colaborador1, new ConexaoClienteMock( colaborador1, new TcpClient() { }, new Queue<MensagemDto>() ) );
            cronogramasConectados.Add( oidCronograma, listaUsuariosEmCronograma );
            //Conectando Pedro no Cronograma C2
            listaUsuariosEmCronograma = new Dictionary<string, ConexaoCliente>();
            listaUsuariosEmCronograma.Add( colaborador2, new ConexaoClienteMock( colaborador2, new TcpClient() { }, new Queue<MensagemDto>() ) );
            cronogramasConectados.Add( oidCronograma2, listaUsuariosEmCronograma );
            //Conectando Joao e Pedro no Cronograma C3
            listaUsuariosEmCronograma = new Dictionary<string, ConexaoCliente>();
            listaUsuariosEmCronograma.Add( colaborador1, new ConexaoClienteMock( colaborador1, new TcpClient() { }, new Queue<MensagemDto>() ) );
            listaUsuariosEmCronograma.Add( colaborador2, new ConexaoClienteMock( colaborador2, new TcpClient() { }, new Queue<MensagemDto>() ) );
            cronogramasConectados.Add( oidCronograma3, listaUsuariosEmCronograma );
            manager.PreencherHashCronogramasConectados( cronogramasConectados );

            //Removendo o colaborador Pedro de um cronograma especifico C3
            manager.RnRemoverUsuarioQueSeDesconectouTest( colaborador2, oidCronograma3 );

            Assert.AreEqual( 1, manager.CronogramasConectados[oidCronograma3].Count, " o cronograma c3 deveria  possuir 1 colaborador" );
            Assert.IsFalse( manager.ColaboradorConectado( colaborador2, oidCronograma3 ), string.Format( "O usuario {0}  não deveria estar no cronograma {1}", colaborador2, oidCronograma3 ) );
            Assert.AreEqual( 1, manager.UsuariosConectados[colaborador2].Count, "Deveria estar conectado em somente um cronograma" );

            //Removendo o colaborador Joao de todos os cronogramas
            manager.RnRemoverUsuarioQueSeDesconectouPublicadoTest( colaborador1 );
            //Verificar se está na hash de usuariosConectados
            Assert.IsFalse( manager.ColaboradorConectado( colaborador1 ), "Não deveria estar conectado em nehum cronograma" );
            //Verificar se está na hash de cronogramasConectados no indice C1
            Assert.IsFalse( manager.ColaboradorConectado( colaborador1, oidCronograma ), "Não deveria estar conectado no cronograma C1" );
            //Verificar se está na hash de cronogramasConectados no indice C3
            Assert.IsFalse( manager.ColaboradorConectado( colaborador1, oidCronograma3 ), "Não deveria estar conectado no cronograma C3" );
        }

        [TestMethod]
        public void RnProcessarEventosAcionarRnRemoverUsuarioQueSeDesconectou()
        {
            int contador = 0;
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Protected().Setup( "RnRemoverUsuarioQueSeDesconectou", ItExpr.IsAny<string>(), ItExpr.IsAny<string>() );
            managerMock.Protected().Setup( "FinalizarConexaoUsuario", ItExpr.IsAny<MensagemDto>() ).Callback( () => { contador++; } );
            // managerMock.Setup(o => o.RnProcessarEventos());
            WexMultiAccessManagerMock manager = managerMock.Object;
            //manager.EnderecoIp = "127.0.0.1";
            //manager.Porta = 8055;
            //manager.Conectar();
            Dictionary<string, Dictionary<string, ConexaoCliente>> cronogramasConectados = new Dictionary<string, Dictionary<string, ConexaoCliente>>();
            Dictionary<string, ConexaoCliente> usuariosConectados = new Dictionary<string, ConexaoCliente>();
            usuariosConectados.Add( "Pedro", new ConexaoCliente( "Pedro", new TcpClient(), manager.FilaProcessamento ) );
            cronogramasConectados.Add( "C1", usuariosConectados );
            usuariosConectados = new Dictionary<string, ConexaoCliente>();
            usuariosConectados.Add( "Joao", new ConexaoCliente( "Joao", new TcpClient(), manager.FilaProcessamento ) );
            usuariosConectados.Add( "Pedro", new ConexaoCliente( "Pedro", new TcpClient(), manager.FilaProcessamento ) );
            cronogramasConectados.Add( "C2", usuariosConectados );
            manager.PreencherHashCronogramasConectados( cronogramasConectados );
            MensagemDto mensagemTemporaria = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { "Joao" }, "C1" );
            manager.RnProcessarMensagemParaTestes( mensagemTemporaria );
            mensagemTemporaria = Mensagem.RnCriarMensagemUsuarioDesconectado( "Pedro", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagemTemporaria );
            mensagemTemporaria = Mensagem.RnCriarMensagemUsuarioDesconectado( "Joao", "C2" );
            manager.RnProcessarMensagemParaTestes( mensagemTemporaria );
            mensagemTemporaria = Mensagem.RnCriarMensagemUsuarioDesconectado( "Pedro", "C2" );
            manager.RnProcessarMensagemParaTestes( mensagemTemporaria );

            ControleDeEsperaUtil.AguardarAte( () => { return manager.FilaProcessamento.Count == 0; } );
            Assert.AreEqual( 4, contador, "Deveria ter acionado 4 vezes o método RnRemoverUsuarioQueSeDesconectou" );
            manager.Desconectar();
        }

        [TestMethod]
        public void ProcessarEventosReceberEEnfileirarMensagemInicioEdicaoTarefa()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8063;

            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock() { EnderecoIp = ipServidor, Porta = porta };
            manager.Conectar();
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock() { EnderecoIp = ipServidor, Porta = porta, Login = "Joao", OidCronograma = "C1" };
            cliente.AoSerAutenticadoComSucesso += ( mensagemDto ) => { };
            cliente.Conectar();
            //aguardar o usuario se conectar
            ControleDeEsperaUtil.AguardarAte( () => { return manager.UsuariosConectados.Count == 1; } );
            //Cliente comunicando que a tarefa T1 está em edição
            cliente.RnComunicarInicioEdicaoTarefa( "T1" );
            MensagemDto mensagemEsperada = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", cliente.Login, cliente.OidCronograma );
            //aguardar que o manager recebe a mensagem de que foi Iniciado a Edicao de uma tarefa
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ContadorMensagens[(int)CsTipoMensagem.InicioEdicaoTarefa] == 1; } );
            //ControleDeEsperaUtil.AguardarAte(() => { return false; },120);
            Assert.AreEqual( 1, manager.ContadorMensagens[(int)CsTipoMensagem.InicioEdicaoTarefa], "Deveria ter contado 1 mensagemd o tipo InicioEdicaoTarefa" );

            List<MensagemDto> mensagens = manager.ListaTodasMensagensProcessadas;

            Assert.AreEqual( mensagemEsperada.Tipo, mensagens.ElementAt( 1 ).Tipo, "A mensagem recebida no manager e resumida deve possuir o mesmo tipo da mensagem esperada" );
            CollectionAssert.AreEquivalent( mensagemEsperada.Propriedades, mensagens.ElementAt( 1 ).Propriedades );
            cliente.RnDesconectar();
            manager.Desconectar();
        }

        [TestMethod]
        public void AdicionarTarefaATarefasEmEdicaoTest()
        {
            const string ipServidor = "127.0.0.1";
            const int porta = 8064;
            const string oidCronograma1 = "C1";
            const string oidCronograma2 = "C2";
            const string login1 = "Joao";
            const string login2 = "Pedro";
            const string login3 = "Marcos";
            const string oidTarefa1 = "T1";
            const string oidTarefa2 = "T2";
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock() { EnderecoIp = ipServidor, Porta = porta };
            // manager.Conectar();

            MensagemDto mensagem;
            // criar mensagem e adicionar a primeira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login1, oidCronograma1 );
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma1, login1, oidTarefa1 );

            // criar mensagem e adicionar a segunda edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa2, login2, oidCronograma1 );
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma1, login2, oidTarefa2 );

            // criar mensagem e adicionar a terceira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login3, oidCronograma2 );
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma2, login3, oidTarefa1 );


            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma1 ), "Deveria conter o cronograma C1" );
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma2 ), "Deveria conter o cronograma C2" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa1 ), "Deveria conter No cronograma C1 a tarefa T1" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa2 ), "Deveria conter No cronograma C1 a tarefa T2" );
            Assert.AreEqual( login1, manager.TarefasEmEdicao[oidCronograma1][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma1, oidTarefa1, login1 ) );
            Assert.AreEqual( login2, manager.TarefasEmEdicao[oidCronograma1][oidTarefa2], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma1, oidTarefa2, login2 ) );
            Assert.AreEqual( login3, manager.TarefasEmEdicao[oidCronograma2][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma2, oidTarefa1, login3 ) );
        }

        [TestMethod]
        public void RnProcessarMensagemAoProcessarMensagemInicioEdicaoTarefaTest()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Setup( o => o.RnAutorizarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) );
            managerMock.Setup( o => o.RnRecusarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) );
            WexMultiAccessManagerMock manager = managerMock.Object;
            const string oidCronograma1 = "C1";
            const string oidCronograma2 = "C2";
            const string login1 = "Joao";
            const string login2 = "Pedro";
            const string login3 = "Marcos";
            const string oidTarefa1 = "T1";
            const string oidTarefa2 = "T2";

            MensagemDto mensagem;
            // criar mensagem e adicionar a primeira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login1, oidCronograma1 );
            manager.RnProcessarMensagemParaTestes( mensagem );

            // criar mensagem e adicionar a segunda edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa2, login2, oidCronograma1 );
            manager.RnProcessarMensagemParaTestes( mensagem );

            // criar mensagem e adicionar a terceira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login3, oidCronograma2 );
            manager.RnProcessarMensagemParaTestes( mensagem );



            //Asserts Mensagens que devem ser adicionadas na lista de tarefas em edição
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma1 ), "Deveria conter o cronograma C1" );
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma2 ), "Deveria conter o cronograma C2" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa1 ), "Deveria conter No cronograma C1 a tarefa T1" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa2 ), "Deveria conter No cronograma C1 a tarefa T2" );
            Assert.AreEqual( login1, manager.TarefasEmEdicao[oidCronograma1][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma1, oidTarefa1, login1 ) );
            Assert.AreEqual( login2, manager.TarefasEmEdicao[oidCronograma1][oidTarefa2], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma1, oidTarefa2, login2 ) );
            Assert.AreEqual( login3, manager.TarefasEmEdicao[oidCronograma2][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma2, oidTarefa1, login3 ) );


            // Cenário: o usuário do login1 um está editando a tarefa t1 no cronograma c1 e o usuário do login2 tenta editar a mesma tarefa
            MensagemDto mensagemRecusadaEsperada, mensagemQueDeveSerRecusada;
            mensagemQueDeveSerRecusada = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login2, oidCronograma1 );
            mensagemRecusadaEsperada = manager.RnProcessarMensagemParaTestes( mensagemQueDeveSerRecusada );

            Assert.IsNull( mensagemRecusadaEsperada );
        }

        [TestMethod]
        public void RnProcessarMensagemAoProcessarMensagemFinalizarEdicaoTarefaTest()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Setup( o => o.RnAutorizarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) );
            managerMock.Setup( o => o.RnRecusarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) );
            WexMultiAccessManagerMock manager = managerMock.Object;
            const string oidCronograma1 = "C1";
            const string oidCronograma2 = "C2";
            const string login1 = "Joao";
            const string login2 = "Pedro";
            const string login3 = "Marcos";
            const string oidTarefa1 = "T1";
            const string oidTarefa2 = "T2";

            MensagemDto mensagem;
            // criar mensagem e adicionar a primeira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login1, oidCronograma1 );
            manager.RnProcessarMensagemParaTestes( mensagem );

            // criar mensagem e adicionar a segunda edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa2, login2, oidCronograma1 );
            manager.RnProcessarMensagemParaTestes( mensagem );

            // criar mensagem e adicionar a terceira edicao de tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa1, login3, oidCronograma2 );
            manager.RnProcessarMensagemParaTestes( mensagem );

            // criar mensagem  de finalizar edicao da tarefa 2 pelo usuários pedro
            mensagem = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa2, login2, oidCronograma1 );
            //ao executar o manager não deverá possuir em sua lista de tarefas em edição a tarefa t2 do cronograma c1
            manager.RnProcessarMensagemParaTestes( mensagem );


            //Asserts Mensagens que devem ser adicionadas na lista de tarefas em edição
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma1 ), "Deveria conter o cronograma C1" );
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma2 ), "Deveria conter o cronograma C2" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa1 ), "Deveria conter No cronograma C1 a tarefa T1" );
            Assert.IsFalse( manager.TarefasEmEdicao[oidCronograma1].ContainsKey( oidTarefa2 ), "Não deveria conter No cronograma C1 a tarefa T2" );
            Assert.AreEqual( login1, manager.TarefasEmEdicao[oidCronograma1][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma1, oidTarefa1, login1 ) );
            Assert.AreEqual( login3, manager.TarefasEmEdicao[oidCronograma2][oidTarefa1], string.Format( "O valor armanezado no cronograma {0} para a tarefa {1} deveria ser {2}", oidCronograma2, oidTarefa1, login3 ) );

            // criar mensagem  de finalizar edicao da tarefa 1 pelo usuário marcos
            mensagem = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa1, login3, oidCronograma2 );
            //ao executar o manager não deverá possuir em sua lista de tarefas em edição a tarefa t2 do cronograma c1
            manager.RnProcessarMensagemParaTestes( mensagem );

            //Quando marcos finalizar a tarefa t1 no cronograma t2 só restará o cronograma C1 com tarefas em edição
            Assert.IsTrue( !manager.TarefasEmEdicao.ContainsKey( oidCronograma2 ), "Não deveria conter o cronograma C2" );

            // criar mensagem  de finalizar edicao da tarefa t1 pelo usuário joao do cronograma c1
            mensagem = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa1, login1, oidCronograma1 );
            //ao executar o manager não deverá possuir em sua lista de tarefas em edição
            manager.RnProcessarMensagemParaTestes( mensagem );

            Assert.AreEqual( 0, manager.TarefasEmEdicao.Count, "Não deveria possuir nenhum cronograma com tarefas em edição" );

            Assert.IsFalse( manager.TarefasEmEdicao.ContainsKey( oidCronograma1 ), "Não deveria mais possuir o cronograma c1 em sua lista" );
        }


        [TestMethod]
        public void RnResumirMensagensTest()
        {
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();
            List<MensagemDto> mensagens = new List<MensagemDto>();

            mensagens.Add( Mensagem.RnCriarMensagemNovoUsuarioConectado( "Joao", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovoUsuarioConectado( "Pedro", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovoUsuarioConectado( "Mario", "C2" ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovoUsuarioConectado( "Marcos", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovoUsuarioConectado( "Marta", "C3" ) );

            //mensagens.Add( Mensagem.RnCriarMensagemUsuarioDesconectado( "Marcos", "C2" ) );
            //mensagens.Add( Mensagem.RnCriarMensagemUsuarioDesconectado( "Pedro", "C2" ) );
            //mensagens.Add( Mensagem.RnCriarMensagemUsuarioDesconectado( "Carlos", "C1" ) );

            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Pedro", "C3" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T2", "Mario", "C3" ) );

            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Marcos", "C2" ) );

            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Joao", "C4" ) );

            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( "T2", "Joao", "C2" ) );
            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( "T5", "Jorge", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( "T3", "Lucas", "C2" ) );

            mensagens.Add( Mensagem.RnCriarMensagemNovaTarefaCriada( "T1", "Gabriel", "C1", null, DateTime.Now ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovaTarefaCriada( "T2", "Anderson", "C1", null, DateTime.Now ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovaTarefaCriada( "T2", "Anderson", "C2", null, DateTime.Now ) );
            mensagens.Add( Mensagem.RnCriarMensagemNovaTarefaCriada( "T3", "Alexandre", "C3", null, DateTime.Now ) );

            List<MensagemDto> resumidas = manager.RnResumirMensagensPublicado( mensagens );
            Assert.AreEqual( 12, resumidas.Count, string.Format( "Deveria ter resumido as mensagens de {0} para 12 mensagens pois deve ter sido feita a união de mensagens semelhantes", mensagens.Count ) );
        }


        [TestMethod]
        public void RnExcluirTarefaMangerTest()
        {
            /*
               Cenário:
             * Quando o manager receber uma mensagem do tipo exclusão tarefa, ele deve processar a requisição avaliando a possibilidade de exclusão da
             * tarefa, caso seja possível excluir a tarefa deverá ser armazenada na hash de tarefas em exclusão e qualquer outra tentativa de alteração
             * na tarefa deverá ser recusada, caso não possa ser excluída, não deverá fazer broadcast e retornará uma mensagem de recusa a quem solicitou
             * a exclusão.
             * 
             * Casos em que Não é permitido excluir a tarefa:
             *  - A tarefa encontra-se em edição.
             *  - A tarefa já se encontra em processo de exclusão.
             *  
             * Expectativas no Teste: 
             *  - Não excluir tarefas que estejam em edição.
             *  - Não excluir tarefas que já estejam  em exclusão.
             *  - Remover tarefas que não se encontrem em nenhuma das possibilidades anteriores.
             * 
             * Cenário de Teste:
             *  - Criar Mensagens em Edição
             *  - Criar Mensagens em Exclusão
             *  - Contar Recusas
             *  - Contar Permissões
             */
            Dictionary<string, Dictionary<string, List<string>>> ExclusoesPermitidas = new Dictionary<string, Dictionary<string, List<string>>>();
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();

            List<MensagemDto> mensagens = new List<MensagemDto>();
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Joao", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T3", "Maria", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioExclusaoTarefas( new string[] { "T3" }, "Joao", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioExclusaoTarefas( new string[] { "T2", "T3", "T4" }, "Pedro", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioExclusaoTarefas( new string[] { "T2", "T3", "T4", "T5", "T6" }, "Mario", "C1" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioExclusaoTarefas( new string[] { "T2", "T3", "T4", "T5", "T6" }, "Mario", "C2" ) );

            mensagens.ForEach( o => manager.RnProcessarMensagemParaTestes( o ) );

            string oidCronograma, login;
            string[] tarefas;

            List<MensagemDto> permitidas = manager.ListaTodasMensagensProcessadas.Where( o => o.Tipo == CsTipoMensagem.ExclusaoTarefaPermitida ).ToList();
            foreach(MensagemDto resposta in permitidas)
            {
                if(resposta != null && resposta.Tipo == CsTipoMensagem.ExclusaoTarefaPermitida)
                {
                    oidCronograma = (string)resposta.Propriedades[Constantes.OIDCRONOGRAMA];
                    login = (string)resposta.Propriedades[Constantes.AUTOR_ACAO];
                    tarefas = (string[])resposta.Propriedades[Constantes.TAREFAS];
                    if(tarefas.Length < 1)
                        continue;
                    if(!ExclusoesPermitidas.ContainsKey( oidCronograma ))
                        ExclusoesPermitidas.Add( oidCronograma, new Dictionary<string, List<string>>() );

                    if(!ExclusoesPermitidas[oidCronograma].ContainsKey( login ))
                        ExclusoesPermitidas[oidCronograma].Add( login, new List<string>() );

                    foreach(string tarefa in tarefas)
                    {
                        if(!ExclusoesPermitidas[oidCronograma][login].Contains( tarefa ))
                            ExclusoesPermitidas[oidCronograma][login].Add( tarefa );
                    }

                }
            }

            Assert.IsTrue( ExclusoesPermitidas.ContainsKey( "C1" ), "Deveria possuir exclusões permitidas no cronograma C1." );
            Assert.IsTrue( ExclusoesPermitidas.ContainsKey( "C2" ), "Deveria possuir exclusões permitidas no cronograma C2." );
            Assert.IsTrue( ExclusoesPermitidas["C1"].ContainsKey( "Pedro" ), "Deveria possuir tarefas permitidas para Pedro No Cronograma C1." );
            CollectionAssert.AreEquivalent( new string[] { "T2", "T4" }, ExclusoesPermitidas["C1"]["Pedro"].ToArray(), "Deveria conter tarefas permitidas para Pedro." );

            Assert.IsTrue( ExclusoesPermitidas["C1"].ContainsKey( "Mario" ), "Deveria possuir tarefas permitidas para Mario No Cronograma C1." );
            CollectionAssert.AreEquivalent( new string[] { "T5", "T6" }, ExclusoesPermitidas["C1"]["Mario"].ToArray(), "Deveria conter tarefas permitidas para Mario no Cronograma C1." );

            Assert.IsTrue( ExclusoesPermitidas["C2"].ContainsKey( "Mario" ), "Deveria possuir tarefas permitidas para Mario No Cronograma C2." );
            CollectionAssert.AreEquivalent( new string[] { "T2", "T3", "T4", "T5", "T6" }, ExclusoesPermitidas["C2"]["Mario"].ToArray(), "Deveria conter tarefas permitidas para Mario no Cronograma C2." );
        }

        [TestMethod]
        public void DeveProcessarSolicitacaoDeEdicaoDeTarefaEPermitirQuandoColaboradorNaoPossuirTarefaEmEdicao()
        {
            const string login = "Joao";
            const string oidCronograma = "C1";
            const string oidTarefa = "T1";
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.AdicionarUsuarioConectado( login, oidCronograma );
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa, login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );
            Assert.IsTrue( manager.VerificarExisteEdicaoPara( login, oidCronograma ), string.Format( "Deveria conter uma edição para o colaborador {0} no cronograma {1}", login, oidCronograma ) );
            managerMock.Verify( o => o.RnAutorizarEdicaoTarefa( oidCronograma, login, oidTarefa, It.IsAny<string>() ), Times.Once(),
                "Deveria ter chamado o método para informar a autorização de edição da tarefa" );
        }

        [TestMethod]
        public void DeveProcessarASolicitacaoDeEdicaoDaTarefaERecusarAEdicaoQuandoColaboradorJaPossuirTarefaEmEdicao()
        {
            const string login = "Joao";
            const string oidCronograma = "C1";
            const string oidTarefa = "T1";
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.AdicionarUsuarioConectado( login, oidCronograma );
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa, login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );
            Assert.IsTrue( manager.VerificarExisteEdicaoPara( login, oidCronograma ), string.Format( "Deveria conter uma edição para o colaborador {0} no cronograma {1}", login, oidCronograma ) );
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T2", login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );
            managerMock.Verify( o => o.RnRecusarEdicaoTarefa( oidCronograma, login, "T2", It.IsAny<string>(), It.IsAny<string>() ), Times.Once(),
                "Deveria ter recusado a edição da tarefa pois já havia uma tarefa em edição." );
            Assert.IsTrue( manager.TarefaEstaLivre( oidCronograma, login, "T2" ), "A tarefa deveria estar livre pois a solicitação de Joao foi recusada, por que ja estava editando outra tarefa" );
        }

        [TestMethod]
        public void DeveProcessarASolicitacaoDeEdicaoDaTarefaEPermitirEdicaoQuandoUmColaboradorLiberarUmaEdicaoAnterior()
        {
            const string login = "Joao";
            const string oidCronograma = "C1";
            const string oidTarefa = "T1";
            const string oidTarefa2 = "T2";
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.AdicionarUsuarioConectado( login, oidCronograma );
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa, login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );
            Assert.IsTrue( manager.VerificarExisteEdicaoPara( login, oidCronograma ), string.Format( "Deveria conter uma edição para o colaborador {0} no cronograma {1}", login, oidCronograma ) );

            //Simular a finalização e liberação da tarefa
            mensagem = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa, login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );

            Assert.IsTrue( manager.TarefaEstaLivre( oidCronograma, login, oidTarefa ), "A tarefa deveria estar livre pois a solicitação de Joao foi finalizada." );

            //Simular o inicio de edição da segunda tarefa
            mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( oidTarefa2, login, oidCronograma );
            manager.RnProcessarMensagemParaTestes( mensagem );

            Assert.IsFalse( manager.TarefaEstaLivre( oidCronograma, login, oidTarefa2 ), "A tarefa não deveria estar livre  pois entrou em edição" );

            managerMock.Verify( o => o.RnAutorizarEdicaoTarefa( oidCronograma, login, oidTarefa2, It.IsAny<string>() ), Times.Once(),
                "Deveria ter permitido a edição da tarefa pois já havia encerrado a edição anterior." );
            Assert.IsFalse( manager.TarefaEstaLivre( oidCronograma, login, oidTarefa2 ), "A tarefa não deveria estar livre pois a solicitação de Joao foi permitida após a liberação da primeira." );
        }


        [TestMethod]
        public void RnProcessarMensagemInicioEdicaoTarefaQuandoTarefaForRecusada()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            int contador = 0;
            managerMock.Setup( o => o.RnAutorizarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) );
            managerMock.Setup( o => o.RnRecusarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { contador++; } );
            WexMultiAccessManagerMock manager = managerMock.Object;
            MensagemDto mensagem = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Joao", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagem );
            MensagemDto mensagem2 = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T1", "Pedro", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagem2 );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 1; } );
            Assert.AreEqual( 1, contador, "Deveria ter disparado o método de recusa da edição da tarefa" );
        }


        [TestMethod]
        public void RnProcessarMensagemInicioExclusaoTarefaTeste()
        {
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            int contadorTarefaEmEdicao = 0;
            MensagemDto mensagemRetornoExclusao = new MensagemDto();
            managerMock.Setup( o => o.RnAutorizarEdicaoTarefa( It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() ) ).Callback( () => { contadorTarefaEmEdicao++; } );
            managerMock.Setup( o => o.RnResponderSolicitacaoExclusaoTarefa( It.IsAny<MensagemDto>() ) ).Callback( ( MensagemDto mensagem ) => { mensagemRetornoExclusao = mensagem; } );
            WexMultiAccessManagerMock manager = managerMock.Object;
            Dictionary<string, Int16> tarefasOrdenadasGrid = new Dictionary<string, short>();
            tarefasOrdenadasGrid.Add( "T1", 1 );
            tarefasOrdenadasGrid.Add( "T2", 2 );
            tarefasOrdenadasGrid.Add( "T3", 3 );
            tarefasOrdenadasGrid.Add( "T4", 4 );
            tarefasOrdenadasGrid.Add( "T5", 5 );
            tarefasOrdenadasGrid.Add( "T6", 6 );

            //Solicitando a exclusão das tarefas de T1 a T4
            string[] tarefas = tarefasOrdenadasGrid.Where( o => o.Value < 5 ).Select( o => o.Key ).ToArray();
            string[] tarefasEmEdicao = new string[] { "T2", "T5" };

            //Solicitando a edição das tarefas de T2 e T5
            MensagemDto mensagemEdicao = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T2", "Marcos", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagemEdicao );
            mensagemEdicao = Mensagem.RnCriarMensagemInicioEdicaoTarefa( "T5", "Pedro", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagemEdicao );

            //solicitando a exclusão das tarefas de T1 a T4
            MensagemDto mensagemExclusao = Mensagem.RnCriarMensagemInicioExclusaoTarefas( tarefas, "Joao", "C1" );
            manager.RnProcessarMensagemParaTestes( mensagemExclusao );
            MensagemDto mensagemExclusaoPermitida = Mensagem.RnCriarMensagemEfetuarExclusaoTarefas( tarefas.Except( tarefasEmEdicao ).ToArray(), new string[] { }, "C1", "Joao" );
            Assert.AreEqual( (string)mensagemExclusaoPermitida.Propriedades[Constantes.OIDCRONOGRAMA], (string)mensagemRetornoExclusao.Propriedades[Constantes.OIDCRONOGRAMA], "Deveria ser o mesmo cronograma" );
            Assert.AreEqual( (string)mensagemExclusaoPermitida.Propriedades[Constantes.AUTOR_ACAO], (string)mensagemRetornoExclusao.Propriedades[Constantes.AUTOR_ACAO], "Deveria ser o mesmo usuário que solicitou a exclusão" );
            CollectionAssert.AreEquivalent( (string[])mensagemExclusaoPermitida.Propriedades[Constantes.TAREFAS], (string[])mensagemExclusaoPermitida.Propriedades[Constantes.TAREFAS], "Deveria ser as mesmas tarefas que não estavam em edição" );
        }

        [TestMethod]
        public void RemoverPermissoesTarefasUsuariosDesconectados()
        {
            string oidCronograma = "C1";
            string login1 = "joao";
            string login2 = "marcos";
            string[] oidTarefas = new string[] { "T1", "T2", "T3" };
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;

            //adicionar edição de tarefa para joao no cronograma C1 a tarefa T1
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login1, oidTarefas[0] );
            //adicionar edição de tarefa para marcos no cronograma C1 a tarefa T2
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login2, oidTarefas[1] );
            //adicionar edição de tarefa para joao no cronograma C1 a tarefa T13
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login1, oidTarefas[2] );
            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma ), "O cronogram deveria conter edições" );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[0] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login1 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[0], login1 ) );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[0] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login1 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[2], login1 ) );
            manager.RemoverEdicoesUsuarioDesconectado( oidCronograma, login1 );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[1] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login2 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[1], login2 ) );
            Assert.IsFalse( manager.TarefasEmEdicao[oidCronograma].ContainsValue( login1 ), string.Format( "Não deveria mais possuir tarefas em edição para o colaborador {0}", login1 ) );
            manager.RemoverEdicoesUsuarioDesconectado( oidCronograma, login2 );
            Assert.IsFalse( manager.TarefasEmEdicao.ContainsKey( oidCronograma ), "Não deveria mais possuir edições para o cronograma" );
        }

        [TestMethod]
        public void RemoverPermissoesTarefasUsuariosDesconectadosConcorrentes()
        {
            string oidCronograma = "C1";
            string login1 = "joao";
            string login2 = "marcos";
            string[] oidTarefas = new string[] { "T1", "T2", "T3" };
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;

            //adicionar edição de tarefa para joao no cronograma C1 a tarefa T1
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login1, oidTarefas[0] );
            //adicionar edição de tarefa para marcos no cronograma C1 a tarefa T2
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login2, oidTarefas[1] );
            //adicionar edição de tarefa para joao no cronograma C1 a tarefa T13
            manager.AdicionarTarefaATarefasEmEdicaoParaTestes( oidCronograma, login1, oidTarefas[2] );

            Assert.IsTrue( manager.TarefasEmEdicao.ContainsKey( oidCronograma ), "O cronogram deveria conter edições" );

            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[0] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login1 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[0], login1 ) );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[0] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login1 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[2], login1 ) );
            Assert.IsTrue( manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefas[1] ) && manager.TarefasEmEdicao[oidCronograma].ContainsValue( login2 ),
                string.Format( "O cronograma {0} deveria possuir tarefa {1} em edição pelo colaborador {2}", oidCronograma, oidTarefas[1], login2 ) );

            int contador = 0;
            AsyncCallback callback = ( result ) =>
            {
                if(result.IsCompleted)
                    contador++;
            };

            Action desconectarJoao = () =>
            {
                manager.RemoverEdicoesUsuarioDesconectado( oidCronograma, login1 );
            };

            Action desconectarMarcos = () =>
            {
                manager.RemoverEdicoesUsuarioDesconectado( oidCronograma, login2 );
            };

            desconectarJoao.BeginInvoke( callback, null );
            desconectarMarcos.BeginInvoke( callback, null );
            ControleDeEsperaUtil.AguardarAte( () => { return contador == 2; } );
            Assert.IsFalse( manager.TarefasEmEdicao.ContainsKey( oidCronograma ), "Não deveria mais possuir edições para o cronograma" );
            Assert.IsTrue( manager.TarefasEmEdicao.Count == 0 );
        }

        [TestMethod]
        public void MultiAcessManagerAceitarMultiplasConexoesSimultaneas()
        {
            WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock()
            {
                Porta = 8065,
                EnderecoIp = "127.0.0.1"
            };
            manager.Conectar();

            string oidCronograma = "C1";

            string[] colaboradores = new string[] { "Joao", "Marcos", "Pedro", "Paulo", "Jonas", "Juliana", "Adriana", "Fabiane", "Samantha", "Joyce" };

            Dictionary<string, WexMultiAccessClientMock> clientes = new Dictionary<string, WexMultiAccessClientMock>();
            List<Thread> threads = new List<Thread>();
            for(int i = 0; i < colaboradores.Length; i++)
            {
                clientes.Add( colaboradores[i], new WexMultiAccessClientMock() { OidCronograma = oidCronograma, EnderecoIp = manager.EnderecoIp, Login = colaboradores[i], Porta = manager.Porta } );
                clientes[colaboradores[i]].AoSerAutenticadoComSucesso += ( mensagem ) => { };
                clientes[colaboradores[i]].AoConectarNovoUsuario += ( mensagem, login ) => { };
                clientes[colaboradores[i]].Conectar();
                threads.Add( new Thread( clientes[colaboradores[i]].Conectar ) );
            }

            for(int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }
            List<string> usuariosConectados = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                usuariosConectados = new List<string>( manager.UsuariosConectados.Keys );
                usuariosConectados = usuariosConectados.Intersect( colaboradores ).ToList();
                return usuariosConectados.Count == colaboradores.Length;
            }, 5 * colaboradores.Length );


            Assert.AreEqual( colaboradores.Length, usuariosConectados.Count, "a lista de usuarios conectados deveria possuir a mesma quantidade de colaboradores que tentaram se conectar" );
            CollectionAssert.AreEquivalent( colaboradores, manager.UsuariosConectados.Keys, "as listas deveriam conter os mesmo usuários" );
        }

        [TestMethod]
        public void ProcessarMensagemInicioEdicaoNomeCronogramaTest()
        {
            List<MensagemDto> mensagensComunicadas = new List<MensagemDto>();
            List<MensagemDto> mensagens = new List<MensagemDto>();
            List<MensagemDto> mensagensProcessadas = new List<MensagemDto>();
            List<MensagemDto> mensagensRecusa, mensagensPermissao;
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            managerMock.Protected().Setup( "ComunicarRespostaSolicitacaoAoUsuario", ItExpr.IsAny<MensagemDto>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>() )
                .Callback( ( MensagemDto msg, string oid, string login ) =>
                {
                    mensagensComunicadas.Add( msg );
                } );
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.CronogramasNomeEmEdicao = new Dictionary<string, string>();

            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C1", "Joao" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C2", "Pedro" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C1", "Marcos" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C2", "Juliana" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C2", "Maria" ) );

            MensagemDto mensagemTemporaria;

            foreach(MensagemDto mensagem in mensagens)
            {
                mensagemTemporaria = manager.RnProcessarMensagemParaTestes( mensagem );
                if(mensagemTemporaria != null)
                {
                    mensagensProcessadas.Add( mensagemTemporaria );
                }
            }
            mensagensPermissao = mensagensComunicadas.Where( o => o.Tipo == CsTipoMensagem.EdicaoNomeCronogramaPermitida ).ToList();
            mensagensRecusa = mensagensComunicadas.Where( o => o.Tipo == CsTipoMensagem.EdicaoNomeCronogramaRecusada ).ToList();
            Assert.AreEqual( 2, mensagensPermissao.Count, "Deveria ter permitido uma edição para cada cronograma" );
            Assert.AreEqual( 3, mensagensRecusa.Count, "Deveria recusar 3 edições pois já haviam outras pessoas editando" );

            MensagemDto mensagemTest;
            mensagemTest = mensagensPermissao.FirstOrDefault( o => o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == "C1" && o.Propriedades[Constantes.AUTOR_ACAO].ToString() == "Joao" );
            Assert.IsNotNull( mensagemTest, "Deveria ter encontrado a mensagem de permissão para o Joao no cronograma C1" );
            mensagemTest = mensagensPermissao.FirstOrDefault( o => o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == "C2" && o.Propriedades[Constantes.AUTOR_ACAO].ToString() == "Pedro" );
            Assert.IsNotNull( mensagemTest, "Deveria ter encontrado a mensagem de permissão para o Pedro no cronograma C2" );
            Assert.AreEqual( 2, manager.CronogramasNomeEmEdicao.Count, "O manager deveria possuir 2 cronogramas com nome em edição" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C1" ), "O manager deveria possuir o cronograma C1 em edição" );
            Assert.AreEqual( "Joao", manager.CronogramasNomeEmEdicao["C1"], "O cronograma C1 deveria estar em edição para o usuário Joao" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C2" ), "O manager deveria possuir o cronograma C2 em edição" );
            Assert.AreEqual( "Pedro", manager.CronogramasNomeEmEdicao["C2"], "O cronograma C2 deveria estar em edição para o usuário Pedro" );
        }

        [TestMethod]
        public void ProcessarMensagemNomeCronogramaAlteradoTest()
        {
            List<MensagemDto> mensagens = new List<MensagemDto>();
            List<MensagemDto> mensagensProcessadas = new List<MensagemDto>();
            Mock<WexMultiAccessManagerMock> managerMock = new Mock<WexMultiAccessManagerMock>() { CallBase = true };
            WexMultiAccessManagerMock manager = managerMock.Object;
            manager.CronogramasNomeEmEdicao = new Dictionary<string, string>();

            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C1", "Joao" ) );
            mensagens.Add( Mensagem.RnCriarMensagemInicioEdicaoNomeCronograma( "C2", "Pedro" ) );

            MensagemDto mensagemTemporaria;

            foreach(MensagemDto mensagem in mensagens)
            {
                mensagemTemporaria = manager.RnProcessarMensagemParaTestes( mensagem );
                if(mensagemTemporaria != null)
                {
                    mensagensProcessadas.Add( mensagemTemporaria );
                }
            }

            Assert.AreEqual( 2, manager.CronogramasNomeEmEdicao.Count, "Deveria possuir 2 cronogrmas com nome em edição, C1 e C2" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C1" ), "Deveria possuir o cronograma C1 na hash de edições" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C2" ), "Deveria possuir o cronograma C2 na hash de edições" );

            mensagens = new List<MensagemDto>();

			MensagemDto mensagemTeste = Mensagem.RnCriarMensagemFimEdicaoDadosCronograma( "C2" , "Pedro" );

            manager.RnProcessarMensagemParaTestes( mensagemTeste );

            Assert.AreEqual( 1, manager.CronogramasNomeEmEdicao.Count, "Deveria possuir apenas 1 cronograma em edição após processar a mensagem de alteraçao do nome do cronograma C2" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C1" ), "Deveria possuir o cronograma C1 na hash de edições" );
            Assert.IsFalse( manager.CronogramasNomeEmEdicao.ContainsKey( "C2" ), "Não deveria possuir o cronograma C2 na hash de edições, pois foi processado o fim da sua edição" );

			mensagemTeste = Mensagem.RnCriarMensagemFimEdicaoDadosCronograma( "C1" , "Pedro" );
            manager.RnProcessarMensagemParaTestes( mensagemTeste );

            Assert.AreEqual( 1, manager.CronogramasNomeEmEdicao.Count, "Deveria ainda conter 1 cronograma pois o usuário Pedro não é o usuário com permissão sobre a edição do nome do cronograma C1" );
            Assert.IsTrue( manager.CronogramasNomeEmEdicao.ContainsKey( "C1" ), "Deveria possuir o cronograma C1 pois o usuário Pedro não é o usuário com permissão sobre a edição do nome do cronograma C1" );

			mensagemTeste = Mensagem.RnCriarMensagemFimEdicaoDadosCronograma( "C1" , "Joao" );
            manager.RnProcessarMensagemParaTestes( mensagemTeste );

            Assert.AreEqual( 0, manager.CronogramasNomeEmEdicao.Count, "Não deveria possuir cronogramas em edição pois o Usuário Joao finalizou a edição do nome do cronograma C1" );
            Assert.IsFalse( manager.CronogramasNomeEmEdicao.ContainsKey( "C1" ), "Não deveria possuir cronograma C1 em edição pois o Usuário Joao finalizou a edição do nome do cronograma C1" );
            managerMock.Protected().Verify( "RemoverEdicaoNomeCronograma", Times.Exactly( 3 ), ItExpr.IsAny<string>(), ItExpr.IsAny<string>() );
            managerMock.VerifyAll();
        }

        [TestMethod]
        public void DeveResumirMensagensDeFimEdicaoTarefaQuandoAMesmaTarefaForEditadaPor2ColaboradoresDistintos()
        {
            CsTipoMensagem tipoMensagem = CsTipoMensagem.EdicaoTarefaFinalizada;
            //WexMultiAccessManagerMock manager = new WexMultiAccessManagerMock();

            //Mock<WexMultiAccessManager> managerMock = new Mock<WexMultiAccessManager>();

            WexMultiAccessManager manager = new WexMultiAccessManager();

            List<MensagemDto> mensagens = new List<MensagemDto>();
            string oidCronograma = Guid.NewGuid().ToString();
            string oidTarefa = Guid.NewGuid().ToString();
            string oidTarefa2 = Guid.NewGuid().ToString();

            const string login1 = "joao";
            const string login2 = "pedro";
            const string login3 = "juca";

            List<MensagemDto> mensagensResumidas = new List<MensagemDto>();

            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa, login1, oidCronograma ) );
            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa, login2, oidCronograma ) );
            mensagens.Add( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( oidTarefa2, login3, oidCronograma ) );

            MensagemDto mensagemResumida = manager.InvocarMetodoPrivado<MensagemDto>( "ResumirMensagemDtoEdicaoTarefas", mensagens, oidCronograma, tipoMensagem );
            Dictionary<string, string> autoresAcao = mensagemResumida.Propriedades[Constantes.AUTORES_ACAO] as Dictionary<string, string>;

            Dictionary<string, string> resultadoEsperado = new Dictionary<string, string>();
            resultadoEsperado.Add( oidTarefa, login1 );
            resultadoEsperado.Add( oidTarefa2, login3 );

            CollectionAssert.AreEquivalent( resultadoEsperado, autoresAcao, "Deveria corresponder ao resultado esperado" );
        }
    }
}