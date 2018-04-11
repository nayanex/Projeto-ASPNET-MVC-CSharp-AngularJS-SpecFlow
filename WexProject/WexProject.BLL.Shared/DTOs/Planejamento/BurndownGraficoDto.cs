using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    /// <summary>
    /// Classe que conterá as informações para o gráfico de burndown do cronograma
    /// </summary>
    public class BurndownGraficoDto
    {
        /// <summary>
        /// Quantidade de horas em desvio
        /// </summary>
        public double Desvio { get; set; }

        /// <summary>
        /// Lista de informações diárias para criação do gráfico de burndown (Planejado/Realizado)
        /// </summary>
        public List<BurndownDadosDto> DadosBurndown { get; set; }

        public BurndownGraficoDto()
        {
            DadosBurndown = new List<BurndownDadosDto>();
        }
    }
}
