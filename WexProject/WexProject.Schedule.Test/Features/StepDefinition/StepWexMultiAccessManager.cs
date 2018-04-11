using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Domains;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Libs;
using WexProject.Library.Libs.Test;
using WexProject.Schedule.Test.Helpers.Mocks;
using System.Diagnostics;
using WexProject.MultiAccess.Library.Delegates;
using WexProject.Schedule.Test.Utils;
using WexProject.Schedule.Test.Features.Helpers;
using WexProject.Schedule.Test.UnitTest;
using WexProject.Schedule.Test.Fixtures.Factory;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding]
    public class StepWexMultiAccessManager : BaseEntityFrameworkTest
    {
        #region Atributos Estáticos
        /// <summary>
        /// Armazenada a porta de conexão com o servidor
        /// </summary>
        private static int portaServidor = 8000;
        #endregion

        #region Constantes
        /// <summary>
        /// Constante Endereço Ip
        /// </summary>
        public const string EnderecoIpServidor = "127.0.0.1";
        #endregion

        #region Propriedades
        /// <summary>
        /// Armarzenar todos Clientes de comunicação
        /// </summary>
        List<WexMultiAccessClientMock> ConexoesClient { get; set; }
        /// <summary>
        /// Armazena a porta tcp de conexão
        /// </summary>
        public int PortaTcp { get; set; }
        #endregion

        #region Métodos Auxiliares
        /// <summary>
        /// Método responsável por retornar a porta do servidor
        /// </summary>
        /// <returns></returns>
        public static int GetPortaServidor()
        {
            return portaServidor++;
        }

        /// <summary>
        /// Instanciar e configurar um WexMultiAccessClient
        /// </summary>
        /// <param name="cronograma">nome do cronograma</param>
        /// <param name="enderecoIp">Endereço ip do servidor</param>
        /// <param name="porta">Porta do servidor</param>
        /// <returns>Um MultiAccessClient configurado para o cenário</returns>
        public WexMultiAccessClientMock CriarMultiAccessClient( string login, string oidCronograma )
        {
            WexMultiAccessClientMock cliente = new WexMultiAccessClientMock()
            {
                Login = login,
                OidCronograma = oidCronograma,
                EnderecoIp = EnderecoIpServidor,
                Porta = PortaTcp
            };
            ConfiguarEventosSemAcao( cliente );
            ConfigurarEventoAoSerCriadaNovaTarefa( cliente );
            ConfigurarEventoAoServidorDesconectar( cliente );
            ConfigurarEventoAoSerAutenticado( cliente );
            ConfigurarEventoAoUsuarioDesconectar( cliente );
            ConfigurarEventoAoIniciarEdicaoTarefa( cliente );
            ConfigurarEventoAoFinalizadaEdicaoTarefa( cliente );
            ConexoesClient.Add( cliente );
            return cliente;
        }

        #region Métodos para configuração de retorno de eventos
        /// <summary>
        /// Mockar os eventos em que o comportamentos de disparo são irrelevantes ao contexto do bdd
        /// </summary>
        /// <param name="cliente">wex client a ser mockado</param>
        private static void ConfiguarEventosSemAcao( WexMultiAccessClientMock cliente )
        {
            //criado para mockar o disparo dos eventos de tela não realizando nenhuma ação
            MensagemDtoEventHandler disparoEventoNaoPrecisaFazerNada = ( mensagem ) => { };

            //empilhamento dos eventos
            cliente.AoFalharConexaoNoServidor += ( oidCronogramaAtual, loginAccessClient ) => { };
            cliente.AoConectarNovoUsuario += ( mensagem, loginAccessClient ) => { };
            cliente.AoIniciarEdicaoDadosCronograma += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerRecusadaEdicaoDadosCronograma += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerPermitidaEdicaoDadosCronograma += disparoEventoNaoPrecisaFazerNada;
            cliente.AoOcorrerMovimentacaoPosicaoTarefa += disparoEventoNaoPrecisaFazerNada;
            cliente.AoServidorDesconectar += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerAutorizadaEdicaoTarefa += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerRecusadaEdicaoTarefa += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerExcluidaTarefaPorOutroUsuario += disparoEventoNaoPrecisaFazerNada;
            cliente.ExecutarExclusaoTarefa += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerNotificadoAlteracaoDadosCronograma += disparoEventoNaoPrecisaFazerNada;
            cliente.AoReceberConexaoRecusada += disparoEventoNaoPrecisaFazerNada;
            cliente.AoSerDesconectado += () => { };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoFinalizadaEdicaoTarefa( WexMultiAccessClientMock cliente )
        {
            string key;
            cliente.AoSerFinalizadaEdicaoTarefaPorOutroUsuario += ( mensagem ) =>
            {
                Dictionary<string, string> autoresAcao = mensagem.Propriedades[Constantes.AUTORES_ACAO] as Dictionary<string, string>;
                foreach(var item in autoresAcao)
                {
                    key = StepContextUtil.CriarKeyEventoFimEdicaoTarefa( cliente.Login, item.Key,item.Value );
                    StepContextUtil.SalvarKey( key );
                    key = StepContextUtil.CriarKeyRecebeuAtualizacaoEdicaoTarefa( cliente.Login, item.Key );
                    StepContextUtil.SalvarKey( key );
                }
            };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoIniciarEdicaoTarefa( WexMultiAccessClientMock cliente )
        {
            cliente.AoIniciarEdicaoTarefa += ( mensagem ) =>
            {
                Dictionary<string, string> autoresAcao = mensagem.Propriedades[Constantes.AUTORES_ACAO] as Dictionary<string, string>;
                string key;
                foreach(var item in autoresAcao)
                {
                    key = StepContextUtil.CriarKeyEventoAoIniciarEdicaoTarefa( cliente.Login, cliente.OidCronograma, item.Value );
                    StepContextUtil.SalvarKey( key );
                }
            };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoSerCriadaNovaTarefa( WexMultiAccessClientMock cliente )
        {
            cliente.AoSerCriadaNovaTarefa += ( mensagem ) =>
            {
                string oidNovaTarefa = mensagem.Propriedades[Constantes.OIDTAREFA] as string;
                string key = StepContextUtil.CriarKeyEventoAoSerCriadaNovaTarefa( cliente.Login, cliente.OidCronograma, oidNovaTarefa );
                StepContextUtil.SalvarKey( key );
            };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoUsuarioDesconectar( WexMultiAccessClientMock cliente )
        {
            cliente.AoUsuarioDesconectar += ( mensagem ) =>
            {
                string[] usuarios = (string[])mensagem.Propriedades[Constantes.USUARIOS];
                string key;
                for(int i = 0; i < usuarios.Length; i++)
                {
                    key = StepContextUtil.CriarKeyEventoUsuarioDesconectado( cliente.Login, cliente.OidCronograma, usuarios[i] );
                    ScenarioContext.Current.Set( true, key );
                }
            };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoSerAutenticado( WexMultiAccessClientMock cliente )
        {
            cliente.AoSerAutenticadoComSucesso += ( mensagem ) =>
            {
                Debug.WriteLine( string.Format( "AccessClient {0} - Usuarios Online:", cliente.Login ) );
                string[] usuarios = (string[])mensagem.Propriedades[Constantes.USUARIOS];
                Dictionary<string, string> EdicoesExistentes = mensagem.Propriedades[Constantes.EDICOES_CRONOGRAMA] as Dictionary<string, string>;
                string key;
                foreach(var item in EdicoesExistentes)
                {
                  key =  StepContextUtil.CriarKeyTarefaJaEstavamEmEdicao( cliente.Login, item.Key, item.Value );
                  StepContextUtil.SalvarKey( key );
                }
                foreach(var usuario in usuarios)
                {
                    Debug.WriteLine( usuario );
                }
            };
        }

        /// <summary>
        /// Configurar comportamento relevante as expectativas do bbd
        /// </summary>
        /// <param name="cliente">wex client que recebera o comportamento do evento</param>
        private static void ConfigurarEventoAoServidorDesconectar( WexMultiAccessClientMock cliente )
        {
            cliente.AoServidorDesconectar += ( mensagem ) =>
            {
                string key = StepContextUtil.CriarKeyEventoServidorDesconectado( cliente.Login, cliente.OidCronograma );
                StepContextUtil.SalvarKey( key );
            };
        } 
        #endregion

        #endregion

        #region Configuração de cenário
        /// <summary>
        /// Método utilizado para reinicializar os cronogramas conectados ao inicializar
        /// </summary>
        [BeforeScenario]
        public void Inicializar()
        {
            ConexoesClient = new List<WexMultiAccessClientMock>();
            PortaTcp = GetPortaServidor();
        }

        /// <summary>
        /// Descartar o manager após a finalização de cada cenário
        /// </summary>
        [AfterScenario]
        public void Descartar()
        {
            if(manager != null)
                manager.Desconectar();
        }
        #endregion

        #region Given
        public  WexMultiAccessManagerMock manager;

        [Given( @"que o servidor esta desligado" )]
        public  void DadoQueOServidorEstaDesligado()
        {
            if(manager == null)
            {
                manager = new WexMultiAccessManagerMock();
            }
        }

        [Given( @"que o servidor esta ligado" )]
        public void DadoQueOServidorEstaLigado()
        {
            manager = new WexMultiAccessManagerMock()
            {
                EnderecoIp = EnderecoIpServidor,
                Porta = PortaTcp,
                TempoMaximoAguardarIdentificacao = 2500
            };
            manager.Conectar();
            StepContextUtil.SalvarInstanciaManager( manager );
        }

        [Given( @"que o servidor contenha o\(s\) colaborador\(es\) (('[A-Za-z\s]+',?[\s]*)+) conectado\(s\) no cronograma '([\w\s]+)'" )]
        public void DadoQueOServidorContenhaOSColaboradorEsConectadoSNoCronograma( string colaboradores, string naoUsado, string cronograma )
        {
            string[] cols = colaboradores.Split( ',' );
            WexMultiAccessClientMock cliente;
            string login;
            foreach(string colaborador in cols)
            {
                login = colaborador.Replace( "\'", "" );
                login = login.Trim();
                string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
                cliente = CriarMultiAccessClient( login, oidCronograma );
                cliente.Conectar();
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    return manager.ColaboradorConectado( login, oidCronograma );
                } );
                StepContextUtil.SalvarAccessClientNoContextoDoCenario( cliente );
            }
        }
        #endregion

        #region When
        [When( @"o servidor desconectar" )]
        public void QuandoOServidorDesconectar()
        {
            ControleDeEsperaUtil.AguardarAte( () => { return manager.UsuariosConectados.Count > 0; } );
            manager.Desconectar();
        }

        [When( @"o servidor receber a solicitacao de alteracao do nome do cronograma '([\w\s]+)' pelo colaborador '([\w\s]+)'" )]
        public void QuandoOServidorReceberASolicitacaoDeAlteracaoDoNomeDoCronogramaPeloColaborador( string cronograma, string login )
        {
            List<MensagemDto> mensagens = null;
            MensagemDto mensagemEsperada = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagens = new List<MensagemDto>( manager.ListaTodasMensagensProcessadas );
                mensagemEsperada = mensagens.FirstOrDefault( o => o.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ) && o.Propriedades[Constantes.AUTOR_ACAO].ToString() == login && o.Tipo == CsTipoMensagem.InicioEdicaoNomeCronograma );
                return mensagemEsperada != null;
            } );

            Assert.IsNotNull( mensagemEsperada, string.Format( "Deveria ter recebido a mensagem enviada pelo colaborador {0}", login ) );
            Assert.AreEqual( 1, manager.CronogramasNomeEmEdicao.Count, "Deveria possuir 1 cronograma com nome em edição" );
            string oidCronograma = mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA] as string;
            Assert.IsNotNull( oidCronograma, "Deveria ter recebido o oid do cronograma que possui o nome a ser alterado" );
        }

        [When( @"o cronograma '([\w\s]+)' for aberto pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaC1ForAbertoPeloColaboradorJoao( string cronograma, string login )
        {
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            WexMultiAccessClientMock client = CriarMultiAccessClient( login, oidCronograma );
            client.Conectar();
            StepContextUtil.SalvarAccessClientNoContextoDoCenario( client );
        }

        #endregion

        #region Then

        [Then( @"o servidor nao deve possuir o colaborador '([\w\s]+)'  em sua lista de usuarios conectados ao cronograma '([\w\s]+)'" )]
        public  void EntaoOServidorNaoDevePossuirOColaboradorJoaoEmSuaListaDeUsuariosConectadosAoCronogramaC1( string Login, string cronograma )
        {
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            Assert.IsFalse( manager.ColaboradorConectado( Login, oidCronograma ) );
        }

        [Then( @"o servidor deve ser comunicado que que o\(s\) colaborador\(es\) '([\w\s]+)' esta\(o\) conectado\(s\) ao cronograma '([\w\s]+)'" )]
        public void EntaoOServidorDeveSerComunicadoQueQueOSColaboradorEsJoaoEstaOConectadoSAoCronograma01( string login, string cronograma )
        {
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ColaboradorConectado( login, oidCronograma ); } );
            Assert.IsTrue( manager.ColaboradorConectado( login, oidCronograma ), "Deveria encontrar o colaborador João no dicionário de usuários conectados" );
        }

        [Then( @"o servidor deve ser comunicado que o colaborador '([\w\s]+)' se desconectou" )]
        public void EntaoOServidorDeveSerComunicadoQueOColaboradorSeDesconectou( string login )
        {
            bool recebeu = false;
            List<MensagemDto> mensagensFiltradas = null;
            string[] usuarios = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagensFiltradas = new List<MensagemDto>( manager.ListaTodasMensagensProcessadas ).Where( o => o.Tipo.Equals( CsTipoMensagem.UsuarioDesconectado ) ).ToList();
                if(mensagensFiltradas != null)
                {
                    foreach(var item in mensagensFiltradas)
                    {
                        usuarios = (string[])item.Propriedades[Constantes.USUARIOS];
                        if(usuarios.Contains( login ))
                        {
                            recebeu = true;
                            break;
                        }
                    }
                }
                return recebeu;
            }, 7 );
            Assert.IsTrue( recebeu, string.Format( "O deveria possuir a mensagem de que {0} se desconectou",login ) );
        }

        [Then( @"o servidor devera conter na lista de colaboradores conectados o colaborador '([\w\s]+)' no cronograma '([\w\s]+)'" )]
        public void EntaoOServidorDeveraConterNaListaDeColaboradoresConectadosOColaboradorNoCronograma( string login, string cronograma )
        {
            string oidCronograma = StepCronograma.GetOidCronograma( cronograma );
            Assert.IsTrue( manager.ColaboradorConectado( login, oidCronograma ),"Deveria ter sido encontrado como colaborador conectado" );
        }


        [Then( @"o servidor nao devera ter em sua lista de colaboradores online no cronograma '([\w\s]+)' o\(s\) colaborador\(es\) '([\w\s]+)'" )]
        public void EntaoOServidorNaoDeveraTerEmSuaListaDeColaboradoresOnlineNoCronogramaOSColaboradorEs( string cronograma, string login )
        {
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            ControleDeEsperaUtil.AguardarAte( () => { return !manager.ColaboradorConectado( login, oidCronograma ); } );
            Assert.IsFalse( manager.ColaboradorConectado( login, oidCronograma ), "Deveria encontrar o colaborador João no dicionário de usuários conectados" );
        }

        [Then( @"o servidor deve ser comunicado que a tarefa '([\w\s]+)' do cronograma '([\w\s]+)' esta sendo editada pelo colaborador '([\w\s]+)'" )]
        public void EntaoOServidorDeveSerComunicadoQueATarefaDoCronogramaEstaSendoEditadaPeloColaborador( string tarefa, string cronograma, string login )
        {
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            string oidTarefa = StepCronograma.CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
            List<MensagemDto> mensagens = null;
            MensagemDto mensagemFiltrada = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagens = new List<MensagemDto>( manager.ListaTodasMensagensProcessadas );

                mensagemFiltrada = mensagens.FirstOrDefault
                    ( o =>
                        o.Tipo == CsTipoMensagem.InicioEdicaoTarefa &&
                        o.Propriedades[Constantes.AUTOR_ACAO].ToString() == login &&
                        o.Propriedades[Constantes.OIDTAREFA].ToString() == oidTarefa
                    );

                return mensagemFiltrada != null;
            } );

            Assert.IsNotNull( mensagemFiltrada, "Não deveria ter sido nula, colaborador deveria ter recebido a mensagem pesquisada" );
            Assert.AreEqual( login, mensagemFiltrada.Propriedades[Constantes.AUTOR_ACAO].ToString(), string.Format( "O colaborador autor da mensagem deveria ser {0}", login ) );
            Assert.AreEqual( oidTarefa, mensagemFiltrada.Propriedades[Constantes.OIDTAREFA].ToString(), string.Format( "O Oid da tarefa editada deveria ser {0}", oidTarefa ) );
            Assert.AreEqual( oidCronograma, mensagemFiltrada.Propriedades[Constantes.OIDCRONOGRAMA].ToString(),
                string.Format( "O Oid do Cronograma atual deveria ser {0}", oidCronograma ) );
        }

        [Then( @"o servidor devera ser comunicado de que a\(s\) tarefa\(s\) '([\w\s]+)', '([\w\s]+)' estao liberadas para edicao" )]
        public void EntaoOServidorDeveraSerComunicadoDeQueASTarefaSEstaoLiberadasParaEdicao( string tarefa1, string tarefa2 )
        {
            ControleDeEsperaUtil.AguardarAte( () => { return manager.ListaTodasMensagensProcessadas.Where( o => o.Tipo == CsTipoMensagem.EdicaoTarefaFinalizada ).Count() == 2 && !manager.TarefasEmEdicao.ContainsKey( tarefa1 ) && !manager.TarefasEmEdicao.ContainsKey( tarefa2 ); } );
            Assert.IsFalse( manager.TarefasEmEdicao.ContainsKey( tarefa1 ) );
            Assert.IsFalse( manager.TarefasEmEdicao.ContainsKey( tarefa2 ) );
        }

        [Then( @"o servidor devera receber a solicitacao para excluir a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) no cronograma '([\w\s]+)'" )]
        public void EntaoOServidorDeveraReceberASolicitacaoParaExcluirASTarefaSNoCronograma( string tarefasString, string naoUsada, string cronograma )
        {
            List<string> tarefasEsperadas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> oidTarefas;
            Dictionary<string, string> TarefasDic = new Dictionary<string, string>();
            Dictionary<string, bool> exclusoesSolicitadas = new Dictionary<string, bool>();
            string oidTarefa;
            foreach(string tarefa in tarefasEsperadas)
            {
                oidTarefa = StepCronograma.CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
                exclusoesSolicitadas.Add( oidTarefa, false );
                TarefasDic.Add( oidTarefa, tarefa );
            }
            oidTarefas = exclusoesSolicitadas.Select( o => o.Key ).ToList();
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return manager.SolicitacoesExclusao.ContainsKey( oidCronograma ) && manager.SolicitacoesExclusao[oidCronograma].Count == tarefasEsperadas.Count;
            } );
            foreach(string oid in oidTarefas)
            {
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    return manager.SolicitacoesExclusao[oidCronograma].Contains( oid );
                } );

                Assert.IsTrue( manager.SolicitacoesExclusao[oidCronograma].Contains( oid ), string.Format( "O cronograma {0} deveria ter recebido a solicitação da exclusão da tarefa {1}", cronograma, TarefasDic[oid] ) );
            }
        }

        [Then( @"o servidor devera receber a confirmacao de que o colaborador '([\w\s]+)' excluiu a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) do cronograma '([\w\s]+)'" )]
        public void EntaoOServidorDeveraReceberAConfirmacaoDeQueOColaboradorExcluiuASTarefaSDoCronograma( string colaborador, string tarefasString, string naoUsado, string cronograma )
        {
            List<string> tarefas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            string oidCronograma = StepCronograma.CronogramasDic[cronograma].Oid.ToString();

            Dictionary<string, bool> tarefasExcluidas = new Dictionary<string, bool>();
            List<string> oidTarefas = new List<string>();
            string oidTarefa;
            foreach(string tarefa in tarefas)
            {
                oidTarefa = StepCronograma.CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
                oidTarefas.Add( oidTarefa );
                tarefasExcluidas.Add( oidTarefa, false );
            }
            MensagemDto mensagemFiltrada = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                //List<MensagemDto> mensagensManager = new List<MensagemDto>( manager.ListaTodasMensagensProcessadas );

                lock(manager.ListaTodasMensagensProcessadas)
                {
                    mensagemFiltrada = manager.ListaTodasMensagensProcessadas.FirstOrDefault(
                               o =>
                                   o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada &&
                                   o.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ) &&
                                   o.Propriedades[Constantes.AUTOR_ACAO].ToString() == colaborador &&
                                   o.Propriedades.ContainsKey( Constantes.OIDCRONOGRAMA ) &&
                                   o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == oidCronograma
                                   ); 
                }

                return mensagemFiltrada != null;
            }, 5 );

            List<MensagemDto> mensagens = new List<MensagemDto>
                (
                    manager.ListaTodasMensagensProcessadas.Where
                       (
                           o =>
                              o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada &&
                              o.Propriedades.ContainsKey( Constantes.AUTOR_ACAO ) &&
                              o.Propriedades[Constantes.AUTOR_ACAO].ToString() == colaborador &&
                              o.Propriedades.ContainsKey( Constantes.OIDCRONOGRAMA ) &&
                              o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == oidCronograma

                       ).ToList()
                );
            string[] tarefasExclusao;
            foreach(MensagemDto mensagem in mensagens)
            {
                tarefasExclusao = (string[])mensagem.Propriedades[Constantes.TAREFAS];
                foreach(string oidTarefaExcluida in oidTarefas)
                {
                    if(tarefasExcluidas.ContainsKey( oidTarefaExcluida ))
                        tarefasExcluidas[oidTarefaExcluida] = true;
                }
            }

            foreach(var tarefaExcluida in tarefasExcluidas)
            {
                Assert.IsTrue( tarefasExcluidas[tarefaExcluida.Key] );
            }
        }
        [Then( @"o servidor devera comunicar o colaborador '([\w\s]+)' que no cronograma '([\w\s]+)' as seguintes tarefa\(s\) estao em edicao:" )]
        public void EntaoOServidorDeveraComunicarOColaboradorQueNoCronogramaAsSeguintesTarefaSEstaoEmEdicao( string colaborador, string cronograma, Table table )
        {
            string oidTarefa;
            string key;
            IEnumerable<EdicaoTarefaHelper> edicoes = table.CreateSet<EdicaoTarefaHelper>();
            foreach(var edicao in edicoes)
            {
                oidTarefa = StepCronograma.GetOidTarefaNoCronograma( cronograma, edicao.Tarefa );
                key = StepContextUtil.CriarKeyTarefaJaEstavamEmEdicao( colaborador, oidTarefa, edicao.Autor );
                ControleDeEsperaUtil.AguardarAte( () => { return StepContextUtil.CenarioAtualContemAChave( key ); } );
                Assert.IsTrue(StepContextUtil.CenarioAtualContemAChave(key),
                    string.Format("{0} deveria ter sido avisado de que a tarefa {1} estava em edição pelo colaborador {2}"+
                    " no cronograma {3}",colaborador,edicao.Tarefa,edicao.Autor,cronograma));
            }
        }

        [Then( @"o cronograma '([\w\s]+)' nao deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que esta\(o\) sendo excluida\(s\) tarefa\(s\) no cronograma '(.*)'" )]
        public void EntaoOCronogramaNaoDeveComunicarAoSColaboradorEsDeQueEstaOSendoExcluidaSTarefaSNoCronograma( string cronograma, string colaboradoresString, string naoUsada1, string outroCronograma )
        {
            List<string> colaboradores = colaboradoresString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            Dictionary<string, bool> colaboradoresAvisados = new Dictionary<string, bool>();

            //Montando Dicionário em que nenhum colaborador foi avisado sobre a exclusão
            foreach(string colaboradorAtual in colaboradores)
            {
                colaboradoresAvisados.Add( colaboradorAtual, false );
            }

            WexMultiAccessClientMock clientAtual;
            string oidCronograma = StepCronograma.GetOidCronograma( cronograma );
            string oidOutroCronograma = StepCronograma.GetOidCronograma( outroCronograma );
            bool foiComunicado;
            List<MensagemDto> mensagens = null;
            MensagemDto mensagem;
            foreach(var colaboradorAtual in colaboradores)
            {
                foiComunicado = false;
                clientAtual = StepContextUtil.GetAccessClientNoContexto( colaboradorAtual, oidCronograma );
                ControleDeEsperaUtil.AguardarAte( () =>
                {

                    mensagens = new List<MensagemDto>( clientAtual.MensagensRecebidas );
                    mensagem = mensagens.FirstOrDefault(
                            o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada &&
                            !o.Propriedades[Constantes.OIDCRONOGRAMA].ToString().Equals( oidOutroCronograma )
                            );
                    if(mensagem != null)
                        foiComunicado = true;
                    return foiComunicado;
                } );
                Assert.IsFalse( foiComunicado, string.Format( "{0} não deveria ser comunicado de que houve exclusão em um cronograma diferente ('{1}')", colaboradorAtual, outroCronograma ) );
            }
        }


        [Then( @"o servidor deve detectar de que o\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) foram desconectado\(s\)" )]
        public void EntaoOServidorDeveDetectarDeQueOSColaboradorEsForamDesconectadoS( string colaboradores, string naoUsado )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            foreach(string colaborador in logins)
            {
                ControleDeEsperaUtil.AguardarAte( () => { return !manager.ColaboradorConectado( colaborador ); }, 10 );
                Assert.IsFalse( manager.ColaboradorConectado( colaborador ), "O Colaborador Não Deveria estar conectado ao manager" );
            }
        }

        [Then( @"o servidor devera permitir a edicao do nome do cronograma '([\w\s]+)' para o colaborador '([\w\s]+)'" )]
        public void EntaoOServidorDeveraPermitirAEdicaoDoNomeDoCronogramaParaOColaborador( string cronograma, string login )
        {
            WexMultiAccessClientMock cliente = StepCronograma.GetAccessClient( cronograma, login );
            string oidCronograma = StepCronograma.GetOidCronograma(cronograma);
            List<MensagemDto> mensagens = null;
            MensagemDto mensagem = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagens = new List<MensagemDto>( cliente.MensagensRecebidas );
                mensagem = mensagens.FirstOrDefault( o => o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == oidCronograma && o.Tipo == CsTipoMensagem.EdicaoNomeCronogramaPermitida );
                return mensagem != null;
            } );

            Assert.IsNotNull( mensagem, "Deveria ter recebido a mensagem do permissão de edição do nome do cronograma" );
            Assert.AreEqual( login, mensagem.Propriedades[Constantes.AUTOR_ACAO], string.Format( "O proprietario da mensagem deveria ser o colaborador {0}", login ) );
        }

        [Then( @"o servidor devera recusar a edicao do nome do cronograma '([\w\s]+)' para o colaborador '([\w\s]+)'" )]
        public void EntaoOServidorDeveraRecusarAEdicaoDoNomeDoCronogramaParaOColaborador( string cronograma, string login )
        {
            string oidCronograma = StepCronograma.GetOidCronograma( cronograma );
            WexMultiAccessClientMock cliente = StepContextUtil.GetAccessClientNoContexto( login,oidCronograma );
            List<MensagemDto> mensagens = null;
            MensagemDto mensagem = null;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagens = new List<MensagemDto>( cliente.MensagensRecebidas );
                mensagem = mensagens.FirstOrDefault( o => o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == oidCronograma && o.Tipo == CsTipoMensagem.EdicaoNomeCronogramaRecusada );
                return mensagem != null;
            } );

            Assert.IsNotNull( mensagem, "Deveria ter recebido a mensagem do permissão de edição do nome do cronograma" );
            Assert.AreEqual( login, mensagem.Propriedades[Constantes.AUTOR_ACAO], string.Format( "O proprietario da mensagem deveria ser o colaborador {0}", login ) );
        }

        [Then( @"o servidor nao deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) recebeu\(ram\) atualizacao\(oes\) no cronograma '([\w\s]+)'" )]
        public void EntaoOServidorNaoDeveComunicarAoSColaboradorEsDeQueASTarefaSRecebeuRamAtualizacaoOesNoCronograma( string colaboradores, string naoUsado, string tarefasOutroCronograma, string naoUsado2, string outroCronograma )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string[] tarefas = tarefasOutroCronograma.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidTarefa;
            string key;
            for(int i = 0; i < logins.Length; i++)
            {
                for(int x = 0; x < tarefas.Length; x++)
                {
                    oidTarefa = StepCronograma.GetOidTarefaNoCronograma( outroCronograma, tarefas[x] );
                    key = StepContextUtil.CriarKeyRecebeuAtualizacaoEdicaoTarefa( logins[i], oidTarefa );
                    ControleDeEsperaUtil.AguardarAte( () => { return StepContextUtil.CenarioAtualContemAChave( key ); } );
                    Assert.IsFalse( StepContextUtil.CenarioAtualContemAChave( key ), string.Format( "{0} não deveria ter sido comunicado da atualização da tarefa {1} que pertence outro cronograma '{2}'", logins[i], tarefas[x], outroCronograma ) );
                }
            }
        }

        #endregion
    }
}
