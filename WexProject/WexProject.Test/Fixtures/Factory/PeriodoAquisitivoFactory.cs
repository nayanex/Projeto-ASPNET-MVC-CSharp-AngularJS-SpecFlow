using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe PeriodoAquisitivo
    /// </summary>
    public class PeriodoAquisitivoFactory : BaseFactory
    {
        /// <summary>
        /// Criação de um novo período aquisitivo
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="dtInicio">Data de início</param>
        /// <param name="save">É para salvar?</param>
        /// <returns>Objeto de ColaboradorPeriodoAquisitivo criado</returns>
        public static ColaboradorPeriodoAquisitivo CriarPeriodoAquisitivo(Session session, Colaborador colaborador, DateTime dtInicio, bool save = true)
        {
            ColaboradorPeriodoAquisitivo periodo = new ColaboradorPeriodoAquisitivo(session)
            {
                Colaborador = colaborador,
                DtInicio = dtInicio,
                DtTermino = dtInicio.AddYears(1).AddDays(-1),
                NbFeriasPlanejadas = 0
            };

            if (save)
                periodo.Save();

            return periodo;
        }
    }
}
