using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using WexProject.BLL.Models.Escopo;
using WexProject.Library.Libs.Xaf;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe ModuloTest
    /// </summary>
    [TestClass]
    public class ModuloTest : BaseTest
    {
        /// <summary>
        /// método SalvarModulo
        /// </summary>
        [TestMethod]
        public void SalvarModulo()
        {
            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto projeto2 = ProjetoFactory.Criar(SessionTest, 0, "", true);

            /**
             * Cenário 1: Serão criados 2 modulos para um projeto e o wex
             * deverá criar os IDs incrementados em sequencia.
             */

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);

            Assert.AreEqual("01", modulo.TxID);

            modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);

            Assert.AreEqual("02", modulo.TxID);

            Assert.AreEqual("01", ModuloFactory.Criar(SessionTest, projeto2, "", true).TxID);
            Assert.AreEqual("02", ModuloFactory.Criar(SessionTest, projeto2, "", true).TxID);
        }
        /// <summary>
        /// método SalvarModuloQueJaPossuiModulosEmCodigo
        /// </summary>
        [TestMethod]
        public void SalvarModuloQueJaPossuiModulosEmCodigo()
        {

            /**
             * Cenário 2: Serão criados 3 modulos para o projeto e em seguida será deletado o segundo modulo.
             * Após isso, será criado mais 1 modulo, que o wex deverá criar como RF_4.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);

            Assert.AreEqual("01", modulo.TxID, "O ID do modulo deveria ser 1");

            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Assert.AreEqual("02", modulo1.TxID, "O ID do modulo deveria ser 2");

            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Assert.AreEqual("03", modulo2.TxID, "O ID do modulo deveria ser 3");

            //Criar o delete Rf_2

            modulo1.Delete();

            Modulo modulo4 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Assert.AreEqual("04", modulo4.TxID, "O ID do modulo deveria ser 4");
        }
        /// <summary>
        /// método SalvarModuloComModuloPai
        /// </summary>
        [TestMethod]
        public void SalvarModuloComModuloPai()
        {

            /**
             * Cenário 3: Será criado um módulo para o projeto.
             * Em seguida, será criado outro módulo, o qual selecionará o primeiro módulo como pai
             * O wex deverá salvar corretamente, respeitando a hierarquia da nomenclatura dos ID's
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo, "", true);
            Assert.AreEqual("01.01", moduloPai.TxID, "O ID do modulo deveria ser 01.01");

        }
        /// <summary>
        /// método SalvarModuloTerceiroNivel
        /// </summary>
        [TestMethod]
        public void SalvarModuloTerceiroNivel()
        {

            /**
             * Cenário 5: Serão criados 4 módulos, sendo que sucessor sempre terá o antecessor como módulo pai.
             * O sistema deverá salvar o ultimo módulo com terceiro nivel de descêndencia.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo, "", true);
            Modulo moduloPai2 = ModuloFactory.CriarModuloFilho(SessionTest, moduloPai, "", true);
            Modulo moduloPai3 = ModuloFactory.CriarModuloFilho(SessionTest, moduloPai2, "", true);

            Assert.AreEqual("01.01.01.01", moduloPai3.TxID, "O ID do modulo deveria ser 01.01.01.01");

        }
        /// <summary>
        /// método TestarAtualizarPontosDeUmModulo
        /// </summary>
        //[TestMethod]
        public void TestarAtualizarPontosDeUmModulo()
        {

            /**
             * Cenário 6: Serão atualizados os pontos de um módulo e o sistema deverá registrar essa atualização
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.CriarTeste(SessionTest, projeto1, "", 20, 3, 72, 38, 5, 132, true);

            Assert.AreEqual(modulo._NbPercNaoIniciado, 15, 1);
            Assert.AreEqual(modulo._NbPercPronto, 28, 8);
            Assert.AreEqual(modulo._NbPercEmAnalise, 1, 5);
            Assert.AreEqual(modulo._NbPercEmDesenv, 54, 5);
            Assert.AreEqual(modulo._NbPercDesvio, 3, 8);
            Assert.AreEqual(modulo._TxSituacao, "Não Iniciado: " + modulo._NbPercNaoIniciado + "%; Pronto: " + modulo._NbPercPronto + "%; Em Análise: "
            + modulo._NbPercEmAnalise + "%; Em Desenv: " + modulo._NbPercEmDesenv + "%; Desvio: " + modulo._NbPercDesvio + "%");


            Modulo modulo2 = ModuloFactory.CriarTeste(SessionTest, projeto1, "", 0, 22, 72, 38, 0, 132, true);
            Assert.AreEqual(modulo2._NbPercEmAnalise, 16, 7);
            Assert.AreEqual(modulo._TxSituacao, "Não Iniciado: " + modulo._NbPercNaoIniciado + "%; Pronto: " + modulo._NbPercPronto + "%; Em Análise: "
            + modulo._NbPercEmAnalise + "%; Em Desenv: " + modulo._NbPercEmDesenv + "%; Desvio: " + modulo._NbPercDesvio + "%");
        }
        /// <summary>
        /// método SalvarEsforcoParaOModuloFilhoEExcluirOEsforcoDoModuloPai
        /// </summary>
        [TestMethod]
        public void SalvarEsforcoParaOModuloFilhoEExcluirOEsforcoDoModuloPai()
        {

            /**
             * Cenário 7: Será criado um módulo "A" com um esforço. Em seguida, será criado um módulo filho "B" que terá o seu proprio esforço.
             * O sistema deverá setar como nulo o esforço do módulo "A" e utilizar o esforço do módulo "B".
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            modulo.NbEsforcoPlanejado = 10;

            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo, "", true);
            moduloPai.NbEsforcoPlanejado = 50;
            Assert.AreEqual(modulo.NbEsforcoPlanejado, (UInt32)0);

        }
        /// <summary>
        /// método SalvarVariosModulosParaUmProjeto
        /// </summary>
        [TestMethod]
        public void SalvarVariosModulosParaUmProjeto()
        {

            /**
             * Cenário 8: Será criado um projeto com 300 pontos.
             * Após isso, será criado um módulo cujo esforço será de 50% do valor dos pontos.
             * O sistema deverá colocar o valor de 150 para o esforço.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 10, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            modulo.NbEsforcoPlanejado = 10;

            Assert.AreEqual((UInt32)10, modulo.NbEsforcoPlanejado);

            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            modulo2.NbEsforcoPlanejado = 10;

            Assert.AreEqual((UInt32)10, modulo2.NbEsforcoPlanejado);

            Modulo modulo3 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            modulo3.NbEsforcoPlanejado = 10;

            Assert.AreEqual((UInt32)10, modulo3.NbEsforcoPlanejado);

        }
        /// <summary>
        /// método SalvarModuloComEsforcoAcimaDe100
        /// </summary>
        [TestMethod]
        public void SalvarModuloComEsforcoAcimaDe100()
        {
            /**
             * Cenário 9: Será criado um projeto A. Em seguida serão criados 2 modulos: O Modulo A e o B.
             * O Modulo A terá um esforço de 50% e o Modulo B terá um esforço de 51%.
             * O sistema não poderá permitir que os modulos sejam salvos até que a somatória dos esforços seja <= 100.
             */

            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 300, "", true);
            Modulo moduloA = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            moduloA.NbEsforcoPlanejado = 50;
            moduloA.Save();

            Modulo moduloB = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            moduloB.NbEsforcoPlanejado = 51;
            moduloB.Save();

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(moduloB,
            "NaoPermitirSalvarSeOsModulosDoProjetoTiveremASomaDosEsforcosMaiorQue100", DefaultContexts.Save));
        }
        /// <summary>
        /// método EditarModuloPai
        /// </summary>
        [TestMethod]
        public void EditarModuloPai()
        {

            /**
             * Cenário 10: Será criado um módulo tendo um módulo pai e será salvo no sistema.
             * Em seguida será editado o módulo pai desse módulo e o sistema deverá mudar o ID do módulo de forma correta.
             */


            Projeto projeto1 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto1, "", true);
            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo, "", true);

            Assert.AreEqual("01.01", moduloPai.TxID, "O ID do módulo deveria ser 01.01");

            moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo2, "", true);

            Assert.AreEqual("02.01", moduloPai.TxID, "O ID do módulo deveria ser 02.01");

        }
        /// <summary>
        /// método TestarCalcularPercentualComBasePontuacao
        /// </summary>
        [TestMethod]
        public void TestarCalcularPercentualComBasePontuacao()
        {


            //Passo 1         
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "nome projeto", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo3 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            modulo1.NbPontosTotal = 40;
            modulo2.NbPontosTotal = 40;
            modulo3.NbPontosTotal = 15;
            modulo1.Save();
            modulo2.Save();

            // (Total de pontos do módulo * 100) / Total de pontos do projeto
            Assert.AreEqual((UInt32)40, modulo1.NbEsforcoPlanejado, "deveria ser calculado o esforço com base número de pontos prlanejados");
            Assert.AreEqual((UInt32)40, modulo2.NbEsforcoPlanejado, "deveria ser calculado o esforço com base número de pontos prlanejados");
            //Passo 2

            modulo3.Save();
            Assert.AreEqual((UInt32)15, modulo3.NbEsforcoPlanejado, "deveria ser calculado o esforço com base número de pontos prlanejados");


            //Passo 3
            //teste do método RnVerificaTotalPontosUltrapassaProjeto()

            Modulo modulo4 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            modulo4.NbPontosTotal = 10;

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modulo4,
            "SalvarModulosProjetoSomaPontosPlanejadosMaiorTamanhoProjeto", DefaultContexts.Save));

        }
        /// <summary>
        /// método 
        /// </summary>
        [TestMethod]
        public void TestarSituacaoProjeto()
        {

            //teste para verificar os métodos criados para calcular a
            //situação do projeto ao qual esse módulo pertence

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "nome projeto", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo3 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            modulo1.NbPontosTotal = 40;
            modulo2.NbPontosTotal = 40;
            modulo3.NbPontosTotal = 15;
            modulo1.Save();
            modulo2.Save();
            modulo3.Save();

            Assert.AreEqual(100, modulo1._NbPontosTotalProjeto, "O total dos pontos do projeto deveria ser igual a 100");
            Assert.AreEqual(95, modulo1._NbPontosUtilizados, "Os pontos utilizados deveriam ser iguais a 95");
            Assert.AreEqual("+5", modulo1._NbDiferencaDePontosProjeto, "A diferença de pontos deveria ser igual a 5");
        }

        /// <summary>
        /// método TestarPercentualAtualizadoPontuacaoZerada
        /// </summary>
        [TestMethod]
        public void TestarPercentualAtualizadoPontuacaoZerada()
        {
            /**
             *Ter um projeto cadastrado que não possui pontuação informada
             */


            //Passo 1
            //CT_4.02.02 - Testar incluir um novo módulo em um projeto que está com a pontuação zerada
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "nome projeto", true);
            projeto.Save();

            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            modulo1.NbPontosTotal = 40;
            modulo1.Save();

            Assert.AreEqual((UInt32)0, modulo1.NbEsforcoPlanejado, "O esforço pranejado deveria ficar com valor 0");
        }
        
        /// <summary>
        /// método MetodoTestaSalvarModuloSemProjeto
        /// </summary>
        [TestMethod]
        public void MetodoTestaSalvarModuloSemProjeto()
        {
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, null, "", true);

            Assert.AreEqual(ValidationState.Invalid, ValidationUtil.GetRuleState(modulo1,
            "NaoPermitirSalvarModuloSeNaoTiverProjetoSelecionado", DefaultContexts.Save));
        }

        /// <summary>
        /// Testar verificar a atualização de valores de % do módulo
        /// </summary>
        [TestMethod]
        public void TestarValidacaoPorcentagem()
        {

            //teste para verificar os métodos criados para calcular a
            //situação do projeto ao qual esse módulo pertence

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "nome projeto", true);
            Modulo modulo1 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo3 = ModuloFactory.Criar(SessionTest, projeto, "", true);
            modulo1.NbPontosTotal = 40;
            modulo2.NbPontosTotal = 40;
            modulo3.NbPontosTotal = 15;
            modulo1.Save();
            modulo2.Save();
            modulo3.Save();

            Assert.AreEqual(100, modulo1._NbPontosTotalProjeto, "O total dos pontos do projeto deveria ser igual a 100");
            Assert.AreEqual(95, modulo1._NbPontosUtilizados, "Os pontos utilizados deveriam ser iguais a 95");
            Assert.AreEqual("+5", modulo1._NbDiferencaDePontosProjeto, "A diferença de pontos deveria ser igual a 5");
        }

        [TestMethod]
        public void TestarRecuperarApenasModulosFilhos()
        {
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "projeto 01", true);
            Modulo mod1 = ModuloFactory.Criar(SessionTest, projeto, "modulo 1", true);
            Modulo mod2 = ModuloFactory.Criar(SessionTest, projeto, "modulo 2", true);
            Modulo mod3 = ModuloFactory.Criar(SessionTest, projeto, "modulo 3", true);

            XPCollection<Modulo> modulos = Modulo.GetModuloPorProjeto(SessionTest, projeto.Oid);
            Assert.AreEqual(3, modulos.Count);

            Modulo mod1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod1, "modulo 1.1", true);
            Modulo mod2_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod2, "modulo 2.1", true);
            Modulo mod3_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod3, "modulo 3.1", true);

            modulos = Modulo.GetModuloPorProjeto(SessionTest, projeto.Oid);
            Assert.AreEqual(3, modulos.Count);

            ModuloFactory.CriarModuloFilho(SessionTest, mod1_1, "modulo 1.1", true);
            ModuloFactory.CriarModuloFilho(SessionTest, mod2_1, "modulo 2.1", true);
            ModuloFactory.CriarModuloFilho(SessionTest, mod3_1, "modulo 3.1", true);

            modulos = Modulo.GetModuloPorProjeto(SessionTest, projeto.Oid);
            Assert.AreEqual(3, modulos.Count);
    }

        [TestMethod]
        public void TestarRecuperarModulosRaiz()
        {
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "projeto 01", true);
            Modulo mod1 = ModuloFactory.Criar(SessionTest, projeto, "modulo 1", true);
            Modulo mod2 = ModuloFactory.Criar(SessionTest, projeto, "modulo 2", true);
            Modulo mod3 = ModuloFactory.Criar(SessionTest, projeto, "modulo 3", true);

            Modulo mod1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod1, "modulo 1.1", true);
            Modulo mod2_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod2, "modulo 2.1", true);
            Modulo mod3_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod3, "modulo 3.1", true);

            Modulo mod1_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod1_1, "modulo 1.1.1", true);
            Modulo mod2_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod2_1, "modulo 2.1.1", true);
            Modulo mod3_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod3_1, "modulo 3.1.1", true);

            Modulo mod1_1_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod1_1_1, "modulo 1.1.1.1", true);
            Modulo mod2_1_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod2_1_1, "modulo 2.1.1.1", true);
            Modulo mod3_1_1_1 = ModuloFactory.CriarModuloFilho(SessionTest, mod3_1_1, "modulo 3.1.1.1", true);


            Assert.AreEqual(mod1, Modulo.RnRecuperarModuloRaiz(mod1_1));
            Assert.AreEqual(mod2, Modulo.RnRecuperarModuloRaiz(mod2_1));
            Assert.AreEqual(mod3, Modulo.RnRecuperarModuloRaiz(mod3_1));

            Assert.AreEqual(mod1, Modulo.RnRecuperarModuloRaiz(mod1_1_1));
            Assert.AreEqual(mod2, Modulo.RnRecuperarModuloRaiz(mod2_1_1));
            Assert.AreEqual(mod3, Modulo.RnRecuperarModuloRaiz(mod3_1_1));

            Assert.AreEqual(mod1, Modulo.RnRecuperarModuloRaiz(mod1_1_1_1));
            Assert.AreEqual(mod2, Modulo.RnRecuperarModuloRaiz(mod2_1_1_1));
            Assert.AreEqual(mod3, Modulo.RnRecuperarModuloRaiz(mod3_1_1_1));
}
    }
}
