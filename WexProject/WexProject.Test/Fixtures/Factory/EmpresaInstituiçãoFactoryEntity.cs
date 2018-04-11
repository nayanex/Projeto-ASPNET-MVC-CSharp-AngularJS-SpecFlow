using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    public class EmpresaInstituiçãoFactoryEntity
    {
        /// <summary>
        /// Método responsável criar um objeto factory Empresa Instituição
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="txNome">Nome empresa instituição </param>
        /// <param name="txSigla">Sigla</param>
        /// <param name="txEmail">Email</param>
        /// <param name="txFoneFax">Fone</param>
        /// <returns></returns>
        public static EmpresaInstituicao Criar( WexDb contexto, string txNome = "", string txSigla = "", string txEmail = "email@email.com", string txFoneFax = "0000-0000" )
        {
            EmpresaInstituicao empresainstituicao = new EmpresaInstituicao()
            {
                TxNome = txNome,
                TxSigla = txSigla,
                TxEmail = txEmail,
                TxFoneFax = txFoneFax
            };

            EmpresaInstituicaoDAO.Instancia.Salvar( contexto, empresainstituicao );

            return empresainstituicao;
        }
    }
}
