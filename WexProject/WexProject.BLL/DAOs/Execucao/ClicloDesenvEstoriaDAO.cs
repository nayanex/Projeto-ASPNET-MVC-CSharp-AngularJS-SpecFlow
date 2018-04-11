using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Extensions.Entities;

namespace WexProject.BLL.DAOs.Execucao
{
    public class ClicloDesenvEstoriaDAO
    {
        /// <summary>
        /// Método para salvar uma criação ou alteração na entidade
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="cicloDesenvEstoria">Objeto a ser salvo</param>
        public static void Salvar( WexDb contexto, CicloDesenvEstoria cicloDesenvEstoria )
        {
            if(!contexto.CicloDesenvEstorias.ExisteLocalmente( o => o.Oid == cicloDesenvEstoria.Oid ))
                contexto.CicloDesenvEstorias.Attach( cicloDesenvEstoria );

            if(contexto.CicloDesenvEstorias.Existe( o => o.Oid == cicloDesenvEstoria.Oid ))
                contexto.Entry( cicloDesenvEstoria ).State = EntityState.Modified;
            else
                contexto.Entry( cicloDesenvEstoria ).State = EntityState.Added;

            contexto.SaveChanges();
        }
    }
}
