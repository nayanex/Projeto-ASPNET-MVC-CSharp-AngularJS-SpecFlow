using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Library.Libs.Extensions.EnumExtension;
using System.Collections.Generic;
using System.Linq;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CalendarioDaoTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void DeveCarregarOCalendarioCadastradoParaUmDerterminadoPeriodo()
        {
            DateTime dataInicio = DateTime.Parse( "01/05/2014" ), dataTermino = DateTime.Parse( "12/05/2014" );

            AdicionarCalendario( "Dia do trabalhador e ponto facultativo", CsCalendarioDomain.Folga, CsVigenciaDomain.PorPeriodo, DateTime.Parse( "01/05/2014" ), DateTime.Parse( "02/05/2014" ) );
            AdicionarCalendario( "Trabalho no domingo", CsCalendarioDomain.Trabalho, CsVigenciaDomain.PorDiaMesAno, DateTime.Parse( "04/05/2014" ) );
            AdicionarCalendario( "Trabalho fim de semana compensar banco de horas", CsCalendarioDomain.Trabalho, CsVigenciaDomain.PorPeriodo, DateTime.Parse( "10/05/2014" ), DateTime.Parse( "11/05/2014" ) );
            AdicionarCalendario( "Dia do trabalhador", CsCalendarioDomain.Folga, CsVigenciaDomain.PorDiaMesAno, DateTime.Parse( "01/05/2014" ) );

            var calendario = CalendarioDAO.ConsultarCalendarioPorPeriodo( dataInicio, dataTermino );
            var listaDias = contexto.Calendarios.ToList();
        }

        /// <summary>
        /// Método auxiliar para cadastrar datas no calendário do wex
        /// </summary>
        /// <param name="descricao">descrição da data cadastrada</param>
        /// <param name="tipoCalendario">tipo de data (Folga ou Trabalho)</param>
        /// <param name="tipoVigencia">tipo de vigência da data do calendário</param>
        /// <param name="dataInicio">data de inicio</param>
        /// <param name="dataTermino">data de término do período (Opcional, usado apenas quando a vigência for por período)</param>
        private void AdicionarCalendario( string descricao, CsCalendarioDomain tipoCalendario, CsVigenciaDomain tipoVigencia, DateTime dataInicio, DateTime? dataTermino = null )
        {
            //TODO: REVER SE DEVERÁ SER PASSADO PARA O CalendarioBo
            Calendario calendario = new Calendario()
             {
                 CsCalendario = tipoCalendario.ToInt(),
                 CsVigencia = tipoVigencia.ToInt(),
                 DtInicio = dataInicio.Date,
                 Periodo = dataInicio.Date,
                 Oid = Guid.NewGuid(),
                 TxDescricao = descricao,
                 CsSituacao = CsSituacaoDomain.Ativo.ToInt()
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
            contexto.Calendarios.Add( calendario );
            contexto.SaveChanges();
        }
    }
}
