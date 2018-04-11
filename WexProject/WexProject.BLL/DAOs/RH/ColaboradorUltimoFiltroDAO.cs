using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.DAOs.RH
{
    public class ColaboradorUltimoFiltroDAO
    {
        /// <summary>
        /// Método responsável por consultar qual foi o último projeto selecionado por um colaborador
        /// </summary>
        /// <param name="oidColaborador">Oid do colaborador que se deseja obter o último projeto selecionado</param>
        /// <returns>último projeto selecionado</returns>
        public static Projeto ConsultarUltimoProjetoSelecionadoPorColaborador( Guid oidColaborador )
        {
            Projeto ultimoProjetoSelecionado;

            if(oidColaborador != new Guid())
            {
                ColaboradorUltimoFiltro colaboradorUltimoFiltro = ColaboradorDAO.ConsultarColaborador( oidColaborador, o => o.ColaboradorUltimoFiltro ).ColaboradorUltimoFiltro;

                if(colaboradorUltimoFiltro != null && colaboradorUltimoFiltro.OidUltimoProjetoSelecionado.HasValue && colaboradorUltimoFiltro.OidUltimoProjetoSelecionado.Value != Guid.Empty)
                {
                    ultimoProjetoSelecionado = ProjetoDao.Instancia.ConsultarProjetoPorOid( colaboradorUltimoFiltro.OidUltimoProjetoSelecionado.Value );

                    return ultimoProjetoSelecionado;
                }
            }

            return null;
        }

        /// <summary>
        /// Método responsável por consultar o ultimo filtro do colaborador
        /// </summary>
        /// <param name="contexto">Sessão do banco</param>
        /// <param name="oidColaboradorUltimoFiltro">Oid do ultimo filtro</param>
        /// <returns></returns>
        public static ColaboradorUltimoFiltro ConsultarColaboradorUltimoFiltro( WexDb contexto, Guid oidColaboradorUltimoFiltro )
        {
            return contexto.ColaboradorUltimoFiltroes.FirstOrDefault( o => o.Oid == oidColaboradorUltimoFiltro );
        }
    }
}
