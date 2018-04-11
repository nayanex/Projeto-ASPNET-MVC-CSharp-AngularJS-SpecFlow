using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.BOs.Geral;
using WexProject.BLL.DAOs.Analise.Custos;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Shared.Domains.Custos;
using WexProject.BLL.Shared.DTOs.Analise.Custos.Geral;
using WexProject.Library.Libs.Nullable;

namespace WexProject.BLL.BOs.Analise.Custos
{
	using WexProject.Library.Libs.Collection;

	public class GeralBo
	{
		private static GeralBo instancia = null;

		public static GeralBo Instancia
		{
			get
			{
				if (instancia == null)
				{
					instancia = new GeralBo();
				}

				return instancia;
			}
		}
		public IGeralDao geralDao { get; set; }

		private GeralBo()
		{
			geralDao = GeralDao.Instancia;
		}

		public void AnaliseGeral(out GeralDto custosDto, out GeralDto fluxoDto)
		{
			custosDto = new GeralDto();
			fluxoDto = new GeralDto();
			var custosProjetos = geralDao.GetCustosProjetos();

			foreach (var custosProjeto in custosProjetos)
			{
				var custoProjetoPlanejado = new DetalhamentoProjetoDto();
				var custoProjetoReal = new DetalhamentoProjetoDto();
				var fluxoProjetoPlanejado = new DetalhamentoProjetoDto();
				var fluxoProjetoReal = new DetalhamentoProjetoDto();

				custoProjetoPlanejado.Nome = custoProjetoReal.Nome =
				fluxoProjetoPlanejado.Nome = fluxoProjetoReal.Nome =
					custosProjeto.Key.Nome;
				custoProjetoPlanejado.Oid = custoProjetoReal.Oid =
				fluxoProjetoPlanejado.Oid = fluxoProjetoReal.Oid =
					custosProjeto.Key.Oid;
				custoProjetoPlanejado.Status = custoProjetoReal.Status =
				fluxoProjetoPlanejado.Status = fluxoProjetoReal.Status =
					custosProjeto.Key.Status;
				custoProjetoPlanejado.Classe = custoProjetoReal.Classe =
				fluxoProjetoPlanejado.Classe = fluxoProjetoReal.Classe =
					custosProjeto.Key.Classe;

				foreach (var entradaSaida in custosProjeto.Value.GroupBy(cp => cp.Entrada))
				{
					DetalhamentoProjetoDto projetoPlanejado;
					DetalhamentoProjetoDto projetoReal;

					if (entradaSaida.Key)
					{
						projetoPlanejado = fluxoProjetoPlanejado;
						projetoReal = fluxoProjetoReal;
					}
					else
					{
						projetoPlanejado = custoProjetoPlanejado;
						projetoReal = custoProjetoReal;
					}

					foreach (var ano in entradaSaida.GroupBy(cp => cp.Ano))
					{
						var anoAtual = ano.Key.ToString();

						if (!custosDto.Planejado.Anos.ContainsKey(anoAtual))
						{
							custosDto.Planejado.Anos.Add(anoAtual, new Custo[12]);
							custosDto.Real.Anos.Add(anoAtual, new Custo[12]);
							custosDto.Resultado.Anos.Add(anoAtual, new Custo[12]);
							custosDto.Acumulado.Anos.Add(anoAtual, new Custo[12]);

							fluxoDto.Planejado.Anos.Add(anoAtual, new Custo[12]);
							fluxoDto.Real.Anos.Add(anoAtual, new Custo[12]);
							fluxoDto.Resultado.Anos.Add(anoAtual, new Custo[12]);
							fluxoDto.Acumulado.Anos.Add(anoAtual, new Custo[12]);
						}

						projetoPlanejado.Anos.Add(anoAtual, new Custo[12]);
						projetoReal.Anos.Add(anoAtual, new Custo[12]);

						foreach (var mes in ano.GroupBy(cp => cp.Mes))
						{
							var mesAtual = (int)mes.Key - 1;

							foreach (var rubrica in mes.GroupBy(cp => cp.TipoRubrica))
							{
								Custo valorPlanejado = rubrica.Sum(cp => cp.Planejado);
								Custo valorReplanejado = rubrica.Sum(cp => cp.Replanejado);
								Custo valorReal = rubrica.Sum(cp => cp.Gasto);

								if (!valorReal.Valor.HasValue)
								{
									if (valorReplanejado.Valor.HasValue)
									{
										valorReal = valorReplanejado;
									}
									else
									{
										valorReal = valorPlanejado;
									}

									if (valorReal.Valor.HasValue)
									{
										valorReal.Previsao = true;
									}
								}

								var valorDiferenca = valorPlanejado - valorReal;

								projetoPlanejado.Total += (Decimal)valorPlanejado;
								projetoPlanejado.Anos[anoAtual][mesAtual] += valorPlanejado;

								projetoReal.Total += (Decimal)valorReal;
								projetoReal.Anos[anoAtual][mesAtual] += valorReal;

								if (entradaSaida.Key)
								{
									fluxoDto.Planejado.Total += (Decimal)valorPlanejado;
									fluxoDto.Planejado.Anos[anoAtual][mesAtual] += valorPlanejado;

									fluxoDto.Real.Total += (Decimal)valorReal;
									fluxoDto.Real.Anos[anoAtual][mesAtual] += valorReal;

									fluxoDto.Resultado.Total -= (Decimal)valorDiferenca;
									fluxoDto.Resultado.Anos[anoAtual][mesAtual] -= valorDiferenca;
								}
								else
								{
									custosDto.Planejado.Total += (Decimal)valorPlanejado;
									custosDto.Planejado.Anos[anoAtual][mesAtual] += valorPlanejado;

									custosDto.Real.Total += (Decimal)valorReal;
									custosDto.Real.Anos[anoAtual][mesAtual] += valorReal;

									custosDto.Resultado.Total += (Decimal)valorDiferenca;
									custosDto.Resultado.Anos[anoAtual][mesAtual] += valorDiferenca;
								}
							}
						}
					}
				}

				custosDto.Planejado.Projetos.Add(custoProjetoPlanejado);
				custosDto.Real.Projetos.Add(custoProjetoReal);
				fluxoDto.Planejado.Projetos.Add(fluxoProjetoPlanejado);
				fluxoDto.Real.Projetos.Add(fluxoProjetoReal);
			}
		}

		public RubricasDto AnaliseProjeto(Guid projetoOid, String tipo, Boolean fluxoCaixa = false)
		{
			var rubricasDto = new RubricasDto();
			var custosRubricas = geralDao.GetCustosRubricas(projetoOid);
			var planejado = tipo.Equals("planejado", StringComparison.CurrentCultureIgnoreCase);

			foreach (var custosRubrica in custosRubricas)
			{
				var rubrica = new DetalhamentoRubricaDto();

				rubrica.Nome = custosRubrica.Key.Nome;

				foreach (var ano in custosRubrica.Value.GroupBy(cr => cr.Ano))
				{
					var anoAtual = ano.Key.ToString();

					rubrica.Anos.Add(anoAtual, new Custo[12]);

					foreach (var mes in ano.GroupBy(cr => cr.Mes))
					{
						var mesAtual = (int)mes.Key - 1;

						Custo valorMes;

						if (planejado)
						{
							valorMes = mes.Sum(cr => cr.Planejado);
						}
						else
						{
							valorMes = mes.Sum(cr => cr.Gasto);

							if (!valorMes.Valor.HasValue)
							{
								valorMes = mes.Sum(cr => cr.Replanejado);

								if (!valorMes.Valor.HasValue)
								{
									valorMes = mes.Sum(cr => cr.Planejado);
								}

								if (valorMes.Valor.HasValue)
								{
									valorMes.Previsao = true;
								}
							}
						}

						rubrica.Total += (Decimal)valorMes;
						rubrica.Anos[anoAtual][mesAtual] = valorMes;
					}
				}

				if (custosRubrica.Key.Classe.HasFlag(CsClasseRubrica.Desenvolvimento))
				{
					rubricasDto.Desenvolvimento.Add(rubrica);
				}
				else if (custosRubrica.Key.Classe.HasFlag(CsClasseRubrica.Administrativo))
				{
					rubricasDto.Administrativa.Add(rubrica);
				}
			}

			return rubricasDto;
		}
	}
}
