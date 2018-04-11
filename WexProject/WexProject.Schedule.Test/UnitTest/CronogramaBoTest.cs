using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaBoTest : BaseEntityFrameworkTest
    {
		[TestInitialize]
		public void ContruirCenarioPadaoTest()
		{
			SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto , "Em Andamento" , CsTipoSituacaoPlanejamento.Ativo , CsTipoPlanejamento.Execução , CsPadraoSistema.Sim , true );
		}

        [TestMethod]
        public void CriarCronogramaQuandoNaoHouverNomePadraoIgualNoBancoTest()
        {
            List<Cronograma> cronogramas = contexto.Cronograma.ToList();

            string TxDescricaoCronograma1 = "Wex Cronograma " + String.Format( "{00:00}", ( cronogramas.Count + 1 ) );

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao();

            cronogramas = contexto.Cronograma.ToList();

            string TxDescricaoCronograma2 = "Wex Cronograma " + String.Format( "{00:00}", ( cronogramas.Count + 1 ) );

            Cronograma cronogramaPadrao2 = CronogramaBo.CriarCronogramaPadrao( contexto );

            Assert.AreEqual( TxDescricaoCronograma1, cronogramaPadrao1.TxDescricao );
            Assert.AreEqual( TxDescricaoCronograma2, cronogramaPadrao2.TxDescricao );
        }

        [TestMethod]
        public void CriarCronogramaPadraoTest()
        {
            List<Cronograma> cronogramas = contexto.Cronograma.ToList();

            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao( contexto );

            cronogramas = contexto.Cronograma.ToList();

            Cronograma cronogramaPadrao2 = CronogramaBo.CriarCronogramaPadrao( contexto );

            Assert.IsNotNull( cronogramaPadrao1 );
            Assert.IsNotNull( cronogramaPadrao2 );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoCronogramaNaoPossuirTarefasEPossuirUltimaSelecaoTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            User usuario = CronogramaFactoryEntity.CriarUsuario( contexto, "anderson.lins", "Anderson", "Lins", "anderson.lins@fpf.br", true );

            CronogramaUltimaSelecao ultimaSelecao = new CronogramaUltimaSelecao();
            ultimaSelecao.DataAcesso = DateTime.Now;
            ultimaSelecao.Usuario = usuario;
            ultimaSelecao.OidUltimoCronograma = cronograma1.Oid;
            ultimaSelecao.Cronograma = cronograma1;

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, cronograma1.Oid );

            Assert.IsTrue( cronogramaExcluido, "Deveria ser verdadeiro, pois o cronograma não possui tarefas associadas a ele." );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoCronogramaPossuirTarefasENaoPossuirUltimaSelecaoTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            CronogramaTarefa cronogramaTarefa = CronogramaFactoryEntity.CriarTarefa( contexto, 1, "Tarefa 01", "", situacaoPlanejamento, 0, new TimeSpan(), colaborador, cronograma1, null, true );

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, cronograma1.Oid );

            Assert.IsTrue( cronogramaExcluido, "Deveria ser true, pois o cronograma exclui quaisquer tarefas associadas a ele." );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoCronogramaNaoPossuirTarefasENaoPossuirUltimaSelecaoParaUmUsuarioTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, cronograma1.Oid );

            Assert.IsTrue( cronogramaExcluido, "Deveria ser verdadeiro, pois o cronograma não possui tarefas e ele ainda existe" );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoCronogramaPossuirTarefasEUltimaSelecaoParaUmUsuarioTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", new DateTime(), "anderson.lins@fpf.br", "Anderson", "", "Lins", "anderson.lins", null, true );

            CronogramaTarefa cronogramaTarefa = CronogramaFactoryEntity.CriarTarefa( contexto, 1, "Tarefa 01", "", situacaoPlanejamento, 0, new TimeSpan(), colaborador, cronograma1, null, true );

            CronogramaUltimaSelecao ultimaSelecao = new CronogramaUltimaSelecao();
            ultimaSelecao.DataAcesso = DateTime.Now;
            ultimaSelecao.Usuario = colaborador.Usuario;
            ultimaSelecao.OidUltimoCronograma = cronograma1.Oid;
            ultimaSelecao.Cronograma = cronograma1;

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, cronograma1.Oid );

            Assert.IsTrue( cronogramaExcluido, "Deveria ser true, pois o cronograma exclui quaisquer tarefas associadas a ele." );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoCronogramaNaoExistirMaisTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Guid oidCronograma = cronograma1.Oid;

            contexto.Cronograma.Remove( cronograma1 );
            contexto.SaveChanges();

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, oidCronograma );

            Assert.IsFalse( cronogramaExcluido, "Deveria ser falo, pois o cronograma não deveria existir mais." );
        }

        [TestMethod]
        public void CriarTarefaQuandoTarefasForemImpactadasTest()
        {
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Cronograma cronograma2 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 02", situacaoPlanejamento, new DateTime(), new DateTime(), true );

            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            string responsaveis = colaborador1.NomeCompleto;

            //iníco tarefa
            DateTime dtInicio = new DateTime();

            List<CronogramaTarefa> tarefasImpactadas1;
            List<CronogramaTarefa> tarefasImpactadas2;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefas cronograma1
            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas1, ref dataHoraAcao, 3, 0 );

            List<CronogramaTarefa> lstCronoTarefa1 = contexto.CronogramaTarefa.Where( o => o.OidCronograma == cronograma1.Oid ).ToList();

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 02", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas1, ref dataHoraAcao, 3, lstCronoTarefa1[0].NbID );

            lstCronoTarefa1 = contexto.CronogramaTarefa.Where( o => o.OidCronograma == cronograma1.Oid ).ToList();

            CronogramaBo.CriarTarefa( cronograma1.Oid, "Tarefa 03", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas1, ref dataHoraAcao, 3, lstCronoTarefa1[0].NbID );

            lstCronoTarefa1 = contexto.CronogramaTarefa.Where( o => o.OidCronograma == cronograma1.Oid ).ToList();

            CronogramaBo.CriarTarefa( cronograma2.Oid, "Tarefa 01", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas2, ref dataHoraAcao, 3, 0 );

            List<CronogramaTarefa> lstCronoTarefa2 = contexto.CronogramaTarefa.Where( o => o.OidCronograma == cronograma2.Oid ).ToList();

            CronogramaBo.CriarTarefa( cronograma2.Oid, "Tarefa 02", situacaoPlanejamento.Oid.ToString(), dtInicio, colaborador1.Usuario.UserName, "Criar método", responsaveis, out tarefasImpactadas2, ref dataHoraAcao, 3, lstCronoTarefa2[0].NbID );

            lstCronoTarefa2 = contexto.CronogramaTarefa.Where( o => o.OidCronograma == cronograma2.Oid ).ToList();

            Assert.IsNotNull( lstCronoTarefa1 );
            Assert.AreEqual( 3, lstCronoTarefa1.Count );
            Assert.AreEqual( 2, tarefasImpactadas1.Count );

            Assert.IsNotNull( lstCronoTarefa2 );
            Assert.AreEqual( 2, lstCronoTarefa2.Count );
        }

        [TestMethod]
        public void ExcluirCronogramaQuandoExistirTarefasComHistoricoNoCronogramaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamentoNaoIniciado = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Não Iniciado",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );

            SituacaoPlanejamento situacaoPlanejamentoEmAndamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Em Andamento",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Planejamento,
                                                        CsPadraoSistema.Sim, true );


            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamentoNaoIniciado, new DateTime(), new DateTime(), true );

            List<CronogramaTarefa> tarefasReordenadas = new List<CronogramaTarefa>();
            DateTime dataHoraDaAcao = new DateTime();

            CronogramaTarefa cronogramaTarefa = CronogramaBo.CriarTarefa( cronograma1.Oid, (string)"Tarefa 01", situacaoPlanejamentoNaoIniciado.Oid.ToString(), DateTime.Today, colaborador.Usuario.UserName, "", "", out tarefasReordenadas, ref dataHoraDaAcao, 5 );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( (Guid)cronogramaTarefa.OidTarefa, colaborador.Usuario.UserName, new TimeSpan( 3, 0, 0 ), DateTime.Now, new TimeSpan( 8, 0, 0 ), new TimeSpan( 11, 0, 0 ), "comentário", new TimeSpan( 2, 0, 0 ), situacaoPlanejamentoEmAndamento.Oid, "" );

            TarefaHistoricoTrabalho historicoCriado = TarefaHistoricoTrabalhoDao.ConsultarTarefaHistoricoAtualPorOidTarefa( (Guid)cronogramaTarefa.OidTarefa);

            bool cronogramaExcluido = CronogramaBo.ExcluirCronograma( contexto, cronograma1.Oid );

            Assert.IsNotNull( historicoCriado, "Deveria ter criado um histórico pra tarefa" );
            Assert.IsTrue( cronogramaExcluido, "Deveria ser true, pois o cronograma exclui quaisquer tarefas associadas a ele." );
        }

        [TestMethod]
        public void DeveNaoAlterarONomeDoCronogramaQuandoPossuirCronogramaComNomesIguais()
        {
            Cronograma cronogramaPadrao1 = CronogramaBo.CriarCronogramaPadrao( contexto );
            Cronograma cronogramaPadrao2 = CronogramaBo.CriarCronogramaPadrao( contexto );

            string novaDescricao = "Wex   Cronograma  02 ";

            bool confirmacao = CronogramaBo.AlterarDescricaoCronograma( cronogramaPadrao1.Oid, novaDescricao );

            Assert.IsFalse( confirmacao, "Não deve permitir salvar um cronograma com o mesmo nome independente se contém espaços a mais." );
        }

        [TestMethod]
        public void DevePoderAlterarOsDadosDoCronograma()
        {
            var situacao = new SituacaoPlanejamento()
            {
                Oid = Guid.NewGuid(),
                CsPadrao = CsPadraoSistema.Sim,
                CsSituacao = CsTipoSituacaoPlanejamento.Ativo,
                CsTipo = CsTipoPlanejamento.Planejamento
            };

            contexto.SituacaoPlanejamento.Add( situacao );
            contexto.SaveChanges();

            var cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );
           
            var novaData = DateTime.Now.Date.AddDays(1);
            const string novaDescricao = "Wex Teste";
            var dto = new CronogramaDto
            {
                Oid =  cronograma.Oid,
                DtInicio =  cronograma.DtInicio ,
                DtFinal = cronograma.DtFinal,
                OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento,
                TxDescricao =  novaDescricao
            };

            Assert.IsTrue( CronogramaBo.EditarCronograma( dto ), "Deveria ter salvo a edição do cronograma" );

            contexto.Entry(cronograma).Reload();
            Assert.AreEqual( dto.TxDescricao, cronograma.TxDescricao, "Deveria ter salvo o nome do cronograma" );
            Assert.AreEqual( dto.DtInicio, cronograma.DtInicio, "Deveria ter salvo a data de inicio do cronograma" );
            Assert.AreEqual( dto.DtFinal, cronograma.DtFinal, "Deveria ter salvo a data de termino do cronograma" );
        }

        [TestMethod]
        public void DeveSalvarOsNovosDadosDoCronogramaQuandoADataDeInicioForAlterada()
        {
            var cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );

            var novaData = DateTime.Now.Date.AddDays( -1 );

            var dto = new CronogramaDto
            {
                Oid = cronograma.Oid,
                DtInicio = novaData,
                DtFinal = cronograma.DtFinal,
                OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento,
                TxDescricao = cronograma.TxDescricao
            };

            Assert.AreNotEqual(novaData,cronograma.DtInicio,"As datas devem ser inicialmente diferentes");
            Assert.IsTrue( CronogramaBo.EditarCronograma( dto ), "Deveria ter salvo a edição do cronograma" );

            contexto.Entry( cronograma ).Reload();
            Assert.AreEqual( dto.TxDescricao, cronograma.TxDescricao, "Deveria ter salvo o nome do cronograma" );
            Assert.AreEqual( dto.DtInicio, cronograma.DtInicio, "Deveria ter salvo a data de inicio do cronograma" );
            Assert.AreEqual( dto.DtFinal, cronograma.DtFinal, "Deveria ter salvo a data de termino do cronograma" );
        }

        [TestMethod]
        public void DeveSalvarOsNovosDadosDoCronogramaQuandoADataFinalForAlterada()
        {
            var cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );

            var novaData = DateTime.Now.Date.AddDays( 1 );

            var dto = new CronogramaDto
            {
                Oid = cronograma.Oid,
                DtInicio = cronograma.DtInicio,
                DtFinal = novaData,
                OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento,
                TxDescricao = cronograma.TxDescricao
            };

            Assert.AreNotEqual( novaData, cronograma.DtFinal, "As datas devem ser inicialmente diferentes" );
            Assert.IsTrue( CronogramaBo.EditarCronograma( dto ), "Deveria ter salvo a edição do cronograma" );

            contexto.Entry( cronograma ).Reload();
            Assert.AreEqual( dto.TxDescricao, cronograma.TxDescricao, "Deveria ter salvo o nome do cronograma" );
            Assert.AreEqual( dto.DtInicio, cronograma.DtInicio, "Deveria ter salvo a data de inicio do cronograma" );
            Assert.AreEqual( dto.DtFinal, cronograma.DtFinal, "Deveria ter salvo a data de termino do cronograma" );
        }

        [TestMethod]
        public void DeveSalvarOsNovosDadosDoCronogramaQuandoForAlteradaASituacaoPlanejamento()
        {
            var naoIniciado = new SituacaoPlanejamento()
            {
                Oid = Guid.NewGuid(),
                CsPadrao = CsPadraoSistema.Não,
                CsSituacao = CsTipoSituacaoPlanejamento.Ativo,
                CsTipo = CsTipoPlanejamento.Planejamento
            };

            contexto.SituacaoPlanejamento.Add( naoIniciado );
            contexto.SaveChanges();

            var cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );

            var dto = new CronogramaDto
            {
                Oid = cronograma.Oid,
                DtInicio = cronograma.DtInicio,
                DtFinal = cronograma.DtFinal,
                OidSituacaoPlanejamento = naoIniciado.Oid,
                TxDescricao = cronograma.TxDescricao
            };

			Assert.AreNotEqual( naoIniciado.Oid , cronograma.OidSituacaoPlanejamento , "a situação de planejamento devem ser inicialmente diferentes" );

            Assert.IsTrue( CronogramaBo.EditarCronograma( dto ), "Deveria ter salvo a edição do cronograma" );

            contexto.Entry( cronograma ).Reload();
            Assert.AreEqual( dto.TxDescricao, cronograma.TxDescricao, "Deveria ter salvo o nome do cronograma" );
            Assert.AreEqual( dto.DtInicio, cronograma.DtInicio, "Deveria ter salvo a data de inicio do cronograma" );
            Assert.AreEqual( dto.DtFinal, cronograma.DtFinal, "Deveria ter salvo a data de termino do cronograma" );
            Assert.AreEqual( dto.OidSituacaoPlanejamento, cronograma.OidSituacaoPlanejamento, "Deveria ter salvo a nova situação planejamento de termino do cronograma" );
        }

		[TestMethod]
		public void NaoDeveSalvarAsDatasDoCronogramaQuandoADataInicioForMaiorQueADataTerminoDoCronograma()
		{
			var situacao = new SituacaoPlanejamento()
			{
				Oid = Guid.NewGuid() ,
				CsPadrao = CsPadraoSistema.Sim ,
				CsSituacao = CsTipoSituacaoPlanejamento.Ativo ,
				CsTipo = CsTipoPlanejamento.Planejamento
			};

			contexto.SituacaoPlanejamento.Add( situacao );
			contexto.SaveChanges();
			var cronograma = CronogramaBo.CriarCronogramaPadrao( contexto );


			var novaData = DateTime.Now.Date.AddDays( 1 );
			var novaDataTermino = DateTime.Now.Date.AddDays( -1 );

			var dto = new CronogramaDto
			{
				Oid = cronograma.Oid ,
				DtInicio = novaData ,
				DtFinal = novaDataTermino ,
				OidSituacaoPlanejamento = cronograma.OidSituacaoPlanejamento ,
				TxDescricao = cronograma.TxDescricao
			};


			Assert.AreNotEqual( novaData , cronograma.DtFinal , "As datas devem ser inicialmente diferentes" );
			Assert.IsFalse( CronogramaBo.EditarCronograma( dto ) , "Deveria ter salvo a edição do cronograma" );

			contexto.Entry( cronograma ).Reload();
			
			Assert.AreNotEqual( dto.DtFinal , cronograma.DtFinal , "Não deveria ter salvo a data de termino do cronograma" );
		}
    }
}
