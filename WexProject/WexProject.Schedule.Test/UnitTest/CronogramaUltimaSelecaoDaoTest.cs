using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Schedule.Test.Fixtures.Factory;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.Outros;

namespace WexProject.Schedule.Test.UnitTest
{
    [TestClass]
    public class CronogramaUltimaSelecaoDaoTest : BaseEntityFrameworkTest
    {
        [TestMethod]
        public void SalvarUltimoCronogramaSelecionadoQuandoUsuarioNaoPossuirCronogramaAnteriorSelecionadoTest()
        {
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "S1", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            User usuario01 = CronogramaFactoryEntity.CriarUsuario( contexto, "anderson.lins", "Anderson", "Lins", "anderson.lins@fpf.br", true );

            User usuario02 = CronogramaFactoryEntity.CriarUsuario( contexto, "gabriel.mattos", "Gabriel", "Mattos", "gabriel.mattos@fpf.br", true );

            Cronograma cronograma01 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma01", situacaoPlanejamento1, DateTime.Now, DateTime.Now, true );

            Cronograma cronograma02 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma02", situacaoPlanejamento1, DateTime.Now, DateTime.Now, true );

            CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, usuario01.UserName, cronograma01.Oid );

            Cronograma cronogramaUsuario01 = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, usuario01.UserName );

            CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, usuario02.UserName, cronograma02.Oid );

            Cronograma cronogramaUsuario02 = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, usuario02.UserName );

            Assert.AreEqual( cronograma01, cronogramaUsuario01, "Deve ser o mesmo Cronograma, pois salvou como último cronograma selecionado para este usuário" );
            Assert.AreEqual( cronograma02, cronogramaUsuario02, "Deve ser o mesmo Cronograma, pois salvou como último cronograma selecionado para este usuário" );
        }

        [TestMethod]
        public void SalvarUltimoCronogramaSelecionadoQuandoUsuarioPossuirCronogramaAnteriorSelecinadoTest()
        {
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Situacao 01", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            User usuario01 = CronogramaFactoryEntity.CriarUsuario( contexto, "anderson.lins", "Anderson", "Lins", "anderson.lins@fpf.br", true );

            Cronograma cronograma01 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma01", situacaoPlanejamento1, DateTime.Now, DateTime.Now, true );

            Cronograma cronograma02 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma02", situacaoPlanejamento1, DateTime.Now, DateTime.Now, true );

            CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, usuario01.UserName, cronograma01.Oid);

            Cronograma cronogramaUsuario01 = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, usuario01.UserName );

            Assert.AreEqual( cronograma01, cronogramaUsuario01, "Deve ser o mesmo Cronograma, pois salvou como último cronograma selecionado para este usuário" );

            CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, usuario01.UserName, cronograma02.Oid );

            cronogramaUsuario01 = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, usuario01.UserName );

            Assert.AreEqual( cronograma02, cronogramaUsuario01, "Deve ser o mesmo Cronograma, pois salvou como último cronograma selecionado para este usuário" );
        }

        [TestMethod]
        public void ConsultarUltimoCronogramaSelecionadoQuandoUmUsuarioPossuirUmCronogramaAnteriormenteSelecinadoTest()
        {
            SituacaoPlanejamento situacaoPlanejamento1 = CronogramaFactoryEntity.CriarSituacaoPlanejamento( contexto, "Situacao 01", CsTipoSituacaoPlanejamento.Ativo, CsTipoPlanejamento.Execução, CsPadraoSistema.Sim, true );

            User usuario01 = CronogramaFactoryEntity.CriarUsuario( contexto, "anderson.lins", "Anderson", "Lins", "anderson.lins@fpf.br", true );

            Cronograma cronograma01 = CronogramaFactoryEntity.CriarCronograma( contexto, "Cronograma01", situacaoPlanejamento1, DateTime.Now, DateTime.Now, true );

            CronogramaUltimaSelecaoDao.SalvarUltimoCronogramaSelecionado( contexto, usuario01.UserName, cronograma01.Oid);

            Cronograma cronogramaUsuario01 = CronogramaUltimaSelecaoDao.ConsultarUltimoCronogramaSelecionado( contexto, usuario01.UserName );

            Assert.AreEqual( cronograma01, cronogramaUsuario01, "Deve ser o mesmo Cronograma, pois salvou como último cronograma selecionado para este usuário" );
        }
    }
}
