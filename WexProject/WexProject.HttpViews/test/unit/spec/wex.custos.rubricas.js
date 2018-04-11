/*global WEX*/

describe('wex.custos.rubricas.js', function () {
	"use strict";

	describe("Deve excluir Rúbrica", function () {
		var rubricas;

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

		it("sem filhos", function () {
			WEX.custo.aditivo.excluiRubrica(rubricas, "Administrativo", 2);

			expect(rubricas.Administrativo.length).toEqual(2);
			expect(rubricas.Administrativo).toEqual([
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
			]);
		});

		it("com filhos", function () {
			WEX.custo.aditivo.excluiRubrica(rubricas, "Administrativo", 0, [
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

			expect(rubricas.Administrativo.length).toEqual(0);
			expect(rubricas.Administrativo).toEqual([]);
		});
	});
});
