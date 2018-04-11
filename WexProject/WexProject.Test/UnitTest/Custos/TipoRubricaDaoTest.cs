using System;
using System.Data.Entity.Migrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Models.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Contexto;

namespace WexProject.Test.UnitTest.Custos
{
    [TestClass]
    public class TipoRubricaDaoTest : BaseEntityFrameworkTest
    {
        TipoRubrica tp1, tp2, tp3, tp4, tp5, tp6, tp7;

        private void InicializarClassesProjetos()
        {
            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                _db.ClassesProjetos.AddOrUpdate(
                    new ClasseProjeto { ClasseProjetoId = 1, TxNome = "Projeto Patrocinado" },
                    new ClasseProjeto { ClasseProjetoId = 2, TxNome = "Projeto sem Patrocínio" },
                    new ClasseProjeto { ClasseProjetoId = 3, TxNome = "Setor" }
                );

                _db.SaveChanges();
            }
        }

        private void InicializarTiposProjetos()
        {
            using (var _db = ContextFactoryManager.CriarWexDb())
            {
                _db.TiposProjetos.AddOrUpdate(
                    new TipoProjeto { TipoProjetoId = 1, ClasseProjetoId = 1, TxNome = "Projeto Base" },
                    new TipoProjeto { TipoProjetoId = 2, ClasseProjetoId = 3, TxNome = "Setor de Administração" },
                    new TipoProjeto { TipoProjetoId = 3, ClasseProjetoId = 2, TxNome = "Projeto Base" }
                );

                _db.SaveChanges();
            }
        }

        private void InicializarTiposRubricas()
        {
            // -------- Inicio instanciar tipos rubricas --------
            tp1 = new TipoRubrica
            {
                TipoRubricaId = 1,
                TxNome = "Viagens",
                CsClasse = CsClasseRubrica.Desenvolvimento,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp1);

            tp2 = new TipoRubrica
            {
                TipoRubricaId = 2,
                TxNome = "RH MDireta",
                CsClasse = CsClasseRubrica.Desenvolvimento,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp2);

            tp3 = new TipoRubrica
            {
                TipoRubricaId = 3,
                TxNome = "RH GDC",
                CsClasse = CsClasseRubrica.Desenvolvimento,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp3);

            tp4 = new TipoRubrica
            {
                TipoRubricaId = 4,
                TxNome = "Custo Fixo",
                CsClasse = CsClasseRubrica.Administrativo,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp4);

            tp5 = new TipoRubrica
            {
                TipoRubricaId = 5,
                TxNome = "Taxa de Adm",
                CsClasse = CsClasseRubrica.Administrativo,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp5);

            tp6 = new TipoRubrica
            {
                TipoRubricaId = 6,
                TxNome = "FACN",
                CsClasse = CsClasseRubrica.Administrativo,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp6);

            tp7 = new TipoRubrica
            {
                TipoRubricaId = 7,
                TxNome = "Impostos",
                CsClasse = CsClasseRubrica.Administrativo,
                TipoProjetoId = 1
            };

            TipoRubricaDao.Instance.SalvarTipoRubrica(tp7);
        }

        private void InicializarCenario()
        {
            InicializarClassesProjetos();
            InicializarTiposProjetos();
            InicializarTiposRubricas();
        }

        [TestMethod]
        public void DeveRetornarUmaListaDeTiposRubricas()
        {
            InicializarCenario();

            var tiposRubricas = TipoRubricaDao.Instance.ConsultarTiposRubricas();

            Assert.AreEqual(7, tiposRubricas.Count);
        }

        [TestMethod]
        public void DeveRetornarUmaListaDeTiposRubricasPorClasse()
        {
            InicializarCenario();

            var tiposRubricas = TipoRubricaDao.Instance.ConsultarTiposRubricas(CsClasseRubrica.Administrativo);

            Assert.AreEqual(4, tiposRubricas.Count);
        }
    }
}
