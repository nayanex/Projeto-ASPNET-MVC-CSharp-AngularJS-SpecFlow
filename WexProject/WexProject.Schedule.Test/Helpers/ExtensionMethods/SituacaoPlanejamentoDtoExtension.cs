using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Test.Helpers.ExtensionMethods
{
    public static class SituacaoPlanejamentoDtoExtension
    {
        /// <summary>
        /// Método para retornar uma situação planejamento de acordo com o tipo determinado em uma determinada lista de situações planejamento
        /// </summary>
        /// <param name="situacoes">Instância da Lista de situações planejamento</param>
        /// <param name="tipo">Tipo de situação planejamento</param>
        /// <returns></returns>
        public static SituacaoPlanejamentoDTO OndeOTipoFor( this List<SituacaoPlanejamentoDTO> situacoes, CsTipoPlanejamento tipo )
        {
            if(situacoes == null || tipo == null)
                return null;
            return situacoes.FirstOrDefault( o => o.CsTipo == tipo );
        }
    }
}
