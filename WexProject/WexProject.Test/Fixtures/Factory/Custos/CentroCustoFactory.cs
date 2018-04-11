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
    public class CentroCustoFactory : BaseEntityFrameworkTest
    {
        public static CentroCusto CriarCentrosDeCusto(int codigo, string nome)
        {
            var centroCusto = new CentroCusto {Codigo = codigo, Nome = nome};
            CentroCustoDao.Instance.SalvarCentroCusto(centroCusto);
            return centroCusto;
        }
    }
}
