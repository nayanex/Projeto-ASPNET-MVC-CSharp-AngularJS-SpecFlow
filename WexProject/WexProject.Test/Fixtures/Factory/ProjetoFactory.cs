using System;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory projeto
    /// </summary>
    public class ProjetoFactory : BaseFactory
    {
        /// <summary>
        /// método criar
        /// </summary>
        /// <param name="contexto">contexto</param>
        /// <param name="nbTamanhoTotal">NbTamanhoTotal</param>
        /// <param name="txNome">txNome</param>
        /// <param name="save">bool</param>
        /// <returns>projeto</returns>
        public static Projeto Criar(Session session, UInt32 nbTamanhoTotal = 0, String txNome = "", bool save = false)
        {
            Projeto projeto = new Projeto(session);
            if (String.IsNullOrEmpty(txNome))
                projeto.TxNome = GetDescricao();

            projeto.EmpresaInstituicao = EmpresaInstituicaoFactory.Criar(session);
            projeto.NbTamanhoTotal = nbTamanhoTotal;

            if (save)
                projeto.Save();

            return projeto;
        }


        /// método criar
        /// </summary>
        /// <param name="contexto">contexto</param>
        /// <param name="nbTamanhoTotal">NbTamanhoTotal</param>
        /// <param name="txNome">txNome</param>
        /// <param name="save">bool</param>
        /// <returns>projeto</returns>
        public static Projeto CriarProjetoRitmo(Session session, UInt32 nbTamanhoTotal = 0, String txNome = "", bool save = false, ushort totalCiclos = 1, ushort ritmo = 1)
        {
            Projeto projeto = new Projeto(session);
            if (String.IsNullOrEmpty(txNome))
                projeto.TxNome = GetDescricao();

            projeto.EmpresaInstituicao = EmpresaInstituicaoFactory.Criar(session);
            projeto.NbTamanhoTotal = nbTamanhoTotal;
            projeto.NbRitmoTime = ritmo;
            projeto.NbCicloTotalPlan = totalCiclos;
            projeto.TxNome = txNome;

            if (save)
                projeto.Save();
            

            return projeto;
        }
    }
}
