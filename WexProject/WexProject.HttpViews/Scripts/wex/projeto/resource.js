(function () {
    "use strict";

    WEX.Service.factory("projeto.resource", ["$resource", function ($resource) {

        var _parser = function (projeto) {
            if (typeof projeto.InicioPlanejado == "string") {
                projeto.InicioPlanejado = new Date(parseInt(projeto.InicioPlanejado.substr(6)));
            }
            if (typeof projeto.InicioReal == "string") {
                projeto.InicioReal = new Date(parseInt(projeto.InicioReal.substr(6)));
            }
            if (typeof projeto.TerminoReal == "string") {
                projeto.TerminoReal = new Date(parseInt(projeto.TerminoReal.substr(6)));
            }
        };
    
        return $resource("/Projetos", null, {

            macros: {
                url: "/Projetos/Macros/",
                method: "GET",
                isArray: true,
                transformResponse: function (data) {
                    var projetos = angular.fromJson(data);
                    angular.forEach(projetos, _parser);
                    return projetos;
                }
            },

            micros: {
                url: "/Projetos/Micros/?idProjetoMacro=:idProjetoMacro",
                params: {
                    idProjetoMacro: "@idProjetoMacro"
                },
                method: "GET",
                isArray: true,
                transformResponse: function (data) {
                    var projetos = angular.fromJson(data);
                    angular.forEach(projetos, _parser);
                    return projetos;
                }
            },

            clientes: {
                url: '/Projetos/Clientes/',
                method: "GET",
                isArray: true
            },

            get: {
                url: '/Projetos/Dados/?idProjeto=:idProjeto',
                method: "GET",
                params: {
                    idProjeto: '@idProjeto'
                },
                transformResponse: function (data) {
                    return angular.fromJson(data);
                },
                isArray: false
            },

            query: {
                transformResponse: function (data) {
                    var projetos = angular.fromJson(data);
                    angular.forEach(projetos, _parser);
                    return projetos;
                }
            },

            save: {
                method: "PUT",
                transformResponse: function (data) {
                    var projeto = _parser(data);
                    return projeto;
                }
            }

        });

    }]);

})();
