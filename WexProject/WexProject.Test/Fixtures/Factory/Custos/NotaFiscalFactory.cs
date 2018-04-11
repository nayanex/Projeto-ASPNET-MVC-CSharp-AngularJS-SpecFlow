using System;
using WexProject.BLL.DAOs.TotvsWex;
using WexProject.BLL.Models.Custos;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory.Custos
{
    public class NotaFiscalFactory : BaseEntityFrameworkTest
    {
        public static NotaFiscal CriarNotaFiscal(DateTime data, int idCentroCusto,
            string descricao, decimal valor, int chaveImportacao)
        {
            var notaFiscal = new NotaFiscal
            {
                Data = data,
                CentroDeCustoId = idCentroCusto,
                Descricao = descricao,
                Valor = valor,
                ChaveImportacao = chaveImportacao
            };

            NotasFiscaisDao.SalvarNotaFiscal(notaFiscal);
            return notaFiscal;
        }
    }
}