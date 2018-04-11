using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;

namespace WexProject.Test.Helpers.BDD.Bind
{
    public class ProjetoBindHelper
    {
        public ProjetoBindHelper()
        {
        }

        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public int TotalDeCiclos { get; set; }
        public int RitmoDoTime { get; set; }


        public Projeto CriarProjeto() 
        {
            Projeto projeto = new Projeto()
            {
                TxNome = Nome,
                NbTamanhoTotal = Tamanho,
                NbCicloTotalPlan = TotalDeCiclos,
                NbRitmoTime = RitmoDoTime
            };
            return projeto;
        }
    }
}
