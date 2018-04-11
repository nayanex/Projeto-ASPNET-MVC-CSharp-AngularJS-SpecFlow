using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Comparacao;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Helpers;
using WexProject.Schedule.Library.Properties;

namespace WexProject.Schedule.Library.Libs.ControleEdicao
{
    /// <summary>
    /// Classe para controle de tarefas que encontram-se em edição
    /// </summary>
    public class TarefaEditada
    {
        /// <summary>
        /// Oid da Tarefa no cronograma
        /// </summary>
        public Guid Oid { get; private set; }

        /// <summary>
        /// Responsável por armazenar uma copia anterior a edição da tarefa
        /// </summary>
        public CronogramaTarefaGridItem TarefaAntiga { get; private set; }

        /// <summary>
        /// Responsável por armazenar se uma tarefa pode ou não ser salva
        /// </summary>
        public bool PodeSalvar { get; set; }

        /// <summary>
        /// Responsável por armazenar se a tarefa Encontra-se em edição
        /// </summary>
        public bool Editando { get; set; }

        /// <summary>
        /// Responsável por armazenar se uma tarefa está
        /// </summary>
        public bool PermissaoPendente { get; private set; }

        public TarefaEditada( CronogramaTarefaGridItem tarefaAtual )
        {
            Oid = tarefaAtual.OidCronogramaTarefa;
            TarefaAntiga = tarefaAtual.Clone();
            PodeSalvar = false;
            PermissaoPendente = true;
            Editando = true;
        }

        /// <summary>
        /// Método para informar que a tarefa recebeu a resposta a solicitacao de edição
        /// </summary>
        public void RecebeuRespostaSolicitacaoEdicao()
        {
            PermissaoPendente = false;
        }

        /// <summary>
        /// Método responsável por transferir valores de uma instância para outra
        /// </summary>
        /// <param name="original">instância de tarefa original</param>
        /// <param name="copia">instância que irá possuir os novos valores</param>
        public static CronogramaTarefaGridItem CopiarValores( CronogramaTarefaGridItem original, CronogramaTarefaGridItem copia )
        {
            copia.OidCronogramaTarefa = original.OidCronogramaTarefa;
            copia.OidCronograma = original.OidCronograma;
            copia.OidSituacaoPlanejamentoTarefa = original.OidSituacaoPlanejamentoTarefa;
            copia.OidTarefa = original.OidTarefa;
            copia.NbRealizado = original.NbRealizado;
            copia.NbID = original.NbID;
            copia.NbEstimativaRestante = original.NbEstimativaRestante;
            copia.NbEstimativaInicial = original.NbEstimativaInicial;
            copia.DtInicio = original.DtInicio;
            copia.DtAtualizadoEm = original.DtAtualizadoEm;
            copia.CsLinhaBaseSalva = original.CsLinhaBaseSalva;
            copia.TxAtualizadoPor = original.TxAtualizadoPor;
            copia.TxDescricaoColaborador = original.TxDescricaoColaborador;
            copia.TxDescricaoSituacaoPlanejamentoTarefa = original.TxDescricaoSituacaoPlanejamentoTarefa;
            copia.TxDescricaoTarefa = original.TxDescricaoTarefa;
            copia.TxObservacaoTarefa = original.TxObservacaoTarefa;
            copia.DtHoraConsulta = original.DtHoraConsulta;
            return copia;
        }

        /// <summary>
        /// Efutar uma cópia clone da tarefa original
        /// </summary>
        /// <param name="original">tarefa original</param>
        /// <returns> um clone criado da tarefa original</returns>
        public static CronogramaTarefaGridItem CopiarValores( CronogramaTarefaGridItem original )
        {
            if(original == null)
                return null;

            return original.Clone();
        }

        /// <summary>
        /// Método responsável por verificar se os campos Situacao Planejamento, Estimativa Inicial e Estimativa Restante foram modificados.
        /// </summary>
        /// <param name="tarefasEditadas">Lista contendo as tarefas que foram solicitadas para edicao</param>
        /// <param name="tarefasAtualizadas">Lista contendo as tarefas atualizadas</param>
        /// <returns>
        /// Hashtable
        /// Key - Guid CronogramaTarefa
        /// Value - List<int> Contendo os Campos Alterados (Cast - Domain -> CsTipoCampoEditado)
        /// </returns>
        public static Hashtable ValidarCamposRelevantesAlterados( List<CronogramaTarefaGridItem> tarefasEditadas, List<CronogramaTarefaGridItem> tarefasAtualizadas )
        {
            Hashtable tarefasAlteradas = new Hashtable();
            for(int i = 0; i < tarefasEditadas.Count; i++)
            {
                CronogramaTarefaGridItem tarefaAtualizada = tarefasAtualizadas[i];
                CronogramaTarefaGridItem tarefaAntiga = tarefasEditadas.FirstOrDefault( o => o.OidCronogramaTarefa == tarefaAtualizada.OidCronogramaTarefa );

                List<int> campos = new List<int>();
                if(tarefaAntiga.TxDescricaoSituacaoPlanejamentoTarefa != tarefaAtualizada.TxDescricaoSituacaoPlanejamentoTarefa)
                    campos.Add( (int)CsTipoCampoEditado.SituacaoPlanejamento );

                if(tarefaAntiga.NbEstimativaInicial != tarefaAtualizada.NbEstimativaInicial)
                    campos.Add( (int)CsTipoCampoEditado.EstimativaInicial );

                if(tarefaAntiga.NbEstimativaRestante != tarefaAtualizada.NbEstimativaRestante)
                    campos.Add( (int)CsTipoCampoEditado.EstimativaRestante );

                //caso possua campo alterado adicionar a lista de tarefas alteradas
                if(campos.Count > 0)
                    tarefasAlteradas.Add( tarefaAtualizada.OidCronogramaTarefa, campos );
            }
            return tarefasAlteradas;
        }

        /// <summary>
        /// Método responsável por verificar se os campos Situacao Planejamento, Estimativa Inicial e Estimativa Restante foram modificados.
        /// </summary>
        /// <param name="tarefasEditadas">Lista contendo as tarefas que foram solicitadas para edicao</param>
        /// <param name="tarefasAtualizadas">Lista contendo as tarefas atualizadas</param>
        /// <returns>
        /// Campos alterados
        /// </returns>
		public static List<int> VerificarAlteracoesRelevantes( CronogramaTarefaGridItem tarefaEditada , CronogramaTarefaGridItem tarefaAtualizada )
		{
			List<int> alteracoes = new List<int>();
			if( tarefaEditada.OidSituacaoPlanejamentoTarefa != tarefaAtualizada.OidSituacaoPlanejamentoTarefa )
				alteracoes.Add( (int)CsTipoCampoEditado.SituacaoPlanejamento );

			if( tarefaEditada.NbEstimativaInicial != tarefaAtualizada.NbEstimativaInicial )
				alteracoes.Add( (int)CsTipoCampoEditado.EstimativaInicial );

			if( tarefaEditada.NbEstimativaRestante != tarefaAtualizada.NbEstimativaRestante )
				alteracoes.Add( (int)CsTipoCampoEditado.EstimativaRestante );

			return alteracoes;
		}

        /// <summary>
        /// Vereficar se houve mudanças na tarefa atual
        /// </summary>
        /// <param name="tarefaAtual"></param>
        /// <returns></returns>
        public bool HouveMudancas( CronogramaTarefaGridItem tarefaAtual )
        {
            return HouveMudancas( tarefaAtual, TarefaAntiga );
        }

        public bool HouveMudancaEm( CronogramaTarefaGridItem tarefaAtual, params Func<CronogramaTarefaGridItem, object>[] propriedades )
        {
            return ComparadorGenerico.HouveMudancaEm( tarefaAtual, TarefaAntiga, propriedades );
        }

        public static bool HouveMudancas( CronogramaTarefaGridItem tarefaAtual, CronogramaTarefaGridItem tarefaAntiga )
        {
            if(tarefaAntiga.OidCronogramaTarefa != tarefaAtual.OidCronogramaTarefa)
                return true;

            if(!string.IsNullOrEmpty( tarefaAtual.TxDescricaoTarefa ) && tarefaAtual.TxDescricaoTarefa != tarefaAntiga.TxDescricaoTarefa)
                return true;

            if(tarefaAtual.DtInicio != null && tarefaAtual.DtInicio != tarefaAntiga.DtInicio)
                return true;

            if(tarefaAtual.NbEstimativaInicial != tarefaAntiga.NbEstimativaInicial)
                return true;

            if(tarefaAtual.NbEstimativaRestante != tarefaAntiga.NbEstimativaRestante)
                return true;

            if(tarefaAtual.NbID != null && tarefaAtual.NbID != tarefaAntiga.NbID)
                return true;

            if(tarefaAtual.NbRealizado != tarefaAntiga.NbRealizado)
                return true;

            if(tarefaAtual.OidSituacaoPlanejamentoTarefa != tarefaAntiga.OidSituacaoPlanejamentoTarefa)
                return true;

            if(!string.IsNullOrEmpty( tarefaAtual.TxDescricaoColaborador ) && tarefaAtual.TxDescricaoColaborador != tarefaAntiga.TxDescricaoColaborador)
                return true;

            if(!string.IsNullOrEmpty( tarefaAtual.TxObservacaoTarefa ) && tarefaAtual.TxObservacaoTarefa != tarefaAntiga.TxObservacaoTarefa)
                return true;

            return false;
        }

        /// <summary>
        /// Método que verifica se tarefa atual pode receber um tipo de situacaoPlanejamento
        /// </summary>
        /// <param name="tarefa">instancia da tarefa</param>
        /// <param name="tipo">tipo de planejamento que irá receber</param>
        public static bool ValidarEdicaoSituacaoPlanejamento( CronogramaTarefaDto tarefa, CsTipoPlanejamento tipo, out string motivo )
        {
            motivo = null;
            if(tarefa == null)
                return false;

            switch(tipo)
            {
                case CsTipoPlanejamento.Encerramento:
                    if(PossuiEstimativaInicial( tarefa ))
                        return true;

                    motivo = Resources.Alerta_DevePossuirDuracaoTarefa;
                    return false;

                case CsTipoPlanejamento.Execução:

                    if(PossuiHorasRestantes( tarefa ))
                        return true;

                    motivo = Resources.Alerta_DevePossuirDuracaoTarefa;
                    return false;

                case CsTipoPlanejamento.Planejamento:
                    if(!PossuiEsforcoRealizado( tarefa ))
                        return true;

                    motivo = Resources.Alerta_PossuiEsforcoRealizadoCadastrado;
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// Método que verifica se uma tarefa possui estimativa inicial
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool PossuiEstimativaInicial( CronogramaTarefaDto tarefa )
        {
            if(TarefaNula( tarefa ))
                return false;
            return PossuiTempoEstimado( tarefa.NbEstimativaInicial );
        }

        /// <summary>
        /// Verifica se uma tarefa não possui estimativa inicial
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool NaoPossuiEstimativaInicial( CronogramaTarefaDto tarefa )
        {
            return !PossuiEstimativaInicial( tarefa );
        }

        /// <summary>
        /// Verifica se uma tarefa não possui estimativa inicial
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool NaoPossuiHorasRestantes( CronogramaTarefaDto tarefa )
        {
            return !PossuiHorasRestantes( tarefa );
        }

        /// <summary>
        /// Retorna se a tarefa possui horas restantes
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool PossuiHorasRestantes( CronogramaTarefaDto tarefa )
        {
            if(TarefaNula( tarefa ))
                return false;
            return PossuiTempoEstimado( tarefa.NbEstimativaRestante );
        }

        /// <summary>
        /// Retorna se a tarefa possui esforço realizado
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool PossuiEsforcoRealizado( CronogramaTarefaDto tarefa )
        {
            if(TarefaNula( tarefa ))
                return false;
            return PossuiTempoEstimado( tarefa.NbRealizado );
        }

        /// <summary>
        /// Retorna se a tarefa possuir ou não esforço realizado
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        public static bool NaoPossuiEsforcoRealizado( CronogramaTarefaDto tarefa )
        {
            return !PossuiEsforcoRealizado( tarefa );
        }

        /// <summary>
        /// Verificar se um horario string possui um valor estimado maior que zero
        /// </summary>
        /// <param name="tarefa">tarefa atual</param>
        /// <param name="hora">propriedade tipo hora</param>
        /// <returns>retorna se possui ou não tempo estimado</returns>
        private static bool PossuiTempoEstimado( double hora )
        {
            return hora > 0;
        }

        /// <summary>
        /// Método para verificar se uma tarefa é nula
        /// </summary>
        /// <param name="tarefa"></param>
        /// <returns></returns>
        private static bool TarefaNula( CronogramaTarefaDto tarefa )
        {
            return tarefa == null;
        }
    }
}