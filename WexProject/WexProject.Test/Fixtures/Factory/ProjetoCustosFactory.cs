using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory
{
    public class ProjetoCustosFactory : BaseEntityFrameworkTest
    {
        public static Projeto CriarProjetoCustos(string txNome, decimal nbValor, CsProjetoSituacaoDomain CsSituacaoProjeto, DateTime dtInicioPlan, DateTime dtInicioReal, int centroCustoId)
        {
            var projeto = new Projeto { TxNome = txNome, NbValor = nbValor, CsSituacaoProjeto = CsSituacaoProjeto, DtInicioPlan = dtInicioPlan, DtInicioReal = dtInicioReal, CentroCustoId = centroCustoId };
            ProjetoDao.Instancia.SalvarProjeto(ContextFactoryManager.CriarWexDb(), projeto);
            return projeto;
        }
    }
}
