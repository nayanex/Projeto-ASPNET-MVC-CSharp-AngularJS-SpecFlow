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
    public class CicloDesenvDAO
    {
        /// <summary>
        /// Método responsável por consultar os ciclos de desenvolvimento por projeto e retorná-los já ordenados pelo NbCiclo
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="oidProjeto">Oid do projeto que servirá para pesquisar os ciclos</param>
        /// <returns>Lista de ciclos do projeto</returns>
        public static List<CicloDesenv> ConsultarCiclosDesenvDoProjeto( WexDb contexto, Guid oidProjeto )
        {
            return contexto.CicloDesenvs.Where( o => o.Projeto == oidProjeto && !o.GCRecord.HasValue ).OrderBy( o => o.NbCiclo ).ToList();
        }

        /// <summary>
        /// Método responsável por criar ou alterar um ciclo
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="cicloDesenv">Ciclo a ser salvo</param>
        public static void SalvarCicloDesenv( WexDb contexto, CicloDesenv cicloDesenv )
        {
            if(!contexto.CicloDesenvs.ExisteLocalmente( o => o.Oid == cicloDesenv.Oid ))
                contexto.CicloDesenvs.Attach( cicloDesenv );

            if(contexto.CicloDesenvs.Existe( o => o.Oid == cicloDesenv.Oid ))
                contexto.Entry( cicloDesenv ).State = EntityState.Modified;
            else
                contexto.Entry( cicloDesenv ).State = EntityState.Added;

            contexto.SaveChanges();
        }
    }
}
