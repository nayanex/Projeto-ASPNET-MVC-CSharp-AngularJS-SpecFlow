using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Shared.DTOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.BOs.Custos;

namespace WexProject.Test.UnitTest.CustosAdministrativos
{
    [TestClass]
    public class RubricaMesesDaoTest : BaseEntityFrameworkTest
    {
        Projeto projeto1, projeto2, projeto3, projeto4;
        TipoRubrica tp1, tp2, tp3, tp4, tp5, tp6, tp7;

        private void InicializarCentrosCusto()
        {
            CentroCustoFactory.CriarCentrosDeCusto(1010, "ATC CONTROL");
            CentroCustoFactory.CriarCentrosDeCusto(2010, "LIFE TEST");
            CentroCustoFactory.CriarCentrosDeCusto(3010, "ANDROID TOYS");
        }

        private void InicializarClassesProjetos()
        {
            ClasseProjetoFactory.CriarClasseProjeto(1, "Projeto Patrocinado");
            ClasseProjetoFactory.CriarClasseProjeto(2, "Projeto sem Patrocínio");
            ClasseProjetoFactory.CriarClasseProjeto(3, "Setor");
        }

        private void InicializarTiposProjetos()
        {
            TipoProjetoFactory.CriarTipoProjeto(1, 1, "Projeto Base");
            TipoProjetoFactory.CriarTipoProjeto(2, 3, "Setor de Administração");
            TipoProjetoFactory.CriarTipoProjeto(3, 2, "Projeto Base");
        }

        private void InicializarProjetos()
        {
            projeto1 = ProjetoCustosFactory.CriarProjetoCustos("P1", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 02, 03), new DateTime(2014, 02, 03), 1);
            projeto2 = ProjetoCustosFactory.CriarProjetoCustos("P2", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 01, 02), new DateTime(2014, 02, 02), 1);
            projeto3 = ProjetoCustosFactory.CriarProjetoCustos("P3", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 06, 01), new DateTime(2014, 06, 01), 2);
            projeto4 = ProjetoCustosFactory.CriarProjetoCustos("P4", 100, CsProjetoSituacaoDomain.Concluido, new DateTime(2014, 07, 01), new DateTime(2014, 07, 01), 3);
        }

        private void InicializarAditivos()
        {
            AditivoFactory.CriarAditivo("Fase 1", 93802, 2, new DateTime(2014, 02, 03), new DateTime(2014, 03, 31), projeto1.Oid);
            AditivoFactory.CriarAditivo("Fase 2", 150000, 3, new DateTime(2014, 04, 01), new DateTime(2014, 06, 30), projeto1.Oid);
            AditivoFactory.CriarAditivo("Aditivo P2", 350000, 12, new DateTime(2014, 01, 02), new DateTime(2014, 12, 31), projeto2.Oid);
            AditivoFactory.CriarAditivo("Aditivo P4", 50100, 3, new DateTime(2014, 07, 01), new DateTime(2014, 09, 30), projeto4.Oid);
        }

        private void InicializarTiposRubricas()
        {
            tp1 = TipoRubricaFactory.CriarTipoRubrica(1, "Viagens", CsClasseRubrica.Desenvolvimento, 1);
            tp2 = TipoRubricaFactory.CriarTipoRubrica(2, "RH MDireta", CsClasseRubrica.Desenvolvimento, 1);
            tp3 = TipoRubricaFactory.CriarTipoRubrica(3, "RH GDC", CsClasseRubrica.Desenvolvimento, 1);
            tp4 = TipoRubricaFactory.CriarTipoRubrica(4, "Custo Fixo", CsClasseRubrica.Administrativo, 1);
            tp5 = TipoRubricaFactory.CriarTipoRubrica(5, "Taxa de Adm", CsClasseRubrica.Administrativo, 1);
            tp6 = TipoRubricaFactory.CriarTipoRubrica(6, "FACN", CsClasseRubrica.Administrativo, 1);
            tp7 = TipoRubricaFactory.CriarTipoRubrica(7, "Impostos", CsClasseRubrica.Administrativo, 1);
        }

        private void InicializarRubricas()
        {
            // -------- Inicio aditivo 3 ------------
            RubricaFactory.CriarRubrica(1, 3, 1000);
            RubricaFactory.CriarRubrica(2, 3, 1000);
            RubricaFactory.CriarRubrica(4, 3, 1000);
            RubricaFactory.CriarRubrica(4, 3, 1000);
            RubricaFactory.CriarRubrica(7, 3, 1000);
            // -------- Fim aditivo 3 ----------------

            // -------- Inicio aditivo 1 ------------
            RubricaFactory.CriarRubrica(1, 1, 1000);
            RubricaFactory.CriarRubrica(2, 1, 1000);
            RubricaFactory.CriarRubrica(3, 1, 1000);
            RubricaFactory.CriarRubrica(5, 1, 1000);
            RubricaFactory.CriarRubrica(6, 1, 1000);
            // -------- Fim aditivo 1 ----------------
        }

        private void InicializarRubricasMes()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;
            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 30);
        }

        [TestInitialize]
        public void InicializarDependenciasBanco()
        {
            InicializarCentrosCusto();
            InicializarProjetos();
            InicializarAditivos();
            InicializarClassesProjetos();
            InicializarTiposProjetos();
            InicializarTiposRubricas();
            InicializarRubricas();
            InicializarRubricasMes();
        }      

        [TestMethod]
        public void DeveRetornarUmaListaDeRubricasMesPorTipoRubricaEProjetoAnoMes()
        {
            var rubricas = RubricaMesDao.Instance.ConsultarRubricaMeses(tp4.TipoRubricaId, projeto2.Oid , 2014, 2);
            Assert.IsNotNull(rubricas, "Não deveria ser nulo");
            Assert.AreEqual(2, rubricas.Count, "Deveria trazer o tamanho da lista de rubricas");
            
        }

        [TestMethod]
        public void DeveAtualizarDespesaRealDeUmaRubricaMes()
        {
            DespesaRealDto despesa = new DespesaRealDto();

            var projetosBo = RubricaMesBo.Instance.ListarCustosProjetos(tp7.TipoRubricaId, 2014, 2);

            despesa = new DespesaRealDto
            {
                ProjetoOid = projetosBo[0].IdProjeto,
                TipoRubricaId = tp7.TipoRubricaId,
                Ano = 2014,
                Mes = CsMesDomain.Fevereiro,
                DespesaReal = 300
            };

            RubricaMesBo.Instance.SalvarDespesaReal(despesa);

            var projetoBo = RubricaMesBo.Instance.ListarCustosProjetos(tp7.TipoRubricaId, 2014, 2);
            Assert.AreEqual(300, projetoBo[0].DespesaReal);
        }

        [TestMethod]
        public void DeveSalvarDespesaRealQuandoNaoExistirRubricaMes()
        {
             DespesaRealDto despesa = new DespesaRealDto();

             var projetosBo = RubricaMesBo.Instance.ListarCustosProjetos(tp7.TipoRubricaId, 2014, 2);

            //Assert.AreEqual(30, projetosBo[0].DespesaReal);

            despesa = new DespesaRealDto
            {
                ProjetoOid = projetosBo[0].IdProjeto,
                TipoRubricaId = tp7.TipoRubricaId,
                Ano = 2014,
                Mes = CsMesDomain.Janeiro,
                DespesaReal = 300
            };

            RubricaMesBo.Instance.SalvarDespesaReal(despesa);

            var rubricas = RubricaMesDao.Instance.ConsultarRubricaMeses(tp7.TipoRubricaId, projetosBo[0].IdProjeto);

            Assert.AreEqual(2, rubricas.Count);

            var projetoBo = RubricaMesBo.Instance.ListarCustosProjetos(tp7.TipoRubricaId, 2014, 1);

            Assert.AreEqual(300, projetoBo[0].DespesaReal);
            
        }
    }

}
