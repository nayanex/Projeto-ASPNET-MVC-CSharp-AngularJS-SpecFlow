using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL.Models.Qualidade;
using WexProject.BLL.Models.Execucao;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe EstoriaTest
    /// </summary>
    [TestClass]
    public class EstoriaTest : BaseTest
    {
        /// <summary>
        /// método SalvarEstoria
        /// </summary>
        [TestMethod]
        public void SalvarEstoria()
        {
            /**
             * Cenário 1: Serão criados 2 requisitos para um projeto e o wex
             * deverá criar os IDs incrementados em sequencia.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.01", estoria.TxID, "O ID do requisito deveria ser PBI_1.01");

            estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.02", estoria.TxID, "O ID do requisito deveria ser PBI_1.02");

        }
        /// <summary>
        /// método SalvarEstoriaProjetoQueJaPossuiEstoria
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaProjetoQueJaPossuiEstoria()
        {

            /**
             * Cenário 2: Serão criadas 3 estórias. Em seguida a estória com ID PBI_1.2 será deletada.
             * Então será criada uma quarta estória e o wex deverá armazena-la com PBI_1.4.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxNome", true);
            ProjetoParteInteressada projetoparteinteressada = ProjetoParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.01", estoria.TxID, "O ID do requisito deveria ser PBI_1.01");

            estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.02", estoria.TxID, "O ID do requisito deveria ser PBI_1.02");

            Assert.AreEqual("PBI_01.03", EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true).TxID, "O ID do requisito deveria ser PBI_1.03");

            estoria.Delete();

            Assert.AreEqual("PBI_01.04", EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true).TxID, "O ID do requisito deveria ser PBI_1.04");
        }

        /// <summary>
        /// método SalvarEstoriaSubModulo
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaSubModulo()
        {

            /**
             * Cenário 3: Será criada uma estória com sub módulo e o wex deverá salvar corretamente. Ex: PBI_1.1.1
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo moduloPai = ModuloFactory.CriarModuloFilho(SessionTest, modulo, "", true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, moduloPai, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            Assert.AreEqual("PBI_01.01.01", estoria.TxID, "O ID do requisito deveria ser PBI_1.01.01");

        }
        /// <summary>
        /// método SalvarEstoriaComEstoriaPai
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaComEstoriaPai()
        {

            /**
             * Cenário 4: Será criada uma estória com uma estória pai e o wex deverá salvar corretamente. Ex: PBI_1.1.1
             */


            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Estoria estoriaFilha = EstoriaFactory.CriarFilho(SessionTest, estoria, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            Assert.AreEqual("PBI_01.01.01", estoriaFilha.TxID, "O ID da estoria deveria ser PBI_1.01.01");
        }

        /// <summary>
        /// método SalvarVariasEstoriasParaUmModulo
        /// </summary>
        [TestMethod]
        public void SalvarVariasEstoriasParaUmModulo()
        {

            /**
             * Cenário 5: Será criado um projeto e um módulo.
             * Em seguida serão criadas varias histórias para esse módulo.
             * O sistema deverá salvar todas corretamente.
             */

            Projeto projetoA = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo moduloA = ModuloFactory.Criar(SessionTest, projetoA, "", true);
            moduloA.NbPontosTotal = 132;
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projetoA, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, moduloA, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            estoria1.NbTamanho = 0;
            estoria1.Save();

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, moduloA, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            estoria2.NbTamanho = 0;
            estoria2.Save();

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, moduloA, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            estoria3.NbTamanho = 0;
            estoria3.Save();

            Assert.AreEqual(moduloA.NbPontosNaoIniciado, 132);

            estoria1.NbTamanho = 30;
            estoria1.NbPrioridade = 1;
            estoria1.Save();

            Assert.AreEqual(moduloA.NbPontosNaoIniciado, 102);
            Assert.AreEqual(moduloA.NbPontosEmAnalise, 30);
        }
        /// <summary>
        /// método SalvarEstoriaAposSalvarUmaEstoriaFilha
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaAposSalvarUmaEstoriaFilha()
        {
            /**
             * Cenário 6: Será criada e salva uma estória A no projeto.
             * Em seguida será criada uma estória tendo com pai a estória A.
             * Após a criação da estória filha, será criada uma nova estória.
             * O sistema deverá salvar os IDs na ordem correta.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Estoria estoriaFilha = EstoriaFactory.CriarFilho(SessionTest, estoria1, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.01.01", estoriaFilha.TxID, "O ID da estoria deveria ser PBI_1.01.01");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual("PBI_01.02", estoria2.TxID, "O ID da estoria deveria ser PBI_1.02");
        }
        /// <summary>
        /// método EditarPrioridadeDaEstoriaSubirPrioridade
        /// </summary>
        [TestMethod]
        public void EditarPrioridadeDaEstoriaSubirPrioridade()
        {
            /**
             * Cenário 7: Serão criadas 3 estórias e o sistema deverá gerar e salvar as prioridades na sequencia correta
             * Após isso, a estória 3 será repriorizada para 0 e as demais deverão ter sua prioridade ajustada corretamente
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria1", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria2", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria3", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3");

            estoria3.NbPrioridade = 1;
            estoria3.Save();

            Assert.AreEqual(1, estoria3.NbPrioridade, "Teste 4");
            Assert.AreEqual(2, estoria1.NbPrioridade, "Teste 5");
            Assert.AreEqual(3, estoria2.NbPrioridade, "Teste 6");
        }

        /// <summary>
        /// mátodo EditarPrioridadeDaEstoriaDescerPrioridade
        /// </summary>
        [TestMethod]
        public void EditarPrioridadeDaEstoriaDescerPrioridade()
        {
            /**
             * Cenário 8: Serão criadas 3 estórias e o sistema deverá gerar e salvar as prioridades na sequencia correta
             * Após isso, a estória 1 será repriorizada para 2 e as demais deverão ter suas prioridades ajustadas corretamente
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria1", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria2", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria3", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3");

            estoria1.NbPrioridade = 3;
            estoria1.Save();

            Assert.AreEqual(estoria2.NbPrioridade, 1, "Teste 4");
            Assert.AreEqual(estoria3.NbPrioridade, 2, "Teste 5");
            Assert.AreEqual(estoria1.NbPrioridade, 3, "Teste 6");
        }
        /// <summary>
        /// método SalvarEstoriaSemPrioridade
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaSemPrioridade()
        {
            /**
             * Cenário 9: Serão salvas duas estórias sem a prioridade informada para dois projetos diferentes.
             * O sistema deverá salvar e atribuir uma prioridade para as estórias corretamente.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto projeto2 = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Modulo modulo2 = ModuloFactory.Criar(SessionTest, projeto2, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Projeto.SelectedProject = projeto.Oid;
            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(1, estoria.NbPrioridade, "Teste 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(2, estoria2.NbPrioridade, "Teste 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(3, estoria3.NbPrioridade, "Teste 3");

            Projeto.SelectedProject = projeto2.Oid;
            Estoria estoria4 = EstoriaFactory.Criar(SessionTest, modulo2, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(1, estoria4.NbPrioridade, "Teste 4");

            Estoria estoria5 = EstoriaFactory.Criar(SessionTest, modulo2, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(2, estoria5.NbPrioridade, "Teste 5");

            Estoria estoria6 = EstoriaFactory.Criar(SessionTest, modulo2, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(3, estoria6.NbPrioridade, "Teste 6");

        }

        /// <summary>
        /// Testar salvar uma estoria Filha e verificar se a estória pai teve sua prioridade modificada para zero
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaFilhaVerificarPrioridadeEstoriaPai()
        {
            /**
             * Cenário X: Verificar a criação de prioridades e se uma estória foi estoriaPai setar o valor de prioridade
             * para zero.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Projeto.SelectedProject = projeto.Oid;
            //Criação de diversas estórias e verificar numeração de prioridade
            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(1, estoria.NbPrioridade, "Primeira estória = prioridade 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(2, estoria2.NbPrioridade, "Segunda estória = prioridade 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(3, estoria3.NbPrioridade, "Terceira estória = prioridade 3");

            Estoria estoria4 = EstoriaFactory.CriarFilho(SessionTest, estoria3, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            //Verificando a criacao de estoria filha com priodidade 4
            Assert.AreEqual(4, estoria4.NbPrioridade, "Quarta estória = prioridade 4");

            //Verificando se a prioridade da estoria 3 foi alterada
            Assert.AreEqual(0, estoria3.NbPrioridade, "Como a estoria 3 passou a ser pai da estoria 4 a prioridade deverá ser modificada");

        }

        /// <summary>
        /// Testar verificar a troca de uma estoria pai e se as prioridades foram invertidas
        /// </summary>
        [TestMethod]
        public void VerificarTrocaPrioridadesEstoriaPais()
        {
            /**
             * Cenário X: Verificar a criação de prioridades e se uma estória foi estoriaPai setar o valor de prioridade
             * para zero.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Projeto.SelectedProject = projeto.Oid;
            //Criação de diversas estórias e verificar numeração de prioridade
            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(1, estoria.NbPrioridade, "Primeira estória = prioridade 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(2, estoria2.NbPrioridade, "Segunda estória = prioridade 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(3, estoria3.NbPrioridade, "Terceira estória = prioridade 3");

            Estoria estoria4 = EstoriaFactory.CriarFilho(SessionTest, estoria3, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            //Verificando a criacao de estoria filha com priodidade 4
            Assert.AreEqual(4, estoria4.NbPrioridade, "Quarta estória = prioridade 4");

            //Verificando se a prioridade da estoria 3 foi alterada
            Assert.AreEqual(0, estoria3.NbPrioridade, "Como a estoria 3 passou a ser pai da estoria 4 a prioridade deverá ser modificada");

            Estoria estoria5 = EstoriaFactory.CriarFilho(SessionTest, estoria4, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            //Verificando a criacao de estoria filha com priodidade 5
            Assert.AreEqual(5, estoria5.NbPrioridade, "Quinta estória = prioridade 5");

            // Como a estoria 3 passou a ser pai da estoria 4 a prioridade deverá ser modificada
            Assert.AreEqual(0, estoria4.NbPrioridade, "Como a estoria 4 passou a ser pai da estoria 5 a prioridade deverá ser modificada");

            estoria5.Reload();
            estoria5.EstoriaPai = estoria2;
            estoria.Save();
            estoria2.Save();
            estoria3.Save();
            estoria4.Save();
            estoria5.Save();

            Assert.AreEqual(1, estoria.NbPrioridade, "A estoria 1 deveria ter valor 1");
            Assert.AreEqual(0, estoria2.NbPrioridade, "A estoria 2 deveria ter valor 0, pois é pai");
            Assert.AreEqual(0, estoria3.NbPrioridade, "A estoria 3 deveria ter valor 0, pois é pai");
            Assert.AreEqual(4, estoria4.NbPrioridade, "Deveria ser 4");
            Assert.AreEqual(5, estoria5.NbPrioridade, "Deveria ser 5");

        }


        /// <summary>
        /// Verificar a troca de uma estoria pai e se as prioridades foram invertidas e reclaculo de prioridades
        /// </summary>
        [TestMethod]
        public void VerificarRecalculoNaTrocaEstoriaPais()
        {
            /**
             * Cenário X: O Recalculo de Prioridades na troca de Pais.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Projeto.SelectedProject = projeto.Oid;
            //Criação de diversas estórias e verificar numeração de prioridade
            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            Assert.AreEqual(1, estoria1.NbPrioridade, "A prioridade deveria ser 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(2, estoria2.NbPrioridade, "A prioridade deveria ser 2");

            Estoria estoria3 = EstoriaFactory.CriarFilho(SessionTest, estoria1, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(0, estoria1.NbPrioridade, "A prioridade deveria ser 0, pois a estoria virou pai");
            Assert.AreEqual(3, estoria3.NbPrioridade, "A prioridade deveria ser 3, pois foi a terceira estoria criada");

            Estoria estoria4 = EstoriaFactory.CriarFilho(SessionTest, estoria2, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(0, estoria2.NbPrioridade, "A prioridade deveria ser 0, pois a estoria virou pai");

            Assert.AreEqual(4, estoria4.NbPrioridade, "A prioridade deveria ser 4, pois foi a quarta estoria criada");

            Estoria estoria5 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            estoria4.Reload();
            estoria4.EstoriaPai = estoria1;
            estoria4.Save();

            Assert.AreEqual(0, estoria1.NbPrioridade, "A prioridade deveria ser 0, pois a continua sendo pai");
            Assert.AreEqual(4, estoria2.NbPrioridade, "A prioridade deveria ser 4, pois foi houve a troca de prioridade de pais");
            Assert.AreEqual(2, estoria3.NbPrioridade, "A prioridade deveria ser 0, pois a continua sendo pai");
            Assert.AreEqual(3, estoria4.NbPrioridade, "A prioridade deveria ser 0, pois a continua sendo pai");
            Assert.AreEqual(5, estoria5.NbPrioridade, "A prioridade deveria ser 0, pois a continua sendo pai");

        }

        /// <summary>
        /// Verificar Prioridade de pai com vários filhos
        /// </summary>
        [TestMethod]
        public void VerificarPrioridadePaiVariosFilhos()
        {
            /**
             * Cenário X: O Recalculo de Prioridades na troca de Pai com muitos filhos.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Projeto.SelectedProject = projeto.Oid;
            //Criação de diversas estórias e verificar numeração de prioridade
            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria1", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(1, estoria1.NbPrioridade, "A prioridade deveria ser 1");

            Estoria estoria2 = EstoriaFactory.CriarFilho(SessionTest, estoria1, "Estoria2", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(0, estoria1.NbPrioridade, "A prioridade deveria ser 0, pois virou pai");
            Assert.AreEqual(2, estoria2.NbPrioridade, "A prioridade deveria ser 2");

            Estoria estoria3 = EstoriaFactory.CriarFilho(SessionTest, estoria1, "Estoria3", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(0, estoria1.NbPrioridade, "A prioridade deveria ser 0, pois a estoria virou pai");
            Assert.AreEqual(2, estoria2.NbPrioridade, "A prioridade deveria ser 2");
            Assert.AreEqual(3, estoria3.NbPrioridade, "A prioridade deveria ser 3, pois foi a terceira estoria criada");

            Estoria estoria4 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria4", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(4, estoria4.NbPrioridade, "A prioridade deveria ser 4");

            estoria3.Reload();
            estoria3.EstoriaPai = estoria4;
            estoria3.Save();

            Assert.AreEqual(3, estoria3.NbPrioridade, "A prioridade deveria ser 2");
            Assert.AreEqual(0, estoria4.NbPrioridade, "A prioridade deveria ser 0, pois é pai.");
            Assert.AreEqual(0, estoria1.NbPrioridade, "A prioridade deveria continuar sendo 0");

            estoria2.Reload();
            estoria2.EstoriaPai = estoria4;
            estoria2.Save();

            Assert.AreEqual(1, estoria2.NbPrioridade, "A prioridade deveria ser 1");
            Assert.AreEqual(0, estoria4.NbPrioridade, "A prioridade deveria ser 0, pois é pai.");
            Assert.AreEqual(2, estoria3.NbPrioridade, "A prioridade deveria ser 2");
            Assert.AreEqual(3, estoria1.NbPrioridade, "A prioridade deveria modificar para 3");

        }


        /// <summary>
        /// método SalvarEstoriaComPrioridadeInformada
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaComPrioridadeInformada()
        {
            /**
             * Cenário 10: Será salva uma estória com a prioridade informada.
             * O sistema deverá salvar e atribuir uma prioridade para a estória.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3");


            Estoria estoria4 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", false);

            estoria4.NbPrioridade = 1;
            estoria4.Save();

            Assert.AreEqual(1, estoria4.NbPrioridade);
            Assert.AreEqual(2, estoria1.NbPrioridade);
            Assert.AreEqual(3, estoria2.NbPrioridade);
            Assert.AreEqual(4, estoria3.NbPrioridade);
        }

        /// <summary>
        /// método DeletarUmaEstoriaComPrioridade
        /// </summary>
        [TestMethod]
        public void DeletarUmaEstoriaComPrioridade()
        {
            /**
             * Cenário 11: Serão criadas 3 estorias com prioridade.
             * A estoria 2 deverá ser deletada e as outras duas devem ser reordenadas para 0 e 1 respectivamente.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1 - A prioridade deveria ser 0");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2 - A prioridade deveria ser 1");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3 - A prioridade deveria ser 2");

            estoria2.Delete();
            Assert.AreEqual(1, estoria1.NbPrioridade, "Teste 4 - A prioridade deveria ser 1");
            Assert.AreEqual(2, estoria3.NbPrioridade, "Teste 5 - A prioridade deveria ser 2");
        }


        /// <summary>
        /// método CorrecaoBugEditarPrioridadeDaEstoriaDescerPrioridade
        /// </summary>
        public void CorrecaoBugEditarPrioridadeDaEstoriaDescerPrioridade()
        {
            /**
             * Cenário 12: Serão criadas 3 estórias e o sistema deverá gerar e salvar as prioridades na sequencia correta
             * Após isso, a estória 1 será repriorizada para 3 e as demais deverão ter suas prioridades ajustadas corretamente
             * Uma serie de repriorizações seguintes serão criadas e o sistema deverá executá-las corretamente
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria1", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria2", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "Estoria3", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3");

            estoria1.NbPrioridade = 3;
            estoria1.Save();

            Assert.AreEqual(estoria2.NbPrioridade, 1, "Teste 4");
            Assert.AreEqual(estoria3.NbPrioridade, 2, "Teste 5");
            Assert.AreEqual(estoria1.NbPrioridade, 3, "Teste 6");


            estoria2.NbPrioridade = 2;
            estoria2.Save();

            Assert.AreEqual(estoria3.NbPrioridade, 1, "Teste 7");
            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 8");
            Assert.AreEqual(estoria1.NbPrioridade, 3, "Teste 9");

            estoria1.NbPrioridade = 3;
            estoria1.Save();

            Assert.AreEqual(estoria3.NbPrioridade, 1, "Teste 10");
            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 11");
            Assert.AreEqual(estoria1.NbPrioridade, 3, "Teste 12");
        }
        /// <summary>
        /// método EditarPrioridadeGridEstoria
        /// </summary>
        [TestMethod]
        public void EditarPrioridadeGridEstoria()
        {
            /**
             * Cenário 13: Serão criadas 3 estorias sem prioridade.
             * A estoria 1 será editada através do grid de estória para 3 e as outras duas devem ser reordenadas para 1 e 2 respectivamente.
             * O sistema deverá converter as prioridades para P01, P02 e P03.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Projeto.SelectedProject = projeto.Oid;

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Teste 1 - A prioridade deveria ser 0");

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Teste 2 - A prioridade deveria ser 1");

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Teste 3 - A prioridade deveria ser 2");

            estoria1._TxQuando = "P03";
            estoria1.Save();

            Assert.AreEqual("P1", estoria2._TxQuando, "Teste 4 - A prioridade deveria ser P01");
            Assert.AreEqual("P2", estoria3._TxQuando, "Teste 5 - A prioridade deveria ser P02");
            Assert.AreEqual("P3", estoria1._TxQuando, "Teste 6 - A prioridade deveria ser P03");

            Assert.AreEqual(1, estoria2.NbPrioridade, "Teste 7 - A prioridade deveria ser 1");
            Assert.AreEqual(2, estoria3.NbPrioridade, "Teste 8 - A prioridade deveria ser 2");
            Assert.AreEqual(3, estoria1.NbPrioridade, "Teste 9 - A prioridade deveria ser 3");
        }
        /// <summary>
        /// método CorrecaoBugCalcularPontos
        /// </summary>
        [TestMethod]
        public void CorrecaoBugCalcularPontos()
        {
            /**
            * Cenário 14: Será criada uma estória com prioridade 2 para o módulo
            * O sistema deverá calcular corretamente a porcentagem dos pontos.
            */

            var projeto = ProjetoFactory.Criar(SessionTest, 0, "");
            Projeto.SelectedProject = projeto.Oid;
            projeto.NbTamanhoTotal = 10;
            projeto.Save();

            var modulo = ModuloFactory.Criar(SessionTest, projeto, "", false);
            modulo.NbPontosTotal = 50;
            modulo.Save();

            //ProjetoParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, projeto, true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);
            var estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas");
            estoria.NbTamanho = 2;
            estoria.Save();

            Assert.AreEqual(96, modulo._NbPercNaoIniciado, "Teste 1 - O perc de pontos deveria ser 60%");
            Assert.AreEqual(4, modulo._NbPercEmAnalise, "Teste 2 - O perc de pontos deveria ser 40%");

        }
        /// <summary>
        /// método TestarCadastrarExcluirCasoTeste
        /// </summary>
        [TestMethod]
        public void TestarCadastrarExcluirCasoTeste()
        {
            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            //Criando caso de teste
            Requisito requisito = RequisitoFactory.Criar(SessionTest, modulo, "nome", "descricao", "link", true);
            CasoTeste casoTeste = CasoTesteFactory.Criar(SessionTest, requisito, "passos", "sumário", "precondicoes", true);
            EstoriaCasoTeste estoriaCasoteste = EstoriaCasoTesteFactory.Criar(SessionTest, casoTeste);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            estoria.CasosTeste.Add(estoriaCasoteste);

            estoria.Save();

            XPCollection<Estoria> estoriasCadastradas = new XPCollection<Estoria>(SessionTest);

            XPCollection<EstoriaCasoTeste> estoriacasosTestesCadastrados = new XPCollection<EstoriaCasoTeste>(SessionTest);
            Assert.AreEqual(1, estoriasCadastradas.Count, "Estória cadastrada deveria ser igual a 1");

            Assert.AreEqual(1, estoriacasosTestesCadastrados.Count, "CasosTesteCadastrados deveria ser igual a 1");

            estoria.Delete();

            Assert.AreEqual(0, estoriasCadastradas.Count, "Estória cadastrada deveria ser igual a 0");
            Assert.AreEqual(0, estoriacasosTestesCadastrados.Count, "EstoriacasosTestesCadastrados deveria ser igual a 0");
        }
        /// <summary>
        /// TestarCalcularEstoriaPai
        /// </summary>
        [TestMethod]
        public void TestarCalcularEstoriaPai()
        {
            /*
                                                             * PRÉ-CONDIÇÕES:

                                                                1. Estar em Projetos -> Escopo -> Estórias
                                                                2. Ter estórias cadastradas

                                                                COMO REPRODUZIR:
                                                                1. Cadastrar uma estória filha da estória da pré-condição 2.
                                                                2. Salvar e fechar.
                                                                3. Atualizar grid
                                                                4. Verificar pontuação da estória pai

                                                             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 0, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);

            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);

            Estoria estoriaFilha = EstoriaFactory.CriarFilho(SessionTest, estoria, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            estoria.NbTamanho = 5;
            estoriaFilha.NbTamanho = 2;


            estoria.Save();
            estoriaFilha.Save();

            Assert.AreEqual((float)2, estoria.NbTamanho, "A tamanho da estoria pai deve ser a soma das  filhas");

            Estoria estoriaFilha2 = EstoriaFactory.CriarFilho(SessionTest, estoria, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
            "TxReferencias", "TxDuvidas", true);
            estoriaFilha2.NbTamanho = 2;

            estoriaFilha2.Save();

            Assert.AreEqual((float)4, estoria.NbTamanho, "A tamanho da estoria pai deve ser a soma das  filhas");

            estoriaFilha2.Delete();

            Assert.AreEqual(1, estoria.EstoriaFilho.Count, "Deve ter só uma estoria filho");
            Assert.AreEqual((float)2, estoria.NbTamanho, "A tamanho da estoria pai deve ser a soma das  filhas");

            estoriaFilha.Delete();
            Assert.AreEqual(0, estoria.EstoriaFilho.Count, "Deve ter só uma estoria filho");

            Assert.AreEqual((float)2, estoria.NbTamanho, "A tamanho da estoria pai deve ser a soma das  filhas");
        }

        /// <summary>
        /// TestarEstoriaConversaoStringDouble
        /// </summary>
        [TestMethod]
        public void TestarEstoriaConversaoStringDouble()
        {
            //Criação das pré-definições
            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 27);
            projeto01.DtInicioReal = new DateTime(2011, 04, 27);
            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo01.NbPontosTotal = 2;
            modulo01.Save();

            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
"TxReferencias", "TxDuvidas", true);

            //Passando o tamanho 0.5

            estoria01.NbPrioridade = 1;
            estoria01._NbTamanho = "0.5";
            estoria01.Save();

            Assert.AreEqual(5, estoria01.NbTamanho, "Conversão errada!");


            // Passando o tamanho 0,5

            estoria01._NbTamanho = "0,5";
            estoria01.Save();

            Assert.AreEqual(0.5, estoria01.NbTamanho, "Conversão certa!");
        }




        /// <summary>
        /// Montar um cenario em que exista estorias filhos no ciclo
        /// </summary>
        [TestMethod]
        public void EstoriaFilhasCiclo()
        {
            /**
             * Cenário: Montar um cenario em que exista estorias filhos no ciclo
             */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 27);
            projeto01.DtInicioReal = new DateTime(2011, 04, 27);
            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();

            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei",
                beneficiado, "TxObservacoes", "TxReferencias", "TxDuvidas", true);

            estoria01._NbTamanho = "33";
            estoria01.Save();

            estoria01._NbTamanho = "0,5";
            estoria01.Save();

            Estoria estoriaFilha01 = EstoriaFactory.CriarFilho(SessionTest, estoria01, "TxTitulo", "TxGostariaDe",
                "TxEntaoPoderei", beneficiado, "TxObservacoes", "TxReferencias", "TxDuvidas", true);

            estoriaFilha01.NbTamanho = 2;


            Estoria estoriaFilha02 = EstoriaFactory.CriarFilho(SessionTest, estoria01, "TxTitulo", "TxGostariaDe",
                "TxEntaoPoderei", beneficiado, "TxObservacoes", "TxReferencias", "TxDuvidas", true);

            estoriaFilha02.NbTamanho = 2;

            Estoria estoriaFilha03 = EstoriaFactory.CriarFilho(SessionTest, estoria01, "TxTitulo", "TxGostariaDe",
                "TxEntaoPoderei", beneficiado, "TxObservacoes", "TxReferencias", "TxDuvidas", true);

            estoriaFilha03.NbTamanho = 5;

            estoriaFilha01.Save();
            Assert.AreEqual(2, estoriaFilha01.NbTamanho, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            estoriaFilha01.Reload(); // força o onload

            estoriaFilha02.Save();
            Assert.AreEqual(2, estoriaFilha02.NbTamanho, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            estoriaFilha02.Reload(); // força o onload

            estoriaFilha03.Save();
            Assert.AreEqual(5, estoriaFilha03.NbTamanho, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            estoriaFilha01.Reload(); // força o onload

            Assert.AreEqual(9, estoria01.NbTamanho, "A situação do item do ciclo deveria ser 'Não Iniciado'");
        }

        /// <summary>
        /// Testar preenchimento do Título da Estória
        /// </summary>
        [TestMethod]
        public void TestarPreenchimentoDoTituloDaEstoria()
        {
            // Criação de um novo projeto
            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 27);
            projeto01.DtInicioReal = new DateTime(2011, 04, 27);
            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            // Criação de um novo Módulo para o Projeto
            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();

            // Criação de um novo Beneficiado
            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Analista de Negócios", true);

            // Criação de uma nova Estória
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei",
                beneficiado01, "TxObservacoes", "TxReferencias", "TxDuvidas", true);

            // Passo 1 - Alteração do valor do 'Gostaria de', e verificação se foi alterado o título
            estoria01.TxGostariaDe = "gerar um relatório";
            Assert.AreEqual("Analista de Negócios gerar um relatório", estoria01.TxTitulo,
                "O título deveria ser 'Analista de Negócios gerar um relatório'");

            // Passo 2 - Alteração do valor do 'Gostaria de', e verificação se foi alterado o título
            estoria01.TxGostariaDe = "Gerar um relatório";
            Assert.AreEqual("Analista de Negócios gerar um relatório", estoria01.TxTitulo,
                "O título deveria ser 'Analista de Negócios gerar um relatório'");

            // Passo 3 - Salvando e verificando se o título foi salvo corretamente
            estoria01.Save();
            Assert.AreEqual("Analista de Negócios gerar um relatório", estoria01.TxTitulo,
                "O título deveria ser 'Analista de Negócios gerar um relatório'");

            // Passo 4 - Alterando o título para um valor qualquer e verificando se o valor foi salvo corretamente
            estoria01.TxTitulo = "Gostaria de gerar um relatório para o Analista de Negócios";
            estoria01.Save();
            Assert.AreEqual("Gostaria de gerar um relatório para o Analista de Negócios", estoria01.TxTitulo,
                "O título deveria ser 'Gostaria de gerar um relatório para o Analista de Negócios'");
        }

        /// <summary>
        /// método VerificarPrioridadesEstoria
        /// </summary>
        [TestMethod]
        public void VerificarPrioridadesEstoria()
        {
            /**
             * Cenário 1: Serão criados 2 requisitos para um projeto e o wex
             * deverá criar os IDs incrementados em sequencia.
             */

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 5, "", true);
            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);

            Estoria estoria1 = EstoriaFactory.Criar(SessionTest, modulo, "estoria1", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
                "TxReferencias", "TxDuvidas", true);

            Estoria estoria2 = EstoriaFactory.Criar(SessionTest, modulo, "estoria2", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
                "TxReferencias", "TxDuvidas", true);

            Estoria estoria3 = EstoriaFactory.Criar(SessionTest, modulo, "estoria3", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
                "TxReferencias", "TxDuvidas", true);

            Estoria estoria4 = EstoriaFactory.Criar(SessionTest, modulo, "estoria4", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
                "TxReferencias", "TxDuvidas", true);

            Estoria estoria5 = EstoriaFactory.Criar(SessionTest, modulo, "estoria5", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes",
                "TxReferencias", "TxDuvidas", true);


            estoria5.NbPrioridade = 5;
            estoria5.Save();
            estoria4.NbPrioridade = 4;
            estoria4.Save();
            estoria3.NbPrioridade = 3;
            estoria3.Save();
            estoria2.NbPrioridade = 2;
            estoria2.Save();
            estoria1.NbPrioridade = 1;
            estoria1.Save();


            Assert.AreEqual(estoria1.NbPrioridade, 1, "Deveria ser 1");

            Assert.AreEqual(estoria2.NbPrioridade, 2, "Deveria ser 2");

            Assert.AreEqual(estoria3.NbPrioridade, 3, "Deveria ser 3");

            Assert.AreEqual(estoria4.NbPrioridade, 4, "Deveria ser 4");

            Assert.AreEqual(estoria5.NbPrioridade, 5, "Deveria ser 5");

            estoria5.NbPrioridade = 1;
            estoria5.Save();

            Assert.AreEqual(estoria5.NbPrioridade, 1, "Deveria ser 1");

            Assert.AreEqual(estoria1.NbPrioridade, 2, "Deveria ser 2");

            Assert.AreEqual(estoria2.NbPrioridade, 3, "Deveria ser 3");

            Assert.AreEqual(estoria3.NbPrioridade, 4, "Deveria ser 4");

            Assert.AreEqual(estoria4.NbPrioridade, 5, "Deveria ser 5");

            estoria1.NbPrioridade = 1;

            estoria1.Save();

            Assert.AreEqual(estoria1.NbPrioridade, 1, "Deveria ser 1");

            Assert.AreEqual(estoria2.NbPrioridade, 3, "Deveria ser 4");

            Assert.AreEqual(estoria3.NbPrioridade, 4, "Deveria ser 3");

            Assert.AreEqual(estoria5.NbPrioridade, 2, "Deveria ser 2");

            Assert.AreEqual(estoria4.NbPrioridade, 5, "Deveria ser 5");
    }
    }
}