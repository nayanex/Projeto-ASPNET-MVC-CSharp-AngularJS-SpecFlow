using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.BOs.RH;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.RH;
using WexProject.Test.Fixtures.Factory;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class ColaboradorUltimoFiltroBOTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void DeveAlterarUltimoProjetoSelecionadoQuandoUsuarioSelecionarOutroProjetoTest()
        {
            ColaboradorUltimoFiltro colaboradorUltimoFiltro = ColaboradorUltimoFiltroBO.CriarColaboradorUltimoFiltroPadrao( contexto );

            Colaborador colaborador = ColaboradorFactoryEntity.CriarColaborador( contexto, "anderson.lins", colaboradorUltimoFiltro.Oid );

            Projeto projeto = ProjetoFactoryEntity.Criar( contexto, 10, "ProjetoTest" );

            Guid oidUltimoProjetoSelecionadoAntigo = ColaboradorUltimoFiltroBO.ConsultarUltimoProjetoSelecionadoPorColaborador( colaborador.Oid );

            ColaboradorUltimoFiltroBO.SalvarUltimoProjetoSelecionado( colaborador.Oid, projeto.Oid );

            Guid oidUltimoProjetoSelecionadoAtualizado = ColaboradorUltimoFiltroBO.ConsultarUltimoProjetoSelecionadoPorColaborador( colaborador.Oid );

            Assert.AreNotEqual( oidUltimoProjetoSelecionadoAntigo, oidUltimoProjetoSelecionadoAtualizado, "Oids deveriam ser diferentes, caso contrário não salvou a alteração." );
        }
    }
}
