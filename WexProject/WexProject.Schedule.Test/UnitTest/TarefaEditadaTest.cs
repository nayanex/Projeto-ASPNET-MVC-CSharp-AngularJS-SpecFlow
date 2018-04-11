using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Helpers;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.Domains;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class TarefaEditadaTest
    {
        short QuantidadeTarefas;

        public CronogramaTarefaGridItem CriarTarefa( Guid oidCronograma, Guid oidSituacaoPlanejamento )
        {
            QuantidadeTarefas++;
            CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
            {
                OidCronograma = oidCronograma,
                OidCronogramaTarefa = Guid.NewGuid(),
                OidSituacaoPlanejamentoTarefa = oidSituacaoPlanejamento,
                DtInicio = DateTime.Now,
                NbEstimativaInicial = 0,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
                NbID = QuantidadeTarefas,
                OidTarefa = new Guid(),
                TxDescricaoTarefa = string.Format( "Tarefa {0}", QuantidadeTarefas ),
                TxObservacaoTarefa = string.Format( "Observação da Tarefa {0}", QuantidadeTarefas )
            };
            return tarefa;
        }

        [TestMethod]
        public void HouveMudancasDescricaoModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "A tarefa não deveria ter sido modificada, pois não houve alteração" );
            tarefaAtual.TxDescricaoTarefa = "";
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "As mudanças nulas não devem ser registradas" );
            tarefaAtual.TxDescricaoTarefa = "Nova tarefa 01";
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificada a descrição da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasObservacaoModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            tarefaAtual.TxObservacaoTarefa = "";
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "As mudanças nulas não devem ser registradas" );
            tarefaAtual.TxObservacaoTarefa = "Nova Observacao Tarefa";
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificada a observação da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasEstimativaInicialModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            tarefaAtual.NbEstimativaInicial = 0;
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "As mudanças nulas não devem ser registradas" );
            tarefaAtual.NbEstimativaInicial = 12;
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificada a estimativa inicial da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasEstimativaRestanteModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            tarefaAtual.NbEstimativaRestante = 0;
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "As mudanças nulas não devem ser registradas" );
            tarefaAtual.NbEstimativaRestante = new TimeSpan(12,0,0).Ticks;
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificada a estimativa restante da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasEsforcoRealizadoModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            tarefaAtual.NbRealizado = 0;
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "As mudanças nulas não devem ser registradas" );
            tarefaAtual.NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "12:00" );
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificado esforço realizado da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasSituacaoPlanejamentoModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            tarefaAtual.OidSituacaoPlanejamentoTarefa = Guid.NewGuid();
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificado a situação planejamento da tarefa" );
        }

        [TestMethod]
        public void HouveMudancasDescricaoColaboradorModificadaTest()
        {
            CronogramaTarefaGridItem tarefaAtual;
            Guid oidCronograma = Guid.NewGuid();
            Guid oidSituacaoPlanejamento = Guid.NewGuid();
            tarefaAtual = CriarTarefa( oidCronograma, oidSituacaoPlanejamento );
            TarefaEditada tarefaEmEdicao = new TarefaEditada( tarefaAtual );
            Assert.IsFalse( tarefaEmEdicao.HouveMudancas( tarefaAtual ) );
            tarefaAtual.TxDescricaoColaborador = "Gabriel Matos";
            Assert.IsTrue( tarefaEmEdicao.HouveMudancas( tarefaAtual ), "Deveria alertar que houve mudança, pois foi modificada a descrição dos colaboradores da tarefa" );
        }

        [TestMethod]
        public void DevePermitirEdicaoSituacaoPlanejamentoParaAndamentoApenasQuandoPossuirHorasParaConsumir()
        {
            CronogramaTarefaDto novaTarefa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 0,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
            };

            CronogramaTarefaDto novaTarefaComEstimativa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks("8:00"),
                NbRealizado = 0,
            };

            CronogramaTarefaDto tarefaConsumidaParcialmente = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
            };

            CronogramaTarefaDto tarefaPronta = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = 0,
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "8:00" ),
            };

            CronogramaTarefaDto tarefaCancelada = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
            };

            string mensagem;
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefa, CsTipoPlanejamento.Execução, out mensagem ), "Não deveria permitir setar a situação como execução pois não foi estimada uma quantidade de horas a ser realizada a tarefa." );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefaComEstimativa, CsTipoPlanejamento.Execução, out mensagem ), "Deveria permitir setar a situação como execução pois foi estimada uma quantidade de horas a ser realizada a tarefa." );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaConsumidaParcialmente, CsTipoPlanejamento.Execução, out mensagem ), "Deveria permitir setar a situação como execução pois ainda há horas restantes para executar a tarefa." );
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaPronta, CsTipoPlanejamento.Execução, out mensagem ), "Não deveria permitir setar a situação como execução pois foram consumidas todas as horas de execução da tarefa." );
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaCancelada, CsTipoPlanejamento.Execução, out mensagem ), "Não deveria permitir setar a situação como execução pois após o cancelamento não há horas restantes para trabalhar na tarefa." );
        }

        [TestMethod]
        public void DevePermitirEdicaoSituacaoPlanejamentoParaNaoIniciadoApenasQuandoNaoFoiRealizadoNenhumEsforco()
        {
            CronogramaTarefaDto novaTarefa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 0,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
            };

            CronogramaTarefaDto novaTarefaComEstimativa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "8:00" ),
                NbRealizado = 0,
            };

            CronogramaTarefaDto tarefaConsumidaParcialmente = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
            };

            CronogramaTarefaDto tarefaPronta = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = 0,
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "8:00" ),
            };

            CronogramaTarefaDto tarefaCancelada = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
            };

            CronogramaTarefaDto tarefaCanceladaParcialmenteRealizada= new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "6:00" ),
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "2:00" ),
            };

            string mensagem;
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefa, CsTipoPlanejamento.Planejamento, out mensagem ), "Deve poder estar na situação de não iniciado pois não foi realizado nenhum esforço" );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefaComEstimativa, CsTipoPlanejamento.Planejamento, out mensagem ), "Deve poder estar na situação de não iniciado pois não foi realizado nenhum esforço." );
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaConsumidaParcialmente, CsTipoPlanejamento.Planejamento, out mensagem ), "Não deve poder estar na situação de não iniciado pois foi estimado um esforço realizado." );
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaPronta, CsTipoPlanejamento.Planejamento, out mensagem ), "Não deve poder estar na situação de não iniciado pois a tarefa já foi executada." );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaCancelada, CsTipoPlanejamento.Planejamento, out mensagem ), "Deve poder estar na situação de não iniciado pois não foi realizado nenhum esforço." );
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaCanceladaParcialmenteRealizada, CsTipoPlanejamento.Planejamento, out mensagem ), "Não deve poder estar na situação de não iniciado pois foi realizado um esforço parcial na tarefa." );
        }

        [TestMethod]
        public void DevePermitirEdicaoSituacaoPlanejamentoParaProntoSePossuirEstimativaInicial()
        {
            CronogramaTarefaDto novaTarefa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 0,
                NbEstimativaRestante = 0,
                NbRealizado = 0,
            };

            CronogramaTarefaDto novaTarefaComEstimativa = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "8:00" ),
                NbRealizado = 0,
            };

            CronogramaTarefaDto tarefaConsumidaParcialmente = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = true,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "4:00" ),
            };

            CronogramaTarefaDto tarefaCanceladaParcialmenteRealizada = new CronogramaTarefaDto()
            {
                CsLinhaBaseSalva = false,
                DtAtualizadoEm = DateTime.Now,
                NbEstimativaInicial = 8,
                NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "6:00" ),
                NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "2:00" ),
            };

            string mensagem;
            Assert.IsFalse( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefa, CsTipoPlanejamento.Encerramento, out mensagem ), "Não deve permitir ir para o estado de pronto pois não possui uma estimativa inicial de duração" );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( novaTarefaComEstimativa, CsTipoPlanejamento.Encerramento, out mensagem ), "Deve permitir pois a tarefa já possui uma estimativa inicial de duração." );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaConsumidaParcialmente, CsTipoPlanejamento.Encerramento, out mensagem ), "Deve permitir pois a tarefa já possui uma estimativa inicial de duração" );
            Assert.IsTrue( TarefaEditada.ValidarEdicaoSituacaoPlanejamento( tarefaCanceladaParcialmenteRealizada, CsTipoPlanejamento.Encerramento, out mensagem ), "Deve permitir a edição pois a tarefa já possui uma estimativa inicial de duração." );
        }

		[TestMethod]
		public void DeveValidarQuandoHouverAlteracaoRelevanteNoOidDaSituacaoPlanejamentoDaTarefa()
		{
			Func<Guid> criarNovoOid =  () => Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = criarNovoOid()
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = criarNovoOid()
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );
			
			CollectionAssert.AreEqual( new List<int>() { (int)CsTipoCampoEditado.SituacaoPlanejamento } , alteracao , "Deveria ter sido alterada a situação de planejamento da tarefa" );
		}

		[TestMethod]
		public void DeveValidarQuandoHouverAlteracaoRelevanteNaEstimativaInicial()
		{
			Guid oidTarefa = Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 5 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );

			Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaInicial , alteracao.First() , "Deveria ter sido alterada a estimativa inicial da tarefa" );
		}

		[TestMethod]
		public void DeveValidarQuandoHouverAlteracaoRelevanteNaEstimativaRestante()
		{
			Guid oidTarefa = Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 5 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );
			Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaRestante , alteracao.First() , "Deveria ter sido alterada a estimativa restante da tarefa" );
		}

		[TestMethod]
		public void DeveValidarQuandoHouverAlteracaoRelevanteNaEstimativaRestanteEEstimativaInicial()
		{
			Guid oidTarefa = Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 2 ,
				NbEstimativaRestante = 5 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );

			CollectionAssert.AreEqual( new List<int>() { (int)CsTipoCampoEditado.EstimativaInicial , (int)CsTipoCampoEditado.EstimativaRestante } , alteracao , "Deveria ter verificado a alteração na estimativa restante e na estimativa inicial" );
		}

		[TestMethod]
		public void DeveValidarQuandoHouverAlteracaoRelevanteNaEstimativaRestanteEstimativaInicialENoOidSituacaoPlanejamento()
		{
			Guid oidTarefa = Guid.NewGuid();
			Guid oidTarefa2 = Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 2 ,
				NbEstimativaRestante = 5 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa2
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );
			CollectionAssert.AreEqual( new List<int>() { (int)CsTipoCampoEditado.SituacaoPlanejamento, (int)CsTipoCampoEditado.EstimativaInicial , (int)CsTipoCampoEditado.EstimativaRestante } , alteracao , "Deveria ter verificado a alteração na estimativa restante e na estimativa inicial" );
		}

		[TestMethod]
		public void NaoDeveRetornarNenhumaAlteracaoRelevante()
		{
			Guid oidTarefa = Guid.NewGuid();
			CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			CronogramaTarefaGridItem tarefaAlterada = new CronogramaTarefaGridItem()
			{
				CsLinhaBaseSalva = false ,
				DtAtualizadoEm = DateTime.Now ,
				NbEstimativaInicial = 0 ,
				NbEstimativaRestante = 0 ,
				NbRealizado = 0 ,
				OidSituacaoPlanejamentoTarefa = oidTarefa
			};

			var alteracao = TarefaEditada.VerificarAlteracoesRelevantes( tarefa , tarefaAlterada );
			CollectionAssert.AreEqual( new List<int>() , alteracao , "Não deveria haver alteracoes relevantes" );
		}
    }
}
