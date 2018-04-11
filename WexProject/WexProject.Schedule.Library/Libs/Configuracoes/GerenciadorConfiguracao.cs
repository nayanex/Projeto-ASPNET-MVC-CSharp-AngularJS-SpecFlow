using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Properties;

namespace WexProject.Schedule.Library.Libs.Configuracoes
{
    public static class GerenciadorConfiguracao
    {
        /// <summary>
        /// Salva o filtro customizado de situação planejamento
        /// </summary>
        public static void SalvarFiltroSituacaoCustom(string filtroCustom)
        {
            Settings.Default.CustomFilter = filtroCustom;
            Settings.Default.FiltroSituacao = (int)CsFiltroSituacaoPlanejamento.Personalizado;
            Settings.Default.Save();
        }

        /// <summary>
        /// Salvar filtro de situação planejamento
        /// </summary>
        /// <param name="filtro">filtro selecionado</param>
        /// <param name="filtroCustom">filtro costumizado</param>
        public static void SalvarFiltroSituacao(CsFiltroSituacaoPlanejamento filtro, string filtroCustom = "") 
        {
            if(filtro.Equals( CsFiltroSituacaoPlanejamento.Personalizado )) 
            {
                SalvarFiltroSituacaoCustom( filtroCustom );
                return;
            }

            Settings.Default.FiltroSituacao = (int)filtro;
            Settings.Default.Save();
        }

        /// <summary>
        /// Método para carregar um filtro de situação planejamento
        /// </summary>
        /// <param name="custom"></param>
        /// <returns></returns>
        public static CsFiltroSituacaoPlanejamento CarregarFiltroSituacaoPlanejamento(out string custom)
        {
            custom = Settings.Default.CustomFilter;
            return (CsFiltroSituacaoPlanejamento)Settings.Default.FiltroSituacao;
        }
    }
}
