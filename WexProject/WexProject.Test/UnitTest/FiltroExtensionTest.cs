using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WexProject.BLL.Entities.Geral;
using WexProject.Library.Libs.Collection;

namespace WexProject.Test.UnitTest
{
	[TestClass]
	public class FiltroExtensionTest
	{
		public List<Projeto> Projetos { get; set; }

		[TestInitialize]
		public void InitFiltroExtensionTest()
		{
			Projetos = new List<Projeto>();

			Projetos.Add(new Projeto()
			{
				Oid = Guid.NewGuid(),
				TxNome = "Projeto de Teste",
				ProjetoMacroOid = null,
				NbValor = 100000,
				DtInicioPlan = new DateTime(2013, 07, 01),
				DtTerminoPlan = new DateTime(2013, 12, 31),
				TipoProjetoId = null,
			});
			Projetos.Add(new Projeto()
			{
				Oid = Guid.NewGuid(),
				TxNome = "Projeto de Teste Micro 1",
				ProjetoMacroOid = Projetos[0].Oid,
				NbValor = 75000,
				DtInicioPlan = new DateTime(2013, 07, 01),
				DtTerminoPlan = new DateTime(2013, 08, 30),
				TipoProjetoId = null,
			});
			Projetos.Add(new Projeto()
			{
				Oid = Guid.NewGuid(),
				TxNome = "Projeto de Teste Micro 2",
				ProjetoMacroOid = Projetos[0].Oid,
				NbValor = 25000,
				DtInicioPlan = new DateTime(2013, 09, 01),
				DtTerminoPlan = new DateTime(2013, 12, 31),
				TipoProjetoId = null,
			});
			Projetos.Add(new Projeto()
			{
				Oid = Guid.NewGuid(),
				TxNome = "Projeto de Produção",
				ProjetoMacroOid = null,
				NbValor = 50000,
				DtInicioPlan = new DateTime(2014, 01, 01),
				DtTerminoPlan = new DateTime(2014, 12, 31),
				TipoProjetoId = 4,
			});
			Projetos.Add(new Projeto()
			{
				Oid = Guid.NewGuid(),
				TxNome = "Projeto de Produção Bruta",
				ProjetoMacroOid = Projetos[3].Oid,
				NbValor = 50000,
				DtInicioPlan = new DateTime(2014, 01, 01),
				DtTerminoPlan = new DateTime(2014, 06, 30),
				TipoProjetoId = null,
			});
		}

		[TestMethod]
		public void DeveFiltrarPorDecimal()
		{
			List<Projeto> projetosFiltrados;

			// Preparando filtro
			var filtro = new Dictionary<string, object>();
			filtro.Add("NbValor", (Decimal)50000);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(2, projetosFiltrados.Count, "Deveria ter encontrado 2 projetos.");
			Assert.AreEqual(Projetos[3], projetosFiltrados[0], "O quarto Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[4], projetosFiltrados[1], "O quinto Projeto devia ter sido filtrado.");
		}

		[TestMethod]
		public void DeveFiltrarPorString()
		{
			List<Projeto> projetosFiltrados;
			Dictionary<string, object> filtro;

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("TxNome", "Projeto de Teste Macro");

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(0, projetosFiltrados.Count, "Deveria não ter encontrado projetos.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("TxNome", "Projeto de Teste");

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[0], projetosFiltrados[0], "O primeiro Projeto devia ter sido filtrado.");
		}

		[TestMethod]
		public void DeveFiltrarPorNullableInt()
		{
			List<Projeto> projetosFiltrados;
			Dictionary<string, object> filtro;

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("TipoProjetoId", (int?)null);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(4, projetosFiltrados.Count, "Deveria ter encontrado 4 projetos.");
			Assert.AreEqual(Projetos[0], projetosFiltrados[0], "O primeiro Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[1], projetosFiltrados[1], "O segundo Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[2], projetosFiltrados[2], "O terceiro Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[4], projetosFiltrados[3], "O quinto Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("TipoProjetoId", (int?)4);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[3], projetosFiltrados[0], "O quarto Projeto devia ter sido filtrado.");
		}

		[TestMethod]
		public void DeveFiltrarPorNullableGuid()
		{
			List<Projeto> projetosFiltrados;
			Dictionary<string, object> filtro;

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("ProjetoMacroOid", (Guid?)null);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(2, projetosFiltrados.Count, "Deveria ter encontrado 2 projetos.");
			Assert.AreEqual(Projetos[0], projetosFiltrados[0], "O primeiro Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[3], projetosFiltrados[1], "O quarto Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("ProjetoMacroOid", (Guid?)Projetos[0].Oid);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(2, projetosFiltrados.Count, "Deveria ter encontrado 2 projetos.");
			Assert.AreEqual(Projetos[1], projetosFiltrados[0], "O segundo Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[2], projetosFiltrados[1], "O terceiro Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("ProjetoMacroOid", (Guid?)Projetos[3].Oid);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[4], projetosFiltrados[0], "O quinto Projeto devia ter sido filtrado.");
		}

		[TestMethod]
		public void DeveFiltrarPorDateTime()
		{
			List<Projeto> projetosFiltrados;
			Dictionary<string, object> filtro;

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("DtInicioPlan", new DateTime(2013, 07, 01));

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(2, projetosFiltrados.Count, "Deveria ter encontrado 2 projetos.");
			Assert.AreEqual(Projetos[0], projetosFiltrados[0], "O primeiro Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[1], projetosFiltrados[1], "O segundo Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("DtInicioPlan", new DateTime(2014, 01, 01));

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(2, projetosFiltrados.Count, "Deveria ter encontrado 2 projetos.");
			Assert.AreEqual(Projetos[3], projetosFiltrados[0], "O quarto Projeto devia ter sido filtrado.");
			Assert.AreEqual(Projetos[4], projetosFiltrados[1], "O quinto Projeto devia ter sido filtrado.");
		}

		[TestMethod]
		public void DeveFiltrarPorGuid()
		{
			List<Projeto> projetosFiltrados;
			Dictionary<string, object> filtro;

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("Oid", Projetos[0].Oid);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[0], projetosFiltrados[0], "O primeiro Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("Oid", (Guid?)Projetos[1].Oid);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[1], projetosFiltrados[0], "O segundo Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("Oid", Projetos[4].Oid);

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(1, projetosFiltrados.Count, "Deveria ter encontrado 1 projetos.");
			Assert.AreEqual(Projetos[4], projetosFiltrados[0], "O quinto Projeto devia ter sido filtrado.");

			// Preparando filtro
			filtro = new Dictionary<string, object>();
			filtro.Add("Oid", Guid.NewGuid());

			projetosFiltrados = Projetos.Filtra(new Filtro<Projeto>(filtro)).ToList();

			Assert.AreEqual(0, projetosFiltrados.Count, "Deveria não ter encontrado projetos.");
		}
	}
}
