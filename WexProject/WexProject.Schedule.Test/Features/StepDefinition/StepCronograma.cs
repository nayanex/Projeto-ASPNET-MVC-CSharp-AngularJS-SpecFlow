using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.MultiAccess.Library.Dtos;
using System.Diagnostics;
using WexProject.MultiAccess.Library.Domains;
using System.Collections;
using WexProject.MultiAccess.Library.Libs;
using WexProject.Schedule.Test.UnitTest;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.Library.Libs.Test;
using WexProject.Schedule.Test.Helpers.Mocks;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Test.Utils;
using WexProject.BLL.Shared.Domains.Planejamento;
using TechTalk.SpecFlow.Assist;
using WexProject.Schedule.Test.Features.Helpers.SituacaoPlanejamentoHelper;
using WexProject.Schedule.Test.Features.Helpers.GeralHelper;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Test.Helpers.Bind;
using WexProject.Library.Libs.DataHora.Extension;
using System.Data.Entity;
using WexProject.Library.Libs.Extensions.Log;

namespace WexProject.Schedule.Test.Features.StepDefinition
{
    /// <summary>
    /// Step de Cronograma
    /// </summary>
    [Binding]
    class StepCronograma : BaseEntityFrameworkTest
    {
        #region Propriedades

        /// <summary>
        /// Responsável por guardar a hora e data que foi realizada a ação
        /// </summary>
        public static DateTime DataHoraAcao
        {
            get;
            set;
        }

        /// <summary>
        /// Dicionário de cronogramas armazenando como chave a descrição do cronograma(Nome) e um objeto cronograma
        /// </summary>
        public static Dictionary<string, Cronograma> CronogramasDic
        {
            get;
            set;
        }

        /// <summary>
        /// Dicionário de Tarefas por cronograma, armazenando como chave a descrição do cronograma(Nome) e como valor um dicionário de
        /// tarefas que armaneza como chave a descrição da tarefa(Nome da Tarefa) e como objeto um CronogramaTarefa
        /// </summary>
        public static Dictionary<string, Dictionary<string, CronogramaTarefa>> CronogramaTarefasDic
        {
            get;
            set;
        }

        #endregion

        #region Pré-Configuração de Cenário

        /// <summary>
        /// Reinicia os valores das listas
        /// </summary>
        [BeforeScenario]
        public static void ReiniciarValores()
        {
            CronogramasDic = new Dictionary<string, Cronograma>();
            CronogramaTarefasDic = new Dictionary<string, Dictionary<string, CronogramaTarefa>>();
            DataHoraAcao = DateTime.Now;
        }

        /// <summary>
        /// Efetua uma pesquisa por guid do cronograma
        /// </summary>
        /// <param name="oidCronograma">Guid oidCronograma</param>
        /// <returns>Cronograma preenchido na hash</returns>
        public static Cronograma GetCronogramaPorOidNoDicionario( string oidCronograma )
        {
            Cronograma cronogramaSelecionado = CronogramasDic.Values.First( o => o.Oid.ToString() == oidCronograma );
            return cronogramaSelecionado;
        }

        /// <summary>
        /// Retornar o access client do colaborador em determinado cronograma
        /// </summary>
        /// <param name="cronograma">cronograma atual</param>
        /// <param name="login">colaborador atual</param>
        /// <returns>AccessClient de comunicação com servidor</returns>
        public static WexMultiAccessClientMock GetAccessClient( string cronograma, string login )
        {
            string oidCronograma = GetOidCronograma( cronograma );
            return StepContextUtil.GetAccessClientNoContexto( login, oidCronograma );
        }

        /// <summary>
        /// Efetua a busca de uma tarefa no Dicionario CronogramaTarefasDic
        /// </summary>
        /// <param name="cronogramaDescricao">Nome Cronograma Atual</param>
        /// <param name="tarefaOid">oid da tarefa selecionada</param>
        /// <returns></returns>
        public CronogramaTarefa GetTarefaPorOidNoDicionario( string cronogramaDescricao, string tarefaOid )
        {
            CronogramaTarefa tarefaSelecionada = CronogramaTarefasDic[cronogramaDescricao].Values.First( o => o.Oid.ToString() == tarefaOid );
            return tarefaSelecionada;
        }


        #endregion

        #region MetodosAuxiliares

        /// <summary>
        /// Método utilizado para retornar o oid do cronograma selecionado
        /// </summary>
        /// <param name="descricaoCronograma">descrição do cronograma selecionado</param>
        /// <returns>oid do cronograma (Como String)</returns>
        public static string GetOidCronograma( string descricaoCronograma )
        {
            return CronogramasDic[descricaoCronograma].Oid.ToString();
        }

        /// <summary>
        /// Método para buscar o oid da tarefa no cronograma em que elea esta armazenada
        /// </summary>
        /// <param name="cronograma">descricao do cronograma</param>
        /// <param name="tarefa">descricao da tarefa</param>
        /// <returns></returns>
        public static string GetOidTarefaNoCronograma( string cronograma, string tarefa )
        {
            return CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
        }
        #endregion

        #region Given

        [Given( @"um cronograma '([\w\s]+)'" )]
        public void DadoUmaTarefaDoCronogramaCronograma01ComOsDadosASeguir( string cronograma )
        {
            CronogramasDic.Add( cronograma, CronogramaFactoryEntity.CriarCronograma( contexto, cronograma, null, DateTime.Now, DateTime.Now, true ) );
        }

        [Given( @"que exista\(m\) o\(s\) cronograma\(s\)" )]
        public void DadoQueExistaMOSCronogramaS( Table table )
        {
            foreach(var cronograma in table.CreateSet<CronogramaBindHelper>())
            {
                CronogramasDic.Add( cronograma.Nome, CronogramaFactoryEntity.CriarCronograma( contexto, cronograma.Nome, SituacaoPlanejamentoDAO.ConsultarSituacaoPadraoEntity( contexto ), cronograma.Inicio, cronograma.Final, true ) );
            }
        }

        [Given( @"que o cronograma '([\w\s]+)' possui as seguintes tarefas criadas pelo colaborador '([\w\s]+)':" )]
        public void DadoQueOCronogramaPossuiAsSeguintesTarefasCriadasPeloColaborador( string cronograma, string colaborador, Table table )
        {
            CronogramaTarefa novaTarefa;
            DateTime dataHoraAcao = new DateTime();
            List<CronogramaTarefa> tarefasImpactadas;
            Guid oidCronograma = CronogramasDic[cronograma].Oid;
            if(!CronogramaTarefasDic.ContainsKey( cronograma ))
                CronogramaTarefasDic.Add( cronograma, new Dictionary<string, CronogramaTarefa>() );
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            for(int position = 0; position < table.RowCount; position++)
            {
                string id = table.Rows[position][table.Header.ToList()[0]];
                string tarefa = table.Rows[position][table.Header.ToList()[1]];

                novaTarefa = CronogramaBo.CriarTarefa( oidCronograma, tarefa, situacaoPlanejamento.Oid.ToString(), DateTime.Now, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, "", String.Empty, out tarefasImpactadas, ref dataHoraAcao, 2, 0 );
                Guid oidTarefa = novaTarefa.Oid;
                CronogramaTarefa tarefaSelecionada = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidTarefa );
                CronogramaTarefasDic[cronograma].Add( tarefa, tarefaSelecionada );
            }
        }

        [Given( @"que o cronograma '([\w\s]+)' tiver a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) solicitada\(s\) para exclusao pelo colaborador '([\w\s]+)'" )]
        public void DadoOCronogramaTiverASTarefaSSolicitadaSParaExclusaoPeloColaborador( string cronograma, string tarefasString, string naoUsado, string colaborador )
        {
            List<string> tarefas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            string oidCronograma = CronogramasDic[cronograma].Oid.ToString();
            List<string> oidTarefas = new List<string>();
            foreach(string tarefa in tarefas)
            {
                oidTarefas.Add( CronogramaTarefasDic[cronograma][tarefa].Oid.ToString() );
            }
            StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma ).RnComunicarInicioExclusaoTarefa( oidTarefas.ToArray() );
        }

        [Given( @"o cronograma '([\w\s]+)' tiver a tarefa '([\w\s]+)' movida para a posicao '([0-9]+)' pelo colaborador '([\w\s]+)'" )]
        public void DadoOCronogramaTiverATarefaMovidaParaAPosicaoPeloColaborador( string cronograma, string tarefa, string posicao, string colaborador )
        {
            DateTime dataHoraDaAcao = new DateTime();
            DateUtil.CurrentDateTime = DateTime.Now;
            Int16 posicaoFinal = Convert.ToInt16( posicao );
            Int16 posicaoInicial;
            string oidCronograma = GetOidCronograma( cronograma );
            WexMultiAccessClientMock cliente = StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma );
            CronogramaTarefa cronogramaTarefa = CronogramaTarefasDic[cronograma][tarefa];
            posicaoInicial = cronogramaTarefa.NbID;
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaDao.ConsultarTarefasImpactadas( CronogramasDic[cronograma].Oid, cronogramaTarefa.NbID, posicaoFinal );
            CronogramaTarefa tarefaDestino = CronogramaTarefasDic[cronograma].Values.Where( o => o.NbID == posicaoFinal ).FirstOrDefault();
            List<CronogramaTarefa> tarefasReordenadas = CronogramaTarefaBo.RecalcularPorBloco( cronogramaTarefa, tarefasImpactadas, ref dataHoraDaAcao, true, false, tarefaDestino.NbID );

            cliente.RnComunicarMovimentacaoTarefa( posicaoInicial, posicaoFinal, cronogramaTarefa.Oid.ToString(), tarefasReordenadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ), DateUtil.CurrentDateTime );
        }

        [Given( @"que o colaborador '([\w\s]+)' tenha solicitado do servidor permissao para alterar o nome do cronograma '([\w\s]+)'" )]
        public void DadoQueOColaboradorTenhaSolicitadoDoServidorPermissaoParaAlterarONomeDoCronograma( string login, string cronograma )
        {
            string oidCronograma = GetOidCronograma( cronograma );
            WexMultiAccessClientMock cliente = StepContextUtil.GetAccessClientNoContexto( login, oidCronograma );
            cliente.RnComunicarInicioEdicaoDadosCronograma();
        }

        [Given( @"o cronograma '([\w\s]+)' tiver a tarefa '([\w\s]+)' em edicao pelo colaborador '([\w\s]+)'" )]
        public void DadoOCronogramaTiverATarefaEmEdicaoPeloColaborador( string cronograma, string tarefa, string login )
        {
            string oidTarefa = GetOidTarefaNoCronograma( cronograma, tarefa );
            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ).RnComunicarInicioEdicaoTarefa( oidTarefa, "" );
            WexMultiAccessManagerMock manager = StepContextUtil.GetInstanciaManager();
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                return manager.TarefasEmEdicao.ContainsKey( oidCronograma ) && manager.TarefasEmEdicao[oidCronograma].ContainsKey( oidTarefa ) && manager.TarefasEmEdicao[oidCronograma][oidTarefa].Equals( login );
            } );
        }

        [Given( @"que o cronograma '(.*)' possui as seguintes tarefas:" )]
        public void DadoQueOCronogramaPossuiAsSeguintesTarefas( string nomeCronograma, Table table )
        {
            SalvaDataEHoraAtual();
            var cronograma = CronogramasDic[nomeCronograma];
            List<CronogramaTarefa> impactadas;
            DateTime dataAcao = DateTime.MinValue;
            foreach(var row in table.CreateSet<TarefaBindHelper>())
            {
                var situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPorTipo( row.Situacao );
                
                CronogramaTarefaBo.CriarCronogramaTarefa( cronograma.Oid, row.Descricao, situacaoPlanejamento, cronograma.DtInicio, "", colaboradorPadrao.Usuario.UserName
                    , out impactadas, ref dataAcao, "", row.EstimativaInicial );
            }
            CarregarDataEHoraAtual();
        }

        private static void CarregarDataEHoraAtual()
        {
            DateUtil.CurrentDateTime = ScenarioContext.Current.Get<DateTime>( "DataAtual" );
        }

        private static void SalvaDataEHoraAtual()
        {
            if(!ScenarioContext.Current.ContainsKey( "DataAtual" ))
                ScenarioContext.Current.Add( "DataAtual", DateUtil.ConsultarDataHoraAtual() );
            else
                ScenarioContext.Current["DataAtual"] = DateUtil.ConsultarDataHoraAtual();
            
        }

        [Given( @"que o cronograma '(.*)' possui o seguinte historico de trabalho:" )]
        public void DadoQueOCronogramaPossuiOSeguinteHistoricoDeTrabalho( string cronograma, Table table )
        {
            SalvaDataEHoraAtual();

            foreach(var item in table.CreateSet<HistoricoTrabalhoHelper>())
                CriarHistorico( item, CronogramasDic[cronograma].Oid );

            CarregarDataEHoraAtual();
        }

        private void CriarHistorico( HistoricoTrabalhoHelper item, Guid oidCronograma )
        {
            DateUtil.CurrentDateTime = item.Data.AddMilliseconds( DateTime.Now.Millisecond ).AddSeconds( DateTime.Now.Second );
            CronogramaTarefa cronogramaTarefa = contexto.CronogramaTarefa
                .Include( o => o.Tarefa.SituacaoPlanejamento )
                .FirstOrDefault( o => o.OidCronograma == oidCronograma && o.Tarefa.TxDescricao.ToLower().Equals( item.Tarefa.ToLower() ) );

            TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa(
                    cronogramaTarefa.OidTarefa,
                    colaboradorPadrao.Usuario.UserName,
                    item.EsforcoRealizado.ToHoursTimeSpan(),
                    DateUtil.ConsultarDataHoraAtual(),
                    new TimeSpan( 8, 0, 0 ),
                    new TimeSpan( (8 * TimeSpan.TicksPerHour + item.EsforcoRealizado.ToTicks()).ToTimeSpan().Ticks ), "",
                    item.EstimativaRestante.ToHoursTimeSpan(),
                    cronogramaTarefa.Tarefa.OidSituacaoPlanejamento.GetValueOrDefault(), "" );
        }

        #endregion

        #region When

        [When( @"o cronograma '([\w\s]+)' tiver o historico da tarefa '([\w\s]+)' salvo pelo colaborador '([\w\s]+)' com os seguintes atributos:" )]
        public void QuandoOCronogramaTiverOHistoricoDaTarefaSalvoPeloColaboradorComOsSeguintesAtributos( string cronograma, string tarefa, string colaborador, Table table )
        {
            SituacaoPlanejamento situacaoExecucao = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                                                                                                   "Em Andamento",
                                                                                                   CsTipoSituacaoPlanejamento.Ativo,
                                                                                                   CsTipoPlanejamento.Execução,
                                                                                                   CsPadraoSistema.Não,
                                                                                                   true );

            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                                                                                                   "Planejamento",
                                                                                                   CsTipoSituacaoPlanejamento.Ativo,
                                                                                                   CsTipoPlanejamento.Planejamento,
                                                                                                   CsPadraoSistema.Não,
                                                                                                   true );

            SituacaoPlanejamento situacaoCancelamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                                                                                                   "Cancelamento",
                                                                                                   CsTipoSituacaoPlanejamento.Ativo,
                                                                                                   CsTipoPlanejamento.Cancelamento,
                                                                                                   CsPadraoSistema.Não,
                                                                                                   true );

            SituacaoPlanejamento situacaoEncerramento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                                                                                                   "Encerramento",
                                                                                                   CsTipoSituacaoPlanejamento.Ativo,
                                                                                                   CsTipoPlanejamento.Encerramento,
                                                                                                   CsPadraoSistema.Não,
                                                                                                   true );

            SituacaoPlanejamento situacaoImpedimento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                                                                                                    "Impediemento",
                                                                                                    CsTipoSituacaoPlanejamento.Ativo,
                                                                                                    CsTipoPlanejamento.Impedimento,
                                                                                                    CsPadraoSistema.Não,
                                                                                                    true );

            List<SituacaoPlanejamento> situacoesPlanejamento = new List<SituacaoPlanejamento>();
            situacoesPlanejamento.Add( situacaoExecucao );
            situacoesPlanejamento.Add( situacaoPlanejamento );
            situacoesPlanejamento.Add( situacaoCancelamento );
            situacoesPlanejamento.Add( situacaoEncerramento );
            situacoesPlanejamento.Add( situacaoImpedimento );

            int qtHistoricos = table.RowCount;

            for(int i = 0; i < qtHistoricos; i++)
            {
                string[] data = table.Rows[i]["data realizado"].Split( '/' );

                SituacaoPlanejamento situacao = ( from objeto in situacoesPlanejamento
                                                  where objeto.TxDescricao.ToLower() == table.Rows[i]["situacao planejamento"].ToLower()
                                                  select objeto ).ToList().First();

                TarefaHistoricoTrabalhoBo.CriarHistoricoTarefa( CronogramaTarefasDic[cronograma][tarefa].Tarefa.Oid, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, ConversorTimeSpan.CalcularHorasTimeSpan( table.Rows[i]["hora realizado"] ), new DateTime( int.Parse( data[2] ), int.Parse( data[1] ), int.Parse( data[0] ) ), ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( table.Rows[i]["hora inicial"] ), ConversorTimeSpan.ConverterHorasDeStringParaTimeSpan( table.Rows[i]["hora final"] ), table.Rows[0]["comentario"], ConversorTimeSpan.CalcularHorasTimeSpan( table.Rows[i]["hora restante"] ), situacao.Oid, "" );
            }

            //Envia mensagem de fim edição.
            QuandoOCronogramaTiverATarefaEditadaPeloColaborador( cronograma, tarefa, colaborador );
        }

        [When( @"o cronograma '([\w\s]+)' tiver uma tarefa '([\w\s]+)' criada pelo colaborador '([\w\s]+)' na posicao '([0-9]+)'" )]
        public void QuandoOCronogramaTiverUmaTarefaCriadaPeloColaboradorNaPosicao( string cronograma, string tarefa, string colaborador, int posicao )
        {
            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto,
                "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            string responsaveis = StepColaborador.ColaboradoresDic[colaborador].NomeCompleto;

            short nbIDReferencia = CronogramaTarefasDic[cronograma].Where( o => o.Value.NbID == (Int16)posicao ).Select( o => o.Value.NbID ).FirstOrDefault();

            CronogramaTarefa novaTarefa;

            if(!( nbIDReferencia <= 0 ))
                novaTarefa = CronogramaBo.CriarTarefa( CronogramasDic[cronograma].Oid, tarefa, situacaoPlanejamento.Oid.ToString(), DateTime.Now, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, "", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, nbIDReferencia );
            else
                novaTarefa = CronogramaBo.CriarTarefa( CronogramasDic[cronograma].Oid, tarefa, situacaoPlanejamento.Oid.ToString(), DateTime.Now, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, "", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );

            CronogramaTarefa cronoTarefa = new CronogramaTarefa();
            Guid oidCronogramaParaPesquisar = CronogramasDic[cronograma].Oid;
            cronoTarefa = contexto.CronogramaTarefa.FirstOrDefault( o => o.OidCronograma == oidCronogramaParaPesquisar && o.Tarefa.TxDescricao == tarefa );

            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma ).RnComunicarNovaTarefaCriada( cronoTarefa.Oid.ToString(), CronogramasDic[cronograma].Oid.ToString(), tarefasImpactadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ), dataHoraAcao );

            if(CronogramaTarefasDic.ContainsKey( cronograma ))
            {
                CronogramaTarefasDic[cronograma].Add( tarefa, cronoTarefa );
            }
            else
            {
                CronogramaTarefasDic.Add( cronograma, new Dictionary<string, CronogramaTarefa>() );
                CronogramaTarefasDic[cronograma].Add( tarefa, cronoTarefa );
            }
        }


        [When( @"o cronograma '([\w\s]+)' tiver uma tarefa '([\w\s]+)' criada pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverUmaTarefaCriadaPeloColaborador( string cronograma, string tarefa, string colaborador )
        {
            List<CronogramaTarefa> tarefasImpactadas;
            DateTime dataHoraAcao = new DateTime();
            //cria situação planejamento
            SituacaoPlanejamento situacaoPlanejamento = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            string responsaveis = StepColaborador.ColaboradoresDic[colaborador].NomeCompleto;
            List<CronogramaTarefa> lstCronoTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( CronogramasDic[cronograma].Oid );

            CronogramaTarefa novaTarefa;

            if(lstCronoTarefas != null && lstCronoTarefas.Count > 0)

                novaTarefa = CronogramaBo.CriarTarefa( CronogramasDic[cronograma].Oid, tarefa, situacaoPlanejamento.Oid.ToString(), DateTime.Now, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, "", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, lstCronoTarefas.Last().NbID );
            else
                novaTarefa = CronogramaBo.CriarTarefa( CronogramasDic[cronograma].Oid, tarefa, situacaoPlanejamento.Oid.ToString(), DateTime.Now, StepColaborador.ColaboradoresDic[colaborador].Usuario.UserName, "", responsaveis, out tarefasImpactadas, ref dataHoraAcao, 3, 0 );



            CronogramaTarefa cronoTarefa = new CronogramaTarefa();
            Guid oidCronogramaParaPesquisar = CronogramasDic[cronograma].Oid;
            cronoTarefa = contexto.CronogramaTarefa.FirstOrDefault( o => o.OidCronograma == oidCronogramaParaPesquisar && o.Tarefa.TxDescricao == tarefa );

            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma ).RnComunicarNovaTarefaCriada( cronoTarefa.Oid.ToString(), CronogramasDic[cronograma].Oid.ToString(), tarefasImpactadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ), dataHoraAcao );

            if(CronogramaTarefasDic.ContainsKey( cronograma ))
            {
                CronogramaTarefasDic[cronograma].Add( tarefa, cronoTarefa );
            }
            else
            {
                CronogramaTarefasDic.Add( cronograma, new Dictionary<string, CronogramaTarefa>() );
                CronogramaTarefasDic[cronograma].Add( tarefa, cronoTarefa );
            }
        }

        [When( @"o cronograma '([\w\s]+)' tiver o\(s\) colaborador\(es\) (('[A-Za-z\s]+',?[\s]*)+) desconectado\(s\)" )]
        public void QuandoOCronogramaTiverOSColaboradorEsDesconectadoS( string cronogramaDescricao, string colaboradores, string naoUsado )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidCronograma = GetOidCronograma( cronogramaDescricao );
            string login;
            for(int i = 0; i < logins.Length; i++)
            {
                login = logins[i];
                StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ).RnDesconectar();
            }
        }

        [When( @"o cronograma '([\w\s]+)' tiver a tarefa '([\w\s]+)' em edicao pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverATarefaEmEdicaoPeloColaborador( string cronograma, string tarefa, string login )
        {
            string oidTarefa = GetOidTarefaNoCronograma( cronograma, tarefa );
            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ).RnComunicarInicioEdicaoTarefa( oidTarefa, "" );
        }

        [When( @"o cronograma '([\w\s]+)' tiver a tarefa '([\w\s]+)' editada pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverATarefaEditadaPeloColaborador( string cronograma, string tarefa, string login )
        {
            string oidTarefa = GetOidTarefaNoCronograma( cronograma, tarefa );
            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ).RnComunicarFimEdicaoTarefa( oidTarefa );
        }

        [When( @"o cronograma '([\w\s]+)' tiver a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) solicitada\(s\) para exclusao pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverASTarefaSSolicitadaSParaExclusaoPeloColaborador( string cronograma, string tarefas, string naoUsado, string login )
        {
            string[] oidTarefas = tarefas.Replace( "\'", "" ).Split( ',' ).Select( o => CronogramaTarefasDic[cronograma][o.Trim()].Oid.ToString() ).ToArray();
            string oidCronograma = GetOidCronograma( cronograma );
            StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ).RnComunicarInicioExclusaoTarefa( oidTarefas );
        }

        [When( @"o cronograma '([\w\s]+)' tiver a\(s\) tarefa\(s\) (('[\w\s]+',?[\s]*)+) excluida\(s\) pelo\(s\) colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverASTarefaSExcluidaSPeloSColaborador( string cronograma, string tarefasString, string naoUsado1, string colaborador )
        {
            DateTime dataHoraAcao = new DateTime();

            List<string> tarefas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> oidTarefas = new List<string>();
            List<Guid> oidTarefasExcluidas = new List<Guid>();

            foreach(string tarefa in tarefas)
            {
                oidTarefas.Add( CronogramaTarefasDic[cronograma][tarefa].Oid.ToString() );
                oidTarefasExcluidas.Add( CronogramaTarefasDic[cronograma][tarefa].Oid );
            }

            List<CronogramaTarefa> tarefasImpactadasPelaExclusao = new List<CronogramaTarefa>();
            List<Guid> tarefasNaoExcluidas = new List<Guid>();

            List<CronogramaTarefa> tarefasExcluidas = CronogramaTarefaBo.ExcluirCronogramaTarefas( oidTarefasExcluidas, CronogramasDic[cronograma].Oid, ref tarefasImpactadasPelaExclusao, ref tarefasNaoExcluidas, ref dataHoraAcao );

            Dictionary<string, Int16> tarefasImpactadas = tarefasImpactadasPelaExclusao.ToDictionary( o => o.Oid.ToString(), o => o.NbID );

            //Recebendo o cliente
            GetAccessClient( cronograma, colaborador ).RnComunicarFimExclusaoTarefaConcluida( oidTarefas.ToArray(), tarefasImpactadas, new string[] { }, DateUtil.CurrentDateTime );
        }

        [When( @"o cronograma '([\w\s]+)' tiver a tarefa '([\w\s]+)' movida para a posicao '([0-9]+)' pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverATarefaMovidaParaAPosicaoPeloColaborador( string cronograma, string tarefa, string posicao, string colaborador )
        {
            DateTime dataHoraAcao = new DateTime();
            Int16 posicaoFinal = Convert.ToInt16( posicao );
            Int16 posicaoInicial;
            WexMultiAccessClientMock cliente = GetAccessClient( cronograma, colaborador );
            CronogramaTarefa cronogramaTarefa = CronogramaTarefasDic[cronograma][tarefa];
            posicaoInicial = cronogramaTarefa.NbID;
            List<CronogramaTarefa> tarefasImpactadas = CronogramaTarefaDao.ConsultarTarefasImpactadas( CronogramasDic[cronograma].Oid, cronogramaTarefa.NbID, posicaoFinal );
            CronogramaTarefa tarefaDestino = CronogramaTarefasDic[cronograma].Values.Where( o => o.NbID == posicaoFinal ).FirstOrDefault();
            short nbIDAtualizadoTarefaMovida = 0;
            Guid oidCronograma = new Guid();
            List<CronogramaTarefa> tarefasReordenadas = CronogramaTarefaBo.ReordenarTarefas( cronogramaTarefa.Oid, tarefaDestino.NbID, ref nbIDAtualizadoTarefaMovida, ref dataHoraAcao, ref oidCronograma );
            cliente.RnComunicarMovimentacaoTarefa( posicaoInicial, posicaoFinal, cronogramaTarefa.Oid.ToString(), tarefasReordenadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ), dataHoraAcao );
        }

        [When( @"o cronograma '([\w\s]+)' tiver o\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) desconectado\(s\) inesperadamente" )]
        public void QuandoOCronogramaTiverOSColaboradorEsDesconectadoSInesperadamente( string cronograma, string colaboradores, string naoUsado )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            foreach(string colaborador in logins)
            {
                GetAccessClient( cronograma, colaborador ).RnDesconectar();
            }
        }

        [When( @"o cronograma '([\w\s]+)' tiver o nome alterado para '([\w\s]+)' pelo colaborador '([\w\s]+)'" )]
        public void QuandoOCronogramaTiverONomeAlteradoParaPeloColaborador( string nomeCronograma, string novoNomeCronograma, string colaborador )
        {
			GetAccessClient( nomeCronograma , colaborador ).RnComunicarAlteracaoDadosCronograma();
        }

        [When( @"o colaborador '([\w\s]+)' solicitar do servidor permissao para alterar nome do cronograma '([\w\s]+)'" )]
        public void QuandoOColaboradorSolicitarDoServidorPermissaoParaAlterarNomeDoCronograma( string login, string cronograma )
        {
            GetAccessClient( cronograma, login ).RnComunicarInicioEdicaoDadosCronograma();
        }

        [When( @"o cronograma '([\w\s]+)' tiver uma tarefa criada pelo colaborador '([\w\s]+)' conforme a seguir:" )]
        public void QuandoOCronogramaTiverUmaTarefaCriadaPeloColaboradorConformeASeguir( string cronograma, string login, Table table )
        {
            Cronograma cronogramaEntity = CronogramaDao.ConsultarCronogramaPorNome( contexto, cronograma );
            Colaborador colaboradorEntity = ColaboradorDAO.ConsultarColaborador( login );

            List<CronogramaTarefa> tarefasReordenadas = new List<CronogramaTarefa>();
            DateTime dataHoraDaAcao = new DateTime();

            for(int i = 0; i < table.RowCount; i++)
            {
                CsTipoPlanejamento tipoPlanejamento = SituacaoPlanejamentoBddHelper.ConverterTipoPlanejamentoStringParaTipoPlanejamentoDomain( table.Rows[i][table.Header.ToList()[1]] );
                SituacaoPlanejamento situacaoPlanejamentoEntity = SituacaoPlanejamentoDAO.ConsultarSituacaoPorTipo( tipoPlanejamento );

                string txDescricaoTarefa = table.Rows[i][table.Header.ToList()[0]];
                Int16 EstimativaInicialHora = Convert.ToInt16( table.Rows[i][table.Header.ToList()[2]] );

                CronogramaTarefa cronogramaTarefaCriada = CronogramaBo.CriarTarefa( cronogramaEntity.Oid, txDescricaoTarefa, situacaoPlanejamentoEntity.Oid.ToString(), DateTime.Today, colaboradorEntity.Usuario.UserName, String.Empty, String.Empty, out tarefasReordenadas, ref dataHoraDaAcao, EstimativaInicialHora );

                StepContextUtil.GetAccessClientNoContexto( login, cronogramaEntity.Oid.ToString() ).RnComunicarNovaTarefaCriada( cronogramaTarefaCriada.Oid.ToString(), cronogramaEntity.Oid.ToString(), tarefasReordenadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ), dataHoraDaAcao );
            }
        }

        [Then( @"o grafico de burndown deve conter os seguintes valores para o cronograma '(.*)':" )]
        public void EntaoOGraficoDeBurndownDeveConterOsSeguintesValoresParaOCronograma( string nomeCronograma, Table table )
        {
            var dtoEsperado = PreencherDadosEsperados( table );
            StepGraficoBurndown.ArmazenarDadosBurndownContexto( nomeCronograma, dtoEsperado );
            var dtoAtual = CarregarDadosBurndownDoContexto( nomeCronograma, true );

            var es = dtoEsperado.DadosBurndown
                .ToLookup( o => o.CsTipo )
                .ToDictionary( o => o.Key, o => o.ToList() ).Dump("Esperado");

            var at = dtoAtual.DadosBurndown
                .ToLookup( o => o.CsTipo )
                .ToDictionary( o => o.Key, o => o.ToList() ).Dump("Atual");


            Assert.IsNotNull( dtoAtual, "Deveria ter retornado ao menos uma lista vazia" );

            foreach(var esperado in dtoEsperado.DadosBurndown)
            {
                Assert.IsTrue( dtoAtual.DadosBurndown.Any( o => CompararDadosGrafico( esperado, o ) ), "Deveria conter o dto com os resultados esperados." );
            }
        }

        [Then( @"o cronograma '(.*)' deve ter um desvio de (.*) horas" )]
        public void EntaoOCronogramaDeveTerUmDesvioDeHoras( string nomeCronograma, double desvio )
        {
            var esperado = CarregarDadosBurndownDoContexto( nomeCronograma, false );
            var desvioCalculado = GraficoBurndownBO.CalcularDesvio( esperado.DadosBurndown );
            Assert.AreEqual( desvio, desvioCalculado, string.Format( "Deveria haver um desvio de {0}, desvio calculado:{1}", desvio, desvioCalculado ) );
        }

        public bool CompararDadosGrafico( BurndownDadosDto esperado, BurndownDadosDto dadosAtuais )
        {
            return dadosAtuais != null && esperado.Dia.Date.Equals( dadosAtuais.Dia.Date ) && esperado.QtdeHoras.GetValueOrDefault().Equals( dadosAtuais.QtdeHoras.GetValueOrDefault() ) && esperado.CsTipo.Equals( dadosAtuais.CsTipo );
        }

        private static BurndownGraficoDto CarregarDadosBurndownDoContexto( string nomeCronograma, bool retorno = false )
        {
            string prefixo = retorno ? "DadosRetorno" : "DadosEsperados";
            return ScenarioContext.Current.Get<BurndownGraficoDto>( prefixo + nomeCronograma );
        }

        private static BurndownGraficoDto PreencherDadosEsperados( Table table )
        {
            var burndownDtoEsperados = new List<BurndownDadosDto>();

            foreach(var row in table.CreateSet<BurndownBindHelper>())
            {
                if(row.Planejado.HasValue)
                {
                    var dto = new BurndownDadosDto()
                            {
                                Dia = row.Data,
                                CsTipo = CsTipoBurndown.Planejado,
                                QtdeHoras = row.Planejado
                            };

                    burndownDtoEsperados.Add( dto );
                }

                if(!row.Restante.HasValue)
                    continue;
                burndownDtoEsperados.Add( new BurndownDadosDto()
                {
                    Dia = row.Data,
                    CsTipo = CsTipoBurndown.Realizado,
                    QtdeHoras = row.Restante
                } );
            }

            return new BurndownGraficoDto { DadosBurndown = burndownDtoEsperados };
        }

        #endregion

        #region Then

        [Then( @"o cronograma '([\w\s]+)' tera a linha de base da tarefa '([\w\s]+)' salva" )]
        public void EntaoOCronogramaTeraALinhaDeBaseDaTarefaSalva( string cronograma, string tarefa )
        {
            Tarefa tarefaHistoricoSalvo = CronogramaTarefasDic[cronograma][tarefa].Tarefa;

            Assert.AreEqual( true, tarefaHistoricoSalvo.CsLinhaBaseSalva, "A linha de base da tarefa deveria estar salva, pois um histórico foi salvo com estimativa inicial maior que 0." );
        }

        [Then( @"o cronograma '([\w\s]+)' devera ter as tarefas visualizadas pelo colaborador '([\w\s]+)' na seguinte ordem:" )]
        public void EntaoOCronogramaDeveraTerAsTarefasVisualizadasPeloColaboradorNaSeguinteOrdem( string cronograma, string login, Table table )
        {
            List<CronogramaTarefa> tarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( CronogramasDic[cronograma].Oid );

            Assert.AreEqual( table.RowCount, tarefas.Count, "Deveriam existir a mesma quantidade existente na tabela informada" );

            for(int i = 0; i < tarefas.Count; i++)
                Assert.AreEqual( short.Parse( table.Rows[i]["id"] ), tarefas[i].NbID, "Deveria ser o mesmo NbID" );
        }

        [Then( @"o cronograma '([\w\s]+)' ser comunicado de que nao houve sucesso na conexao com o servidor para o colaborador '([\w\s]+)'" )]
        public static void EntaoOCronogramaC1SerComunicadoDeQueNaoHouveSucessoNaConexaoComOServidorParaOColaboradorJoao( string cronograma, string login )
        {
            string oidCronograma = CronogramasDic[cronograma].Oid.ToString();
            using(WexMultiAccessClientMock cliente = StepContextUtil.GetAccessClientNoContexto( login, oidCronograma ))
            {
                Assert.IsFalse( cliente.Conectado, "Não deveria estar conectado" );
                Assert.IsFalse( cliente.Autenticado, "Não deveria estar autenticado" );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' do\(s\) colaborador\(es\) (('[\w\s]+',?[\s]*)+) deve\(m\) ser comunicado\(s\) que o colaborador '([\w\s]+)' ficou online" )]
        public void ThenOCronogramaC1DoSColaboradorEsMariaJoseDeveMSerComunicadoSQueOColaboradorJoaoFicouOnline( string cronograma, string colaboradores, string naoUsado, string login )
        {
            string oidCronograma = GetOidCronograma( cronograma );
            string[] usuarios = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            WexMultiAccessClientMock clientAtual;
            List<MensagemDto> mensagens;
            bool foiComunicado;
            string[] usuariosMensagem;
            foreach(string colaborador in usuarios)
            {
                foiComunicado = false;
                clientAtual = StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma );
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    mensagens = new List<MensagemDto>( clientAtual.MensagensRecebidas );
                    mensagens = mensagens.Where( o => o.Tipo == CsTipoMensagem.NovosUsuariosConectados ).ToList();
                    if(mensagens != null)
                        for(int i = 0; i < mensagens.Count; i++)
                        {
                            usuariosMensagem = (string[])mensagens[i].Propriedades[Constantes.USUARIOS];
                            if(usuariosMensagem.Contains( login ))
                            {
                                foiComunicado = true;
                                break;
                            }
                        }
                    return foiComunicado;
                } );
                Assert.IsTrue( foiComunicado, string.Format( "{0} no Cronograma {1} deveria ter sido comunicado que {2} se conectou.", colaborador, cronograma, login ) );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' do\(s\) colaborador\(es\) (('[A-Za-z\sçãáéíóú]+',?[\s]*)+) deve\(m\) ser comunicado\(s\) que o colaborador '([\w\s]+)' se desconectou" )]
        public void EntaoOCronogramaDoSColaboradorEsDeveMSerComunicadoSQueOColaboradorSeDesconectou( string cronograma, string colaboradores, string naoUsado, string login )
        {
            string oidCronograma = GetOidCronograma( cronograma );
            string[] usuarios = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            WexMultiAccessClientMock clientAtual;
            List<MensagemDto> mensagens;
            bool foiComunicado;
            string[] usuariosMensagem;
            foreach(string colaborador in usuarios)
            {
                foiComunicado = false;
                clientAtual = StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronograma );
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    mensagens = new List<MensagemDto>( clientAtual.MensagensRecebidas );
                    mensagens = mensagens.Where( o => o.Tipo == CsTipoMensagem.UsuarioDesconectado ).ToList();
                    if(mensagens != null)
                        for(int i = 0; i < mensagens.Count; i++)
                        {
                            usuariosMensagem = (string[])mensagens[i].Propriedades[Constantes.USUARIOS];
                            if(usuariosMensagem.Contains( login ))
                            {
                                foiComunicado = true;
                                break;
                            }
                        }
                    return foiComunicado;
                } );
                Assert.IsTrue( foiComunicado, string.Format( "{0} no Cronograma {1} deveria ter sido comunicado que {2} se desconectou.", colaborador, cronograma, login ) );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' do\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) nao deve\(m\) ser comunicado\(s\) que o colaborador '([\w\s]+)' se desconectou" )]
        public void EntaoOCronogramaDoSColaboradorEsNaoDeveMSerComunicadoSQueOColaboradorSeDesconectou( string cronograma, string logins, string naoUsado, string loginDesconectado )
        {
            string[] loginsComunicados = logins.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidCronograma = GetOidCronograma( cronograma );
            string key;
            for(int i = 0; i < loginsComunicados.Length; i++)
            {
                key = StepContextUtil.CriarKeyEventoUsuarioDesconectado( loginsComunicados[i], oidCronograma, loginDesconectado );
                Assert.IsFalse( ScenarioContext.Current.ContainsKey( key ), "O colaborador {0} no cronograma {1} não deveria ser avisado de que {2} se desconectou", loginsComunicados[i], cronograma, loginDesconectado );
            }
        }
        [Then( @"o cronograma '([\w\s]+)' deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a tarefa '([\w\s]+)' foi criada com o ID '([0-9]+)'" )]
        public void EntaoOCronogramaDeveComunicarAoSColaboradorEsDeQueATarefaFoiCriadaComOID( string cronograma, string colaboradores, string naoUsado, string tarefa, int idTarefa )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            List<string> colaboradoresQueReceberamMensagem = new List<string>();
            WexMultiAccessClientMock clienteAtual;
            List<MensagemDto> mensagensAtuais;
            MensagemDto mensagemEsperada = new MensagemDto();
            string oidTarefaAtual = CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
            string oidCronogramaAtual = CronogramasDic[cronograma].Oid.ToString();
            foreach(string colaborador in logins)
            {
                clienteAtual = StepContextUtil.GetAccessClientNoContexto( colaborador, oidCronogramaAtual );

                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    mensagensAtuais = new List<MensagemDto>( clienteAtual.MensagensRecebidas );

                    mensagemEsperada = mensagensAtuais.Where( o => o.Tipo == CsTipoMensagem.NovaTarefaCriada && o.Propriedades.ContainsValue( oidTarefaAtual ) ).FirstOrDefault();
                    return mensagemEsperada != null && mensagemEsperada.Propriedades.ContainsValue( oidTarefaAtual );

                }, 10 );

                Assert.IsNotNull( mensagemEsperada, string.Format( "{0} deveria ter recebido a mensagem de criação da tarefa {1} " +
                    "no cronograma {2}", colaborador, tarefa, cronograma ) );
                Int16 ID = CronogramaTarefasDic[cronograma][tarefa].NbID;
                Assert.AreEqual( idTarefa, ID, string.Format( "Os Id estão diferentes {0} != {1}", idTarefa, ID ) );
                Assert.IsTrue( mensagemEsperada.Propriedades.ContainsValue( oidTarefaAtual ), "A mensagem recebida deveria conter a tarefa atual" );
                Assert.AreEqual( oidCronogramaAtual, (string)mensagemEsperada.Propriedades[Constantes.OIDCRONOGRAMA],
                    string.Format( "A tarefa criada deveria pertencer ao cronograma {0}", cronograma ) );
                colaboradoresQueReceberamMensagem.Add( colaborador );
            }

            CollectionAssert.AreEquivalent( logins, colaboradoresQueReceberamMensagem, "Todos os colaboradores deveriam ter sido avisados sobre a criação da nova tarefa" );
        }

        [Then( @"o cronograma '([\w\s]+)' do\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) deve\(m\) ser comunicado\(s\) que o servidor desconectou" )]
        public void EntaoOCronogramaDoSColaboradorEsDeveMSerComunicadoSQueOServidorDesconectou( string cronograma, string colaboradores, string naoUsado )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidCronograma = GetOidCronograma( cronograma );
            string key;
            foreach(string login in logins)
            {
                key = StepContextUtil.CriarKeyEventoServidorDesconectado( login, oidCronograma );
                ControleDeEsperaUtil.AguardarAte( () => { return ScenarioContext.Current.ContainsKey( key ); } );
                Assert.IsTrue( ScenarioContext.Current.ContainsKey( key ), "Deveria ter sido comunicado de que o servidor foi desconectado" );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' nao deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que no cronograma '([\w\s]+)' foi criada a tarefa '([\w\s]+)'" )]
        public void EntaoOCronogramaNaoDeveComunicarAoSColaboradorEsDeQueNoCronogramaFoiCriadaATarefa( string cronogramaAtual, string colaboradores, string naoUsado, string outroCronograma, string tarefaCriada )
        {
            string oidCronograma = GetOidCronograma( cronogramaAtual );
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidNovaTarefa = GetOidTarefaNoCronograma( outroCronograma, tarefaCriada );
            string key;
            foreach(string login in logins)
            {
                key = StepContextUtil.CriarKeyEventoAoSerCriadaNovaTarefa( login, oidCronograma, oidNovaTarefa );
                ControleDeEsperaUtil.AguardarAte( () => { return ScenarioContext.Current.ContainsKey( key ); } );
                Assert.IsFalse( ScenarioContext.Current.ContainsKey( key ), string.Format( "{0} conectado no cronograma '{2}' não deveria ter sido comunicado da criação da tarefa '{1} que pertence ao cronograma {3}'", login, tarefaCriada, cronogramaAtual, outroCronograma ) );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a tarefa '([\w\s]+)' esta sendo editada pelo colaborador '([\w\s]+)'" )]
        public void EntaoOCronogramaDeveComunicarAoSColaboradorEsDeQueATarefaEstaSendoEditadaPeloColaborador( string cronograma, string colaboradores, string naoUsado, string tarefa, string autorAcao )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidCronograma = GetOidCronograma( cronograma );
            string key;
            foreach(string login in logins)
            {
                key = StepContextUtil.CriarKeyEventoAoIniciarEdicaoTarefa( login, oidCronograma, autorAcao );
                ControleDeEsperaUtil.AguardarAte( () => { return StepContextUtil.CenarioAtualContemAChave( key ); } );
                Assert.IsTrue( StepContextUtil.CenarioAtualContemAChave( key ), string.Format( "O colaborador {0} deveria ter sido comunicado da edição da tarefa {1} no cronograma {2} pelo colaborador {3} ", login, tarefa, cronograma, autorAcao ) );
            }

        }

        [Then( @"o cronograma '([\w\s]+)' deve comunicar ao\(s\) colaborador\(es\) '([\w\s]+)' de que foi recusada a edicao da a tarefa '([\w\s]+)' pois a mesma esta sendo editada pelo colaborador '([\w\s]+)'" )]
        public void EntaoOCronogramaDeveComunicarAoSColaboradorEsDeQueFoiRecusadaAEdicaoDaATarefaPoisAMesmaEstaSendoEditadaPeloColaborador( string cronograma, string login1, string tarefa, string login2 )
        {
            string oidCronograma = GetOidCronograma( cronograma );
            string oidTarefa = GetOidTarefaNoCronograma( cronograma, tarefa );
            WexMultiAccessClientMock client = StepContextUtil.GetAccessClientNoContexto( login1, oidCronograma );
            MensagemDto mensagemEsperada = null;
            List<MensagemDto> mensagensRecebidas;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagensRecebidas = new List<MensagemDto>( client.MensagensRecebidas );
                mensagemEsperada = mensagensRecebidas.FirstOrDefault(
                    o => o.Tipo == CsTipoMensagem.EdicaoTarefaRecusada &&
                    o.Propriedades[Constantes.AUTOR_ACAO].ToString().Equals( login2 ) &&
                    o.Propriedades[Constantes.OIDTAREFA].ToString().Equals( oidTarefa ) );
                return mensagemEsperada != null;
            }, 5 );

            Assert.IsNotNull( mensagemEsperada, "Deveria ter recebido a mensagem esperada" );
            Assert.AreEqual( oidTarefa, mensagemEsperada.Propriedades[Constantes.OIDTAREFA] as string, string.Format( "{0} deveria ter sido avisado que a tarefa {1} estava em edição", login1, tarefa ) );
            Assert.AreEqual( login2, mensagemEsperada.Propriedades[Constantes.AUTOR_ACAO] as string, string.Format( "O usuário que está editando a tarefa deveria ser {0}", login2 ) );
        }

        [Then( @"o cronograma '([\w\s]+)' deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) recebeu\(ram\) atualizacao\(oes\)" )]
        public void EntaoOCronogramaDeveComunicarAoSColaboradorEsDeQueASTarefaSRecebeuRamAtualizacaoOes( string cronograma, string colaboradores, string naoUsada1, string tarefasSplit, string naoUsada2 )
        {
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string[] tarefas = tarefasSplit.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            string oidTarefa;
            string key;
            for(int i = 0; i < logins.Length; i++)
            {
                for(int x = 0; x < tarefas.Length; x++)
                {
                    oidTarefa = GetOidTarefaNoCronograma( cronograma, tarefas[x] );
                    key = StepContextUtil.CriarKeyRecebeuAtualizacaoEdicaoTarefa( logins[i], oidTarefa );
                    ControleDeEsperaUtil.AguardarAte( () => { return StepContextUtil.CenarioAtualContemAChave( key ); } );
                    Assert.IsTrue( StepContextUtil.CenarioAtualContemAChave( key ), string.Format( "{0} deveria ter sido comunicado da atualização da tarefa {1} que pertence ao cronograma '{2}'", logins[i], tarefas[x], cronograma ) );
                }
            }
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar ao colaborador '([\w\s]+)' da exclusao da\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+)" )]
        public void EntaoOCronogramaDeveraComunicarAoSColaboradorEsDaExclusaoDaSTarefaS( string cronograma, string colaborador, string tarefas, string naoUsado )
        {
            int total = 0;
            List<string> descricoesTarefas = tarefas.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            string oidCronograma = GetOidCronograma( cronograma );
            WexMultiAccessClientMock client = GetAccessClient( cronograma, colaborador );
            total = descricoesTarefas.Count;
            List<MensagemDto> mensagensEsperadas;
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagensEsperadas = new List<MensagemDto>( client.MensagensRecebidas );
                return mensagensEsperadas.Count
                    (
                        o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada &&
                            (string)o.Propriedades[Constantes.OIDCRONOGRAMA] == oidCronograma
                    ) == total;
            }, 10 );

            List<string> oidTarefasRecebidas = new List<string>();
            WexMultiAccessClientMock cliente = GetAccessClient( cronograma, colaborador );
            List<MensagemDto> mensagens = new List<MensagemDto>();
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagens = new List<MensagemDto>( cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada && (string)o.Propriedades["login"] != colaborador ) );
                mensagens.ForEach( o => oidTarefasRecebidas = oidTarefasRecebidas.Union( (string[])o.Propriedades[Constantes.TAREFAS] ).ToList() );
                return oidTarefasRecebidas.Count == descricoesTarefas.Count;
            }, 10 );

            Assert.AreEqual( descricoesTarefas.Count, oidTarefasRecebidas.Count, "Deveria ter recebido todas as mensagens esperadas" );

            string[] oidTarefasEsperadas = descricoesTarefas.Select( o => CronogramaTarefasDic[cronograma][o].Oid.ToString() ).ToArray();
            CollectionAssert.AreEquivalent( oidTarefasEsperadas, oidTarefasRecebidas, "Deveria ter recebido a informção de todas tarefas que foram excluidas" );
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar o servidor de que a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) foram excluidas pelo colaborador '([\w\s]+)' e que a\(s\) tarefas foram reorganizadas na seguinte ordem:" )]
        public void EntaoOCronogramaDeveraComunicarOServidorDeQueASTarefaSForamExcluidasPeloColaboradorEQueASTarefasForamReorganizadasNaSeguinteOrdem( string cronograma, string tarefas, string naoUsado, string colaborador, Table table )
        {
            DateUtil.CurrentDateTime = DateTime.Now;
            // string cronograma, string tarefas, string naoUsado, string colaborador 
            List<string> listaTarefas = tarefas.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> oidTarefas = new List<string>();
            foreach(string tarefa in listaTarefas)
            {
                oidTarefas.Add( CronogramaTarefasDic[cronograma][tarefa].Oid.ToString() );
            }
            var ordemEsperada = table.Rows.Select( o => new KeyValuePair<string, Int16>( CronogramaTarefasDic[cronograma][o["descricao"]].Oid.ToString(), Convert.ToInt16( o["id"] ) ) ).ToList();
            Dictionary<string, Int16> tarefasImpactadas = new Dictionary<string, short>();
            foreach(KeyValuePair<string, Int16> item in ordemEsperada)
            {
                tarefasImpactadas.Add( item.Key, item.Value );
            }

            GetAccessClient( cronograma, colaborador ).RnComunicarFimExclusaoTarefaConcluida( oidTarefas.ToArray(), tarefasImpactadas, new string[] { }, DateUtil.CurrentDateTime );
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) foi excluida\(s\) pelo colaborador '([\w\s]+)'" )]
        public void EntaoOCronogramaDeveraComunicarAoSColaboradorEsDeQueASTarefaSFoiExcluidaSPeloColaborador( string cronograma, string colaboradoresString, string naoUsada1, string tarefasString, string naoUsada2, string colaborador )
        {
            List<string> tarefas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            string[] oidTarefasExcluidas = tarefas.Select( o => CronogramaTarefasDic[cronograma][o].Oid.ToString() ).ToArray();
            List<string> colaboradores = colaboradoresString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            Dictionary<string, bool> colaboradoresAvisados = new Dictionary<string, bool>();

            //Preenchendo o Dicionário em que nenhum colaborador foi avisado sobre a exclusão
            colaboradores.ForEach( o => colaboradoresAvisados.Add( o, false ) );
            //recebendo o client de comunicação de cada colaborador
            WexMultiAccessClientMock[] clientes = colaboradores.Select( o => GetAccessClient( cronograma, o ) ).ToArray();


            MensagemDto mensagemEsperada;
            foreach(WexMultiAccessClientMock cliente in clientes)
            {
                mensagemEsperada = null;
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    lock(cliente.MensagensRecebidas)
                    {
                        mensagemEsperada = cliente.MensagensRecebidas.Where
                            (
                                o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada &&
                                o.Propriedades[Constantes.AUTOR_ACAO].ToString() == colaborador
                            ).FirstOrDefault();
                    }

                    return mensagemEsperada != null;

                }, 10 );

                Assert.IsNotNull( mensagemEsperada, string.Format( "{0} deveria ter recebido a mensagem de exclusão de tarefas do colaborador {1}", cliente.Login, colaborador ) );
                CollectionAssert.AreEquivalent( oidTarefasExcluidas, (string[])mensagemEsperada.Propriedades[Constantes.TAREFAS], "Ha uma diferença entre as tarefas excluidas esperadas e as tarefas excluidas recebidas" );
                Assert.IsNull( (string[])mensagemEsperada.Propriedades[Constantes.TAREFAS_NAO_EXCLUIDAS], "Não deveria haver tarefas recusadas" );
                colaboradoresAvisados[cliente.Login] = true;

            }

            string descricaoTarefas = "";
            for(int i = 0; i < tarefas.Count; i++)
            {
                descricaoTarefas += tarefas[i];

                if(i < tarefas.Count - 1)
                    descricaoTarefas += ", ";
            }

            foreach(var avisado in colaboradoresAvisados)
            {
                Assert.IsTrue( avisado.Value, string.Format( "{0} deveria ter sido avisado que {1} excluiu a(s) tarefa(s): {2} no cronograma {3}", avisado.Key, colaborador, descricaoTarefas, cronograma ) );
            }

        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) da exclusao da\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+)" )]
        public void EntaoOCronogramaDeveraComunicarAoSColaboradorEsDaExclusaoDaSTarefaS( string cronograma, string colaboradoresString, string naoUsado1, string tarefasString, string naoUsado2 )
        {
            List<string> colaboradores = colaboradoresString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> tarefasAguardadas = tarefasString.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> oidTarefaEsperadas = tarefasAguardadas.Select( o => CronogramaTarefasDic[cronograma][o].Oid.ToString() ).ToList();
            List<MensagemDto> mensagensRecebidas, mensagensFiltradas;
            List<string> tarefasInformadas;
            string[] tarefasExcluidas;
            WexMultiAccessClientMock[] clientes = colaboradores.Select( login => GetAccessClient( cronograma, login ) ).ToArray();
            foreach(WexMultiAccessClientMock cliente in clientes)
            {
                mensagensRecebidas = null;
                mensagensFiltradas = null;
                tarefasInformadas = new List<string>();

                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    lock(cliente.MensagensRecebidas)
                    {
                        mensagensFiltradas = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.ExclusaoTarefaFinalizada ).ToList();
                    }

                    foreach(MensagemDto mensagem in mensagensFiltradas)
                    {
                        tarefasExcluidas = (string[])mensagem.Propriedades[Constantes.TAREFAS];
                        tarefasInformadas = tarefasInformadas.Union( tarefasExcluidas ).ToList();
                    }

                    return tarefasInformadas.Count == tarefasAguardadas.Count;
                } );

                CollectionAssert.AreEquivalent( oidTarefaEsperadas, tarefasInformadas, "Deveria ter sido informado da exclusão das tarefas conforme o esperado" );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' deve comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) (('[A-Za-z\d\s]+',?[\s]*)+) nao podera\(o\) ser excluida\(s\)" )]
        public void EntaoOCronogramaDeveComunicarAoSColaboradorEsDeQueASTarefaSNaoPoderaOSerExcluidaS( string cronograma, string colaboradores, string naoUsado1, string tarefas, string naoUsado2 )
        {
            List<string> listaColaboradores = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            List<string> listaTarefas = tarefas.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToList();
            Dictionary<string, string> TarefasDic = new Dictionary<string, string>();
            List<string> oidTarefas = new List<string>();
            string oidTarefa;
            foreach(string tarefa in listaTarefas)
            {
                oidTarefa = CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
                TarefasDic.Add( oidTarefa, tarefa );
                oidTarefas.Add( oidTarefa );
            }

            List<MensagemDto> mensagensTemporarias;
            string[] listaTarefasDaMensagemDto;
            bool PermitidoExcluir;
            foreach(string colaborador in listaColaboradores)
            {
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    mensagensTemporarias = new List<MensagemDto>( GetAccessClient( cronograma, colaborador ).MensagensRecebidas );
                    return mensagensTemporarias.Count( o => o.Tipo == CsTipoMensagem.ExclusaoTarefaPermitida ) == 1;
                }, 5 );

                PermitidoExcluir = false;
                List<MensagemDto> mensagens = GetAccessClient( cronograma, colaborador ).MensagensRecebidas.Where( o => o.Tipo ==
                CsTipoMensagem.ExclusaoTarefaPermitida ).ToList();
                foreach(MensagemDto mensagem in mensagens)
                {
                    listaTarefasDaMensagemDto = (string[])mensagem.Propriedades[Constantes.TAREFAS];
                    if(listaTarefasDaMensagemDto.Length == 0)
                    {
                        PermitidoExcluir = false;
                        Assert.IsFalse( PermitidoExcluir, string.Format( "Não teve Nenhuma Tarefa Permitida", colaborador ) );
                    }
                    else
                    {
                        foreach(string oidTarefaAtual in oidTarefas)
                        {
                            if(listaTarefasDaMensagemDto.Contains( oidTarefaAtual ))
                            {
                                PermitidoExcluir = true;
                            }
                            Assert.IsFalse( PermitidoExcluir, string.Format( "{0} não deveria poder excluir a tarefa {1} pois a mesma já se encontra em edição", colaborador, TarefasDic[oidTarefaAtual] ) );
                        }
                    }
                }
            }
        }

        [Then( @"o cronograma '([\w\s]+)' deve comunicar o colaborador '([\w\s]+)' de que a tarefa '([\w\s]+)' foi permitida para edicao" )]
        public void EntaoOCronogramaDeveComunicarOColaboradorDeQueATarefaFoiPermitidaParaEdicao( string cronograma, string colaborador, string tarefa )
        {
            WexMultiAccessClientMock cliente = StepContextUtil.GetAccessClientNoContexto( colaborador, GetOidCronograma( cronograma ) );
            List<MensagemDto> mensagensRecebidas = null, mensagensFiltradas = null;
            MensagemDto mensagemRecebida = null;

            string oidTarefa = CronogramaTarefasDic[cronograma][tarefa].Oid.ToString();
            string oidCronograma = CronogramasDic[cronograma].Oid.ToString();
            ControleDeEsperaUtil.AguardarAte( () =>
            {
                mensagensRecebidas = new List<MensagemDto>( cliente.MensagensRecebidas );
                mensagensFiltradas = mensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.EdicaoTarefaAutorizada ).ToList();
                mensagemRecebida = mensagensFiltradas.FirstOrDefault
                    ( o =>
                        (string)o.Propriedades[Constantes.OIDCRONOGRAMA] == oidCronograma &&
                        (string)o.Propriedades[Constantes.OIDTAREFA] == oidTarefa
                    );
                return mensagemRecebida != null;
            } );

            Assert.IsNotNull( mensagemRecebida, string.Format( "Deveria ter sido filtrada e recebida a mensagem de autorização da tarefa {0}", oidTarefa ) );
            Assert.AreEqual( oidCronograma, (string)mensagemRecebida.Propriedades[Constantes.OIDCRONOGRAMA],
                string.Format( "A tarefa comunicada deveria pertencer ao cronograma {0}", oidCronograma ) );
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) sofreram movimentacao conforme a seguir:" )]
        public void EntaoOCronogramaDeveraComunicarAoSColaboradorEsDeQueASTarefaSSofreramMovimentacaoConformeASeguir( string cronograma, string colaboradores, string naoUsado, Table table )
        {
            //filtrando os logins vindos do parametro, tratando e armazenando em um vetor
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            List<WexMultiAccessClientMock> clientes = new List<WexMultiAccessClientMock>();
            for(int i = 0; i < logins.Length; i++)
            {
                clientes.Add( GetAccessClient( cronograma, logins[i] ) );
            }
            //filtrando a ordem esperada da visualização das tarefas
            var ordemEsperada = table.Rows.Select( o => new KeyValuePair<string, Int16>( CronogramaTarefasDic[cronograma][o["descricao"]].Oid.ToString(), Convert.ToInt16( o["id"] ) ) ).ToList();
            Dictionary<string, Int16> ordemRecebida;
            Int16 posicaoFinal;
            string oidTarefaAtual;
            foreach(WexMultiAccessClientMock cliente in clientes)
            {
                //controle de espera para que seja aguardado o recebimento da mensagem do tipo de movimentação
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    return cliente.contagemTiposMensagemDtoRecebidas.ContainsKey( (int)CsTipoMensagem.MovimentacaoPosicaoTarefa ) &&
                           cliente.contagemTiposMensagemDtoRecebidas[(int)CsTipoMensagem.MovimentacaoPosicaoTarefa] == 1;
                } );

                MensagemDto mensagem = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.MovimentacaoPosicaoTarefa ).FirstOrDefault();
                Assert.IsNotNull( mensagem, "O client deveria ter recebido a mensagem de movimentação" );
                ordemRecebida = (Dictionary<string, Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS];
                posicaoFinal = Convert.ToInt16( mensagem.Propriedades["posicaoFinal"] );
                oidTarefaAtual = (string)mensagem.Propriedades[Constantes.OIDTAREFA];
                ordemRecebida.Add( oidTarefaAtual, posicaoFinal );

                CollectionAssert.AreEquivalent( ordemEsperada, ordemRecebida, "Deveriam possuir a mesma ordem de NbID" );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar ao\(s\) colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) de que a\(s\) tarefa\(s\) sofreram reordenacao conforme a seguir:" )]
        public void EntaoOCronogramaDeveraComunicarAoSColaboradorEsDeQueASTarefaSSofreramReordenacaoConformeASeguir( string cronograma, string colaboradores, string naoUsado, Table table )
        {
            //filtrando os logins vindos do parametro, tratando e armazenando em um vetor
            string[] logins = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => o.Trim() ).ToArray();
            List<WexMultiAccessClientMock> clientes = new List<WexMultiAccessClientMock>();
            for(int i = 0; i < logins.Length; i++)
            {
                clientes.Add( GetAccessClient( cronograma, logins[i] ) );
            }
            string oidCronograma = GetOidCronograma( cronograma );
            //filtrando a ordem esperada da visualização das tarefas
            var ordemEsperada = table.Rows.Select( o => new KeyValuePair<string, Int16>( CronogramaTarefasDic[cronograma][o["descricao"]].Oid.ToString(), Convert.ToInt16( o["id"] ) ) ).ToList();
            Dictionary<string, Int16> ordemRecebida;
            foreach(WexMultiAccessClientMock cliente in clientes)
            {
                MensagemDto mensagem = null;
                //controle de espera para que seja aguardado o recebimento da mensagem do tipo de movimentação
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    lock(cliente.MensagensRecebidas)
                    {
                        mensagem = cliente.MensagensRecebidas.FirstOrDefault( o => o.Tipo.Equals( CsTipoMensagem.MovimentacaoPosicaoTarefa ) && o.Propriedades[Constantes.OIDCRONOGRAMA].ToString().Equals( oidCronograma ) );
                    }
                    return mensagem != null;
                } );

                //MensagemDto mensagem = cliente.MensagensRecebidas.Where( o => o.Tipo == CsTipoMensagem.MovimentacaoPosicaoTarefa ).FirstOrDefault();
                Assert.IsNotNull( mensagem, "O client deveria ter recebido a mensagem de movimentação" );
                ordemRecebida = (Dictionary<string, Int16>)mensagem.Propriedades[Constantes.TAREFAS_IMPACTADAS];
                CollectionAssert.AreEquivalent( ordemEsperada, ordemRecebida, "Deveriam possuir a mesma ordem de NbID" );

            }
        }

        [Then( @"o cronograma '([\w\s]+)' devera comunicar os colaborador\(es\) (('[A-Za-z\d\s]+',?[\s]*)+) que o cronograma teve o nome alterado para '([\w\s]+)'" )]
        public void EntaoOCronogramaDeveraComunicarOsColaboradorEsQueOCronogramaTeveONomeAlteradoPara( string nomeCronograma, string colaboradores, string naoUsada, string novoNomeCronograma )
        {
            string oidCronograma = CronogramasDic[nomeCronograma].Oid.ToString();
            WexMultiAccessClientMock[] clientes = colaboradores.Replace( "\'", "" ).Split( ',' ).Select( o => GetAccessClient( nomeCronograma, o.Trim() ) ).ToArray();
            List<MensagemDto> mensagensRecebidas;
            MensagemDto mensagemEsperada;
            foreach(WexMultiAccessClientMock cliente in clientes)
            {
                mensagensRecebidas = null;
                mensagemEsperada = null;
                ControleDeEsperaUtil.AguardarAte( () =>
                {
                    mensagensRecebidas = new List<MensagemDto>( cliente.MensagensRecebidas );

                    mensagemEsperada = mensagensRecebidas.FirstOrDefault( o => o.Tipo == CsTipoMensagem.DadosCronogramaAlterados && o.Propriedades[Constantes.OIDCRONOGRAMA].ToString() == oidCronograma );
                    return false;
                } );
            }
        }

        [Then( @"o cronograma '([\w\s]+)' deve possuir as tarefas a seguir:" )]
        public void EntaoOCronogramaDevePossuirAsTarefasASeguir( string cronograma, Table table )
        {
            Cronograma cronogramaEntity = CronogramaDao.ConsultarCronogramaPorNome( contexto, cronograma );

            List<CronogramaTarefa> cronogramasTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( cronogramaEntity.Oid );

            for(int i = 0; i < table.RowCount; i++)
            {
                short id = Convert.ToInt16( table.Rows[i][table.Header.ToList()[0]] );
                string descricao = table.Rows[i][table.Header.ToList()[1]];
                CsTipoPlanejamento tipoSituacaoPlanejamento = SituacaoPlanejamentoBddHelper.ConverterTipoPlanejamentoStringParaTipoPlanejamentoDomain( table.Rows[i][table.Header.ToList()[2]] );
                TimeSpan EstimativaInicialHora = ConversorTimeSpan.ConverterHoraInteiraParaTimeSpan( Convert.ToInt32( table.Rows[i][table.Header.ToList()[3]] ) );
                DateTime dataInicio = GeralBddHelper.ConverterDataEmStringParaDateTime( table.Rows[i][table.Header.ToList()[4]] );

                CronogramaTarefa cronogramaTarefaEntity = CronogramaTarefaDao.ConsultarCronogramaTarefaPorNbId( cronogramaEntity.Oid, id );

                Assert.AreEqual( id, cronogramaTarefaEntity.NbIdAntigo, "Os nbIds devem ser iguais, pois não houve reordenação e as tarefas foram criadas na ordem" );
                Assert.AreEqual( descricao, cronogramaTarefaEntity.Tarefa.TxDescricao );
                Assert.AreEqual( tipoSituacaoPlanejamento, cronogramaTarefaEntity.Tarefa.SituacaoPlanejamento.CsTipo );
                Assert.AreEqual( EstimativaInicialHora, cronogramaTarefaEntity.Tarefa.EstimativaInicialHora );
                Assert.AreEqual( dataInicio, cronogramaTarefaEntity.Tarefa.DtInicio );
            }
        }


        #endregion
    }
}