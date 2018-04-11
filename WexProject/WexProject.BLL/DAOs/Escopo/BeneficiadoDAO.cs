using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.BLL.DAOs.Escopo
{
    public class BeneficiadoDAO
    {
        /// <summary>
        /// Método responsável por buscar um beneficiado por nome
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="nome">Nome do beneficiado</param>
        /// <returns>Objeto Beneficiado</returns>
        public static Beneficiado ConsultarBeneficiadoPorNome( WexDb contexto, string nome )
        {
            return contexto.Beneficiados.FirstOrDefault( o => o.TxDescricao == nome );
        }

        /// <summary>
        /// Método responsável por criar ou alterar um beneficiado
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="beneficiado">Objeto Beneficiado a ser salvo</param>
        public static void SalvarBeneficiado( WexDb contexto, Beneficiado beneficiado )
        {
            if(!contexto.Beneficiados.ExisteLocalmente(o=>o.Oid == beneficiado.Oid))
                contexto.Beneficiados.Attach( beneficiado );

            if(contexto.Beneficiados.Existe( o => o.Oid == beneficiado.Oid ))
                contexto.Entry( beneficiado ).State = EntityState.Modified;
            else 
                contexto.Entry( beneficiado ).State = EntityState.Added;
            
            contexto.SaveChanges();
        }
    }
}
