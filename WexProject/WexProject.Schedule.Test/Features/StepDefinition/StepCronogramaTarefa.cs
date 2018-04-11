using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Test.UnitTest;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.BOs.Planejamento;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Tarefa do Cronograma
    /// </summary>
    [Binding]
    class StepCronogramaTarefa : BaseEntityFrameworkTest
    {
        #region Properties

        /// <summary>
        /// Dicionario de Tarefas do Cronograma usados no Step
        /// </summary>
        public static Dictionary<string, CronogramaTarefa> TarefasDic { get; set; }

        /// <summary>
        /// Lista de Tarefas selecionadas
        /// </summary>
        private List<CronogramaTarefa> tarefasSelecionadas;

        #endregion

        #region BeforeCenarios

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public void ReiniciarValores()
        {
            TarefasDic = new Dictionary<string, CronogramaTarefa>();
            tarefasSelecionadas = new List<CronogramaTarefa>();
        }

        #endregion

        #region Dados

        [Given(@"uma tarefa do cronograma '(.*)' com os dados:")]
        public void DadoUmaTarefaDoCronogramaCronograma01ComOsDadosASeguir(string cronograma, Table table)
        {
            string id = table.Rows[0][table.Header.ToList()[1]];
            string nome = table.Rows[1][table.Header.ToList()[1]];

            TarefasDic.Add(nome, CronogramaFactoryEntity.CriarTarefa( contexto, ushort.Parse(id), nome, string.Empty, null, 2, new TimeSpan(0), colaboradorPadrao, StepCronograma.CronogramasDic[cronograma], null, true));
        }

        //[Given(@"o colaborador '(.*)' ter modificado a tarefa '(.*)':"),
        // When(@"o colaborador '(.*)' modificar a tarefa '(.*)':")]
        //public static void DadoOColaboradorColaborador01TerModificadoAaTarefaTarefa01(string colaborador, string tarefa, Table table)
        //{
        //    UserDao.CurrentUser = StepColaborador.ColaboradoresDic[colaborador].Usuario;

        //    if (table.Rows.Count > 1)
        //    {
        //        DateUtil.CurrentDateTime = DateTime.Parse(table.Rows[1][table.Header.ToList()[1]]);
        //    }

        //    string descricao = table.Rows[0][table.Header.ToList()[1]];
        //    CronogramaTarefa old = TarefasDic[tarefa].RnGetClone();

        //    TarefasDic[tarefa].Tarefa.TxDescricao = descricao;
        //    TarefasDic[tarefa].Tarefa.Save();
        //    //TarefaLogAlteracao.RnCriarLogTarefa(TarefasDic[tarefa].Tarefa, old.Tarefa);
        //}

        #endregion

        #region Quando

        [When(@"selecionar a\(s\) tarefa\(s\) (('[\w\sçãáéíóú]+',?[\s]*?)+)$")]
        public void QuandoSelecionarASTarefaSTarefa01(string tarefas, string naousado)
        {
            foreach (string tarefa in tarefas.Split(','))
            {
                string value01;

                // Valores encontrados
                value01 = tarefa.Substring(1, tarefa.Length - 2); // retiradas das aspas simples

                // Selecionando tarefa
                tarefasSelecionadas.Add(TarefasDic[value01]);
            }
        }

        [When(@"o colaborador logado criar as seguintes tarefas para o cronograma '(.*)':")]
        public void QuandoOColaboradorColaborador01CriarAsSeguintesTarefas(string cronograma, Table table)
        {
            string id = table.Header.ToList()[0];
            string nome = table.Header.ToList()[1];

            foreach (TableRow row in table.Rows)
            {
                string idRow = row[id];
                string nomeRow = row[nome];

                TarefasDic.Add(nomeRow,
                CronogramaFactoryEntity.CriarTarefa( contexto, ushort.Parse( idRow ), nomeRow, string.Empty, null, 2, new TimeSpan( 0 ), ColaboradorDAO.ConsultarColaborador( "anderson.lins" ), StepCronograma.CronogramasDic[cronograma], null, true ) );
            }
        }

        #endregion

        #region Então

        [Then(@"o botão 'Histórico de Atualização' deve estar habilitado")]
        public void EntaoOBotaoHistoricoDeAtualizacaoDeveEstarHabilitadoParaASTarefaSTarefa01()
        {
            Assert.IsTrue( TarefaLogAlteracaoBo.IsHabilitarHistoricoAtualizacao( tarefasSelecionadas.Count, tarefasSelecionadas[0].Tarefa ) );
        }

        [Then(@"o botão 'Histórico de Atualização' não deve estar habilitado")]
        public void EntaoOBotaoHistoricoDeAtualizacaoNaoDeveEstarHabilitadoParaASTarefaSTarefa01()
        {
            Assert.IsFalse( TarefaLogAlteracaoBo.IsHabilitarHistoricoAtualizacao( tarefasSelecionadas.Count, tarefasSelecionadas[0].Tarefa ) );
        }

        [Then(@"o histórico de log da '(.*)' deve estar:")]
        public static void EntaoOHistoricoDeAtualizacaoDaTarefaTarefa01DeveSer(string tarefa, Table table)
        {
            string dataHoraColumn = table.Header.ToList()[0];
            string colaboradorColumn = table.Header.ToList()[1];
            string alteracaoColumn = table.Header.ToList()[2];

            // Ordenação dos Logs
            TarefasDic[tarefa].Tarefa.LogsAlteracao = TarefasDic[tarefa].Tarefa.LogsAlteracao.OrderByDescending( o => o.DtDataHoraLog ).ToList();
            List<TarefaLogAlteracao> tarefasLogsAlteracao = TarefasDic[tarefa].Tarefa.LogsAlteracao.ToList();

            Assert.AreEqual(table.RowCount, TarefasDic[tarefa].Tarefa.LogsAlteracao.Count, "Deveriam ter a mesma quantidade de registros");

            for (short position = 0; position < table.RowCount; position++)
            {
                // Data/Hora
                Assert.AreEqual( DateTime.Parse( table.Rows[position][dataHoraColumn] ), tarefasLogsAlteracao[position].DtDataHoraLog );

                // Colaborador
                Assert.AreEqual( table.Rows[position][colaboradorColumn], tarefasLogsAlteracao[position].Colaborador.Usuario.UserName );

                // Colaborador
                Assert.AreEqual( table.Rows[position][alteracaoColumn], tarefasLogsAlteracao[position].TxAlteracoes );
            }
        }

        [Then(@"a tarefa '(.*)' deve estar com os dados de última atualização:")]
        public static void EntaoATarefa02DeveEstarComOsDadosDeUltimaAtualizacao(string tarefa, Table table)
        {
            Assert.AreEqual(table.Rows[0][table.Header.ToList()[1]],
                TarefasDic[tarefa].Tarefa.DtAtualizadoEm);

            Assert.AreEqual(table.Rows[1][table.Header.ToList()[1]],
                TarefasDic[tarefa].Tarefa.AtualizadoPor);
        }

        #endregion
    }
}
