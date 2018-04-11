using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;

namespace WexProject.Test.Fixtures.Factory
{
    public class PlanejamentoFeriasFactory
    {

        public static FeriasPlanejamento Criar(Session session, Colaborador colaborador, DateTime dataIncio, ModalidadeFerias modalidade, bool save)
        {
            FeriasPlanejamento ferias = new FeriasPlanejamento(session);
            ferias.Periodo = colaborador.PeriodosAquisitivos[0];
            ferias.Modalidade = modalidade;
            ferias.DtInicio = dataIncio;
            ferias.TxPlanejado = "Superior";

            if (save)
                ferias.Save();

            return ferias;
    }
}
}
