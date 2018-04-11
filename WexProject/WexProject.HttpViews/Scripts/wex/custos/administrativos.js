/// <reference path="~/Scripts/angular-1.2.16/angular.min.js" />
/// <reference path="~/Scripts/angular-1.2.16/angular-resource.min.js" />
/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.common.js" />
/// <reference path="~/Scripts/wex.custos.config.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/config.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/resource.js" />
/*global WEX angular*/

(function () {
    "use strict";

    WEX.custo.administrativos.Modulo = angular.module("wex.custos.administrativos", ["wex.service", "wex.common"]);

    WEX.custo.administrativos.Modulo.controller("CustosAdministrativosCtrl",
        ["$scope", "CustosAdministrativosViewModel", "CustosAdministrativosCommand", function ($scope, viewModel, command) {

            var FLAG = WEX.custo.administrativos.Flags;

            $scope.viewModel = viewModel;

            $scope.carregandoCustosRubricas = function () {
                return viewModel.flags.check(FLAG.CARREGANDO_CUSTOS_RUBRICAS);
            };

            $scope.contemCustosRubricas = function () {
                return viewModel.custosRubricas.length > 0;
            };

            $scope.alternarCustosProjetos = function (custosRubricaDto) {
                (viewModel.custosProjetos.containsKey(custosRubricaDto.TipoRubricaId)) ?
                    command.limparCustosProjetos(custosRubricaDto.TipoRubricaId):
                    command.listarCustosProjetos(custosRubricaDto.TipoRubricaId);
            };

            $scope.mostrandoCustosProjetos = function (custosRubricaDto) {
                return viewModel.flags.check(FLAG.MOSTRANDO_CUSTOS_PROJETOS, custosRubricaDto.TipoRubricaId);
            }

            $scope.carregandoCustosProjetos = function (custosRubricaDto) {
                return viewModel.flags.check(FLAG.CARREGANDO_CUSTOS_PROJETOS, custosRubricaDto.TipoRubricaId);
            }

            $scope.contemCustosProjetos = function (custosRubricaDto) {
                return viewModel.custosProjetos.containsKey(custosRubricaDto.TipoRubricaId)
                    && viewModel.custosProjetos.get(custosRubricaDto.TipoRubricaId).length > 0;
            };

            $scope.salvandoDespesaReal = function (custosRubricaDto, custosProjetoDto) {
            	return viewModel.flags.check(FLAG.SALVANDO_DESPESA_REAL, custosRubricaDto.TipoRubricaId, custosProjetoDto.IdProjeto);
            };

            $scope.salvarDespesaReal = function (custosRubricaDto, custosProjetoDto) {

                command.salvarDespesaReal(custosRubricaDto, custosProjetoDto).then(function success(message) {

                    var successMessage = WEX.message.success(message);
                    setTimeout(successMessage.close, 3000);

                }, function error(message) {

                    WEX.message.error(message, { title: "Erro ao salvar Despesa Real" });

                });

            };

            $scope.decorarValorNegativo = function (valor) {
                return valor < 0;
            };

            $scope.copiarValor = function (custosDto) {
                custosDto.DespesaReal = custosDto.SaldoDisponivel;
            };

            $scope.copiarSaldoDisponivelProjeto = function (custosProjetoDto) {
                command.copiarSaldoDisponivelProjeto(custosProjetoDto);
            };

            $scope.copiarSaldosDisponiveisProjetos = function (tipoRubricaId) {
                command.copiarSaldosDisponiveisProjetos(tipoRubricaId);
            };
            

            $scope.$watch("viewModel.dataConsulta", function () {

                var loadingMessage = WEX.message.loading("Carregando custos administrativos", { modal: true });

                command.listarCustosRubricas().then(function success() {

                    loadingMessage.close();

                }, function error(message) {

                    loadingMessage.close();

                    WEX.message.error(message);

                });

            });

        }]);

    WEX.custo.administrativos.Modulo
        .service("CustosAdministrativosCommand", WEX.custo.administrativos.Command);

    WEX.custo.administrativos.Modulo
        .factory("CustosAdministrativosViewModel", WEX.custo.administrativos.ViewModel);

}());