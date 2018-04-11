using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.RH;

namespace WexProject.BLL.BOs.RH
{
    public class ColaboradorPeriodoAquisitivoBO
    {
        /// <summary>
        /// Método responsável por criar um ColaboradorPeriodoAquisitivo Padrão
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="novoColaborador">Colaborador que foi criado recentemente</param>
        /// <param name="dtAdmissao">Data de admissao do novo coloborador</param>
        /// <returns>Objeto ColaboradorPeriodoAquisitivo criado</returns>
        public static ColaboradorPeriodoAquisitivo CriarPeridoAquisitivoParaColaborador( WexDb contexto, Colaborador novoColaborador, DateTime dtAdmissao )
        {
            ColaboradorPeriodoAquisitivo colPeriodoAquisitivo = new ColaboradorPeriodoAquisitivo()
            {
                OidColaborador = novoColaborador.Oid,
                DtInicio = dtAdmissao,
                DtTermino = dtAdmissao.AddYears( 1 ),
                NbFeriasPlanejadas = 0,
                DtMaxima = dtAdmissao.AddYears( 2 )
            };

            contexto.ColaboradorPeriodoAquisitivoes.Add( colPeriodoAquisitivo );
            contexto.SaveChanges();

            return colPeriodoAquisitivo;
        }
    }
}
