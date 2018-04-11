(function () {
    "use strict";

    WEX.Service.factory("colaborador.resource", ["$resource", function ($resource) {

        return $resource("/colaborador/", {}, {
            gerentes: {
                url: "/projetos/gerentes/",
                method: "GET",
                isArray: true
            }
        });

    }]);

})();
