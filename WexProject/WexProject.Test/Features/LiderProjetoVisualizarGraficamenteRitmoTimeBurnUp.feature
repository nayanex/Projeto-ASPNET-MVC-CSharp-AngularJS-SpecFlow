#language: pt-br

@pbi_5.08
Funcionalidade: Líder de Projeto visualizar graficamente o ritmo do time e o burn up
       Como um Líder de Projeto
       Eu quero poder visualizar graficamente o ritmo do time e o burn up
       Para que eu possa tomar ações de forma ágil, caso meu projeto esteja com desvio não aceitável de estimado x realizado

@rf_5.03 @rn_1.1 @rn_1.2 @rn_1.3 @rn
Cenário: 01 - Cálculo dos dados para o gráfico Estimado vs Realizado
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 01 | Pronto      | 5      | EscopoContratado	| Mandatorio		|
          | estória 02 | Pronto      | 3      | EscopoContratado	| Mandatorio		|
          | estória 03 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
        E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 04 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
          | estória 05 | Pronto      | 3      | EscopoContratado	| Mandatorio		|
          | estória 06 | Replanejado | 13     | EscopoContratado	| Mandatorio		|
        E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 07 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
          | estória 08 | Replanejado | 5      | EscopoContratado	| Mandatorio		|
        E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias:
          | estória    | situação    | pontos | tipo                | valor do negócio |
          | estória 09 | Pronto      | 3      | EscopoContratado    | Mandatorio       |
          | estória 10 | EmDesenv    | 3      | EscopoContratado	| Mandatorio	   |
          | estória 11 | NaoIniciado | 3      | EscopoContratado	| Mandatorio	   |
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 10       | 10        |           |                |
          | 2     | 20       | 15        |           |                |
          | 3     | 30       | 17        | 17        | 16,5           |
          | 4     | 40       |           | 27        |                |
          | 5     | 50       |           | 37        |                |

@rf_5.03 @rn_1.1 @rn_1.2 @rn_1.3 @rn
Cenário: 02 - Cálculo dos dados para o gráfico Estimado vs Realizado quando existir ciclo cancelado
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 60      | 6               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação           | pontos | tipo				| valor do negócio	|
          | estória 01 | Pronto             | 5      | EscopoContratado	| Mandatorio		|
          | estória 02 | Pronto             | 3      | EscopoContratado	| Mandatorio		|
          | estória 03 | Pronto             | 2      | EscopoContratado	| Mandatorio		|
        E ciclo '2' do projeto 'projeto 01' na situação 'Cancelado' com as estórias:
          | estória    | situação           | pontos | tipo				| valor do negócio	|
          | estória 04 | Replanejado        | 5      | EscopoContratado	| Mandatorio		|
        E ciclo '3' do projeto 'projeto 01' na situação 'Cancelado'
        E ciclo '4' do projeto 'projeto 01' na situação 'Cancelado' com as estórias:
          | estória    | situação          | pontos  | tipo				| valor do negócio	|
          | estória 04 | Pronto            | 5       | EscopoContratado	| Mandatorio		|
          | estória 05 | Replanejado       | 5       | EscopoContratado	| Mandatorio		|
        E ciclo '5' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias:
          | estória    | situação	       | pontos | tipo				| valor do negócio	|
          | estória 06 | Pronto			   | 3      | EscopoContratado	| Mandatorio		|
          | estória 07 | EmDesenv		   | 3      | EscopoContratado	| Mandatorio		|
          | estória 08 | NaoIniciado	   | 3      | EscopoContratado	| Mandatorio		|
        E ciclo '6' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 10       | 10        |           |                |
          | 2     | 20       | 10        |           |                |
          | 3     | 30       | 10        |           |                |
          | 4     | 40       | 15        | 15        | 22,5           |
          | 5     | 50       |           | 25        |                |
          | 6     | 60       |           | 35        |                |


@rf_5.03 @rn_1.1 @rn_1.2 @rn_1.3 @rn
Cenário: 03 - Cálculo dos dados para o gráfico Estimado vs Realizado quando existir ciclo em andamento entre ciclos realizados
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 01 | Pronto      | 5      | EscopoContratado	| Mandatorio		|
          | estória 02 | Pronto      | 3      | EscopoContratado	| Mandatorio		|
          | estória 03 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
        E ciclo '2' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 04 | Pronto		 | 3      | EscopoContratado	| Mandatorio		|
          | estória 05 | EmDesenv	 | 3      | EscopoContratado	| Mandatorio		|
          | estória 06 | NaoIniciado | 3      | EscopoContratado	| Mandatorio		|
        E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 07 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
        E ciclo '4' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 10       | 10        |           |                |
          | 2     | 20       | 10        |           |                |
          | 3     | 30       | 12        | 12        | 19             |
          | 4     | 40       |           | 22        |                |
          | 5     | 50       |           | 32        |                |

@rf_5.03 @rn_1.1 @rn_1.2 @rn_1.3 @rn
Cenário: 04 - Cálculo dos dados para o gráfico Estimado vs Realizado quando existir ciclo não planejado entre ciclos realizados
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 01 | Pronto      | 5      | EscopoContratado	| Mandatorio		|
          | estória 02 | Pronto      | 3      | EscopoContratado	| Mandatorio		|
          | estória 03 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
        E ciclo '2' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias:
          | estória    | situação	 | pontos | tipo				| valor do negócio	|
          | estória 04 | Pronto		 | 3      | EscopoContratado	| Mandatorio		|
          | estória 05 | EmDesenv	 | 3      | EscopoContratado	| Mandatorio		|
          | estória 06 | NaoIniciado | 3      | EscopoContratado	| Mandatorio		|
        E ciclo '5' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo				| valor do negócio	|
          | estória 07 | Pronto      | 2      | EscopoContratado	| Mandatorio		|
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 10       | 10        |           |                |
          | 2     | 20       | 10        |           |                |
          | 3     | 30       | 10        |           |                |
          | 4     | 40       | 10        |           |                |
          | 5     | 50       | 12        |           |                |

@rf_5.03 @rn_1.1 @rn_1.2 @rn_1.3 @rn
Cenário: 05 - Cálculo dos dados para o gráfico Estimado vs Realizado para um projeto sem ciclos concluídos
     Dado que exista(m) o(s) projeto(s) a seguir:
	      | nome       | tamanho | total de ciclos | ritmo do time |
		  | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '2' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '4' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 10       |           | 10        | 10             |
          | 2     | 20       |           | 20        |                |
          | 3     | 30       |           | 30        |                |
          | 4     | 40       |           | 40        |                |
          | 5     | 50       |           | 50        |                |

@rf_5.04 @rn_1.1 @rn
Cenário: 06 - Cálculo dos dados para o gráfico Ritmo do Time
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 01 | Pronto   | 5      | EscopoContratado | Mandatorio       |  sim |
          | estória 02 | Pronto   | 3      | EscopoContratado | Mandatorio       |  sim |
          | estória 03 | Pronto   | 2      | EscopoContratado | Mandatorio       |  sim |
        E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação   | pontos | tipo				| valor do negócio	| meta |
          | estória 04 | Pronto     | 2      | EscopoContratado	| Mandatorio		|  sim |
          | estória 05 | Pronto     | 3      | EscopoContratado	| Mandatorio		|  sim |
        E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 06 | Pronto   | 2      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias e metas:
          | estória    | situação    | pontos | tipo             | valor do negócio | meta |
          | estória 07 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 08 | EmDesenv    | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 09 | NaoIniciado | 3      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |
          |   1   |   10  |  10  |       10  |
          |   2   |    5  |   5  |        5  |
          |   3   |    2  |   2  |        2  |
          |   4   |       |      |           |
          |   5   |       |      |           |

@rf_5.04 @rn_1.1 @rn
Cenário: 07 - Cálculo dos dados para o gráfico Ritmo do Time quando existir ciclo cancelado
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 01 | Pronto   | 5      | EscopoContratado | Mandatorio       | sim  |
          | estória 02 | Pronto   | 3      | EscopoContratado | Mandatorio       | nao  |
          | estória 03 | Pronto   | 2      | EscopoContratado | Mandatorio       | nao  |
        E ciclo '2' do projeto 'projeto 01' na situação 'Cancelado' com as estórias e metas:
          | estória    | situação    | pontos | tipo             | valor do negócio | meta |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 05 | Replanejado | 3      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '3' do projeto 'projeto 01' na situação 'Cancelado'
        E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias e metas:
          | estória    | situação    | pontos | tipo             | valor do negócio | meta |
          | estória 05 | Pronto      | 3      | EscopoContratado | Mandatorio       |  sim |
          | estória 06 | EmDesenv    | 3      | EscopoContratado | Mandatorio       |  nao |
          | estória 07 | NaoIniciado | 3      | EscopoContratado | Mandatorio       |  nao |
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |
          | 1     |   10  |   5  |       10  |
          | 2     |    3  |   6  |        6  |
          | 3     |    0  |   0  |        0  |
          | 4     |       |      |           |
          | 5     |       |      |           |

@rf_5.04 @rn_1.1 @rn
Cenário: 08 - Cálculo dos dados para o gráfico Ritmo do Time quando existir ciclo em andamento entre ciclos realizados
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 01 | Pronto   | 5      | EscopoContratado | Mandatorio       | sim  |
          | estória 02 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 03 | Pronto   | 2      | EscopoContratado | Mandatorio       | nao  |
        E ciclo '2' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias e metas:
          | estória    | situação    | pontos | tipo             | valor do negócio | meta |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 05 | EmDesenv    | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 06 | NaoIniciado | 3      | EscopoContratado | Mandatorio       | nao  |
        E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 07 | Pronto   | 2      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '4' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '5' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
		 | estória    | situação | pontos | tipo             | valor do negócio | meta |
		 | estória 08 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
		 | estória 09 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
		 | estória 10 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
   Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |
          | 1     |   10  |   8  |       10  |
          | 2     |    0  |   6  |        9  |
          | 3     |    2  |   2  |        2  |
          | 4     |    0  |   0  |        0  |
          | 5     |    9  |   9  |        9  |

@rf_5.04 @rn_1.1 @rn
Cenário: 09 - Cálculo dos dados para o gráfico Ritmo do Time quando existir ciclo não planejado entre ciclos realizados
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | pontos | tipo             | valor do negócio | meta |
          | estória 01 | Pronto   | 5      | EscopoContratado | Mandatorio       | nao  |
          | estória 02 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 03 | Pronto   | 2      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '2' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias e metas:
          | estória    | situação    | pontos | tipo             | valor do negócio | meta |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 05 | EmDesenv    | 3      | EscopoContratado | Mandatorio       | nao  |
          | estória 06 | NaoIniciado | 3      | EscopoContratado | Mandatorio       | sim  |
        E ciclo '5' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação | ponto | tipo             | valor do negócio | meta |
          | estória 06 | Pronto   | 2     | EscopoContratado | Mandatorio       | sim  |
    Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
		 Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
           | ciclo | ritmo | meta | planejado |
           | 1     |   10  |   5  |       10  |
           | 2     |    0  |   0  |        0  |
           | 3     |    0  |   0  |        0  |
           | 4     |    0  |   6  |        9  |
           | 5     |    2  |   2  |        2  |

@rf_5.04 @rn_1.1 @rn
Cenário: 10 - Cálculo dos dados para o gráfico Ritmo do Time para um projeto sem ciclos concluídos
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '2' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '4' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '5' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |
          | 1     |       |      |           |
          | 2     |       |      |           |
          | 3     |       |      |           |
          | 4     |       |      |           |
          | 5     |       |      |           |

@bug
Cenário: 11 - Projeto sem ciclos
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 0               | 10            |
     Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |

@bug
Cenário: 12 - Projeto sem ciclos
     Dado que exista(m) o(s) projeto(s) a seguir:
	      | nome       | tamanho | total de ciclos | ritmo do time |
		  | projeto 01 | 50      | 0               | 10            |
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |

		  
@rn
Cenário: 13 - Cálculo da meta e planejado com as possíveis valores de meta e planejado
     Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 50      | 5               | 10            |
        E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação    | pontos | tipo			 | valor do negócio | meta |
          | estória 01 | Replanejado | 5      | EscopoContratado | Mandatorio       | sim  |
          | estória 02 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 03 | Pronto      | 2      | EscopoContratado | Mandatorio       | sim  |
		 E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação    | pontos | tipo			 | valor do negócio | meta |
		  | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 05 | Replanejado | 3      | EscopoContratado | Mandatorio       | sim  |
          | estória 06 | Replanejado | 3      | EscopoContratado | Mandatorio       | nao  |
		 E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
          | estória    | situação    | pontos | tipo			 | valor do negócio | meta |
		  | estória 07 | Pronto		 | 2      | EscopoContratado | Mandatorio       | sim  |
		 E ciclo '4' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias e metas:
		  | estória    | situação | pontos | tipo             | valor do negócio | meta |
		  | estória 08 | Pronto   | 3      | EscopoContratado | Mandatorio       | sim  |
		  | estória 09 | Pronto   | 3      | EscopoContratado | Mandatorio       | nao  |
		  | estória 10 | Pronto   | 3      | EscopoContratado | Mandatorio       | nao  |
		 E ciclo '5' do projeto 'projeto 01' na situação 'Concluido' com as estórias e metas:
		  | estória    | situação    | pontos | tipo             | valor do negócio | meta |
		  | estória 11 | Pronto      | 3      | EscopoContratado | Mandatorio       | sim  |
		  | estória 12 | Pronto      | 3      | EscopoContratado | Mandatorio       | nao  |
		  | estória 13 | Replanejado | 3      | EscopoContratado | Mandatorio       | nao  |
   Quando montar os dados necessários para o gráfico de ritmo do time do projeto 'projeto 01'
        Então os dados para o gráfico de ritmo do time do projeto 'projeto 01' devem ser:
          | ciclo | ritmo | meta | planejado |
          | 1     |    5  |  10  |       10  |
          | 2     |    3  |   6  |        9  |
          | 3     |    2  |   2  |        2  |
          | 4     |    9  |   3  |        9  |
          | 5     |    6  |   3  |        9  |

@BUG
Cenário: 14 - Testando o calculo dos pontos estimados.
     Dado que exista(m) o(s) projeto(s) a seguir:
             | nome       | tamanho | total de ciclos | ritmo do time |
               | projeto 01 | 40      | 3               | 10          |
        E ciclo '1' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '2' do projeto 'projeto 01' na situação 'NaoPlanejado'
        E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
   Quando montar os dados necessários para o gráfico de estimado vs realizado do projeto 'projeto 01'
        Então os dados para o gráfico estimado vs realizado do projeto 'projeto 01' devem ser:
          | ciclo | estimado | realizado | tendência | ritmo sugerido |
          | 1     | 13       |           | 10        | 13,33          |
          | 2     | 26       |           | 20        |                |
          | 3     | 40       |           | 30        |                |
