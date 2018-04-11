using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Comparacao;
using Moq;

namespace WexProject.Schedule.Test.UnitTest
{
	[TestClass]
	public class GerenciadorEdicaoCronogramaTest
	{
		#region Atributos auxiliares do ambiente de teste
		private CronogramaDto cronogramaAtual,resultadoComparacao;
		private GerenciadorEdicaoCronograma gerenciador;
		private Mock<IEditorDadosCronograma> mockEditor;
		private IEditorDadosCronograma editor;
		#endregion

		#region Métodos Auxiliares do ambiente de testes
		[TestInitialize]
		public void Inicializar()
		{
			mockEditor = new Mock<IEditorDadosCronograma>();
			mockEditor.Setup( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) ).Callback<CronogramaDto , CronogramaDto>( SalvarEdicao );
			mockEditor.Setup( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) ).Callback<CronogramaDto , CronogramaDto>( RecusarEdicao );
			editor = mockEditor.Object;
			cronogramaAtual = CriarCronograma( "Cronograma 01" , DateTime.Now.Date , DateTime.Now.Date.AddDays( 10 ) );
			gerenciador = new GerenciadorEdicaoCronograma( editor );
		}

		private void RecusarEdicao( CronogramaDto cronogramaAlterado , CronogramaDto cronogramaOriginal )
		{
			cronogramaAlterado.TxDescricao = cronogramaOriginal.TxDescricao;
			cronogramaAlterado.DtInicio = cronogramaOriginal.DtInicio;
			cronogramaAlterado.DtFinal = cronogramaOriginal.DtFinal;
			resultadoComparacao = cronogramaOriginal;
		}

		private void SalvarEdicao( CronogramaDto cronogramaAlterado , CronogramaDto cronogramaOriginal )
		{
			resultadoComparacao = cronogramaAlterado;
		}


		private CronogramaDto CriarCronograma( string descricao , DateTime dtInicio , DateTime dtFinal )
		{
			return new CronogramaDto
			{
				TxDescricao = descricao ,
				Oid = Guid.NewGuid() ,
				OidSituacaoPlanejamento = Guid.NewGuid() ,
				DtInicio = dtInicio ,
				DtFinal = dtFinal
			};
		} 
		#endregion

		[TestMethod]
		public void DeveAoEntrarNoModoEdicaoDeveClonarOCronogramaParaSalvarOEstadoAnteriorAEdicao()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			Assert.IsTrue( gerenciador.EmEdicaoNaView , "Deveria estar no estado de edição na view" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );
			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de recusa de edição" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de permitido salvar" );
			mockEditor.Verify( o => o.ComunicarInicioEdicaoDadosCronograma() , Times.Once() , "Deveria comunicar o inicio da edição" );
		}

		[TestMethod]
		public void DeveEntrarSomenteUmVezEmEdicaoEnquantoNaoReceberRespostaSolicitacao()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			Assert.IsTrue( gerenciador.EmEdicaoNaView , "Deveria estar no estado de edição na view" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );

			gerenciador.FimEdicaoDadosCronograma();
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );

			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de recusa de edição" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de permitido salvar" );
			mockEditor.Verify( o => o.ComunicarInicioEdicaoDadosCronograma() , Times.Once() , "Deveria comunicar o inicio da edição" );
		}

		[TestMethod]
		public void NaoDeveSalvarEnquantoNaoReceberMensagemDeAutorizacao()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			gerenciador.FimEdicaoDadosCronograma();

			Assert.IsFalse( gerenciador.EmEdicaoNaView , "A view não deveria estar em edição" );

			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de recusa de edição" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de permitido salvar" );
			
			Assert.IsTrue( gerenciador.AguardandoRetorno , "Deveria estar no estado de aguardo do retorno" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );
		}

		[TestMethod]
		public void NaoDeveDispararOEventoDeSalvarQuandoReceberAInformacaoDeAutorizacaoDaEdicaoMasEstiverEmEdicaoNaView()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			gerenciador.PermitirSalvarEdicao();

			Assert.IsTrue( gerenciador.EmEdicaoNaView , "A view ainda deveria estar em edição" );

			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de recusa de edição" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter executado o evento de permitido salvar" );
			
			Assert.IsFalse( gerenciador.AguardandoRetorno , "Não deveria estar aguardando o retorno" );
			Assert.IsTrue( gerenciador.Autorizado , "deveria estar no estado de autorizado" );
		}

		[TestMethod]
		public void DeveDispararOEventoDeSalvarQuandoReceberAInformacaoDeAutorizacaoDaEdicaoEJaTiverEncerradoAEdicao()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );

			gerenciador.FimEdicaoDadosCronograma();
			cronogramaAtual.TxDescricao = "Cronograma Descricao Alterada";
			gerenciador.PermitirSalvarEdicao();

			Assert.IsFalse( gerenciador.EmEdicaoNaView , "A view não deveria estar em edição" );

			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter acionado o desfazer alteracao" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Once , "Deveria ter acionado o salvar edicao" );
			Assert.IsFalse( gerenciador.AguardandoRetorno , "Não deveria estar aguardando o retorno" );
		}

		[TestMethod]
		public void DeveDispararOEventoDeSalvarQuandoReceberAInformacaoDeAutorizacaoDaEdicaoEEntaoEncerrarAEdicao()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			gerenciador.PermitirSalvarEdicao();

			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never , "Não deveria ter acionado o salvar edicao" );
			gerenciador.FimEdicaoDadosCronograma();

			Assert.IsFalse( gerenciador.EmEdicaoNaView , "A view não deveria estar em edição" );

			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Once , "Deveria ter acionado o salvar edicao" );
			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never() , "Não deveria ter acionado o desfazer alteracao" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );
		}

		[TestMethod]
		public void DeveDispararOEventoDeRecusaQuandoReceberAInformacaoDeRecusaDaEdicaoEEstiverEditando()
		{
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );
			gerenciador.RecusarSalvarEdicao();

			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never , "Não Deveria ter acionado o salvar edicao" );
			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Once() , "Deveria ter acionado o desfazer alteracao" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );
		}

		[TestMethod]
		public void DeveDispararOEventoDeRecusaQuandoReceberAInformacaoDeRecusaEJaTiverEncerradoAEdicao()
		{
			var cronogramaAlterado = CriarCronograma( "Cronograma Descricao Alterada" , cronogramaAtual.DtInicio , cronogramaAtual.DtFinal );
			gerenciador.InicioEdicaoDadosCronograma( cronogramaAtual );

			gerenciador.RecusarSalvarEdicao();
			gerenciador.FimEdicaoDadosCronograma();
			Assert.IsFalse( gerenciador.EmEdicaoNaView , "A view não deveria estar em edição" );
			mockEditor.Verify( o => o.SalvarEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Never , "Não deveria ter acionado o salvar edicao" );
			mockEditor.Verify( o => o.DesfazerEdicaoDadosCronograma( It.IsAny<CronogramaDto>() , It.IsAny<CronogramaDto>() ) , Times.Once() , "Deveria ter acionado o desfazer alteracao" );
			Assert.IsFalse( gerenciador.Autorizado , "Não deveria estar no estado de autorizado" );
		}
	}
}
