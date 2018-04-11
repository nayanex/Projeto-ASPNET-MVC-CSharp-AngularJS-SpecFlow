using System;

using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe com métodos para criar calendários coorporativos.
    /// </summary>
    public class CalendarioFactory : BaseFactory
    {
        /// <summary>
        /// Cria uma instância de Calendario de folga para um dia específico.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="dia">Dia do calendário</param>
        /// <param name="mes">Mês do calendário</param>
        /// <param name="ano">Ano do calendário</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarDiaUtil(Session session, int dia, CsMesDomain mes, int ano, bool save = false)
        {
            Calendario plan = CriarCalendarioPorDiaMesAno(session, CsCalendarioDomain.Trabalho,
            dia, mes, ano, save);
            return plan;
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp de trabalho para um dia específico.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="dia">Dia do calendário</param>
        /// <param name="mes">Mês do calendário</param>
        /// <param name="ano">Ano do calendário</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarDiaNaoUtil(Session session, int dia, CsMesDomain mes, int ano, bool save = false)
        {
            Calendario plan = CriarCalendarioPorDiaMesAno(session, CsCalendarioDomain.Folga,
            dia, mes, ano, save);
            return plan;
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp de trabalho para um dia e 
        /// mês repetidos anualmente.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="dia">Dia do calendário não-útil</param>
        /// <param name="mes">Mês do calendário não-útil</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarDiaMesNaoUtil(Session session, int dia, CsMesDomain mes, bool save = false)
        {
            Calendario plan = CriarCalendarioPorDiaMes(session, CsCalendarioDomain.Folga,
            dia, mes, save);
            return plan;
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp para um período útil.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="dtInicio">Dia de início do período não-útil</param>
        /// <param name="dtTermino">Dia de término do período não-útil</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarPeriodoUtil(Session session, DateTime dtInicio, DateTime dtTermino, bool save = false)
        {
            Calendario plan = CriarCalendarioPorPeriodo(session, CsCalendarioDomain.Trabalho,
            dtInicio, dtTermino, save);
            return plan;
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp para um período.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="dtInicio">Dia de início do período não-útil</param>
        /// <param name="dtTermino">Dia de término do período não-útil</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarPeriodoNaoUtil(Session session, DateTime dtInicio, DateTime dtTermino, bool save = false)
        {
            Calendario plan = CriarCalendarioPorPeriodo(session, CsCalendarioDomain.Folga,
            dtInicio, dtTermino, save);
            return plan;
        }


        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="csCalendario">Folga ou Trabalho</param>
        /// <param name="csVigencia">PorPeriodo, PorDiaMes ou PorDiaMesAno</param>
        /// <param name="dtInicio">Dia de inicio do calendário por Período</param>
        /// <param name="dtTermino">Dia de término do calendário por Período</param>
        /// <param name="dia">Dia do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="mes">Mês do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="ano">Ano do calendário PorDiaMesAno</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarCalendario(Session session, CsCalendarioDomain csCalendario,
        CsVigenciaDomain csVigencia, DateTime dtInicio,
        DateTime dtTermino, int dia, CsMesDomain mes, int ano, bool save = false)
        {
            Calendario cal = new Calendario(session)
            {
            CsCalendario = csCalendario,
            CsVigencia = csVigencia,
            CsSituacao = CsSituacaoDomain.Ativo,
            TxDescricao = GetDescricao()
            };

            switch (csVigencia)
            {
                case CsVigenciaDomain.PorPeriodo:
                    cal.DtInicio = dtInicio;
                    cal.DtTermino = dtTermino;
                    break;
                case CsVigenciaDomain.PorDiaMes:
                    cal.CsMes = mes;
                    cal.NbDia = dia;
                    break;
                case CsVigenciaDomain.PorDiaMesAno:
                    cal.DtInicio = new DateTime(ano, (int)mes, dia);
                    break;
            }

            if (save)
                cal.Save();

            return cal;
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp por um determinado período.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="csCalendario">Folga ou Trabalho</param>
        /// <param name="dtInicio">Dia de inicio do calendário por Período</param>
        /// <param name="dtTermino">Dia de término do calendário por Período</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarCalendarioPorPeriodo(Session session, CsCalendarioDomain csCalendario,
        DateTime dtInicio, DateTime dtTermino, bool save = false)
        {
            return CriarCalendario(session, csCalendario, CsVigenciaDomain.PorPeriodo, dtInicio, dtTermino,
            0, CsMesDomain.Janeiro, 0, save);
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp por determinado Dia, mês e Ano.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="csCalendario">Folga ou Trabalho</param>
        /// <param name="dia">Dia do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="mes">Mês do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="ano">Ano do calendário PorDiaMesAno</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarCalendarioPorDiaMesAno(Session session, CsCalendarioDomain csCalendario,
        int dia, CsMesDomain mes, int ano, bool save = false)
        {
            return CriarCalendario(session, csCalendario, CsVigenciaDomain.PorDiaMesAno, new DateTime(),
            new DateTime(), dia, mes, ano, save);
        }

        /// <summary>
        /// Cria uma instância de PlanCalendarioCoorp por determinado Dia e mês repetidos a cada ano.
        /// </summary>
        /// <param name="session">Seção atual</param>
        /// <param name="csCalendario">Folga ou Trabalho</param>
        /// <param name="dia">Dia do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="mes">Mês do calendário PorDiaMesAno e PorDiaMes</param>
        /// <param name="save">Se verdadeiro, persiste</param>
        /// <returns>Instância de PlanCalendario para testes</returns>
        public static Calendario CriarCalendarioPorDiaMes(Session session, CsCalendarioDomain csCalendario,
        int dia, CsMesDomain mes, bool save = false)
        {
            return CriarCalendario(session, csCalendario, CsVigenciaDomain.PorDiaMes, new DateTime(),
            new DateTime(), dia, mes, 0, save);
        }
    }
}
