(function () {
    "use strict";

    WEX.Service.factory("notasfiscais.resource", ["$resource", function ($resource) {

        var parserNotaFiscal = function (notaFiscal) {
            if (typeof notaFiscal.Data == "string") {
                notaFiscal.Data = new Date(parseInt(notaFiscal.Data.substr(6)));
            }
        };

        return $resource("/Custos/CentrosCustos/:centroCustoId/NotasFiscais/:notaFiscalId", null, {

            save: {
                method: "PUT",
                isArray: false,
                params: {
                	centroCustoId: "@CentroDeCustoId",
                	notaFiscalId: "@GastoId"
                }
            },

            query: {
            	method: "GET",
            	isArray: true,
                params: {
                	centroCustoid: "@centroCustoId"
                },
                transformResponse: function (data) {
                    var json = angular.fromJson(data);
                    angular.forEach(json.notasFiscaisDto, parserNotaFiscal);
                    return json.notasFiscaisDto;
                }
            },

            queryAssociadas: {
            	method: "GET",
            	url: "/Custos/Aditivos/:aditivoId/Rubricas/:rubricaId/NotasFiscais",
            	isArray: true,
            	params: {
            		aditivoId: "@aditivoId",
            		rubricaId: "@rubricaId"
            	},
            	transformResponse: function (data) {
            		var json = angular.fromJson(data);
            		angular.forEach(json.notasFiscaisDto, parserNotaFiscal);
            		return json.notasFiscaisDto;
            	}
            },

            associar: {
            	method: "POST",
            	url: "/Custos/Aditivos/:aditivoId/Rubricas/:rubricaId/NotasFiscais",
            	params: {
            		aditivoId: "@aditivoId",
					rubricaId: "@rubricaId"
            	}
            },

            // NotaFiscalRubrica
            desassociar: {
            	method: "DELETE",
            	url: "/Custos/Aditivos/:aditivoId/Rubricas/:rubricaId/NotasFiscais/:notaFiscalId",
            	params: {
            		aditivoId: "@aditivoId",
            		rubricaId: "@RubricaId",
            		notaFiscalId: "@GastoId"
            	}
            },

            // NotasFiscaisTiposRubricas
            rubricas: {
            	method: "GET",
            	url: "/Custos/Aditivos/:aditivoId/NotasFiscais/TiposRubricas",
            	isArray: true,
            	params: {
            		aditivoId: "@aditivoId"
            	}
            }

        });

    }]);

})();