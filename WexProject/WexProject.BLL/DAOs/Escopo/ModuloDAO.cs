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
    public class ModuloDAO
    {
        /// <summary>
        /// Get que retorna os modulos por projeto
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <returns>Modulos por projeto</returns>
        public static List<Modulo> ConsultarModulos( WexDb contexto, Guid oidProjeto )
        {
            List<Modulo> modulos = new List<Modulo>();

            if(oidProjeto != new Guid())
                modulos = contexto.Modulos.Where( o => o.OidProjeto == oidProjeto && o.ModuloPai == null && !o.GCRecord.HasValue ).OrderBy( o => o.TxNome ).ToList();

            return modulos;
        }

        /// <summary>
        /// Método responsável por buscar um Módulo por nome
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="nome">Nome do módulo</param>
        /// <returns></returns>
        public static Modulo ConsultarModuloPorNome ( WexDb contexto, string nome )
        {
            return contexto.Modulos.FirstOrDefault( o => o.TxNome == nome &&  !o.GCRecord.HasValue );
        }

        /// <summary>
        /// Método para salvar um módulo na base
        /// </summary>
        /// <param name="contexto">contexto de conexão com o banco</param>
        /// <param name="modulo">modulo a ser salvo</param>
        public static void SalvarModulo( WexDb contexto, Modulo modulo )
        {
            if(!contexto.Modulos.ExisteLocalmente(o=>o.Oid == modulo.Oid))
                contexto.Modulos.Attach( modulo );

            if(contexto.Modulos.Existe( o => o.Oid == modulo.Oid ))
                contexto.Entry( modulo ).State = EntityState.Modified;
            else 
                contexto.Entry( modulo ).State = EntityState.Added;
            
            contexto.SaveChanges();
        }
    }
}
