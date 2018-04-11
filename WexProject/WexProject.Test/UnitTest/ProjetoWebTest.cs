using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.Shared.DTOs.Projeto;
using WexProject.BLL.Shared.DTOs.Rh;

namespace WexProject.Test.UnitTest
{

    /**
     * @summary Testes relacionados ao cadastro de projeto via Web
     * */
    [TestClass]
    public class ProjetoWebTest
    {

        [TestMethod]
        public void VerificarExistenciaGerenteProjeto()
        {

            ColaboradorDto gerenteProjeto = new ColaboradorDto();
            gerenteProjeto.OidColaborador = Guid.NewGuid();
            gerenteProjeto.TxNomeCompletoColaborador = "Gerente A";

            DadosBasicoProjetoDto projeto = new DadosBasicoProjetoDto();
            projeto.Gerente = gerenteProjeto;

            Assert.AreEqual(true, ProjetoBo.Instancia.ExisteGerenteProjeto(projeto), "O projeto deveria possuir Gerente de Projeto");

        }

        [TestMethod]
        public void VerificarExistenciaCentroCusto()
        {

            CentroCustoDto centroCusto = new CentroCustoDto();
            centroCusto.IdCentroCusto = 24;
            centroCusto.Nome = "Centro de Custo Teste";

			DadosBasicoProjetoDto projeto = new DadosBasicoProjetoDto();
            projeto.CentroCusto = centroCusto;

            Assert.AreEqual(true, ProjetoBo.Instancia.ExisteCentroCusto(projeto), "O projeto deveria possuir o Centro de Custo");

        }

        [TestMethod]
        public void VerificarExistenciaClientes()
        {

            ClienteDto cliente1 = new ClienteDto();
            cliente1.IdCliente = Guid.NewGuid();
            cliente1.Nome = "Cliente 1";

            ClienteDto cliente2 = new ClienteDto();
            cliente2.IdCliente = Guid.NewGuid();
            cliente2.Nome = "Cliente 2";

            List<ClienteDto> clientes = new List<ClienteDto>();
            clientes.Add(cliente1);
            clientes.Add(cliente2);

			DadosBasicoProjetoDto projeto = new DadosBasicoProjetoDto();
            projeto.Clientes = clientes;

            Assert.AreEqual(true, ProjetoBo.Instancia.ExistemClientes(projeto), "O projeto deveria possuir clientes");

        }

        [TestMethod]
        public void VerificarSeProjetoMacroPossuiCentroCusto()
        {

            CentroCustoDto centroCusto = new CentroCustoDto();
            centroCusto.IdCentroCusto = 24;
            centroCusto.Nome = "Centro de Custo Teste";

			DadosBasicoProjetoDto projeto = new DadosBasicoProjetoDto();
            projeto.CentroCusto = centroCusto;

            Assert.AreEqual(true, ProjetoBo.Instancia.VerificarProjetoMacro(projeto.IdProjeto), "O projeto deveria ser um projeto macro");

            Assert.AreEqual(true, ProjetoBo.Instancia.ExisteCentroCusto(projeto), "O projeto deveria possuir o Centro de Custo");

        }

        [TestMethod]
        public void VerificarSeProjetoMicroPossuiCentroCusto()
        {

            CentroCustoDto centroCusto = new CentroCustoDto();
            centroCusto.IdCentroCusto = 24;
            centroCusto.Nome = "Centro de Custo Teste";

			DadosBasicoProjetoDto projetoMacro = new DadosBasicoProjetoDto();
            projetoMacro.IdProjeto = Guid.NewGuid();

			DadosBasicoProjetoDto projetoFilho = new DadosBasicoProjetoDto();
            projetoFilho.CentroCusto = centroCusto;
            projetoFilho.ProjetoMacro = projetoMacro;

            Assert.AreEqual(true, ProjetoBo.Instancia.VerificarProjetoMicro(projetoFilho.IdProjeto), "O projeto deveria ser um projeto micro");

            Assert.AreNotEqual(true, ProjetoBo.Instancia.ExisteCentroCusto(projetoFilho), "O projeto não deveria possuir o Centro de Custo");

        }

    }

}