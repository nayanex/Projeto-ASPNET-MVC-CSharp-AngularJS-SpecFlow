using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using WexProject.BLL.Models.Custos;

namespace WexProject.Test.Helpers.BDD.Bind
{
    [Binding]
    public class AditivoBindHelper
    {
        public string Descricao { get; set; }
        public string Projeto { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }
        public int QtdeMeses { get; set; }
        public decimal OrcamentoAprovado { get; set; }

    }
}
