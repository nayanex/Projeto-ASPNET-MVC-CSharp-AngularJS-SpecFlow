using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;

namespace WexProject.BLL.BOs.Planejamento
{
    public class TarefaHistoricoEstimativaBo
    {

        /// <summary>
        /// Método responsável por efetuar a criação de um histórico de estimativa
        /// </summary>
        /// <param name="dataHistorico">data do histórico</param>
        /// <param name="oidTarefa">oid da tarefa</param>
        /// <param name="nbHorasRestantes"></param>
        public static TarefaHistoricoEstimativa CriarHistorico( DateTime dataHistorico, Guid oidTarefa, double nbHorasRestantes )
        {
            if (oidTarefa == Guid.Empty)
                throw new ArgumentException( string.Format( "O oid da tarefa indicada é inválido. valor:{0}", oidTarefa ) );

            var historicoEstimativa = new TarefaHistoricoEstimativa
            {
                DtPlanejado =  dataHistorico,
                OidTarefa = oidTarefa,
                NbHoraRestante =  nbHorasRestantes
            };

            return TarefaHistoricoEstimativaDao.SalvarHistorico( historicoEstimativa );
        }
    }
}
