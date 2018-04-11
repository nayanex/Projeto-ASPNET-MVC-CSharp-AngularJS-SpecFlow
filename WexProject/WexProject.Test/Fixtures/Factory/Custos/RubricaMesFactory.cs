using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory
{
    public class RubricaMesFactory : BaseEntityFrameworkTest
    {
        public static RubricaMes CriarRubricaMes (int rubricaId, CsMesDomain csMesDomain, int nbAno, bool PossuiGastosRelacionados, decimal? nbPlanejado, decimal? nbGasto)
        {
            var rubricaMes = new RubricaMes { RubricaId = rubricaId, CsMes = csMesDomain, NbAno = nbAno, PossuiGastosRelacionados = PossuiGastosRelacionados, NbPlanejado = nbPlanejado, NbGasto = nbGasto };
            RubricaMesDao.Instance.SalvarRubricaMes(rubricaMes);
            return rubricaMes;       
        }
    }
}
