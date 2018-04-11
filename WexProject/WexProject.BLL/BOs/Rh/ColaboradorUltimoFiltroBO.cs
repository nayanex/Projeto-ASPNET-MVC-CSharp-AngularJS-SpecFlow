using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.BOs.RH
{
    public class ColaboradorUltimoFiltroBO
    {
        /// <summary>
        /// Método responsável por salvar o último projeto selecionado pelo colaborador
        /// </summary>
        /// <param name="contexto">Instância de conexão com o banco</param>
        /// <param name="oidColaborador">Oid do colaborador</param>
        /// <param name="oidProjeto">Oid do projeto</param>
        public static void SalvarUltimoProjetoSelecionado( Guid oidColaborador, Guid oidProjeto )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                ColaboradorUltimoFiltro colUltimoFiltro = ColaboradorDAO.ConsultarColaborador( oidColaborador, o => o.ColaboradorUltimoFiltro ).ColaboradorUltimoFiltro;

                colUltimoFiltro.OidUltimoProjetoSelecionado = oidProjeto;

                contexto.ColaboradorUltimoFiltroes.Attach( colUltimoFiltro );
                contexto.Entry<ColaboradorUltimoFiltro>( colUltimoFiltro ).State = System.Data.EntityState.Modified;
                contexto.SaveChanges();
            }
        }

        /// <summary>
        /// Método responsável por instaciar e salvar no banco um colaboradorUltimoFiltro padrão.
        /// </summary>
        /// <returns>Objeto criado e salvo no banco</returns>
        public static ColaboradorUltimoFiltro CriarColaboradorUltimoFiltroPadrao( WexDb contexto )
        {
            ColaboradorUltimoFiltro colaboradorUltimoFiltro = new ColaboradorUltimoFiltro()
            {
                OidUltimoProjetoSelecionado = null,
                LastSuperiorImediatoFilterPlanejamentoFerias = null,
                LastSituacaoFeriasFilterPlanejamentoFerias = null,
                OidLastSituacaoFilterSeot = new Guid(),
                OidLastUsuarioFilterSeot = new Guid(),
                LastPeriodoFilterPlanejamentoFerias = -1,
                LastSituacaoFilterPlanejamentoFerias = -1,
                OidLastEmpresaInstituicaoSEOT = null,
                OidLastTipoSolicitacaoSEOT = null
            };

            contexto.ColaboradorUltimoFiltroes.Add( colaboradorUltimoFiltro );
            contexto.SaveChanges();

            return colaboradorUltimoFiltro;
        }


        /// <summary>
        /// Método responsável por retornar o Oid do último projeto selecionado. de um colaborador
        /// </summary>
        /// <param name="oidColaborador">Oid do colaborador que se deseja obter o último projeto selecionado</param>
        /// <returns>Oid do projeto</returns>
        public static Guid ConsultarUltimoProjetoSelecionadoPorColaborador( Guid oidColaborador )
        {
            Projeto ultimoProjetoSelecionado = ColaboradorUltimoFiltroDAO.ConsultarUltimoProjetoSelecionadoPorColaborador( oidColaborador );

            if(ultimoProjetoSelecionado == null)
                return new Guid();

            return ultimoProjetoSelecionado.Oid;
        }
    }
}
