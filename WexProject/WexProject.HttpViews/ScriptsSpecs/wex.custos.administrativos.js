/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.common.js" />
/// <reference path="~/Scripts/wex.service.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/config.js" />
/// <reference path="~/Scripts/wex/custos/administrativos/resource.js" />
/// <reference path="~/Scripts/wex/custos/administrativos.js" />

describe('wex.custos.administrativos.js', function () {
    "use strict";

    it("WEX.custo.administrativos.CustosAdministrativosModulo precisa estar definido", function () {
        expect(WEX.custo.administrativos.CustosAdministrativosModulo).toBeDefined();
    });

    describe("TiposRubricasCtrl", function() {
        
        var $scope,
            $rootScope,
            tiposRubricasCtrl;

        var $q,
            mockCustosAdministrativosResource,
            consultarTiposRubricasDeferred,
            consultarProjetosDeferred;

        var rubricasAdministrativasList = [
            {
                "tipoRubricaId": 16,
                "Nome": "Custo Fixo FPF (Rateio)",
                "OrcamentoAprovado": 200000.00,
                "SaldoDisponivel": 0.00,
                "DespesaReal": 100000.00
            },
            {
                "tipoRubricaId": 19,
                "Nome": "Taxa de Administração",
                "OrcamentoAprovado": 120000.00,
                "SaldoDisponivel": 80000.00,
                "DespesaReal": 20000.00
            },
            {
                "tipoRubricaId": 21,
                "Nome": "Apoio a Clientes",
                "OrcamentoAprovado": 200000.00,
                "SaldoDisponivel": 70000.00,
                "DespesaReal": 50000.00
            }
        ];

        var projetosList = [
            {
                "Oid": "229d30a8-caac-4f14-8957-b56f18faeff2",
                "Nome": "ActiveIris - Fase 2",
                "OrcamentoAprovado": 100000.00,
                "SaldoDisponivel": 0.00,
                "DespesaReal": 50000.00
            },
            {
                "Oid": "cb944705-5005-47d9-9211-954dedeefa34",
                "Nome": "Android Toys",
                "OrcamentoAprovado": 100000.00,
                "SaldoDisponivel": 0.00,
                "DespesaReal": 50000.00
            }
        ];

        // carregando o modulo
        beforeEach(module('wex.custos.administrativos'));

        // mock do wex.custos.administrativos.resource
        beforeEach(inject(function ($injector) {

            $q = $injector.get("$q");

            consultarTiposRubricasDeferred = $q.defer();
            consultarTiposRubricasDeferred.resolve(rubricasAdministrativasList);

            consultarProjetosDeferred = $q.defer();
            consultarProjetosDeferred.resolve(projetosList);

            mockCustosAdministrativosResource = {
                consultarTiposRubricas: function () {
                    return { $promise: consultarTiposRubricasDeferred.promise };
                },
                consultarProjetos: function () {
                    return { $promise: consultarProjetosDeferred.promise };
                }
            };

            spyOn(mockCustosAdministrativosResource, 'consultarTiposRubricas').andCallThrough();
            spyOn(mockCustosAdministrativosResource, 'consultarProjetos').andCallThrough();

        }));

        // inicializando o controller TiposRubricasCtrl
        beforeEach(inject(function ($injector) {

            var $controller = $injector.get('$controller');
            $rootScope = $injector.get('$rootScope');
            $scope = $rootScope.$new();

            tiposRubricasCtrl = $controller('TiposRubricasCtrl', {
                '$scope': $scope,
                'wex.custos.administrativos.resource': mockCustosAdministrativosResource
            });

        }));

        describe('propriedade $scope.modelo', function () {

            it("deve estar definida", function () {
                expect($scope.modelo).toBeDefined();
            });

            describe('propriedade "dataAtual"', function () {

                it("deve estar definida", function () {
                    expect($scope.modelo.dataAtual).toBeDefined();
                });

                it("deve possuir a data atual", function () {
                    var hoje = new Date();
                    expect($scope.modelo.dataAtual.getDate()).toBe(hoje.getDate());
                    expect($scope.modelo.dataAtual.getMonth()).toBe(hoje.getMonth());
                    expect($scope.modelo.dataAtual.getFullYear()).toBe(hoje.getFullYear());
                });

            });

            describe('propriedade "rubricas"', function () {

                it("deve estar definida", function () {
                    expect($scope.modelo.rubricas).toBeDefined();
                });

                it("deve ser nula", function () {
                    expect($scope.modelo.rubricas).toBe(null);
                });

            });

        });

        describe("listagem de rubricas", function () {

            beforeEach(function () {
                $rootScope.$apply();
            });

            it("deve listar as rubricas", function () {
                $scope.$apply();
                expect($scope.modelo.rubricas.length).toBeGreaterThan(0);
                expect($scope.modelo.rubricas.length).toBe(3);
            });

            it('deve ter sido executado o metodo wex.custos.administrativos.resource.consultarTiposRubricas', function () {
                expect(mockCustosAdministrativosResource.consultarTiposRubricas).toHaveBeenCalled();
            });

        });

        describe("listagem de projetos", function () {

            var rubricasAdministrativa = rubricasAdministrativasList[0];

            beforeEach(function () {
                $rootScope.$apply();
            });

            it("deve listar os projetos de uma rubrica", function () {
                $scope.toggleRubricas(rubricasAdministrativa);
                $scope.$apply();
                expect(rubricasAdministrativa.aberta).toBe(true);
                expect(rubricasAdministrativa.projetos.length).toBeGreaterThan(0);
                expect(rubricasAdministrativa.projetos.length).toBe(2);
            });

            it('deve ter sido executado o metodo wex.custos.administrativos.resource.consultarProjetos', function () {
                $scope.toggleRubricas(rubricasAdministrativa);
                $scope.$apply();
                expect(mockCustosAdministrativosResource.consultarProjetos).toHaveBeenCalled();
            });

        });

    });

});