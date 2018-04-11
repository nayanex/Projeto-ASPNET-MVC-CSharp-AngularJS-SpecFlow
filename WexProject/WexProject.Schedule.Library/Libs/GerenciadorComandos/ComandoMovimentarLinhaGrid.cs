using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Schedule.Library.Libs.CrontroleMovimentacao;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe responsável por efetuar comando de  movimentação de linha no grid
    /// </summary>
    public class ComandoMovimentarLinhaGrid : Comando
    {
        /// <summary>
        /// armarnezar o gerenciador de comandos
        /// </summary>
        GerenciadorComandos gerenciador;

        /// <summary>
        /// armazenar os detalhes da movimentação da tarefa
        /// </summary>
        TarefaMovida tarefaMovida;

        /// <summary>
        /// Construtor que armazena os atributos necessários e o comando para atualizar tarefas impactadas
        /// </summary>
        /// <param name="gerenciador">gerenciador de comandos</param>
        /// <param name="tarefaMovida">Objeto contendo dados da movimentação</param>
        public ComandoMovimentarLinhaGrid( GerenciadorComandos gerenciador, TarefaMovida tarefaMovida )
        {
            this.gerenciador = gerenciador;
            this.tarefaMovida = tarefaMovida;
        }

        /// <summary>
        /// Executar a movimentação da tarefa
        /// </summary>
        public override void Executar()
        {
            gerenciador.EsperarLeituraDataSource();
            CronogramaTarefaDto tarefaAtual = gerenciador.Datasource.FirstOrDefault( o => o.OidCronogramaTarefa == tarefaMovida.OidTarefa );
            gerenciador.LiberarLeituraDataSource();
            tarefaAtual.NbID = tarefaMovida.PosicaoFinal;
        }
    }
}
