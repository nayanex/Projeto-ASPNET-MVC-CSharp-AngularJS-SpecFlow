using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.BOs.Geral;
using WexProject.Test.Fixtures.Factory;
using System.Collections.Generic;
using System.Linq;
using WexProject.BLL.BOs.Custos;

namespace WexProject.Test.UnitTest
{
    [TestClass]
    public class ProjetoCustosTest : BaseEntityFrameworkTest
    {
        Projeto projeto1, projeto2, projeto3, projeto4;
        TipoRubrica tp1, tp2, tp3, tp4, tp5, tp6, tp7;
       
        private void InicializarCentrosCusto()
        {
            CentroCustoFactory.CriarCentrosDeCusto(1010, "ATC CONTROL");
            CentroCustoFactory.CriarCentrosDeCusto(2010, "LIFE TEST");
            CentroCustoFactory.CriarCentrosDeCusto(3010, "ANDROID TOYS");
        }

        private void InicializarProjetos()
        {
            projeto1 = ProjetoCustosFactory.CriarProjetoCustos("P1", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 02, 03), new DateTime(2014, 02, 03), 1);
            projeto2 = ProjetoCustosFactory.CriarProjetoCustos("P2", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 01, 02), new DateTime(2014, 02, 02), 1);
            projeto3 = ProjetoCustosFactory.CriarProjetoCustos("P3", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 06, 01), new DateTime(2014, 06, 01), 2);
            projeto4 = ProjetoCustosFactory.CriarProjetoCustos("P4", 100, CsProjetoSituacaoDomain.EmAndamento, new DateTime(2014, 07, 01), new DateTime(2014, 07, 01), 3);
        }

        private void InicializarAditivos()
        {

            AditivoFactory.CriarAditivo("Fase 1", 93802, 2, new DateTime(2014, 06, 01), new DateTime(2015, 01, 31), projeto1.Oid);
            AditivoFactory.CriarAditivo("Fase 2", 150000, 3, new DateTime(2014, 04, 01), new DateTime(2014, 06, 30), projeto1.Oid);
            AditivoFactory.CriarAditivo("Aditivo P2", 350000, 12, new DateTime(2014, 01, 02), new DateTime(2015, 12, 31), projeto2.Oid);
            AditivoFactory.CriarAditivo("Aditivo P4", 50100, 3, new DateTime(2014, 07, 01), new DateTime(2014, 09, 30), projeto4.Oid);    

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
            RubricaFactory.CriarRubrica(5, 3, 1000);
            RubricaFactory.CriarRubrica(7, 3, 1000);
            // -------- Fim aditivo 3 -----------

            // ------ Inicio aditivo 1 ----------
            RubricaFactory.CriarRubrica(1, 1, 1000);
            RubricaFactory.CriarRubrica(2, 1, 1000);
            RubricaFactory.CriarRubrica(3, 1, 1000);
            RubricaFactory.CriarRubrica(5, 1, 1000);
            RubricaFactory.CriarRubrica(6, 4, 1000);
            // ------- Fim aditivo 1 ---------

        }

        [TestInitialize]
        public void InicializarDependenciasBanco()
        {
            InicializarCentrosCusto();
            InicializarProjetos();
            InicializarClassesProjetos();

            InicializarTiposProjetos();
            InicializarAditivos();
            InicializarTiposRubricas();
            InicializarRubricas();
        }
        
        [TestMethod]
        public void DeveListarProjetosRubricasAdministrativas()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;

            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 20);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 40);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 50);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 60);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 70);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 80);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 90);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 100);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 110);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 120);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 130);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 140);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 150);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 160);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 170);

            // Testando ProjetoDAO
            var projetos = ProjetoDao.Instancia.ConsultarProjetosPorTipoRubrica(tp5.TipoRubricaId, 2015, 1);

            Assert.IsNotNull(projetos, "Deveria trazer uma lista de projetos");
            Assert.AreEqual(2, projetos.Count);
            Assert.AreEqual(projetos[0].TxNome, projeto1.TxNome, "Deveria trazer o projeto P2");
            Assert.AreEqual(projetos[1].TxNome, projeto2.TxNome, "Deveria trazer o projeto P2");
                       
            // Testando ProjetoBo
            var projetosBo = RubricaMesBo.Instance.ListarCustosProjetos(tp7.TipoRubricaId, 2014, 2);

            Assert.IsNotNull(projetosBo, "Deveria listar os projetos");
            Assert.AreEqual(1, projetosBo.Count);
            Assert.AreEqual(projetosBo[0].NomeProjeto, projeto2.TxNome, "Deveria trazer o projeto P2");
            
        }

        [TestMethod]
        public void DeveMostrarOrcamentoAprovadoDeUmProjetoDeRubricaAdministrativa()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;

            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 20);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 40);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 50);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 60);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 70);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 80);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 90);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 100);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 110);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 120);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 130);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 140);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 150);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 160);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 170);


            var projetos = RubricaMesBo.Instance.ListarCustosProjetos(4, 2014, 2);

            Assert.IsNotNull(projetos, "Deveria trazer uma lista de projetos");
            Assert.AreEqual(1, projetos.Count);
            Assert.AreEqual(100, projetos[0].OrcamentoAprovado);          
        }

        [TestMethod]
        public void DeveMostrarDespesaRealDeUmProjetoDeRubricaAdministrativa()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;

            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 20);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 40);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 50);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 60);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 70);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 80);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 90);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 100);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 110);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 120);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 130);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 140);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 150);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 160);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 170);

            var projetos = RubricaMesBo.Instance.ListarCustosProjetos(4, 2014, 2);

            Assert.IsNotNull(projetos, "Deveria trazer uma lista de projetos");
            Assert.AreEqual(1, projetos.Count);
            Assert.AreEqual(60, projetos[0].DespesaReal);
        }

        [TestMethod]
        public void DeveMostrarSaldoDisponivelDeUmProjeto()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;

            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 20);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 40);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 50);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 60);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 70);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 80);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 90);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 100);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 110);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 120);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 130);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 140);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 150);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 160);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 170);

            var projetos = RubricaMesBo.Instance.ListarCustosProjetos(4, 2014, 2);

            Assert.IsNotNull(projetos);
            Assert.AreEqual(1, projetos.Count);
            Assert.AreEqual(150, projetos[0].SaldoDisponivel);

            projetos = RubricaMesBo.Instance.ListarCustosProjetos(1, 2014, 2);
            Assert.IsNotNull(projetos);
            Assert.AreEqual(1, projetos.Count);
            Assert.AreEqual(100, projetos[0].SaldoDisponivel);
        }

        [TestMethod]
        public void ConsultarValorOuRetornarNulo_QuandoValorDeDespesaRealForVazio_RetornarValorNulo()
        {
            const bool PossuiGastosRelacionados = false;
            const int nbAno = 2014;
            decimal? despesaReal = null;

            RubricaMesFactory.CriarRubricaMes(1, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 20);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 30);
            RubricaMesFactory.CriarRubricaMes(2, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 40);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Janeiro, nbAno, PossuiGastosRelacionados, 100, 50);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, despesaReal);
            RubricaMesFactory.CriarRubricaMes(3, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 70);
            RubricaMesFactory.CriarRubricaMes(4, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 80);
            RubricaMesFactory.CriarRubricaMes(5, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 90);
            RubricaMesFactory.CriarRubricaMes(6, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 100);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 110);
            RubricaMesFactory.CriarRubricaMes(7, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 120);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 130);
            RubricaMesFactory.CriarRubricaMes(8, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 140);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Fevereiro, nbAno, PossuiGastosRelacionados, 100, 150);
            RubricaMesFactory.CriarRubricaMes(9, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 160);
            RubricaMesFactory.CriarRubricaMes(10, CsMesDomain.Marco, nbAno, PossuiGastosRelacionados, 100, 170);

            var projetos = RubricaMesBo.Instance.ListarCustosProjetos(4, 2014, 2);

            Assert.IsNotNull(projetos, "Deveria trazer uma lista de projetos");
            Assert.AreEqual(1, projetos.Count);

            Assert.IsNull(projetos[0].DespesaReal);

            Assert.IsNull(despesaReal);

        }
        
    }
}
