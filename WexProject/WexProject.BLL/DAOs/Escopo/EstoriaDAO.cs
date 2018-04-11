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
    public class EstoriaDAO
    {
        /// <summary>
        /// Método responsável por consultar as estórias de um projeto
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="oidProjeto">Oid do projeto</param>
        /// <returns>Estórias do projeto</returns>
        public static List<Estoria> ConsultarEstoriasPorProjeto( WexDb contexto, Guid oidProjeto )
        {
            List<Estoria> estorias = new List<Estoria>();

            if(oidProjeto != new Guid())
                estorias = contexto.Estorias.Where( o => o.Modulo.OidProjeto == oidProjeto  && !o.GCRecord.HasValue).ToList();

            return estorias;
        }

        /// <summary>
        /// Método responsável por pesquisar uma estoria pelo nome
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="tituloEstoria">Nome da Estoria</param>
        /// <returns>Estoria</returns>
        public static Estoria ConsultarEstoriaPorNome( WexDb contexto, string tituloEstoria )
        {
            return contexto.Estorias.FirstOrDefault( o => o.TxTitulo == tituloEstoria && !o.GCRecord.HasValue );
        }

        /// <summary>
        /// Método responsável por salvar uma estória
        /// </summary>
        /// <param name="contexto"></param>
        /// <param name="estoria"></param>
        public static void SalvarEstoria(WexDb contexto,Estoria estoria) 
        {
            if(!contexto.Estorias.ExisteLocalmente( o => o.Oid == estoria.Oid ))
                contexto.Estorias.Attach( estoria );

            if(contexto.Estorias.Existe( o => o.Oid == estoria.Oid ))
                contexto.Entry( estoria ).State = EntityState.Modified;
            else
                contexto.Entry( estoria ).State = EntityState.Added;

            contexto.SaveChanges();
        }
    }
}
