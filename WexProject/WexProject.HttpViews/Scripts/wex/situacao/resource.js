(function () {
    "use strict";

    WEX.Service.factory("situacao.resource", ["$resource", function ($resource) {

        return $resource("/projetos/situacoes/", null, {
            query: {
                method: "GET",
                isArray: true,
                transformResponse: function (data) {
                    var json = angular.fromJson(data);
                    return json.situacoes;
                }
            }
        });

    }]);

})();
