using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Geral;

namespace WexProject.BLL.Shared.DTOs.Planejamento
{
    public class InicializadorEstimativaDto
    {
        public DateTime DataEstimativa { get; set; }
        public TimeSpan HoraInicialEstimativa { get; set; }
        public DiaTrabalhoDto DiaAtual { get; set; }

        public InicializadorEstimativaDto()
        {
        }

        /// <summary>
        /// Instanciar um objeto do tipo InicializadorEstimativa
        /// </summary>
        /// <param name="dataEstimativa">Data calculara para inicio de estimativa da tarefa</param>
        /// <param name="horaInicial">hora selecionada para inicio da estimativa da tarefa</param>
        public InicializadorEstimativaDto(DateTime dataEstimativa,TimeSpan horaInicial)
        {
            DataEstimativa = dataEstimativa;
            HoraInicialEstimativa = horaInicial;
        }
    }
}
