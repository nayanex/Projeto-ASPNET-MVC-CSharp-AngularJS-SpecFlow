#language: pt-br

Funcionalidade: Manter o total de pontos dos modulos inalterados quando o total de pontos do projeto sofrer alteracao.

@bug
Cenário: 01 - Um projeto é criado e posteriormente tem seu total de pontos alterado.
	 Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto01  | 50      | 1               | 10            |
	    E que existam os seguintes modulos para o 'projeto01':
          | nome     | tamanho | modulo pai |
          | modulo01 | 20      |	    	| 
		  | modulo02 | 15      |		    |
		  | modulo03 | 5       |		    |
		E o projeto 'projeto01' tenha seu tamanho alterado para '100'
		  | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto01  | 100     | 1               | 10            |
	 Quando o projeto 'projeto01' for salvo:
	 Entao os modulos a seguir do projeto 'projeto01' devem estar com os seguintes valores:
		  | nome     | tamanho | modulo pai |
		  | modulo01 | 20      |			|
		  | modulo02 | 15      |			|
		  | modulo03 | 5       |			|
	  