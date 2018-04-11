#language: pt-br

Funcionalidade: Manutencao da situacao das estorias.

@bug
Cenário: 01 - Estoria replanejada em um ciclo e finalizado no ciclo seguinte.
	Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 30      | 3               | 10            |
		 E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Replanejado | 5      | EscopoContratado | Mandatorio       |
          | estória 02 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 03 | Pronto      | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Pronto      | 5      | EscopoContratado | Mandatorio       |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 05 | Replanejado | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '3' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
		  | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 05 | Pronto      | 5      | EscopoContratado | Mandatorio       |
          | estória 06 | Pronto      | 3      | EscopoContratado | Mandatorio       |
	Quando salvar o ciclo '2' do projeto 'projeto 01' 
		Então as estorias do projeto 'projeto 01' devem estar com as seguintes situacoes:
		 | estória    | situação | quando  |
		 | estória 01 | Pronto   | Ciclo 2 |
		 | estória 02 | Pronto   | Ciclo 1 |
		 | estória 03 | Pronto   | Ciclo 1 |
		 | estória 04 | Pronto   | Ciclo 2 |
		 | estória 05 | Pronto   | Ciclo 3 |
		 | estória 06 | Pronto   | Ciclo 3 |

@bug
Cenário: 02 - Atualizar estoria replanejadas em um cilco e finalizadas em um ciclo posterior.
	Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 30      | 3               | 10            |
		 E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Replanejado | 5      | EscopoContratado | Mandatorio       |
          | estória 02 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 03 | Pronto      | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Pronto      | 5      | EscopoContratado | Mandatorio       |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 05 | Replanejado | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '3' do projeto 'projeto 01' na situação 'EmAndamento' com as estórias:
		  | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 05 | EmDesenv      | 5      | EscopoContratado | Mandatorio       |
	Quando alterar a pontuacao da estoria 'estória 01' do projeto 'projeto 01' 
		Então as estorias do projeto 'projeto 01' devem estar com as seguintes situacoes:
		 | estória    | situação | quando  |
		 | estória 01 | Pronto   | Ciclo 2 |
		 | estória 02 | Pronto   | Ciclo 1 |
		 | estória 03 | Pronto   | Ciclo 1 |
		 | estória 04 | Pronto   | Ciclo 2 |
		 | estória 05 | EmDesenv | Ciclo 3 |

@bug
Cenário: 03 - Remover a estória do ciclo.
	Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 30      | 3               | 10            |
		 E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Replanejado | 5      | EscopoContratado | Mandatorio       |
          | estória 02 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 03 | Pronto      | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | EmDesenv    | 5      | EscopoContratado | Mandatorio       |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 05 | Replanejado | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
	Quando remover a estoria 'estória 01' do ciclo '2' do projeto 'projeto 01'
		Então as estorias do projeto 'projeto 01' devem estar com as seguintes situacoes:
		 | estória    | situação    | quando  |
		 | estória 01 | Replanejado | P1      |
		 | estória 02 | Pronto      | Ciclo 1 |
		 | estória 03 | Pronto      | Ciclo 1 |
		 | estória 04 | Pronto      | Ciclo 2 |
		 | estória 05 | Replanejado | P2	  |

@bug
Cenário: 04 - Remover a estória nao replanejada do ciclo.
	Dado que exista(m) o(s) projeto(s) a seguir:
          | nome       | tamanho | total de ciclos | ritmo do time |
          | projeto 01 | 30      | 3               | 10            |
		 E ciclo '1' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | Replanejado | 5      | EscopoContratado | Mandatorio       |
          | estória 02 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 03 | Pronto      | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '2' do projeto 'projeto 01' na situação 'Concluido' com as estórias:
          | estória    | situação    | pontos | tipo             | valor do negócio |
          | estória 01 | EmDesenv    | 5      | EscopoContratado | Mandatorio       |
          | estória 04 | Pronto      | 3      | EscopoContratado | Mandatorio       |
          | estória 05 | Replanejado | 2      | EscopoContratado | Mandatorio       |
		 E ciclo '3' do projeto 'projeto 01' na situação 'NaoPlanejado'
	Quando remover a estoria 'estória 04' do ciclo '2' do projeto 'projeto 01'
		Então as estorias do projeto 'projeto 01' devem estar com as seguintes situacoes:
		 | estória    | situação    | quando  |
		 | estória 01 | Pronto      | Ciclo 2 |
		 | estória 02 | Pronto      | Ciclo 1 |
		 | estória 03 | Pronto      | Ciclo 1 |
		 | estória 04 | NaoIniciado | P1      |
		 | estória 05 | Replanejado | P2	  |
