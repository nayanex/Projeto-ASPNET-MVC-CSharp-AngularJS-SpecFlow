using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    public class ComandoAtualizarTarefasCronograma : Comando
    {
        /// <summary>
        /// gerenciador de comandos
        /// </summary>
        private GerenciadorComandos gerenciador;

        /// <summary>
        /// lista de tarefas atualizadas
        /// </summary>
        private List<CronogramaTarefaGridItem> tarefasAtualizadas;
        public ComandoAtualizarTarefasCronograma( GerenciadorComandos gerenciadorComandos, List<CronogramaTarefaGridItem> tarefas )
        {
            gerenciador = gerenciadorComandos;
            tarefasAtualizadas = tarefas;
        }
        public override void Executar()
        {
            if(tarefasAtualizadas == null)
                return;

            if(tarefasAtualizadas.Count == 0)
            {
                gerenciador.EsperarEscritaDataSource();
                gerenciador.Datasource.Clear();
                gerenciador.LiberarEscritaDataSource();
            }

            gerenciador.EsperarLeituraDataSource();
            List<CronogramaTarefaGridItem> tarefasView = gerenciador.Datasource.ToList();
            gerenciador.LiberarLeituraDataSource();
            List<Guid> oidTarefasAtuais, oidTarefasView;
            oidTarefasView = new List<Guid>( tarefasView.Select( o => o.OidCronogramaTarefa ) );
            oidTarefasAtuais = new List<Guid>( tarefasAtualizadas.Select( o => o.OidCronogramaTarefa ) );
            List<CronogramaTarefaGridItem> tarefasParaRemover = new List<CronogramaTarefaGridItem>( GetTarefasParaRemover( tarefasView, oidTarefasAtuais, oidTarefasView ) );
            List<CronogramaTarefaGridItem> tarefasParaAdicionar = new List<CronogramaTarefaGridItem>( GetTarefasParaAdicionar( tarefasAtualizadas, oidTarefasAtuais, oidTarefasView ) );
            RemoverTarefas( tarefasParaRemover );
            AdicionarTarefas( tarefasParaAdicionar );
            AtualizarTarefas( tarefasAtualizadas );
        }

        private IEnumerable<CronogramaTarefaGridItem> GetTarefasParaRemover( List<CronogramaTarefaGridItem> tarefasView, List<Guid> oidTarefasAtuais, List<Guid> oidTarefasView )
        {
            List<Guid> oidTarefasARemover = oidTarefasView.Except( oidTarefasAtuais ).ToList();
            return tarefasView.Where( o => oidTarefasARemover.Contains( o.OidCronogramaTarefa ) );
        }

        private IEnumerable<CronogramaTarefaGridItem> GetTarefasParaAdicionar( List<CronogramaTarefaGridItem> tarefasAtuais, List<Guid> oidTarefasAtuais, List<Guid> oidTarefasView )
        {
            List<Guid> oidTarefasAdicionar = oidTarefasAtuais.Except( oidTarefasView ).ToList();
            return tarefasAtuais.Where( o => oidTarefasAdicionar.Contains( o.OidCronogramaTarefa ) );
        }
        /// <summary>
        /// Método para remover tarefas que não existem mais no dataSource novo
        /// </summary>
        /// <param name="tarefasRemocao"></param>
        public void RemoverTarefas( List<CronogramaTarefaGridItem> tarefasRemocao )
        {
            gerenciador.EsperarEscritaDataSource();
            foreach(CronogramaTarefaGridItem tarefa in tarefasRemocao)
            {
                gerenciador.Datasource.Remove( gerenciador.Datasource.First( o => o.OidCronogramaTarefa == tarefa.OidCronogramaTarefa ) );
            }
            gerenciador.LiberarEscritaDataSource();
        }

        /// <summary>
        /// Método para adicionar as tarfas que ainda não existem no datasource desatualizado
        /// </summary>
        /// <param name="tarefasAdicao"> novas tarefas</param>
        public void AdicionarTarefas( List<CronogramaTarefaGridItem> tarefasAdicao )
        {
            gerenciador.EsperarEscritaDataSource();
            foreach(CronogramaTarefaGridItem tarefa in tarefasAdicao)
            {
                gerenciador.Datasource.Add( tarefa );
            }
            gerenciador.LiberarEscritaDataSource();
        }

        /// <summary>
        /// método para atualizar as tarefas antigas
        /// </summary>
        /// <param name="tarefasParaAtualizar"></param>
        public void AtualizarTarefas( List<CronogramaTarefaGridItem> tarefasParaAtualizar )
        {
            CronogramaTarefaGridItem tarefa;
            foreach(CronogramaTarefaGridItem tarefaAtual in tarefasParaAtualizar)
            {
                gerenciador.EsperarLeituraDataSource();
                tarefa = gerenciador.Datasource.FirstOrDefault( o => o.OidCronogramaTarefa == tarefaAtual.OidCronogramaTarefa );
                gerenciador.LiberarLeituraDataSource();
                if(tarefa != null)
                    if(gerenciador.GerenciadorTarefasImpactadas.AplicarDataAtualizacao( tarefaAtual.OidCronogramaTarefa, tarefaAtual.DtHoraConsulta ))
                        AtualizarTarefa( tarefa, tarefaAtual );
                    else
                        tarefa.NbID = tarefaAtual.NbID;
            }
        }

        /// <summary>
        /// Método para realizar atualização da tarefa
        /// </summary>
        /// <param name="tarefaAntiga"> tarefa armazenado na view</param>
        /// <param name="tarefaAtual"> tarefa com dados mais atuais</param>
        public static void AtualizarTarefa( CronogramaTarefaGridItem tarefaAntiga, CronogramaTarefaGridItem tarefaAtual )
        {
            tarefaAntiga.NbRealizado = tarefaAtual.NbRealizado;
            tarefaAntiga.NbID = tarefaAtual.NbID;
            tarefaAntiga.NbEstimativaRestante = tarefaAtual.NbEstimativaRestante;
            tarefaAntiga.NbEstimativaInicial = tarefaAtual.NbEstimativaInicial;
            tarefaAntiga.DtInicio = tarefaAtual.DtInicio;
            tarefaAntiga.DtAtualizadoEm = tarefaAtual.DtAtualizadoEm;
            tarefaAntiga.CsLinhaBaseSalva = tarefaAtual.CsLinhaBaseSalva;
            tarefaAntiga.TxAtualizadoPor = tarefaAtual.TxAtualizadoPor;
            tarefaAntiga.TxDescricaoColaborador = tarefaAtual.TxDescricaoColaborador;
            tarefaAntiga.TxDescricaoSituacaoPlanejamentoTarefa = tarefaAtual.TxDescricaoSituacaoPlanejamentoTarefa;
            tarefaAntiga.TxDescricaoTarefa = tarefaAtual.TxDescricaoTarefa;
            tarefaAntiga.TxObservacaoTarefa = tarefaAtual.TxObservacaoTarefa;
            tarefaAntiga.OidSituacaoPlanejamentoTarefa = tarefaAtual.OidSituacaoPlanejamentoTarefa;
        }
    }
}
