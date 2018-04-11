using System;
using DevExpress.Xpo;

using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Execucao;
using WexProject.Test.Features.StepDefinition;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe de Beneficiado Factory
    /// </summary>
    public class CicloDesenvEstoriaFactory : BaseFactory
    {
        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="ciclo">Ciclo</param>
        /// <param name="estoria">Estoria</param>
        /// <param name="save">bool</param>
        /// <returns>cicloDesenv</returns>
        public static CicloDesenvEstoria Criar(Session session, CicloDesenv ciclo, Estoria estoria, bool save = false)
        {
            CicloDesenvEstoria cicloDesenv = new CicloDesenvEstoria(session);

            cicloDesenv.Ciclo = ciclo;
            cicloDesenv.Estoria = estoria;

            if (save)
                cicloDesenv.Save();

            return cicloDesenv;
        }

        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="ciclo">Ciclo</param>
        /// <param name="estoria">Estoria</param>
        /// <param name="save">bool</param>
        /// <returns>cicloDesenv</returns>
        public static CicloDesenvEstoria CriarComMeta(Session session, CicloDesenv ciclo, Estoria estoria, string meta, bool save = false)
        {
            CicloDesenvEstoria cicloDesenv = new CicloDesenvEstoria(session);

            cicloDesenv.Ciclo = ciclo;
            cicloDesenv.Estoria = estoria;
            if (meta.ToLower().Equals("sim") ) {
                cicloDesenv.Meta = true;
            } else {
                cicloDesenv.Meta = false;
            }
            if (save)
                cicloDesenv.Save();

            return cicloDesenv;
        }


        internal static CicloDesenvEstoria Criar(Session session, CicloDesenv ciclo, Estoria est, string situacaoEstoria, bool save)
        {
            CicloDesenvEstoria cicloDesenv = new CicloDesenvEstoria(session) { 
                Ciclo = ciclo,Estoria = est,
                CsSituacao = StepCiclo.SituacaoEstoriaCicloByText(situacaoEstoria) 
            };
            if (save)
                cicloDesenv.Save();

            return cicloDesenv;
    }
}
}
