using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory
{
    public class TipoRubricaFactory : BaseEntityFrameworkTest
    {
        public static TipoRubrica CriarTipoRubrica(int tipoRubricaId, string txNome, CsClasseRubrica csClasse, int tipoProjetoId)
        {
            var tipoRubrica = new TipoRubrica { TipoRubricaId = tipoRubricaId, TxNome = txNome, CsClasse = csClasse, TipoProjetoId = tipoProjetoId};
            TipoRubricaDao.Instance.SalvarTipoRubrica(tipoRubrica);
            return tipoRubrica;
        }
    }
}
