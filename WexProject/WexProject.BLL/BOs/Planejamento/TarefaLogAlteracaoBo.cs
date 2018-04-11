using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Concorrencia;
using WexProject.Library.Libs.DataHora;

namespace WexProject.BLL.BOs.Planejamento
{
    public class TarefaLogAlteracaoBo
    {
        /// <summary>
        /// Indica se é para habilitar ou não o botão de Histórico de Atualização
        /// </summary>
        /// <param name="selecionadas">Tarefas selecionadas</param>
        /// <returns>Se é para habilitar ou não o botão de Histórico de Atualização</returns>
        public static bool IsHabilitarHistoricoAtualizacao( int selecionadas, Tarefa tarefa )
        {
            return selecionadas == 1 && tarefa != null && tarefa.LogsAlteracao.Count > 0;
        }

        /// <summary>
        /// Criar Log para uma Tarefa específica
        /// </summary>
        /// <param name="tarefaAtual">Objeto de Tarefa</param>
        /// <param name="tarefaAntiga">Objeto de Tarefa antes das alterações</param>
        public static void CriarLogTarefa( Tarefa tarefaAtual, Tarefa tarefaAntiga )
        {
            if(tarefaAtual == null)
                return;

            string valorNovo, valorAntigo;
            StringBuilder alteracoes = new StringBuilder();

            if(tarefaAntiga == null)
            {
                alteracoes.Append( "Criação da tarefa\n" );
            }
            else
            {
                if(tarefaAtual.TxDescricao != tarefaAntiga.TxDescricao)
                    alteracoes.Append( String.Format( "Descrição alterada de ' {0} ' para ' {1} '\n", tarefaAntiga.TxDescricao, tarefaAtual.TxDescricao ) );

                if(tarefaAtual.SituacaoPlanejamento != tarefaAntiga.SituacaoPlanejamento)
                {
                    valorNovo = tarefaAtual.SituacaoPlanejamento != null ? tarefaAtual.SituacaoPlanejamento.TxDescricao : string.Empty;
                    valorAntigo = tarefaAntiga.SituacaoPlanejamento != null ? tarefaAntiga.SituacaoPlanejamento.TxDescricao : string.Empty;

                    alteracoes.Append( String.Format( "Situação alterada de ' {0} ' para ' {1} '\n", valorAntigo, valorNovo ) );
                }

                if(tarefaAtual.TxResponsaveis != tarefaAntiga.TxResponsaveis)
                    alteracoes.Append( String.Format( "Responsável alterado de ' {0} ' para ' {1} '\n",
                        tarefaAntiga.TxResponsaveis, tarefaAtual.TxResponsaveis ) );

                if(tarefaAtual.NbEstimativaRestante != tarefaAntiga.NbEstimativaRestante)
                    alteracoes.Append( String.Format( "Estimativa Restante alterada de ' {0} ' para ' {1} '\n",
                                       ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAntiga.EstimativaRestanteHora ),
                                        ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAtual.EstimativaRestanteHora ) ) );

                if(tarefaAtual.NbEstimativaInicial != tarefaAntiga.NbEstimativaInicial)
                    alteracoes.Append( String.Format( "Estimativa Inicial alterada de ' {0} ' para ' {1} '\n",
                        ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAntiga.EstimativaInicialHora ),
                        ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAtual.EstimativaInicialHora ) ) );

                if(tarefaAtual.NbRealizado != tarefaAntiga.NbRealizado)
                    alteracoes.Append( String.Format( "Horas Realizadas alteradas de ' {0} ' para ' {1} '\n",
                        ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAntiga.EstimativaRealizadoHora ),
                        ConversorTimeSpan.ConverterTimeSpanParaString( tarefaAtual.EstimativaRealizadoHora ) ) );

                if(tarefaAtual.TxObservacao != tarefaAntiga.TxObservacao)
                    alteracoes.Append( String.Format( "Observação alterada de ' {0} ' para ' {1} '\n",
                        tarefaAntiga.TxObservacao, tarefaAtual.TxObservacao ) );

                if(( (DateTime)tarefaAtual.DtInicio ).Date != ( (DateTime)tarefaAntiga.DtInicio ).Date)
                    alteracoes.Append( String.Format( "Data de Início alterada de ' {0:dd/MM/yyyy} ' para ' {1:dd/MM/yyyy} '\n",
                       tarefaAntiga.DtInicio, tarefaAtual.DtInicio ) );
            }

            if(alteracoes.Length > 0)
            {
                TarefaLogAlteracao logAlteracao = new TarefaLogAlteracao();
                logAlteracao.OidTarefa = tarefaAtual.Oid;
                logAlteracao.DtDataHoraLog = DateUtil.ConsultarDataHoraAtual();
                logAlteracao.OidColaborador = tarefaAtual.OidAtualizadoPor;
                logAlteracao.TxAlteracoes = alteracoes.ToString();

                TarefaLogAlteracaoDao.Salvar( logAlteracao );
            }
        }

		/// <summary>
		/// Pesquisar as alterações efetuadas em uma determinada tarefa baseada no guid da tarefa
		/// </summary>
		/// <param name="session">Sessão atual do banco</param>
		/// <param name="oidTarefa">guid da tarefa selecionada</param>
		/// <returns>Retornar uma lista de TarefaLogAlteracaoDto com as alterações efetuadas em uma determinada tarefa indexada pelo Oid da tarefa</returns>
		public static List<TarefaLogAlteracaoDto> ConsultarAlteracoesTarefaPorOidDto( WexDb contexto, Guid oidTarefa )
		{
			List<TarefaLogAlteracao> alteracoes = TarefaLogAlteracaoDao.ConsultarAlteracoesTarefaPorOid( contexto, oidTarefa );
			List<TarefaLogAlteracaoDto> alteracoesEfetivadas = new List<TarefaLogAlteracaoDto>();

			foreach(var alteracao in alteracoes)
			{
				alteracoesEfetivadas.Add( DtoFactory( alteracao ) );
			}
			return alteracoesEfetivadas;
		}

		#region Factories

		/// <summary>
		/// Criar Dto a partir de uma tarefaLogAlteracao
		/// </summary>
		/// <param name="tarefaLog">log de alteracao</param>
		/// <returns></returns>
		public static TarefaLogAlteracaoDto DtoFactory( TarefaLogAlteracao tarefaLog )
		{
			TarefaLogAlteracaoDto logAlteracao = new TarefaLogAlteracaoDto()
			{
				descricaoColaborador = tarefaLog.Colaborador.NomeCompleto,
				descricaoAlteracao = tarefaLog.TxAlteracoes,
				DtDataEHora = (DateTime)tarefaLog.DtDataHoraLog
			};

			return logAlteracao;
		}

		#endregion
    }
}
