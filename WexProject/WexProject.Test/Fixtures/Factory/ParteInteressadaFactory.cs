using System;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Classe da Parte Interessada Factory
    /// </summary>
    public class ParteInteressadaFactory : BaseFactory
    {
        /// <summary>
        /// setarCampos
        /// </summary>
        /// <param name="parteinteressada">parteinteressada</param>
        /// <param name="csParteInteressada">csParteInteressada</param>
        /// <param name="colaborador">colaborador</param>
        /// <param name="cargo">cargo</param>
        public static void SetarCampos(ParteInteressada parteinteressada, CsSimNao csParteInteressada, Colaborador colaborador, Cargo cargo)
        {
            if(csParteInteressada == CsSimNao.Sim)
            {
                parteinteressada.Colaborador = colaborador;
            }
            else
            {
                parteinteressada.Cargo = cargo;
                parteinteressada.TxTelefoneFixo = "000000000000";
                parteinteressada.TxCelular = "000000000000";
                parteinteressada.TxEmail = "nome_usuario@fpf.br";
            }
        }
        /// <summary>
        /// ParteInteressada
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="csParteInteressada">csParteInteressada</param>
        /// <param name="colaborador">colaborador</param>
        /// <param name="cargo">cargo</param>
        /// <param name="save">save</param>
        /// <returns>parteinteressada</returns>
        public static ParteInteressada Criar( Session session, CsSimNao csParteInteressada, Colaborador colaborador, Cargo cargo, bool save = false )
        {
            ParteInteressada parteinteressada = new ParteInteressada(session);

            parteinteressada.IsColaborador = csParteInteressada;

            SetarCampos(parteinteressada, csParteInteressada, colaborador, cargo);

            if (save)
                parteinteressada.Save();

            return parteinteressada;
        }

    }
}
