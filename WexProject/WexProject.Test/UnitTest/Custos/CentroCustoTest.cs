using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.BOs.Custos;
using WexProject.BLL.Shared.DTOs.Projeto;

namespace WexProject.Test.UnitTest.Custos
{
    [TestClass]
    public class CentroCustoTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void DeveCadastrarUmNovoCentroDeCusto()
        {
            CentroCusto centroCusto = new CentroCusto
            {
                Codigo = 1010,
                Nome = "ATC Control"
            };

            CentroCustoDao.Instance.SalvarCentroCusto(centroCusto);
            var centrosCusto = CentroCustoDao.Instance.ListarCentrosCustos();

            Assert.AreEqual(1, centrosCusto.Count);
        }

        [TestMethod]
        public void DeveAtualizarUmCentroDeCusto()
        {
            CentroCusto centroCusto = new CentroCusto
            {
                Codigo = 1010,
                Nome = "ATC Control"
            };
            CentroCustoDao.Instance.SalvarCentroCusto(centroCusto);

            CentroCustoDto centroCusto2 = new CentroCustoDto
            {
                Codigo = 1010,
                Nome = "3M"
            };
            CentroCustoBo.Instance.SalvarCentroCusto(centroCusto2);

            var cc = CentroCustoDao.Instance.ConsultarCentroCustoPorCodigo(1010);

            cc.Nome = "ATC";

            CentroCustoDao.Instance.SalvarCentroCusto(cc);

            var centro = CentroCustoDao.Instance.ConsultarCentroCustoPorCodigo(1010);

            Assert.AreEqual("ATC", centro.Nome);
            Assert.AreEqual(1, centro.CentroCustoId);
        }
    }
}
