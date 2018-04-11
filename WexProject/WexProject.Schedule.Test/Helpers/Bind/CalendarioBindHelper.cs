using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Library.Libs.Extensions.EnumExtension;

namespace WexProject.Schedule.Test.Helpers.Bind
{
    public class CalendarioBindHelper
    {

        public DateTime Data { get; set; }

        public string Descricao { get; set; }

        public CsCalendarioDomain Tipo { get; set; }

        public CsSituacaoDomain Situacao { get; set; }

        public string Vigencia { get; set; }

        public CsVigenciaDomain CsVigencia { get; set; }

        private CsVigenciaDomain ConverterVigencia( string vigencia )
        {
            switch(vigencia)
            {
                case "Por periodo":
                    return CsVigenciaDomain.PorPeriodo;
                case "Por dia do mes":
                    return CsVigenciaDomain.PorDiaMes;
                case "Por dia, mes e ano":
                    return CsVigenciaDomain.PorDiaMesAno;
                default:
                    return CsVigenciaDomain.PorDiaMesAno;
            }
        }

        public Calendario ToCalendario()
        {
            CsVigencia = ConverterVigencia( Vigencia );
            return CriarCalendario( Descricao, Tipo, CsVigencia, Data, Situacao );
        }

        private Calendario CriarCalendario( string descricao, CsCalendarioDomain tipoCalendario, CsVigenciaDomain tipoVigencia, DateTime dataInicio, CsSituacaoDomain situacao, DateTime? dataTermino = null )
        {
            Calendario calendario = new Calendario()
            {
                CsCalendario = tipoCalendario.ToInt(),
                CsVigencia = tipoVigencia.ToInt(),
                DtInicio = dataInicio.Date,
                Periodo = dataInicio.Date,
                Oid = Guid.NewGuid(),
                TxDescricao = descricao,
                CsSituacao = situacao.ToInt()
            };

            switch(tipoVigencia)
            {
                case CsVigenciaDomain.PorDiaMes:
                    calendario.CsMes = dataInicio.Month;
                    calendario.NbDia = dataInicio.Day;
                    break;
                case CsVigenciaDomain.PorDiaMesAno:
                    break;
                case CsVigenciaDomain.PorPeriodo:
                    if(!dataTermino.HasValue)
                        throw new ArgumentException( "Deveria ter preenchido a data de término do período." );
                    calendario.DtTermino = dataTermino.Value.Date;
                    break;
                default:
                    break;
            }
            return calendario;
        }
    }
}
