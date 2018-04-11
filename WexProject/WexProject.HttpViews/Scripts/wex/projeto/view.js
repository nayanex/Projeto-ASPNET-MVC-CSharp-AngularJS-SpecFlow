(function () {
    "use strict";

    WEX.Projeto.View = angular.module("projeto.view", ["wex.common", "wex.service", "ui.bootstrap"]);

    WEX.Projeto.View.factory("projeto.view.model", function () {
        return {
            cadastrandoNovoProjeto: false,
            novoProjeto: new WEX.Projeto.ProjetoDTO(),
            projetosMacros: [],
            gerentesProjetos: [],
            centrosCustos: [],
            situacoes: [],
            clientes: []
        };
    });

    WEX.Projeto.View.controller("ProjetoCtrl",
        ["$scope", "projeto.view.model", "projeto.resource", "situacao.resource", "centrocusto.resource", "colaborador.resource",
            function ($scope, projetoViewModel, projetoResource, situacaoResource, centroCustoResource, colaboradorResource) {

                $scope.model = projetoViewModel;

                var loadingMessage = WEX.message.loading("Carregando projetos macros", { modal: true });

                projetoViewModel.projetosMacros = projetoResource.macros(function () {
                    
                    loadingMessage.close();

                }, function () {

                    loadingMessage.close();

                    WEX.message.error("Entre em contato com o administrador", { title: "Erro ao carregar os projetos macros" });

                });

                projetoViewModel.situacoes = situacaoResource.query();
                projetoViewModel.centrosCustos = centroCustoResource.query();
                projetoViewModel.gerentesProjetos = colaboradorResource.gerentes();
                projetoViewModel.clientes = projetoResource.clientes();

                $scope.toggleNovoProjeto = function () {
                    $scope.model.cadastrandoNovoProjeto = !$scope.model.cadastrandoNovoProjeto;
                };

                $scope.alterarTipoProjeto = function () {
                    projetoViewModel.novoProjeto.IsMacro = (projetoViewModel.novoProjeto.ProjetoMacro == null);
                };

                $scope.salvarProjeto = function () {

                    projetoResource.save(projetoViewModel.novoProjeto, function (data) {

                        projetoViewModel.cadastrandoNovoProjeto = false;

                        var loadingMessage = WEX.message.loading("Atualizando lista de projetos macros", { modal: true });

                        projetoViewModel.projetosMacros = projetoResource.macros(function () {

                            loadingMessage.close();

                            var messageSuccess = WEX.message.success('Projeto "' + projetoViewModel.novoProjeto.Nome + '" criado com sucesso.');

                            setTimeout(messageSuccess.close, 3000);

                            projetoViewModel.novoProjeto = new WEX.Projeto.ProjetoDTO();

                        }, function () {

                            loadingMessage.close();

                            WEX.message.error("Entre em contato com o administrador", { title: "Erro ao carregar os projetos macros" });

                        });

                    }, function (error) {

                        if (error.status == 409) {

                            WEX.message.validation("Preencha todos os campos obrigatórios", { title: "Problemas ao salvar o projeto", modal: true });

                        } else {

                            WEX.message.fatal("Infelizmente a aplicação comportou-se de maneira inesperada. Entre em contato com o administrador");

                        }

                    });

                };

            }]);

    /**
     * Directive utilizada para gerar os dados do projeto na listagem da pagina principal do cadastro de projetos
     **/
    WEX.Projeto.View.directive("dadosProjeto", ["$compile", "projeto.resource", function ($compile, projetoResource) {

        return {
            templateUrl: "/angular/template/?id=/projeto/dadosprojeto",
            restrict: 'AE',
            scope: { projeto: '=', styleClass: '@' },
            controller: function ($scope) {

                $scope.model = {
                    modoEdicao: false,
                    showProjetosMicros: false,
                    projetosMicros: []
                };

                $scope.toggleModoEdicao = function () {
                    $scope.model.modoEdicao = !$scope.model.modoEdicao;
                };

                $scope.toggleProjetosMicros = function () {

                    $scope.model.showProjetosMicros = !$scope.model.showProjetosMicros;

                    if (!$scope.model.showProjetosMicros) {
                        $scope.model.projetosMicros = [];
                        return;
                    }

                    $scope.model.projetosMicros = projetoResource.micros({ idProjetoMacro: $scope.projeto.IdProjeto });

                };

            },
            // recursive fix: http://stackoverflow.com/a/19172067
            compile: function (tElement, tAttr, transclude) {
                var contents = tElement.contents().remove();
                var compiledContents;
                return function (scope, iElement, iAttr) {
                    if (!compiledContents) {
                        compiledContents = $compile(contents, transclude);
                    }
                    compiledContents(scope, function (clone, scope) {
                        iElement.append(clone);
                    });
                };
            }

        };
    }]);

    /**
     * Directive utilizada para gerar os dados do projeto na listagem da pagina principal do cadastro de projetos
     **/
    WEX.Projeto.View.directive("editarDadosProjeto",
        ["projeto.view.model", "projeto.resource",
            function (projetoViewModel, projetoResource) {

                return {
                    templateUrl: "/angular/template/?id=/projeto/editardadosprojeto",
                    restrict: 'E',
                    scope: { projeto: '=' },
                    controller: function ($scope) {

                        $scope.model = {
                            clientesNomes: "",
                            clientes: projetoViewModel.clientes,
                            situacoes: projetoViewModel.situacoes,
                            projetosMacros: projetoViewModel.projetosMacros,
                            centrosCustos: projetoViewModel.centrosCustos,
                            gerentesProjetos: projetoViewModel.gerentesProjetos
                        };

                        angular.forEach($scope.projeto.Clientes, function (cliente) {
                            $scope.model.clientesNomes += cliente.Nome + "; ";
                        });
                        
                        $scope.salvarProjeto = function () {

                            $scope.projeto.$save(function (data) {

                                var messageSuccess = WEX.message.success('Projeto "' + $scope.projeto.Nome + '" atualizado');

                                setTimeout(messageSuccess.close, 2000);

                            }, function (error) {

                                if (error.status === 409) {

                                    WEX.message.validation("Preencha todos os campos obrigatórios", { title: "Problemas ao salvar o projeto", modal: true });

                                } else if (status === 403) {

                                    WEX.message.fatal('Projeto "' + $scope.projeto.Nome + '" não está vazio e não pode ser movido como Projeto Micro', { title: "Operação proibida" });

                                } else {

                                    WEX.message.fatal("Infelizmente a aplicação comportou-se de maneira inesperada. Entre em contato com o administrador");
                                }

                            });

                        };

                        $scope.alterarTipoProjeto = function () {
                            if (!$scope.projeto.IsMacro) {
                                $scope.projeto.Gerente = $scope.projeto.ProjetoMacro.Gerente;
                                $scope.projeto.CentroCusto = $scope.projeto.ProjetoMacro.CentroCusto;
                            }
                            $scope.salvarProjeto();
                            projetoViewModel.projetosMacros = projetoResource.macros();
                        };

                    }

                };

            }]);


    /**
     *
     **/
    WEX.Projeto.View.directive("clientesAutocomplete", ["projeto.view.model", function (projetoViewModel) {

        var split = function (val) {
            return val.split(/;\s*/);
        }

        var extractLast = function (term) {
            return split(term).pop();
        }

        // envia os valores selecionados para o array scope.toModel
        var transportValues = function (terms, scope) {

            scope.toModel = [];

            angular.forEach(terms, function (term) {

                angular.forEach(projetoViewModel.clientes, function (cliente) {
                    if (term == cliente.Nome) {
                        scope.toModel.push({
                            IdCliente: cliente.Oid,
                            Nome: cliente.Nome
                        });
                    }
                });

            });

        };

        return {
            restrict: 'A', scope: { toModel: '=', close: "&" },

            link: function (scope, element, attrs) {

                if (typeof scope.toModel == "undefined") {
                    throw "Atributo [to-model] não especificado.";
                }

                element.bind("keydown", function (event) {
                    if (event.keyCode === $.ui.keyCode.TAB && $(this).data("ui-autocomplete").menu.active) {
                        event.preventDefault();
                    }
                });

                element.autocomplete({

                    minLength: 0,

                    source: function (request, response) {
                        // trecho retirado e readaptado do codigo do jquery.ui ($.ui.autocomplete.filter)
                        var matcher = new RegExp(extractLast(request.term).replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&"), "i");
                        response($.grep(projetoViewModel.clientes, function (value) {
                            return matcher.test(value.Nome || value.Sigla);
                        }));
                    },

                    focus: function () {
                        return false;
                    },

                    close: function () {
                        var terms = split(this.value);
                        scope.$apply(function () {
                            transportValues(terms, scope);
                        });
                        scope.$apply(scope.close);
                        return false;
                    },

                    select: function (event, ui) {
                        var terms = split(this.value);
                        terms.pop();
                        terms.push(ui.item.Nome);
                        terms.push("");
                        this.value = terms.join("; ");
                        return false;
                    }

                });

                var autoComplete = element.data("ui-autocomplete");

                if (typeof(autoComplete) == "undefined")
                    autoComplete = element.data("autocomplete");
                
                autoComplete._renderItem = function (ul, item) {
                    return jQuery("<li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + item.Nome + "</a>")
                        .appendTo(ul);

                };

            }

        };

    }]);

})();