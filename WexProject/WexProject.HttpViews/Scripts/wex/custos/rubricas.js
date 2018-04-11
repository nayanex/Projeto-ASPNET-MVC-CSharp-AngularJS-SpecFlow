/// <reference path="wex.main.js" />
/// <reference path="wex.common.js" />
/// <reference path="wex.custos.config.js" />
/// <reference path="wex.custos.aditivos.js" />
/*global WEX*/

(function () {
	"use strict";

	var Dinheiro;

	Dinheiro = {};
	Dinheiro.format = function (valor, permiteNulo) {
		var negativo,
			inteiro,
			decimal,
			moeda,
			casasDecimais,
			pontoMilhar,
			pontoDecimal,
			tipoValor,
			retorno;

		if (typeof permiteNulo === "undefined") {
			permiteNulo = false;
		}

		tipoValor = typeof valor;

		switch (tipoValor) {
			case "number":
				moeda = "R$";
				casasDecimais = 2;
				pontoMilhar = ".";
				pontoDecimal = ",";

				negativo = valor < 0;
				valor = Math.abs(valor);

				valor = (valor).toFixed(2 * casasDecimais).match(/^(\d+)\.(\d+)$/);
				inteiro = valor[1];
				decimal = valor[2].substr(0, casasDecimais);

				inteiro = inteiro.replace(/(\d)(?=(\d{3})+$)/g, "$1" + pontoMilhar);

				retorno = inteiro + pontoDecimal + decimal;

				if (negativo) {
					retorno = "(" + retorno + ")";
				}

				return retorno;
				break;
			case "string":
				return Dinheiro.format(Dinheiro.parse(valor), permiteNulo);
				break;
			default:
				return permiteNulo ? "" : Dinheiro.format(0);
		}
	};
	Dinheiro.parse = function (valor, padrao) {
		var tipoValor,
			sinal,
			regexQuantia;

		if (typeof padrao === "undefined") {
			padrao = null;
		}

		tipoValor = typeof valor;

		switch (tipoValor) {
			case "string":
				sinal = 1;
				regexQuantia = /[^0-9,]+/g;

				if (valor.match(/^-/) || valor.match(/^\((R\$)?\s*[0-9,.]+\)$/)) {
					sinal = -1;
				}

				valor = valor.replace(regexQuantia, "");
				valor = valor.replace(",", ".");

				if (valor === "") {
					return padrao;
				}

				return parseFloat(valor) * sinal;
				break;
			case "number":
				return valor;
				break;
			default:
				return padrao;
		}
	};

	WEX.custo.Dinheiro = Dinheiro;

	WEX.custo.aditivo.RubricaMesDto = function RubricaMesDto(rubricaMes) {
		rubricaMes = new WEX.custo.aditivo.Mes(rubricaMes);

		this.Ano = rubricaMes.Ano;
		this.Mes = rubricaMes.Mes;
		this.Planejado = Dinheiro.parse(rubricaMes.Planejado);
		this.Replanejado = Dinheiro.parse(rubricaMes.Replanejado);
		this.Gasto = Dinheiro.parse(rubricaMes.Gasto);
		this.RubricaId = rubricaMes.RubricaId;
		this.RubricaMesId = rubricaMes.RubricaMesId;
	};
	WEX.custo.aditivo.RubricaDto = function RubricaDto(rubrica) {
		rubrica = new WEX.custo.aditivo.Rubrica(rubrica);

		this.RubricaId = rubrica.RubricaId;
		this.PaiId = rubrica.PaiId;
		this.Tipo = rubrica.Tipo;
		this.TotalPlanejado = Dinheiro.parse(rubrica.TotalPlanejado, 0);
	};
	WEX.custo.aditivo.Rubrica = function Rubrica(obj) {
		obj = obj || {};

		this.AditivoId = obj.AditivoId || 0;
		this.RubricaId = obj.RubricaId || 0;
		this.PaiId = obj.PaiId || null;
		this.Tipo = obj.Tipo || 0;
		this.Classe = obj.Classe || 0;
		this.Nome = obj.Nome || "Nova Rubrica";
		this.TotalPlanejado = Dinheiro.format(obj.TotalPlanejado || 0);
		this.ValorRestante = Dinheiro.format(obj.ValorRestante || 0);
		this.Anos = obj.Anos || [];
	};
	WEX.custo.aditivo.Mes = function Mes(obj) {
		obj = obj || {};

		this.Ano = obj.Ano || 0;
		this.Mes = obj.Mes || 0;
		this.Planejado = Dinheiro.format(obj.Planejado, true);
		this.Replanejado = Dinheiro.format(obj.Replanejado, true);
		this.Gasto = Dinheiro.format(obj.Gasto, true);
		this.RubricaId = obj.RubricaId || 0;
		this.RubricaMesId = obj.RubricaMesId || 0;
	};

	WEX.custo.aditivo.excluiRubrica = function (listaRubricas, classe, indice, filhos) {
		var countDeletar;

		// Filhos *sempre* virão após o pai
		countDeletar = 1;

		if (typeof filhos !== "undefined") {
			countDeletar += filhos.length;
		}

		listaRubricas[classe].splice(indice, countDeletar);
		WEX.feedback.infoGeral("<span>Rubrica removida!</span>");
	};
	WEX.custo.aditivo.excluiCentroCusto = function (listaCentrosCusto, indice) {
		listaCentrosCusto.splice(indice, 1);
		WEX.feedback.infoGeral("<span>Centro de Custo removido!</span>");
	};
	WEX.custo.aditivo.excluiPatrocinador = function (listaPatrocinadores, indice) {
		listaPatrocinadores.splice(indice, 1);
		WEX.feedback.infoGeral("<span>Patrocinador removido!</span>");
	};
})();
