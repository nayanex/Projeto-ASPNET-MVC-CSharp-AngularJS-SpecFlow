using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory
{
    public class RubricaFactory : BaseEntityFrameworkTest
    {
        public static Rubrica CriarRubrica(int tipoRubricaId, int aditivoId, decimal nbTotalPlanejado)
        {
            var rubrica = new Rubrica { TipoRubricaId = tipoRubricaId, AditivoId = aditivoId, NbTotalPlanejado = nbTotalPlanejado};
            RubricaDao.Instance.SalvarRubrica(rubrica);
            return rubrica;
        }

    }
}
