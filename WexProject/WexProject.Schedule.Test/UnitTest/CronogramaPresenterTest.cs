using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Views;
using WexProject.Schedule.Library.Presenters;
using Moq;
using Moq.Protected;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.ServiceUtils;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.MultiAccess.Library.Dtos;
using WexProject.MultiAccess.Library.Libs;
using WexProject.BLL.Shared.Domains.Planejamento;
using System.Drawing;
using WexProject.Library.Libs.Test;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.MultiAccess.Library.Components;
using WexProject.Schedule.Test.Helpers.Utils;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Library.Properties;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaPresenterTest
    {
        #region Atributos auxiliares (Testes)
        List<SituacaoPlanejamentoDTO> situacoesPlanejamentoAtivas;
        List<SituacaoPlanejamentoDTO> situacoesPlanejamentoInativas;
        SituacaoPlanejamentoDTO situacaoPlanejamentoPadrao;
        ColaboradorDto colaboradorLogado;
        CronogramaDto cronogramaSelecionado;
        List<CronogramaColaboradorConfigDto> colaboradoresConfig;
        List<CronogramaTarefaGridItem> tarefas;
        List<ColaboradorDto> colaboradoresResponsaveis;
        List<CronogramaDto> cronogramas;
        #endregion

        [TestInitialize]
        public void InicializarTeste()
        {
            situacoesPlanejamentoAtivas = new List<SituacaoPlanejamentoDTO>();
            situacoesPlanejamentoInativas = new List<SituacaoPlanejamentoDTO>();
            CriarSituacoesPlanejamento();
            SelecionarSituacaoPlanejamentoPadrao();
            CriarCronogramas();
            SelecionarCronograma( 1 );
            tarefas = new List<CronogramaTarefaGridItem>();
            CriarColaboradores();
            CriarCronogramaColaboradoresConfig();
        }

        #region Métodos auxiliares (Testes)
        /// <summary>
        /// Criar situações planejamento
        /// </summary>
        private void CriarSituacoesPlanejamento()
        {
            situacoesPlanejamentoAtivas = new List<SituacaoPlanejamentoDTO>();
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( 1 ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( 2 ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( 3 ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( 4 ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( 5 ) );
        }

        /// <summary>
        /// Criar os colaboradores
        /// </summary>
        private void CriarColaboradores()
        {
            string[] colaboradores = new string[] { "gabriel.matos", "anderson.lins", "anderlan.castro", "alexandre.amorim" };
            colaboradoresResponsaveis = new List<ColaboradorDto>();
            foreach(string colaborador in colaboradores)
            {
                colaboradoresResponsaveis.Add( CriarColaboradorDto( colaborador ) );
            }
            colaboradorLogado = colaboradoresResponsaveis.FirstOrDefault();
        }

        /// <summary>
        /// Criar as configuracoes dos colaboradores para o cronograma selecionado
        /// </summary>
        private void CriarCronogramaColaboradoresConfig()
        {

            colaboradoresConfig = new List<CronogramaColaboradorConfigDto>();

            foreach(ColaboradorDto colaborador in colaboradoresResponsaveis)
            {
                colaboradoresConfig.Add( CriarConfig( colaborador ) );
            }
        }

        /// <summary>
        /// Criar um cronograma colaborador config
        /// </summary>
        /// <param name="colaborador"></param>
        /// <returns></returns>
        private CronogramaColaboradorConfigDto CriarConfig( ColaboradorDto colaborador )
        {
            Random rd = new Random();
            CronogramaColaboradorConfigDto config = new CronogramaColaboradorConfigDto()
            {
                OidCronograma = cronogramaSelecionado.Oid,
                OidColaborador = colaborador.OidColaborador,
                Login = colaborador.Login,
                NomeCompletoColaborador = colaborador.TxNomeCompletoColaborador,
                Cor = Color.FromArgb( rd.Next( 255 ), rd.Next( 255 ), rd.Next( 255 ) ).ToArgb().ToString()
            };
            return config;
        }

        /// <summary>
        /// Selecionar um cronograma 
        /// </summary>
        /// <param name="num"></param>
        private void SelecionarCronograma( int num )
        {
            num--;
            if(num >= cronogramas.Count)
                num = cronogramas.Count - 1;
            if(num < 0)
                num = 0;
            cronogramaSelecionado = cronogramas[num];
        }

        /// <summary>
        /// Selecionar uma situação planejamento padrao
        /// </summary>
        private void SelecionarSituacaoPlanejamentoPadrao()
        {
            situacaoPlanejamentoPadrao = situacoesPlanejamentoAtivas.FirstOrDefault();
        }

        /// <summary>
        /// Criar cronogramas
        /// </summary>
        private void CriarCronogramas()
        {
            cronogramas = new List<CronogramaDto>();
            for(int i = 0; i < 5; i++)
            {
                cronogramas.Add( CriarCronograma( i + 1 ) );
            }
        }

        /// <summary>
        /// Criar uma nova situação planejamento
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private SituacaoPlanejamentoDTO CriarSituacaoPlanejamento( int num )
        {
            Random random = new Random();
            SituacaoPlanejamentoDTO situacao = new SituacaoPlanejamentoDTO()
            {
                Oid = Guid.NewGuid(),
                TxDescricao = string.Format( "Situacao Planejamento {0}", num ),
                CsPadrao = (CsPadraoSistema)random.Next( 0, 1 ),
                CsSituacao = (CsTipoSituacaoPlanejamento)random.Next( 0, 1 ),
                CsTipo = (CsTipoPlanejamento)random.Next( 0, 4 ),
                BlImagemSituacaoPlanejamento = null
            };
            return situacao;
        }

        /// <summary>
        /// Criar um novo cronograma
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private CronogramaDto CriarCronograma( int num )
        {
            CronogramaDto cronograma = new CronogramaDto();
            cronograma.DtInicio = DateTime.Now;
            cronograma.DtFinal = DateTime.Now.AddDays( 15 );
            cronograma.Oid = Guid.NewGuid();
            cronograma.OidSituacaoPlanejamento = situacaoPlanejamentoPadrao.Oid;
            cronograma.TxDescricao = string.Format( "WexCronograma {0}", num );
            cronograma.TxDescricaoSituacaoPlanejamento = situacaoPlanejamentoPadrao.TxDescricao;
            return cronograma;
        }

        /// <summary>
        /// Criar um novo colaborador
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private ColaboradorDto CriarColaboradorDto( string login )
        {
            ColaboradorDto colaborador = new ColaboradorDto()
            {
                Login = login,

            };
            string[] nomes = login.Split( '.' );
            foreach(string nome in nomes)
            {
                colaborador.TxNomeCompletoColaborador += nome + " ";
            }
            colaborador.OidUsuario = Guid.NewGuid();
            colaborador.OidColaborador = Guid.NewGuid();
            colaborador.TxMatriculaColaborador = new Random().Next( 100, 999 ).ToString();
            colaborador.TxNomeCompletoColaborador.Trim();
            return colaborador;
        }

        /// <summary>
        /// Método de auxiliar de testes gerando um presenter com os métodos de serviço mockados
        /// </summary>
        /// <param name="view">view para instancia do prensenter</param>
        /// <param name="client">cliente de acesso do presenter</param>
        /// <returns>Mock de cronogramaPresenter</returns>
        public Mock<CronogramaPresenter> CriarCronogramaPresenterComServicosMockados( ICronogramaView view, IWexMultiAccessClient client )
        {
            Mock<CronogramaPresenter> presenterMock = new Mock<CronogramaPresenter>( view, client ) { CallBase = true };
            //presenterMock.Protected().Setup<GeralServiceUtil>( "CarregarGeralService" ).Returns<GeralServiceUtil>( null );
            //presenterMock.Protected().Setup<List<SituacaoPlanejamentoDto>>( "ListarSituacoesPlanejamento" ).Returns( situacoesPlanejamentoAtivas );
            //presenterMock.Protected().Setup<List<SituacaoPlanejamentoDto>>( "ListarSituacoesPlanejamentoInativas" ).Returns( situacoesPlanejamentoInativas );
            //presenterMock.Protected().Setup<SituacaoPlanejamentoDto>( "GetSituacaoPlanejamentoPadrao" ).Returns( situacaoPlanejamentoPadrao );
            //presenterMock.Protected().Setup<List<ColaboradorDto>>( "GetColaboradoresResponsaveis" ).Returns( colaboradoresResponsaveis );
            //presenterMock.Protected().Setup<CronogramaDto>( "GetCronogramaUltimoCronogramaSelecionado", colaboradorLogado.Login ).Returns( cronogramaSelecionado );
            //presenterMock.Protected().Setup<ColaboradorDto>( "GetColaborador", colaboradorLogado.Login ).Returns( colaboradorLogado );
            //presenterMock.Protected().Setup<List<UsuarioConectadoDto>>( "ListarConfigColaboradores", ItExpr.IsAny<List<string>>() ).Returns( colaboradoresConfig );
            ////presenterMock.Protected().Setup( "ConfigurarEventosServiceClient" );
            //presenterMock.Setup( o => o.ConectarCronograma() );
            return presenterMock;
        }

        #endregion

        [TestMethod]
        public void AdicionarTarefaEdicaoQuandoTarefaNaoEstaEmEdicaoTest()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada );
            CronogramaViewMock view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;
            CronogramaPresenter presenter = new CronogramaPresenter( view, accessClient );
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Verify( o => o.ConsultarTarefaSelecionada(), Times.Once(), "Deveria ter chamado a tarefa selecionada ao menos uma vez" );
            Assert.IsTrue( presenter.ExisteTarefaEmEdicao(), "A tarefa deveria ter entrado em edição" );
            Assert.AreEqual( tarefaSelecionada.OidCronogramaTarefa, presenter.TarefaEmEdicao.Oid );
        }

        [TestMethod]
        public void NaoDeveAdicionarOutraTarefaEmEdicaoQuandoJaHouverUmaTarefaEmEdicao()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, tarefaSelecionada2;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada2 = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, accessClient );
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada2 );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Verify( o => o.NotificarAlerta( Resources.Caption_Atencao, Resources.Alerta_JaPossuiTarefaEmEdicao ), Times.Once(), "Deveria chamar a a notificação de que já possui tarefa em edição" );
            Assert.IsTrue( presenter.ExisteTarefaEmEdicao(), "Deveria possuir tarefa em edição" );
            Assert.AreEqual( tarefaSelecionada1.OidCronogramaTarefa, presenter.TarefaEmEdicao.Oid, "A tarefa em seleção deveria continuar sendo a 1ª tarefa selecionada" );
        }

        [TestMethod]
        public void DevePermitirQueOUsuarioRetomeAEdicaoDaTarefaQueSolicitouParaEdicao()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, tarefaSelecionada2;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada2 = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, accessClient );
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada2 );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Verify( o => o.NotificarAlerta( Resources.Caption_Atencao, Resources.Alerta_JaPossuiTarefaEmEdicao ), Times.Once(), "Deveria chamar a a notificação de que já possui tarefa em edição" );
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            presenter.SolicitarInicioEdicaoTarefa();
            viewMock.Verify( o => o.NotificarAlerta( Resources.Caption_Atencao, Resources.Alerta_JaPossuiTarefaEmEdicao ), Times.Once(), "Não deveria ter chamado novamente a notificação de que já possui tarefa em edição" +
                ", pois a tarefa selecionada é a tarefa em edição" );

            Assert.IsNotNull( presenter.TarefaEmEdicao, "Deveria conter uma tarefa em edição" );
            Assert.AreEqual( tarefaSelecionada1.OidCronogramaTarefa, presenter.TarefaEmEdicao.Oid, "A tarefa em edição deveria ser a tarefa selecionada" );
        }

        [TestMethod]
        public void DevePermitirSalvarQuandoReceberAutorizacaoDeEdicaoAntesDeTerminarAEdicao()
        {
            #region Pre condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, tarefaSelecionada2;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada2 = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;
            Mock<CronogramaPresenter> presenterMock = new Mock<CronogramaPresenter>( view, accessClient ) { CallBase = true };
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            tarefaSelecionada1.TxDescricaoTarefa = "abc";
            MensagemDto mensagemPermissao = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada( oidCronogramaSelecionado, tarefaSelecionada1.OidCronogramaTarefa.ToString(), "" );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemPermissao );
            #endregion

            //Exercicio do teste
            presenter.TarefaSaiuDeEdicao();

            //Expectativas após sair de edição
            presenterMock.Verify( o => o.SalvarEdicaoTarefa( tarefaSelecionada1 ), Times.Once(), "Deveria entrado no método salvar com a seguinte tarefa" );
        }

        [TestMethod]
        public void DevePermitirSalvarQuandoReceberAutorizacaoDeEdicaoAposTerTerminadoAEdicao()
        {
            #region Pre condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, tarefaSelecionada2;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada2 = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;
            Mock<CronogramaPresenter> presenterMock = new Mock<CronogramaPresenter>( view, accessClient ) { CallBase = true };
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            tarefaSelecionada1.TxDescricaoTarefa = "abc";
            //Exercicio do teste
            presenter.TarefaSaiuDeEdicao();
            #endregion

            //Exercicio do teste
            MensagemDto mensagemPermissao = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada( "", oidCronogramaSelecionado, tarefaSelecionada1.OidCronogramaTarefa.ToString() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemPermissao );

            //Expectativas
            presenterMock.Verify( o => o.SalvarEdicaoTarefa( tarefaSelecionada1 ), Times.Once(), "Deveria entrado no método salvar com a seguinte tarefa" );
        }

        [TestMethod]
        public void NaoDevePermitirSalvarERetornarOsValoresAntigosDaTarefaQuandoReceberARecusaDeEdicaoAntesDeSairDeEdicao()
        {
            #region Pre condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, copiaTarefaSelecionada;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            copiaTarefaSelecionada = TarefaEditada.CopiarValores( tarefaSelecionada1, new CronogramaTarefaGridItem() );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );
            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;
            Mock<CronogramaPresenter> presenterMock = new Mock<CronogramaPresenter>( view, accessClient ) { CallBase = true };
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            tarefaSelecionada1.TxDescricaoTarefa = "abc";
            MensagemDto mensagemDeRecusa = Mensagem.RnCriarMensagemRecusarEdicaoTarefa( tarefaSelecionada1.OidCronogramaTarefa.ToString(), colaboradorLogado.Login, oidCronogramaSelecionado );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemDeRecusa );
            #endregion

            //Exercicio do teste
            presenter.TarefaSaiuDeEdicao();

            //Expectativas
            presenterMock.Verify( o => o.SalvarEdicaoTarefa( tarefaSelecionada1 ), Times.Never(), "Deveria entrado no método salvar com a seguinte tarefa" );
            viewMock.Verify( o => o.ForcarFimEdicao(), "Deveria ter forçado o fim de edição" );
            viewMock.Verify( o => o.NotificarMensagem( Resources.Caption_Atencao, It.IsAny<string>() ), Times.Once(), "Deveria ter notificado o colaborador que a tarefa teve a edição recusada" );
            Assert.IsFalse( TarefaEditada.HouveMudancas( tarefaSelecionada1, copiaTarefaSelecionada ), "Não deveria haver diferenças na tarefa selecionada pois a edição foi recusada" );
        }

        [TestMethod]
        public void NaoDevePermitirSalvarERetornarOsValoresAntigosDaTarefaEnquantoNaoReceberRespostaDaSolicitacaoDeEdicaoDaTarefa()
        {
            #region Pre condições de cenário
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, copiaTarefaSelecionada;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            copiaTarefaSelecionada = TarefaEditada.CopiarValores( tarefaSelecionada1, new CronogramaTarefaGridItem() );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );
            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;
            Mock<CronogramaPresenter> presenterMock = new Mock<CronogramaPresenter>( view, accessClient ) { CallBase = true };
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            presenter.SolicitarInicioEdicaoTarefa();
            tarefaSelecionada1.TxDescricaoTarefa = "abc";
            #endregion

            //Exercicio do teste
            presenter.TarefaSaiuDeEdicao();

            //Expectativas após sair da edição
            presenterMock.Verify( o => o.SalvarEdicaoTarefa( tarefaSelecionada1 ), Times.Never(), "Deveria entrar no método salvar com a seguinte tarefa pois não recebeu autorização de edição" );
            clientMock.Verify( o => o.RnComunicarFimEdicaoTarefa( It.IsAny<string>() ), Times.Never(), "Não deveria comunicar o fim da edição da tarefa" );
        }

        [TestMethod]
        public void DevePermitirSalvarATarefaQueRecebeuAPermissaoDeEdicaoQuandoEstejaEditandoUmaOutraTarefa()
        {
            #region Pré condições
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada1, tarefaSelecionada2;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            tarefaSelecionada1 = planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada2 = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada1 );
            ICronogramaView view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, accessClient );
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            #endregion

            //inicio da edição da primeira tarefa
            presenter.SolicitarInicioEdicaoTarefa();
            tarefaSelecionada1.TxDescricaoTarefa = "abc";
            //simulando o fim da edição da primeira tarefa porém ainda não obteve a permissão do servidor para salvar a edição
            presenter.TarefaSaiuDeEdicao();

            //Simulação da selecão de uma nova tarefa e solicitação de permissão para edição da nova tarefa
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada2 );
            presenter.SolicitarInicioEdicaoTarefa();

            //Criação da mensagem de permissão de edição da primeira tarefa
            MensagemDto mensagemPermissao = Mensagem.RnCriarMensagemEdicaoTarefaAutorizada( "", oidCronogramaSelecionado, tarefaSelecionada1.OidCronogramaTarefa.ToString() );

            //Está editando uma segunda tarefa quando recebe a autorização de permissão de edição da primeira
            accessClient.ProcessarMensagemEventoParaTeste( mensagemPermissao );
            //Expectativa é de que deverá salvar a primeira tarefa que estava em edição e deverá comunicar o fim da edição da primeira tarefa
            clientMock.Verify( o => o.RnComunicarFimEdicaoTarefa( tarefaSelecionada1.OidCronogramaTarefa.ToString() ) );
        }

        [TestMethod]
        public void NaoDeveSolicitarAtualizacaoDasTarefasQuandoReceberMensagemUsuarioDesconectadoComASolicitacaoDeDesconexao()
        {
            CronogramaViewMock view = new CronogramaViewMock();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.Login = colaboradorLogado.Login;
            string loginDesconectado = colaboradoresResponsaveis[1].Login;
            Mock<CronogramaPresenter> presenterMock = CriarCronogramaPresenterComServicosMockados( view, cliente );
            CronogramaPresenterMock presenter = new CronogramaPresenterMock( view, cliente );
            presenter.InicializarVariaveis();
            MensagemDto msgConexaoEfetuada = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), new Dictionary<string, string>() );
            cliente.ProcessarMensagemEventoParaTeste( msgConexaoEfetuada );
            MensagemDto mensagemUsuarioDesconectado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString() );
            cliente.ProcessarMensagemEventoParaTeste( mensagemUsuarioDesconectado );
            clienteMock.Verify( o => o.AcionarEventoAoUsuarioDesconectar( mensagemUsuarioDesconectado ) );
            presenterMock.Verify( o => o.ForcarAtualizacaoTarefas( It.IsAny<Guid>() ), Times.Never(), "Não deveria forçar atualização das tarefas, pois o usuário solicitou a desconexão " );
        }

        [TestMethod]
        public void DeveSolicitarAtualizacaoDasTarefasQuandoReceberMensagemUsuarioDesconectadoInesperadamente()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaViewMock view = new CronogramaViewMock();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.Login = colaboradorLogado.Login;
            string loginDesconectado = colaboradoresResponsaveis[1].Login;
            Mock<CronogramaPresenter> presenterMock = CriarCronogramaPresenterComServicosMockados( view, cliente );
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            MensagemDto msgConexaoEfetuada = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), new Dictionary<string, string>() );
            cliente.ProcessarMensagemEventoParaTeste( msgConexaoEfetuada );
            MensagemDto mensagemUsuarioDesconectado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), true );
            cliente.ProcessarMensagemEventoParaTeste( mensagemUsuarioDesconectado );
            clienteMock.Verify( o => o.AcionarEventoAoUsuarioDesconectar( mensagemUsuarioDesconectado ) );
            presenterMock.Verify( o => o.ForcarAtualizacaoTarefas( It.IsAny<Guid>() ), Times.Once(), "deveria forçar atualização das tarefas, pois o usuário foi desconectado inesperadamente " );
        }

        [TestMethod]
        public void DeveAtualizarTodasTarefasQuandoReceberMensagemUsuarioDesconectadoInesperadamente()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaViewMock view = new CronogramaViewMock();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.Login = colaboradorLogado.Login;
            string loginDesconectado = colaboradoresResponsaveis[1].Login;
            Mock<CronogramaPresenter> presenterMock = CriarCronogramaPresenterComServicosMockados( view, cliente );
            CronogramaPresenter presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            MensagemDto msgConexaoEfetuada = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), new Dictionary<string, string>() );
            cliente.ProcessarMensagemEventoParaTeste( msgConexaoEfetuada );
            MensagemDto novoUsuarioConectado = Mensagem.RnCriarMensagemNovoUsuarioConectado( new[] { "anderson.lins" }, cliente.OidCronograma );
            cliente.ProcessarMensagemEventoParaTeste( novoUsuarioConectado );

            MensagemDto mensagemUsuarioDesconectado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), true );
            cliente.ProcessarMensagemEventoParaTeste( mensagemUsuarioDesconectado );
            clienteMock.Verify( o => o.AcionarEventoAoUsuarioDesconectar( mensagemUsuarioDesconectado ) );
            presenterMock.Verify( o => o.ForcarAtualizacaoTarefas( It.IsAny<Guid>() ), Times.Once(), "deveria forçar atualização das tarefas, pois o usuário foi desconectado inesperadamente " );
        }

        [TestMethod]
        public void DeveAguardarFimEdicaoTarefaParaPoderForcarAtualizacaoTarefasQuandoReceberMensagemUsuarioDesconectadoInesperadamente()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaViewMock view = new CronogramaViewMock();
            Mock<WexMultiAccessClientMock> clienteMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock cliente = clienteMock.Object;
            cliente.Login = colaboradorLogado.Login;
            string loginDesconectado = colaboradoresResponsaveis[1].Login;
            Mock<CronogramaPresenterMock> presenterMock = new Mock<CronogramaPresenterMock>( view, cliente ) { CallBase = true };
            CronogramaPresenterMock presenter = presenterMock.Object;
            presenter.InicializarVariaveis();
            MensagemDto msgConexaoEfetuada = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), new Dictionary<string, string>() );
            cliente.ProcessarMensagemEventoParaTeste( msgConexaoEfetuada );
            MensagemDto mensagemUsuarioDesconectado = Mensagem.RnCriarMensagemUsuarioDesconectado( new string[] { loginDesconectado }, cronogramaSelecionado.Oid.ToString(), false );
            presenter.PodeExecutar = false;
            cliente.ProcessarMensagemEventoParaTeste( mensagemUsuarioDesconectado );
            clienteMock.Verify( o => o.AcionarEventoAoUsuarioDesconectar( mensagemUsuarioDesconectado ) );
        }

        [TestMethod]
        public void DeveRetornarFalsoQuandoATarefaNaoEstiverNaHashDeEdicoes()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
            CronogramaPresenterMock.ServicoPlanejamento = planejamentoServiceStub;
            CronogramaPresenterMock.ServicoGeral = geralServiceStub;

            CronogramaTarefaGridItem tarefaSelecionada;
            //Criando tarefas para simular tarefas salvas
            planejamentoServiceStub.CriarTarefa( "T1" );
            planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            tarefaSelecionada = planejamentoServiceStub.CriarTarefa( "T3" );
            planejamentoServiceStub.CriarTarefa( "T4" );

            Mock<CronogramaViewMock> viewMock = new Mock<CronogramaViewMock>() { CallBase = true };
            viewMock.Setup( o => o.ConsultarTarefaSelecionada() ).Returns( tarefaSelecionada );
            CronogramaViewMock view = viewMock.Object;
            Mock<WexMultiAccessClientMock> clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock accessClient = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, accessClient );
            presenter.InicializarVariaveis();
            string oidCronogramaSelecionado = presenter.CronogramaSelecionado.Oid.ToString();
            MensagemDto mensagemAutenticadoComSucesso = Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, oidCronogramaSelecionado, new Dictionary<string, string>() );
            accessClient.ProcessarMensagemEventoParaTeste( mensagemAutenticadoComSucesso );
            Assert.IsFalse( presenter.EdicaoComPermissaoPendente( tarefaSelecionada.OidCronogramaTarefa ), "Não deveria estar pendente pois não existem tarefas em edição" );
        }

        [TestMethod]
        public void NaoDeveConsultarOsDadosDoGraficoQuandoOGraficoNaoEstiverVisivel()
        {

            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );

            var viewMock = new Mock<ICronogramaView>();
            viewMock.SetupAllProperties();
            ICronogramaView view = viewMock.Object;
            view.BurndownVisivel = false;

            var clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock client = clientMock.Object;


            CronogramaPresenter presenter = new CronogramaPresenter( view, client );

            client.ProcessarMensagemEventoParaTeste(Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso(new string[]{},Guid.NewGuid().ToString(), new Dictionary<string,string>()));
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Never ,"Não deveria ter invocado a atualização do gráfico de burndown");
        }

        [TestMethod]
        public void DeveConsultarOsDadosDoGraficoQuandoOGraficoEstiverVisivel()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );

            var viewMock = new Mock<ICronogramaView>();
            viewMock.SetupAllProperties();
            ICronogramaView view = viewMock.Object;
            view.BurndownVisivel = true;

            var clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock client = clientMock.Object;


            CronogramaPresenter presenter = new CronogramaPresenter( view, client );

            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, Guid.NewGuid().ToString(), new Dictionary<string, string>() ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Once, "Deveria ter invocado a atualização do gráfico de burndown ao conectar-se e o gráfico estiver visível" );
        }

        [TestMethod]
        public void DeveConsultarOsDadosDoGraficoQuandoOGraficoEstiverVisivelAoSerInformadoDaCriacaoDeUmaNovaTarefa()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );

            planejamentoServiceStub.CriarTarefa( "T1" );
            planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            var tarefaCriada = planejamentoServiceStub.CriarTarefa( "T3" );

            var viewMock = new Mock<ICronogramaView>();
            viewMock.SetupAllProperties();
            ICronogramaView view = viewMock.Object;
            view.BurndownVisivel = true;

            var clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock client = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, client );

            Func<string> novoOidString = () => Guid.NewGuid().ToString();
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, Guid.NewGuid().ToString(), new Dictionary<string, string>() ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Once, "Deveria ter invocado a atualização do gráfico de burndown ao conectar-se e o gráfico estiver visível" );
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemNovaTarefaCriada( tarefaCriada.OidCronogramaTarefa.ToString(), "joaquim.barbosa", novoOidString(), null, DateTime.Now ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Exactly( 2 ), "Deveria ter invocado o método para atualizar os dados do gráfico pela segunda vez." );
        }

        [TestMethod]
        public void DeveConsultarOsDadosDoGraficoQuandoOGraficoEstiverVisivelAoSerInformadoSobreAEdicaoDeUmaTarefa()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );

            planejamentoServiceStub.CriarTarefa( "T1" );
            planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            var tarefaEditada = planejamentoServiceStub.CriarTarefa( "T3" );

            var viewMock = new Mock<ICronogramaView>();
            viewMock.SetupAllProperties();
            ICronogramaView view = viewMock.Object;
            view.BurndownVisivel = true;

            var clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock client = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, client );

            Func<string> novoOidString = () => Guid.NewGuid().ToString();
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, Guid.NewGuid().ToString(), new Dictionary<string, string>() ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Once, "Deveria ter invocado a atualização do gráfico de burndown ao conectar-se e o gráfico estiver visível" );
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemFinalizarEdicaoTarefa( tarefaEditada.OidCronogramaTarefa.ToString(), "joaquim.barbosa", tarefaEditada.OidCronograma.ToString() ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Exactly( 2 ), "Deveria ter invocado o método para atualizar os dados do gráfico pela segunda vez." );
        }

        [TestMethod]
        public void DeveConsultarOsDadosDoGraficoQuandoOGraficoEstiverVisivelAoSerInformadoSobreAExclusaoDeUmaTarefa()
        {
            PlanejamentoServiceUtilStub planejamentoServiceStub;
            GeralServiceUtilStub geralServiceStub;
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );

            planejamentoServiceStub.CriarTarefa( "T1" );
            planejamentoServiceStub.CriarTarefa( "T2" );
            //armazenando uma tarefa para simular a seleção da tarefa na view
            var tarefaEditada = planejamentoServiceStub.CriarTarefa( "T3" );

            var viewMock = new Mock<ICronogramaView>();
            viewMock.SetupAllProperties();
            ICronogramaView view = viewMock.Object;
            view.BurndownVisivel = true;

            var clientMock = new Mock<WexMultiAccessClientMock>() { CallBase = true };
            WexMultiAccessClientMock client = clientMock.Object;

            CronogramaPresenter presenter = new CronogramaPresenter( view, client );

            Func<string> novoOidString = () => Guid.NewGuid().ToString();
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemConexaoEfetuadaComSucesso( new string[] { }, Guid.NewGuid().ToString(), new Dictionary<string, string>() ) );
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Once, "Deveria ter invocado a atualização do gráfico de burndown ao conectar-se e o gráfico estiver visível" );
            client.ProcessarMensagemEventoParaTeste( Mensagem.RnCriarMensagemComunicarExclusaoTarefaConcluida(new string[]{ tarefaEditada.OidCronogramaTarefa.ToString()},new Dictionary<string,short>(),tarefaEditada.OidCronograma.ToString(),"joaquim.barbosa",DateTime.Now ));
            viewMock.Verify( o => o.AtualizarGraficoBurndown( It.IsAny<BurndownGraficoDto>() ), Times.Exactly( 2 ), "Deveria ter invocado o método para atualizar os dados do gráfico pela segunda vez." );
        }
    }
}
