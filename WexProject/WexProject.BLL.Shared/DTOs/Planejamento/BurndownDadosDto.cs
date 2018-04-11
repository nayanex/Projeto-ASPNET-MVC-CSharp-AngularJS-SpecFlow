using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Shared.Domains.Planejamento;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    /// <summary>
    /// Classe que conterá a informação de um dia no gráfico de burndown do cronograma
    /// </summary>
    public class BurndownDadosDto
    {
        /// <summary>
        /// Dia o qual os dados representam
        /// </summary>
        public DateTime Dia { get; set; }

        /// <summary>
        /// Somatório das horas calculadas para o determinado dia
        /// </summary>
        public double? QtdeHoras { get; set; }

        /// <summary>
        /// Tipo da informação para o burndown
        /// </summary>
        public CsTipoBurndown CsTipo { get; set; }
    }
}
