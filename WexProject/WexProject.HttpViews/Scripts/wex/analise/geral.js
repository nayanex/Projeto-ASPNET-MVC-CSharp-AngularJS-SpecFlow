/// <reference path="wex.main.js" />
/// <reference path="wex.common.js" />
/// <reference path="wex.analise.config.js" />
/*global WEX angular*/

(function () {
	"use strict";

	WEX.analise.geral.GeralModulo = angular.module("wex.analise.geral", ["wex.common"]);

	WEX.analise.geral.AnaliseCtrl = ["$scope", "CustosGeral", "Situacoes", "Classes", function ($scope, CustosGeral, Situacoes, Classes) {

		$scope.situacoes = Situacoes;

		$scope.classes = Classes;

		var loadingMessage = WEX.message.loading("Os custos estão sendo carregados...", { modal: true });

		CustosGeral.isCarregado().then(
			function sucesso() {
			    loadingMessage.close();
				CustosGeral.calculaAcumulado({});
			},
			function falha() {
			    loadingMessage.close();
			    WEX.message.error("Entre em contato com o administrador", { title: "Erro ao carregar os custos" });
			}
		);

		$scope.$watch("filtro", function (filtro) {
			if (filtro) {
				CustosGeral.filtrar(filtro);
				CustosGeral.calculaAcumulado(filtro);
			}
		}, true);
	}];
	WEX.analise.geral.ProjetoCtrl = ["$scope", "CustosRubricas", function ($scope, CustosRubricas) {
		$scope.projeto.aberto = false;

		$scope.toggleRubricas = function (projeto, tipo) {
			projeto.aberto = !projeto.aberto;

			if (projeto.aberto) {
				projeto.Rubricas = CustosRubricas.getRubricas(projeto.Oid, tipo);
			}
		};
	}];

	WEX.analise.geral.GeralModulo
		.directive("input", [function () {
			var regexData;

			regexData = /(\d{4})-(\d{2})/;

			return {
				restrict: "E",
				require: "?ngModel",
				link: function (scope, elem, attrs, ctrl) {
					var data;

					if (ctrl && attrs.type.toLowerCase() === "month") {
						ctrl.$parsers.push(function (valor) {
							if (typeof valor !== "undefined") {
								if ((data = valor.match(regexData)) !== null) {
									return {
										ano: parseInt(data[1], 10),
										mes: parseInt(data[2], 10)
									};
								}
							}
						});

						ctrl.$formatters.push(function (valor) {
							if (typeof valor === "undefined") {
								return "";
							}

							return valor.ano + "-" + valor.mes;
						});
					}
				}
			}
		}])
		.directive("valores", ["CustosGeral", "Anos", "Meses", function (CustosGeral, Anos, Meses) {
			return {
				restrict: "A",
				scope: true,
				link: function (scope, elem, attrs) {
					scope.anoAtual =
					scope.anoMinimo =
					scope.anoMaximo = 0;

					scope.mesAtual = 0;

					scope.anosAnteriores = "";
					scope.anosAtuais = "";
					scope.anosPosteriores = "";

					scope.anos;

					scope.geral = attrs.valores === "geral" ? CustosGeral.getGeral() : CustosGeral.getFluxo();

					scope.ano = function (anos) {
						anos = Anos(anos);

						return anos.getAno(scope.anoAtual, scope.mesAtual);
					};

					scope.meses = function () {
						if (scope.mesAtual === 0) {
							return Meses;
						}

						return Meses.concat(Meses).splice(scope.mesAtual, 12);
					};

					scope.retornarAno = function () {
						if (scope.anoAtual > scope.anoMinimo) {
							scope.anoAtual -= 1;
						} else {
							scope.anoAtual = scope.anoMinimo;
							scope.mesAtual = 0;
						}
					};

					scope.avancarAno = function () {
						scope.anoAtual += 1;

						if (scope.anoAtual >= scope.anoMaximo) {
							scope.anoAtual = scope.anoMaximo;
							scope.mesAtual = 0;
						}
					};

					scope.mostrarAno = function (ano) {
						scope.anoAtual = ano;
						scope.mesAtual = 0;
					};

					scope.isAtual = function (ano) {
						if (ano === scope.anoAtual) {
							return true;
						}

						if (ano === scope.anoAtual + 1 && scope.mesAtual > 0) {
							return true;
						}

						return false;
					};

					scope.isMultiAnos = function (ano) {
						if (ano === scope.anoAtual && scope.mesAtual > 0) {
							return true;
						}

						return false;
					};

					scope.$watch("geral", function (geral) {
						var anos;

						if (geral.Planejado) {
							anos = Anos(geral.Planejado.Anos);

							scope.anoMinimo =
								anos.primeiroAno();

							scope.anoMaximo =
								anos.ultimoAno();

							if (scope.anoAtual < scope.anoMinimo || scope.anoAtual > scope.anoMaximo) {
								scope.anoAtual = Math.min(Math.max(scope.anoAtual, scope.anoMinimo), scope.anoMaximo);
								scope.mesAtual = 0;
							}

							atualizaAnoInfo();
						}
					}, true);

					scope.$watch("[anoAtual, mesAtual]", atualizaAnoInfo, true);

					scope.avancarMes = function (event) {
						if (scope.mesAtual < 11) {
							if (scope.anoAtual < scope.anoMaximo) {
								scope.mesAtual += 1;
							}
						} else {
							scope.mesAtual = 0;
							scope.avancarAno();
						}
					};

					scope.retornarMes = function (event) {
						if (scope.mesAtual > 0) {
							scope.mesAtual -= 1;
						} else {
							scope.mesAtual = 11;
							scope.retornarAno();
						}
					};

					function atualizaAnoInfo() {
						var ano;

						scope.anos = [];

						for (ano = scope.anoMinimo; ano <= scope.anoMaximo; ano++) {
							scope.anos.push(ano);
						}
					}
				}
			};
		}])
		.directive("detalhamento", [function () {
			return {
				restrict: "A",
				scope: true,
				link: function (scope, elem, attrs) {
					scope.$watch(attrs.detalhamento, function (detalhamento) {
						if (detalhamento) {
							scope.detalhamento = detalhamento;

							detalhamento.aberto = false;
						}
					});

					scope.toggleAberto = function () {
						scope.detalhamento.aberto = !scope.detalhamento.aberto;
					};
				}
			};
		}]);

	WEX.analise.geral.GeralModulo
		.filter("projetos", ["Anos", "Memoizar", function (Anos, Memoizar) {
			return Memoizar(function (projetos, filtro) {
				if (projetos && filtro) {
					projetos = projetos.filter(function (projeto) {
						var pertence;

						pertence = true;

						if (typeof filtro.Nome === "string" && projeto.Nome.toLowerCase().indexOf(filtro.Nome.toLowerCase()) === -1) {
							pertence = false;
						}

						if (typeof filtro.Status === "number" && projeto.Status !== filtro.Status) {
							pertence = false;
						}

						if (typeof filtro.Classe === "number" && projeto.Classe !== filtro.Classe) {
							pertence = false;
						}

						return pertence;
					});

					if (typeof filtro.Inicio !== "undefined" && typeof filtro.Termino !== "undefined") {
						projetos = projetos.map(function (projeto) {
							var anos;

							projeto = angular.extend({}, projeto);

							anos = {};

							Anos(projeto.Anos).forAnos(function (meses, ano) {
								if (ano >= filtro.Inicio.ano && ano <= filtro.Termino.ano) {
									anos[ano] = meses.slice();
								}

								if (ano === filtro.Inicio.ano) {
									anos[ano] = Array(filtro.Inicio.mes - 1).concat(anos[ano].slice(filtro.Inicio.mes - 1));
								}

								if (ano === filtro.Termino.ano) {
									anos[ano] = anos[ano].slice(0, filtro.Termino.mes).concat(Array(12 - filtro.Termino.mes));
								}
							});

							projeto.Anos = anos;

							return projeto;
						});
					}
				}

				return projetos;
			});
		}]);

	WEX.analise.geral.GeralModulo
		.factory("CustosGeral", ["$http", "$filter", "$q", "Anos", "Memoizar", function ($http, $filter, $q, Anos, Memoizar) {
			var CustosGeral,
				carregando,
				custosProjetos,
				custosProjetosFiltrado,
				fluxoProjetos,
				fluxoProjetosFiltrado;

			CustosGeral = {};
			custosProjetosFiltrado = {};
			fluxoProjetosFiltrado = {};
			carregando = $q.defer();

			$http({
				url: "/Analise/Custos/Geral",
				method: "get"
			})
				.success(function (data, status, headers, config) {
					custosProjetos = data.custosProjetos;
					fluxoProjetos = data.fluxoProjetos;
					angular.copy(custosProjetos, custosProjetosFiltrado);
					angular.copy(fluxoProjetos, fluxoProjetosFiltrado);

					carregando.resolve();
				})
				.error(function (data, status, headers, config) {
					carregando.reject();
				});

			CustosGeral.isCarregado = function () {
				return carregando.promise;
			};
			CustosGeral.getGeral = function () {
				return custosProjetosFiltrado;
			};
			CustosGeral.getFluxo = function () {
				return fluxoProjetosFiltrado;
			};
			CustosGeral.calculaAcumulado = function (filtro) {

				acumuladoProjetos(fluxoProjetosFiltrado);
				acumuladoProjetos(custosProjetosFiltrado);

				function acumuladoProjetos(projetos) {
					var anos,
						ano,
						mes,
						ultimoMes,
						anoAtual,
						ultimoAno,
						acumulado,
						acumuladoMes;

					if (projetos.Resultado) {
						anos = Anos(projetos.Resultado.Anos);

						acumulado = {
							Valor: 0,
							Previsao: false
						};

						anoAtual = anos.primeiroAno();
						ultimoAno = anos.ultimoAno();

						if (typeof filtro.Inicio !== "undefined" && typeof filtro.Termino !== "undefined") {
							mes = filtro.Inicio.mes - 1;
							ultimoMes = filtro.Termino.mes - 1;
						} else {
							mes = 0;
							ultimoMes = 11;
						}

						ano = anos.getAno(anoAtual);

						while (anoAtual < ultimoAno || (anoAtual === ultimoAno && mes <= ultimoMes)) {
							acumuladoMes = projetos.Acumulado.Anos[anoAtual][mes];
							acumuladoMes.Valor =
							acumulado.Valor +=
								ano[mes].Valor;
							acumuladoMes.Previsao = acumulado.Previsao = acumuladoMes.Previsao || ano[mes].Previsao || acumulado.Previsao;

							mes++;
							if (mes === 12) {
								mes = 0;
								anoAtual++;
								ano = anos.getAno(anoAtual);
							}
						}

						projetos.Acumulado.Total = acumulado.Valor;
					}
				}
			};
			CustosGeral.recalcula = Memoizar(function (projetos, filtro, inverso) {

				var filtroFn,
					projetosPlanejado,
					projetosReal,
					anosPlanejado,
					anosReal,
					anosResultado,
					copiaProjetos;

				filtroFn = $filter("projetos");

				copiaProjetos = angular.copy(projetos);

				projetosPlanejado = filtroFn(copiaProjetos.Planejado.Projetos, filtro);
				projetosReal = filtroFn(copiaProjetos.Real.Projetos, filtro);

				anosPlanejado = Anos(copiaProjetos.Planejado.Anos);
				anosReal = Anos(copiaProjetos.Real.Anos);
				anosResultado = Anos(copiaProjetos.Resultado.Anos);

				if (typeof filtro.Inicio !== "undefined" && typeof filtro.Termino !== "undefined") {
					anosPlanejado.periodo(filtro.Inicio.ano, filtro.Termino.ano);
					anosReal.periodo(filtro.Inicio.ano, filtro.Termino.ano);
					anosResultado.periodo(filtro.Inicio.ano, filtro.Termino.ano);
					Anos(copiaProjetos.Acumulado.Anos).periodo(filtro.Inicio.ano, filtro.Termino.ano);
				}

				copiaProjetos.Planejado.Total = 0;
				copiaProjetos.Real.Total = 0;
				copiaProjetos.Resultado.Total = 0;

				anosPlanejado.forAnos(zeraAno);
				anosReal.forAnos(zeraAno);
				anosResultado.forAnos(zeraAno);

				projetosPlanejado.forEach(function (projeto) {
					projeto.Total = 0;

					Anos(projeto.Anos).forAnos(function (valoresAno, ano) {
						var mes,
							valorMes,
							planejado,
							resultado;

						planejado = anosPlanejado.getAno(ano);
						resultado = anosResultado.getAno(ano);

						for (mes = 0; mes < 12; mes++) {
							valorMes = valoresAno[mes];

							if (typeof valorMes === "object" && typeof valorMes.Valor === "number") {
								planejado[mes].Valor += valorMes.Valor;
								resultado[mes].Valor += inverso ? -valorMes.Valor : valorMes.Valor;

								projeto.Total += valorMes.Valor;
								copiaProjetos.Planejado.Total += valorMes.Valor;
								copiaProjetos.Resultado.Total += inverso ? -valorMes.Valor : valorMes.Valor;
							}
						}
					});
				});
				projetosReal.forEach(function (projeto) {
					projeto.Total = 0;

					Anos(projeto.Anos).forAnos(function (valoresAno, ano) {
						var mes,
							valorMes,
							real,
							resultado;

						real = anosReal.getAno(ano);
						resultado = anosResultado.getAno(ano);

						for (mes = 0; mes < 12; mes++) {
							valorMes = valoresAno[mes];

							if (typeof valorMes === "object" && typeof valorMes.Valor === "number") {
								real[mes].Valor += valorMes.Valor;
								resultado[mes].Valor -= inverso ? -valorMes.Valor : valorMes.Valor;

								real[mes].Previsao = real[mes].Previsao || valorMes.Previsao;
								resultado[mes].Previsao = resultado[mes].Previsao || valorMes.Previsao;

								projeto.Total += valorMes.Valor;
								copiaProjetos.Real.Total += valorMes.Valor;
								copiaProjetos.Resultado.Total -= inverso ? -valorMes.Valor : valorMes.Valor;
							}
						}
					});
				});

				copiaProjetos.Planejado.Projetos = projetosPlanejado;
				copiaProjetos.Real.Projetos = projetosReal;

				return copiaProjetos;

				function zeraAno(ano) {
					var mes;

					for (mes = 0; mes < 12; mes++) {
						ano[mes].Valor = null;
						ano[mes].Previsao = false;
					}
				}
			});
			CustosGeral.filtrar = function (filtro) {
				angular.copy(this.recalcula(custosProjetos, filtro), custosProjetosFiltrado);
				angular.copy(this.recalcula(fluxoProjetos, filtro, true), fluxoProjetosFiltrado);
			};

			return CustosGeral;
		}])
		.factory("CustosRubricas", ["$http", "Memoizar", function ($http, Memoizar) {
			var CustosRubricas;

			CustosRubricas = {};

			CustosRubricas.getRubricas = Memoizar(function (projetoOid, tipo) {
				var custosRubricas;

				custosRubricas = {};

				$http({
					url: "/Analise/Custos/Geral/" + tipo + "/" + projetoOid,
					method: "get"
				})
					.success(function (data, status, headers, config) {
						angular.copy(data.rubricas, custosRubricas);
					});

				return custosRubricas;
			});

			return CustosRubricas;
		}])
		.factory("Anos", [function () {
			return function (anos) {
				var Anos;

				Anos = {};

				Anos.primeiroAno = function () {
					var primeiroAno,
						ano;

					for (ano in anos) {
						if (anos.hasOwnProperty(ano)) {
							ano = parseInt(ano, 10);

							if (!primeiroAno) {
								primeiroAno = ano;
							} else {
								primeiroAno = ano < primeiroAno ? ano : primeiroAno;
							}
						}
					}

					return primeiroAno;
				};
				Anos.ultimoAno = function () {
					var ultimoAno,
						ano;

					for (ano in anos) {
						if (anos.hasOwnProperty(ano)) {
							ano = parseInt(ano, 10);

							if (!ultimoAno) {
								ultimoAno = ano;
							} else {
								ultimoAno = ano > ultimoAno ? ano : ultimoAno;
							}
						}
					}

					return ultimoAno;
				};
				Anos.getAno = function (ano, mes) {
					var anoAtual;

					if (mes) {
						anoAtual = Anos.getAno(ano).concat(Anos.getAno(ano + 1));

						return anoAtual.splice(mes, 12);
					}

					if (!anos || !anos.hasOwnProperty(ano)) {
						return new Array(12);
					}

					return anos[ano];
				};
				Anos.forAnos = function (anoFn) {
					var ultimoAno,
						ano;

					for (ano = Anos.primeiroAno(), ultimoAno = Anos.ultimoAno() ; ano <= ultimoAno; ano++) {
						anoFn(Anos.getAno(ano), ano);
					}
				};
				Anos.periodo = function (anoInicio, anoTermino) {
					Anos.forAnos(function (_, ano) {
						if (ano < anoInicio || ano > anoTermino) {
							delete anos[ano];
						}
					});

					return anos;
				};

				return Anos;
			};
		}]);

	WEX.analise.geral.GeralModulo
		.controller("AnaliseCtrl", WEX.analise.geral.AnaliseCtrl)
		.controller("ProjetoCtrl", WEX.analise.geral.ProjetoCtrl);
}());