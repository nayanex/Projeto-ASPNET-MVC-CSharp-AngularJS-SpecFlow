/// <reference path="wex.main.js" />
/// <reference path="wex.common.js" />
/// <reference path="wex.custos.config.js" />
/*global angular*/
/*global WEX*/

(function () {
	"use strict";

	WEX.custo.termoAditivo = {};

	WEX.custo.termoAditivo.TermoAditivoModulo = angular.module("wex.custos.termoAditivo", ["wex.common"]);

	WEX.custo.termoAditivo.TermoAditivoModulo
		.controller("TermoAditivoCtrl", ["$scope", "TermosAditivos", "TermoAditivo", "Patrocinadores", function ($scope, TermosAditivos, TermoAditivo, Patrocinadores) {
			var carregando;

			$scope.termosAditivos = TermosAditivos.get();

			carregando = WEX.feedback.carregando("<span>Termos Aditivos estão sendo carregados...</span>");

			TermosAditivos.listaCarregada().then(
				function sucesso() {
					carregando.carregado();
				},
				function falha() {
					carregando.carregado();
				}
			);

			$scope.patrocinadores = Patrocinadores.empresas();

			$scope.novoTermoAditivo = new TermoAditivo();

			$scope.adicionarTermoAditivo = function () {
				$scope.novoTermoAditivo.aberto = !$scope.novoTermoAditivo.aberto;
			};

			$scope.criarTermoAditivo = function () {
				TermosAditivos.salvar($scope.novoTermoAditivo).then(function () {
					$scope.novoTermoAditivoForm.$setPristine();
					$scope.novoTermoAditivo = new TermoAditivo();
				});
			};
		}])

		.directive("termoAditivo", ["TermosAditivos", function (TermosAditivos) {
			return {
				restrict: "A",
				link: function (scope, elem, attrs) {
					scope.projetos = function (termoAditivo) {
						var projetos;

						projetos = "";

						if (termoAditivo.Projetos.length === 0) {
							return "nenhum projeto associado.";
						} else {
							termoAditivo.Projetos.forEach(function (projeto) {
								if (projetos !== "") {
									projetos += ", ";
								}
								projetos += projeto.Nome;
							});
						}

						return projetos;
					};

					scope.excluirTermoAditivo = function (termoAditivo) {
						TermosAditivos.remover(termoAditivo);
					};

					scope.alternarTermoAditivo = function (termoAditivo) {
						termoAditivo.aberto = !termoAditivo.aberto;
					};

					scope.associarProjetos = function (termoAditivo) {
						TermosAditivos.associarProjetos(termoAditivo);
					};

					scope.disassociarProjeto = function (termoAditivo, projeto) {
						TermosAditivos.disassociarProjeto(termoAditivo, projeto);
					};
				}
			};
		}])

		.factory("ProjetosMacros", ["$http", "$q", "Data", function ($http, $q, Data) {
			var ProjetosMacros;

			ProjetosMacros = {};

			ProjetosMacros.get = function (filtro) {
				var projetosDeferido;

				projetosDeferido = $q.defer();

				$http({
					url: "/Projetos/Macros",
					params: filtro,
					method: "get"
				})
					.success(function sucesso(data, status, headers, config) {
						projetosDeferido.resolve(data.projetos.map(function (projeto) {
							projeto.DataInicial = Data.fromJSONDate(projeto.DataInicial);
							projeto.DataFinal = Data.fromJSONDate(projeto.DataFinal);

							return projeto
						}));
					})
					.error(function falha(data, status, headers, config) {
						projetosDeferido.reject();
					});

				return projetosDeferido.promise;
			};

			return ProjetosMacros;
		}])
		.factory("TermosAditivos", ["$http", "$q", "Modal", "TermoAditivo", "Data", function ($http, $q, Modal, TermoAditivo, Data) {
			var TermosAditivos,
				termosAditivos,
				listaPresente,
				urlBase;

			TermosAditivos = {};
			termosAditivos = [];
			listaPresente = $q.defer();

			urlBase = "/Custos/TermosAditivos/";

			TermosAditivos.get = function () {
				$http({
					url: urlBase,
					method: "get"
				})
					.success(function sucesso(data, status, headers, config) {
						termosAditivos.replace(data.termosAditivos);

						termosAditivos.forEach(function (termoAditivo) {
							termoAditivo.DataInicio = Data.fromJSONDate(termoAditivo.DataInicio);
							termoAditivo.DataTermino = Data.fromJSONDate(termoAditivo.DataTermino);

							termoAditivo.aberto = false;
						});

						listaPresente.resolve();

						WEX.feedback.infoGeral("<span>Termos Aditivos carregados.</span>");
					})
					.error(function falha(data, status, headers, config) {
						listaPresente.reject();

						WEX.feedback.erroGeral("<span>Erro ao carregar Termos Aditivos!</span>");
					});

				return termosAditivos;
			};

			TermosAditivos.remover = function (termoAditivo) {
				var indice,
					removeDeferred;

				indice = termosAditivos.indexOf(termoAditivo);
				removeDeferred = $q.defer();

				if (termoAditivo.TermoAditivoId === 0) {
					termosAditivos.splice(indice, 1);
					WEX.feedback.infoGeral("<span>Termo Aditivo removido!</span>");
				} else {
					$http({
						url: urlBase + termoAditivo.TermoAditivoId,
						method: "delete"
					})
						.success(sucesso)
						.error(falha);
				}

				function sucesso(data, status, headers, config) {
					removeDeferred.resolve();

					termosAditivos.splice(indice, 1);
					WEX.feedback.infoGeral("<span>Termo Aditivo removido!</span>");
				}

				function falha(data, status, headers, config) {
					var titulo,
						mensagem;

					titulo = "Confirmar exclusão?";
					mensagem = "O Termo Aditivo possui valores, tem certeza que deseja excluir?";

					if (status === 405 && typeof config.data === "undefined") {
						Modal.confirmar(titulo, mensagem).then(function (resultado) {
							if (resultado === "sim") {
								config.data = {
									force: true
								};

								$http(config)
									.success(sucesso)
									.error(falha);
							}
						});
					} else {
						removeDeferred.reject();

						WEX.feedback.erroGeral("<span>Erro ao remover Termo Aditivo!</span>");
					}
				}

				return removeDeferred.promise;
			};

			TermosAditivos.salvar = function (termoAditivo) {
				var config,
					salvarDeferido;

				salvarDeferido = $q.defer();

				if (TermoAditivo.isValido(termoAditivo)) {
					config = {
						url: urlBase,
						method: "post",
						data: new TermoAditivo.Dto(termoAditivo)
					};

					if (termoAditivo.TermoAditivoId !== 0) {
						config.url += termoAditivo.TermoAditivoId;
						config.method = "put";
					}

					$http(config)
						.success(function (data, status, headers, config) {
							termoAditivo.TermoAditivoId = data.id;
							termosAditivos.push(termoAditivo);

							salvarDeferido.resolve();
							WEX.feedback.infoGeral("<span>Termo Aditivo criado!</span>");
						})
						.error(function (data, status, headers, config) {
							salvarDeferido.reject();
							WEX.feedback.erroGeral("<span>Erro ao criar Termo Aditivo!</span>");
						});
				} else {
					salvarDeferido.reject();
				}

				return salvarDeferido.promise;
			};

			TermosAditivos.listaCarregada = function () {
				return listaPresente.promise;
			};

			TermosAditivos.associarProjetos = function (termoAditivo) {
				Modal({
					templateUrl: "projetosExistentes",
					controller: ["$scope", "ProjetosMacros", function ($scope, ProjetosMacros) {
						$scope.projetos = [];

						ProjetosMacros.get({
							TermoAditivoId: "null"
						}).then(
							function sucesso(projetos) {
								WEX.feedback.infoGeral("<span>Projetos recuperados com sucesso!</span>");
								$scope.projetos = projetos;
							},
							function falha() {
								WEX.feedback.erroGeral("<span>Falha ao recuperar projetos!</span>");
								$scope.$close();
							}
						);

						$scope.alternarProjeto = function (projeto) {
							projeto.selecionado = !projeto.selecionado;
						};

						$scope.projetosSelecionados = function () {
							return $scope.projetos.filter(function (projeto) {
								return projeto.selecionado;
							});
						};
					}],
					windowClass: "modalAssociacaoProjeto"
				}).then(function (projetos) {
					if (angular.isArray(projetos)) {
						projetos.forEach(function (projeto) {
							$http({
								url: urlBase + termoAditivo.TermoAditivoId + "/Projetos",
								method: "post",
								data: {
									projetoOid: projeto.Oid
								}
							})
								.success(function sucesso(data, status, headers, config) {
									termoAditivo.Projetos.push(projeto);
								})
								.error(function falha(data, status, headers, config) {
									WEX.feedback.erroGeral("<span>Erro ao associar projeto " + projeto.Nome + "|</span>");
								});
						});
					}
				});
			};

			TermosAditivos.disassociarProjeto = function (termoAditivo, projeto) {
				$http({
					url: urlBase + termoAditivo.TermoAditivoId + "/Projetos/" + projeto.Oid,
					method: "delete"
				})
					.success(sucesso)
					.error(falha);

				function sucesso(data, status, headers, config) {
					termoAditivo.Projetos.splice(termoAditivo.Projetos.indexOf(projeto), 1);

					WEX.feedback.infoGeral("<span>Projeto disassociado com sucesso!</span>");
				}

				function falha(data, status, headers, config) {
					var titulo,
						mensagem;

					titulo = "Confirmar disassociação?";
					mensagem = "O Projeto possui valores, tem certeza que deseja disassociar do Termo Aditivo?";

					if (status === 405 && typeof config.data === "undefined") {
						Modal.confirmar(titulo, mensagem).then(function (resultado) {
							if (resultado === "sim") {
								config.data = {
									force: true
								};

								$http(config)
									.success(sucesso)
									.error(falha);
							}
						});
					} else {
						WEX.feedback.erroGeral("<span>Erro ao disassociar Projeto!</span>");
					}
				}
			};

			return TermosAditivos;
		}])
		.factory("TermoAditivo", ["AdicionaErro", function (adicionaErro) {
			var TermoAditivo,
				contador = 0;

			TermoAditivo = function TermoAditivo(obj) {
				obj = obj || {};

				contador += 1;

				this.TermoAditivoId = obj.TermoAditivoId || 0;
				this.Nome = obj.Nome || "Termo Aditivo " + contador;
				this.DataInicio = obj.DataInicio || null;
				this.DataTermino = obj.DataTermino || null;
				this.Descricao = obj.Descricao || "";
				this.Projetos = obj.Projetos || [];
				this.Patrocinador = obj.Patrocinador || null;

			};

			TermoAditivo.setContador = function (count) {
				contador = count;
			};

			TermoAditivo.isValido = function (termoAditivo) {
				var valido;

				valido = true;
				delete termoAditivo.erros;

				if (termoAditivo.Descricao.trim() === "") {
					adicionaErro(termoAditivo, "Descricao", "Descricao Inválida!");
					valido = false;
				}

				if (termoAditivo.Patrocinador === null) {
					adicionaErro(termoAditivo, "Patrocinador", "Patrocinador Inválido!");
					valido = false;
				}

				if (!(termoAditivo.DataInicio instanceof Date)) {
					adicionaErro(termoAditivo, "DataInicio", "Data inválida!");
					valido = false;
				}

				if (!(termoAditivo.DataTermino instanceof Date)) {
					adicionaErro(termoAditivo, "DataTermino", "Data inválida!");
					valido = false;
				}

				if (termoAditivo.DataInicio >= termoAditivo.DataTermino) {
					adicionaErro(termoAditivo, "DataTermino", "Período inválido!");
					valido = false;
				}

				return valido;
			};

			TermoAditivo.Dto = function TermoAditivoDto(termoAditivo) {
				if (typeof termoAditivo !== "object" || !(termoAditivo instanceof TermoAditivo)) {
					throw new Error("Esperado instância de TermoAditivo, passado " + termoAditivo.constructor.name);
				}

				this.TermoAditivoId = termoAditivo.TermoAditivoId;
				this.Nome = termoAditivo.Nome;
				this.DataInicio = termoAditivo.DataInicio;
				this.DataTermino = termoAditivo.DataTermino;
				this.Descricao = termoAditivo.Descricao;
				this.Projetos = termoAditivo.Projetos;
				this.Patrocinador = termoAditivo.Patrocinador;
			};

			return TermoAditivo;
		}]);
}());
