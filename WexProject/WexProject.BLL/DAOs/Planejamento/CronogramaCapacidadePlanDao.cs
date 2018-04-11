using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Exceptions.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Contexto;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class CronogramaCapacidadePlanDao
    {

        /// <summary>
        /// Método para criar uma capacidade de planejamento
        /// </summary>
        /// <param name="contexto">contexto</param>
        /// <param name="cronogramaAtual">cronograma atual</param>
        /// <param name="capacidade">capacidade de planejamento atual</param>
        public static void CriarCapacidadePlanejamento( WexDb contexto, Cronograma cronogramaAtual, CronogramaCapacidadePlan capacidade )
        {
            if(capacidade.DtDia < cronogramaAtual.DtInicio || capacidade.DtDia > cronogramaAtual.DtFinal)
                throw new CapacidadePlanejamentoForaDoPeriodoCronogramaException( string.Format( "A data da capacidade de planejamento deveria estar entre {0} e {1} atual:{2}",
                    cronogramaAtual.DtInicio, cronogramaAtual.DtFinal, capacidade.DtDia ) );

            if(!ValidarCronogramaCapacidadePlanUnicaPorDia( cronogramaAtual, capacidade.DtDia ))
                throw new CronogramaCapacidadePlanDataJaCadastradaException( "Já existe capacidade de planejamento cadastrada para a data atual" );

            capacidade.Cronograma = cronogramaAtual;
            contexto.CronogramaCapacidadePlan.Add( capacidade );
            contexto.SaveChanges();
        }

        /// <summary>
        /// Verificar se a capacidade planejamento é única para a data selecionada em um determinado cronograma
        /// </summary>
        /// <param name="cronograma">cronograma atual</param>
        /// <param name="data">data selecionada</param>
        /// <returns></returns>
        public static bool ValidarCronogramaCapacidadePlanUnicaPorDia(Cronograma cronograma, DateTime data) 
        {
            return cronograma.CapacidadesPlanejamento.FirstOrDefault( o => o.DtDia.Date.Equals( data.Date ) ) == null;
        }

        /// <summary>
        /// ConsultarIncluindoRelacionamentos um CronogramaCapacidadePlan pela chave
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="oidCronogramaCapacidadePlan">chave</param>
        /// <returns>CronogramaCapacidadePlan relativa a chave indicada</returns>
        public static CronogramaCapacidadePlan ConsultarCronogramaCapacidadePlanPorOid(WexDb contexto, Guid oidCronogramaCapacidadePlan)
        {
            return contexto.CronogramaCapacidadePlan.Find( oidCronogramaCapacidadePlan );
        }
    }
}
