using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Execucao;
using DevExpress.Persistent.Validation;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// classe CicloTest
    /// </summary>
    [TestClass]
    public class CicloTest : BaseTest
    {
        /// <summary>
        /// método TestarSalvarUmCicloDeDesenvolvimentoComItensEmVariasSituacoes
        /// </summary>
        [TestMethod]
        public void TestarSalvarUmCicloDeDesenvolvimentoComItensEmVariasSituacoes()
        {
            /**
            * Cenário 1: Será cadastrado um projeto que conterá dois ciclos.
            * Serão adicionadas Estórias para esse Ciclo e em seguida serão feitas alterações nos itens do ciclo
            * O sistema deverá alterar e salvar corretamente essas mudanças.
            */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 27);
            projeto01.DtInicioReal = new DateTime(2011, 04, 27);
            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();
            Modulo modulo02 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo02.NbPontosTotal = 40;
            modulo02.Save();

            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descrição", true);

            Estoria estoria01_3pts = EstoriaFactory.Criar(SessionTest, modulo01, "Estória01", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria01_3pts.NbPrioridade = 1;
            estoria01_3pts.NbTamanho = 3;
            estoria01_3pts.Save();
            estoria01_3pts.Reload(); // força o onload

            Estoria estoria02_5pts = EstoriaFactory.Criar(SessionTest, modulo01, "Estória02", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria02_5pts.NbPrioridade = 2;
            estoria02_5pts.NbTamanho = 5;
            estoria02_5pts.Save();
            estoria02_5pts.Reload(); // força o onload

            Estoria estoria03_13pts = EstoriaFactory.Criar(SessionTest, modulo01, "Estória03", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria03_13pts.NbPrioridade = 3;
            estoria03_13pts.NbTamanho = 13;
            estoria03_13pts.Save();
            estoria03_13pts.Reload(); // força o onload

            Estoria estoria04_1pt = EstoriaFactory.Criar(SessionTest, modulo01, "Estória04", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria04_1pt.NbPrioridade = 4;
            estoria04_1pt.NbTamanho = 1;
            estoria04_1pt.Save();
            estoria04_1pt.Reload(); // força o onload

            //Passo 01

            CicloDesenvEstoria cicloEstoria01_3pts = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria01_3pts, true);
            CicloDesenvEstoria cicloEstoria02_5pts = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria02_5pts, true);

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloEstoria01_3pts.CsSituacao, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloEstoria02_5pts.CsSituacao, "A situação do item do ciclo deveria ser 'Não Iniciado'");

            //Passo 02
            //Trocando situação dos itens no ciclo.
            cicloEstoria01_3pts.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoria02_5pts.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;

            projeto01.Ciclos[0].TxMeta = "TxMeta";
            projeto01.Ciclos[0].Save();

            Assert.AreEqual(8, projeto01.Ciclos[0].NbPontosPlanejados, "O total de pontos deveria ser 8");
            Assert.AreEqual(0, projeto01.Ciclos[0].NbPontosRealizados, "O total de pontos realizados deveria ser 0");
            Assert.AreEqual(0, projeto01.Ciclos[0].NbAlcanceMeta, "O alcance da meta deveria ser 0%");
            Assert.AreEqual("Ciclo 1", estoria01_3pts._TxQuando, "A variável 'Quando?' deveria ser alterada para 'Ciclo 1'");
            Assert.AreEqual("Ciclo 1", estoria02_5pts._TxQuando, "A variável 'Quando?' deveria ser alterada para 'Ciclo 1'");
            Assert.AreEqual(projeto01.Ciclos[0], estoria01_3pts.Ciclo, "O ciclo da estoria deveria ser Ciclo 1");
            Assert.AreEqual(projeto01.Ciclos[0], estoria02_5pts.Ciclo, "O ciclo da estoria deveria ser Ciclo 1");
            Assert.AreEqual(CsSituacaoCicloDomain.EmAndamento, projeto01.Ciclos[0].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Não Iniciado'");
            Assert.AreEqual(CsEstoriaDomain.EmDesenv, estoria01_3pts.CsSituacao, "A estória deveria estar com a situação 'Em Desenvolvimento'");
            Assert.AreEqual(8, modulo01.NbPontosEmDesenv, "Os pontos em desenv deveriam ter sido recalculados.");
            Assert.AreEqual(13.33, modulo01._NbPercEmDesenv, "O percentual em desenv deveria ter sido recalculado.");

            Assert.AreEqual(1, estoria03_13pts.NbPrioridade, "A estoria que ficou no backlog deveria ter sido repriorizada.");
            Assert.AreEqual(2, estoria04_1pt.NbPrioridade, "A estoria que ficou no backlog deveria ter sido repriorizada.");

            //Passo 5

            projeto01.Ciclos[0].Reload();
            cicloEstoria01_3pts.Reload();
            cicloEstoria02_5pts.Reload();

            cicloEstoria01_3pts.Meta = true;
            cicloEstoria01_3pts.CsSituacao = CsSituacaoEstoriaCicloDomain.Pronto;

            cicloEstoria01_3pts.Save();
            projeto01.Ciclos[0].Save();

            Assert.AreEqual(CsEstoriaDomain.Pronto, estoria01_3pts.CsSituacao, "A situação da estória deveria ter sido alterada para pronto");
            Assert.AreEqual(CsEstoriaDomain.EmDesenv, estoria02_5pts.CsSituacao, "A estória deveria estar com a situação 'Em Desenvolvimento'");
            Assert.AreEqual(3, modulo01.NbPontosPronto, "A quantidade de pontos pronto deveria ter sido recalculada.");
            Assert.AreEqual(5, modulo01.NbPontosEmDesenv, "A quantidade de pontos em desenveria ter sido mantida.");
            Assert.AreEqual(14, modulo01.NbPontosEmAnalise, "O percentual em analise foi calculado incorretamente");
            Assert.AreEqual(38, modulo01.NbPontosNaoIniciado, "O percentual nao iniciado foi calculado incorretamente");
            Assert.AreEqual(3, projeto01.Ciclos[0].NbPontosRealizados, "O total de pontos realizados deveria ser 3");
            Assert.AreEqual(100, projeto01.Ciclos[0].NbAlcanceMeta, "O alcance da meta deveria ser 100%");

            //Passo 6
            cicloEstoria02_5pts.CsSituacao = CsSituacaoEstoriaCicloDomain.Replanejado;
            cicloEstoria02_5pts.Save();

            projeto01.Ciclos[0].Save();

            Assert.AreEqual(3, modulo01.NbPontosPronto, "Os pontos prontos deveriam ter sido mantidos.");
            Assert.AreEqual(0, modulo01.NbPontosEmDesenv, "Os pontos em desenv deveriam ter sido recalculados.");
            Assert.AreEqual(19, modulo01.NbPontosEmAnalise, "Os pontos em desenv deveriam ter sido recalculados.");
            Assert.AreEqual(38, modulo01.NbPontosNaoIniciado, "Os pontos Nao iniciados deveriam ter sido mantidos.");
            Assert.AreEqual(CsEstoriaDomain.Replanejado, estoria02_5pts.CsSituacao, "A estória deveria estar com a situação 'Replanejado'");
            Assert.AreEqual(8, projeto01.Ciclos[0].NbPontosPlanejados, "O total de pontos planejados nao foi calculado corretamente");
            Assert.AreEqual(3, projeto01.Ciclos[0].NbPontosRealizados, "O total de pontos realizados deveria ser 3");
            Assert.AreEqual(CsSituacaoCicloDomain.Concluido, projeto01.Ciclos[0].CsSituacaoCiclo, "O ciclo deveria estar concluido");
            Assert.AreEqual(1, estoria02_5pts.NbPrioridade, "A estoria que ficou no backlog deveria ter sido repriorizada.");
            Assert.AreEqual(2, estoria03_13pts.NbPrioridade, "A estoria que ficou no backlog deveria ter sido repriorizada.");
            Assert.AreEqual(3, estoria04_1pt.NbPrioridade, "A estoria que ficou no backlog deveria ter sido repriorizada.");

            //Passo 7

            CicloDesenvEstoria cicloEstoria03_13pts = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria03_13pts);
            cicloEstoria03_13pts.Meta = true;
            cicloEstoria03_13pts.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoria03_13pts.Save();

            CicloDesenvEstoria cicloEstoria04_1pt = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[1], estoria04_1pt);
            cicloEstoria04_1pt.Meta = true;
            cicloEstoria04_1pt.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloEstoria04_1pt.Save();

            projeto01.Ciclos[1].Save();

            Assert.AreEqual(3, modulo01.NbPontosPronto, "Os pontos prontos deveriam ter sido mantidos.");
            Assert.AreEqual(5, modulo01.NbPontosEmAnalise, "Os pontos em analise deveriam ter sido recalculados.");
            Assert.AreEqual(38, modulo01.NbPontosNaoIniciado, "Os pontos nao iniciados deveriam ter sido mantidos.");
            Assert.AreEqual(14, modulo01.NbPontosEmDesenv, "Os pontos em desenv deveriam ter sido recalculados.");

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.EmDesenv, cicloEstoria03_13pts.CsSituacao, "A situação do item do ciclo deveria ser 'Em desenvolvimento'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.EmDesenv, cicloEstoria04_1pt.CsSituacao, "A situação do item do ciclo deveria ser 'Em desenvolvimento'");
            Assert.AreEqual(8, projeto01.Ciclos[0].NbPontosPlanejados, "O total de pontos planejados nao foi calculado corretamente");
            Assert.AreEqual(14, projeto01.Ciclos[1].NbPontosPlanejados, "O total de pontos planejados deveria ser 18");
            Assert.AreEqual(0, projeto01.Ciclos[1].NbPontosRealizados, "O total de pontos realizados deveria ser 0");
            Assert.AreEqual(0, projeto01.Ciclos[1].NbAlcanceMeta, "Alcance da meta deveria ser 0");
            Assert.AreEqual(CsSituacaoCicloDomain.EmAndamento, projeto01.Ciclos[1].CsSituacaoCiclo, "A situação do ciclo deveria ser 'Em Andamento'");
            Assert.AreEqual(1, estoria02_5pts.NbPrioridade, "A prioridade deveria ter sido mantida");
            Assert.AreEqual(0, estoria03_13pts.NbPrioridade, "A prioridade deveria ter sido zerada pois a entrega esta no ciclo");
            Assert.AreEqual(0, estoria04_1pt.NbPrioridade, "A prioridade deveria ter sido zerada pois a entrega esta no ciclo");
        }
        /// <summary>
        /// método TestrarAlterarAOrdemDeItensDoCiclo
        /// </summary>
        [TestMethod]
        public void TestrarAlterarAOrdemDeItensDoCiclo()
        {
            /**
             * Cenário 2: Será criado um projeto01 com 1 ciclo
             * Em seguida será feita a ordenação dos itens desse ciclo
             * O sistema deverá ordenar corretamente
             */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "Nome");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 25);
            projeto01.DtInicioReal = new DateTime(2011, 05, 25);
            projeto01.NbCicloTotalPlan = 1;
            projeto01.NbCicloDuracaoDiasPlan = 10;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Nome", false);
            modulo01.NbEsforcoPlanejado = 100;
            modulo01.Save();

            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descrição", true);
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Titulo", "GostariaDe", "EntaoPoderei", beneficiado01, "Observacoes", "Referencias", "Duvidas", true);

            //Passo 1

            CicloDesenvEstoria cicloDesenvEstoria01 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria01, true);
            CicloDesenvEstoria cicloDesenvEstoria02 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria02, true);
            CicloDesenvEstoria cicloDesenvEstoria03 = CicloDesenvEstoriaFactory.Criar(SessionTest, projeto01.Ciclos[0], estoria03, true);

            Assert.AreEqual(1, cicloDesenvEstoria01.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, cicloDesenvEstoria02.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, cicloDesenvEstoria03.NbSequencia, "A sequência deveria ser 3");

            //Passo 2

            cicloDesenvEstoria01.NbSequencia = 2;
            cicloDesenvEstoria01.Save();

            Assert.AreEqual(1, cicloDesenvEstoria02.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, cicloDesenvEstoria01.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, cicloDesenvEstoria03.NbSequencia, "A sequência deveria ser 3");

            //Passo 3

            cicloDesenvEstoria02.NbSequencia = 3;
            cicloDesenvEstoria02.Save();

            Assert.AreEqual(1, cicloDesenvEstoria01.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, cicloDesenvEstoria03.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, cicloDesenvEstoria02.NbSequencia, "A sequência deveria ser 3");

            //Passo 4

            cicloDesenvEstoria02.NbSequencia = 2;
            cicloDesenvEstoria02.Save();

            Assert.AreEqual(1, cicloDesenvEstoria01.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, cicloDesenvEstoria02.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, cicloDesenvEstoria03.NbSequencia, "A sequência deveria ser 3");

            //Passo 5

            cicloDesenvEstoria03.NbSequencia = 1;
            cicloDesenvEstoria03.Save();

            Assert.AreEqual(1, cicloDesenvEstoria03.NbSequencia, "A sequência deveria ser 1");
            Assert.AreEqual(2, cicloDesenvEstoria01.NbSequencia, "A sequência deveria ser 2");
            Assert.AreEqual(3, cicloDesenvEstoria02.NbSequencia, "A sequência deveria ser 3");
        }
        /// <summary>
        /// método TestrarDeletarUmCiclo
        /// </summary>
        [TestMethod]
        public void TestrarDeletarUmCiclo()
        {
            /**
             * Cenário 3: Será criado um projeto01 com 4 ciclos
             * Em seguida serão deletados 2 ciclos
             * Os ciclos restantes deverão manter seus valores
             */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "Nome");
            projeto01.DtInicioPlan = new DateTime(2011, 06, 09);
            projeto01.DtInicioReal = new DateTime(2011, 06, 09);
            projeto01.NbCicloTotalPlan = 4;
            projeto01.NbCicloDuracaoDiasPlan = 5;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();

            Assert.AreEqual(new DateTime(2011, 06, 09), projeto01.Ciclos[0].DtInicio, "Teste1");
            Assert.AreEqual(new DateTime(2011, 06, 15), projeto01.Ciclos[0].DtTermino, "Teste2");

            Assert.AreEqual(new DateTime(2011, 06, 17), projeto01.Ciclos[1].DtInicio, "Teste3");
            Assert.AreEqual(new DateTime(2011, 06, 23), projeto01.Ciclos[1].DtTermino, "Teste4");

            Assert.AreEqual(new DateTime(2011, 06, 27), projeto01.Ciclos[2].DtInicio, "Teste5");
            Assert.AreEqual(new DateTime(2011, 07, 01), projeto01.Ciclos[2].DtTermino, "Teste6");

            Assert.AreEqual(new DateTime(2011, 07, 05), projeto01.Ciclos[3].DtInicio, "Teste7");
            Assert.AreEqual(new DateTime(2011, 07, 11), projeto01.Ciclos[3].DtTermino, "Teste8");

            projeto01.Ciclos[0].CsSituacaoCiclo = CsSituacaoCicloDomain.Concluido;
            projeto01.Ciclos[0].Save();

            projeto01.Ciclos[1].CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
            projeto01.Ciclos[1].Save();

            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[0].CsSituacaoCiclo, "Teste9");
            Assert.AreEqual(CsSituacaoCicloDomain.Cancelado, projeto01.Ciclos[1].CsSituacaoCiclo, "Teste10");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[2].CsSituacaoCiclo, "Teste11");
            Assert.AreEqual(CsSituacaoCicloDomain.NaoPlanejado, projeto01.Ciclos[3].CsSituacaoCiclo, "Teste12");

            projeto01.NbCicloTotalPlan = 2;
            projeto01.Save();

            Assert.AreEqual(new DateTime(2011, 06, 09), projeto01.Ciclos[0].DtInicio, "Teste13");
            Assert.AreEqual(new DateTime(2011, 06, 15), projeto01.Ciclos[0].DtTermino, "Teste14");

            Assert.AreEqual(new DateTime(2011, 06, 17), projeto01.Ciclos[1].DtInicio, "Teste15");
            Assert.AreEqual(new DateTime(2011, 06, 23), projeto01.Ciclos[1].DtTermino, "Teste16");
        }
        /// <summary>
        /// método SalvarEstoriaComMaisDeTrezePontos
        /// </summary>
        [TestMethod]
        public void SalvarEstoriaComMaisDeTrezePontos()
        {
            /*
                                                             * Cenario 1 - Criar um projeto01 incluir um ciclo a esse projeto  
                                                             * tentar incluir uma estória com mais de 13 pontos no ciclo - CT_3.02.01 
                                                             */

            //PASSO 1 

            Projeto projeto = ProjetoFactory.Criar(SessionTest, 100, "Nome");
            projeto.DtInicioPlan = new DateTime(2011, 04, 25);
            projeto.DtInicioReal = new DateTime(2011, 08, 25);
            projeto.NbCicloTotalPlan = 1;
            projeto.NbCicloDuracaoDiasPlan = 10;
            projeto.NbCicloDiasIntervalo = 1;
            projeto.Save();

            //PASSO 2

            Modulo modulo = ModuloFactory.Criar(SessionTest, projeto, "", true);
            Beneficiado beneficiado = BeneficiadoFactory.Criar(SessionTest, "TxDescricao", true);
            Estoria estoria = EstoriaFactory.Criar(SessionTest, modulo, "TxTitulo", "TxGostariaDe", "TxEntaoPoderei", beneficiado, "TxObservacoes", "TxReferencias", "TxDuvidas", true);
            estoria.NbTamanho = 8;
            estoria.Save();

            CicloDesenv ciclo = projeto.Ciclos[0];
            CicloDesenvEstoria cicloDesenvEstoria = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria, true);
        }

        /// <summary>
        /// Metodo que testa a exclusão de ciclos do projeto
        /// </summary>
        [TestMethod]
        public void TestarExcluirCiclos()
        {
            //Cenário: Criar o projeto01 com 20 ciclos e salvar
            //alterar a quantidade de ciclos para 10 e salvar
            //verificar quantos ciclos existem no projeto01
            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "Nome");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 25);
            projeto01.DtInicioReal = new DateTime(2011, 08, 25);
            projeto01.NbCicloTotalPlan = 20;
            projeto01.NbCicloDuracaoDiasPlan = 10;
            projeto01.NbCicloDiasIntervalo = 1;
            projeto01.Save();

            projeto01.NbCicloTotalPlan = 10;
            projeto01.NbCicloDuracaoDiasPlan = 20;
            projeto01.Save();

            Assert.AreEqual(10, projeto01.NbCicloTotalPlan, "O projeto deveria ter 10 ciclos");

        }

        /// <summary>
        /// Métoque que testa o deletar de um item de um ciclo e verificar a repriorização de estorias no backlog
        /// </summary>
        [TestMethod]
        public void TestarDeletarItemCicloRepriorizandoEstoria()
        {
            /**
            * Cenário 1: Será cadastrado um ciclo e adicionar estorias a este e verificar itens na lista de prioridades (não estarão lá)
            * Após isso deletar os itens no ciclo e verificar se os itens voltaram as prioridades do backlog
            */

            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "");
            projeto01.DtInicioPlan = new DateTime(2011, 04, 27);
            projeto01.DtInicioReal = new DateTime(2011, 04, 27);
            projeto01.NbCicloTotalPlan = 3;
            projeto01.Save();

            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Módulo 1", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();

            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descrição", true);

            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória01", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria01.NbPrioridade = 1;
            estoria01.NbTamanho = 3;
            estoria01.Save();

            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória02", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria02.NbPrioridade = 2;
            estoria02.NbTamanho = 5;
            estoria02.Save();

            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória03 - 12345678901234567890", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria03.NbPrioridade = 3;
            estoria03.NbTamanho = 13;
            estoria03.Save();

            Estoria estoria04 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória04 - 12345678901234567890", "GostariaDe", "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");
            estoria04.NbPrioridade = 4;
            estoria04.NbTamanho = 1;
            estoria04.Save();

            estoria01.RnSelecionarProjeto(projeto01);
            estoria02.RnSelecionarProjeto(projeto01);
            estoria03.RnSelecionarProjeto(projeto01);
            estoria04.RnSelecionarProjeto(projeto01);

            CicloDesenv ciclo = projeto01.Ciclos[0];

            // Pre-Condicao

            CicloDesenvEstoria cicloDesenvEstoria01 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria01, true);
            CicloDesenvEstoria cicloDesenvEstoria02 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria02, true);
            CicloDesenvEstoria cicloDesenvEstoria03 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria03, true);

            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloDesenvEstoria01.CsSituacao, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloDesenvEstoria02.CsSituacao, "A situação do item do ciclo deveria ser 'Não Iniciado'");
            Assert.AreEqual(CsSituacaoEstoriaCicloDomain.NaoIniciado, cicloDesenvEstoria03.CsSituacao, "A situação do item do ciclo deveria ser 'Não Iniciado'");

            // Pre-Condicao

            cicloDesenvEstoria01.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria02.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;

            cicloDesenvEstoria01.Save();
            cicloDesenvEstoria02.Save();
            cicloDesenvEstoria03.Save();

            projeto01.Ciclos[0].TxMeta = "TxMeta";
            projeto01.Ciclos[0].Save();

            // Pre-Condicao

            cicloDesenvEstoria01.Meta = true;
            cicloDesenvEstoria02.Meta = true;
            cicloDesenvEstoria03.Meta = true;
            cicloDesenvEstoria01.Save();

            projeto01.Ciclos[0].Save();
            cicloDesenvEstoria01.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria02.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;

            cicloDesenvEstoria01.Save();
            cicloDesenvEstoria02.Save();
            cicloDesenvEstoria03.Save();

            projeto01.Ciclos[0].Save();

            // Passo 1 - Exclui a estoria do ciclo.

            Assert.AreEqual(0, estoria02.NbPrioridade, "A estoria que estiver no ciclo deveria ter prioridade 0");
            Assert.AreEqual("Ciclo 1", estoria02._TxQuando, "A variável 'Quando?' deveria ser estar apontando para o ciclo");

            cicloDesenvEstoria01.Delete();

            projeto01.Ciclos[0].Save();

            Assert.AreEqual(1, estoria01.NbPrioridade, "A estória deveria ter voltado a ficar com prioridade 1");
            Assert.AreEqual("P1", estoria01._TxQuando, "A variável 'Quando?' deveria ser alterada para 'P1'");

            Assert.AreEqual(2, estoria04.NbPrioridade, "A estória deveria sido repriorizada");
            Assert.AreEqual("P2", estoria04._TxQuando, "A estória deveria sido repriorizada");

            Assert.AreEqual(0, estoria02.NbPrioridade, "A estoria que estiver no ciclo deveria ter permanecido com prioridade 0, pois o ciclo nao foi salvo");
            Assert.AreEqual(0, estoria03.NbPrioridade, "A estoria que estiver no ciclo deveria ter permanecido com prioridade 0, pois o ciclo nao foi salvo");
            Assert.AreEqual("Ciclo 1", estoria02._TxQuando, "A variável 'Quando?' deveria ter permanecido apontando para o ciclo");
            Assert.AreEqual("Ciclo 1", estoria03._TxQuando, "A variável 'Quando?' deveria ter permanecido apontando para o ciclo");

            projeto01.Ciclos[0].Save();

            // Passo 2 - Exclui mais de um estoria ao mesmo tempo
            cicloDesenvEstoria02.Delete();
            cicloDesenvEstoria03.Delete();

            string msg = "A estória deveria ter voltado para o topo do backlog";

            Assert.AreEqual(1, estoria03.NbPrioridade, msg);
            Assert.AreEqual("P1", estoria03._TxQuando, msg);

            Assert.AreEqual(2, estoria02.NbPrioridade, msg);
            Assert.AreEqual("P2", estoria02._TxQuando, msg);

            Assert.AreEqual(3, estoria01.NbPrioridade, msg);
            Assert.AreEqual("P3", estoria01._TxQuando, msg);

            Assert.AreEqual(4, estoria04.NbPrioridade, "A estória deveria sido repriorizada");
            Assert.AreEqual("P4", estoria04._TxQuando, "A estória deveria sido repriorizada");
        }

        /// <summary>
        /// Teste de alteração do tamanho da Estória (por consequência, alteração do tamanho do Ciclo)
        /// </summary>
        [TestMethod]
        public void TestarAlterarTamanhoEstoria()
        {
            // Criação do Projeto
            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "Projeto Teste");
            projeto01.DtInicioPlan = new DateTime(2012, 01, 30);
            projeto01.DtInicioReal = new DateTime(2012, 02, 17);
            projeto01.NbCicloTotalPlan = 3;
            projeto01.Save();

            // Criação do Módulo
            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Módulo 1", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();

            // Criação do Beneficiado
            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descrição", true);

            // Criação da Estória 1
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 01", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria01.NbPrioridade = 1;
            estoria01.NbTamanho = 3;
            estoria01.Save();

            // Criação da Estória 2
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 02", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria02.NbPrioridade = 2;
            estoria02.NbTamanho = 5;
            estoria02.Save();

            // Criação da Estória 3
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 03", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria03.NbPrioridade = 3;
            estoria03.NbTamanho = 13;
            estoria03.Save();

            // Criação da Estória 4
            Estoria estoria04 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 04", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria04.NbPrioridade = 4;
            estoria04.NbTamanho = 1;
            estoria04.Save();

            // Ligação das Estórias com o Projeto
            estoria01.RnSelecionarProjeto(projeto01);
            estoria02.RnSelecionarProjeto(projeto01);
            estoria03.RnSelecionarProjeto(projeto01);
            estoria04.RnSelecionarProjeto(projeto01);

            CicloDesenv ciclo = projeto01.Ciclos[0];

            // Ligação das Estórias com o Ciclo 1
            CicloDesenvEstoria cicloDesenvEstoria01 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria01, true);
            CicloDesenvEstoria cicloDesenvEstoria02 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria02, true);
            CicloDesenvEstoria cicloDesenvEstoria03 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria03, true);

            cicloDesenvEstoria01.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria02.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;

            cicloDesenvEstoria01.Save();
            cicloDesenvEstoria02.Save();
            cicloDesenvEstoria03.Save();

            // Verificação do total de pontos planejados para o Ciclo
            Assert.AreEqual(21, ciclo.NbPontosPlanejados, "O total de pontos planejados para o Ciclo 1 deveria ser 21.");

            // Alteração de tamanho da Estória 1
            estoria01.NbTamanho = 10;
            estoria01.Save();

            // Verificação do total de pontos planejados para o Ciclo
            Assert.AreEqual(28, ciclo.NbPontosPlanejados, "O total de pontos planejados para o Ciclo 1 deveria ser 28.");

            // Alteração de tamanho da Estória 4 (que não faz parte do Ciclo 1)
            estoria04.NbTamanho = 5;
            estoria04.Save();

            // Verificação do total de pontos planejados para o Ciclo
            Assert.AreEqual(28, ciclo.NbPontosPlanejados, "O total de pontos planejados para o Ciclo 1 deveria ser 28.");
        }

        /// <summary>
        /// Testar adicionar Estórias no Ciclo depois que o mesmo já existir
        /// </summary>
        [TestMethod]
        public void TestarAdicionarEstoriasNoCicloDepoisDeSalvo()
        {
            // Criação do Projeto
            Projeto projeto01 = ProjetoFactory.Criar(SessionTest, 100, "Projeto Teste");
            projeto01.DtInicioPlan = new DateTime(2012, 01, 30);
            projeto01.DtInicioReal = new DateTime(2012, 02, 17);
            projeto01.NbCicloTotalPlan = 3;
            projeto01.Save();

            // Criação do Módulo
            Modulo modulo01 = ModuloFactory.Criar(SessionTest, projeto01, "Módulo 1", false);
            modulo01.NbPontosTotal = 60;
            modulo01.Save();

            // Criação do Beneficiado
            Beneficiado beneficiado01 = BeneficiadoFactory.Criar(SessionTest, "Descrição", true);

            // Criação da Estória 1
            Estoria estoria01 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 01", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria01.NbPrioridade = 1;
            estoria01.NbTamanho = 3;
            estoria01.Save();

            // Criação da Estória 2
            Estoria estoria02 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 02", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria02.NbPrioridade = 2;
            estoria02.NbTamanho = 5;
            estoria02.Save();

            // Criação da Estória 3
            Estoria estoria03 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 03", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria03.NbPrioridade = 3;
            estoria03.NbTamanho = 13;
            estoria03.Save();

            // Criação da Estória 4
            Estoria estoria04 = EstoriaFactory.Criar(SessionTest, modulo01, "Estória 04", "GostariaDe",
                "EntaoPoderei", beneficiado01, "Observações", "Referências", "Dúvidas");

            estoria04.NbPrioridade = 4;
            estoria04.NbTamanho = 1;
            estoria04.Save();

            // Ligação das Estórias com o Projeto
            estoria01.RnSelecionarProjeto(projeto01);
            estoria02.RnSelecionarProjeto(projeto01);
            estoria03.RnSelecionarProjeto(projeto01);
            estoria04.RnSelecionarProjeto(projeto01);

            CicloDesenv ciclo = projeto01.Ciclos[0];

            // Ligação das Estórias com o Ciclo 1
            CicloDesenvEstoria cicloDesenvEstoria01 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria01, true);
            CicloDesenvEstoria cicloDesenvEstoria02 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria02, true);
            CicloDesenvEstoria cicloDesenvEstoria03 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria03, true);

            cicloDesenvEstoria01.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria02.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria03.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;

            cicloDesenvEstoria01.Save();
            cicloDesenvEstoria02.Save();
            cicloDesenvEstoria03.Save();

            // Verificação do total de pontos planejados para o Ciclo
            Assert.AreEqual(21, ciclo.NbPontosPlanejados, "O total de pontos planejados para o Ciclo 1 deveria ser 21.");

            // Ligação da Estória 4 com o Ciclo 1
            CicloDesenvEstoria cicloDesenvEstoria04 = CicloDesenvEstoriaFactory.Criar(SessionTest, ciclo, estoria04, true);
            cicloDesenvEstoria04.CsSituacao = CsSituacaoEstoriaCicloDomain.EmDesenv;
            cicloDesenvEstoria04.Save();

            // Verificação do total de pontos planejados para o Ciclo
            Assert.AreEqual(22, ciclo.NbPontosPlanejados, "O total de pontos planejados para o Ciclo 1 deveria ser 22.");
        }
    }
}
