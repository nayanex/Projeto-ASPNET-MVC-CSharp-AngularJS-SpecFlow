/// <reference path="~/Scripts/wex.main.js" />
/// <reference path="~/Scripts/wex.common.js" />
/// <reference path="~/Scripts/wex.custos.rubricas.js" />

describe('wex.common.js', function () {
	"use strict";

	it("WEX.feedback deve estar definido", function () {
		expect(WEX.feedback).toBeDefined();
	});

	describe("Modulo wex.common", function () {
		
		beforeEach(function () {
			angular.module("ui.bootstrap", []);
		});
		beforeEach(module("wex.common"));
		
		describe("Filtro de data", function () {
			var dataFilter;

			beforeEach(inject(function ($filter) {
				dataFilter = $filter("data");
			}));

			it("deve aceitar objetos Date", function () {
				expect(dataFilter(new Date(2013, 11, 20))).toBe("20/12/2013");
			});

			it("deve aceitar strings no formato ISO", function () {
				expect(dataFilter("2013-12-20")).toBe("20/12/2013");
				expect(dataFilter("2000-12-12")).toBe("12/12/2000");
				expect(dataFilter("1989-01-15")).toBe("15/01/1989");
			});

			it("deve aceitar strings no formato dd/mm/aaaa", function () {
				expect(dataFilter("20/12/2013")).toBe("20/12/2013");
				expect(dataFilter("12/12/2000")).toBe("12/12/2000");
				expect(dataFilter("15/01/1989")).toBe("15/01/1989");
			});

			it("deve retornar string vazia para entradas de tipo inválido", function () {
				expect(dataFilter([])).toBe("");
				expect(dataFilter("3.14")).toBe("");
				expect(dataFilter(3.14)).toBe("");
				expect(dataFilter(null)).toBe("");
			});
		});

		describe("Filtro monetário", function () {
			var monetarioFilter;

			beforeEach(inject(function ($filter) {
				monetarioFilter = $filter("monetario");
			}));

			it("deve aceitar Number positivo sem casa decimal", function () {
				expect(monetarioFilter(20000)).toBe("20.000,00");
			});

			it("deve aceitar Number positivo com casa decimal", function () {
				expect(monetarioFilter(20000.34)).toBe("20.000,34");
			});

			it("deve aceitar Number negativo sem casa decimal", function () {
				expect(monetarioFilter(-20000)).toBe("(20.000,00)");
			});

			it("deve aceitar Number negativo com casa decimal", function () {
				expect(monetarioFilter(-20000.34)).toBe("(20.000,34)");
			});

			it("deve aceitar string no monetarioFiltero 'R$ d.ddd,dd' sem casa decimal", function () {
				expect(monetarioFilter("R$ 3.456,00")).toBe("3.456,00");
			});

			it("deve aceitar string no monetarioFiltero 'R$ d.ddd,dd' com casa decimal", function () {
				expect(monetarioFilter("R$ 3.456,17")).toBe("3.456,17");
			});

			it("deve aceitar string no monetarioFiltero 'd.ddd,dd' sem casa decimal", function () {
				expect(monetarioFilter("3.456,00")).toBe("3.456,00");
			});

			it("deve aceitar string no monetarioFiltero 'd.ddd,dd' com casa decimal", function () {
				expect(monetarioFilter("3.456,17")).toBe("3.456,17");
			});

			it("deve aceitar string no monetarioFiltero '(R$ d.ddd,dd)' sem casa decimal", function () {
				expect(monetarioFilter("(R$ 3.456,00)")).toBe("(3.456,00)");
			});

			it("deve aceitar string no monetarioFiltero '(R$ d.ddd,dd)' com casa decimal", function () {
				expect(monetarioFilter("(R$ 3.456,17)")).toBe("(3.456,17)");
			});

			it("deve aceitar string no monetarioFiltero '(d.ddd,dd)' sem casa decimal", function () {
				expect(monetarioFilter("(3.456,00)")).toBe("(3.456,00)");
			});

			it("deve aceitar string no monetarioFiltero '(d.ddd,dd)' com casa decimal", function () {
				expect(monetarioFilter("(3.456,17)")).toBe("(3.456,17)");
			});

			describe("quando 'permiteNulo' não for especificado,", function () {
				it("deve retornar string '0,00' para entradas de tipo inválido", function () {
					expect(monetarioFilter({})).toBe("0,00");
				});

				it("deve retornar string '0,00' para entrada 'null'", function () {
					expect(monetarioFilter(null)).toBe("0,00");
				});
			});

			describe("quando 'permiteNulo' for 'false',", function () {
				it("deve retornar string '0,00' para entradas de tipo inválido", function () {
					expect(monetarioFilter({}, false)).toBe("0,00");
				});

				it("deve retornar string '0,00' para entrada 'null'", function () {
					expect(monetarioFilter(null, false)).toBe("0,00");
				});
			});

			describe("quando 'permiteNulo' for 'true',", function () {
				it("deve retornar string vazia para entradas de tipo inválido", function () {
					expect(monetarioFilter({}, true)).toBe("");
				});

				it("deve retornar string vazia para entrada 'null'", function () {
					expect(monetarioFilter(null, true)).toBe("");
				});
			});
		});

		describe("Filtro asciify", function () {
			var asciifyFilter;

			beforeEach(inject(function ($filter) {
				asciifyFilter = $filter("asciify");
			}));

			it("deve substituir a vogal 'a' acentuada", function () {
				expect(asciifyFilter("áàâã")).toBe("aaaa");
			});

			it("deve substituir o 'ç'", function () {
				expect(asciifyFilter("braço")).toBe("braco");
			});

			it("deve substituir a vogal 'e' acentuada", function () {
				expect(asciifyFilter("éê")).toBe("ee");
			});

			it("deve substituir a vogal 'i' acentuada", function () {
				expect(asciifyFilter("í")).toBe("i");
			});

			it("deve substituir a vogal 'o' acentuada", function () {
				expect(asciifyFilter("óôõ")).toBe("ooo");
			});

			it("deve substituir a vogal 'u' acentuada", function () {
				expect(asciifyFilter("ú")).toBe("u");
			});
		});

		describe("Filtro camelCase", function () {
			var camelCaseFilter;

			beforeEach(inject(function ($filter) {
				camelCaseFilter = $filter("camelCase");
			}));

			it("deve transformar palavras para CamelCase", function () {
				expect(camelCaseFilter("frase para transformar para camel case")).toBe("fraseParaTransformarParaCamelCase");
			});

			it("deve retirar carácteres em branco", function () {
				expect(camelCaseFilter("	 ")).toBe("");
			});

			it("deve retirar carácteres que não sejam parte de palavras", function () {
				expect(camelCaseFilter("ola. caros$- cole]gas+ de= tra[balho?;")).toBe("olaCarosColegasDeTrabalho");
			});
		});

		describe("Serviço Data", function () {
			var Data;

			beforeEach(function () {
				inject(function (_Data_) {
					Data = _Data_;
				});
			});

			// Precisa de mais teste Ass.: Thiago ;*
			describe("Função toDate", function () {
				var toDateFn;

				beforeEach(function () {
					toDateFn = Data.toDate;
				});

				it("deve aceitar strings no formato ISO", function () {
					expect(toDateFn("2013-12-20")).toEqual(new Date(2013, 11, 20));
					expect(toDateFn("2000-12-12")).toEqual(new Date(2000, 11, 12));
					expect(toDateFn("1989-01-15")).toEqual(new Date(1989, 0, 15));
				});
			});

			describe("Função toCommonDateString", function () {
				var toCommonDateStringFn;

				beforeEach(function () {
					toCommonDateStringFn = Data.toCommonDateString;
				});

				it("deve aceitar strings no formato ISO", function () {
					expect(toCommonDateStringFn("2015-12-20")).toBe("20/12/2015");
					expect(toCommonDateStringFn("1999-06-12")).toBe("12/06/1999");
					expect(toCommonDateStringFn("1979-03-15")).toBe("15/03/1979");
				});

				it("deve aceitar strings no formato dd/mm/aaaa", function () {
					expect(toCommonDateStringFn("24/12/2010")).toBe("24/12/2010");
					expect(toCommonDateStringFn("12/09/2001")).toBe("12/09/2001");
					expect(toCommonDateStringFn("17/08/1989")).toBe("17/08/1989");
				});

				it("deve retornar string vazia para entradas de tipo inválido", function () {
					expect(toCommonDateStringFn([])).toBe("");
					expect(toCommonDateStringFn("3.14")).toBe("");
					expect(toCommonDateStringFn(3.14)).toBe("");
					expect(toCommonDateStringFn(null)).toBe("");
				});
			});

			describe("Função toISODateString", function () {
				var toISODateStringFn;

				beforeEach(function () {
					toISODateStringFn = Data.toISODateString;
				});

				it("deve aceitar strings no formato ISO", function () {
					expect(toISODateStringFn("2015-12-20")).toBe("2015-12-20");
					expect(toISODateStringFn("1999-06-12")).toBe("1999-06-12");
					expect(toISODateStringFn("1979-03-15")).toBe("1979-03-15");
				});

				it("deve aceitar strings no formato dd/mm/aaaa", function () {
					expect(toISODateStringFn("24/12/2010")).toBe("2010-12-24");
					expect(toISODateStringFn("12/09/2001")).toBe("2001-09-12");
					expect(toISODateStringFn("17/08/1989")).toBe("1989-08-17");
				});

				it("deve retornar string vazia para entradas de tipo inválido", function () {
					expect(toISODateStringFn([])).toBe("");
					expect(toISODateStringFn("3.14")).toBe("");
					expect(toISODateStringFn(3.14)).toBe("");
					expect(toISODateStringFn(null)).toBe("");
				});
			});

			describe("Função fromJSONDate", function () {
				var fromJSONDateFn;

				beforeEach(function () {
					fromJSONDateFn = Data.fromJSONDate;
				});

				it("deve aceitar strings no formato /Date(ticks)/", function () {
					expect(fromJSONDateFn("/Date(123456789)/")).toEqual(new Date(123456789));
					expect(fromJSONDateFn("/Date(987654321)/")).toEqual(new Date(987654321));
					expect(fromJSONDateFn("/Date(963852741)/")).toEqual(new Date(963852741));
				});

				it("deve retornar ´null´para entradas inválidas", function () {
					expect(fromJSONDateFn([])).toBe(null);
					expect(fromJSONDateFn("3.14")).toBe(null);
					expect(fromJSONDateFn(3.14)).toBe(null);
					expect(fromJSONDateFn(null)).toBe(null);
				});
			});
		});

		describe("Serviço Dinheiro", function () {
			var Dinheiro;

			beforeEach(inject(function (_Dinheiro_) {
				Dinheiro = _Dinheiro_;
			}));

			describe("Função format", function () {
				var format;

				beforeEach(function () {
					format = Dinheiro.format;
				});

				it("deve aceitar Number positivo sem casa decimal", function () {
					expect(format(20000)).toBe("20.000,00");
				});

				it("deve aceitar Number positivo com casa decimal", function () {
					expect(format(20000.34)).toBe("20.000,34");
				});

				it("deve aceitar Number negativo sem casa decimal", function () {
					expect(format(-20000)).toBe("(20.000,00)");
				});

				it("deve aceitar Number negativo com casa decimal", function () {
					expect(format(-20000.34)).toBe("(20.000,34)");
				});

				it("deve aceitar string no formato 'R$ d.ddd,dd' sem casa decimal", function () {
					expect(format("R$ 3.456,00")).toBe("3.456,00");
				});

				it("deve aceitar string no formato 'R$ d.ddd,dd' com casa decimal", function () {
					expect(format("R$ 3.456,17")).toBe("3.456,17");
				});

				it("deve aceitar string no formato 'd.ddd,dd' sem casa decimal", function () {
					expect(format("3.456,00")).toBe("3.456,00");
				});

				it("deve aceitar string no formato 'd.ddd,dd' com casa decimal", function () {
					expect(format("3.456,17")).toBe("3.456,17");
				});

				it("deve aceitar string no formato '(R$ d.ddd,dd)' sem casa decimal", function () {
					expect(format("(R$ 3.456,00)")).toBe("(3.456,00)");
				});

				it("deve aceitar string no formato '(R$ d.ddd,dd)' com casa decimal", function () {
					expect(format("(R$ 3.456,17)")).toBe("(3.456,17)");
				});

				it("deve aceitar string no formato '(d.ddd,dd)' sem casa decimal", function () {
					expect(format("(3.456,00)")).toBe("(3.456,00)");
				});

				it("deve aceitar string no formato '(d.ddd,dd)' com casa decimal", function () {
					expect(format("(3.456,17)")).toBe("(3.456,17)");
				});

				it("deve arredondar '7,999999999999993' para '8'", function () {
					expect(format(7.999999999999993)).toBe("8,00");
					expect(format("7,999999999999993")).toBe("8,00");
				});

				it("deve arredondar '7,000000000000003' para '7'", function () {
					expect(format(7.000000000000003)).toBe("7,00");
					expect(format("7,000000000000003")).toBe("7,00");
				});

				it("deve arredondar '7,666666666666667' para '7,66'", function () {
					expect(format(7.666666666666667)).toBe("7,66");
					expect(format("7,666666666666667")).toBe("7,66");
				});

				describe("quando 'permiteNulo' não for especificado,", function () {
					it("deve retornar string '0,00' para entradas de tipo inválido", function () {
						expect(format({})).toBe("0,00");
					});

					it("deve retornar string '0,00' para entrada 'null'", function () {
						expect(format(null)).toBe("0,00");
					});
				});

				describe("quando 'permiteNulo' for 'false',", function () {
					it("deve retornar string '0,00' para entradas de tipo inválido", function () {
						expect(format({}, false)).toBe("0,00");
					});

					it("deve retornar string '0,00' para entrada 'null'", function () {
						expect(format(null, false)).toBe("0,00");
					});
				});

				describe("quando 'permiteNulo' for 'true',", function () {
					it("deve retornar string vazia para entradas de tipo inválido", function () {
						expect(format({}, true)).toBe("");
					});

					it("deve retornar string vazia para entrada 'null'", function () {
						expect(format(null, true)).toBe("");
					});
				});
			});

			describe("Função parse", function () {
				var parse;

				beforeEach(function () {
					parse = Dinheiro.parse;
				});

				it("deve aceitar Number positivo sem casa decimal", function () {
					expect(parse(20000)).toBe(20000);
				});

				it("deve aceitar Number positivo com casa decimal", function () {
					expect(parse(20000.34)).toBe(20000.34);
				});

				it("deve aceitar Number negativo sem casa decimal", function () {
					expect(parse(-20000)).toBe(-20000);
				});

				it("deve aceitar Number negativo com casa decimal", function () {
					expect(parse(-20000.34)).toBe(-20000.34);
				});

				it("deve aceitar string no formato 'R$ d.ddd,dd' sem casa decimal", function () {
					expect(parse("R$ 3.456,00")).toBe(3456);
				});

				it("deve aceitar string no formato 'R$ d.ddd,dd' com casa decimal", function () {
					expect(parse("R$ 3.456,17")).toBe(3456.17);
				});

				it("deve aceitar string no formato 'd.ddd,dd' sem casa decimal", function () {
					expect(parse("3.456,00")).toBe(3456);
				});

				it("deve aceitar string no formato 'd.ddd,dd' com casa decimal", function () {
					expect(parse("3.456,17")).toBe(3456.17);
				});

				it("deve aceitar string no formato '(R$ d.ddd,dd)' sem casa decimal", function () {
					expect(parse("(R$ 3.456,00)")).toBe(-3456);
				});

				it("deve aceitar string no formato '(R$ d.ddd,dd)' com casa decimal", function () {
					expect(parse("(R$ 3.456,17)")).toBe(-3456.17);
				});

				it("deve aceitar string no formato '(d.ddd,dd)' sem casa decimal", function () {
					expect(parse("(3.456,00)")).toBe(-3456);
				});

				it("deve aceitar string no formato '(d.ddd,dd)' com casa decimal", function () {
					expect(parse("(3.456,17)")).toBe(-3456.17);
				});

				describe("quando 'padrao' não for especificado,", function () {
					it("deve retornar 'null' para entradas de tipo inválido", function () {
						expect(parse({})).toBe(null);
					});

					it("deve retornar 'null' para entrada 'null'", function () {
						expect(parse(null)).toBe(null);
					});

					it("deve retornar string '0,00' para entrada '0,00'", function () {
						expect(parse("0,00")).toBe(0);
					});
				});

				describe("quando 'padrao' for '0',", function () {
					it("deve retornar 0 para entradas de tipo inválido", function () {
						expect(parse({}, 0)).toBe(0);
					});

					it("deve retornar 0 para entrada 'null'", function () {
						expect(parse(null, 0)).toBe(0);
					});
				});
			});
		});
	});
});
