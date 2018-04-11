using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.Test.Helpers.BDD.Bind
{
    public class EstoriaBindHelper
    {
        public string ComoUm { get; set; }
        public string Titulo { get; set; }
        public string Modulo { get; set; }
        public int Tamanho { get; set; }
        public string EmAnalise { get; set; }
        //public CsEstoriaDomain Situacao { get; set; }
        public CsSituacaoEstoriaCicloDomain Situacao { get; set; }
        
    }
}
