/// <reference path="~/Scripts/angular-1.3.0-beta.13/angular.min.js" />
/// <reference path="~/Scripts/angular-1.3.0-beta.13/angular-resource.min.js" />
/// <reference path="~/Scripts/wex/main.js" />
/// <reference path="~/Scripts/wex/common.js" />
/// <reference path="~/Scripts/wex/eventos/config.js" />
/// <reference path="~/Scripts/wex/eventos/aditivos.js" />
/// <reference path="~/Scripts/wex/custos/config.js" />
/// <reference path="~/Scripts/wex/custos/termoAditivo.js" />
/// <reference path="~/Scripts/wex/custos/rubricas.js" />

(function () {
	"use strict";

	var PRISTINE_CLASS = "ng-pristine",
		DIRTY_CLASS = "ng-dirty";

	WEX.custo.CustosDirective = angular.module("custos.directive", []);
	WEX.custo.CustosFilters = angular.module("custos.filters", []);
	WEX.custo.CustosModule = angular.module("custos", ["custos.directive", "custos.filters", "wex.common", "wex.projetos.selecao", "wex.service", "ngStorage"]);

	WEX.custo.ProjetoCtrl = ["$scope", "$filter", "Projeto", "Projetos", "Aditivos", "Aditivo", "Situacoes", "ModalSelecao", "Dinheiro", "PopulaAnos", "$sessionStorage", "$window", "$location", function ($scope, $filter, Projeto, Projetos, Aditivos, Aditivo, Situacoes, ModalSelecao, Dinheiro, populaAnos, $sessionStorage, $window, $location) {

		var loadingMessage = WEX.message.loading($filter('translate')('Os projetos estão sendo carregados...'), {
			modal: true
		});

		Projetos.listaCarregada().then(
			function sucesso() {
				loadingMessage.close();
			},
			function falha() {
				loadingMessage.close();
				WEX.message.error($filter('translate')('Entre em contato com o administrador'), {
					title: $filter('translate')('Erro ao carregar os projetos')
				});
			}
		);

		$scope.projeto = Projeto;

		$scope.aditivos = [];

		$scope.situacoes = Situacoes;

		$scope.$on("$locationChangeSuccess", function (event, novaUrl) {
			var idRegex,
				projetoId;

			idRegex = /http:\/\/.*\/Custos(\/)?/i;

			projetoId = novaUrl.replace(idRegex, "");

			Projetos.listaCarregada().then(function () {
				if (projetoId === "") {
					if (typeof $sessionStorage.projetoOid !== "undefined") {
						$location.path("/Custos/" + $sessionStorage.projetoOid);
					} else {
						ModalSelecao.abrir();
						ModalSelecao.podeFechar = false;
					}
				} else {
					ModalSelecao.fechar();
					ModalSelecao.podeFechar = true;

					$sessionStorage.projetoOid = projetoId;

					Projetos.get(projetoId).then(function (projeto) {
						Object.replace($scope.projeto, projeto);

						$scope.$emit("recuperadoProjeto", $scope.projeto);
					});

					$scope.aditivos = Aditivos.get(projetoId);
				}
			});
		});

		$scope.$on("mudouProjeto", function (event) {
			$scope.$broadcast("fechaAditivo");
		});

		$scope.mudarProjeto = function () {
			ModalSelecao.abrir();
		};

		$scope.adicionaAditivo = function (event) {
			$scope.$broadcast("fechaAditivo");

			$scope.aditivos.push(new Aditivo({ Projeto: $scope.projeto.Oid, editando: true, selecionado: true }));
			event.stopPropagation();
		};

		$scope.selecionaAditivo = function (indice) {
			var aditivo;

			aditivo = $scope.aditivos[indice];

			if (aditivo.selecionado === false) {
				$scope.aditivos.forEach(function (aditivo) {
					aditivo.selecionado = false;
				});

				aditivo.selecionado = true;

				if (typeof aditivo.erros === "undefined") {
					$scope.$broadcast("mostraAditivo", aditivo);
				}
			}
		};

		$scope.editaAditivo = function (indice) {
			$scope.aditivos[indice].editando = true;
		};

		$scope.excluiAditivo = function (event, indice) {
			var selecionado;

			selecionado = $scope.aditivos[indice].selecionado;

			Aditivos.remove(indice).then(function () {
				if (selecionado) {
					$scope.$broadcast("fechaAditivo");
				}

				atualizaValor();
			});

			event.stopPropagation();
		};

		$scope.congelaAditivo = function (aditivo) {
			congelarAditivo(aditivo);
		};

		$scope.stopPropagation = function (event) {
			event.stopPropagation();
		};

		$($window).on("click", function () {
			$scope.$apply(function () {
				$scope.aditivos.forEach(congelarAditivo);
			});
		});

		function congelarAditivo(aditivo) {
			var dirty;

			if (aditivo.editando === true) {
				if (Aditivo.isValido(aditivo)) {
					populaAnos(aditivo);

					Aditivos.salva(aditivo).then(function () {
						$scope.$broadcast("pristine", aditivo.AditivoId);

						$scope.$broadcast("mostraAditivo", aditivo);
					});

					atualizaValor();
				} else {
					$scope.$broadcast("fechaAditivo");
				}

				aditivo.editando = false;
			}
		}

		function atualizaValor() {
			$scope.projeto.Valor = 0;

			$scope.aditivos.forEach(function (aditivo) {
				$scope.projeto.Valor += Dinheiro.parse(aditivo.Orcamento, 0);
			});
		}
	}];
	WEX.custo.AditivoCtrl = ["$scope", "$http", "$filter", "Rubrica", "Rubricas", "Aditivos", "Aditivo", "TiposRubricas", "PopulaRubricas", "CopiaArrayAsync", "Dinheiro", function ($scope, $http, $filter, Rubrica, Rubricas, Aditivos, Aditivo, TiposRubricas, PopulaRubricas, CopiaArrayAsync, Dinheiro) {
		$scope.aberto = false;

		$scope.rubricas = Rubricas.listar();

		$scope.tipos = TiposRubricas;

		$scope.$on("mostraAditivo", function (event, aditivo) {
			var classe,
				carregando;

			Aditivo.AditivoId = aditivo.AditivoId;

			if ($scope.aberto && $scope.aditivo === aditivo) {
				// Atualizando após editar
				TiposRubricas.classes.forEach(function (classe) {
					PopulaRubricas.atualiza(aditivo, classe, $scope.rubricas[classe]);
				});

				if (aditivo.Anos.indexOf(aditivo.AnoAtual) === -1) {
					aditivo.AnoAtual = aditivo.Anos[0];
				}
			} else {
				// Abrindo do servidor

				var loadingMessage = WEX.message.loading($filter('translate')('As rubricas estão sendo carregadas...'), {
					modal: true
				});

				Rubricas.carregar(aditivo.AditivoId).then(
					function sucesso() {
						loadingMessage.close();
					},
					function falha() {
						loadingMessage.close();
						WEX.message.error($filter('translate')('Entre em contato com o administrador'), {
							title: $filter('translate')('Erro ao carregar as rubricas')
						});
					}
				);		

				$scope.aditivo = aditivo;

				$scope.aberto = true;

			    /*Função para trazer o ano atual selecionado no projeto 
                ou o último ano caso o projeto não tenha ano atual*/
				var anoPresente;

				aditivo.Anos.forEach(function (ano) {
				    if (ano == new Date().getFullYear()) {
				        anoPresente = true;
				    } 
				});

				if (anoPresente) {
				    aditivo.AnoAtual = new Date().getFullYear();
				} else {
				    var tam = aditivo.Anos.length;
				    aditivo.AnoAtual = aditivo.Anos[tam - tam];
			}

                /*Fim*/
			}

			$scope.$broadcast("selecionaAba", "Orçamento Aprovado");
		});

		$scope.$on("fechaAditivo", function (event) {
			Aditivo.AditivoId = 0;

			$scope.aberto = false;
			if (typeof $scope.aditivo !== "undefined") {
				$scope.aditivo.selecionado = false;
			}
		});

		$scope.$on(WEX.Rubrica.Evento.Atualizada, function (event, rubricaId) {
		    Rubricas.recarregarRubrica(rubricaId);
		});

		$scope.$on(WEX.Eventos.Aditivos.Atualizar, function (event, aditivoId) {
			Aditivos.recarregar(aditivoId);
		});

		$scope.valorTotal = function () {
			return $scope.aditivo ? Dinheiro.parse($scope.aditivo.Orcamento, 0) : 0;
		};

		$scope.valorRestante = function () {
			return $scope.aditivo ? Dinheiro.parse($scope.aditivo.OrcamentoRestante, 0) : 0;
		};

		$scope.fechaAditivo = function () {
			$scope.aberto = false;
			$scope.aditivo.selecionado = false;
			//TODO: Salvar modificações
		};

		$scope.repetirValor = function (rubrica, ano, mes, attr) {
			var indice,
				valor,
				tamanho;

			Rubrica.forMesesRubrica(rubrica, function (rubricaMes, anoAtual) {
				if (typeof valor === "undefined" && anoAtual.Ano === ano) {
					valor = anoAtual.Meses[mes - 1][attr];
				}

				if (pertenceAditivo(anoAtual.Ano, rubricaMes.Mes)) {
					rubricaMes[attr] = valor;
				}
			});

			function pertenceAditivo(anoTeste, mesTeste) {
				var mesInicial,
					mesFinal,
					mesAtual;

				mesInicial = (ano - 1) * 12 + mes;
				mesFinal = ($scope.aditivo.DataInicio.getUTCFullYear() - 1) * 12 + $scope.aditivo.DataInicio.getUTCMonth() + $scope.aditivo.Duracao;
				mesAtual = (anoTeste - 1) * 12 + mesTeste;

				if (mesAtual >= mesInicial && mesAtual <= mesFinal) {
					return true;
				}

				return false;
			}
		};
	}];
	WEX.custo.DespesasCtrl = [
        "$rootScope", "$scope", "$filter", "$q", "Meses", "Rubricas", "CentrosCusto", "TiposRubricas", "Projeto", "notasfiscais.resource",
        function ($rootScope, $scope, $filter, $q, Meses, Rubricas, CentrosCusto, TiposRubricas, Projeto, notasFiscaisResource) {
        	var rubricas;

        	rubricas = Rubricas.listar();

            $scope.meses = Meses;

		    $scope.tipos = TiposRubricas;

		    $scope.totalMes = function (ano, mes, classe) {
			    var totalMes;

			    totalMes = 0;

			    if (typeof rubricas[classe] === "undefined") {
				    return 0;
			    }

			    rubricas[classe].forEach(function (rubrica) {
				    var anos;

				    anos = rubrica.Anos.filter(function (anoAtual) {
					    return anoAtual.Ano === ano;
				    });

				    if (anos.length > 0) {
					    totalMes += Dinheiro.parse(anos[0].Meses[mes].Gasto, 0);
				    }
			    });

			    return totalMes;
		    };

            // ==================================================
            // implementacao referente a view de notas fiscais
            // ==================================================

		    $scope.estilo = "fixo";

		    var dateNow = new Date();
            var mesAnterior = new Date(dateNow.getFullYear(), dateNow.getMonth() - 1, 1).getMonth();

		    $scope.model = {
		        view: 'rubricas',
		        notasFiscais: [],
		        rubricas: [],
		        mesSelecionado: Meses[mesAnterior]
		    };

		    $scope.changeView = function (view) {
		        $scope.model.view = view;
		        if (view == 'notasFiscais') {
		            $scope.atualizarListaNotasFiscais();
		        }
		    };

		    var getNotasFiscaisSelecionadas = function () {
		        return $filter("filter")($scope.model.notasFiscais, { checked: true, RubricaId: null });
		    };

            var getNotasFiscaisRubricaSelecionados = function() {
                return $filter("filter")($scope.model.notasFiscais, { checked: true, RubricaId: '!!' });
            };

            var getRubricaAberta = function () {
		        var rubrica = null;
		        for (var i = 0, l = $scope.model.rubricas.length; i < l; i++) {
		            if ($scope.model.rubricas[i].model.mostrarNotasFiscais === true) {
		                rubrica = $scope.model.rubricas[i];
                        break;
		            }
		        }
		        return rubrica;
		    };

		    $scope.toggleNotasFiscaisRubrica = function (rubrica) {
		        if (rubrica.model.mostrarNotasFiscais) {
		            rubrica.model.mostrarNotasFiscais = false;
		            $scope.estilo = "fixo";
		            return;
		        }
		        angular.forEach($scope.model.rubricas, function (r) {
		            r.model.mostrarNotasFiscais = false;
		        });
		        rubrica.model.mostrarNotasFiscais = true;
		        $scope.estilo = "rubricaTituloFixo";
		    };

		    $scope.atualizarListaNotasFiscais = function () {
		        if (typeof Projeto.CentroCustoId == "undefined") {
		        	console.log($filter('translate')('Projeto não possui Centro de Custo.'), Projeto);
		            return;
		        }
		        var anoAtual = ($scope.aditivo) ? $scope.aditivo.AnoAtual : new Date().getFullYear();
		        $scope.model.notasFiscais = notasFiscaisResource.query({
                    centroCustoId: Projeto.CentroCustoId,
                    ano: anoAtual,
                    mes: Meses.indexOf($scope.model.mesSelecionado) + 1
                });
            };

		    $scope.$watch("model.mesSelecionado", function() {
		        $scope.atualizarListaNotasFiscais();
		    });
		  
		    $scope.getNotasFiscais = function (rubrica) {
		        if (typeof rubrica == "undefined") {
		            return $filter("filter")($scope.model.notasFiscais, { RubricaId: null });
		        } else {
		            return $filter("filter")($scope.model.notasFiscais, { RubricaId: rubrica.RubricaId });
		        }
		    };

		    $scope.calcularTotalNotasFiscais = function (rubrica) {
		        var valorTotal = 0;
		        angular.forEach($scope.getNotasFiscais(rubrica), function (notaFiscal) {
		            valorTotal += notaFiscal.Valor;
		        });
		        return valorTotal;
		    };

		    $scope.exibirQuantidadeNotasFiscais = function (rubrica) {
		        var quantidadeNotasFiscais = $scope.getNotasFiscais(rubrica).length;
		        if (quantidadeNotasFiscais === 1) {		         
		            return quantidadeNotasFiscais  + " " + $filter('translate')('Nota Fiscal');
		        }
		        else if (quantidadeNotasFiscais > 1) {
		        	return quantidadeNotasFiscais + " " + $filter('translate')('Notas Fiscais');
		        }
		    };


            // -- Tratamentos checkbox--------
            //Rubricas
		    $scope.checkTodasRubricas = function (rubrica) {
		        var rubricasMarcadas = !$scope.todosMarcadosRubricas(rubrica);
		        angular.forEach($scope.getNotasFiscais(rubrica), function (r) {
		            r.checked = rubricasMarcadas;
		        });
		    };

            // Retorna verdadeiro para a funcão checkTodasRubricas() se todos os checkbox estiverem preenchidos.
		    $scope.todosMarcadosRubricas = function (rubrica) {
		        var quantidadeRubricasMarcadas = 0;
		        angular.forEach($scope.getNotasFiscais(rubrica), function (r) {
		            quantidadeRubricasMarcadas += r.checked ? 1 : 0;
		        });
		        return (quantidadeRubricasMarcadas === $scope.getNotasFiscais(rubrica).length);
		    };
		  
            //Notas Fiscais
		    $scope.marcarNotasFiscais = function () {
		        var notasFiscaisMarcadas = !$scope.verificarTodasNotasFiscaisMarcadas();
		        angular.forEach($scope.getNotasFiscais(), function (notaFiscal) {
		            notaFiscal.checked = notasFiscaisMarcadas;
		        });
		    };

		    // Retorna verdadeiro se todos os checkbox estiverem preenchidos.
		    $scope.verificarTodasNotasFiscaisMarcadas = function () {
		        var quantidadeNotasMarcadas = 0;
		        angular.forEach($scope.getNotasFiscais(), function (notaFiscal) {
		            quantidadeNotasMarcadas += notaFiscal.checked ? 1 : 0;
		        });
		        return (quantidadeNotasMarcadas === $scope.getNotasFiscais().length);
		    };

            // -- Fim Tratamentos checkbox

		    $scope.podeAssociarNotasFiscais = function () {
		        var testeRubricaAberta = function (rubrica) {
		            return rubrica.model.mostrarNotasFiscais;
		        };
		        var rubricaAberta = ($filter("filter")($scope.model.rubricas, testeRubricaAberta).length > 0);
		        var check = (getNotasFiscaisSelecionadas().length > 0);
		        return rubricaAberta && check;
		    };

		    $scope.podeDesassociarNotasFiscais = function () {
		        return (getNotasFiscaisRubricaSelecionados().length > 0);
		    };

		    $scope.salvarNotaFiscal = function (notaFiscal) {
		        notaFiscal.$save(function () {
		    		var messageSuccess = WEX.message.success($filter('translate')('Dados salvos com sucesso'));
		            setTimeout(messageSuccess.close, 3000);
		        }, function () {
		            WEX.message.error($filter('translate')('Problemas ao salvar os dados'));
		        });
		    };

		    $scope.associarNotasFiscais = function () {
		    	var rubricasAtualizadas,
					associacaoPromise;
				
		    	rubricasAtualizadas = [];
		    	associacaoPromise = [];

		        angular.forEach(getNotasFiscaisSelecionadas(), function (notaFiscal) {
		        	var rubrica,
		        		promise;
					
		        	rubrica = getRubricaAberta();
		        	promise = $q.defer();

		            if (!rubrica) {
		            	WEX.message.validation($filter('translate')('Rubrica não selecionada'));
		                return;
		            }

		            associacaoPromise.push(promise.promise);
		            rubricasAtualizadas.push(rubrica.RubricaId);

		            notaFiscal.RubricaId = rubrica.RubricaId;

		            notaFiscal.$associar({ aditivoId: rubrica.AditivoId, rubricaId: rubrica.RubricaId }, function () {
		            	promise.resolve();

		            	var messageSuccess = WEX.message.success($filter('translate')('As notas fiscais foram associadas com sucesso'));
		                setTimeout(messageSuccess.close, 3000);

		            }, function () {
		            	promise.reject();

		                notaFiscal.RubricaId = null;
		                WEX.message.error($filter('translate')('Problemas ao associar as notas fiscais'));

		            });

		        });

		        $q.all(associacaoPromise)
					.then(function () {
						angular.forEach(rubricasAtualizadas.unique(), function (rubricaId) {
							$rootScope.$broadcast(WEX.Rubrica.Evento.Atualizada, rubricaId);
						});
					});
		    };

		    $scope.desassociarNotasFiscais = function () {
		    	var rubricasAtualizadas,
					associacaoPromise;

		    	rubricasAtualizadas = [];
		    	associacaoPromise = [];

		    	angular.forEach(getNotasFiscaisRubricaSelecionados(), function (notaFiscal) {
		    		var rubricaId,
		        		promise;

		    		rubricaId = notaFiscal.RubricaId;
		    		promise = $q.defer();

		    		associacaoPromise.push(promise.promise);
		    		rubricasAtualizadas.push(rubricaId);

		    		notaFiscal.$desassociar({
		    			aditivoId: $scope.aditivo.AditivoId,
		    		},
					function () {
						notaFiscal.RubricaId = null;

		            	promise.resolve();

		                var messageSuccess = WEX.message.success($filter('translate')('As notas fiscais foram desassociadas com sucesso'));
		                setTimeout(messageSuccess.close, 3000);

		            }, function () {
		            	promise.reject();

		                notaFiscal.RubricaId = rubricaId;
		                WEX.message.error($filter('translate')('Problemas ao desassociar as notas fiscais'));

		            });

		        });

		        $q.all(associacaoPromise)
					.then(function () {
						angular.forEach(rubricasAtualizadas.unique(), function (rubricaId) {

							$rootScope.$broadcast(WEX.Rubrica.Evento.Atualizada, rubricaId);

						});
					});

		    };

		    var aditivoAtual = null;
            var normalizarRubrica = function(rubrica) {
                rubrica.model = {};
                rubrica.model.mostrarNotasFiscais = false;
            };

            $scope.$on("mostraAditivo", function (event, aditivo) {

                aditivoAtual = aditivo;

		        $scope.model.mes = $scope.meses[new Date().getMonth()]; // selecionando mes corrente

		        $scope.$watch("aditivo.AnoAtual", function (novoAno) {

		            $scope.atualizarListaNotasFiscais();

		        });

		        $scope.model.rubricas = notasFiscaisResource.rubricas({ aditivoId: aditivo.AditivoId }, function () {
		            angular.forEach($scope.model.rubricas, normalizarRubrica);
		        });

            });

            $scope.$on(WEX.Rubrica.Evento.Adicionada, function (event, rubrica) {

                if (!aditivoAtual) {
                    console.log("Nenhum aditivo em edição");
                    return;
                }

                normalizarRubrica(rubrica);

                $scope.model.rubricas.push(rubrica);

            });

		    $scope.$on(WEX.Rubrica.Evento.Removida, function (event, rubricaId) {

		        if (!aditivoAtual) {
		            console.log("Nenhum aditivo em edição");
		            return;
		        }

		        angular.forEach($scope.model.rubricas, function (rubricaNotaFiscal, index) {
		            if (rubricaNotaFiscal.RubricaId == rubricaId) {
		                $scope.model.rubricas.splice(index, 1);
		            }
		        });

		    });

	    }];

	WEX.custo.OrcamentoCtrl = ["$scope", "Rubricas", "Objetos", function ($scope, Rubricas, Objetos) {
		var rubricas;

		rubricas = Rubricas.listar();

		$scope.novaRubrica = function (classe) {
			var novaRubrica;

			novaRubrica = new WEX.custo.aditivo.Rubrica({
				AditivoId: $scope.aditivo.AditivoId
			});

			rubricas[classe].push(novaRubrica);
		};

	}];
	WEX.custo.CentrosCustoCtrl = ["$scope", "CentrosCusto", function ($scope, CentrosCusto) {
		$scope.centrosCusto = CentrosCusto.centrosCusto();

		$scope.adicionaCentroCusto = function () {
			CentrosCusto.put($scope.aditivo.AditivoId, $scope.novoCentroCusto);
			$scope.novoCentroCusto = {};
		};
	}];
	WEX.custo.PatrocinadoresCtrl = ["$scope", "Patrocinadores", function ($scope, Patrocinadores) {
		$scope.empresas = Patrocinadores.empresas();

		$scope.adicionaPatrocinador = function () {
			Patrocinadores.put($scope.aditivo.AditivoId, $scope.novoPatrocinador);
			$scope.novoPatrocinador = {};
		};
	}];
	WEX.custo.TotalizacaoCtrl = ["$scope", "Totais", function ($scope, Totais) {
		$scope.totais = Totais;

		$scope.anoAtual = undefined;

		$scope.mostrarAno = function (ano) {
			$scope.anoAtual = ano;
		};

		$scope.$watch("aditivo.Anos", function (anos) {
			if (anos !== undefined) {
				Totais.atualizaAnos(anos);
			}
		}, true);

		$scope.$watch("aditivo.AnoAtual", function (novoAno) {
			if (novoAno) {
				$scope.anoAtual = novoAno;
			}
		});
	}];

	WEX.custo.CustosDirective
		.directive("ngModel", [function () {
			return {
				require: "ngModel",
				link: function (scope, elem, attrs, ctrl) {
					scope.$on("pristine", function (event, aditivoId) {
						if (scope.aditivo && scope.aditivo.AditivoId === aditivoId) {
							ctrl.$dirty = false;
							ctrl.$pristine = true;
							elem.removeClass(DIRTY_CLASS).addClass(PRISTINE_CLASS);
						}
					});
				}
			};
		}])
		.directive("centrosCusto", ["CentrosCusto", function (CentrosCusto) {
			return {
				restrict: "A",
				scope: {
					aditivoId: "=aditivoId"
				},
				templateUrl: "/Angular/centrosCusto",
				link: function (scope, element, attrs) {
					scope.$watch("aditivoId", function (novoId) {
						if (novoId !== undefined) {
							scope.centrosCusto = CentrosCusto.get(novoId);
						}
					});

					scope.excluiCentroCusto = function (indice) {
						CentrosCusto.remove(scope.aditivoId, indice);
					};
				}
			};
		}])
		.directive("patrocinadores", ["Patrocinadores", function (Patrocinadores) {
			return {
				restrict: "A",
				scope: {
					aditivoId: "=aditivoId"
				},
				templateUrl: "/Angular/patrocinadores",
				link: function (scope, element, attrs) {
					scope.$watch("aditivoId", function (novoId) {
						if (novoId !== undefined) {
							scope.patrocinadores = Patrocinadores.get(novoId);
						}
					});

					scope.excluiPatrocinador = function (indice) {
						Patrocinadores.remove(scope.aditivoId, indice);
					};
				}
			};
		}])
		.directive("rubricas", ["$rootScope", "$http", "$filter", "Modal", "Meses", "Aditivo", "Rubrica", "Rubricas", "TiposRubricas", "Autosave", "Objetos", "PopulaRubricas", "Dinheiro", "hasFlagFilter", function ($rootScope, $http, $filter,Modal, Meses, Aditivo, Rubrica, Rubricas, TiposRubricas, Autosave, Objetos, PopulaRubricas, Dinheiro, hasFlagFilter) {
			return {
				restrict: "A",
				scope: {
					aditivo: "=aditivo",
					classe: "=classe",
					aditivoId: "=aditivoId"
				},
				templateUrl: "/Angular/rubricas",
				controller: [function () {
					var pais,
						self;

					pais = {};
					self = this;

					self.addPai = function (paiId, paiCtrl) {
						pais[paiId] = paiCtrl;

						paiCtrl.getRubrica().TotalPlanejado = 0;
					};

					self.addFilho = function (paiId, filhoCtrl) {
						pais[paiId].addFilho(filhoCtrl);
					};

					self.removePai = function (paiId) {
						if (pais[paiId]) {
							delete pais[paiId];
						}
					};
				}],
				link: function (scope, element, attrs) {
					scope.rubricas = Rubricas.listar();

					scope.tipos = TiposRubricas;

					scope.dinheiro = Dinheiro;

					scope.meses = Meses;

					scope.excluiRubrica = function (indice) {
						var titulo,
							mensagem,
							botoes,
							httpConfig,
							rubricaId;

						titulo = $filter('translate')('Confirmar Exclusão?');
						mensagem = $filter('translate')('A Rubrica possui valores, tem certeza que deseja excluir?');

					    rubricaId = scope.rubricas[scope.classe][indice].RubricaId;

						if (rubricaId === 0) {

						    WEX.custo.aditivo.excluiRubrica(scope.rubricas, scope.classe, indice);

						} else {

							httpConfig = {
							    url: "/Custos/Aditivo/" + scope.aditivoId + "/Rubricas/" + rubricaId,
								method: "delete"
							};

							$http(httpConfig)
								.success(sucesso)
								.error(falha);

						}

						function sucesso(data, status, header, config) {

							if (data.id !== 0) {
							    WEX.custo.aditivo.excluiRubrica(scope.rubricas, scope.classe, indice, data.filhos);
							}

							$rootScope.$broadcast(WEX.Rubrica.Evento.Removida, rubricaId);

							$rootScope.$broadcast(WEX.Eventos.Aditivos.Atualizar, Aditivo.AditivoId);

						}

						function falha(data, status, header, config) {
							if (status === 405 && typeof config.data === "undefined") {
								Modal.confirmar(titulo, mensagem).then(function (resultado) {
									if (resultado === "sim") {
										config.data = {
											force: true
										};

										$http(config)
											.success(sucesso)
											.error(falha);
									}
								});
							} else {
								WEX.message.error($filter('translate')('Erro ao remover Rubrica!'));
							}
						}
					};

					scope.distribuirTotal = function (indice) {
						var rubrica,
							titulo,
							mensagem;

						titulo = $filter('translate')('Confirmar distribuição?');
						mensagem = $filter('translate')('A Rubrica possui valores preenchidos. Tem certeza que deseja distribuir?');

						rubrica = scope.rubricas[scope.classe][indice];

						if (Rubrica.rubricaVazia(rubrica, "Planejado")) {
							distribuiTotal();
						} else {
							Modal.confirmar(titulo, mensagem).then(function (resultado) {
								if (resultado === "sim") {
									distribuiTotal();
								}
							});
						}

						function distribuiTotal() {
							var valorTotal,
								valorMeses,
								quantMeses,
								valorUltimoMes;

							valorTotal = Dinheiro.parse(rubrica.TotalPlanejado, 0);
							quantMeses = scope.aditivo.Duracao;
							valorMeses = Dinheiro.format(valorTotal / quantMeses);

							valorUltimoMes = Dinheiro.format(Dinheiro.parse(valorMeses, 0) + valorTotal - (Dinheiro.parse(valorMeses, 0) * quantMeses));

							Rubrica.forMesesRubrica(rubrica, function (rubricaMes, ano) {
								if (Aditivo.pertenceAditivo(scope.aditivo, ano.Ano, rubricaMes.Mes)) {
									if (quantMeses > 1) {
										rubricaMes.Planejado = valorMeses;
									} else {
										rubricaMes.Planejado = valorUltimoMes;
									}
									quantMeses -= 1;
								} else {
									rubricaMes.Planejado = null;
								}
							});
						}
					};

					scope.limparRubrica = function (rubrica) {
						var titulo,
							mensagem;

						titulo = $filter('translate')('Confirmar limpeza da Rubrica?');
						mensagem = $filter('translate')('A Rubrica possui valores preenchidos. Tem certeza que deseja apagar os valores?');

						if (Rubrica.rubricaVazia(rubrica, "Planejado")) {
							limparRubrica();
						} else {
							Modal.confirmar(titulo, mensagem).then(function (resultado) {
								if (resultado === "sim") {
									limparRubrica();
								}
							});
						}

						function limparRubrica() {
							Rubrica.forMesesRubrica(rubrica, function (rubricaMes) {
								rubricaMes.Planejado = null;
							});
						}
					};

					scope.adicionarRubrica = function (indice, novoTipo) {
						var novaRubrica,
							salvar;

						novaRubrica = scope.rubricas[scope.classe][indice];

						novaRubrica.Tipo = novoTipo.TipoRubricaId;
						novaRubrica.Nome = novoTipo.Nome;
						novaRubrica.Classe = novoTipo.Classe;

						salvar = Autosave.enviar(novaRubrica, Objetos.Rubrica, function (data) {
							var rubricaPai,
								indicePai,
								novoIndice,
								quantRubricas;

							if (data.pai !== null) {
								novaRubrica.PaiId = data.pai;

								rubricaPai = scope.rubricas[scope.classe].filter(function (rubrica) {
									return rubrica.RubricaId === data.pai;
								})[0];

								scope.rubricas[scope.classe].splice(indice, 1);

								if (rubricaPai) {
									quantRubricas = scope.rubricas[scope.classe].length;
									indicePai = scope.rubricas[scope.classe].indexOf(rubricaPai) + 1;

									for (novoIndice = indicePai; novoIndice < quantRubricas && scope.rubricas[scope.classe][novoIndice].PaiId === data.pai; novoIndice += 1);

									scope.rubricas[scope.classe].splice(novoIndice, 0, novaRubrica);

									indice = novoIndice;

									normalizaNovaRubrica();

								} else {
									indicePai = scope.rubricas[scope.classe].length;
									novoIndice = indicePai + 1;

									$http({
										url: "/Custos/Aditivo/" + scope.aditivoId + "/Rubricas/" + data.pai,
										method: "get",
										params: { classe: scope.classe },
									})
										.success(function sucesso(data, status, headers, config) {
											scope.rubricas[scope.classe][indicePai] = data.rubrica;

											scope.rubricas[scope.classe].splice(novoIndice, 0, novaRubrica);

											indice = novoIndice;

											normalizaNovaRubrica();
										})
										.error(function falha(data, status, headers, config) {
											WEX.message.error($filter('translate')('Erro ao recuperar Rubrica.'));
										});
								}
							} else {
								normalizaNovaRubrica();
							}
						});

						salvar();

						function normalizaNovaRubrica() {
							if (hasFlagFilter(novaRubrica.Classe, TiposRubricas.classesMap.Pai)) {
								scope.rubricas[scope.classe][indice] = new WEX.custo.aditivo.Rubrica(novaRubrica);
							} else {
							    PopulaRubricas.adiciona(scope.aditivo, scope.classe, novaRubrica, indice);
							}
							$rootScope.$broadcast(WEX.Rubrica.Evento.Adicionada, novaRubrica);
						}
					};

					scope.$watch("rubricas[classe]", function (rubricas) {
						if (typeof rubricas !== 'undefined') {
							rubricas.forEach(function (rubrica) {
								var ano,
									mes,
									anos,
									meses;

								rubrica.ValorRestante = Dinheiro.parse(rubrica.TotalPlanejado, 0);

								for (ano = 0, anos = rubrica.Anos.length; ano < anos; ano++) {
									for (mes = 0, meses = rubrica.Anos[ano].Meses.length; mes < meses; mes++) {
										rubrica.ValorRestante -= Dinheiro.parse(rubrica.Anos[ano].Meses[mes].Planejado, 0);
									}
								}

								rubrica.ValorRestante = Dinheiro.format(rubrica.ValorRestante);
							});
						}
					}, true);
				}
			};
		}])
		.directive("rubrica", ["hasFlagFilter", "TiposRubricas", "Dinheiro", function (hasFlagFilter, TiposRubricas, Dinheiro) {
			return {
				restrict: "A",
				require: ["^rubricas", "rubrica"],
				controller: ["$scope", function ($scope) {
					var filhos,
						self;

					self = this;

					filhos = [];

					$scope.filhos = [];

					self.getRubrica = function () {
						return $scope.rubrica;
					};

					self.getRubricaPai = function () {
						return self.pai.getRubrica();
					};

					self.addFilho = function (filhoCtrl) {
						filhos.push(filhoCtrl);

						filhoCtrl.pai = self;

						self.getRubrica().TotalPlanejado += Dinheiro.parse(filhoCtrl.getRubrica().TotalPlanejado, 0);

						$scope.filhos.push(filhoCtrl.Nome);

						filhoCtrl.setVisivel($scope.aberto);
					};

					self.removeFilho = function (filhoCtrl) {
						filhos.remove(filhoCtrl);

						$scope.filhos.remove(filhoCtrl.Nome);
					};

					self.toggleFilhos = function () {
						$scope.aberto = !$scope.aberto;

						filhos.forEach(function (filho) {
							filho.setVisivel($scope.aberto);
						});
					};

					self.setVisivel = function (visivel) {
						$scope.visivel = visivel;
					};

					self.Nome = $scope.rubrica.Nome;
				}],
				link: function (scope, elem, attrs, ctrls) {
					var rubrica;

					rubrica = scope.rubrica;

					rubrica.pai = hasFlagFilter(rubrica.Classe, TiposRubricas.classesMap.Pai);

					rubrica.filho = (rubrica.PaiId !== null);

					scope.visivel = true;

					scope.mostrarFilhos = function () {
						ctrls[1].toggleFilhos();
					};

					if (rubrica.pai) {
						elem.addClass("rubricaPai");

						scope.aberto = false;

						ctrls[0].addPai(rubrica.RubricaId, ctrls[1]);
					}

					if (rubrica.filho) {
						elem.addClass("rubricaFilha");

						ctrls[0].addFilho(rubrica.PaiId, ctrls[1]);

						scope.$watch("rubrica.TotalPlanejado", function (valorNovo, valorAntigo) {
							valorNovo = Dinheiro.parse(valorNovo, 0);
							valorAntigo = Dinheiro.parse(valorAntigo, 0);

							ctrls[1].getRubricaPai().TotalPlanejado += valorNovo - valorAntigo;
						});
					}

					scope.$on("$destroy", function () {
						if (rubrica.filho && ctrls[1].pai) {
							ctrls[1].pai.removeFilho(ctrls[1]);
						} else if (rubrica.pai) {
							ctrls[0].removePai(rubrica.RubricaId);
						}
					});
				}
			};
		}])
		.directive("despesas", ["$http", "Meses", "Rubricas", "TiposRubricas", "Dinheiro", function ($http, Meses, Rubricas, TiposRubricas, Dinheiro) {
			return {
				restrict: "A",
				scope: {
					aditivo: "=",
					classe: "=classe",
					aditivoId: "=aditivoId",
					repetirValor: "&"
				},
				templateUrl: "/Angular/despesas",
				link: function (scope, element, attrs) {
					scope.meses = Meses;

					scope.rubricas = Rubricas.listar();

					scope.tipos = TiposRubricas;

					scope.dinheiro = Dinheiro;

					scope.TotalPlanejado = function (rubrica) {
						var total;

						total = 0;

						rubrica.Anos.forEach(function (ano) {
							ano.Meses.forEach(function (mes) {
								total += Dinheiro.parse(mes.Planejado, 0);
							});
						});

						return total;
					};

					scope.TotalGasto = function (rubrica) {
						var total;

						total = 0;

						rubrica.Anos.forEach(function (ano) {
							ano.Meses.forEach(function (mes) {
								total += Dinheiro.parse(mes.Gasto, 0);
							});
						});

						return total;
					};

					scope.Previsao = function (rubrica) {
						var previsao;

						previsao = 0;

						rubrica.Anos.forEach(function (ano) {
							ano.Meses.forEach(function (mes) {
								if (Dinheiro.parse(mes.Gasto) === null) {
									previsao += Dinheiro.parse(mes.Replanejado) === null ? 
                                        Dinheiro.parse(mes.Planejado, 0) : Dinheiro.parse(mes.Replanejado, 0);
								}
							});
						});

						return previsao;
					};
				}
			};
		}])
		.directive("despesa",
            ["$rootScope", "$modal", "$filter", "$q", "Projeto", "Rubrica", "Modal", "notasfiscais.resource",
                function ($rootScope, $modal, $filter, $q, Projeto, Rubrica, Modal, notasFiscaisResource) {
			        return {
				        restrict: "A",
				        templateUrl: "/Angular/despesa",
				        link: function (scope, elem, attrs) {

					        scope.rubrica.aberto = false;
							
							scope.toggleRubrica = function () {
						        scope.rubrica.aberto = !scope.rubrica.aberto;
					        };

					        scope.copiarValor = function (rubricaMes) {
						        rubricaMes.Gasto = rubricaMes.Replanejado;
						        rubricaMes.Replanejado = null;
					        };

					        scope.isRubricaMaoObraDireta = function () {
					            return (scope.rubrica.Tipo == 15 || scope.rubrica.Tipo == 89);
					        };

					        scope.confirmarEdicao = function (rubricaMes) {
					        	var titulo = $filter('translate')('Confirmar Edição');
					        	var mensagem = $filter('translate')('Existem notas fiscais associadas a essa rubrica. Ao adicionar o valor de previsão irá desassociar todas as notas fiscais da rubrica. Deseja realmente fazer isso?');
					        	Modal.confirmar(titulo, mensagem).then(function (resposta) {
					        		if (resposta === "sim") {
					        			var loader;

					        			loader = WEX.feedback.carregando("<span>Desassociando Notas Fiscais...</span>");

					            		notasFiscaisResource.queryAssociadas({
					            			aditivoId: scope.rubrica.AditivoId,
					            			rubricaId: rubricaMes.RubricaId,
					            			ano: rubricaMes.Ano,
					            			mes: rubricaMes.Mes
					            		}, function (notasFiscais) {
					            			var desassociadas = [];

					            			angular.forEach(notasFiscais, function (notaFiscal) {
					            				desassociadas.push(notaFiscal.$desassociar({
					            					aditivoId: scope.rubrica.AditivoId
					            				}));
					            			});

					            			$q.all(desassociadas).then(function () {
					            				loader.carregado();
					            				$rootScope.$broadcast(WEX.Rubrica.Evento.Atualizada, rubricaMes.RubricaId);
					            			});
					            		});
					                }
					            });
					        };

					        scope.$watch(function (scope) {

					            return JSON.stringify(scope.rubrica.Anos);

					        }, function () {

						        if (Rubrica.rubricaVazia(scope.rubrica, ["Planejado", "Replanejado", "Gasto"])) {
							        scope.rubrica.aberto = false;
						        } else {
							        scope.rubrica.aberto = true;
						        }

					        });

					        var showModalNotasFiscais = function (rubricaMes) {
					            $modal.open({
					                templateUrl: "/angular/template/?id=/custos/modalnotasfiscais",
					                controller: "ModalNotasFiscaisCtrl",
					                resolve: {
					                    rubrica: function () {
					                        return scope.rubrica;
					                    },
					                    rubricaMes: function () {
					                        return rubricaMes;
					                    },
					                    notasFiscais: function () {
					                    	return notasFiscaisResource.queryAssociadas({
												aditivoId: scope.rubrica.AditivoId,
					                        	rubricaId: rubricaMes.RubricaId,
					                            ano: rubricaMes.Ano,
					                            mes: rubricaMes.Mes
					                        });
					                    }
					                }
					            });
					        };

					        var showModalCustosMaosDeObra = function (rubricaMes) {

					            $modal.open({
					                templateUrl: "/angular/template/?id=/custos/modalcustosmaoobra",
					                controller: "ModalCustosMaoDeObraCtrl",
					                resolve: {
					                    rubrica: function () {
					                        return scope.rubrica;
					                    },
					                    rubricaMes: function () {
					                        return rubricaMes;
					                    }
					                }
					            }); 

					        };

					        scope.showNotasFiscais = function (rubricaMes) {
					            if (scope.isRubricaMaoObraDireta()) {
					                showModalCustosMaosDeObra(rubricaMes);
					            } else {
					                showModalNotasFiscais(rubricaMes);
					            }
					        };

				        }
			        };
		}])
		.directive("wexValorMutex", ["$parse", "Dinheiro", function ($parse, Dinheiro) {
			return {
				restrict: "A",
				link: function (scope, elem, attrs) {
				    scope.$watch(attrs.ngModel, function (valorNovo, valorAntigo) {
							var setter;

							if (valorNovo != valorAntigo && Dinheiro.parse(valorNovo) !== null) {
								setter = $parse(attrs.wexValorMutex).assign;

								setter(scope, null);
							}
					});
				}
			};
		}])
		.directive("somaTotal", ["Totais", "Dinheiro", "$parse", function (Totais, Dinheiro, $parse) {
			return {
				restrict: "A",
				require: "ngModel",
				link: function (scope, elem, attrs) {
					var valorInicial;

					valorInicial = $parse(attrs.ngModel);

					Totais.atualizaMes(Dinheiro.parse(valorInicial(scope), 0), scope.rubricaMes.Ano, scope.classe, scope.rubricaMes.Mes - 1);

					scope.$watch(attrs.ngModel, function (valorNovo, valorAntigo) {
						Totais.atualizaMes(Dinheiro.parse(valorNovo, 0) - Dinheiro.parse(valorAntigo, 0), scope.rubricaMes.Ano, scope.classe, scope.rubricaMes.Mes - 1);
					});
				}
			};
		}])
		.directive("salvar", ["$rootScope", "Autosave", "Objetos", function ($rootScope, Autosave, Objetos) {
			return {
				link: function (scope, element, attrs) {
					scope.$watch(attrs.ngModel, function (valorNovo, valorAntigo) {
						if (valorNovo !== valorAntigo) {
							switch (attrs.salvar) {
							    case "RubricaMes":
							    	Autosave.enfileirar(scope.rubricaMes, Objetos.RubricaMes);
									break;
								case "Rubrica":
									Autosave.enfileirar(scope.rubrica, Objetos.Rubrica)
										.then(function () {
											$rootScope.$broadcast(WEX.Eventos.Aditivos.Atualizar, scope.rubrica.AditivoId);
										});
									break;
							}
						}
					});
				}
			};
		}])
		.directive("scrollTop", ["$window", function ($window) {
			return {
				restrict: "A",
				scope: {
					scrollTop: '@'
				},
				link: function (scope, elem, attrs) {
					var offsetPai,
						pai,
						janela,
						_oldScroll;

					_oldScroll = null;
					janela = angular.element($window);
					pai = elem.parent();

					offsetPai = elem.offset().top - pai.offset().top;

					janela.bind("scroll", function () {
						var topoJanela,
							topoPai;

						topoJanela = janela.scrollTop();
						topoPai = pai.offset().top;

						if (_oldScroll) {
							elem.removeClass(_oldScroll);
						}

						if (topoPai + offsetPai < topoJanela) {
							elem.addClass(scope.scrollTop);
						} else {
							elem.removeClass(scope.scrollTop);
						}

						_oldScroll = scope.scrollTop;

					});
				}
			};
		}])
		.directive("keyEnter", ["$parse", function ($parse) {
			return {
				restrict: "A",
				link: function (scope, elem, attrs) {
					elem.bind("keypress", function (event) {
						if (event.keyCode === 13) {
							scope.$apply(function () {
								$parse(attrs["keyEnter"])(scope);
							});
						}
					});
				}
			};
		}]);

	WEX.custo.CustosFilters
		.filter("classe", ["TiposRubricas", "hasFlagFilter", function (TiposRubricas, hasFlagFilter) {
			return function (tipos, classe) {
				return tipos.filter(function (tipo) {
					return hasFlagFilter(tipo.Classe, TiposRubricas.classesMap[classe]);
				});
			};
		}])
		.filter("hasFlag", function () {
			return function (flags, flag) {
				return (flags & flag) > 0;
			};
		})
		.filter("rubricasFilhas", function () {
			return function (rubricas, paiId) {
				return rubricas.filter(function (rubrica) {
					return rubrica.PaiId === paiId;
				});
			};
		})
		.filter("subRubricas", function () {
			return function (rubricas, subRubricas) {

				if (angular.isArray(rubricas)) {
					return rubricas.filter(function (rubrica) {
						var isSubRubrica;

						isSubRubrica = (rubrica.PaiId !== null);

						return subRubricas ? isSubRubrica : !isSubRubrica;
					});
				}

				return [];
			};
		});

	WEX.custo.CustosModule
		.factory("CentrosCusto", ["$http", "$filter", function ($http, $filter) {
			var CentrosCusto,
				todosCentrosCusto,
				centrosCusto;

			CentrosCusto = {};
			todosCentrosCusto = [];
			centrosCusto = [];

			CentrosCusto.get = function (aditivoId) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/CentrosCusto",
					method: "get"
				})
					.success(function (data, status, headers, config) {
						centrosCusto.replace(data.centrosCusto);
					})
					.error(function (data, status, header, config) {
						WEX.message.error($filter('translate')('Erro ao recuperar Centros de Custo!'));
					});
				return centrosCusto;
			};

			CentrosCusto.put = function (aditivoId, centroCusto) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/CentrosCusto/" + centroCusto.CentroCustoId,
					method: "post"
				})
					.success(function (data, status, headers, config) {
						centrosCusto.push(centroCusto);
						WEX.feedback.infoGeral("<span>Centro de Custo adicionado com sucesso!</span>");
					});
			};

			CentrosCusto.remove = function (aditivoId, indice) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId + "/CentrosCusto/" + centrosCusto[indice].CentroCustoId,
					method: "delete"
				})
					.success(function (data, status, headers, config) {
						if (data.id !== 0) {
							WEX.custo.aditivo.excluiCentroCusto(centrosCusto, indice);
						}
					});
			};

			CentrosCusto.centrosCusto = function () {
				$http({
					url: "/CentrosCusto",
					method: "get"
				})
					.success(function (data, status, headers, config) {
						todosCentrosCusto.replace(data.centrosCusto);
						todosCentrosCusto.forEach(function (centroCusto) {
							centroCusto.value = centroCusto.Nome;
						});
					});

				return todosCentrosCusto;
			};

			return CentrosCusto;
		}])
		.factory("Aditivos", ["$rootScope", "$http", "$q", "$filter", "PopulaAditivo", "Aditivo", "Modal", function ($rootScope, $http, $q, $filter, populaAditivo, Aditivo, Modal) {
			var Aditivos,
				aditivos;

			Aditivos = {};
			aditivos = [];

			Aditivos.get = function (projetoOid) {
				$http({
					url: "/Custos/" + projetoOid + "/Aditivos",
					method: "get"
				})
					.success(function (data, status, headers, config) {
						aditivos.replace(data.aditivos.map(function (aditivo) {
							return new Aditivo(populaAditivo(aditivo));
						}));
					});

				return aditivos;
			};

			Aditivos.remove = function (indice) {
				var aditivo,
					removeDeferred;

				aditivo = aditivos[indice];
				removeDeferred = $q.defer();

				if (aditivo.AditivoId === 0) {
					aditivos.splice(indice, 1);
					WEX.feedback.infoGeral("<span>Aditivo removido!</span>");
				} else {
					$http({
						url: "/Custos/Aditivo/" + aditivo.AditivoId,
						method: "delete"
					})
						.success(sucesso)
						.error(falha);
				}

				function sucesso(data, status, headers, config) {
					removeDeferred.resolve();

					aditivos.splice(indice, 1);
					WEX.feedback.infoGeral("<span>Aditivo removido!</span>");
				}

				function falha(data, status, headers, config) {
					var titulo,
						mensagem;

					titulo = $filter('translate')('Confirmar Exclusão?');
					mensagem = $filter('translate')('O Aditivo possui valores, tem certeza que deseja excluir?');

					if (status === 405 && typeof config.data === "undefined") {
						Modal.confirmar(titulo, mensagem).then(function (resultado) {
							if (resultado === "sim") {
								config.data = {
									force: true
								};

								$http(config)
									.success(sucesso)
									.error(falha);
							}
						});
					} else {
						removeDeferred.reject();

						WEX.feedback.erroGeral("<span>Erro ao remover Aditivo!</span>");
					}
				}

				return removeDeferred.promise;
			};

			Aditivos.salva = function (aditivo) {
				var config,
					dirty,
					deferido;

				deferido = $q.defer();
				dirty = $("#listaAditivos [data-id=" + aditivo.AditivoId + "] .ng-dirty");

				if (dirty.length > 0) {
					config = {};

					config.url = "/Custos/Aditivo/";
					config.method = "post";
					config.data = new Aditivo.Dto(aditivo);

					if (aditivo.AditivoId > 0) {
					    config.url += aditivo.AditivoId;
					    config.method = "put";
					} else {
					    aditivo.OrcamentoRestante = aditivo.Orcamento;
					}

					$http(config)
						.success(function (data, status, headers, config) {
							var mensagemErro;

							if (typeof data.id === "undefined") {
								mensagemErro = "<span>Aditivo não salvo!</span>";

								data.forEach(function (elem) {
									elem.erros.forEach(function (erro) {
										mensagemErro += "<br>" + erro;
									});
								});

								WEX.feedback.erroGeral(mensagemErro);

								deferido.reject();
							} else {
								aditivo.AditivoId = data.id;
								WEX.feedback.infoGeral("<span>Aditivo salvo com sucesso!</span>");

								$rootScope.$broadcast(WEX.Eventos.Aditivos.Atualizar, aditivo.AditivoId);

								deferido.resolve();
							}
						})
						.error(function (data, status, headers, config) {
							WEX.message.error($filter('translate')('Aditivo não salvo!'));
							deferido.reject();
						});
				} else {
					deferido.resolve();
				}

				return deferido.promise;
			};

			Aditivos.recarregar = function (aditivoId) {
				$http({
					url: "/Custos/Aditivo/" + aditivoId,
					method: "get"
				})
					.success(function (data, status, headers, config) {
						var aditivoAtualizado;

						angular.forEach(aditivos, function (aditivo) {
							if (aditivo.AditivoId === aditivoId) {
								aditivoAtualizado = aditivo;
							}
						});

						angular.extend(aditivoAtualizado, populaAditivo(data.aditivo));
					});
			};

			return Aditivos;
		}])
		.factory("Aditivo", ["$filter", "Dinheiro", "AdicionaErro", function ($filter, Dinheiro, adicionaErro) {
			var Aditivo;

			Aditivo = function Aditivo(obj) {
				obj = obj || {};

				this.AditivoId = obj.AditivoId || 0;
				this.Nome = obj.Nome || "";
				this.DataInicio = obj.DataInicio || null;
				this.DataTermino = obj.DataTermino || null;
				this.Duracao = obj.Duracao || 0;
				this.Anos = obj.Anos || [];
				this.Orcamento = Dinheiro.format(obj.Orcamento || 0);
				this.OrcamentoRestante = Dinheiro.format(obj.OrcamentoRestante || 0);
				this.Projeto = obj.Projeto || "";
				this.editando = obj.editando || false;
				this.selecionado = obj.selecionado || false;
			};

			Aditivo.isValido = function (aditivo) {
				var valido;

				valido = true;
				delete aditivo.erros;

				if (aditivo.Nome.trim() === "") {
					adicionaErro(aditivo, "Nome", $filter('translate')('Nome inválido!'));
					valido = false;
				}

				if (!(aditivo.DataInicio instanceof Date)) {
					adicionaErro(aditivo, "DataInicio", $filter('translate')('Data inválida!'));
					valido = false;
				}

				if (!(aditivo.DataTermino instanceof Date)) {
					adicionaErro(aditivo, "DataTermino", $filter('translate')('Data inválida!'));
					valido = false;
				}

				if (aditivo.DataInicio >= aditivo.DataTermino) {
					adicionaErro(aditivo, "DataTermino", $filter('translate')('Período inválido!'));
					valido = false;
				}

				return valido;
			};

			Aditivo.Dto = function AditivoDto(aditivo) {
				if (typeof aditivo !== "object" || !(aditivo instanceof Aditivo)) {
					throw new Error($filter('translate')('Esperado instância de Aditivo, passado ') + rubrica.constructor.name);
				}

				this.AditivoId = aditivo.AditivoId;
				this.Nome = aditivo.Nome;
				this.DataInicio = aditivo.DataInicio;
				this.DataTermino = aditivo.DataTermino;
				this.Duracao = aditivo.Duracao;
				this.Orcamento = Dinheiro.parse(aditivo.Orcamento, 0);
				this.OrcamentoRestante = Dinheiro.parse(aditivo.OrcamentoRestante, 0);
				this.Projeto = aditivo.Projeto;
			};

			Aditivo.pertenceAditivo = function (aditivo, ano, mes) {
				var mesInicial,
					mesFinal,
					mesAtual;

				mesInicial = (aditivo.DataInicio.getUTCFullYear() - 1) * 12 + aditivo.DataInicio.getUTCMonth() + 1;
				mesFinal = mesInicial + aditivo.Duracao - 1;
				mesAtual = (ano - 1) * 12 + mes;

				if (mesAtual >= mesInicial && mesAtual <= mesFinal) {
					return true;
				}

				return false;
			}

			return Aditivo;
		}])
		.factory("Rubrica", ["$filter", "Aditivo", "Dinheiro", function ($filter, Aditivo, Dinheiro) {
			var Rubrica;

			/**
			 * Funções para Rúbricas.
			 *
			 * @class Rubrica
			 * @static
			**/
			Rubrica = {};

			/**
			 * Itera por todos os meses de todos os anos de uma Rúbrica,
			 * chamando um callback para cada mês. Caso o callback retorne
			 * `true` a iteração termina imediatamente.
			 *
			 * @method forMesesRubrica
			 * @static
			 * @param {Object} rubrica Rubrica a ser iterada.
			 * @param {Function} callback Callback a ser chamado para cada mês.
			 * @return {Boolean} Retorna `true` caso tenha terminado mais cedo,
			 *   `false`caso contrário.
			 */
			Rubrica.forMesesRubrica = function (rubrica, callback) {
				 return rubrica.Anos.some(function (ano) {
					return ano.Meses.some(function (mes) {
						return callback(mes, ano);
					});
				});
			};

			/**
			 * Testa se Rúbrica é vazia, ou seja, não possui valores diferentes de zero.
			 *
			 * @method rubricaVazia
			 * @static
			 * @param {Object} rubrica Rubrica a ser testada.
			 * @param {String|Array} propriedades Propriedade(s) a ser(em) testada(s).
			 * @return {Boolean} Retorna `true` caso seja vazia, `false`caso contrário.
			 */
			Rubrica.rubricaVazia = function (rubrica, propriedades) {
				var compFn,
					todos;

				compFn = (function () {
					if (angular.isArray(propriedades)) {
						return function (rubricaMes) {
							var indice;

							for (indice = 0; indice < propriedades.length; indice += 1) {
								if (Dinheiro.parse(rubricaMes[propriedades[indice]], 0) !== 0) {
									return true;
								}
							}
						}
					} else if (typeof propriedades === "string") {
						return function (rubricaMes) {
							if (Dinheiro.parse(rubricaMes[propriedades], 0) !== 0) {
								return true;
							}
						}
					} else {
						throw new Error($filter('translate')('Tipo inesperado do argumento "propriedades"'));
					}
				}());

				todos = Rubrica.forMesesRubrica(rubrica, compFn);

				return !todos;
			};

			return Rubrica;
		}])
		.factory("Rubricas", ["$http", "$q", "$filter", "TiposRubricas", function ($http, $q, $filter, TiposRubricas) {
			var Rubricas,
				rubricas,
				valorPresente;

			Rubricas = {};
			rubricas = {};
			valorPresente = null;

			Rubricas.listar = function () {
				return rubricas;
			};

			Rubricas.carregar = function (aditivoId) {

				valorPresente = [];

				TiposRubricas.classes.forEach(function (classe) {
					var classePromise;

					classePromise = $q.defer();

					valorPresente.push(classePromise.promise);

					$http({
						url: "/Custos/Aditivo/" + aditivoId + "/Rubricas",
						method: "get",
						params: { classe: classe },
					})
						.success(function (data, status, headers, config) {
							rubricas[classe] = data.rubricas;
							classePromise.resolve();
						})
						.error(function (data, status, headers, config) {
							classePromise.reject();
							WEX.message.error($filter('translate')('Erro ao recuperar Rubricas!'));
						});
				});

				valorPresente = $q.all(valorPresente);

				return valorPresente;
			};

			Rubricas.recarregarRubrica = function (rubricaId) {
				var rubrica;

				rubrica = rubricas["Desenvolvimento"].find(rubricaId)[0];

				if (!rubrica) {
				    console.log($filter('translate')('Rubrica não encontrada.'), rubricaId);
				    return;
				}

				$http({
					url: "/Custos/Aditivo/" + rubrica.AditivoId + "/Rubricas/" + rubrica.RubricaId,
					method: "get"
				})
				    .success(function (data, status, headers, config) {
				        // não consegui identificar o motivo para a rubrica "sumir" após a atualização, entao,
				        // resolvi atualizar apenas as informações referentes aos Anos (custos) e o TotalPlanejado
				        rubrica.Anos = data.rubrica.Anos;
				        rubrica.TotalPlanejado = data.rubrica.TotalPlanejado;

				        WEX.feedback.infoGeral("<span>Rubrica recarregada!</span>");
				    })
				    .error(function (data, status, headers, config) {
				    	WEX.message.error($filter('translate')('Erro ao recuperar Rubrica.'));
				    });
			};

			return Rubricas;
		}])
		.factory("Objetos", function () {
			var Objetos;

			Objetos = {
				Rubrica: {
					info: "Rubrica",
					id: "RubricaId",
					action: "Rubricas",
					dto: "RubricaDto"
				},
				RubricaMes: {
					info: "Detalhes de Rubrica",
					id: "RubricaMesId",
					action: "RubricasMeses",
					dto: "RubricaMesDto"
				}
			};

			return Objetos;
		})
		.factory("Totais", ["TiposRubricas", function (TiposRubricas) {
			var Totais;

			Totais = {};

			Totais.atualizaMes = function (diffMes, ano, classe, mes) {
				var saida;

				saida = true;

				if (classe === "Aportes") {
					saida = false;
				}

				Totais[ano][classe].meses[mes] += diffMes;
				Totais[ano][classe].Total += diffMes;
				Totais[ano].totalGeral[mes] += saida ? -diffMes : diffMes;
				Totais[ano].Total += saida ? -diffMes : diffMes;
				Totais.Total += saida ? -diffMes : diffMes;
			};

			Totais.atualizaAnos = function (anos) {
				var prop,
					mes;

				anos.forEach(function (ano) {
					if (!(ano in Totais)) {
						Totais[ano] = {};
					}

					Totais[ano].Total = 0;

					TiposRubricas.classes.forEach(function (classe) {
						var totaisMeses;

						totaisMeses = Totais[ano][classe] || {};

						totaisMeses.meses = [];
						totaisMeses.Total = 0;

						for (mes = 0; mes < 12; mes++) {
							totaisMeses.meses.push(0);
						}

						Totais[ano][classe] = totaisMeses;
					});

					Totais[ano].totalGeral = [];

					for (mes = 0; mes < 12; mes++) {
						Totais[ano].totalGeral.push(0);
					}
				});

				for (prop in Totais) {
					if (Totais.hasOwnProperty(prop)) {
						prop = parseInt(prop, 10);

						if (!isNaN(prop) && anos.indexOf(prop) === -1) {
							delete Totais[prop];
						}
					}
				}

				Totais.Total = 0;
			};

			return Totais;
		}])
		.factory("CopiaArrayAsync", ["$timeout", "$q", function ($timeout, $q) {
			var maxCopias;

			maxCopias = 3;

			return function (orig, dest) {
				var tamanho,
					indice,
					finalizado;

				indice = 0;
				tamanho = orig.length;
				finalizado = $q.defer();

				dest.length = 0;

				copia();

				return finalizado.promise;

				function copia() {
					var count;

					count = 0;

					while (indice < tamanho) {
						dest[indice] = orig[indice];

						indice += 1;
						count += 1;

						if (count === maxCopias) {
							$timeout(copia, 800);

							finalizado.notify(indice);

							return;
						}
					}

					finalizado.resolve();
				}
			};
		}])
		.factory("PopulaRubricas", ["$rootScope", "$filter", "Rubricas", "CopiaArrayAsync", function ($rootScope, $filter, Rubricas, CopiaArrayAsync) {
			var worker,
				rubricas,
				PopulaRubricas;

			PopulaRubricas = {};
			rubricas = Rubricas.listar();
			worker = new Worker("/Scripts/wex/custos/populaRubrica.js");

			worker.addEventListener("message", function (event) {
				$rootScope.$apply(function () {
					var carregando;

					if (typeof event.data.indice === "number") {
						rubricas[event.data.classe][event.data.indice] = new WEX.custo.aditivo.Rubrica(event.data.rubricas[0]);
					} else {
						carregando = WEX.message.loading($filter('translate')('As rubricas estão sendo carregadas...'), {
							modal: true
						});

						CopiaArrayAsync(event.data.rubricas, rubricas[event.data.classe])
							.then(
							function sucesso() {
								carregando.close();
							},
							function falha() {
								carregando.close();
								WEX.message.error($filter('translate')('Entre em contato com o administrador'), {
									title: $filter('translate')('Erro ao carregar as rubricas')
										});
							}								
						);
					}
				});
			});

			PopulaRubricas.atualiza = function (aditivo, classe, rubricas) {
				worker.postMessage({ aditivo: aditivo, classe: classe, rubricas: rubricas, atualizar: true });

				rubricas[classe] = [];
			};

			PopulaRubricas.adiciona = function (aditivo, classe, rubrica, indice) {
				worker.postMessage({ aditivo: aditivo, classe: classe, rubricas: [rubrica], indice: indice });
			};

			return PopulaRubricas;
		}])
		.factory("TiposRubricas", ["$http", "$rootScope", function ($http, $rootScope) {
			var TiposRubricas;

			TiposRubricas = {};

			$rootScope.$on("recuperadoProjeto", function (event, projeto) {
				$http({
					url: "/Custos/Rubricas/Tipos",
					method: "get",
					params: {
						tipoProjetoId: projeto.TipoProjetoId
					}
				})
					.success(function (data, status, headers, config) {
						Object.replace(TiposRubricas, data);
					});
			});

			return TiposRubricas;
		}])
		.factory("Autosave", ["$http", "$q", "$filter", "$timeout", "Aditivo", function ($http, $q, $filter, $timeout, Aditivo) {
			var Autosave,
				enviar,
				enfileirar,
				callback;

			Autosave = {};

			enviar = function (dado, opcoes, callback) {
				var config;

				config = {};

				config.method = "POST";
				config.url = "/Custos/Aditivo/" + Aditivo.AditivoId + "/" + opcoes.action;

				if (dado[opcoes.id] !== 0) {
					config.method = "PUT";
					config.url += "/" + dado[opcoes.id];
				}

				return function () {
					config.data = new WEX.custo.aditivo[opcoes.dto](dado);

					return $http(config)
						.success(function (data, status, headers, config) {
							dado[opcoes.id] = data.id;
							WEX.feedback.infoGeral("<span>" + opcoes.info + " salvo com sucesso!</span>");

							callback(data);
						})
						.error(function (data, status, headers, config) {
							WEX.message.error($filter('translate')('Erro ao salvar') + " " + opcoes.info + "!");
						});
				};
			};

			enfileirar = function (objeto, opcoes) {
				var metaObjeto,
					existente,
					deferido;

				existente = Autosave.dados.find(objeto);

				if (existente.length === 0) {
					metaObjeto = {
						dado: objeto
					};
					Autosave.dados.push(metaObjeto);
				} else {
					metaObjeto = existente[0];
				}

				deferido = $q.defer();

				$timeout.cancel(metaObjeto.promessa);
				metaObjeto.promessa = $timeout(enviar(metaObjeto.dado, opcoes, callback(metaObjeto)), WEX.custo.timeoutSalvar, false);

				metaObjeto.promessa.then(
					function timeoutResolved() {
						deferido.resolve();
					},
					function timeoutCancel() {
						deferido.reject();
					}
				);

				return deferido.promise;
			};

			callback = function (metaObjeto) {
				return function () {
					var indice;

					indice = Autosave.dados.indexOf(metaObjeto);

					if (indice !== -1) {
						Autosave.dados.splice(indice, 1);
					}
				};
			};

			Autosave.dados = [];
			Autosave.enfileirar = enfileirar;
			Autosave.enviar = enviar;

			return Autosave;
		}])
		.factory("PopulaAnos", function () {
			return function (aditivo) {
				var ano,
					mesFinal,
					anoFinal;

				aditivo.Anos = [];

				mesFinal = (aditivo.DataInicio.getUTCFullYear() - 1) * 12 + aditivo.DataInicio.getUTCMonth() + aditivo.Duracao;
				anoFinal = Math.max(aditivo.DataTermino.getUTCFullYear(), Math.ceil(mesFinal / 12));

				for (ano = aditivo.DataInicio.getUTCFullYear() ; ano <= anoFinal; ano++) {
					aditivo.Anos.push(ano);
				}
			};
		})
		.factory("PopulaAditivo", ["PopulaAnos", "Data", function (populaAnos, Data) {
			return function (aditivo) {
				aditivo.DataInicio = Data.fromJSONDate(aditivo.DataInicio);
				aditivo.DataTermino = Data.fromJSONDate(aditivo.DataTermino);

				populaAnos(aditivo);

				return aditivo;
			};
		}]);

	var ModalNotasFiscaisCtrl = ["$rootScope", "$scope", "$filter", "rubrica", "rubricaMes", "notasFiscais",
        function ($rootScope, $scope, $filter, rubrica, rubricaMes, notasFiscais) {

	    $scope.rubrica = rubrica;
	    $scope.data = new Date(rubricaMes.Ano, rubricaMes.Mes - 1);
	    $scope.notasFiscais = notasFiscais;
	    
        // Função para retornar o valor total das notasFiscais da rubrica no modal
	    $scope.calcularTotalNotasFiscais = function () {
	        var totalNotasFiscais = 0;
	        notasFiscais.forEach(function (notaFiscal) {
	            totalNotasFiscais += notaFiscal.Valor;
	        });
	        return totalNotasFiscais;
	    };

		$scope.desassociar = function (notaFiscal) {

			notaFiscal.$desassociar({ aditivoId: rubrica.AditivoId },
				function success() {

				    var messageSuccess = WEX.message.success($filter("translate")("Nota fiscal desassociada com sucesso"));

					notaFiscal.RubricaId = null;

	                setTimeout(messageSuccess.close, 3000);

	                $rootScope.$broadcast(WEX.Rubrica.Evento.Atualizada, rubrica.RubricaId);

	                $scope.notasFiscais = $filter("filter")(notasFiscais, { RubricaId: rubrica.RubricaId });

	            }, function error () {

	                notaFiscal.RubricaId = rubrica.RubricaId;
	                WEX.message.error($filter("translate")("Problemas ao desassociar a nota fiscal"));

	            });

	        };

	    }];

	var ModalCustosMaoDeObraCtrl = ["$rootScope", "$scope", "$filter", "Projeto", "Aditivo", "rubrica", "rubricaMes", "maodeobra.resource",
        function ($rootScope, $scope, $filter, Projeto, Aditivo, rubrica, rubricaMes, maoDeObraResource) {

            var loadingCustos,
                loadingLote,
                loadingImportacao,
                requestCustos,
                requestLote,
                requestVerificarAtualizacoesLote,
                loteRequerAtualizacao;

            loadingCustos = false;
            loadingLote = false;
            loadingImportacao = false;
            loteRequerAtualizacao = null;

            $scope.data = new Date(rubricaMes.Ano, rubricaMes.Mes - 1);
	        $scope.rubrica = rubrica;
	        $scope.custos = [];
	        $scope.lote = null;
	        $scope.somaTotal = null;
	        $scope.quantidadeColaboradores = null;
	        

	        $scope.requerAtualizacao = function () {
	            return loteRequerAtualizacao === true;
	        }

	        $scope.isLoaded = function () {
	            return !(loadingCustos || loadingLote || loadingImportacao);
	        };

	        $scope.hasLote = function () {
	            return $scope.isLoaded() && $scope.lote != null;
	        }

	        $scope.hasCustos = function () {
	            return $scope.isLoaded() && $scope.custos.length > 0;
	        }

	        requestCustos = function () {

	            loadingCustos = true;


	            maoDeObraResource.query({

	                centroCustoId: Projeto.CentroCustoId,
	                aditivoId: Aditivo.AditivoId,
	                ano: rubricaMes.Ano,
	                mes: rubricaMes.Mes

	            }, function (response) {

	                loadingCustos = false;

	                $scope.custos = response.maosDeObra;
                    
	                $scope.somaTotal = response.somaValorTotal;

	                $scope.quantidadeColaboradores = response.quantidadeColaboradores;
	                

	            }, function () {

	                loadingCustos = false;

	            });

	        };

	        requestLote = function () {

	            loadingLote = true;

	            maoDeObraResource.query({

	                centroCustoId: Projeto.CentroCustoId,
	                aditivoId: Aditivo.AditivoId,
	                ano: rubricaMes.Ano,
	                mes: rubricaMes.Mes

	            }, function (response) {

	                loadingLote = false;

	                $scope.lote = response.lote;

	            }, function () {

	                loadingLote = false;

	            });

	        };

	        requestVerificarAtualizacoesLote = function () {

	            maoDeObraResource.verificarAtualizacao({

	                centroCustoId: Projeto.CentroCustoId,
	                ano: rubricaMes.Ano,
	                mes: rubricaMes.Mes

	            }, function (response) {

	                loteRequerAtualizacao = response.requerAtualizacao;

	            });

	        };

	        requestCustos();

	        requestLote();

	        requestVerificarAtualizacoesLote();

	        $scope.importar = function () {

	            loadingImportacao = true;

	            maoDeObraResource.importar({

	                centroCustoId: Projeto.CentroCustoId,
	                aditivoId: Aditivo.AditivoId,
	                ano: rubricaMes.Ano,
	                mes: rubricaMes.Mes

	            }, function (json) {

	                loadingImportacao = false;

	                requestCustos();

	                requestLote();

	                loteRequerAtualizacao = false;

	                var messageSuccess = WEX.message.success($filter('translate')('Importação concluida'));
	                setTimeout(messageSuccess.close, 3000);

	            }, function () {

	                loadingImportacao = false;

	                WEX.message.error($filter('translate')('Problemas ao importar os dados.'));

	            });

	        };

	        $scope.aplicarSomatorio = function () {

	            maoDeObraResource.aplicarSomatorioCustos({

	                centroCustoId: Projeto.CentroCustoId,
	                aditivoId: Aditivo.AditivoId,
	                ano: rubricaMes.Ano,
	                mes: rubricaMes.Mes

	            }, function () {

	                $scope.$close();

	                $rootScope.$broadcast(WEX.Rubrica.Evento.Atualizada, rubrica.RubricaId);

	                var messageSuccess = WEX.message.success($filter('translate')('Custos aplicados com sucesso'));
	                setTimeout(messageSuccess.close, 3000);

	            }, function () {

	                WEX.message.error($filter('translate')('Problemas ao aplicar o somatorio de custos'));

	            });

	        }

	    }];

	WEX.custo.CustosModule
		.controller("ProjetoCtrl", WEX.custo.ProjetoCtrl)
		.controller("AditivoCtrl", WEX.custo.AditivoCtrl)
		.controller("DespesasCtrl", WEX.custo.DespesasCtrl)
		.controller("OrcamentoCtrl", WEX.custo.OrcamentoCtrl)
		.controller("CentrosCustoCtrl", WEX.custo.CentrosCustoCtrl)
		.controller("PatrocinadoresCtrl", WEX.custo.PatrocinadoresCtrl)
		.controller("TotalizacaoCtrl", WEX.custo.TotalizacaoCtrl)
	    .controller("ModalNotasFiscaisCtrl", ModalNotasFiscaisCtrl)
	    .controller("ModalCustosMaoDeObraCtrl", ModalCustosMaoDeObraCtrl);

	WEX.custo.CustosModule.config(["$locationProvider", "$modalProvider", function ($locationProvider, $modalProvider) {
		$locationProvider.html5Mode(true);

		$modalProvider.options = {
			keyboard: false,
			backdrop: "static"
		};
	}]);
}());
