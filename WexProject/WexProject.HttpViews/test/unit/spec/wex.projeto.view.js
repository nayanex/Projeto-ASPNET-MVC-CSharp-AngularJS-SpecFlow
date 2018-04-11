describe("wex.projeto.view.js", function () {

	it("deve definir WEX.Projeto.View", function () {
		expect(WEX.Projeto.View).toBeDefined();
	});

	describe("Modulo projeto.view", function () {
	    beforeEach(function () {
	        angular.module("ngResource");
	        angular.module("ui.bootstrap");
		    angular.module("wex.common");
		    angular.module("wex.service");

			module("projeto.view");
	    });

		describe("Diretiva dadosProjeto", function () {
			var elem,
				$compile,
				$rootScope,
				$scope,
				elemScope;

			beforeEach(function () {
				
			    inject(["$compile", "$rootScope", "$templateCache", "projeto.resource", function (_$compile_, _$rootScope_, _$templateCache_) {
			        $compile = _$compile_;
			        $rootScope = _$rootScope_;
			        _$templateCache_.put("/angular/template/?id=/projeto/dadosprojeto", "");
			    }]);

			    $scope = $rootScope.$new();

			    $scope.projetoAtual = new WEX.Projeto.ProjetoDTO();
			    $scope.projetoAtual.IdProjeto = "4f919baa-2bc5-4ccf-b259-00a9434bdfa6";
			    $scope.projetoAtual.Nome = "Projeto Unit Teste";
			    $scope.projetoAtual.InicioPlanejado = new Date("2000-12-12");
			    $scope.projetoAtual.InicioReal = new Date("2015-12-31");

				elem = angular.element("<div dados-projeto projeto='projetoAtual'></div>");
				$compile(elem)($scope);

				$rootScope.$digest();

				elemScope = elem.isolateScope();

			});

			it("deve iniciar em modo de apresentação", function () {
			    expect(elemScope.model.modoEdicao).toBeFalsy();
			});

			it("deve iniciar com projetos micros encolhidos", function () {
			    expect(elemScope.model.showProjetosMicros).toBeFalsy();
			    expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
			    expect(elemScope.model.projetosMicros.length).toBe(0);
			});

			it("deve alternar modo de edição ao chamar toggleModoEdicao", function () {
				expect(elemScope.model.modoEdicao).toBeFalsy();
				elemScope.toggleModoEdicao();
				expect(elemScope.model.modoEdicao).toBeTruthy();
				elemScope.toggleModoEdicao();
				expect(elemScope.model.modoEdicao).toBeFalsy();
			});

			describe("Função toggleProjetosMicros", function () {
				var $httpBackend,
					projetosMicros;

				beforeEach(function () {
					inject(function (_$httpBackend_) {
						$httpBackend = _$httpBackend_;
					});

					var projetoA = new WEX.Projeto.ProjetoDTO();
					projetoA.IdProjeto = "70af4a2c-362b-463a-bbea-111275783b02";
					projetoA.Nome = "Projeto Alpha";

					var projetoB = new WEX.Projeto.ProjetoDTO();
					projetoB.IdProjeto = "9f655343-17ce-4308-8d08-211b91b9fe47";
					projetoB.Nome = "Projeto Beta";

					projetosMicros = [projetoA, projetoB];

					$httpBackend
						.expectGET("/Projetos/Micros/?idProjetoMacro=4f919baa-2bc5-4ccf-b259-00a9434bdfa6")
						.respond(projetosMicros);

				});

				it("deve alternar a exibição de Projetos Micros", function () {
					expect(elemScope.model.showProjetosMicros).toBeFalsy();
					expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
					expect(elemScope.model.projetosMicros.length).toBe(0);

					elemScope.toggleProjetosMicros();
					$httpBackend.flush();

					expect(elemScope.model.showProjetosMicros).toBeTruthy();
					expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
					expect(elemScope.model.projetosMicros.length).toBeGreaterThan(0);
				});

				it("deve requisitar projetos Micros do respectivo projeto Macro", function () {
					expect(elemScope.model.showProjetosMicros).toBeFalsy();
					expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
					expect(elemScope.model.projetosMicros.length).toBe(0);

					elemScope.toggleProjetosMicros();
					$httpBackend.flush();

					expect(elemScope.model.showProjetosMicros).toBeTruthy();
					expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
					expect(elemScope.model.projetosMicros.length).toBe(2);

					expect(angular.equals(elemScope.model.projetosMicros, projetosMicros)).toBeTruthy();

					elemScope.toggleProjetosMicros();

					expect(elemScope.model.showProjetosMicros).toBeFalsy();
					expect(elemScope.model.projetosMicros instanceof Array).toBeTruthy();
					expect(elemScope.model.projetosMicros.length).toBe(0);

					$httpBackend.verifyNoOutstandingExpectation();
					$httpBackend.verifyNoOutstandingRequest();

				});

			});

		});

	});
});