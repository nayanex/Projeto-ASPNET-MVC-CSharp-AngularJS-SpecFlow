using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.Helpers.BDD.Bind
{
    public class ProjetoCustosBindHelper
    {
        public string Nome { get; set; }
        public DateTime InicioPlanejado { get; set; }
        public DateTime InicioReal { get; set; }
        public CsProjetoSituacaoDomain Situacao { get; set; }

        public Projeto ToProjeto() 
        {
            return new Projeto()
            {
                TxNome = Nome,
                DtInicioPlan = InicioPlanejado,
                DtInicioReal = InicioReal,
                CsSituacaoProjeto = Situacao
            };
        }
    }
}
