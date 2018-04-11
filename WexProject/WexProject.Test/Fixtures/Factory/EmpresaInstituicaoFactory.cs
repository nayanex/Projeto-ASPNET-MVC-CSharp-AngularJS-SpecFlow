
using DevExpress.Xpo;
using System;
using WexProject.BLL.Models.Geral;
using WexProject.BLL;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// Factory da classe EmpresaInstituicao
    /// </summary>
    public class EmpresaInstituicaoFactory : BaseFactory
    {
        /// <summary>
        /// Cria um novo objeto de EmpresaInstituicao
        /// </summary>
        /// <param name="contexto">Sessão</param>
        /// <param name="txNome">Nome da Empresa/Instituição</param>
        /// <param name="txSigla">Sigla da Empresa/Instituição</param>
        /// <param name="txEmail">Email da Empresa/Instituição</param>
        /// <param name="txFoneFax">Fone/Fax da Empresa/Instituição</param>
        /// <param name="save">Indica se é pra salvar ou não</param>
        /// <returns>O objeto de EmpresaInstituicao</returns>
        public static EmpresaInstituicao Criar(Session session, string txNome = "", string txSigla = "", string txEmail = "", string txFoneFax = "", bool save = false)
        {
            EmpresaInstituicao empresainstituicao = new EmpresaInstituicao(session);

            if (String.IsNullOrEmpty(txNome))
                empresainstituicao.TxNome = GetDescricao();
            else
                empresainstituicao.TxNome = txNome;

            if (String.IsNullOrEmpty(txSigla))
                empresainstituicao.TxSigla = GetDescricao();
            else
                empresainstituicao.TxSigla = txSigla;

            if (String.IsNullOrEmpty(txEmail))
                empresainstituicao.TxEmail = "email@email.com";
            else
                empresainstituicao.TxEmail = txEmail;

            if (String.IsNullOrEmpty(txFoneFax))
                empresainstituicao.TxFoneFax = "0000-0000";
            else
                empresainstituicao.TxFoneFax = txFoneFax;

            if (save)
                empresainstituicao.Save();

            return empresainstituicao;
        }
    }
}
