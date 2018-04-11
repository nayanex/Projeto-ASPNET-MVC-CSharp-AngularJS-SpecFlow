/// <reference path="~/Scripts/angular-1.3.0-beta.13/angular.min.js" />
/// <reference path="~/Scripts/angular-1.3.0-beta.13/angular-resource.min.js" />
/// <reference path="~/Scripts/wex/main.js" />
/// <reference path="~/Scripts/wex/common.js" />
/// <reference path="~/Scripts/wex/custos/config.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/config.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/resource.js" />
/*global WEX angular*/

(function () {
	"use strict";

	WEX.custo.administrativos.Command = ["$q", "CustosAdministrativosViewModel", "wex.custos.administrativos.resource", function ($q, viewModel, resource) {

	    var self = this;
	    var FLAG = WEX.custo.administrativos.Flags;

	    /**
         * Lista de custos consolidados das Rubricas Administrativas
         **/
	    self.listarCustosRubricas = function () {

	        var deferred = $q.defer();

	        var params = {
	            ano: viewModel.dataConsulta.getUTCFullYear(),
	            mes: viewModel.dataConsulta.getUTCMonth() + 1
	        };

	        viewModel.flags.up(FLAG.CARREGANDO_CUSTOS_RUBRICAS);

	        resource.consultarCustosRubricas(params)
                .$promise.then(
                    function success(custosRubricas) {

                        if (!angular.isArray(viewModel.custosRubricas)) {
                            viewModel.custosRubricas = [];
                        }

                        angular.extend(viewModel.custosRubricas, custosRubricas.TiposRubricas);

                        angular.extend(viewModel.total, custosRubricas.Total);

                        angular.forEach(viewModel.custosRubricas, function (custosRubrica) {

                            if (viewModel.flags.check(FLAG.MOSTRANDO_CUSTOS_PROJETOS, custosRubrica.TipoRubricaId)) {

                                self.listarCustosProjetos(custosRubrica.TipoRubricaId);

                            }

                        });

                        viewModel.flags.down(FLAG.CARREGANDO_CUSTOS_RUBRICAS);

                        deferred.resolve("Custos carregados com sucesso");

                    }, function error() {

                        viewModel.flags.down(FLAG.CARREGANDO_CUSTOS_RUBRICAS);

                        deferred.reject("Problemas ao carregar os custos por rubrica");

                    });

	        return deferred.promise;

	    };

	    /**
         * Lista os custos de projetos de uma determinada Rubrica
         **/
	    self.listarCustosProjetos = function (tipoRubricaId) {

	        var deferred = $q.defer();

	        var params = {
	            tipoRubricaId: tipoRubricaId,
	            ano: viewModel.dataConsulta.getUTCFullYear(),
	            mes: viewModel.dataConsulta.getUTCMonth() + 1
	        };

	        viewModel.flags.up(FLAG.CARREGANDO_CUSTOS_PROJETOS, tipoRubricaId);

	        resource.consultarCustosProjetos(params)
                .$promise.then(
                    function success(custosProjetosDto) {

                        viewModel.custosProjetos.put(tipoRubricaId, custosProjetosDto);

                        viewModel.flags.down(FLAG.CARREGANDO_CUSTOS_PROJETOS, tipoRubricaId);

                        viewModel.flags.up(FLAG.MOSTRANDO_CUSTOS_PROJETOS, tipoRubricaId);

                        deferred.resolve("Custos de projetos carregados com sucesso");

                    }, function error() {

                        viewModel.flags.down(FLAG.CARREGANDO_CUSTOS_PROJETOS, tipoRubricaId);

                        deferred.reject("Problemas ao carregar os custos de projetos");

                    });

	        return deferred.promise;

	    };

	    /**
         * Remove os custos de projetos listados de uma determinada Rubrica
         **/
	    self.limparCustosProjetos = function (tipoRubricaId) {

	        viewModel.flags.down(FLAG.MOSTRANDO_CUSTOS_PROJETOS, tipoRubricaId);

	        viewModel.custosProjetos.remove(tipoRubricaId);

	    };

	    /**
         * Salva a Despesa Real de uma determinada Rubrica e Projeto 
         **/
	    self.salvarDespesaReal = function (custosRubricaDto, custosProjetoDto) {

	        var deferred = $q.defer();

	        var despesaRealDto = {
	            TipoRubricaId: custosRubricaDto.TipoRubricaId,
	            ProjetoOid: custosProjetoDto.IdProjeto,
	            Ano: viewModel.dataConsulta.getUTCFullYear(),
	            Mes: viewModel.dataConsulta.getUTCMonth() + 1,
	            DespesaReal: custosProjetoDto.DespesaReal
	        };

	        viewModel.flags.up(FLAG.SALVANDO_DESPESA_REAL, custosRubricaDto.TipoRubricaId, custosProjetoDto.IdProjeto);

	        resource.salvarDespesaReal(despesaRealDto)
                .$promise.then(
                    function success() {

                        viewModel.flags.down(FLAG.SALVANDO_DESPESA_REAL, custosRubricaDto.TipoRubricaId, custosProjetoDto.IdProjeto);

                        self.listarCustosRubricas();

                        deferred.resolve("Despesa Real salva com sucesso");

                    },
                    function error(reason) {

                        viewModel.flags.down(FLAG.SALVANDO_DESPESA_REAL, custosRubricaDto.TipoRubricaId, custosProjetoDto.IdProjeto);

                        deferred.reject(reason.data.exceptionMessage);

                    });

	        return deferred.promise;

	    };

	    /**
         * Copia os valores de campo saldo disponível para despesa real
         **/
	    self.copiarSaldoDisponivelProjeto = function (custosProjetoDto) {

	        if (custosProjetoDto.SaldoDisponivel >= 0) {
	            custosProjetoDto.DespesaReal = custosProjetoDto.SaldoDisponivel;
	        }
	       
	    };

	    /**
         * Copia os valores de campo saldo disponível para despesa real para os projetos de uma rubrica administrativa
         **/
	    self.copiarSaldosDisponiveisProjetos = function (tipoRubricaId) {

	        angular.forEach(viewModel.custosProjetos.get(tipoRubricaId), function (custosProjetosDto) {

	            self.copiarSaldoDisponivelProjeto(custosProjetosDto);
	            
	        });

	    };

	}];

})();