using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.Test.Fixtures.Factory;
using WexProject.BLL.Models.Geral;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Test.UnitTest
{
    /// <summary>
    /// Testes da classe ParteInteressada
    /// </summary>
    [TestClass]
    public class ParteInteressadaTest : BaseTest
    {
        /// <summary>
        /// Verifica colaborador existente
        /// </summary>
        [TestMethod]
        public void VerificarColaborador()
        {
            /**
            * Passo 1: instanciação das variáveis necessárias.
            */

            Colaborador colaborador = ColaboradorFactory.CriarColaborador(SessionTest, "000", DateTime.Now, "nome@fpf.br", "Solicitacao", "Orcamento", "Historico", "nome.completo");
            colaborador.Save();

            Cargo cargo = CargoFactory.Criar(SessionTest, "Programador", true);
            cargo.Save();

            ParteInteressada parteinteressada = ParteInteressadaFactory.Criar(SessionTest, CsSimNao.Sim, colaborador, cargo, true);
            parteinteressada.Save();

            /*
                                                             * Cenário 1: Verificação se colaborador existente é válido  se selecionado para ser colaborador
                                                             */
            Assert.AreEqual( CsSimNao.Sim, parteinteressada.IsColaborador, "É um colaborador" );
            Assert.AreEqual("000", parteinteressada.Colaborador.TxMatricula, "Se é colaborador, tem a matrícula 000 oriundo da tabela colaborador");
            Assert.AreEqual("nome@fpf.br", parteinteressada.Colaborador.Usuario.Email, "Se é colaborador, tem o email nome@fpf.br oriundo da tabela colaborador");
            Assert.AreEqual("nome.completo", parteinteressada.Colaborador.Usuario.UserName, "Se é colaborador, ter o username 'nome.completo' oriundo da tabela colaborador");

            /*
                                                             * Cenário 2: Verificação de não ser um colaborador.
                                                             */
            parteinteressada.IsColaborador = CsSimNao.Não;

            ParteInteressadaFactory.SetarCampos(parteinteressada, parteinteressada.IsColaborador, null, cargo);

            parteinteressada.Save();
            Assert.AreEqual( CsSimNao.Não, parteinteressada.IsColaborador, "Não é um colaborador" );
            Assert.AreEqual("nome_usuario@fpf.br", parteinteressada.TxEmail, "Verificação de salvar na tabela parte interessada o email");
            Assert.AreEqual("Programador", parteinteressada.Cargo.TxDescricao, "Verificação de salvar na tabela parte interessada o cargo");
            Assert.AreEqual("000000000000", parteinteressada.TxTelefoneFixo, "Verificação de salvar na tabela parte interessada o telefone fixo");
            Assert.AreEqual("000000000000", parteinteressada.TxCelular, "Verificação de salvar na tabela parte interessada o celular");

            parteinteressada.IsColaborador = CsSimNao.Sim;
            parteinteressada.Save();
            Assert.AreEqual( CsSimNao.Sim, parteinteressada.IsColaborador, "É um colaborador" );
            Assert.AreEqual("000", parteinteressada.Colaborador.TxMatricula, "Se é colaborador, tem a matrícula 000 oriundo da tabela colaborador");
            Assert.AreEqual("nome@fpf.br", parteinteressada.Colaborador.Usuario.Email, "Se é colaborador, tem o email nome@fpf.br oriundo da tabela colaborador");
            Assert.AreEqual("nome.completo", parteinteressada.Colaborador.Usuario.UserName, "Se é colaborador, ter o username 'nome.completo' oriundo da tabela colaborador");

            Assert.AreEqual(1, new XPCollection<ParteInteressada>(SessionTest).Count);
        }
    }
}