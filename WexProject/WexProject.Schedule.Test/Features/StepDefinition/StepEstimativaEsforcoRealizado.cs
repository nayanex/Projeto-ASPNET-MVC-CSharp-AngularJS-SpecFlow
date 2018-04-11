using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using WexProject.Schedule.Test.Stubs;
using WexProject.Schedule.Test.Helpers.Utils;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.Schedule.Test.Features.Helpers.EstimativaEsforcoRealizado;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Test.Builders;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.Presenters;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.BLL.Shared.Domains.Planejamento;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WexProject.Schedule.Test.UnitTest;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    [Binding]
    public class StepEstimativaEsforcoRealizado : BaseEntityFrameworkTest
    {
        #region Atributos
        /// <summary>
        /// Stub representando o PlanejamentoServiceUtil
        /// </summary>
        PlanejamentoServiceUtilStub planejamentoServiceStub;

        /// <summary>
        /// Stub representando o GeralServiceUtil
        /// </summary>
        GeralServiceUtilStub geralServiceStub;

        /// <summary>
        /// Armazenar o colaborador logado
        /// </summary>
        ColaboradorDto colaboradorLogado;

        /// <summary>
        /// A tarefa que está sendo editada no momento
        /// </summary>
        CronogramaTarefaDecorator tarefaAtual;

        /// <summary>
        /// Dia de trabalho atual contendo os periodos de trabalho do colaborador
        /// </summary>
        DiaTrabalhoDto diaTrabalhoAtual;

        /// <summary>
        /// Armazenar o inicializador da ultima data trabalhada pelo colaborador
        /// </summary>
        InicializadorEstimativaDto inicializadorDeEstimativa;

        /// <summary>
        /// Controlador da view
        /// </summary>
        TarefaHistoricoPresenter presenter;

        /// <summary>
        /// View de visualização
        /// </summary>
        TarefaHistoricoViewMock view;

        /// <summary>
        /// Situação selecionada atualmente para tarefa
        /// </summary>
        SituacaoPlanejamentoDTO situacaoSelecionada;

        /// <summary>
        /// Situação selecionada antes de solicitação de mudança
        /// </summary>
        SituacaoPlanejamentoDTO situacaoSelecionadaAnterior;

        /// <summary>
        /// Mock da view armazenado para auxiliar no ambiente de teste
        /// </summary>
        Mock<TarefaHistoricoViewMock> viewMock;

        #endregion

        #region Métodos Auxiliares

        /// <summary>
        /// Método pare realizar inicialização do presenter
        /// </summary>
        private void Inicializar()
        {
            viewMock = new Mock<TarefaHistoricoViewMock>() { CallBase = true };
            view = viewMock.Object;
            presenter = new TarefaHistoricoPresenter( view );
            presenter.CarregarSituacoesPlanejamentoPorTipo( planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas() );
            presenter.InicializarHistoricoTarefa( tarefaAtual, colaboradorLogado.Login );
        }

        /// <summary>
        /// Selecionar situação por oid
        /// </summary>
        /// <param name="oidSituacao">oid da situação</param>
        /// <returns>retornar a situação planejamento em que o oid seja igual ao oid esperada</returns>
        public SituacaoPlanejamentoDTO GetSituacaoPlanejamento(Guid oidSituacao) 
        {
            return planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.Oid.Equals( oidSituacao ) );
        }

        /// <summary>
        /// Selecionar situação por tipo
        /// </summary>
        /// <param name="tipo">tipo da situação planejamento</param>
        /// <returns>retornar a situação planejamento em que o tipo seja igual ao tipo esperado</returns>
        public SituacaoPlanejamentoDTO GetSituacaoPlanejamento( CsTipoPlanejamento tipo )
        {
            return planejamentoServiceStub.ConsultarSituacoesPlanejamentoTipadas().FirstOrDefault( o => o.CsTipo ==  tipo );
        }

        #endregion

        #region Pre condições (Given)
        [Given( @"que o servico foi inicializado" )]
        public void DadoQueOServicoFoiInicializado()
        {
            ConfigServiceStubUtil.InicializarServicosStubCronograma( out planejamentoServiceStub, out geralServiceStub );
        }

        [Given( @"que o colaborador '(.*)' esteja logado" )]
        public void DadoQueOColaboradorEstejaLogado( string login )
        {
            planejamentoServiceStub.CriarColaboradorDto( login, true );
            planejamentoServiceStub.SelecionarColaboradorLogado( login );
            colaboradorLogado = planejamentoServiceStub.colaboradorLogado;
        }

        [Given( @"que exista a tarefa :" )]
        public void DadoQueExistaATarefa( Table table )
        {
            TarefaEstimativaHelper tarefaHelper = table.CreateInstance<TarefaEstimativaHelper>();
            tarefaAtual = planejamentoServiceStub.CriarTarefa( tarefaHelper.Descricao );
            tarefaAtual.NbEstimativaInicial = tarefaHelper.EstimativaInicial;
            tarefaAtual.NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( tarefaHelper.Realizado );
            tarefaAtual.NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks(  tarefaHelper.Restante );
        }

        [Given( @"que o dia de trabalho para o colaborador '(.*)' no dia '(.*)' contenha os periodos:" )]
        public void DadoQueODiaDeTrabalhoParaOColaboradorNoDiaContenhaOsPeriodos( string colaborador, DateTime data, Table periodos )
        {
            diaTrabalhoAtual = new DiaTrabalhoDto() { DiaSemana = data.DayOfWeek, PeriodosTrabalho = periodos.CreateSet<PeriodoTrabalhoDto>().ToList() };
        }

        [Given( @"que o ultimo esforco estimado pelo colaborador seja no dia '(.*)' as '(.*)'" )]
        public void DadoQueOUltimoEsforcoEstimadoPeloColaboradorSejaNoDiaAs( DateTime dataAtual, string horaInicio )
        {
            inicializadorDeEstimativa = new InicializadorEstimativaDto() { DataEstimativa = dataAtual, DiaAtual = diaTrabalhoAtual, HoraInicialEstimativa = ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( horaInicio ) };
            planejamentoServiceStub.RetornoInicializadorEstimativa = inicializadorDeEstimativa;
        }
        [Given( @"que foi aberto a janela de estimativa de esforco realizado" )]
        public void DadoQueFoiAbertoAJanelaDeEstimativaDeEsforcoRealizado()
        {
            Inicializar();
        }


        #endregion

        #region Exercicio das funcionalidades (When)
        [When( @"for aberto o a janela de estimativa de esforco realizado" )]
        public void QuandoForAbertoOAJanelaDeEstimativaDeEsforcoRealizado()
        {
            Inicializar();
        }

        [When( @"o colaborador alterar a situacao da tarefa para o tipo '(.*)'" )]
        public void QuandoOColaboradorAlterarASituacaoDaTarefaParaOTipo( CsTipoPlanejamento tipoSituacaoPlanejamento )
        {
            situacaoSelecionadaAnterior = GetSituacaoPlanejamento( view.OidSituacaoPlanejamento );
            situacaoSelecionada = GetSituacaoPlanejamento( tipoSituacaoPlanejamento );
            view.OidSituacaoPlanejamento = situacaoSelecionada.Oid;
            presenter.AlterarSituacaoPlanejamento();
        }

        [Given( @"que o colaborador alterou a hora restante para '(.*)'" )]
        [When( @"o colaborador alterar a hora restante para '(.*)'" )]
        public void QuandoOColaboradorAlterarAHoraRestantePara( string horaRestante )
        {
            view.NbHoraRestante = horaRestante;
            presenter.HoraRestanteForAlterada();
        }

        [When( @"o colaborador estimar a o esforco realizado como '(.*)'" )]
        public void QuandoOColaboradorEstimarAOEsforcoRealizadoComo( string horaRealizado )
        {
            view.NbHoraRealizado = horaRealizado;
            presenter.HoraRealizadoForAlterado();
        }

        [When( @"o colaborador alterar a situacao planejamento da tarefa diretamente para o tipo '(.*)'" )]
        public void QuandoOColaboradorAlterarASituacaoPlanejamentoDaTarefaDiretamenteParaOTipo( CsTipoPlanejamento tipoSituacaoPlanejamento )
        {
            Guid oidSituacaoPlanejamentoAnterior = view.OidSituacaoPlanejamento;
            situacaoSelecionada = GetSituacaoPlanejamento( tipoSituacaoPlanejamento );
            presenter.AlterarSituacaoPlanejamento();
        }

        #endregion

        #region Expectativas (Then)
        [Then( @"a situacao planejamento devera ser do tipo '(.*)' pois '(.*)'" )]
        public void EntaoASituacaoPlanejamentoDeveraSerDoTipoPois( CsTipoPlanejamento tipoSituacaoPlanejamento, string motivo )
        {
            SituacaoPlanejamentoDTO situacao = GetSituacaoPlanejamento( view.OidSituacaoPlanejamento );
            Assert.AreEqual( tipoSituacaoPlanejamento, situacao.CsTipo, motivo );
        }

        [Then( @"a situacao planejamento devera ser do tipo '(.*)'" )]
        public void EntaoASituacaoPlanejamentoDeveraSerDoTipo( CsTipoPlanejamento tipoSituacaoPlanejamento )
        {
            SituacaoPlanejamentoDTO situacao = GetSituacaoPlanejamento( view.OidSituacaoPlanejamento );
            Assert.AreEqual( tipoSituacaoPlanejamento, situacao.CsTipo, string.Format( "A situação planejamento deveria ser do tipo {2} (Situação Selecionada:{0} | Tipo:{1})", situacao.TxDescricao, situacao.CsTipo, tipoSituacaoPlanejamento ) );
        }

        [Then( @"deveria ter habilitado o campo de justificativa de reducao" )]
        public void EntaoDeveriaTerHabilitadoOCampoDeJustificativaDeReducao()
        {
            Assert.IsTrue( presenter.JustificativaReducaoAtiva,"Deveria ter habilitado a justificava de redução" );
        }

        [Then( @"nao deveria ter habilitado o campo de justificativa de reducao" )]
        public void EntaoNaoDeveriaTerHabilitadoOCampoDeJustificativaDeReducao()
        {
            Assert.IsFalse( presenter.JustificativaReducaoAtiva, "Deveria ter habilitado a justificava de redução" );
        }

        [Then( @"os horarios esperados deverao ser hora realizado '(.*)',hora restante '(.*)' e hora final '(.*)'" )]
        public void EntaoOsHorariosEsperadosDeveraoSerHoraRealizadoHoraRestanteEHoraFinal( string horaRealizado, string horaRestante, string horaFinal )
        {
            Assert.AreEqual( horaRealizado, view.NbHoraRealizado ,string.Format("Era esperado que a estimativa do realizado fosse igual a {0}",horaRealizado));
            Assert.AreEqual( horaRestante, view.NbHoraRestante, string.Format( "Era esperado que a estimativa de horas restantes fosse igual a {0}", horaRestante ) );
            Assert.AreEqual( horaFinal, view.NbHoraFinal, string.Format( "Era esperado que a hora final calculada fosse igual a {0}", horaFinal ) );
        }

        [Then( @"o colaborador nao devera poder salvar as alterações na tarefa" )]
        public void EntaoOColaboradorNaoDeveraPoderSalvarAsAlteracoesNaTarefa()
        {
            viewMock.Verify();
        }
        #endregion
    }
}
