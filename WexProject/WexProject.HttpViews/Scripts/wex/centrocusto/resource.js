(function () {
    "use strict";

    WEX.Service.factory("centrocusto.resource", ["$resource", function ($resource) {

        return $resource("/projetos/centroscustos/");

    }]);

})();
