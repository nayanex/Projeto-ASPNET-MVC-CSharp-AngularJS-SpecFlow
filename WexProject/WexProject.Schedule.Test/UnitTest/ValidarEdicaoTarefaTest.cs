using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Libs.ControleEdicao;
using WexProject.Schedule.Test.Fixtures.Factory;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class ValidarEdicaoTarefaTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void RnValidarTarefaEditadaQuandoSituacaoPlanejamentoSejaAlteradaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento,
                                                        CsPadraoSistema.Não, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento1, new DateTime(),
                                                                        new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //adiciona colaborador como responsável pela tarefa
            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da RnIncluirTarefa em CronogramaTarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento1, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            //recupera a tarefa criada
            CronogramaTarefa tarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid , o => o.Tarefa.SituacaoPlanejamento );

            //criar um Dto da tarefa e repassa as informações para poder utilizar o método que está sendo testado
            CronogramaTarefaGridItem tarefaDto = new CronogramaTarefaGridItem();
            tarefaDto.OidCronogramaTarefa = tarefa.Oid;
            tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa = tarefa.Tarefa.SituacaoPlanejamento.TxDescricao;
            tarefaDto.NbEstimativaInicial = tarefa.Tarefa.NbEstimativaInicial;
            tarefaDto.NbEstimativaRestante = tarefa.Tarefa.NbEstimativaRestante;

            //adiciona tarefa na lista de tarefas antigas (antes de serem alteradas)
            List<CronogramaTarefaGridItem> tarefasAntigas = new List<CronogramaTarefaGridItem>();
            tarefasAntigas.Add( tarefaDto );

            //cria uma tarefa Dto alterada e modifica o campo situação de planejamento
            CronogramaTarefaGridItem tarefaDtoAlterada = new CronogramaTarefaGridItem();
            tarefaDtoAlterada.OidCronogramaTarefa = tarefaDto.OidCronogramaTarefa;
            tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa = situacaoPlanejamento2.TxDescricao;
            tarefaDtoAlterada.NbEstimativaInicial = tarefaDto.NbEstimativaInicial;
            tarefaDtoAlterada.NbEstimativaRestante = tarefaDto.NbEstimativaRestante;

            //adiciona a tarefa na lista de tarefas modificadas.
            List<CronogramaTarefaGridItem> tarefasAtualizadas = new List<CronogramaTarefaGridItem>();
            tarefasAtualizadas.Add( tarefaDtoAlterada );

            //valida os campos alterados
            Hashtable camposAlterados = TarefaEditada.ValidarCamposRelevantesAlterados( tarefasAntigas, tarefasAtualizadas );

            List<int> campos = new List<int>();
            campos = (List<int>)camposAlterados[tarefaDto.OidCronogramaTarefa];

            Assert.AreEqual( 1, campos.Count, "Deveria ter sido alterado apenas um campo." );
            Assert.AreEqual( (int)CsTipoCampoEditado.SituacaoPlanejamento, campos[0], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreNotEqual( tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa, tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa, "Não deveriam ser iguais, pois o campo foi alterado." );
        }

        [TestMethod]
        public void RnValidarTarefaEditadaQuandoEstimativaInicialSejaAlteradaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento1, new DateTime(),
                                                                        new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //adiciona colaborador como responsável pela tarefa
            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da RnIncluirTarefa em CronogramaTarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento1, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            //recupera a tarefa criada
            CronogramaTarefa tarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid , o=>o.Tarefa.SituacaoPlanejamento );

            //criar um Dto da tarefa e repassa as informações para poder utilizar o método que está sendo testado
            CronogramaTarefaGridItem tarefaDto = new CronogramaTarefaGridItem();
            tarefaDto.OidCronogramaTarefa = tarefa.Oid;
            tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa = tarefa.Tarefa.SituacaoPlanejamento.TxDescricao;
            tarefaDto.NbEstimativaInicial = tarefa.Tarefa.NbEstimativaInicial;
            tarefaDto.NbEstimativaRestante = tarefa.Tarefa.NbEstimativaRestante;

            //adiciona tarefa na lista de tarefas antigas (antes de serem alteradas)
            List<CronogramaTarefaGridItem> tarefasAntigas = new List<CronogramaTarefaGridItem>();
            tarefasAntigas.Add( tarefaDto );

            //cria uma tarefa Dto alterada e modifica o campo situação de planejamento
            CronogramaTarefaGridItem tarefaDtoAlterada = new CronogramaTarefaGridItem();
            tarefaDtoAlterada.OidCronogramaTarefa = tarefaDto.OidCronogramaTarefa;
            tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa = tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa;
            tarefaDtoAlterada.NbEstimativaInicial = 5;
            tarefaDtoAlterada.NbEstimativaRestante = tarefaDto.NbEstimativaRestante;

            //adiciona a tarefa na lista de tarefas modificadas.
            List<CronogramaTarefaGridItem> tarefasAtualizadas = new List<CronogramaTarefaGridItem>();
            tarefasAtualizadas.Add( tarefaDtoAlterada );

            //valida os campos alterados
            Hashtable camposAlterados = TarefaEditada.ValidarCamposRelevantesAlterados( tarefasAntigas, tarefasAtualizadas );

            List<int> campos = new List<int>();
            campos = (List<int>)camposAlterados[tarefaDto.OidCronogramaTarefa];

            Assert.AreEqual( 1, campos.Count, "Deveria ter sido alterado apenas um campo." );
            Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaInicial, campos[0], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreNotEqual( tarefaDto.NbEstimativaInicial, tarefaDtoAlterada.NbEstimativaInicial, "Não deveriam ser iguais, pois o campo foi alterado." );
        }

        [TestMethod]
        public void RnValidarTarefaEditadaQuandoEstimativaRestanteSejaAlteradaTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento1, new DateTime(),
                                                                        new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //adiciona colaborador como responsável pela tarefa
            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da RnIncluirTarefa em CronogramaTarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento1, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            //recupera a tarefa criada
            CronogramaTarefa tarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid , o=> o.Tarefa.SituacaoPlanejamento );

            //criar um Dto da tarefa e repassa as informações para poder utilizar o método que está sendo testado
            CronogramaTarefaGridItem tarefaDto = new CronogramaTarefaGridItem();
            tarefaDto.OidCronogramaTarefa = tarefa.Oid;
            tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa = tarefa.Tarefa.SituacaoPlanejamento.TxDescricao;
            tarefaDto.NbEstimativaInicial = tarefa.Tarefa.NbEstimativaInicial;
            tarefaDto.NbEstimativaRestante = tarefa.Tarefa.NbEstimativaRestante;

            //adiciona tarefa na lista de tarefas antigas (antes de serem alteradas)
            List<CronogramaTarefaGridItem> tarefasAntigas = new List<CronogramaTarefaGridItem>();
            tarefasAntigas.Add( tarefaDto );

            //cria uma tarefa Dto alterada e modifica o campo situação de planejamento
            CronogramaTarefaGridItem tarefaDtoAlterada = new CronogramaTarefaGridItem();
            tarefaDtoAlterada.OidCronogramaTarefa = tarefaDto.OidCronogramaTarefa;
            tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa = tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa;
            tarefaDtoAlterada.NbEstimativaInicial = tarefaDto.NbEstimativaInicial;
            tarefaDtoAlterada.NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "1:00" );

            //adiciona a tarefa na lista de tarefas modificadas.
            List<CronogramaTarefaGridItem> tarefasAtualizadas = new List<CronogramaTarefaGridItem>();
            tarefasAtualizadas.Add( tarefaDtoAlterada );

            //valida os campos alterados
            Hashtable camposAlterados = TarefaEditada.ValidarCamposRelevantesAlterados( tarefasAntigas, tarefasAtualizadas );

            List<int> campos = new List<int>();
            campos = (List<int>)camposAlterados[tarefaDto.OidCronogramaTarefa];

            Assert.AreEqual( 1, campos.Count, "Deveria ter sido alterado apenas um campo." );
            Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaRestante, campos[0], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreNotEqual( tarefaDto.NbEstimativaRestante, tarefaDtoAlterada.NbEstimativaRestante, "Não deveriam ser iguais, pois o campo foi alterado." );
        }

        [TestMethod]
        public void RnValidarTarefaEditadaQuandoSituacaoEEstimativasSejamAlteradasTest()
        {
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução,
                                                        CsPadraoSistema.Sim, true );

            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento2 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S2",
                                                        CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Cancelamento,
                                                        CsPadraoSistema.Não, true );

            //cria cronograma
            Cronograma cronograma1 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma 01", situacaoPlanejamento1, new DateTime(),
                                                                        new DateTime(), true );

            //cria colaborador
            Colaborador colaborador1 = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", true );

            //adiciona colaborador como responsável pela tarefa
            string responsaveis = colaborador1.NomeCompleto;

            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria tarefa a partir da RnIncluirTarefa em CronogramaTarefa
            CronogramaTarefa novaTarefa = CronogramaTarefaBo.CriarCronogramaTarefa( cronograma1.Oid, "Tarefa 01", situacaoPlanejamento1, DateTime.Now, responsaveis, colaborador1.Usuario.UserName, out tarefasImpactadas, ref dataHoraAcao, "Criar método1", 3, 0 );

            //recupera a tarefa criada
            CronogramaTarefa tarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( novaTarefa.Oid , o => o.Tarefa.SituacaoPlanejamento );

            //criar um Dto da tarefa e repassa as informações para poder utilizar o método que está sendo testado
            CronogramaTarefaGridItem tarefaDto = new CronogramaTarefaGridItem();
            tarefaDto.OidCronogramaTarefa = tarefa.Oid;
            tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa = tarefa.Tarefa.SituacaoPlanejamento.TxDescricao;
            tarefaDto.NbEstimativaInicial = tarefa.Tarefa.NbEstimativaInicial;
            tarefaDto.NbEstimativaRestante = tarefa.Tarefa.NbEstimativaRestante;

            //adiciona tarefa na lista de tarefas antigas (antes de serem alteradas)
            List<CronogramaTarefaGridItem> tarefasAntigas = new List<CronogramaTarefaGridItem>();
            tarefasAntigas.Add( tarefaDto );

            //cria uma tarefa Dto alterada e modifica o campo situação de planejamento
            CronogramaTarefaGridItem tarefaDtoAlterada = new CronogramaTarefaGridItem();
            tarefaDtoAlterada.OidCronogramaTarefa = tarefaDto.OidCronogramaTarefa;
            tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa = situacaoPlanejamento2.TxDescricao;
            tarefaDtoAlterada.NbEstimativaInicial = 18;
            tarefaDtoAlterada.NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks( "5:00" );

            //adiciona a tarefa na lista de tarefas modificadas.
            List<CronogramaTarefaGridItem> tarefasAtualizadas = new List<CronogramaTarefaGridItem>();
            tarefasAtualizadas.Add( tarefaDtoAlterada );

            //valida os campos alterados
            Hashtable camposAlterados = TarefaEditada.ValidarCamposRelevantesAlterados( tarefasAntigas, tarefasAtualizadas );

            List<int> campos = new List<int>();
            campos = (List<int>)camposAlterados[tarefaDto.OidCronogramaTarefa];

            Assert.AreEqual( 3, campos.Count, "Deveriam ter sido alterados 3 campos." );
            Assert.AreEqual( (int)CsTipoCampoEditado.SituacaoPlanejamento, campos[0], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaInicial, campos[1], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreEqual( (int)CsTipoCampoEditado.EstimativaRestante, campos[2], "O campo alterado deveria ser do mesmo Tipo" );
            Assert.AreNotEqual( tarefaDto.TxDescricaoSituacaoPlanejamentoTarefa, tarefaDtoAlterada.TxDescricaoSituacaoPlanejamentoTarefa, "Não deveriam ser iguais, pois o campo foi alterado." );
            Assert.AreNotEqual( tarefaDto.NbEstimativaInicial, tarefaDtoAlterada.NbEstimativaInicial, "Não deveriam ser iguais, pois o campo foi alterado." );
            Assert.AreNotEqual( tarefaDto.NbEstimativaRestante, tarefaDtoAlterada.NbEstimativaRestante, "Não deveriam ser iguais, pois o campo foi alterado." );
        }
    }
}