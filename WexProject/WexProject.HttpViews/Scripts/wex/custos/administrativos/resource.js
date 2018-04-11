(function () {
	"use strict";

	WEX.Service.factory("wex.custos.administrativos.resource", ["$resource", function ($resource) {
		var urlBase;

		urlBase = "/custos/administrativos/:ano-:mes/rubricas";

	    return $resource(urlBase, {
			ano: "@ano",
			mes: "@mes"
		}, {
			
			consultarCustosRubricas: {
				method: "get",
				isArray: false,
			},

			consultarCustosProjetos: {
			    url: urlBase + "/:tipoRubricaId/projetos",
				method: "get",
				isArray: true,
				params: {
					tipoRubricaId: "@tipoRubricaId"
				}
			},

			salvarDespesaReal: {
			    url: urlBase + "/:tipoRubricaId/projetos/:projetoOid/despesa-real/",
			    method: "put",
			    params: {
			        tipoRubricaId: "@TipoRubricaId",
			        projetoOid: "@ProjetoOid",
			        ano: "@Ano",
			        mes: "@Mes"
			    }
			}

		});
	}]);

})();