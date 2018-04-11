//Worker
if (typeof importScripts == "function") {
	importScripts("/Scripts/wex/main.js", "/Scripts/wex/custos/config.js", "/Scripts/wex/custos/rubricas.js");
}

(function () {
	"use strict";

	self.addEventListener("message", function (event) {
		var aditivo,
			rubricas;

		aditivo = event.data.aditivo;

		rubricas = event.data.rubricas.map(function (rubrica) {
			var indice,
				ano,
				anos,
				mes,
				novoAno,
				novoMeses;

			anos = [];

			if (!rubrica.pai) {
				for (indice = 0; indice < aditivo.Anos.length; indice++) {
					ano = aditivo.Anos[indice];

					if (event.data.atualizar) {
						novoAno = rubrica.Anos.filter(function (anoObj) {
							return anoObj.Ano == ano;
						});

						if (novoAno.length === 0) {
							novoAno = {};
							novoMeses = [];

							for (mes = 1; mes <= 12; mes++) {
								novoMeses.push(new WEX.custo.aditivo.Mes({
									Ano: ano,
									Mes: mes,
									RubricaId: rubrica.RubricaId
								}));
							}

							novoAno.Ano = ano;
							novoAno.Meses = novoMeses;
						} else {
							novoAno = novoAno[0];
						}
					} else {
						novoAno = {};
						novoMeses = [];

						for (mes = 1; mes <= 12; mes++) {
							novoMeses.push(new WEX.custo.aditivo.Mes({
								Ano: ano,
								Mes: mes,
								RubricaId: rubrica.RubricaId
							}));
						}

						novoAno.Ano = ano;
						novoAno.Meses = novoMeses;
					}

					anos.push(novoAno);
				}
			}

			rubrica.Anos = anos;

			return new WEX.custo.aditivo.Rubrica(rubrica);
		});

		self.postMessage({
			indice: event.data.indice,
			classe: event.data.classe,
			rubricas: rubricas
		});
	});
}());