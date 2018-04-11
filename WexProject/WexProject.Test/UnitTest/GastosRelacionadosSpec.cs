using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Shared.DTOs.TotvsWex;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    ///     Teste da classe NotasFiscaisDao
    /// </summary>
    [TestClass]
    public class GastosRelacionadosSpec : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void DtoDeveConterTodosOsAtributosObrigatoriosQueSeraoTrafegados()
        {
            var dto = new NotaFiscalDto();

            //Abaixo estão os campos esperados pelo frontend
            Assert.AreEqual(0, dto.GastoId, "Gasto deve possuir GastoID");
            Assert.AreEqual(0, dto.CentroDeCustoId, "Gasto deve possuir CentroDeCustoId");
            Assert.AreEqual(null, dto.CentroDeCustoDesc, "Gasto deve possuir CentroDeCustoDesc");
            Assert.AreEqual(null, dto.RubricaId, "Gasto deve possuir RubricaId");
            Assert.AreEqual(null, dto.Descricao, "Gasto deve possuir Descricao");
            Assert.AreEqual(null, dto.HistoricoLancamento, "Gasto deve possuir HistoricoLancamento");
            Assert.AreEqual(null, dto.Justificativa, "Gasto deve possuir Justificativa");
            Assert.AreEqual(null, dto.Data, "Gasto deve possuir Data");
            Assert.AreEqual(0, dto.Valor, "Gasto deve possuir Valor");
        }
    }
}