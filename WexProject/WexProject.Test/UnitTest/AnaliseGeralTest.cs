using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WexProject.BLL.BOs.Analise.Custos;
using WexProject.BLL.DAOs.Analise.Custos;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.Test.UnitTest
{
	[TestClass]
	public class AnaliseGeralTest
	{
		[TestMethod]
		public void DeveProcessarTodosOsAnosSemAportes()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(1, custo.Planejado.Anos.Count, "Deveria ter um ano planejado");
			Assert.IsTrue(custo.Planejado.Anos.ContainsKey("2014"), "Deveria ter planejamento do ano de 2014");
			Assert.AreEqual(1, custo.Real.Anos.Count, "Deveria ter um ano em gasto real");
			Assert.IsTrue(custo.Real.Anos.ContainsKey("2014"), "Deveria ter gasto real do ano de 2014");
			Assert.AreEqual(1, custo.Resultado.Anos.Count, "Deveria ter um ano em resultado");
			Assert.IsTrue(custo.Resultado.Anos.ContainsKey("2014"), "Deveria ter resultado do ano de 2014");
			Assert.AreEqual(1, custo.Acumulado.Anos.Count, "Deveria ter um ano em acumulado");
			Assert.IsTrue(custo.Acumulado.Anos.ContainsKey("2014"), "Deveria ter acumulado do ano de 2014");

			Assert.AreEqual(1, fluxo.Planejado.Anos.Count, "Deveria ter um ano planejado");
			Assert.IsTrue(fluxo.Planejado.Anos.ContainsKey("2014"), "Deveria ter planejamento do ano de 2014");
			Assert.AreEqual(1, fluxo.Real.Anos.Count, "Deveria ter um ano em gasto real");
			Assert.IsTrue(fluxo.Real.Anos.ContainsKey("2014"), "Deveria ter gasto real do ano de 2014");
			Assert.AreEqual(1, fluxo.Resultado.Anos.Count, "Deveria ter um ano em resultado");
			Assert.IsTrue(fluxo.Resultado.Anos.ContainsKey("2014"), "Deveria ter resultado do ano de 2014");
			Assert.AreEqual(1, fluxo.Acumulado.Anos.Count, "Deveria ter um ano em acumulado");
			Assert.IsTrue(fluxo.Acumulado.Anos.ContainsKey("2014"), "Deveria ter acumulado do ano de 2014");
		}

		[TestMethod]
		public void DeveProcessarTodosOsProjetosSemAportes()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(3, custo.Planejado.Projetos.Count, "Deveria ter três projetos planejados");
			Assert.AreEqual(3, custo.Real.Projetos.Count, "Deveria ter três projetos em gasto real");

			Assert.AreEqual(3, fluxo.Planejado.Projetos.Count, "Deveria ter três projetos planejados");
			Assert.AreEqual(3, fluxo.Real.Projetos.Count, "Deveria ter três projetos em gasto real");
		}

		[TestMethod]
		public void DeveCriarProjetosEmFluxoECustosParaCadaProjetoSemAportes()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(custo.Planejado.Projetos[0].Oid, fluxo.Planejado.Projetos[0].Oid, "Deve existir um projeto em fluxo para o primeiro projeto em custos");
			Assert.AreEqual(custo.Planejado.Projetos[1].Oid, fluxo.Planejado.Projetos[1].Oid, "Deve existir um projeto em fluxo para o segundo projeto em custos");
			Assert.AreEqual(custo.Planejado.Projetos[2].Oid, fluxo.Planejado.Projetos[2].Oid, "Deve existir um projeto em fluxo para o terceiro projeto em custos");
		}

		[TestMethod]
		public void DeveSomarTodosOsValoresPlanejadosEmCadaMesSemAportes()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(10000, custo.Planejado.Anos["2014"][0].Valor, "Deveria ter R$10.000,00 em Janeiro de 2014");
			Assert.AreEqual(47500, custo.Planejado.Anos["2014"][1].Valor, "Deveria ter R$47.500,00 em Fevereiro de 2014");
			Assert.AreEqual(47500, custo.Planejado.Anos["2014"][2].Valor, "Deveria ter R$47.500,00 em Março de 2014");
			Assert.AreEqual(22500, custo.Planejado.Anos["2014"][3].Valor, "Deveria ter R$22.500,00 em Abril de 2014");
			Assert.AreEqual(22500, custo.Planejado.Anos["2014"][4].Valor, "Deveria ter R$22.500,00 em Maio de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][5].Valor, "Deveria não ter valor em Junho de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][6].Valor, "Deveria não ter valor em Julho de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][7].Valor, "Deveria não ter valor em Agosto de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][8].Valor, "Deveria não ter valor em Setembro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][9].Valor, "Deveria não ter valor em Outubro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][10].Valor, "Deveria não ter valor em Novembro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][11].Valor, "Deveria não ter valor em Dezembro de 2014");

		}

		[TestMethod]
		public void DeveProcessarTodosOsAnosComAportes()
		{
			// Arrange
			PreparaUmAnoComAportes();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(1, custo.Planejado.Anos.Count, "Deveria ter um ano planejado");
			Assert.IsTrue(custo.Planejado.Anos.ContainsKey("2014"), "Deveria ter planejamento do ano de 2014");
			Assert.AreEqual(1, custo.Real.Anos.Count, "Deveria ter um ano em gasto real");
			Assert.IsTrue(custo.Real.Anos.ContainsKey("2014"), "Deveria ter gasto real do ano de 2014");
			Assert.AreEqual(1, custo.Resultado.Anos.Count, "Deveria ter um ano em resultado");
			Assert.IsTrue(custo.Resultado.Anos.ContainsKey("2014"), "Deveria ter resultado do ano de 2014");
			Assert.AreEqual(1, custo.Acumulado.Anos.Count, "Deveria ter um ano em acumulado");
			Assert.IsTrue(custo.Acumulado.Anos.ContainsKey("2014"), "Deveria ter acumulado do ano de 2014");

			Assert.AreEqual(1, fluxo.Planejado.Anos.Count, "Deveria ter um ano planejado");
			Assert.IsTrue(fluxo.Planejado.Anos.ContainsKey("2014"), "Deveria ter planejamento do ano de 2014");
			Assert.AreEqual(1, fluxo.Real.Anos.Count, "Deveria ter um ano em gasto real");
			Assert.IsTrue(fluxo.Real.Anos.ContainsKey("2014"), "Deveria ter gasto real do ano de 2014");
			Assert.AreEqual(1, fluxo.Resultado.Anos.Count, "Deveria ter um ano em resultado");
			Assert.IsTrue(fluxo.Resultado.Anos.ContainsKey("2014"), "Deveria ter resultado do ano de 2014");
			Assert.AreEqual(1, fluxo.Acumulado.Anos.Count, "Deveria ter um ano em acumulado");
			Assert.IsTrue(fluxo.Acumulado.Anos.ContainsKey("2014"), "Deveria ter acumulado do ano de 2014");
		}

		[TestMethod]
		public void DeveProcessarTodosOsProjetosComAportes()
		{
			// Arrange
			PreparaUmAnoComAportes();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(3, custo.Planejado.Projetos.Count, "Deveria ter três projetos planejados");
			Assert.AreEqual(3, custo.Real.Projetos.Count, "Deveria ter três projetos em gasto real");

			Assert.AreEqual(3, fluxo.Planejado.Projetos.Count, "Deveria ter três projetos planejados");
			Assert.AreEqual(3, fluxo.Real.Projetos.Count, "Deveria ter três projetos em gasto real");
		}

		[TestMethod]
		public void DeveCriarProjetosEmFluxoECustosParaCadaProjetoComAportes()
		{
			// Arrange
			PreparaUmAnoComAportes();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(custo.Planejado.Projetos[0].Oid, fluxo.Planejado.Projetos[0].Oid, "Deve existir um projeto em fluxo para o primeiro projeto em custos");
			Assert.AreEqual(custo.Planejado.Projetos[1].Oid, fluxo.Planejado.Projetos[1].Oid, "Deve existir um projeto em fluxo para o segundo projeto em custos");
			Assert.AreEqual(custo.Planejado.Projetos[2].Oid, fluxo.Planejado.Projetos[2].Oid, "Deve existir um projeto em fluxo para o terceiro projeto em custos");
		}

		[TestMethod]
		public void DeveSomarTodosOsValoresPlanejadosEmCadaMesComAportes()
		{
			// Arrange
			PreparaUmAnoComAportes();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(10000, custo.Planejado.Anos["2014"][0].Valor, "Deveria ter R$10.000,00 em Janeiro de 2014");
			Assert.AreEqual(47500, custo.Planejado.Anos["2014"][1].Valor, "Deveria ter R$47.500,00 em Fevereiro de 2014");
			Assert.AreEqual(47500, custo.Planejado.Anos["2014"][2].Valor, "Deveria ter R$47.500,00 em Março de 2014");
			Assert.AreEqual(22500, custo.Planejado.Anos["2014"][3].Valor, "Deveria ter R$22.500,00 em Abril de 2014");
			Assert.AreEqual(22500, custo.Planejado.Anos["2014"][4].Valor, "Deveria ter R$22.500,00 em Maio de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][5].Valor, "Deveria não ter valor em Junho de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][6].Valor, "Deveria não ter valor em Julho de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][7].Valor, "Deveria não ter valor em Agosto de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][8].Valor, "Deveria não ter valor em Setembro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][9].Valor, "Deveria não ter valor em Outubro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][10].Valor, "Deveria não ter valor em Novembro de 2014");
			Assert.AreEqual(null, custo.Planejado.Anos["2014"][11].Valor, "Deveria não ter valor em Dezembro de 2014");

		}

		[TestMethod]
		public void DeveCalcularGastoRealParaMesesComGastoLancado()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Assert.AreEqual(10000, custo.Real.Projetos[1].Anos["2014"][4].Valor, "Deveria ter R$20.000,00 em Maio de 2014 como Gasto Real do segundo Projeto");
			Assert.IsFalse(custo.Real.Projetos[1].Anos["2014"][4].Previsao, "Deveria não ser Previsão o valor em Maio de 2014 como Gasto Real do segund Projeto");
			Assert.AreEqual(20000, custo.Real.Projetos[2].Anos["2014"][1].Valor, "Deveria ter R$20.000,00 em Fevereiro de 2014 como Gasto Real do terceiro Projeto");
			Assert.IsFalse(custo.Real.Projetos[2].Anos["2014"][1].Previsao, "Deveria não ser Previsão o valor em Fevereiro de 2014 como Gasto Real do terceiro Projeto");
		}

		[TestMethod]
		public void DeveUsarValorReplanejadoQuandoValorRealForNuloEHouverValorReplanejado()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Custo[] anoProjeto;

			anoProjeto = custo.Real.Projetos[0].Anos["2014"];

			Assert.AreEqual(8000, anoProjeto[0].Valor, "Deveria ter R$8.000,00 em Janeiro de 2014 em Gasto Real no primeiro Projeto");
			Assert.IsTrue(anoProjeto[0].Previsao, "Deveria ser Previsão o valor em Janeiro de 2014 em Gasto Real no primeiro Projeto");
			Assert.AreEqual(8000, anoProjeto[1].Valor, "Deveria ter R$8.000,00 em Fevereiro de 2014 em Gasto Real no primeiro Projeto");
			Assert.IsTrue(anoProjeto[1].Previsao, "Deveria ser Previsão o valor em Fevereiro de 2014 em Gasto Real no primeiro Projeto");
			Assert.AreEqual(8000, anoProjeto[2].Valor, "Deveria ter R$8.000,00 em Março de 2014 em Gasto Real no primeiro Projeto");
			Assert.IsTrue(anoProjeto[2].Previsao, "Deveria ser Previsão o valor em Março de 2014 em Gasto Real no primeiro Projeto");
			Assert.AreEqual(8000, anoProjeto[3].Valor, "Deveria ter R$8.000,00 em Abril de 2014 em Gasto Real no primeiro Projeto");
			Assert.IsTrue(anoProjeto[3].Previsao, "Deveria ser Previsão o valor em Abril de 2014 em Gasto Real no primeiro Projeto");
			Assert.AreEqual(18000, anoProjeto[4].Valor, "Deveria ter R$18.000,00 em Maio de 2014 em Gasto Real no primeiro Projeto");
			Assert.IsTrue(anoProjeto[4].Previsao, "Deveria ser Previsão o valor em Maio de 2014 em Gasto Real no primeiro Projeto");

			anoProjeto = custo.Real.Projetos[1].Anos["2014"];

			Assert.AreEqual(20000, anoProjeto[1].Valor, "Deveria ter R$20.000,00 em Fevereiro de 2014 em Gasto Real no segundo Projeto");
			Assert.IsTrue(anoProjeto[1].Previsao, "Deveria ser Previsão o valor em Fevereiro de 2014 em Gasto Real no segundo Projeto");
			Assert.AreEqual(0, anoProjeto[2].Valor, "Deveria ter R$0,00 em Março de 2014 em Gasto Real no segundo Projeto");
			Assert.IsTrue(anoProjeto[2].Previsao, "Deveria ser Previsão o valor em Março de 2014 em Gasto Real no segundo Projeto");
			Assert.AreEqual(20000, anoProjeto[3].Valor, "Deveria ter R$20.000,00 em Abril de 2014 em Gasto Real no segundo Projeto");
			Assert.IsTrue(anoProjeto[3].Previsao, "Deveria ser Previsão o valor em Abril de 2014 em Gasto Real no segundo Projeto");
		}

		[TestMethod]
		public void DeveUsarValorPlanejadoQuandoValorRealForNuloEValorReplanejadoForNulo()
		{
			// Arrange
			PreparaUmAno();

			// Act
			GeralDto custo;
			GeralDto fluxo;

			GeralBo.Instancia.AnaliseGeral(out custo, out fluxo);

			// Assert
			Custo[] anoProjeto;

			anoProjeto = custo.Real.Projetos[2].Anos["2014"];

			Assert.AreEqual(25000, anoProjeto[2].Valor, "Deveria ter R$25.000,00 em Março de 2014 em Gasto Real no terceiro Projeto");
			Assert.IsTrue(anoProjeto[2].Previsao, "Deveria ser Previsão o valor em Março de 2014 em Gasto Real no terceiro Projeto");
		}

		private static void PreparaUmAno()
		{
			var projetos = new Dictionary<ProjetoDto, List<CustoRubricaDto>>();

			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Projeto Legal",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Janeiro,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Abril,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Maio,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 18000,
					Gasto = null,
					Entrada = false
				},
			});
			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Atestado",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 20000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 0,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Abril,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 20000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Maio,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = null,
					Gasto = 10000,
					Entrada = false
				}
			});
			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Android",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 25000,
					Replanejado = null,
					Gasto = 20000,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 25000,
					Replanejado = null,
					Gasto = null,
					Entrada = false
				}
			});

			Mock<IGeralDao> daoMock = new Mock<IGeralDao>();
			daoMock.Setup(o => o.GetCustosProjetos()).Returns(projetos);
			GeralBo.Instancia.geralDao = daoMock.Object;
		}

		private static void PreparaUmAnoComAportes()
		{
			var projetos = new Dictionary<ProjetoDto, List<CustoRubricaDto>>();

			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Projeto Legal",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Janeiro,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Abril,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 8000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Maio,
					TipoRubrica = 1,
					Planejado = 10000,
					Replanejado = 18000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Janeiro,
					TipoRubrica = 23,
					Planejado = null,
					Replanejado = null,
					Gasto = null,
					Entrada = true
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 23,
					Planejado = 25000,
					Replanejado = null,
					Gasto = 10000,
					Entrada = true
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 23,
					Planejado = null,
					Replanejado = null,
					Gasto = 12000,
					Entrada = true
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Abril,
					TipoRubrica = 23,
					Planejado = 25000,
					Replanejado = 13000,
					Gasto = null,
					Entrada = true
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Maio,
					TipoRubrica = 23,
					Planejado = null,
					Replanejado = 10000,
					Gasto = null,
					Entrada = true
				}
			});
			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Atestado",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 20000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 0,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Abril,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = 20000,
					Gasto = null,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Maio,
					TipoRubrica = 1,
					Planejado = 12500,
					Replanejado = null,
					Gasto = 10000,
					Entrada = false
				}
			});
			projetos.Add(new ProjetoDto()
			{
				Oid = Guid.NewGuid(),
				Nome = "Android",
				Status = (int)CsProjetoSituacaoDomain.EmAndamento,
				Classe = 1
			}, new List<CustoRubricaDto>()
			{
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Fevereiro,
					TipoRubrica = 1,
					Planejado = 25000,
					Replanejado = null,
					Gasto = 20000,
					Entrada = false
				},
				new CustoRubricaDto() {
					Ano = 2014,
					Mes = CsMesDomain.Marco,
					TipoRubrica = 1,
					Planejado = 25000,
					Replanejado = null,
					Gasto = null,
					Entrada = false
				}
			});

			Mock<IGeralDao> daoMock = new Mock<IGeralDao>();
			daoMock.Setup(o => o.GetCustosProjetos()).Returns(projetos);
			GeralBo.Instancia.geralDao = daoMock.Object;
		}
	}
}
