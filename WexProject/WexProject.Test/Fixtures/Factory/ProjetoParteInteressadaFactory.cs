using System;
using DevExpress.Xpo;

using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe da Parte Interessada Factory
    /// </summary>
    public class ProjetoParteInteressadaFactory : BaseFactory
    {
        /// <summary>
        /// ProjetoParteInteressada
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <param name="save">save</param>
        /// <returns>projetoparteinteressada</returns>
        public static ProjetoParteInteressada Criar(Session session, WexProject.BLL.Models.Geral.Projeto projeto, bool save = false)
        {
            ProjetoParteInteressada projetoparteinteressada = new ProjetoParteInteressada(session);

            // if (String.IsNullOrEmpty(ParteInteressada))
            //   parteinteressada.ParteInteressada = GetDescricao();

            //if (String.IsNullOrEmpty(Papel))
            //  parteinteressada.Papel = GetDescricao();

            projetoparteinteressada.Projeto = projeto;

            if (save)
                projetoparteinteressada.Save();

            return projetoparteinteressada;
        }

    }
}
