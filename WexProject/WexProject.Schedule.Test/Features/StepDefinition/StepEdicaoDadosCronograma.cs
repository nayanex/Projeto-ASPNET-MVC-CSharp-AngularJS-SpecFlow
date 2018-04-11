using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.Shared.DTOs.Planejamento;
using TechTalk.SpecFlow.Assist;
using WexProject.Schedule.Test.Helpers.Bind;
using Moq;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Library.Presenters;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.MultiAccess.Library.Components;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Test.Helpers.Utils;
using WexProject.MultiAccess.Library.Libs;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Helpers.CronogramaConfig;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding, Scope( Tag = "CronogramaPresenter" )]
    public class StepEdicaoDadosCronograma
    {
        #region Constantes
        /// <summary>
        /// Armazena o formato da chave para salvar um cronograma no contexto do BDD ex.: Cronograma_{0} => Cronograma_C1
        /// {0} -> Descricao do cronograma
        /// </summary>
        private const string FORMATO_CHAVE_CRONOGRAMA = "Cronograma_{0}";

        /// <summary>
        /// Armazena o formato da chave para salvar um usuario no contexto do BDD ex.: Usuario_{0} => Usuario_Gabriel
        /// {0} -> Nome do usuário
        /// </summary>
        private const string FORMATO_CHAVE_USUARIO = "Usuario_{0}";

        /// <summary>
        /// Armazena o formato da chave para salvar um WexMultiAccessClient no contexto do BDD ex.: Client_MOCK_{0}_{1} => Client_C1_Gabriel
        /// {0} -> Descricao do cronograma
        /// {1} -> Nome do usuário
        /// </summary>
        private const string FORMATO_CHAVE_ACCESS_CLIENT_MOCK = "Client_MOCK_{0}_{1}";

        /// <summary>
        /// Armazena o formato da chave para salvar um WexMultiAccessClient no contexto do BDD ex.: Client_MOCK_{0}_{1} => Client_C1_Gabriel
        /// 
        /// </summary>
        private const string FORMATO_CHAVE_VIEW_MOCK = "View_Mock_{0}_{1}";

        /// <summary>
        /// Armazena o formato da chave para salvar um Presenter no contexto do BDD ex.: Presenter_{0}_{1} => Presenter_C1_Gabriel
        /// {0} -> Descricao do cronograma
        /// {1} -> Nome do usuário
        /// </summary>
        private const string FORMATO_CHAVE_PRESENTER = "Presenter_{0}_{1}";

        /// <summary>
        /// Armazena a chave para salvar e recuperar o cronograma atual do contexto do BDD
        /// </summary>
        private const string CRONOGRAMA_ATUAL = "Cronograma_Atual";

        /// <summary>
        /// Armazena a chave para salvar e recuperar o usuario atual do contexto do BDD
        /// </summary>
        /// 
        private const string USUARIO_ATUAL = "Usuario_Atual";
        private const string CRONOGRAMA_PRESENTER = "Cronograma_Presenter";
        private const string ACCESS_CLIENT_MOCK = "accessClientMock"; 
        private const string VIEW_MOCK = "viewMock";

        #endregion

        #region Atributos
        private PlanejamentoServiceUtilStub planejamentoServiceStub;
        private GeralServiceUtilStub geralServiceStub; 
        #endregion

        #region Metodos de inicialização dos cenários
        [BeforeScenario]
        public void Inicializar()
        {
            InicializarServicosStubs();
        }

        [AfterScenario]
        public void FinalizarServicosStub()
        {
            planejamentoServiceStub = null;
            geralServiceStub = null;
        }

        private void InicializarServicosStubs()
        {
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
        } 
        #endregion

        #region Given

		[Given( @"que exista o servidor '(.*)' com a porta '(.*)'" )]
		public void DadoQueExistaOServidorComAPorta( string nomeServidor , int portaServidor )
		{
			CronogramaConfigStub.NomeServidor = nomeServidor;
			CronogramaConfigStub.PortaServidor = portaServidor;
			CronogramaPresenter.CronogramaConfig = new CronogramaConfigStub();
		}

        [Given( @"que exista\(m\) o\(s\) cronograma\(s\)" ), Scope( Tag = "Dto" )]
        public void DadoQueExistaMOSCronogramaS( Table table )
        {
            table.CreateSet<CronogramaBindHelper>()
                .Select( o => new CronogramaDto { TxDescricao = o.Nome, DtInicio = o.Inicio, DtFinal = o.Final } )
                .ToList()
                .ForEach( CriarEArmazenarCronogramaDto );
        }

        [Given( @"que o cronograma '(.*)' esta sendo utilizado pelo usuario '(.*)'" )]
        public void DadoQueOCronogramaEstaSendoUtilizadoPeloUsuario(  string descricaoCronograma, string nomeUsuario )
        {
            CronogramaDto cronograma;
            UsuarioCronogramaBindHelper usuario;

            CarregarDoContextoAtualBDD( CriarChave( FORMATO_CHAVE_CRONOGRAMA, descricaoCronograma ), out cronograma );
            CarregarDoContextoAtualBDD( CriarChave( FORMATO_CHAVE_USUARIO, nomeUsuario ), out usuario );

            AdicionarAoContextoAtualBDD( CRONOGRAMA_ATUAL, cronograma );
            AdicionarAoContextoAtualBDD( USUARIO_ATUAL, usuario );
        }


        [Given( @"que existam os usuarios no cronograma '(.*)':" )]
        public void DadoQueExistamOsUsuariosNoCronograma( string descricaoCronograma, Table table )
        {
            CronogramaDto cronograma;
            CarregarDoContextoAtualBDD( CriarChave( FORMATO_CHAVE_CRONOGRAMA, descricaoCronograma ), out cronograma );
            foreach(var usuario in table.CreateSet<UsuarioCronogramaBindHelper>())
            {
                Mock<IWexMultiAccessClient> clientMock = new Mock<IWexMultiAccessClient>();
                Mock<ICronogramaView> viewMock = new Mock<ICronogramaView>();
                clientMock.SetupAllProperties();
                viewMock.SetupAllProperties();

                var client = clientMock.Object;
                client.Porta = 8000;
                client.OidCronograma = cronograma.Oid.ToString();
                client.Login = usuario.Login;
                CronogramaPresenter presenter = new CronogramaPresenter( viewMock.Object, client );
				presenter.ConectarCronograma();
				var mensagemConectado = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso(new string[]{},cronograma.Oid.ToString(),new Dictionary<string,string>());
				clientMock.Raise( o => o.AoSerAutenticadoComSucesso += null , mensagemConectado );
                AdicionarAoContextoAtualBDD( CriarChave( FORMATO_CHAVE_USUARIO, usuario.Nome ), usuario );
                AdicionarAoContextoAtualBDD( CriarChave( FORMATO_CHAVE_ACCESS_CLIENT_MOCK, descricaoCronograma, usuario.Nome ), clientMock );
                AdicionarAoContextoAtualBDD( CriarChave( FORMATO_CHAVE_VIEW_MOCK, descricaoCronograma, usuario.Nome ), viewMock );
                AdicionarAoContextoAtualBDD( CriarChave( FORMATO_CHAVE_PRESENTER, descricaoCronograma, usuario.Nome ), presenter );
            }
        }

        #endregion
        #region When

        [When( @"o cronograma atual iniciar a edicao de dados do cronograma" )]
        public void QuandoOCronogramaAtualIniciarAEdicaoDeDadosDoCronograma()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.CronogramaPresenter );
            CronogramaPresenter presenter = hash[CRONOGRAMA_PRESENTER] as CronogramaPresenter;
            presenter.InicioEdicaoDadosCronograma();
        }

        [When( @"o cronograma atual recebeu a permissao de edicao dos dados" )]
        public void QuandoOCronogramaAtualRecebeuAPermissaoDeEdicaoDosDados()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.AccessClientMock );
            CronogramaDto cronograma = hash[CRONOGRAMA_ATUAL] as CronogramaDto;
            UsuarioCronogramaBindHelper usuario = hash[USUARIO_ATUAL] as UsuarioCronogramaBindHelper;
            Mock<IWexMultiAccessClient> clientMock = hash[ACCESS_CLIENT_MOCK] as Mock<IWexMultiAccessClient>;
            clientMock.Raise( o => o.AoSerPermitidaEdicaoDadosCronograma += null, Mensagem.RnCriarMensagemPermitirEdicaoNomeCronograma( cronograma.Oid.ToString(), usuario.Login ) );
        }

        [When( @"o cronograma atual encerrar a edicao dos dados" )]
        public void QuandoOCronogramaAtualEncerrarAEdicaoDosDados()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.CronogramaPresenter );
            var presenter = hash[CRONOGRAMA_PRESENTER] as CronogramaPresenter;
			presenter.FimEdicaoDadosCronograma();
        }

        [When( @"o cronograma atual recebeu a recusa de edicao dos dados" )]
        public void QuandoOCronogramaAtualRecebeuARecusaDeEdicaoDosDados()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.AccessClientMock );
            CronogramaDto cronograma = hash[CRONOGRAMA_ATUAL] as CronogramaDto;
            UsuarioCronogramaBindHelper usuario = hash[USUARIO_ATUAL] as UsuarioCronogramaBindHelper;
            Mock<IWexMultiAccessClient> clientMock = hash[ACCESS_CLIENT_MOCK] as Mock<IWexMultiAccessClient>;
            clientMock.Raise( o => o.AoSerRecusadaEdicaoDadosCronograma += null, Mensagem.RnCriarMensagemRecusaEdicaoNomeCronograma( cronograma.Oid.ToString(), usuario.Login ) );
        }


        #endregion

        #region Then


        [Then( @"o cronograma atual devera solicitar a permissao de edicao dos dados" )]
        public void EntaoOCronogramaDeveraSolicitarAPermissaoDeEdicaoDosDados()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.AccessClientMock );
            Mock<IWexMultiAccessClient> clientMock = hash[ACCESS_CLIENT_MOCK] as Mock<IWexMultiAccessClient>;
            clientMock.Verify( o => o.RnComunicarInicioEdicaoDadosCronograma(), Times.Once, "Deveria ter solicitado a permissão para edição dos dados do cronograma" );
        }

        [Then( @"o cronograma atual deve se manter em edicao" )]
        public void EntaoOCronogramaAtualDeveSeManterEmEdicao()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.CronogramaPresenter );
            var presenter = hash[CRONOGRAMA_PRESENTER] as CronogramaPresenter;

			Assert.AreEqual( true , presenter.GerenciadorEdicaoCronograma.EmEdicaoNaView , "Não deveria ter alterado a condição de edição do cronograma enquanto editando" );
        }

        [Then( @"o cronograma atual devera comunicar automaticamente o fim da edicao dos dados" )]
        public void EntaoOCronogramaAtualDeveraComunicarAutomaticamenteOFimDaEdicaoDosDados()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.AccessClientMock );
            Mock<IWexMultiAccessClient> clientMock = hash[ACCESS_CLIENT_MOCK] as Mock<IWexMultiAccessClient>;
            clientMock.Verify( o => o.RnComunicarAlteracaoDadosCronograma(), Times.Once(), "Deveria ter solicitado a permissão para edição dos dados do cronograma" );
        }

        [Then( @"o cronograma atual deve forcar o fim da edicao" )]
        public void EntaoOCronogramaAtualDeveForcarOFimDaEdicao()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.CronogramaPresenter | StepEdicaoCronogramaHelperFlag.ViewMock );
            var presenter = hash[CRONOGRAMA_PRESENTER] as CronogramaPresenter;
            var viewMock = hash[VIEW_MOCK] as Mock<ICronogramaView>;
            
            viewMock.Verify( o => o.ForcarFimEdicaoDadosCronograma(), Times.Once(), "Deveria ter forçado o fim da edição do cronograma" );
        }

        [Then( @"o cronograma devera atualizar os dados a partir do servico" )]
        public void EntaoOCronogramaDeveraAtualizarOsDadosAPartirDoServico()
        {
            var hash = CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag.CronogramaPresenter | StepEdicaoCronogramaHelperFlag.ViewMock );
            var presenter = hash[CRONOGRAMA_PRESENTER] as CronogramaPresenter;
            var viewMock = hash[VIEW_MOCK] as Mock<ICronogramaView>;
        }

        #endregion

        #region Métodos Auxilires BDD
        /// <summary>
        /// Adiciona um dto no serviço stub e armazena o cronograma dto criado no ao contexto do BDD
        /// </summary>
        /// <param name="cronogramaDto"></param>
        private void CriarEArmazenarCronogramaDto( CronogramaDto cronogramaDto )
        {
            cronogramaDto = planejamentoServiceStub.CriarCronograma( cronogramaDto.TxDescricao, cronogramaDto.DtInicio, cronogramaDto.DtFinal );
            AdicionarAoContextoAtualBDD( CriarChave( FORMATO_CHAVE_CRONOGRAMA, cronogramaDto.TxDescricao ), cronogramaDto );
        }


        /// <summary>
        /// Método para efetuar a criação da chave
        /// </summary>
        /// <param name="formato"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        private string CriarChave( string formato, params object[] parametros )
        {
            return string.Format( formato, parametros );
        }

        /// <summary>
        /// Método para salvar a instância  no contexto para compartilhar entre steps do BDD
        /// </summary>
        /// <typeparam name="TObject">Tipo de objeto</typeparam>
        /// <param name="chave">chave de identificação para recuperar posteriormente</param>
        /// <param name="objeto">instância do objeto a ser salva</param>
        private void AdicionarAoContextoAtualBDD<TObject>( string chave, TObject objeto )
        {
            ScenarioContext.Current.Add( chave, objeto );
        }

        /// <summary>
        /// Método para carregar do contexto do BDD um objeto salvo
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="chave"></param>
        /// <param name="objeto"></param>
        private void CarregarDoContextoAtualBDD<TObject>( string chave, out TObject objeto )
        {
            objeto = ScenarioContext.Current.Get<TObject>( chave );
        }

        /// <summary>
        /// Método para recuperar informando o tipo para recuperar um objeto salvo no contexto do BDD
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="chave"></param>
        /// <returns></returns>
        private TObject CarregarDoContextoAtualBDD<TObject>( string chave )
        {
            return ScenarioContext.Current.Get<TObject>( chave );
        }

        /// <summary>
        /// Método para recuperar objetos armazenados no contexto do BDD, a partir de flags
        /// </summary>
        /// <param name="flags">tipos a serem carregados</param>
        /// <returns>uma hastable com os objetos carregados através das flags</returns>
        private Hashtable CarregarObjetosContexto( StepEdicaoCronogramaHelperFlag flags )
        {
            Hashtable hash = new Hashtable();
            CronogramaDto cronograma = null;
            UsuarioCronogramaBindHelper usuario = null;
            if(flags.HasFlag( StepEdicaoCronogramaHelperFlag.Usuario ))
            {
                CarregarDoContextoAtualBDD( USUARIO_ATUAL, out usuario );
                hash.Add( USUARIO_ATUAL, usuario );
            }

            if(flags.HasFlag( StepEdicaoCronogramaHelperFlag.Cronograma ))
            {
                CarregarDoContextoAtualBDD( CRONOGRAMA_ATUAL, out cronograma );
                hash.Add( CRONOGRAMA_ATUAL, cronograma );
            }

            if(flags.HasFlag( StepEdicaoCronogramaHelperFlag.CronogramaPresenter ))
                hash.Add( CRONOGRAMA_PRESENTER, CarregarDoContextoAtualBDD<CronogramaPresenter>( CriarChave( FORMATO_CHAVE_PRESENTER, cronograma.TxDescricao, usuario.Nome ) ) );

            if(flags.HasFlag( StepEdicaoCronogramaHelperFlag.ViewMock ))
                hash.Add( VIEW_MOCK, CarregarDoContextoAtualBDD<Mock<ICronogramaView>>( CriarChave( FORMATO_CHAVE_VIEW_MOCK, cronograma.TxDescricao, usuario.Nome ) ) );

            if(flags.HasFlag( StepEdicaoCronogramaHelperFlag.AccessClientMock ))
                hash.Add( ACCESS_CLIENT_MOCK, CarregarDoContextoAtualBDD<Mock<IWexMultiAccessClient>>( CriarChave( FORMATO_CHAVE_ACCESS_CLIENT_MOCK, cronograma.TxDescricao, usuario.Nome ) ) );
            return hash;
        }

        #endregion
    }

    /// <summary>
    /// Enum flag para indicar o tipo de objetos salvos no contexto e auxiliar na recuperação de múltiplos objetos
    /// </summary>
    [Flags]
    internal enum StepEdicaoCronogramaHelperFlag
    {
        Cronograma = 1,
        Usuario = 2,
        UsuarioECronograma = StepEdicaoCronogramaHelperFlag.Usuario | StepEdicaoCronogramaHelperFlag.Cronograma,
        CronogramaPresenter = 4 | StepEdicaoCronogramaHelperFlag.UsuarioECronograma,
        ViewMock = 8 | StepEdicaoCronogramaHelperFlag.UsuarioECronograma,
        AccessClientMock = 16 | StepEdicaoCronogramaHelperFlag.UsuarioECronograma,
    }
}
