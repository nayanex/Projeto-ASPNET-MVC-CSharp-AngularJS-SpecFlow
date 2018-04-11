using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Escopo;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Generico;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.Test.Fixtures.Factory
{
    public class BeneficiadoFactoryEntity
    {
        /// <summary>
        /// Método responsável por criar um beneficiado para o teste
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="txDescricao">Descricao do beneficiado</param>
        /// <returns>Objeto Beneficiado</returns>
        public static Beneficiado Criar( WexDb contexto, string txDescricao = "" )
        {
            Beneficiado beneficiado = new Beneficiado() 
            {
                TxDescricao = txDescricao
            };

            BeneficiadoDAO.SalvarBeneficiado( contexto, beneficiado );
            return beneficiado;
        }
    }
}
