/*global WEX*/

describe('wex.custos.app.js', function () {
	"use strict";

	it("WEX.custo.CustosModule precisa estar definido", function () {
		expect(WEX.custo.CustosModule).toBeDefined();
	});

	describe("Modulo custos.filter", function () {
		
		beforeEach(module("custos.filters"));
		
		describe("Filtro hasFlag", function () {
			var hasFlagFilter;

			beforeEach(inject(function ($filter) {
				hasFlagFilter = $filter("hasFlag");
			}));

			it("deve aceitar bits individuais setados", function () {
				expect(hasFlagFilter(12, 8)).toBe(true);
				expect(hasFlagFilter(12, 4)).toBe(true);
				expect(hasFlagFilter(9, 1)).toBe(true);
			});

			it("deve rejeitar bits individuais não setados", function () {
				expect(hasFlagFilter(12, 2)).toBe(false);
				expect(hasFlagFilter(12, 1)).toBe(false);
				expect(hasFlagFilter(9, 4)).toBe(false);
			});
		});

		describe("Filtro rubricasFilhas", function () {
			var rubricasFilhasFilter,
				rubricas;

			beforeEach(function () {
				rubricas = {
					Desenvolvimento: [],
					Administrativo: [
						{
							Nome: "Pessoal",
							PaiId: null,
							RubricaId: 1531,
						},
						{
							Nome: "Salários",
							PaiId: 1531,
							RubricaId: 1539,
						},
						{
							Nome: "Rescisão / Férias",
							PaiId: 1531,
							RubricaId: 1540,
						},
					],
					Aportes: [],
				};
			});

			beforeEach(inject(function ($filter) {
				rubricasFilhasFilter = $filter("rubricasFilhas");
			}));

			it("deve retornar rubricas filhas ao existirem", function () {
				expect(rubricasFilhasFilter(rubricas.Administrativo, 1531)).toEqual([
						{
							Nome: "Salários",
							PaiId: 1531,
							RubricaId: 1539,
						},
						{
							Nome: "Rescisão / Férias",
							PaiId: 1531,
							RubricaId: 1540,
						},
					]);
			});

			it("deve retornar lista vazia ao não existirem", function () {
				expect(rubricasFilhasFilter(rubricas.Administrativo, 1539)).toEqual([]);
			});
		});

		describe("Filtro subRubricas", function () {
			var subRubricasFilter,
				rubricas;

			beforeEach(function () {
				rubricas = {
					Desenvolvimento: [],
					Administrativo: [
						{
							Nome: "Pessoal",
							PaiId: null,
							RubricaId: 1531,
						},
						{
							Nome: "Salários",
							PaiId: 1531,
							RubricaId: 1539,
						},
						{
							Nome: "Rescisão / Férias",
							PaiId: 1531,
							RubricaId: 1540,
						},
						{
							Nome: "Material de Consumo",
							PaiId: null,
							RubricaId: 1532,
						},
						{
							Nome: "Escritório",
							PaiId: 1532,
							RubricaId: 1548,
						},
						{
							Nome: "Limpeza",
							PaiId: 1532,
							RubricaId: 1549,
						},
					],
					Aportes: [],
				};
			});

			beforeEach(inject(function ($filter) {
				subRubricasFilter = $filter("subRubricas");
			}));

			it("deve retornar sub-Rúbricas se existirem", function () {
				expect(subRubricasFilter(rubricas.Desenvolvimento, true)).toEqual([]);
				expect(subRubricasFilter(rubricas.Administrativo, true)).toEqual([
						{
							Nome: "Salários",
							PaiId: 1531,
							RubricaId: 1539,
						},
						{
							Nome: "Rescisão / Férias",
							PaiId: 1531,
							RubricaId: 1540,
						},
						{
							Nome: "Escritório",
							PaiId: 1532,
							RubricaId: 1548,
						},
						{
							Nome: "Limpeza",
							PaiId: 1532,
							RubricaId: 1549,
						},
				]);
			});

			it("deve retornar Rúbricas de primeiro nível se existirem", function () {
				expect(subRubricasFilter(rubricas.Desenvolvimento, false)).toEqual([]);
				expect(subRubricasFilter(rubricas.Administrativo, false)).toEqual([
						{
							Nome: "Pessoal",
							PaiId: null,
							RubricaId: 1531,
						},
						{
							Nome: "Material de Consumo",
							PaiId: null,
							RubricaId: 1532,
						},
				]);
			});

			it("deve retornar lista vazia se entrada for inválida", function () {
				expect(subRubricasFilter(undefined)).toEqual([]);
				expect(subRubricasFilter(42)).toEqual([]);
			});
		});

		describe("Filtro classe", function () {
			var classeFilter;

			beforeEach(module(function ($provide) {
				$provide.value("TiposRubricas", {
					"tipos": [
						{ "TipoRubricaId": 1, "PaiId": null, "Pai": "", "Nome": "Viagens e Deslocamentos", "Classe": 1 },
						{ "TipoRubricaId": 2, "PaiId": null, "Pai": "", "Nome": "Contratações (fora de Manaus)", "Classe": 1 },
						{ "TipoRubricaId": 3, "PaiId": null, "Pai": "", "Nome": "Treinamentos", "Classe": 1 },
						{ "TipoRubricaId": 4, "PaiId": null, "Pai": "", "Nome": "Manutenção (após entrega)", "Classe": 1 },
						{ "TipoRubricaId": 5, "PaiId": null, "Pai": "", "Nome": "Livros e Periódicos", "Classe": 1 },
						{ "TipoRubricaId": 6, "PaiId": null, "Pai": "", "Nome": "Software", "Classe": 1 },
						{ "TipoRubricaId": 7, "PaiId": null, "Pai": "", "Nome": "Hardware", "Classe": 1 },
						{ "TipoRubricaId": 8, "PaiId": null, "Pai": "", "Nome": "Obras de Infra-Estrutura", "Classe": 1 },
						{ "TipoRubricaId": 9, "PaiId": null, "Pai": "", "Nome": "Terceiros", "Classe": 1 },
						{ "TipoRubricaId": 10, "PaiId": null, "Pai": "", "Nome": "RH GDC", "Classe": 1 },
						{ "TipoRubricaId": 11, "PaiId": null, "Pai": "", "Nome": "RH TI", "Classe": 1 },
						{ "TipoRubricaId": 12, "PaiId": null, "Pai": "", "Nome": "RH Designer", "Classe": 1 },
						{ "TipoRubricaId": 13, "PaiId": null, "Pai": "", "Nome": "RH Testes", "Classe": 1 },
						{ "TipoRubricaId": 14, "PaiId": null, "Pai": "", "Nome": "RH Qualidade", "Classe": 1 },
						{ "TipoRubricaId": 15, "PaiId": null, "Pai": "", "Nome": "Mão de Obra Direta (Exclusiva)", "Classe": 1 },
						{ "TipoRubricaId": 16, "PaiId": null, "Pai": "", "Nome": "Custo Fixo FPF (Rateio)", "Classe": 2 },
						{ "TipoRubricaId": 17, "PaiId": null, "Pai": "", "Nome": "Depreciação de Equipamentos", "Classe": 2 },
						{ "TipoRubricaId": 19, "PaiId": null, "Pai": "", "Nome": "Taxa de Administração", "Classe": 2 },
						{ "TipoRubricaId": 20, "PaiId": null, "Pai": "", "Nome": "Impostos e Encargos", "Classe": 2 },
						{ "TipoRubricaId": 21, "PaiId": null, "Pai": "", "Nome": "Apoio a Clientes", "Classe": 2 },
						{ "TipoRubricaId": 22, "PaiId": null, "Pai": "", "Nome": "FACN", "Classe": 2 },
						{ "TipoRubricaId": 23, "PaiId": null, "Pai": "", "Nome": "Aportes", "Classe": 4 },
						{ "TipoRubricaId": 24, "PaiId": null, "Pai": "", "Nome": "RH Direto (Dissídio)", "Classe": 1 }
					],
					"classes": ["Desenvolvimento", "Administrativo", "Aportes"],
					"classesMap": {
						"Desenvolvimento": 1,
						"Administrativo": 2,
						"Aportes": 4,
						"Tudo": 7,
						"Pai": 8
					}
				})
			}));

			beforeEach(inject(function ($filter) {
				classeFilter = $filter("classe");
			}));

			it("deve filtrar Rúbricas por classe", inject(function (TiposRubricas) {
				expect(classeFilter(TiposRubricas.tipos, TiposRubricas.classes[0])).toEqual([
						{ "TipoRubricaId": 1, "PaiId": null, "Pai": "", "Nome": "Viagens e Deslocamentos", "Classe": 1 },
						{ "TipoRubricaId": 2, "PaiId": null, "Pai": "", "Nome": "Contratações (fora de Manaus)", "Classe": 1 },
						{ "TipoRubricaId": 3, "PaiId": null, "Pai": "", "Nome": "Treinamentos", "Classe": 1 },
						{ "TipoRubricaId": 4, "PaiId": null, "Pai": "", "Nome": "Manutenção (após entrega)", "Classe": 1 },
						{ "TipoRubricaId": 5, "PaiId": null, "Pai": "", "Nome": "Livros e Periódicos", "Classe": 1 },
						{ "TipoRubricaId": 6, "PaiId": null, "Pai": "", "Nome": "Software", "Classe": 1 },
						{ "TipoRubricaId": 7, "PaiId": null, "Pai": "", "Nome": "Hardware", "Classe": 1 },
						{ "TipoRubricaId": 8, "PaiId": null, "Pai": "", "Nome": "Obras de Infra-Estrutura", "Classe": 1 },
						{ "TipoRubricaId": 9, "PaiId": null, "Pai": "", "Nome": "Terceiros", "Classe": 1 },
						{ "TipoRubricaId": 10, "PaiId": null, "Pai": "", "Nome": "RH GDC", "Classe": 1 },
						{ "TipoRubricaId": 11, "PaiId": null, "Pai": "", "Nome": "RH TI", "Classe": 1 },
						{ "TipoRubricaId": 12, "PaiId": null, "Pai": "", "Nome": "RH Designer", "Classe": 1 },
						{ "TipoRubricaId": 13, "PaiId": null, "Pai": "", "Nome": "RH Testes", "Classe": 1 },
						{ "TipoRubricaId": 14, "PaiId": null, "Pai": "", "Nome": "RH Qualidade", "Classe": 1 },
						{ "TipoRubricaId": 15, "PaiId": null, "Pai": "", "Nome": "Mão de Obra Direta (Exclusiva)", "Classe": 1 },
						{ "TipoRubricaId": 24, "PaiId": null, "Pai": "", "Nome": "RH Direto (Dissídio)", "Classe": 1 }
				]);
			}));
		});
		
	});

	describe("Modulo custos.directive", function () {

		beforeEach(module("custos.directive"));

		describe("Diretiva wexValorMutex", function () {
			var elem,
				Dinheiro,
				$compile,
				$rootScope,
				$scope;

			beforeEach(module("wex.common"));

			beforeEach(inject(function (_Dinheiro_, _$compile_, _$rootScope_) {
				spyOn(_Dinheiro_, "parse").andCallThrough();

				Dinheiro = _Dinheiro_;
				$compile = _$compile_;
				$rootScope = _$rootScope_;
			}));

			beforeEach(function () {
				$scope = $rootScope.$new();
				elem = angular.element("<input ng-model='valor' wex-valor-mutex='exclusivo' />");
				$compile(elem)($scope);

				$scope.exclusivo = 50;

				$rootScope.$digest();
			});

			it("deve modificar valor da referencia ao receber valor diferente de nulo", function () {
				elem.val(45);
				elem.trigger("input");
				elem.trigger("change");

				expect(elem.val()).toBe("45");
				expect($scope.valor).toBe("45");
				expect($scope.exclusivo).toBe(null);
				expect(Dinheiro.parse).toHaveBeenCalled();
			});

			it("deve não modificar valor da referencia ao receber nulo", function () {
				elem.val("");
				elem.trigger("input");
				elem.trigger("change");

				expect(elem.val()).toBe("");
				expect($scope.valor).toBe("");
				expect($scope.exclusivo).toBe(50);
				expect(Dinheiro.parse).toHaveBeenCalled();
			});

			it("deve modificar valor da referencia ao receber valor igual a 0", function () {
				elem.val(0);
				elem.trigger("input");
				elem.trigger("change");

				expect(elem.val()).toBe("0");
				expect($scope.valor).toBe("0");
				expect($scope.exclusivo).toBe(null);
				expect(Dinheiro.parse).toHaveBeenCalled();
			});
		});

		describe("Diretiva despesa", function () {
			var elem,
				rubricaMes,
				Rubrica,
				$compile,
				$rootScope,
				$scope;

			beforeEach(module(function ($provide) {
				Rubrica = jasmine.createSpyObj("Rubrica", ["rubricaVazia"]);
				Rubrica.rubricaVazia.andReturn(true);

				$provide.value("Rubrica", Rubrica);
			}));

			beforeEach(inject(function (_$compile_, _$rootScope_, $templateCache) {
				$compile = _$compile_;
				$rootScope = _$rootScope_;

				$templateCache.put("/Angular/despesa", "");
			}));

			beforeEach(function () {
				$scope = $rootScope.$new();
				elem = angular.element("<table ng-show='rubrica.aberto' despesa></table>");
				$compile(elem)($scope);

				rubricaMes = {
					Replanejado: 50,
					Gasto: null
				};

				$scope.rubrica = {
					Anos: [
					{
						"2013": [rubricaMes]
					}
					]
				};

				$rootScope.$digest();
			});

			it("deve iniciar fechada caso não tenha valor", function () {
				Rubrica.rubricaVazia.andReturn(true);

				rubricaMes.Replanejado = 25;

				$rootScope.$digest();

				expect(Rubrica.rubricaVazia).toHaveBeenCalled();
				expect(Rubrica.rubricaVazia.calls.length).toBeGreaterThan(0);
				expect($scope.rubrica.aberto).toBe(false);
			});

			it("deve iniciar aberta caso tenha valor", function () {
				Rubrica.rubricaVazia.andReturn(false);

				rubricaMes.Replanejado = 25;

				$rootScope.$digest();

				expect(Rubrica.rubricaVazia).toHaveBeenCalled();
				expect(Rubrica.rubricaVazia.calls.length).toBeGreaterThan(0);
				expect($scope.rubrica.aberto).toBe(true);
			});

			it("deve abrir/fechar ao ser chamada a função toggleRubrica", function () {
				expect($scope.rubrica.aberto).toBe(false);

				$scope.toggleRubrica();

				expect($scope.rubrica.aberto).toBe(true);

				$scope.toggleRubrica();

				expect($scope.rubrica.aberto).toBe(false);
			});

			it("deve copiar valor de Replanejado para Gasto ao chamar a função copiarValor", function () {
				expect(rubricaMes.Replanejado).toBe(50);
				expect(rubricaMes.Gasto).toBe(null);

				$scope.copiarValor(rubricaMes);

				expect(rubricaMes.Replanejado).toBe(null);
				expect(rubricaMes.Gasto).toBe(50);
			});
		});

	});

	describe("Modulo custos", function () {
		
		beforeEach(module(function ($provide) {
			$provide.value("$modal", {});
		}));

		beforeEach(module("custos"));

		describe("Serviço CentrosCusto", function () {
			var CentrosCusto,
				$httpBackend;

			beforeEach(inject(function (_CentrosCusto_, _$httpBackend_) {
				CentrosCusto = _CentrosCusto_;
				$httpBackend = _$httpBackend_;
			}));

			afterEach(function () {
				$httpBackend.verifyNoOutstandingExpectation();
				$httpBackend.verifyNoOutstandingRequest();
			});
			
			describe("Função get", function () {
				it("deve usar GET", function () {
					var centrosCusto;

					$httpBackend.expectGET("/Custos/Aditivo/25/CentrosCusto").respond({ "centrosCusto": [{ "CentroCustoId": 5, "Codigo": 9625, "Nome": "iDoctor" }] });
					centrosCusto = CentrosCusto.get(25);
					$httpBackend.flush(1);
					expect(centrosCusto).toEqual([{ "CentroCustoId": 5, "Codigo": 9625, "Nome": "iDoctor" }]);

					$httpBackend.expectGET("/Custos/Aditivo/15/CentrosCusto").respond({ "centrosCusto": [{ "CentroCustoId": 3, "Codigo": 5478, "Nome": "João e Maria" }] });
					centrosCusto = CentrosCusto.get(15);
					$httpBackend.flush();
					expect(centrosCusto).toEqual([{ "CentroCustoId": 3, "Codigo": 5478, "Nome": "João e Maria" }]);
				});
			});

			describe("Função put", function () {
				it("deve usar POST", function () {
					$httpBackend.expectPOST("/Custos/Aditivo/25/CentrosCusto/42").respond(200);
					CentrosCusto.put(25, { CentroCustoId: 42 });
					$httpBackend.flush();
				});
			});

		});

		describe("Serviço Patrocinadores", function () {
			var Patrocinadores,
				$httpBackend;

			beforeEach(inject(function (_Patrocinadores_, _$httpBackend_) {
				Patrocinadores = _Patrocinadores_;
				$httpBackend = _$httpBackend_;
			}));

			afterEach(function () {
				$httpBackend.verifyNoOutstandingExpectation();
				$httpBackend.verifyNoOutstandingRequest();
			});

			describe("Função get", function () {
				it("deve usar GET", function () {
					var patrocinadores;

					$httpBackend.expectGET("/Custos/Aditivo/25/Patrocinadores").respond({ "patrocinadores": [{ "Oid": "3f4238c4-eefb-45a7-8b08-2d2b3db0f64e", "Nome": "PositiFabrica", "Sigla": "PositiFabrica", "Email": "alicec@positivo.com.br", "FoneFax": "(92)3183-7909" }] });
					patrocinadores = Patrocinadores.get(25);
					$httpBackend.flush(1);
					expect(patrocinadores).toEqual([{ "Oid": "3f4238c4-eefb-45a7-8b08-2d2b3db0f64e", "Nome": "PositiFabrica", "Sigla": "PositiFabrica", "Email": "alicec@positivo.com.br", "FoneFax": "(92)3183-7909" }]);

					$httpBackend.expectGET("/Custos/Aditivo/15/Patrocinadores").respond({ "patrocinadores": [{ "Oid": "57d7519b-65fe-41fe-9c2a-0895313e2785", "Nome": "Positivo", "Sigla": "POS", "Email": "rogerio.saran@gmail.com", "FoneFax": null }] });
					patrocinadores = Patrocinadores.get(15);
					$httpBackend.flush();
					expect(patrocinadores).toEqual([{ "Oid": "57d7519b-65fe-41fe-9c2a-0895313e2785", "Nome": "Positivo", "Sigla": "POS", "Email": "rogerio.saran@gmail.com", "FoneFax": null }]);
				});
			});

			describe("Função put", function () {
				it("deve usar POST", function () {
					$httpBackend.expectPOST("/Custos/Aditivo/25/Patrocinadores/42").respond(200);
					Patrocinadores.put(25, { Oid: 42 });
					$httpBackend.flush();
				});
			});

		});
	});
});
