using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.BLL.DAOs.Geral
{
    public class CalendarioDAO
    {
        /// <summary>
        /// Método responsável por consultar um período de datas no calendário.
        /// </summary>
        /// <param name="dataInicio">Data início do período a ser pesquisado</param>
        /// <param name="dataFinal">Data término do período a ser pesquisado</param>
        /// <returns>Retorna uma lista de datas cadastradas no calendário do wex de um determinado período</returns>
        public static List<Calendario> ConsultarCalendarioPorPeriodo(  DateTime dataInicio, DateTime dataFinal )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.Calendarios.
                Where( o =>
                         o.CsSituacao == (int)CsSituacaoDomain.Ativo
                        && ( ( o.CsVigencia == (int)CsVigenciaDomain.PorDiaMesAno && o.DtInicio.Value >= dataInicio.Date && o.DtInicio.Value <= dataFinal.Date)
                            || ( o.CsVigencia == (int)CsVigenciaDomain.PorDiaMes && ( o.NbDia >= dataInicio.Day && o.NbDia <= dataFinal.Day ) && ( o.CsMes >= dataInicio.Month && o.CsMes <= dataFinal.Month ) )
                            || ( o.CsVigencia == (int)CsVigenciaDomain.PorPeriodo && o.DtInicio.Value >= dataInicio.Date && o.DtTermino.Value <= dataFinal.Date ) ) ).ToList();
            }
        }
    }
}
