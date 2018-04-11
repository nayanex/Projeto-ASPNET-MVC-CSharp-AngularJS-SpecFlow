using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;
using WexProject.BLL.Shared.DTOs.Custos;

namespace WexProject.BLL.DAOs.Analise.Custos
{
	public class GeralDao : IGeralDao
	{
		private static IGeralDao instancia = null;

		public static IGeralDao Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new GeralDao();
				}

				return instancia;
			}
		}

		private GeralDao()
		{
		}

		public Dictionary<ProjetoDto, List<CustoRubricaDto>> GetCustosProjetos()
		{
			Dictionary<ProjetoDto, List<CustoRubricaDto>> custosProjetos;

			using (var _db = new WexDb())
			{
				custosProjetos = (from rm in _db.RubricaMeses
								  join p in _db.Projetos on rm.Rubrica.Aditivo.ProjetoOid equals p.Oid
								  join tp in _db.TiposProjetos on p.TipoProjetoId equals tp.TipoProjetoId
								  where p.ProjetoMacroOid == null
								  group new CustoRubricaDto
								  {
									  Ano = rm.NbAno,
									  Mes = rm.CsMes,
									  TipoRubrica = rm.Rubrica.TipoRubricaId,
									  Planejado = rm.NbPlanejado,
									  Replanejado = rm.NbReplanejado,
									  Gasto = rm.NbGasto,
									  Entrada = rm.Rubrica.TipoRubrica.CsClasse == CsClasseRubrica.Aportes
								  } by new
								  {
									  Oid = p.Oid,
									  Nome = p.TxNome,
									  Status = p.CsSituacaoProjeto,
									  Classe = tp.ClasseProjetoId
								  })
								  .ToDictionary(c => new ProjetoDto { Oid = c.Key.Oid, Nome = c.Key.Nome, Status = (int)c.Key.Status, Classe = c.Key.Classe }, c => c.ToList());
			}

			return custosProjetos;
		}

		public Dictionary<TipoRubricaDto, List<CustoRubricaDto>> GetCustosRubricas(Guid projetoOid)
		{
			Dictionary<TipoRubricaDto, List<CustoRubricaDto>> custosRubricas;

			using (var _db = new WexDb())
			{
				custosRubricas = (from rm in _db.RubricaMeses
								  where rm.Rubrica.Aditivo.ProjetoOid == projetoOid
								  group new CustoRubricaDto
								  {
									  Ano = rm.NbAno,
									  Mes = rm.CsMes,
									  TipoRubrica = rm.Rubrica.TipoRubricaId,
									  Planejado = rm.NbPlanejado,
									  Replanejado = rm.NbReplanejado,
									  Gasto = rm.NbGasto
								  } by new
								  {
									  Nome = rm.Rubrica.TipoRubrica.TxNome,
									  Classe = rm.Rubrica.TipoRubrica.CsClasse
								  })
								  .ToDictionary(cr => new TipoRubricaDto() { Nome = cr.Key.Nome, Classe = cr.Key.Classe }, cr => cr.ToList());
			}

			return custosRubricas;
		}
	}
}
