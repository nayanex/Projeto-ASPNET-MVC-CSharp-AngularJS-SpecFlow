(function () {
    "use strict";

    WEX.Service.factory("maodeobra.resource", ["$resource", function ($resource) {

        var parserMaoObra = function (maoObra) {
            if (typeof maoObra.Data == "string") {
                maoObra.Data = new Date(parseInt(maoObra.Data.substr(6)));
            }
        };

        var parserLote = function (lote) {
        	if (typeof lote.DataAtualizacao == "string") {
        		lote.DataAtualizacao = new Date(parseInt(lote.DataAtualizacao.substr(6)));
            }
        };

        return $resource("/MaosDeObra/?centroCustoId=:centroCustoId&ano=:ano&mes=:mes", {
            centroCustoId: "@centroCustoId",
            ano: "@ano",
            mes: "@mes"
        }, {

            importar: {
                method: "GET",
                url: "/MaosDeObra/Importar?centroCustoId=:centroCustoId&ano=:ano&mes=:mes",
                params: {
                    centroCustoId: "@centroCustoId",
                    aditivoId: "@aditivoId",
                    ano: "@ano",
                    mes: "@mes"
                },
                isArray: false,
                transformResponse: function (data) {
                    var json = angular.fromJson(data);
                    parserLote(json.lote);

                    return json;
                }
            },

            query: {
                method: "GET",
                params: {
                    centroCustoId: "@centroCustoId",
                    aditivoId: "@aditivoId",
                    ano: "@ano",
                    mes: "@mes"
                },
                isArray: false,
                transformResponse: function (response) {
                    var json;

                    try {
                        json = angular.fromJson(response);
                    } catch (e) {
                        throw e;
                    }

                    if (json.StatusCode == 404) {
                        throw json.Message;
                    }

                    parserLote(json.lote);

                    angular.forEach(json.maosDeObra, function (maoObra) {
                        parserMaoObra(maoObra);
                    });

                    return json;
                }
            },

            verificarAtualizacao: {
                url: "/MaosDeObra/VerificarAtualizacao?centroCustoId=:centroCustoId&ano=:ano&mes=:mes",
                params: {
                    centroCustoId: "@centroCustoId",
                    ano: "@ano",
                    mes: "@mes"
                },
                method: "GET",
                isArray: false,
                transformResponse: function (response) {
                    var json;

                    try {
                        json = angular.fromJson(response);
                    } catch (e) {
                        throw e;
                    }

                    if (json.StatusCode == 404) {
                        throw json.Message;
                    }

                    return json;
                }
            },

            aplicarSomatorioCustos: {
                url: "/MaosDeObra/AplicarSomatorioCustos",
                params: {
                    centroCustoId: "@centroCustoId",
                    aditivoId: "@aditivoId",
                    ano: "@ano",
                    mes: "@mes"
                },
                method: "PUT",
                isArray: false,
                transformResponse: function (response) {
                    if (!response) {
                        return true;
                    }

                    var json;

                    try {
                        json = angular.fromJson(response);
                    } catch (e) {
                        throw e;
                    }

                    if (json.StatusCode == 404) {
                        throw json.Message;
                    }
                }
            }

        });

    }]);

})();