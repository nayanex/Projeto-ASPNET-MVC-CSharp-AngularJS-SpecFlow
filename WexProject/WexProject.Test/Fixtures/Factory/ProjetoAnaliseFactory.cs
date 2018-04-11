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
    class ProjetoAnaliseFactory : BaseEntityFrameworkTest
    {
        public static Projeto CriarProjetoCustos(string txNome, CsProjetoSituacaoDomain CsSituacaoProjeto, DateTime dtInicioPlan, DateTime dtInicioReal, DateTime? DtTerminoReal)
        {
            var projeto = new Projeto { TxNome = txNome, CsSituacaoProjeto = CsSituacaoProjeto, DtInicioPlan = dtInicioPlan, DtInicioReal = dtInicioReal, DtTerminoReal = DtTerminoReal };
            ProjetoDao.Instancia.SalvarProjeto(ContextFactoryManager.CriarWexDb(), projeto);
            return projeto;
        }
    }
}
