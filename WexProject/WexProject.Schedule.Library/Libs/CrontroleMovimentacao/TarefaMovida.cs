using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.Schedule.Library.Domains;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Library.Libs.CrontroleMovimentacao
{
    /// <summary>
    /// Classe para auxilio do gerenciador de comandos para efetuar as movimentações das tarefas
    /// </summary>
    public class TarefaMovida
    {
        #region Atributos

        /// <summary>
        /// oid de  identificação da tarefa atual
        /// </summary>
        private Guid oidTarefa;

        /// <summary>
        /// posição inicial da tarefa
        /// </summary>
        private short posicaoInicial;

        /// <summary>
        /// posição final da tarefa
        /// </summary>
        private short posicaoFinal;

        /// <summary>
        /// tipo de movimentação da tarefa acima/abaixo
        /// </summary>
        private CsTipoSituacaoLinhaGrid movimento;

        /// <summary>
        /// Nova ordem das tarefas após a reordenação das tarefas afetadas pela movimentação
        /// </summary>
        Dictionary<string, Int16> tarefasImpactadas;
        #endregion

        #region Propriedades

        /// <summary>
        /// retornar a identificação da tarefa atual
        /// </summary>
        public Guid OidTarefa
        {
            get { return oidTarefa; }
        }

        /// <summary>
        /// retornar a posição inicial
        /// </summary>
        public short PosicaoInicial
        {
            get { return posicaoInicial; }
        }

        /// <summary>
        /// retornar a posição final da tarefa
        /// </summary>
        public short PosicaoFinal
        {
            get { return posicaoFinal; }
        }

        /// <summary>
        /// retornar o movimento da tarefa acima ou abaixo
        /// </summary>
        public CsTipoSituacaoLinhaGrid Movimento
        {
            get { return movimento; }
        }

        /// <summary>
        /// retornar as tarefas impactadas pela movimentação reordenadas
        /// </summary>
        public Dictionary<string, Int16> TarefasImpactadas
        {
            get { return tarefasImpactadas; }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Calcular o tipo de movimento ocasionado pela movimentação da tarefa
        /// </summary>
        /// <returns></returns>
        private CsTipoSituacaoLinhaGrid TipoMovimento()
        {
            if(posicaoInicial > posicaoFinal)
                return CsTipoSituacaoLinhaGrid.MovidaAcima;
            else
                return CsTipoSituacaoLinhaGrid.MovidaAbaixo;
        }

        #endregion

        #region Construtor


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="oidCronogramaTarefa">oid da tarefa atual</param>
        /// <param name="inicial">posição inicial da tarefa</param>
        /// <param name="final">nova posição da tarefa</param>
        /// <param name="tarefasImpactadas">dicionário de reordenação das tarefas impactadas</param>
        public TarefaMovida( Guid oidCronogramaTarefa, Int16 inicial, Int16 final, Dictionary<string, Int16> tarefasImpactadas )
        {
            oidTarefa = oidCronogramaTarefa;
            posicaoInicial = inicial;
            posicaoFinal = final;
            movimento = TipoMovimento();
            this.tarefasImpactadas = tarefasImpactadas;
        }


        #endregion
    }
}
