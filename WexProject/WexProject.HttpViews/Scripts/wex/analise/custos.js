/// <reference path="wex.main.js" />
/// <reference path="wex.common.js" />
/// <reference path="wex.analise.config.js" />
/*global WEX angular*/

(function () {
    "use strict";

    WEX.analise.AnaliseModulo = angular.module("wex.analise.custos", ["wex.common"]);

    WEX.analise.ProjetosCtrl = ["$scope", "$location", "Projetos", "Meses", function ($scope, $location, Projetos, Meses) {
        var carregando;

        $scope.projetos = Projetos.get();

        carregando = WEX.feedback.carregando("<span>Os projetos estão sendo carregados...</span>");

        Projetos.isCarregado().then(
			function sucesso() {
			    carregando.carregado();
			},
			function falha() {
			    carregando.carregado();
			}
		);

        $scope.hintTempo = "hint--top hint--rounded hint--success";

        $scope.hintOrcamento = "hint--bottom hint--rounded hint--success";

        $scope.selecionado = "";

        $scope.mesesNome = Meses;

        $scope.abreProjeto = function (projeto) {
            var abrir,
				url;

            abrir = !projeto.aberto;

            if (abrir) {
                url = "/Analise/Custos/" + projeto.Oid;
            } else {
                url = "/Analise/Custos/";
            }

            $location.path(url);
        };

        $scope.$on("$locationChangeSuccess", function (event, novaUrl) {
            var idRegex,
				projetoOid;

            idRegex = /http:\/\/.*\/Analise\/Custos(\/)?/i;

            projetoOid = novaUrl.replace(idRegex, "");

            if (projetoOid === "") {
                $scope.selecionado = "";
            } else {
                $scope.selecionado = projetoOid;
            }
        });
    }];

    WEX.analise.AnaliseCtrl = ["$scope", "Projetos", function ($scope, Projetos) {

        $scope.isMesExtratoProjetoVisivel = function (orcamentoDespesasDto) {
            if (!orcamentoDespesasDto.DespesasDesenvolvimento &&
                !orcamentoDespesasDto.DespesasAdministrativas) {
                return false;
            }
            return ((orcamentoDespesasDto.DespesasDesenvolvimento.Valor != 0) ||
            (orcamentoDespesasDto.DespesasAdministrativas.Valor != 0) ||
            (orcamentoDespesasDto.OrcamentoAprovadoDesenvolvimento != 0) ||
            (orcamentoDespesasDto.OrcamentoAprovadoAdministracao != 0));
        };

        $scope.isMesExtratoFinanceiroVisivel = function (despesasAportesDto) {
            if (!despesasAportesDto.AportePlanejado &&
                !despesasAportesDto.AporteRealizado) {
                return false;
            }
            return ((despesasAportesDto.AportePlanejado != 0) ||
            (despesasAportesDto.AporteRealizado.Valor != 0) ||
            (despesasAportesDto.DespesasReaisAdministrativas.Valor != 0) ||
            (despesasAportesDto.DespesasReaisDesenvolvimento.Valor != 0));
        };

        $scope.exibirMenssagemDeTempoOcorrido = function (projeto) {
            if (projeto.TempoConsumido === 1) {
                if (projeto.TempoPlanejado === 1) {
                    return projeto.TempoConsumido + " mes realizado de " + projeto.TempoPlanejado + " mes planejado";
                }
                    return projeto.TempoConsumido + " mes realizado de " + projeto.TempoPlanejado + " meses planejados";
            }
            else if (projeto.TempoPlanejado === 1) {
                    return projeto.TempoConsumido + " meses realizados de " + projeto.TempoPlanejado + " mes planejado";
            }
            return projeto.TempoConsumido + " meses realizados de " + projeto.TempoPlanejado + " meses planejados";
        };

        $scope.$watch("selecionado", function (novoProjetoOid) {

            if ($scope.projeto.Oid === novoProjetoOid) {

                $scope.projeto.aberto = true;

                Projetos.get($scope.projeto.Oid).then(
					function sucesso(projeto) {
					    angular.extend($scope.projeto, projeto);
					},
					function falha() {
					    WEX.feedback.erroGeral("<span>Erro ao recuperar Detalhes de Projeto!</span>");
					}
				);

            } else {

                $scope.projeto.aberto = false;

            }

        });

    }];

    WEX.analise.AnaliseModulo
		.directive("wexProgresso", [function () {
		    return {
		        restrict: "A",
		        scope: {
		            progresso: "=wexProgresso",
		            titulo: "@wexProgressoTitulo"
		        },
		        templateUrl: "/Angular/template/wexProgresso",
		        controller: ["$scope", function ($scope) {

		            $scope.maxBarra = function (porcentagem) {
		                return Math.max(100, porcentagem);
		            };

		            $scope.limiteBarraInicial = 8;

		            $scope.valorBarra = function (porcentagem) {
		                if (porcentagem > 100) {
		                    porcentagem = 100;
		                }
		                return porcentagem || 0;
		            };

		            $scope.excedenteBarra = function (porcentagem) {
		                if (porcentagem >= 100) {
		                    porcentagem -= 100;
		                } else {
		                    porcentagem = 0;
		                }
		                return porcentagem;
		            };

		            $scope.posicaoValor = function (porcentagem) {
		                var posicao;

		                if (porcentagem <= 100) {
		                    posicao = 100 - porcentagem;
		                } else {
		                    posicao = Math.round($scope.excedenteBarra(porcentagem) * 100 / $scope.maxBarra(porcentagem));
		                }

		                return posicao + 0.5;
		            };
		        }],
		        link: function (scope, elem, attrs) {
		            // Valor empírico (baseado em experimentos)
		            scope.tamanhoTitulo = scope.titulo.length * 0.4 - 0.6;
		        }
		    };
		}]);

    WEX.analise.AnaliseModulo
		.factory("Projetos", ["$http", "$q", function ($http, $q) {
		    var Projetos,
				projetos,
				carregando;

		    Projetos = {};
		    projetos = [];
		    carregando = $q.defer();

		    Projetos.get = function (projetoOid) {
		        var carregadoProjeto;

		        if (typeof projetoOid === "undefined") {
		            $http({
		                url: "/Analise/Custos",
		                method: "get",
		            })
						.success(function (data, status, headers, config) {
						    projetos.replace(data.projetos);
						    carregando.resolve();
						})
						.error(function (data, status, header, config) {
						    WEX.feedback.erroGeral("<span>Erro ao recuperar Listagem de Projetos!</span>");
						    carregando.reject();
						});

		            return projetos;
		        } else {
		            carregadoProjeto = $q.defer();

		            $http({
		                url: "/Analise/Custos/" + projetoOid,
		                method: "get",
		            })
						.success(function (data, status, headers, config) {
						    carregadoProjeto.resolve(data.projeto);
						})
						.error(function (data, status, header, config) {
						    carregadoProjeto.reject();
						});

		            return carregadoProjeto.promise;
		        }
		    };

		    Projetos.isCarregado = function () {
		        return carregando.promise;
		    };

		    return Projetos;
		}]);

    WEX.analise.AnaliseModulo
		.controller("ProjetosCtrl", WEX.analise.ProjetosCtrl)
		.controller("AnaliseCtrl", WEX.analise.AnaliseCtrl);

    WEX.analise.AnaliseModulo.config(["$locationProvider", function ($locationProvider) {
        $locationProvider.html5Mode(true);
    }]);
}());