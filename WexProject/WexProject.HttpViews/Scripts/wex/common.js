/*global WEX*/

(function () {
	"use strict";

	WEX.CommonModulo = angular.module("wex.common", ["ui.bootstrap", "pascalprecht.translate"]);

	WEX.CommonModulo.config(["$httpProvider", "$translateProvider", function ($httpProvider, $translateProvider) {
		$httpProvider.defaults.headers.delete = {
			"Content-Type": $httpProvider.defaults.headers.post["Content-Type"]
		};
		$translateProvider.preferredLanguage('pt_BR');
		$translateProvider.useStaticFilesLoader({
			prefix: '/languages/',
			suffix: '.json'
		});
	}]);

	WEX.CommonModulo
        .directive("datepicker", ["$log", function ($log) {
            return {
                restrict: "A",
                require: "datepicker",
                link: function (scope, elem, attrs, ctrl) {
                    switch (attrs.mode) {
                        case "day":
                         //Não precisa fazer nada
                            break;
                        case "month":
                            ctrl.modes.splice(0, 1);
                            break;
                        case "year":
                            ctrl.modes.splice(0, 2);
                            break;
                        default:
                            if (angular.isString(attrs.mode)) {
                                $log.warn("Modo desconhecido");
                            }
                            break;
                     }
                }
            };
        }])
		.directive("autocomplete", ["$parse", function ($parse) {
			return {
				restrict: "A",
				scope: {
					dados: "=autocomplete",
					selecionado: "=modelo",
					seleciona: "&seleciona"
				},
				controller: ["$scope", function ($scope) {
					var self;

					self = this;

					self.seleciona = function () {
						$scope.$apply($scope.seleciona);
					};
				}],
				link: function (scope, element, attrs, ctrl) {
					var input;

					input = $("input", element);
					input.autocomplete({
						source: function (request, response) {
							var filtrados = scope.dados.filter(function (dado) {
								return dado.value.toLowerCase().indexOf(request.term.toLowerCase()) !== -1;
							});

							response(filtrados);
						},
						select: function (event, ui) {
							scope.selecionado = ui.item;
							scope.$apply();
						}
					});

					input.bind("keypress", function (event) {
						if (event.which === 13) {
							ctrl.seleciona();
						}
					});

					scope.$watch("selecionado", function (valorNovo) {
						input.val(scope.selecionado ? scope.selecionado.Nome : "");
					});
				}
			};
		}])
		.directive("autocompleteSeleciona", function () {
			return {
				restrict: "A",
				require: "^autocomplete",
				link: function (scope, elem, attrs, ctrl) {
					elem.bind("click", ctrl.seleciona);
				}
			};
		})
		.directive("aba", [function () {
			var classeSelecionado;

			classeSelecionado = "tabSelecionada";

			return {
				restrict: "A",
				link: function (scope, elem, attrs) {
					var cabecalho;

					elem = $(elem);

					cabecalho = $("<div>");
					cabecalho.text(attrs.aba);
					cabecalho.addClass("tabs");

					elem.parent().children("[aba]:first").before(cabecalho);

					if (elem.parent().children("[aba]:first").is(elem)) {
						elem.show();
						cabecalho.addClass(classeSelecionado);
					} else {
						elem.hide();
						cabecalho.removeClass(classeSelecionado);
					}

					cabecalho.bind("click", function () {
						scope.$apply(selecionaAba);
					});

					scope.$on("selecionaAba", function (event, aba) {
						if (aba === attrs.aba) {
							selecionaAba();
						}
					});

					function selecionaAba() {
						if (!cabecalho.hasClass(classeSelecionado)) {
							elem.siblings("[aba]").hide();
							elem.show();
							cabecalho.addClass(classeSelecionado);
							cabecalho.siblings(".tabs").removeClass(classeSelecionado);
						}
					}
				}
			};
		}])
		.directive("collapse", function () {
			return {
				restrict: "A",
				controller: ["$attrs", function ($attrs) {
					var _cabecalho,
						_conteudo,
						_fechado,
						_aberto,
						self;

					self = this;

					_cabecalho = $();
					_conteudo = $();

					_fechado = $attrs.fechado || "statusFechado";
					_aberto = $attrs.aberto || "statusAberto";

					self.cabecalho = function (elem) {
						_cabecalho = _cabecalho.add(elem);

						elem.addClass(_fechado);

						elem.bind("click", function () {
							if (_conteudo.is(":visible")) {
								_cabecalho.removeClass(_aberto);
								_cabecalho.addClass(_fechado);

								_conteudo.hide();
							} else {
								_cabecalho.removeClass(_fechado);
								_cabecalho.addClass(_aberto);

								_conteudo.show();
							}
						});
					};

					self.conteudo = function (elem) {
						_conteudo = _conteudo.add(elem);

						elem.hide();
					};
				}],
			};
		})
		.directive("collapseCabecalho", function () {
			return {
				restrict: "A",
				require: "^collapse",
				link: function (scope, elem, attrs, ctrl) {
					ctrl.cabecalho(elem);
				}
			};
		})
		.directive("collapseConteudo", function () {
			return {
				restrict: "A",
				require: "^collapse",
				link: function (scope, elem, attrs, ctrl) {
					ctrl.conteudo(elem);
				}
			};
		})
		.directive("data", [function () {
			return {
				require: "ngModel",
				link: function (scope, elem, attrs, ctrl) {
					ctrl.$parsers.push(function (data) {
						if (data) {
							return new Date(data);
						}
					});

					ctrl.$formatters.push(function (data) {
						if (data) {
							return data.toISOString().replace(/T.*/, "");
						}
					});
				}
			};
		}])
		.directive("valida", ["$parse", function ($parse) {
			return {
				restrict: "A",
				link: function (scope, elem, attrs) {
					if (attrs.validaCampo) {
						scope.$watch(attrs.valida + ".erros", function (erros) {
							if (erros && erros[attrs.validaCampo]) {
								elem.attr("data-hint", erros[attrs.validaCampo].join(" "));
								elem.addClass(WEX.feedback.erroClasse);
							} else {
								elem.removeAttr("data-hint");
								elem.removeClass(WEX.feedback.erroClasse);
							}
						}, true);
					}
				}
			};
		}])
		.directive("valor", ["Dinheiro", "$parse", function (Dinheiro, $parse) {
			return {
				require: "ngModel",
				link: function (scope, element, attrs, ctrl) {
					var permiteNulo;

					permiteNulo = $parse(attrs.valor)(scope);

					if (typeof permiteNulo === "undefined") {
						permiteNulo = true;
					}

					element.bind("change", function () {
						scope.$apply(function () {
							element.val(Dinheiro.format(element.val(), permiteNulo));
						});
					});

					ctrl.$parsers.push(function (valor) {
						if (valor !== undefined) {
							return Dinheiro.parse(valor);
						}
					});

					ctrl.$formatters.push(function (valor) {
						if (valor !== undefined) {
							return Dinheiro.format(valor, permiteNulo);
						}
					});
				}
			};
		}])
		.directive("wexDebounce", ["$timeout", function ($timeout) {
			return {
				restrict: "A",
				require: "ngModel",
				link: function (scope, elem, attrs) {
					var promessa,
						debounce;

					debounce = function () {
						scope.$eval(attrs.wexDebounce);
					};

					scope.$watch(attrs.ngModel, function (valorNovo, valorAntigo) {
						if (valorNovo !== valorAntigo) {
							$timeout.cancel(promessa);
							promessa = $timeout(debounce, 1000, false);
						}
					});
				}
			};
		}])
		.directive("wexDropdown", [function () {
			return {
				restrict: "A",
				controller: ["$scope", function ($scope) {
					var self,
						_cabecalho,
						_conteudo;

					self = this;

					self.mostrar = function () {
						_conteudo.show();
					};

					self.esconder = function () {
						_conteudo.hide();
					}

					self.cabecalho = function (elem) {
						_cabecalho = elem;
					};

					self.conteudo = function (elem) {
						_conteudo = elem;

						elem.hide();
					};
				}],
				link: function (scope, elem, attrs, ctrl) {
					elem.on("mouseenter", function () {
						scope.$apply(ctrl.mostrar);
					});

					elem.on("mouseleave", function () {
						scope.$apply(ctrl.esconder);
					});
				}
			};
		}])
		.directive("wexDropdownCabecalho", [function () {
			return {
				restrict: "A",
				require: "^wexDropdown",
				link: function (scope, elem, attrs, ctrl) {
					ctrl.cabecalho(elem);
				}
			};
		}])
		.directive("wexDropdownConteudo", [function () {
			return {
				restrict: "A",
				require: "^wexDropdown",
				link: function (scope, elem, attrs, ctrl) {
					ctrl.conteudo(elem);
				}
			};
		}]);

	WEX.CommonModulo
		.filter("data", ["Data", function (Data) {
			return function (data) {
				var ISODate;

				if (typeof data === "object" && data instanceof Date) {
					ISODate = data.toISOString();
				} else if (typeof data === "string") {
					ISODate = data;
				} else {
					return "";
				}

				return Data.toCommonDateString(ISODate);
			};
		}])
		.filter("monetario", ["Dinheiro", function (Dinheiro) {
			return function (texto, permiteNulo) {
				if (typeof permiteNulo === "undefined") {
					permiteNulo = false;
				}

				return Dinheiro.format(texto, permiteNulo);
			}
		}])
		.filter("asciify", function () {
			var substituicoes,
				substituicoes_regex;

			substituicoes = {
				"á": "a",
				"à": "a",
				"â": "a",
				"ã": "a",
				"ç": "c",
				"é": "e",
				"ê": "e",
				"í": "i",
				"ó": "o",
				"ô": "o",
				"õ": "o",
				"ú": "u",
			};
			substituicoes_regex = /[áàâãçéêíóôõú]/g;

			return function (texto) {
				return texto.replace(substituicoes_regex, function (match) {
					return substituicoes[match];
				});
			};
		})
		.filter("camelCase", function () {
			return function (texto) {
				return texto.trim()
					.replace(/[^\w\s]/g, "")
					.toLowerCase()
					.replace(/\s+(\w)/g, function (match, group) {
						return group.toUpperCase();
					});
			};
		});

	WEX.CommonModulo
		.factory("AdicionaErro", [function () {
			return function (obj, attr, msg) {
				var erros;

				erros = (obj.erros || (obj.erros = {}));
				(erros[attr] || (erros[attr] = [])).push(msg);
			}
		}])
		.factory("Patrocinadores", ["$http", function ($http) {
			var Patrocinadores,
				empresas,
				patrocinadores;

			Patrocinadores = {};
			empresas = [];
			patrocinadores = [];

			Patrocinadores.get = function (aditivoId) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/Patrocinadores",
					method: "get"
				})
					.success(function (data, status, headers, config) {
						patrocinadores.replace(data.patrocinadores);
					});
				return patrocinadores;
			};

			Patrocinadores.put = function (aditivoId, patrocinador) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/Patrocinadores/" + patrocinador.Oid,
					method: "post"
				})
					.success(function (data, status, headers, config) {
						patrocinadores.push(patrocinador);
					});
			};

			Patrocinadores.remove = function (aditivoId, indice) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/Patrocinadores/" + patrocinadores[indice].Oid,
					method: "delete"
				})
					.success(function (data, status, headers, config) {
						if (data.id !== 0) {
							WEX.custo.aditivo.excluiPatrocinador(patrocinadores, indice);
						}
					});
			};

			Patrocinadores.empresas = function () {
				$http({
					url: "/Patrocinadores",
					method: "get"
				})
					.success(function (data, status, headers, config) {
						empresas.replace(data.patrocinadores);
						empresas.forEach(function (empresa) {
							empresa.value = empresa.Nome;
						});
					});

				return empresas;
			};

			return Patrocinadores;
		}])
		.factory("Data", function () {
			var Data,
				regexISODateString,
				regexCommonDateString,
				regexJSONDateString;

			regexISODateString = /(\d{4})-(\d{2})-(\d{2})/;
			regexCommonDateString = /(\d{2})\/(\d{2})\/(\d{4})/;
			regexJSONDateString = /\/(Date\(\d+\))\//;

			Data = {};

			Data.toDate = function (data) {
				data = data.match(regexISODateString);

				if (data === null) {
					return new Date();
				}

				return new Date(data[1], data[2] - 1, data[3]);
			};

			Data.toCommonDateString = function (dataString) {
				var data;

				if (typeof dataString !== "string") {
					return "";
				}

				data = dataString.match(regexISODateString);

				if (data === null) {
					if (regexCommonDateString.test(dataString)) {
						return dataString;
					}

					return "";
				}

				return data[3] + "/" + data[2] + "/" + data[1];
			};

			Data.toISODateString = function (dataString) {
				var data;

				if (typeof dataString !== "string") {
					return "";
				}

				data = dataString.match(regexCommonDateString);

				if (data === null) {
					if (regexISODateString.test(dataString)) {
						return dataString;
					}

					return "";
				}

				return data[3] + "-" + data[2] + "-" + data[1];
			};

			Data.fromJSONDate = function (data) {
				if (typeof data === "string" && regexJSONDateString.test(data)) {
					return eval(data.replace(regexJSONDateString, "new $1;"));
				}

				return null;
			};

			return Data;
		})
		.factory("Dinheiro", function () {
			return WEX.custo.Dinheiro;
		})
		.factory("Meses", function ($filter) {
			var Meses;

			Meses = [
			{
			    longo: $filter('translate')('Janeiro'),
			    curto: $filter('translate')('Jan')
			},
			{
			    longo: $filter("translate")("Fevereiro"),
			    curto: $filter("translate")("Fev")
			},
			{
			    longo: $filter("translate")("Março"),
			    curto: $filter("translate")("Mar")
			},
			{
			    longo: $filter("translate")("Abril"),
			    curto: $filter("translate")("Abr")
			},
			{
			    longo: $filter("translate")("Maio"),
			    curto: $filter("translate")("Mai")
			},
			{
			    longo: $filter("translate")("Junho"),
			    curto: $filter("translate")("Jun")
			},
			{
			    longo: $filter("translate")("Julho"),
			    curto: $filter("translate")("Jul")
			},
			{
			    longo: $filter("translate")("Agosto"),
			    curto: $filter("translate")("Ago")
			},
			{
			    longo: $filter("translate")("Setembro"),
			    curto: $filter("translate")("Set")
			},
			{
			    longo: $filter("translate")("Outubro"),
			    curto: $filter("translate")("Out")
			},
			{
			    longo: $filter("translate")("Novembro"),
			    curto: $filter("translate")("Nov")
			},
			{
			    longo: $filter("translate")("Dezembro"),
			    curto: $filter("translate")("Dez")
			}
			];

			return Meses;
		})
		.factory("Situacoes", ["$http", function ($http) {
			var Situacoes;

			Situacoes = [];

			$http({
				url: "/Projetos/Situacoes",
				method: "get"
			})
				.success(function sucesso(data, status, headers, config) {
					Situacoes.replace(data.situacoes);
				})
				.error(function falha(data, status, headers, config) {
					WEX.feedback.erroGeral("<span>Erro ao recuperar Situações!</span>");
				});

			return Situacoes;
		}])
		.factory("Classes", ["$http", function ($http) {
			var Classes;

			Classes = [];

			$http({
				url: "/Projetos/Classes",
				method: "get"
			})
				.success(function (data, status, headers, config) {
					Classes.replace(data.classes);
				});

			return Classes;
		}])
		.factory("Modal", ["$modal", function ($modal) {
			var Modal,
				escopo,
				opcoes;

			escopo = {};
			opcoes = {
				keyboard: false,
				backdrop: "static",
				controller: ["$scope", function ($scope) {
					$scope = angular.extend($scope, escopo);
				}]
			};

			Modal = function (opts) {
				opts = angular.extend({}, opcoes, opts);

				return $modal.open(opts).result;
			};

			Modal.abrir = function (opts) {
				opts = angular.extend({}, opcoes, opts);

				return $modal.open(opts);
			};

			Modal.confirmar = function (titulo, texto, botoes) {
				opcoes.template = "<div class='modal-header modal-exclusao-header'>" +
									"	<h2>{{titulo}}</h2>" +
									"</div>" +
									"<div class='modal-body modal-exclusao-body'>" +
									"	<p>{{texto}}</p>" +
									"</div>" +
									"<div class='modal-footer modal-exclusao-footer'>" +
									"	<button class='btn {{botao.cssClass}}' ng-click='$close(botao.resultado)' ng-repeat='botao in botoes'>{{botao.label}}</button>" +
									"</div>";

				escopo.titulo = titulo;
				escopo.texto = texto;
				escopo.botoes = [{ resultado: 'sim', label: 'Sim', cssClass: 'btn-confirmar' }, { resultado: 'nao', label: 'Não', cssClass: 'btn-negar' }];

				return $modal.open(opcoes).result;
			};

			return Modal;
		}])
		.factory("Memoizar", [function () {
			return function (funcao) {
				var cache;

				cache = {};

				return function () {
					var chave;

					chave = JSON.stringify(arguments);

					return (chave in cache) ? cache[chave] : cache[chave] = funcao.apply(this, arguments);
				};
			};
		}]);

	WEX.feedback = {};
	WEX.feedback.erroClasse = "hint--bottom hint--error hint--always hint--rounded";

	WEX.feedback.erro = function (objeto, mensagem) {
		objeto
			.attr("data-hint", mensagem)
			.addClass("hint--bottom")
			.addClass("hint--error")
			.addClass("hint--always")
			.addClass("hint--rounded");
	};
	WEX.feedback.erroGeral = function (mensagem) {
		var feedback;

		feedback = $("#feedback");

		feedback.find("#msg")
			.empty()
			.html(mensagem);

		feedback
			.addClass("feedbackErro")
	        .removeClass("feedbackCarregando")
			.show();
	};

	WEX.feedback.infoGeral = function (mensagem) {
		var feedback;

		feedback = $("#feedback");

		feedback.find("#msg")
			.empty()
			.html(mensagem);

	    feedback
			.removeClass("feedbackErro")
            .removeClass("feedbackCarregando")
			.show()
			.delay(5000)
			.fadeOut("slow");
	};

	WEX.feedback.carregando = function (mensagem) {

	    var feedback, spin, backdrop, messageElement;

	    mensagem = mensagem || "Carregando...";

	    feedback = $("#feedback");

	    messageElement = $("#feedback").find("#msg");

	    spin = $("<img src='/Content/themes/temaPadrao/loading_fpf.gif' class='feedback-spin' />");

	    backdrop = $("<div>");

		messageElement.html(mensagem);

		if (feedback.find(".feedback-spin").length == 0) {
		    messageElement.before(spin);
		}

		if ($(".modal-backdrop").length == 0) {
		    backdrop
                .addClass("modal-backdrop")
                .addClass("backdrop-white")
                .appendTo("body");
		}

	    feedback
            .addClass("feedbackCarregando")
			.removeClass("feedbackErro")
            .show();

		return {
		    carregado: function carregado() {
		        feedback.hide("fade", {
		            complete: function () {
		                spin.remove();
		                backdrop.remove();
		            }
		        }, "fast");
			}
		};

	};

	WEX.feedback.init = function () {
		$("#btnFecharFeedback").on("click", function () {
			var feedback;

			feedback = $("#feedback");

			feedback.hide("fade", {
			    complete: function () {
			        feedback.find(".feedback-spin").remove();
			        $(".modal-backdrop").remove();
			    }
			}, "fast");

		});
	};

	$(WEX.feedback.init);

	WEX.message = function () {

	    var _defaultOptions = {
            title: null,
	        modal: false,
	        icon: null,
	        canClose: true
	    };

	    var _createMessage = function (type, messages, options) {

	        var opts = options || _defaultOptions;

	        var messageElement = jQuery("<div>");
	        messageElement.addClass("wex-message");
	        messageElement.addClass("wex-message-" + type);
	        messageElement.appendTo("body");

	        var messageIcon = null;

	        if (opts.icon != null) {
	            messageIcon = jQuery("<div>");
	            messageIcon.addClass("wex-message-icon");
	            messageIcon.addClass(opts.icon);
	            messageIcon.appendTo(messageElement);
	        }

	        var messageContent = jQuery("<div>");
	        messageContent.addClass("wex-message-content");
	        messageContent.appendTo(messageElement);

	        if (opts.title != null) {
	            var messageTitle = jQuery("<h1>");
	            messageTitle.addClass("wex-message-title");
	            messageTitle.text(opts.title);
	            messageTitle.appendTo(messageContent);
	        }

	        if (typeof messages === "string") {
	            messageContent.append(jQuery("<p>").html(messages));
	        } else {
	            var list = jQuery("<ul>");
	            for (var i = 0, l = messages.length; i < l; i++) {
	                jQuery("<li>").html(messages[i]).appendTo(list);
	            }
	            list.appendTo(messageContent);
	        }

	        var messageModal = null;

	        if (opts.modal) {
	            messageModal = jQuery("<div>");
	            messageModal.addClass("wex-message-modal-overlay");
	            messageModal.appendTo("body");
	        }

	        var _actions = {};

	        _actions.close = function () {

	            messageElement.hide("fade", {
	                complete: function () {
	                    messageElement.remove();
	                }
	            }, "fast");

	            if (messageModal != null) {
	                messageModal.hide("fade", {
	                    complete: function () {
	                        messageModal.remove();
	                    }
	                }, "fast");

	            }
	        };

	        if (opts.canClose) {

	            var buttonClose = jQuery("<button>");

	            buttonClose.addClass("wex-message-button-close");
	            buttonClose.text("X");
	            buttonClose.appendTo(messageElement);

	            messageElement.addClass("can-close");

	            buttonClose.click(_actions.close);

	        }

	        return _actions;

	    };

	    return {

	        loading: function (message, options) {
	            var opts = options || _defaultOptions;
	            opts.icon = "wex-message-icon-loading";
	            opts.canClose = false;
	            return _createMessage("loading", message, opts);
	        },
	        
	        success: function (message, options) {
	            var opts = options || _defaultOptions;
	            opts.canClose = true;
	            return _createMessage("success", message, opts);
	        },
	        
	        validation: function (message, options) {
	            var opts = options || _defaultOptions;
	            opts.canClose = true;
	            return _createMessage("validation", message, opts);
	        },
	        
	        error: function (message, options) {
	            var opts = options || _defaultOptions;
	            opts.canClose = true;
	            return _createMessage("error", message, opts);
	        },
	        
	        fatal: function (message, options) {
	            var opts = options || _defaultOptions;
	            opts.modal = true;
	            opts.canClose = true;
	            return _createMessage("fatal", message, opts);
	        }

	    };

	}();

	WEX.CommonModulo.directive('stickyHeaders', function () {
	    return {
	        restrict: "A",
	        link: function (scope, element, attrs) {
	            element.stickyTableHeaders();
	        }
	    };
	});

})();
