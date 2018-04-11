using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Libs;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Domains;
using System.Collections;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using WexProject.Library.Libs.Test;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Library.Libs.DataHora;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class ConexaoClienteTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void ReceberMensagemQuandoJsonEstiverCompleto()
        {
            #region Estimativa de Teste:
            /*
             * Estimativa Teste:
             * Receber uma mensagemDto Json e enfileirar na fila de leitura;
             * Cenário:
             *  - Instanciar um Listener (gerenciar as conexoes)
             *  - Instanciar 2 TcpClients (Efetuar a comunicação de conexão) 
             *  - Montar as MensagemDTOs de NovoUsarioConectado
             *  - Serializar as Mensagens 
             *  - Escrever cada mensagem no stream de seu respectivo tcp
             *  - Ler a Mensagem no stream
             *  - Processar Cada Mensagem e enfileirar na fila de leitura
             * Resultado Estimado:
             *  - Deve ter sido possivel o enfileiramento das 2 mensagens dto recebidas
             */
            #endregion

            //Configurações do Servidor
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8000;

            //Instanciar e iniciar o Servidor (Listener)
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();
            Queue<MensagemDto> filaProcessamento = new Queue<MensagemDto>();

            //Instanciar os Clientes (2x TcpClient)
            TcpClient cliente1 = new TcpClient();
            TcpClient cliente2 = new TcpClient();
            const string login1 = "Joao";
            const string login2 = "Pedro";
            const string oidCronograma = "C1";

            //Tentativa de conexao do cliente1
            cliente1.Connect(ipServidor,portaServidor);
            // Servidor aceitar a conexao do cliente1 e armazenar
            TcpClient cliente1EmServidor = servidor.AcceptTcpClient();

            //Tentativa de conexao do cliente2
            cliente2.Connect(ipServidor,portaServidor);
            // Servidor aceitar a conexao do cliente1 e armazenar
            TcpClient cliente2EmServidor = servidor.AcceptTcpClient();

            //Iniciar ConexaoCliente do cliente1 
            ConexaoClienteMock c1 = new ConexaoClienteMock(login1,cliente1EmServidor,filaProcessamento) { PermissaoDeEscrita = false };

            //Iniciar ConexaoCliente do cliente2 
            ConexaoClienteMock c2 = new ConexaoClienteMock(login2,cliente2EmServidor,filaProcessamento) { PermissaoDeEscrita = false };

            //Criar mensagemDto novoUsuarioConectado para o cliente1
            MensagemDto m1 = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { login1 },oidCronograma);
            string m1Json = JsonConvert.SerializeObject(m1);
            m1Json = TcpUtil.AdicionarStringProtecaoDeIntegridade(m1Json);

            //Criar mensagemDto novoUsuarioConectado para o cliente2
            MensagemDto m2 = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { login2 },oidCronograma);
            string m2Json = JsonConvert.SerializeObject(m2);
            m2Json = TcpUtil.AdicionarStringProtecaoDeIntegridade(m2Json);

            //Enviar a mensagem Json do primeiro cliente atráves do tcp cliente1;
            TcpUtil.EnviarMensagemTcp(m1Json,cliente1);

            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count > 0; });

            //Enviar a  mensagem Json do segundo cliente atráves do tcp cliente2;
            TcpUtil.EnviarMensagemTcp(m2Json,cliente2);
            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count > 1; });

            Assert.AreEqual(2,filaProcessamento.Count,"Deveria ser enfileirada 2 objetos MensagemDto");

            #region Testes:
            //Verificar see enfileirou as mensagens recebidas
            //   Assert.AreEqual(2, filaProcessamento.Count, "Deveria possuir 2 mensagens enfileiradas");
            //Checar se não há nenhuma mensagem incompleta no buffer de mensagens das conexões
            //    Assert.IsTrue(string.IsNullOrEmpty(conexaoCliente.Buffer), "Buffer deveria estar vazio pois as mensagens json eram mensagens válidas");
            //   Assert.IsTrue(string.IsNullOrEmpty(conexaoCliente2.Buffer), "Buffer deveria estar vazio pois as mensagens json eram mensagens válidas");
            #endregion
            #region Finalização Cenário
            c1.Desconectar();
            c2.Desconectar();
            servidor.Stop();
            cliente1.Close();
            cliente2.Close();
            cliente1EmServidor.Close();
            cliente2EmServidor.Close();
            #endregion
        }

        [TestMethod]
        public void ReceberMensagemQuandoJsonIncompleto()
        {
            #region Estimativa do teste:
            /*
             * Estimativa do teste:
             * - Receber uma mensagem json incompleta e armazenar no buffer e a fila deve estar vazia
             * - receber o resto da mensagem processar e enfileirar
             * Cenário:
             * - conexão instanciada e configurada
             * - filaProcessamento
             * - MensagemJson convertida
             * - MensagemJson repartida em 2
             * - enviar as 2 partes separadamente
             * Acontecimento Estimado:
             * - Armazenar no buffer a mensagem incompleta
             * - Verificar quando ela completar
             * - Enfileirar a mensagem(somente se for completa)
             * Resultado Estimado:
             * - Deve conseguir enfileirar a mensagem
             */
            #endregion

            //Configurações de Ip e porta do servidor
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8001;
            const string login = "Joao";
            string[] usuarios = new string[1];
            //Instanciação do Servidor para escutar o cliente
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();

            //Instanciação do cliente que irá conectar no servidor
            TcpClient cliente = new TcpClient();
            cliente.Connect(ipServidor,portaServidor);
            //Servidor Aceitando a solicitação do cliente
            TcpClient conexao = servidor.AcceptTcpClient();

            //Fila de processamento
            Queue<MensagemDto> filaProcessamento = new Queue<MensagemDto>();
            ConexaoCliente conexaoCliente = new ConexaoCliente(login,conexao,filaProcessamento);

            //Montando a Mensagem que será enviada
            MensagemDto mensagem = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { login },"C1");
            //Serializando a Mensagem que será enviada
            string mensagemEnvio = JsonConvert.SerializeObject(mensagem);
            //Adicionando string de verificação de integridade
            mensagemEnvio = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagemEnvio);
            //Dividindo a mensagem em 2 partes
            string mensagemParte1 = mensagemEnvio.Substring(0,12);
            string mensagemParte2 = mensagemEnvio.Substring(12,(mensagemEnvio.Length - 12));
            Assert.AreEqual(mensagemEnvio,mensagemParte1 + mensagemParte2,"Deveriam Ser Iguais");
            //Enviando a Mensagem através da conexão do cliente
            TcpUtil.EnviarMensagemTcp(mensagemParte1,cliente);
            TcpUtil.EnviarMensagemTcp(mensagemParte2,cliente);
            //Aguardando o enfileiramento
            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count == 1; });
            Assert.AreEqual(1,filaProcessamento.Count,"Deveria possuir 1 mensagem enfileirada");
            Assert.IsTrue(conexaoCliente.Buffer == "","Deveria ter resolvido o buffer e enfileirado o evento");
            conexaoCliente.Desconectar();
            servidor.Stop();
            conexao.Close();
        }

        [TestMethod]
        public void ReceberMensagemQuandoHouverMaisDeUmJsonNaMensagem()
        {
            const string USUARIOS = "usuarios";
            //Configurações de Ip e porta do servidor
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8002;
            const string login = "Joao";
            const string oidCronograma = "C1";
            string[] usuarios = new string[1];
            //Instanciação do Servidor para escutar o cliente
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();

            //Instanciação do cliente que irá conectar no servidor
            TcpClient cliente = new TcpClient();
            cliente.Connect(ipServidor,portaServidor);
            //Servidor Aceitando a solicitação do cliente
            TcpClient conexao = servidor.AcceptTcpClient();
            //Fila de processamento
            Queue<MensagemDto> filaProcessamento = new Queue<MensagemDto>();
            usuarios[0] = login;
            ConexaoCliente conexaoCliente = new ConexaoCliente(login,conexao,filaProcessamento);
            //Configurando propriedades da mensagem
            Hashtable propriedades = new Hashtable();
            propriedades.Add(USUARIOS,usuarios);
            propriedades.Add(oidCronograma,oidCronograma);
            //Montando a Mensagem que será enviada
            MensagemDto mensagem = new MensagemDto() { Tipo = CsTipoMensagem.NovosUsuariosConectados,Propriedades = propriedades };
            //Serializando a Mensagem que será enviada
            string mensagemJsonEnviada = JsonConvert.SerializeObject(mensagem);
            mensagemJsonEnviada = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagemJsonEnviada);
            //Enviando a Mensagem através da conexão do cliente

            const string login2 = "Pedro";
            //Configurando propriedades da mensagem
            propriedades = new Hashtable();
            usuarios = new string[1];
            usuarios[0] = login2;
            propriedades.Add(Constantes.USUARIOS,usuarios);
            propriedades.Add(Constantes.OIDCRONOGRAMA,oidCronograma);
            //Montando a Mensagem que será enviada
            mensagem = new MensagemDto() { Tipo = CsTipoMensagem.NovosUsuariosConectados,Propriedades = propriedades };
            //Serializando a Mensagem que será enviada
            string mensagemJsonEnviada2 = JsonConvert.SerializeObject(mensagem);
            mensagemJsonEnviada2 = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagemJsonEnviada2);
            //Enviando a Mensagem através da conexão do cliente
            TcpUtil.EnviarMensagemTcp(mensagemJsonEnviada + mensagemJsonEnviada2,cliente);

            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count == 2; });

            Assert.AreEqual(2,filaProcessamento.Count,"Deveria possuir 2 mensagens enfileiradas");
            Assert.IsTrue(conexaoCliente.Buffer == "","Buffer deveria estar vazio pois as mensagens json eram mensagens válidas");

            conexaoCliente.Desconectar();
            servidor.Stop();
            cliente.Close();
            conexao.Close();
        }

        [TestMethod]
        public void DesconectarConexaoClienteTest()
        {
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int porta = 8003;
            TcpListener servidor = new TcpListener(ipServidor,porta);
            servidor.Start();
            TcpClient tcpCliente = new TcpClient();
            tcpCliente.Connect(ipServidor,porta);
            servidor.AcceptTcpClient();
            Queue<MensagemDto> fila = new Queue<MensagemDto>();
            const string login = "Joao";
            ConexaoCliente conexao = new ConexaoCliente(login,tcpCliente,fila);
            conexao.Desconectar();

            ControleDeEsperaUtil.AguardarAte( () => { return ( !conexao.threadProcessarLeitura.IsAlive ); } );
            //TODO: @wex developer rever permissão cliente.
            Assert.IsFalse(conexao.PermissaoDeEscrita,"Deveria ter a permissão de processamento de escrita desativada");
            Assert.IsFalse(conexao.PermissaoDeLeitura,"Deveria ter a permissão de processamento de leitura desativada");
            Assert.IsFalse(conexao.threadProcessarLeitura.IsAlive,"Sem a permissão a thread de processamento de leitura deveria ter encerrado");
        }

        [TestMethod]
        public void ReceberMensagemFimEdicaoTarefaTest()
        {

            //Configurações do Servidor
            IPAddress ipServidor = IPAddress.Parse("127.0.0.1");
            const int portaServidor = 8007;

            //Instanciar e iniciar o Servidor (Listener)
            TcpListener servidor = new TcpListener(ipServidor,portaServidor);
            servidor.Start();
            Queue<MensagemDto> filaProcessamento = new Queue<MensagemDto>();

            //Instanciar os Clientes (2x TcpClient)
            TcpClient cliente1 = new TcpClient();
            TcpClient cliente2 = new TcpClient();
            const string login1 = "Joao";
            const string login2 = "Pedro";
            const string oidCronograma = "C1";

            //Tentativa de conexao do cliente1
            cliente1.Connect(ipServidor,portaServidor);
            // Servidor aceitar a conexao do cliente1 e armazenar
            TcpClient cliente1EmServidor = servidor.AcceptTcpClient();

            //Tentativa de conexao do cliente2
            cliente2.Connect(ipServidor,portaServidor);
            // Servidor aceitar a conexao do cliente1 e armazenar
            TcpClient cliente2EmServidor = servidor.AcceptTcpClient();

            //Iniciar ConexaoCliente do cliente1 
            ConexaoClienteMock c1 = new ConexaoClienteMock(login1,cliente1EmServidor,filaProcessamento) { PermissaoDeEscrita = false };

            //Iniciar ConexaoCliente do cliente2 
            ConexaoClienteMock c2 = new ConexaoClienteMock(login2,cliente2EmServidor,filaProcessamento) { PermissaoDeEscrita = false };

            //Criar mensagemDto novoUsuarioConectado para o cliente1
            MensagemDto m1 = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { login1 },oidCronograma);
            string m1Json = JsonConvert.SerializeObject(m1);
            m1Json = TcpUtil.AdicionarStringProtecaoDeIntegridade(m1Json);

            //Criar mensagemDto novoUsuarioConectado para o cliente2
            MensagemDto m2 = Mensagem.RnCriarMensagemNovoUsuarioConectado(new string[] { login2 },oidCronograma);
            string m2Json = JsonConvert.SerializeObject(m2);
            m2Json = TcpUtil.AdicionarStringProtecaoDeIntegridade(m2Json);

            //Enviar a mensagem Json do primeiro cliente atráves do tcp cliente1;
            TcpUtil.EnviarMensagemTcp(m1Json,cliente1);


            

            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count > 0; });

            //Enviar a  mensagem Json do segundo cliente atráves do tcp cliente2;
            TcpUtil.EnviarMensagemTcp(m2Json,cliente2);
            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count > 1; });

            Assert.AreEqual(2,filaProcessamento.Count,"Deveria ser enfileirada 2 objetos MensagemDto");

            MensagemDto m3 = Mensagem.RnCriarMensagemFinalizarEdicaoTarefa("T1","gabriel","C1");
            string m3Json = JsonConvert.SerializeObject(m3);
            m3Json = TcpUtil.AdicionarStringProtecaoDeIntegridade(m3Json);

            TcpUtil.EnviarMensagemTcp(m3Json,cliente2);
            ControleDeEsperaUtil.AguardarAte(() => { return filaProcessamento.Count > 2; });

            c1.Desconectar();
            c2.Desconectar();
            servidor.Stop();
            cliente1.Close();
            cliente2.Close();
            cliente1EmServidor.Close();
            cliente2EmServidor.Close();
        }

        [TestMethod]
        public void RnEnviarMensagensQuePossuemUsuariosQuandoAMensagemForDeMovimentacaoPosicaoTarefa() 
        {
            //const string ipServidor = "127.0.0.1";
            //const int porta = 8090;
            ////iniciando servidor de escuta
            //TcpListener servidor = new TcpListener(IPAddress.Parse(ipServidor),porta);
            //servidor.Start();
            ////iniciando cliente de comunicação
            //TcpClient tcpUsuario = new TcpClient();
            ////solicitando a conexão com o servidor de escuta
            //tcpUsuario.Connect(IPAddress.Parse(ipServidor) ,porta);

            ////servidor aceitando a conexao do cliente armazenada do lado do servidor
            //TcpClient tcpEmServidor =  servidor.AcceptTcpClient();

            ////iniciando a fila de mensagens a serem processadas
            //Queue<MensagemDto> filaMensagens = new Queue<MensagemDto>();
            //Dictionary<string,Int16> tarefasImpactadas = new Dictionary<string,short>();
            //tarefasImpactadas.Add("T3",3);
            //tarefasImpactadas.Add("T4",4);
            //tarefasImpactadas.Add("T5",5);
            //////criando mensagem de movimentacao de tarefa
            //MensagemDto mensagem = Mensagem.RnCriarMensagemMovimentacaoTarefa(2,5,"T2",tarefasImpactadas,"Mario","C1");
            ////string mensagemJson = JsonConvert.SerializeObject(mensagem);
            ////mensagemJson = TcpUtil.AdicionarStringProtecaoDeIntegridade(mensagemJson);
            //filaMensagens.Enqueue(mensagem);
            //ConexaoCliente conexao = new ConexaoCliente("Joao" ,tcpEmServidor ,filaMensagens);
            //conexao.FilaEscrita.Enqueue(mensagem);
            //ControleDeEsperaUtil.AguardarAte(() => { return tcpUsuario.Available > 0; });
            //string mensagemJson = TcpUtil.ReceberMensagemTcp(tcpUsuario);
            //mensagemJson = TcpUtil.RemoverStringProtecaoDeIntegridade(mensagemJson);
            //MensagemDto mensagemEsperada = Mensagem.DeserializarMensagemDto(mensagemJson);
            //Assert.AreEqual(mensagem.Tipo ,mensagemEsperada.Tipo ,"As mensagens deveriam ser do mesmo tipo");
            //Assert.AreEqual("T2" ,(string)mensagemEsperada.Propriedades[Constantes.OIDTAREFA],"o oid da tarefa recebida deveria ser o mesmo da tarefa enviada");
            //Assert.AreEqual("C1" ,(string)mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA]);
            //CollectionAssert.AreEqual(tarefasImpactadas ,(Dictionary<string ,Int16>)mensagemEsperada.Propriedades[Constantes.TAREFAS_IMPACTADAS]);
        }

        [TestMethod]
        public void RemoverProprioUsuarioDoVetorUsuariosDaMensagemDtoTest() 
        {
           // Assert.Inconclusive("Teste para simular a remoção de um usuário de uma mensagem dto que possua uma hash de usuários montar cenário e expectativas");
            string[] usuarios = new string[]{"Joao","Paulo","Gabriel","Anderson"};
            string[] usuariosRefeitos = new string[]{"Paulo","Gabriel","Anderson"};
            string[] tarefas = new string[]{"T1","T2","T3","T4","T5"};
            Dictionary<string,Int16> tarefasImpactadas = new Dictionary<string,short>();
            tarefasImpactadas.Add("T1",1);
            tarefasImpactadas.Add("T2",2);
            tarefasImpactadas.Add("T3",3);
            tarefasImpactadas.Add("T4",4);
            tarefasImpactadas.Add("T5",5);
            DateUtil.CurrentDateTime = DateTime.Now;
            /*
             Testar a mensagem com os tipos:
             *  RnCriarMensagemConexaoEfetuadaComSucesso
             *  RnCriarMensagemNovoUsuarioConectado
             *  RnCriarMensagemUsuarioDesconectado
             *  RnCriarMensagemMovimentacaoTarefa
             */

            MensagemDto mensagemConexaoEfetuadaComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso(usuarios ,"C1",null);
            MensagemDto mensagemNovoUsuarioConectado = Mensagem.RnCriarMensagemNovoUsuarioConectado(usuarios ,"C1");
            //Simulação Quando houver somente o proprio usuário se desconectando (Resumir mensagens pode resumir a mensagem e adicionar outros usuários)
            MensagemDto mensagemUsuarioDesconectado = Mensagem.RnCriarMensagemUsuarioDesconectado("Joao","C1");
            //Simulando quando vários usuários solicitaram a desconexão e deve ser removido a si mesmo da mensagem
            MensagemDto mensagemUsuarioDesconectado2 = Mensagem.RnCriarMensagemUsuarioDesconectado(usuarios ,"C1");
            //Simulando quando usuário efetuar a movimentação de tarefas, o usuário não deverá receber notificação da sua própria ação
            MensagemDto mensagemMovimentacaoTarefa = Mensagem.RnCriarMensagemMovimentacaoTarefa(1,5,"T1" ,tarefasImpactadas ,"Joao" ,"C1", DateUtil.CurrentDateTime);
            //Simulando quando outro usuário efetuar a movimentação de tarefas, o usuário deverá receber notificação ação de movimentação
            MensagemDto mensagemMovimentacaoTarefa2 = Mensagem.RnCriarMensagemMovimentacaoTarefa( 1, 5, "T1", tarefasImpactadas, "Pedro", "C1", DateUtil.CurrentDateTime );

            ConexaoCliente conexao = new ConexaoCliente("Joao" ,new TcpClient() ,new Queue<MensagemDto>());
            
            mensagemConexaoEfetuadaComSucesso = conexao.RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto(mensagemConexaoEfetuadaComSucesso);
            mensagemNovoUsuarioConectado = conexao.RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto(mensagemNovoUsuarioConectado);
            mensagemUsuarioDesconectado = conexao.RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto(mensagemUsuarioDesconectado);
            mensagemUsuarioDesconectado2 = conexao.RemoverProprioUsuarioDoVetorUsuariosDaMensagemDto(mensagemUsuarioDesconectado2);

            //asserts

            CollectionAssert.AreEquivalent(usuariosRefeitos ,(string[])mensagemConexaoEfetuadaComSucesso.Propriedades[Constantes.USUARIOS],"Não deveria ter recebido a notificação relativa a si mesmo");
            CollectionAssert.AreEquivalent(usuariosRefeitos ,(string[])mensagemNovoUsuarioConectado.Propriedades[Constantes.USUARIOS] ,"Não deveria ter recebido a notificação relativa a si mesmo");
            CollectionAssert.AreEquivalent(usuariosRefeitos ,(string[])mensagemUsuarioDesconectado2.Propriedades[Constantes.USUARIOS] ,"Não deveria ter recebido a notificação relativa a si mesmo");
            CollectionAssert.AreEquivalent(usuariosRefeitos ,(string[])mensagemConexaoEfetuadaComSucesso.Propriedades[Constantes.USUARIOS] ,"Não deveria ter recebido a notificação relativa a si mesmo");

        }

        [TestMethod]
        public void RemocaoProprioUsuarioDoDicionarioAutoresTarefaDaMensagemDto() 
        {
            string[] colaboradores = new string[] { "Joao","Paulo","Pablo","Mario"};
            ConexaoCliente conexao = new ConexaoCliente("Joao" ,new TcpClient() ,new Queue<MensagemDto>());
            Dictionary<string,string> autoresAcao = new Dictionary<string,string>();
            int i = 0;
            colaboradores.ToList().ForEach( ( o ) => { i++; autoresAcao.Add( string.Format( "T{0}", i ), o ); } );
            Dictionary<string ,string> autoresAcaoResultadoEsperado = new Dictionary<string ,string>(autoresAcao);
            MensagemDto mensagemInicioEdicaoTarefa = Mensagem.RnCriarMensagemInicioEdicaoTarefaResumida(autoresAcao ,"C1");

            MensagemDto mensagemProcessada = conexao.RemocaoProprioUsuarioDoDicionarioAutoresAcao(mensagemInicioEdicaoTarefa);
            
            autoresAcaoResultadoEsperado.Remove("T1");

            CollectionAssert.AreEquivalent(autoresAcaoResultadoEsperado ,(Dictionary<string ,string>)mensagemProcessada.Propriedades[Constantes.AUTORES_ACAO],"Deveriam possuir os mesmo valores sendo removido o usuário Joao");
        }

        [TestMethod]
        public void AindaContemAutoresAcaQuandoRestaremAutoresAcaoAposRemocaoTest() 
        {
            string[] colaboradores = new string[] { "Joao" ,"Paulo" ,"Pablo" ,"Mario" };
            ConexaoCliente conexao = new ConexaoCliente("Joao" ,new TcpClient() ,new Queue<MensagemDto>());
            Dictionary<string ,string> autoresAcao = new Dictionary<string ,string>();
            int i = 0;
            colaboradores.ToList().ForEach((o) => { i++; autoresAcao.Add(o ,string.Format("T{0}" ,i)); });
            Dictionary<string ,string> autoresAcaoResultadoEsperado = new Dictionary<string ,string>(autoresAcao);
            MensagemDto mensagemInicioEdicaoTarefa = Mensagem.RnCriarMensagemInicioEdicaoTarefaResumida(autoresAcao ,"C1");

            MensagemDto mensagemProcessada = conexao.RemocaoProprioUsuarioDoDicionarioAutoresAcao(mensagemInicioEdicaoTarefa);
            Assert.IsTrue(ConexaoCliente.AindaContemAutoresAcao(mensagemProcessada) ,"Deveria ainda conter usuários pois foi removido apenas o Joao da Mensagem");

            ConexaoCliente conexao2 = new ConexaoCliente("Joao" ,new TcpClient() ,new Queue<MensagemDto>());
            autoresAcao = new Dictionary<string ,string>();
            autoresAcao.Add("Joao" ,"T1");
            mensagemInicioEdicaoTarefa = Mensagem.RnCriarMensagemInicioEdicaoTarefaResumida(autoresAcao ,"C1");
            mensagemProcessada = conexao.RemocaoProprioUsuarioDoDicionarioAutoresAcao(mensagemInicioEdicaoTarefa);
        }

        [TestMethod]
        public void AindaContemAutoresAcaQuandoNaoRestaremAutoresAcaoAposRemocaoTest()
        {
            ConexaoCliente conexao = new ConexaoCliente("Joao" ,new TcpClient() ,new Queue<MensagemDto>());
            Dictionary<string ,string> autoresAcao = new Dictionary<string ,string>();
            autoresAcao.Add("T1","Joao");
            MensagemDto mensagemInicioEdicaoTarefa = Mensagem.RnCriarMensagemInicioEdicaoTarefaResumida(autoresAcao ,"C1");
            MensagemDto mensagemProcessada = conexao.RemocaoProprioUsuarioDoDicionarioAutoresAcao(mensagemInicioEdicaoTarefa);
            Assert.IsFalse(ConexaoCliente.AindaContemAutoresAcao(mensagemProcessada) ,"Não deveria ainda conter usuários "+
                "pois foi removido o unico autor existente na mensagem 'Joao'");
        }
    }
}
