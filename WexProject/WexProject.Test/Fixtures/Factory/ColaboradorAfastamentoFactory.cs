using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.Models.Rh;
using DevExpress.Xpo;

namespace WexProject.Test.Fixtures.Factory
{
    public class ColaboradorAfastamentoFactory : BaseFactory
    {
        public static ColaboradorAfastamento CriarColaboradorAfastamento(Session session, Colaborador colaborador, DateTime dtInicio, DateTime dtTermino, TipoAfastamento tipo, string txObservacao, bool save = true)
        {
            ColaboradorAfastamento afastamento = new ColaboradorAfastamento(session)
            {
                Colaborador = colaborador,
                DtInicio = dtInicio,
                DtTermino = dtTermino,
                TipoAfastamento = tipo,
                TxObservacao = txObservacao
            };

            if (save)
                afastamento.Save();

            return afastamento;
        }
    }
}