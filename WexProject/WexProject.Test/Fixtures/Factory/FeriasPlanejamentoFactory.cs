using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe FeriasPlanejamento
    /// </summary>
    public class FeriasPlanejamentoFactory : BaseFactory
    {
        /// <summary>
        /// Criação de um novo planejamento de férias
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="periodo">Período aquisitivo</param>
        /// <param name="modalidade">Modalidade de Férias</param>
        /// <param name="dtInicio">Data de início</param>
        /// <param name="realizadas">Já foram realizadas?</param>
        /// <param name="vender">Vendidas?</param>
        /// <param name="save">É para salvar?</param>
        /// <returns>Objeto de FeriasPlanejamento criado</returns>
        public static FeriasPlanejamento CriarPlanejamentoFerias(Session session, ColaboradorPeriodoAquisitivo periodo,
            ModalidadeFerias modalidade, DateTime dtInicio, bool realizadas = false, CsSimNao vender = CsSimNao.Não, bool save = true)
        {
            FeriasPlanejamento planejamento = new FeriasPlanejamento(session)
            {
                Periodo = periodo,
                Modalidade = modalidade,
                DtInicio = dtInicio,
                Realizadas = realizadas,
                Vender = vender
            };

            if (save)
                planejamento.Save();

            return planejamento;
        }
    }
}