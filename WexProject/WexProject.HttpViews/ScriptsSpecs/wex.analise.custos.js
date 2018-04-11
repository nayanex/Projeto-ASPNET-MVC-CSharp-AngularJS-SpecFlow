/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.analise.config.js" />
/// <reference path="~/Scripts/wex.analise.custos.js" />
describe('wex.analise.custos.js', function () {
	it("WEX.analise.AnaliseModulo precisa estar definido", function () {
		expect(WEX.analise.AnaliseModulo).toBeDefined();
	});

	describe("Módulo wex.analise.custos", function () {

		beforeEach(function () {
			module("wex.analise.custos");
		});

		describe("Serviço Projetos", function () {
			var Projetos,
				$httpBackend,
				listaProjetos,
				lista;

			beforeEach(function () {
				inject(function (_Projetos_, _$httpBackend_) {
					Projetos = _Projetos_;
					$httpBackend = _$httpBackend_;
				});

				spyOn(WEX.feedback, "erroGeral");

				listaProjetos = [
					{
						FluxoCaixa: -20000,
						Nome: "Android Toys",
						Oid: "cb944705-5005-47d9-9211-954dedeefa34",
						OrcamentoConsumido: 40,
						OrcamentoConsumidoInfo: "Orçamento Previsto: R$ 50.000,00 / Orçamento Consumido: R$ 20.000,00",
						Projecao: -20000,
						SituacaoFinanceira: "Critico",
						Status: "EmAndamento",
						TempoOcorrido: 0,
						TempoOcorridoInfo: "0 de 2 meses realizados."
					},
					{
						FluxoCaixa: 0,
						Nome: "Astronauta",
						Oid: "26b78873-8099-40e2-961e-f768c8084edc",
						OrcamentoConsumido: 0,
						OrcamentoConsumidoInfo: "Orçamento Previsto: R$ 800.000,00 / Orçamento Consumido: R$ 0,00",
						Projecao: 800000,
						SituacaoFinanceira: "Positivo",
						Status: "EmAndamento",
						TempoOcorrido: 100,
						TempoOcorridoInfo: "6 de 6 meses realizados."
					}
				];

				$httpBackend.whenGET("/Analise/Custos").respond({ projetos: listaProjetos });
			});

			describe("Função get", function () {
				beforeEach(function () {
					lista = Projetos.get();
				});

				it("deve iniciar com lista vazia", function () {
					var lista = Projetos.get();

					expect(lista instanceof Array).toBeTruthy();
					expect(lista.length).toBe(0);
				});

				describe("sem argumento", function () {
					it("deve requisitar lista do servidor", function () {
						var lista = Projetos.get();

						$httpBackend.expectGET("/Analise/Custos");

						$httpBackend.verifyNoOutstandingExpectation();
					});

					it("deve retornar atualizar a lista em caso de sucesso", function () {
						var lista = Projetos.get();

						$httpBackend.expectGET("/Analise/Custos");

						expect(lista instanceof Array).toBeTruthy();
						expect(lista.length).toBe(0);

						$httpBackend.flush();

						expect(lista).toEqual(listaProjetos);

						$httpBackend.verifyNoOutstandingExpectation();
						$httpBackend.verifyNoOutstandingRequest();
					});

					it("deve informar mensagem de erro ao falha requisição", function () {
						var lista = Projetos.get();

						$httpBackend.expectGET("/Analise/Custos").respond(500, {});

						expect(lista instanceof Array).toBeTruthy();
						expect(lista.length).toBe(0);

						$httpBackend.flush();

						expect(WEX.feedback.erroGeral).toHaveBeenCalled();

						$httpBackend.verifyNoOutstandingExpectation();
						$httpBackend.verifyNoOutstandingRequest();
					});
				});

				describe("com argumento", function () {
					
				});
			});
		});

		describe("AnaliseCtrl", function () {

		    var $scope, $location, $rootScope, AnaliseCtrl;

		    beforeEach(inject(function ($injector) {

		        $rootScope = $injector.get('$rootScope');

		        $scope = $rootScope.$new();

		        var $controller = $injector.get('$controller');

		        AnaliseCtrl = function () {
		            return $controller('AnaliseCtrl', {
		                '$scope': $scope,
		                'Projetos': $injector.get('Projetos')
		            });
		        };

		    }));

		    it("deve estar definido", function () {

		        expect(AnaliseCtrl).toBeDefined();

		    });

		    describe("função isMesDetalhamentoVisivel", function () {

		        it("deve estar definido", function () {
		            var controller = AnaliseCtrl();
		            expect($scope.isMesDetalhamentoVisivel).toBeDefined();
		        });

		        it("deve retornar falso quando nenhum valor for informado", function () {
		            var controller = AnaliseCtrl();
		            var orcamentoDespesasDTO = {};
		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeFalsy();
		        });

		        it("deve retornar falso quando todos os valores forem iguais a zero", function () {
		            var controller = AnaliseCtrl();
		            var orcamentoDespesasDTO = {};
		            orcamentoDespesasDTO.OrcamentoAprovadoAdministracao = 0;
		            orcamentoDespesasDTO.OrcamentoAprovadoDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasAdministrativas = 0;
		            orcamentoDespesasDTO.ResultadoMensal = 0;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeFalsy();
		        });

		        it("deve retornar verdadeiro quando pelo menos um valor for informado", function () {
		            var controller = AnaliseCtrl();
		            var orcamentoDespesasDTO = {};
		            orcamentoDespesasDTO.OrcamentoAprovadoAdministracao = 1;
		            orcamentoDespesasDTO.OrcamentoAprovadoDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasAdministrativas = 0;
		            orcamentoDespesasDTO.ResultadoMensal = 0;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeTruthy();

		            orcamentoDespesasDTO.OrcamentoAprovadoAdministracao = 0;
		            orcamentoDespesasDTO.OrcamentoAprovadoDesenvolvimento = 1;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeTruthy();

		            orcamentoDespesasDTO.OrcamentoAprovadoDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasDesenvolvimento = 1;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeTruthy();

		            orcamentoDespesasDTO.DespesasDesenvolvimento = 0;
		            orcamentoDespesasDTO.DespesasAdministrativas = 1;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeTruthy();

		            orcamentoDespesasDTO.DespesasAdministrativas = 0;
		            orcamentoDespesasDTO.ResultadoMensal = 1;

		            expect($scope.isMesDetalhamentoVisivel(orcamentoDespesasDTO)).toBeTruthy();

		        });

		    });

		    describe("função isMesFluxoVisivel", function () {

		        it("deve estar definido", function () {
		            var controller = AnaliseCtrl();
		            expect($scope.isMesFluxoVisivel).toBeDefined();
		        });

		        it("deve retornar falso quando nenhum valor for informado", function () {
		            var controller = AnaliseCtrl();
		            var despesasAportesDTO = {};
		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeFalsy();
		        });

		        it("deve retornar falso quando todos os valores forem iguais a zero", function () {
		            var controller = AnaliseCtrl();
		            var despesasAportesDTO = {};
		            despesasAportesDTO.AportePlanejado = 0;
		            despesasAportesDTO.AporteRealizado = 0;
		            despesasAportesDTO.DespesasReaisAdministrativas = 0;
		            despesasAportesDTO.DespesasReaisDesenvolvimento = 0;

		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeFalsy();
		        });

		        it("deve retornar verdadeiro quando pelo menos um valor for informado", function () {
		            var controller = AnaliseCtrl();
		            var despesasAportesDTO = {};
		            despesasAportesDTO.AportePlanejado = 1;
		            despesasAportesDTO.AporteRealizado = 0;
		            despesasAportesDTO.DespesasReaisDesenvolvimento = 0;
		            despesasAportesDTO.DespesasReaisAdministrativas = 0;

		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeTruthy();

		            despesasAportesDTO.AportePlanejado = 0;
		            despesasAportesDTO.AporteRealizado = 1;

		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeTruthy();

		            despesasAportesDTO.AporteRealizado = 0;
		            despesasAportesDTO.DespesasReaisDesenvolvimento = 1;

		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeTruthy();

		            despesasAportesDTO.DespesasReaisDesenvolvimento = 0;
		            despesasAportesDTO.DespesasReaisAdministrativas = 1;

		            expect($scope.isMesFluxoVisivel(despesasAportesDTO)).toBeTruthy();

		        });

		    });

		});

	});
});
