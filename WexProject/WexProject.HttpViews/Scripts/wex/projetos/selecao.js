/// <reference path="wex.main.js" />
/// <reference path="wex.common.js" />
/// <reference path="wex.projetos.config.js" />
/*global WEX angular*/

(function () {
	"use strict";

	WEX.projetos.consertaDatas = function (projeto) {
		projeto.DataInicial = projeto.DataInicial.fromJSONDate();
		projeto.DataFinal = projeto.DataFinal.fromJSONDate();
	};

	WEX.projetos.ProjetosSelecaoModulo = angular.module("wex.projetos.selecao", ["wex.common"]);
	WEX.projetos.ProjetoSelecaoCtrl = ["$scope", "$rootScope", "Projetos", "Projeto", "ModalSelecao", "$location", function ($scope, $rootScope, Projetos, Projeto, ModalSelecao, $location) {
		$scope.projetos = Projetos.listaProjetos();

		$scope.modalSelecao = ModalSelecao;

		$scope.projeto = {};

		$scope.exibirInformacoes = function () {
			if ($scope.projeto.Oid) {
				$rootScope.$broadcast("mudouProjeto");

				$location.path("/Custos/" + $scope.projeto.Oid);

				ModalSelecao.fechar();
			}
		};
	}];

	WEX.projetos.ProjetosSelecaoModulo
		.factory("Projeto", [function () {
			var Projeto;

			Projeto = {};

			return Projeto;
		}])
		.factory("Projetos", ["$http", "$q", function ($http, $q) {
			var Projetos,
				projetos,
				urlBase,
				listaPresente;

			Projetos = {};
			projetos = [];
			urlBase = "/Custos/Projetos/";
			listaPresente = $q.defer();

			$http({
				url: urlBase,
				method: "get"
			})
				.success(function (data, status, headers, config) {
					projetos.replace(data.projetos);
					projetos.forEach(function (projeto) {
						projeto.value = projeto.Nome;
						WEX.projetos.consertaDatas(projeto);
					});

					listaPresente.resolve(projetos);
				})
				.error(function (data, status, headers, config) {
					listaPresente.reject();
				});

			Projetos.listaProjetos = function () {
				return projetos;
			};

			Projetos.get = function (projetoOid) {
				var projetoPresente;

				projetoPresente = $q.defer();

				listaPresente.promise.then(function (projetos) {
					var projeto;

					projeto = projetos.filter(function (projeto) {
						return projeto.Oid === projetoOid;
					});

					projeto = projeto[0];

					if (projeto === undefined) {
						buscarProjeto();
					} else {
						projetoPresente.resolve(projeto);
					}
				});

				return projetoPresente.promise;

				function buscarProjeto() {
					$http({
						url: urlBase + projetoOid,
						method: "get"
					})
						.success(function (data, status, headers, config) {
							WEX.projetos.consertaDatas(data.projeto);
							projetoPresente.resolve(data.projeto);
						});
				}
			};

			Projetos.listaCarregada = function () {
				return listaPresente.promise;
			};

			return Projetos;
		}])
		.factory("ModalSelecao", ["Modal", function (Modal) {
			var ModalSelecao,
				instancia;

			instancia = null;

			ModalSelecao = {
				podeFechar: false,
			};

			ModalSelecao.abrir = function () {
				instancia = Modal.abrir({
					templateUrl: "/Angular/template/modalSelecao",
					controller: WEX.projetos.ProjetoSelecaoCtrl,
				});

				return instancia.result;
			};

			ModalSelecao.fechar = function () {
				if (instancia !== null) {
					instancia.close();
					instancia = null;
				}
			};

			return ModalSelecao;
		}]);

	WEX.projetos.ProjetosSelecaoModulo
		.controller("ProjetoSelecaoCtrl", WEX.projetos.ProjetoSelecaoCtrl);
})();
