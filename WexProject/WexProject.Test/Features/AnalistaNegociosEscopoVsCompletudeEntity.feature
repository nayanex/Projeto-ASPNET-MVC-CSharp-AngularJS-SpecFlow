#language: pt-br

@pbi_2.15, @Entity
Funcionalidade: Analista de Negócios poder visualizar o gráfico de escopo x completude Entity

@rn
Cenário: 01 - Calculo dos dados para o grafico escopo vs completude quando houver estorias vinculadas a submodulos
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome         | tamanho | modulo pai |
		  | modulo 01    | 20      |            |
		  | modulo 01.01 | 15      | modulo 01  |
		  | modulo 01.02 | 5       | modulo 01  |
		  | modulo 02    | 25      |            |
		  | modulo 02.01 | 25      | modulo 02  |
		  | modulo 03    | 3       |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo          | modulo       | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01.01 | modulo 01.01 | 5       | nao        |
		  | Desenvolvedor | Estoria01.01.02 | modulo 01.01 | 3       | sim        |
		  | Desenvolvedor | Estoria01.02.01 | modulo 01.02 | 5       | sim        |
		  | Desenvolvedor | Estoria02.01.01 | modulo 02.01 | 3       | sim        |
		  | Desenvolvedor | Estoria03.01    | modulo 03    | 2       | sim        |
		  | Desenvolvedor | Estoria03.02    | modulo 03    | 1       | sim        |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias:
          | titulo          | situacao |
          | Estoria01.02.01 | EmDesenv |
          | Estoria03.01    | EmDesenv |
          | Estoria03.02    | Pronto   |
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados	| pts analise	| % analise | pts desenvolvimento	| % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
          | modulo 01 | 12				  | 60				| 3				| 15		| 5						| 25				| 0				| 0			| 0				| 0			| 0	          | 0         | 20               |
          | modulo 02 | 22				  | 88				| 3				| 12		| 0						| 0					| 0				| 0			| 0				| 0			| 0	          | 0         | 25               |
          | modulo 03 |  0				  |  0				| 0 			|  0		| 2						| 66,67				| 1				| 33,33		| 0				| 0			| 0	          | 0         |  3               |

@rn
Cenário: 02 - Calculo dos dados para o gráfico escopo vs completude pontos nao inciados
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
          | nome      | tamanho | modulo pai |
          | modulo 01 | 10      |            |
		E que existam as seguintes estorias para o 'projeto 01':
          | como um       | titulo       | modulo    | tamanho | em analise |
          | Desenvolvedor | Estoria01.01 | modulo 01 | 2       | nao         |
          | Desenvolvedor | Estoria01.02 | modulo 01 | 8       | nao         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias:
          | titulo      | situacao    | 
          | Estoria01.01 | Pronto      | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados | pts analise	| % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio | % desvio	| pts mudança	| % mudança | total pts modulo	|
          | modulo 01 | 8				  | 80			    | 0			    | 0		    | 0					  | 0				  | 2			| 20		| 0			 | 0		| 0				| 0         | 10				|
	

@rn
Cenário: 03 - Calculo dos dados para o gráfico escopo vs completude quando nao tiver uma estoria em analise
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
          | nome      | tamanho | modulo pai |
          | modulo 01 | 20      |            |
		E que existam as seguintes estorias para o 'projeto 01':
          | como um       | titulo       | modulo    | tamanho | em analise |
          | Desenvolvedor | Estoria01.01 | modulo 01 | 8       | nao         |
          | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | nao         |
		  | Desenvolvedor | Estoria01.03 | modulo 01 | 8       | sim         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias:
          | titulo      | situacao    | 
          | Estoria01.01 | Pronto      | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados | pts analise	| % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio | % desvio	| pts mudança	| % mudança | total pts modulo	|
          | modulo 01 | 4				  | 20			    | 8			    | 40		| 0					  | 0				  | 8			| 40		| 0			 | 0		| 0				| 0         | 20				|
	
@rn
Cenário: 04 - Calculo dos dados para o gráfico escopo vs completude quando nao tiver estorias em analise
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
          | nome      | tamanho | modulo pai |
          | modulo 01 | 10      |            |
          | modulo 02 | 15      |            |
          | modulo 03 | 25      |            |
		E que existam as seguintes estorias para o 'projeto 01':
          | como um       | titulo       | modulo    | tamanho | em analise |
          | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao         |
          | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | nao         |
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados | pts analise	| % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio | % desvio	| pts mudança	| % mudança | total pts modulo	|
          | modulo 01 | 10				  | 100			    | 0			    | 0			| 0					  | 0				  | 0			| 0			| 0			 | 0		| 0				| 0         | 10				|
          | modulo 02 | 15				  | 100			    | 0			    | 0			| 0					  | 0				  | 0			| 0			| 0			 | 0		| 0				| 0         | 15				|
          | modulo 03 | 25				  | 100			    | 0			    | 0			| 0					  | 0				  | 0			| 0			| 0			 | 0		| 0				| 0         | 25				|

@rn
Cenário: 05 - Calculo dos dados para o grafico escopo vs completude quando houver estorias em analise
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		  | modulo 02 | 15      |            |
		  | modulo 03 | 25      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao         |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         |
		  | Desenvolvedor | Estoria03.01 | modulo 03 | 5       | sim         |
		  | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | sim         |
		  | Desenvolvedor | Estoria03.03 | modulo 03 | 2       | sim         |
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados	| pts analise	| % analise | pts desenvolvimento	| % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
          | modulo 01 | 7				  | 70				| 3				| 30		| 0						| 0					| 0				| 0			| 0				| 0			| 0	          | 0         | 10               |
          | modulo 02 | 15				  | 100				| 0				| 0			| 0						| 0					| 0				| 0			| 0				| 0			| 0	          | 0         | 15               |
          | modulo 03 | 15				  | 60				| 10			| 40		| 0						| 0					| 0				| 0			| 0				| 0			| 0	          | 0         | 25               |
		  
@rn		  
Cenário: 06 - Calculo dos dados para o grafico escopo vs completude quando houver estorias em desenvolvimento
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		  | modulo 02 | 15      |            |
		  | modulo 03 | 25      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao         |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         |
		  | Desenvolvedor | Estoria02.01 | modulo 02 | 13      | nao         |
		  | Desenvolvedor | Estoria03.01 | modulo 03 | 5       | sim         |
		  | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | sim         |
		  | Desenvolvedor | Estoria03.03 | modulo 03 | 2       | sim         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias:
          | titulo      | situacao    | 
          | Estoria01.01 | EmDesenv    | 
		  | Estoria01.02 | NaoIniciado |
          | Estoria03.01 | EmDesenv    | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados	| pts analise	| % analise | pts desenvolvimento	| % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
          | modulo 01 | 2				  | 20				| 3				| 30		| 5						| 50	     		| 0				| 0			| 0				| 0			| 0           | 0         | 10				 |
          | modulo 02 | 15				  | 100				| 0				| 0			| 0						| 0					| 0				| 0			| 0				| 0			| 0           | 0         | 15				 |
          | modulo 03 | 15				  | 60				| 5			    | 20		| 5						| 20				| 0				| 0			| 0				| 0			| 0           | 0         | 25				 |

@rn
Cenário: 07 - Calculo dos dados para o grafico escopo vs completude quando houver estorias prontas
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		  | modulo 02 | 15      |            |
		  | modulo 03 | 25      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao        |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim        |
		  | Desenvolvedor | Estoria02.01 | modulo 02 | 13      | nao        |
		  | Desenvolvedor | Estoria03.01 | modulo 03 | 5       | sim        |
		  | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | sim        |
		  | Desenvolvedor | Estoria03.03 | modulo 03 | 2       | sim        |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias:
          | titulo    	 | situacao | 
          | Estoria01.01 | EmDesenv | 
          | Estoria03.01 | Pronto   | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados	| pts analise	| % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudanca | total pts modulo |
          | modulo 01 | 2				  | 20				| 3				| 30		| 5					  | 50	     		  | 0			| 0			| 0				| 0			| 0           | 0         | 10			     |
          | modulo 02 | 15				  | 100				| 0				| 0			| 0					  | 0				  | 0			| 0			| 0				| 0			| 0           | 0         | 15			     |
          | modulo 03 | 15				  | 60				| 5				| 20		| 0					  | 0				  | 5			| 20		| 0				| 0			| 0           | 0         | 25			     |

@rn
Cenário: 08 - Calculo dos dados para o grafico escopo vs completude quando houver estorias com solicitacoes de mudanca
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		  | modulo 02 | 15      |            |
		  | modulo 03 | 25      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao         |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         |
		  | Desenvolvedor | Estoria01.03 | modulo 01 | 13      | nao         |
		  | Desenvolvedor | Estoria02.01 | modulo 02 | 13      | sim         |
		  | Desenvolvedor | Estoria03.01 | modulo 03 | 5       | sim         | 
		  | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | sim         |
		  | Desenvolvedor | Estoria03.03 | modulo 03 | 2       | sim         |
		  | Desenvolvedor | Estoria03.04 | modulo 03 | 8       | nao         |
		  | Desenvolvedor | Estoria03.05 | modulo 03 | 8       | nao         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
          | titulo    	 | situacao | 
          | Estoria01.01 | EmDesenv | 
          | Estoria03.01 | Pronto   | 
		  | Estoria03.03 | EmDesenv |
		  | Estoria03.04 | Pronto   |
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados   | % nao iniciados	| pts analise | % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
          | modulo 01 | 2					| 20				| 3			  | 30		  | 5					| 50	     		| 0				| 0			| 0				| 0			| 11		  | 110		  | 10     		     |
          | modulo 02 | 2					| 13,33				| 13	      | 86,67     | 0					| 0				    | 0				| 0			| 0				| 0			| 0			  | 0		  | 15	    	     |
          | modulo 03 | 7					| 28				| 3			  | 12		  | 2					| 8				    | 13			| 52		| 0				| 0			| 1			  | 4		  | 25			     |

@rn
Cenário: 09 - Calculo dos dados para o grafico escopo vs completude quando houver estorias em desenvolvimento que, apesar de serem desvio, nao entram no calculo de desvio
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | sim         |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         |
		  | Desenvolvedor | Estoria01.03 | modulo 01 | 5       | sim         |
		  | Desenvolvedor | Estoria01.04 | modulo 01 | 3       | sim         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
          | titulo    	 | situacao | 
          | Estoria01.01 | EmDesenv | 
          | Estoria01.02 | Pronto   | 
          | Estoria01.03 | Pronto   | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados | pts analise | % analise | pts desenvolvimento | % desenvolvimento | pts prontos | % prontos | pts desvio | % desvio | pts mudança | % mudança | total pts modulo |
          | modulo 01 | 0                 | 0               | 0           | 0         | 5                   | 50                | 8           | 80		  | 0		   | 0        | 3           | 30        | 10               |

@rn
Cenário: 10 - Calculo do grafico escopo vs completude quando houver estorias prontas acima da capacidade do modulo, causando desvio
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		E que existam as seguintes estorias para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | sim         |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         |
		  | Desenvolvedor | Estoria01.03 | modulo 01 | 5       | sim         |
		  | Desenvolvedor | Estoria01.04 | modulo 01 | 3       | sim         |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
          | titulo    	 | situacao | 
          | Estoria01.01 | Pronto   | 
          | Estoria01.02 | Pronto   | 
          | Estoria01.03 | Pronto   | 
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados | pts analise | % analise | pts desenvolvimento | % desenvolvimento | pts prontos | % prontos | pts desvio | % desvio | pts mudança | % mudança | total pts modulo |
          | modulo 01 | 0                 | 0               | 0           |  0        | 0                   | 0                 | 10          | 100		  | 3		   | 30       | 3           | 30        | 10               |

@bug
Cenário: 11 - Projeto sem ciclos
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 0               | 10            |
	Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados | % nao iniciados	| pts analise	| % analise | pts desenvolvimento	| % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
@bug
Cenário: 12 - Calculo do grafico escopo vs completude.
		 Dado que exista(m) o(s) projeto(s) a seguir:
			  | nome       | tamanho | total de ciclos | ritmo do time |
			  | projeto 01 | 50      | 3               | 10            |
			E que existam os seguintes modulos para o 'projeto 01':
			  | nome         | tamanho | modulo pai |
			  | modulo 01    | 13      |            |
			  | modulo 01.01 |  5      | modulo 01  |
			  | modulo 01.02 |  8      | modulo 01  |
			E que existam as seguintes estorias para o 'projeto 01':
			  | como um       | titulo          | modulo       | tamanho | em analise |
			  | Desenvolvedor | Estoria01.01.01 | modulo 01.01 | 5       | nao         |
			  | Desenvolvedor | Estoria01.02.01 | modulo 01.02 | 3       | nao         |
			  | Desenvolvedor | Estoria01.02.02 | modulo 01.02 | 3       | nao         |
			  | Desenvolvedor | Estoria01.02.03 | modulo 01.02 | 3       | nao         |
			  | Desenvolvedor | Estoria01.02.04 | modulo 01.02 | 3       | nao         |
			E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
			  | titulo    	    | situacao    | 
			  | Estoria01.02.01 | Pronto      | 
			  | Estoria01.02.02 | Replanejado | 
			  E que o ciclo '2' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
			  | titulo    	    | situacao | 
			  | Estoria01.01.01 | EmDesenv | 
			  | Estoria01.02.04 | Pronto   | 
			  E que o ciclo '3' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
			  | titulo    	    | situacao    | 
			  | Estoria01.02.03 | NaoIniciado | 
	   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
		Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
			  | modulo	  | pts nao iniciados | % nao iniciados | pts analise | % analise | pts desenvolvimento | % desenvolvimento | pts prontos | % prontos | pts desvio | % desvio | pts mudança | % mudança | total pts modulo |
			  | modulo 01 | 2                 | 15,38           | 0           |  0        | 5                   | 38,46             | 6           | 46,15     | 0		   | 0        | 4           | 30,77     | 13               |

@rn
Cenário: 13 - Calculo dos dados para o grafico escopo vs completude quando houver estorias com filhos.
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto 01':
		  | nome      | tamanho | modulo pai |
		  | modulo 01 | 10      |            |
		  | modulo 02 | 15      |            |
		  | modulo 03 | 25      |            |
		E que existam as seguintes estorias pai para o 'projeto 01':
		  | como um       | titulo       | modulo    | tamanho | em analise | estoriaPai |
		  | Desenvolvedor | Estoria01    | modulo 01 | 21      | nao         |            |
		  | Desenvolvedor | Estoria02    | modulo 02 | 13      | nao         |            |
		  | Desenvolvedor | Estoria03    | modulo 03 | 26      | nao         |            |
		  | Desenvolvedor | Estoria01.01 | modulo 01 | 5       | nao         | Estoria01  |
		  | Desenvolvedor | Estoria01.02 | modulo 01 | 3       | sim         | Estoria01  |
		  | Desenvolvedor | Estoria01.03 | modulo 01 | 13      | nao         | Estoria01  |
		  | Desenvolvedor | Estoria02.01 | modulo 02 | 13      | sim         | Estoria02  |
		  | Desenvolvedor | Estoria03.01 | modulo 03 | 5       | sim         | Estoria03  |
		  | Desenvolvedor | Estoria03.02 | modulo 03 | 3       | sim         | Estoria03  |
		  | Desenvolvedor | Estoria03.03 | modulo 03 | 2       | sim         | Estoria03  |
		  | Desenvolvedor | Estoria03.04 | modulo 03 | 8       | nao         | Estoria03  |
		  | Desenvolvedor | Estoria03.05 | modulo 03 | 8       | nao         | Estoria03  |
		E que o ciclo '1' do projeto 'projeto 01' esteja com situacao 'EmAndamento' com as estorias: 
          | titulo    	 | situacao | 
          | Estoria01.01 | EmDesenv | 
          | Estoria03.01 | Pronto   | 
		  | Estoria03.03 | EmDesenv |
		  | Estoria03.04 | Pronto   |
   Quando montar os dados necessarios para o grafico de escopo vs completude do projeto 'projeto 01'
    Então os dados para o grafico de escopo vs completude do projeto 'projeto 01' devem ser:
          | modulo	  | pts nao iniciados   | % nao iniciados	| pts analise | % analise | pts desenvolvimento | % desenvolvimento | pts prontos	| % prontos | pts desvio	| % desvio	| pts mudança | % mudança | total pts modulo |
          | modulo 01 | 2					| 20				| 3			  | 30		  | 5					| 50	     		| 0				| 0			| 0				| 0			| 11		  | 110		  | 10     		     |
          | modulo 02 | 2					| 13,33				| 13	      | 86,67     | 0					| 0				    | 0				| 0			| 0				| 0			| 0			  | 0		  | 15	    	     |
          | modulo 03 | 7					| 28				| 3			  | 12		  | 2					| 8				    | 13			| 52		| 0				| 0			| 1			  | 4		  | 25			     |

