#language: pt-BR

@pbi_3.02.02
Funcionalidade: ter a data de inicio das tarefas calculadas automaticamente conforme a capacidade de planejamento do time

Contexto:
  Dado que exista(m) a(s) situação(oes) de planejamento a seguir:
	   | descricao    | tipo         | padrao |
	   | Nao iniciado | Planejamento | Sim    |
	   | Em andamento | Execução     | Não    |
	   | Pronto       | Encerramento | Não    |
	   | Cancelado    | Cancelamento | Não    |
	   | Impedido     | Impedimento  | Não    |
	 E que exista(m) o(s) cronograma(s) 
	   | nome | inicio | final |
	   | C1   | 01/08  | 10/08 |
	 E que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | Joao        |          | 01/01/2011 | 
	   | Maria       |          | 01/01/2011 |
	   | Jose        |          | 01/01/2011 |
     E que o servidor esta ligado	 
	 #E que o(s) cronograma(s) 'C1' estao prontos para serem conectados ao servidor //retirado pois não precisa desse step, os cronogramas já estão disponíveis.
     #E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria', 'Jose' conectado(s) // step não criado, pois já existe 1 para ser reaproveitado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.01 - Criar uma nova tarefa no final da lista quando nao houver capacidade de planejamento estimada
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
		| descricao | situacao     | estimativa |
		| T1        | Não Iniciado | 300        |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 300        | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 300   | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 300        | 01/08  |	 

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.02 - Criar uma nova tarefa no final da lista com estimativa que caiba no primeiro dia da capacidade de planejamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |   dia   | horas |
       |  01/08  |  10   |
	   |  02/08  |  15   |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao | situacao     | estimativa |
       | TI        | Nao iniciado | 5          |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 5          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 5          | 01/08  |
	   	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.03 - Criar uma nova tarefa no final da lista quando existir sobra de estimativa do dia anterior
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |   dia   | horas |
       |  01/08  |  10   |
	   |  02/08  |  15   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 9          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | descricao | situacao     | estimativa |
	   | T2        | Nao iniciado | 5          |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T2  		| Nao iniciado | 5          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 4     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 5          | 02/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.04 - Criar uma nova tarefa no final da lista em que a estimatva nao caiba totalmente em um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |   dia   | horas |
       |  01/08  |  10   |
	   |  02/08  |  15   |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao | situacao     | estimativa |
       | T1        | Nao iniciado | 11         |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 11         | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 11         | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.05 - Criar uma nova tarefa no final da lista que ultrapasse o prazo final do cronograma
  Dado que exista(m) o(s) cronograma(s) 
	   | nome | inicio | termino |
	   | C2   | 01/08  |  02/08  |
	 E que o cronograma 'C2' possui a capacidade de planejamento a seguir:
       |   dia   | horas |
       |  01/08  |  10   |
	   |  02/08  |  5    |
	 E que o cronograma 'C2' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 9          | 01/08  |
	 E que o cronograma 'C2' tenha o(s) colaborador(es) 'Joao', 'Maria', 'Jose' conectado(s)	 
Quando o cronograma 'C2' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao | situacao     | estimativa |
       | T1        | Nao iniciado | 7          |
 Entao o cronograma 'C2' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 7          | 01/08  |
	 E o cronograma 'C2' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 5     | planejado |
	   | 03/08 | 1     | desvio    |
	 E o cronograma 'C2' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T1        | Nao iniciado | 11         | 01/08  |	   

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.06 - Criar uma nova tarefa no final da lista em dia sem capacidade de planejamento estimada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  8    |
	   | 03/08 |  6    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 8          | 01/08  |
	 E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria', 'Jose' conectado(s)	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao  | T1           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 3          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 8     | planejado |
	   | 03/08 | 6     | planejado |
	   | 04/08 | 1     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T2        | Nao iniciado | 7          | 03/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.07 - Criar uma nova tarefa no final da lista consumindo a capacidade de planejamento de mais de um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |   5   |
	   | 02/08 |   3   |
	   | 03/08 |   6   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao  | T2           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 6      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1        | Nao iniciado | 3          | 01/08  |
	   | 2  | T2        | Nao iniciado | 6          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T2        | Nao iniciado | 6          | 01/08  |	   
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.08 - Criar uma nova tarefa no inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  15   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 4          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 7          | 01/08  |
	   | 2  | T1  		| Nao iniciado | 3          | 01/08  |
	   | 3  | T2  		| Nao iniciado | 4          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 4     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 7          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 01/08  |
	   | T2        | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.09 - Criar uma nova tarefa no inicio da lista em que a estimatva nao caiba totalmente em um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  15   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 4          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 11           |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 11         | 01/08  |
	   | 2  | T1  		| Nao iniciado | 3          | 02/08  |
	   | 3  | T2  		| Nao iniciado | 4          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 8     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 11         | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 02/08  |
	   | T2        | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.10 - Criar uma nova tarefa no inicio da lista que impacte em desvio no prazo final do cronograma
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  8    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 4          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 12           |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 12         | 01/08  |
	   | 2  | T1  		| Nao iniciado | 3          | 02/08  |
	   | 3  | T2  		| Nao iniciado | 4          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 10    | planejado |
	   | 02/08 | 8     | planejado |
	   | 03/08 | 1     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 12         | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 02/08  |
	   | T2        | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.11 - Criar uma nova tarefa no inicio da lista quando impactar em um dia sem capacidade de planejamento estimada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 6            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 6          | 01/08  |
	   | 2  | T1  		| Nao iniciado | 2          | 03/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 03/08 | 5     | planejado |
	   | 04/08 | 1     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 6          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 03/08  |
	   | T2        | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.12 - Criar uma nova tarefa no inicio da lista que consuma a capacidade de planejamento de mais de um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	   | 03/08 |  6    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 8            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 8          | 01/08  |
	   | 2  | T1  		| Nao iniciado | 2          | 03/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 2     | planejado |
	   | 03/08 | 6     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 8          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 03/08  |
	   | T2        | 03/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.13 - Criar uma nova tarefa no meio da lista	   
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  10   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 5          | 02/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 2			   |
       | descricao  | T4           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 3            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T4  		| Nao iniciado | 3          | 01/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 02/08  |
	   | 4  | T3  		| Nao iniciado | 5          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 5     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T4  		| Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.14 - Criar uma nova tarefa no meio da lista em que a estimatva nao caiba totalmente em um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  10   |
	   | 03/08 |  6    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 5          | 02/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 2			   |
       | descricao  | T4           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 8            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T4  		| Nao iniciado | 8          | 01/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 02/08  |
	   | 4  | T3  		| Nao iniciado | 5          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 10    | planejado |
	   | 03/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T4  		| Nao iniciado | 8          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.15 - Criar uma nova tarefa no meio da lista que impacte em desvio no prazo final do cronograma
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  10   |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 5          | 02/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 2			   |
       | descricao  | T4           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 10           |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T4  		| Nao iniciado | 10         | 01/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 02/08  |
	   | 4  | T3  		| Nao iniciado | 5          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 10    | planejado |
	   | 03/08 | 5     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T4  		| Nao iniciado | 10         | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.16 - Criar uma nova tarefa no meio da lista quando impactar em um dia sem capacidade de planejamento estimada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  6    |
	   | 03/08 |  8    |	   
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 01/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 2			   |
       | descricao  | T4           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 5            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T4  		| Nao iniciado | 6          | 01/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 03/08  |
	   | 4  | T3  		| Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 6     | planejado |
	   | 03/08 | 5     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T4  		| Nao iniciado | 5          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.17 - Criar uma nova tarefa no meio da lista consumindo a capacidade de planejamento de mais de um dia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	   | 03/08 |  7    |	   
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 03/08  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 2			   |
       | descricao  | T4           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7            |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T4  		| Nao iniciado | 7          | 01/08  |
	   | 3  | T2  		| Nao iniciado | 3          | 03/08  |
	   | 4  | T3  		| Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 5     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 2  | T4  		| Nao iniciado | 7          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.18 - Excluir uma tarefa no inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 3          | 01/08  |
	   | 2  | T3  		| Nao iniciado | 1          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 4     | planejado |
	   | 02/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.19 - Excluir uma tarefa no meio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T3  		| Nao iniciado | 1          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 3     | planejado |
	   | 02/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.20 - Excluir uma tarefa no meio da lista quando impactar em um dia sem capacidade de planejamento estimada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 03/08  |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T3  		| Nao iniciado | 1          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 3     | planejado |
	   | 03/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.21 - Excluir uma tarefa no fim da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  |
	 E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria', 'Jose' conectado(s)
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T3' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T2  		| Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.22 - Excluir uma tarefa no fim da lista removendo tarefas em desvio
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 5          | 02/08  |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T3' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T2  		| Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 0     | planejado |
	   | 03/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.22 - Excluir uma tarefa no fim da lista mantendo tarefas em desvio
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 5          | 02/08  |
	   | 4  | T4        | Nao iniciado | 5          | 03/08  |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T4' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T1  		| Nao iniciado | 2          | 01/08  |
	   | 2  | T2  		| Nao iniciado | 3          | 01/08  |
	   | 3  | T3  		| Nao iniciado | 5          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 2     | planejado |
	   | 03/08 | 3     | devio 	   |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.23 - Mover tarefa para o inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 1          | 01/08  |
	   | 2  | T1  		| Nao iniciado | 2          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.24 - Mover tarefa para o meio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  |
	   | 4  | T4        | Nao iniciado | 1          | 02/08  |
Quando o cronograma 'C1' tiver a tarefa 'T4' movida para a posicao '2' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 1          | 01/08  |
	   | 2  | T4        | Nao iniciado | 1          | 01/08  |
	   | 3  | T1  		| Nao iniciado | 2          | 01/08  |
       | 4  | T2        | Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 2     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |
	   | T4  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.25 - Mover tarefa para o meio da lista em um dia em que a estimativa da tarefa nao caiba totalmente
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  4    |
	   | 03/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 4          | 02/08  |
	   | 4  | T4        | Nao iniciado | 1          | 03/08  |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '2' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
	   | 2  | T3        | Nao iniciado | 4          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 02/08  |
	   | 4  | T4        | Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 4     | planejado |
	   | 03/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.26 - Mover tarefa que estava em desvio para o meio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 3          | 02/08  |
	   | 4  | T4        | Nao iniciado | 4          | 03/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '2' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
	   | 2  | T4        | Nao iniciado | 4          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 02/08  |
	   | 4  | T3        | Nao iniciado | 3          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 4     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 03/08  |
	   | T4  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.27 - Mover tarefa para o meio da lista impactando em um dia em que nao ha capacidade de planejamento disponivel
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  4    |
	   | 04/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 4          | 03/08  |
	   | 4  | T4        | Nao iniciado | 2          | 04/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '2' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
	   | 2  | T3        | Nao iniciado | 4          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 03/08  |
	   | 4  | T4        | Nao iniciado | 2          | 04/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.28 - Mover tarefa que consome mais de um dia de capacidade de planejamento para o meio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	   | 03/08 |  2    |
	   | 04/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 3          | 02/08  |
	   | 4  | T4        | Nao iniciado | 2          | 03/08  |
	   | 5  | T5        | Nao iniciado | 5          | 04/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T5' movida para a posicao '3' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T5        | Nao iniciado | 5          | 02/08  |
	   | 4  | T3        | Nao iniciado | 3          | 04/08  |
	   | 5  | T4        | Nao iniciado | 2          | 04/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 2     | planejado |
	   | 04/08 | 5     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 04/08  |
	   | T4  	   | 04/08  |
	   | T5 	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.29 - Mover tarefa para o fim da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 3          | 02/08  |
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T2        | Nao iniciado | 3          | 01/08  |
	   | 2  | T3        | Nao iniciado | 3          | 01/08  |
       | 3  | T1        | Nao iniciado | 2          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 01/08  |
	   | T3 	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.30 - Mover tarefa para o fim da lista impactando no consumo de mais um dia da capacidade de planejamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	   | 03/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 02/08  |
	   | 3  | T3        | Nao iniciado | 2          | 03/08  |	
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T2        | Nao iniciado | 3          | 01/08  |
	   | 2  | T3        | Nao iniciado | 2          | 01/08  |
       | 3  | T1        | Nao iniciado | 5          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 02/08 | 2     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 02/08  |
	   | T2  	   | 01/08  |
	   | T3 	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.31 - Mover tarefa para o fim da lista impactando em um dia em que nao ha capacidade de planejamento disponivel
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  3    |
	   | 04/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 03/08  |
	   | 3  | T3        | Nao iniciado | 2          | 04/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T2        | Nao iniciado | 3          | 01/08  |
	   | 2  | T3        | Nao iniciado | 2          | 01/08  |
       | 3  | T1        | Nao iniciado | 5          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 03/08 | 3     | planejado |
	   | 04/08 | 2     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 03/08  |
	   | T2  	   | 01/08  |
	   | T3 	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.32 - Mover tarefa para o fim da lista quando o cronograma estiver com desvio de planejamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 02/08  |
	   | 3  | T3        | Nao iniciado | 2          | 03/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T2        | Nao iniciado | 3          | 01/08  |
	   | 2  | T3        | Nao iniciado | 2          | 01/08  |
       | 3  | T1        | Nao iniciado | 5          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 2     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T1  	   | 02/08  |
	   | T2  	   | 01/08  |
	   | T3 	   | 01/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.32 - Aumentar a estimativa inicial de uma tarefa nao iniciada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.33 - Aumentar a estimativa inicial de uma tarefa gerando desvio
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '6'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 6          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 02/08 | 1     | desvio    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.36 - Aumentar a estimativa inicial impactando em um dia em que nao ha capacidade de planejamento disponivel
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.37 - Aumentar a estimativa inicial impactando no consumo de mais de um dia da capacidade de planejamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	   | 03/08 |  4    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Nao iniciado | 2          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 01/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '9'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 9          | 01/08  |
       | 2  | T2        | Nao iniciado | 2          | 03/08  |
	   | 3  | T3        | Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 4     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.38 - Reduzir a estimativa inicial de uma tarefa nao iniciada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 2          | 02/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '4'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 4          | 01/08  |
       | 2  | T2        | Nao iniciado | 2          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.39 - Reduzir a estimativa inicial de uma tarefa nao iniciada removendo desvio
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 4          | 02/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '4'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 4          | 01/08  |
       | 2  | T2        | Nao iniciado | 4          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 3     | planejado |
	   | 03/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.40 - Reduzir a estimativa inicial impactando em um dia em que nao ha capacidade de planejamento disponivel
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 03/08 |  3    |
	   | 04/08 |  1    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 03/08  |
	   | 3  | T3        | Nao iniciado | 1          | 04/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '2'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	   | 3  | T3        | Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 03/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   | T3  	   | 03/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.41 - Reduzir a estimativa inicial impactando no consumo de mais de um dia da capacidade de planejamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	   | 03/08 |  1    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 8          | 01/08  |
       | 2  | T2        | Nao iniciado | 1          | 03/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 1          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 5     | planejado |
	   | 02/08 | 1     | planejado |
	   | 03/08 | 0     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.42 - Criar nova tarefa quando houver tarefas prontas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Pronto       | 1          | 02/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Pronto       | 1          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 3     | planejado |
	   | 02/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.43 - Excluir uma tarefa no inicio da lista na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Pronto       | 1          | 02/08  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T2        | Pronto       | 1          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | horas | situacao  |
	   | 01/08 | 0     | planejado |
	   | 02/08 | 1     | planejado |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	 
	 
										#// -------------------------------------------------- //
										#// CENARIOS DE TAREFAS COM A SITUACAO DE ENCERRAMENTO //
										#// -------------------------------------------------- //	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.44 - Mover tarefa para o inicio da lista quando houverem tarefas prontas
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Pronto       | 2          | 01/08  | 0        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  | 1        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 01/08 | T1     | Em andamento | 2            | 1        | Maria       | 7:00      | 9:00   |
	   | 02/08 | T1     | Pronto       | 1            | 0        | Maria       | 8:00      | 9:00   |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3        | Nao iniciado | 1          | 01/08  |
       | 2  | T1        | Pronto       | 2          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 5        | 2        |
	   | 02/08 | 1        | 1        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 2     | 0     |
	   |           | realizado | 2     | 1     |
	   |           | restante  | 1     | 0     |
	   | T2        | planejado | 2     | 1     |
	   |           | restante  | 2     | 1     |
	   | T3        | planejado | 1     |       |
	   |           | restante  | 1     |       |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.45 - Mover tarefa para o inicio da lista quando houverem tarefas prontas que consomem o dia inteiro
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Pronto       | 2          | 01/08  | 0        |
       | 2  | T2        | Pronto       | 2          | 01/08  | 0        |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  | 1        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
       | 01/08 | T1     | Pronto   | 3            | 0        | Maria       | 7:00      | 10:00  |
	   | 01/08 | T2     | Pronto   | 2            | 0        | Jose        | 7:00      | 10:00  |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3        | Nao iniciado | 1          | 02/08  |
       | 2  | T1        | Pronto       | 2          | 01/08  |
       | 3  | T2        | Pronto       | 2          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 4        | 5        |
	   | 02/08 | 1        |          |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 2     |       |
	   |           | realizado | 3     |       |
	   |           | restante  | 0     |       |
	   | T2        | planejado | 2     |       |
	   |           | realizado | 2     |       |
	   |           | restante  | 0     |       |
	   | T3        | planejado |       | 1     |
	   |           | restante  |       | 1     |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.46 - Mover uma tarefa pronta para o inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Pronto       | 4          | 02/08  | 0        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T3     | Pronto   | 5            | 0        | Maria       | 7:00      | 12:00  |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3        | Pronto       | 4          | 02/08  |
       | 2  | T1        | Nao iniciado | 2          | 01/08  |
       | 3  | T2        | Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 4        | 5        |
	   | 02/08 | 1        |          |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 2     |       |       |
	   |           | restante  | 2     |       |       |
	   | T2        | planejado | 3     |       |       |
	   |           | restante  | 3     |       |       |
	   | T3        | planejado |       | 2     | 2     |
	   |           | realizado |       | 5     | 0     |
	   |           | restante  |       | 0     |       |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.47 - Aumentar a estimativa inicial de uma tarefa que possua tarefas prontas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 4          | 01/08  | 4        |
       | 2  | T2        | Pronto       | 3          | 01/08  | 3        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T2     | Pronto   | 3            | 0        | Jose        | 7:00      | 10:00  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '6'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 6          | 01/08  |
       | 2  | T2        | Pronto       | 3          | 01/08  |	
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 2     | 3     | 1     |
	   |           | realizado |       |       |       |
	   | T2        | planejado | 1     | 2     |       |
	   |           | realizado | 3     | 0     |       |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 3        | 3        |
	   #// -> desconsiderando as horas planejadas 	
	   | 02/08 | 3        | 0        |   
	   #//    que ja possuem esforco realizado.
	   | 03/08 | 0        | 1        | 
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T1        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.48 - Reduzir a estimativa inicial de uma tarefa que possua tarefas prontas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Pronto       | 3          | 02/08  |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 02/08 | T2     | Pronto   | 3            | 0        | Jose        | 7:00      | 10:00  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Pronto       | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 3     |       |
	   |           | restante  | 3     |       |
	   | T2        | planejado |       | 3     |
	   |           | realizado |       | 3     |
	   |           | restante  |       | 0     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 3        |          |
	   | 02/08 | 3        | 3        |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.49 - Criar tarefa quando houverem tarefas prontas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  9    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Pronto       | 10         | 01/08  |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T1     | Pronto   | 10           | 0        | Jose        | 8:00      | 19:00  |	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T2           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	   | 2  | T1  		| Nao iniciado | 10         | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 10    |       |
	   |           | realizado | 10    |       |
	   |           | restante  | 0     |       |
	   | T2        | planejado |       | 7     |
	   |           | restante  |       | 7     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 10       | 10       |
	   | 02/08 | 7        |          |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.50 - Criar uma tarefa nao iniciada para apos uma tarefa pronta
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  2    |
	   | 02/08 |  5    |
	   | 03/08 |  1    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
	   | 2  | T2        | Pronto       | 4          | 02/08  | 0        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T2     | Em andamento | 4            | 1        | Maria       | 7:00      | 11:00  |
	   | 03/08 | T2     | Pronto       | 1            | 0        | Maria       | 7:00      | 8:00   |	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 1      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 2          | 01/08  |
	   | 2  | T2        | Pronto       | 4          | 02/08  |
	   | 3  | T3        | Nao iniciado | 1          | 04/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 8        |          |
	   | 02/08 | 4        | 4        |
	   | 03/08 | 1        | 1        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 | 04/08 |
	   | T1        | planejado | 2     |       |       |       |
	   |           | restante  | 2     |       |       |       |
	   | T2        | planejado |       | 4     | 0     |       |
	   |           | realizado |       | 4     | 1     |       |
	   |           | restante  |       | 1     | 0     |       |
	   | T3        | planejado |       |       |       | 1     |
	   |           | restante  |       |       |       | 1     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 04/08 |
	   | T3        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 1          | 04/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	   

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.51 - Excluir tarefa quando houverem tarefas prontas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Pronto       | 3          | 02/08  | 0        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T3     | Pronto   | 3            | 0        | Jose        | 8:00      | 11:00  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Pronto       | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado |       |       |
	   |           | restante  |       |       |
	   | T2        | planejado |       | 3     |
	   |           | realizado |       | 3     |
	   |           | restante  |       | 0     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 0        |          |
	   | 02/08 | 3        | 3        |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
										#// -------------------------------------------------- //
										#// CENARIOS DE TAREFAS COM A SITUACAO DE CANCELAMENTO //
										#// -------------------------------------------------- //	 
	 	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.52 - Mover tarefa para o inicio da lista quando houverem tarefas canceladas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Cancelada    | 2          | 01/08  | 0        |
       | 2  | T2        | Nao iniciado | 5          | 01/08  | 5        |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  | 1        |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3        | Nao iniciado | 1          | 01/08  |
       | 2  | T1        | Cancelada    | 2          | 01/08  |
       | 3  | T2        | Nao iniciado | 5          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 2     |       |
	   |           | restante  | 0     |       |
	   | T2        | planejado | 4     | 1     |
	   |           | restante  | 4     | 1     |
	   | T3        | planejado | 1     |       |
	   |           | restante  | 1     |       |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 5        |          |
	   | 02/08 | 1        |          |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.53 - Mover tarefa cancelada para o inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
	   | 2  | T2        | Nao iniciado | 1          | 02/08  | 1        |
       | 3  | T3        | Cancelada    | 2          | 02/08  | 0        |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
       | 1  | T3        | Cancelada    | 2          | 02/08  |
       | 2  | T1        | Nao iniciado | 5          | 01/08  |
	   | 3  | T2        | Nao iniciado | 1          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       | 1     |
	   |           | restante  |       | 1     |
	   | T3        | planejado |       | 2     |
	   |           | restante  |       | 0     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 5        |          |
	   | 02/08 | 1        |          |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.54 - Aumentar a estimativa inicial de uma tarefa que possua tarefas canceladas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 3          | 01/08  |
       | 2  | T2        | Cancelada    | 4          | 01/08  |
       | 3  | T3        | Nao iniciado | 2          | 01/08  |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 2  | T2        | Cancelada    | 4          | 01/08  |
       | 3  | T3        | Nao iniciado | 2          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado | 4     |       |
	   |           | restante  | 0     |       |
	   | T3        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 5        |          |
	   | 02/08 | 2        |          |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.55 - Aumentar a estimativa inicial de uma tarefa cancelada
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Cancelada    | 3          | 01/08  | 0        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Cancelada    | 5          | 01/08  |
       | 2  | T2        | Nao iniciado | 3          | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 0     |       |
	   | T2        | planejado | 3     |       |
	   |           | restante  | 3     |       |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 3        |          |
	   | 02/08 | 0        |          |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.56 - Reduzir a estimativa inicial de uma tarefa que possua tarefas canceladas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
       | 2  | T2        | Cancelada    | 2          | 02/08  | 0        |	 
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '4'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 4          | 01/08  |
       | 2  | T2        | Cancelada    | 2          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 |
	   | T1        | planejado | 4     |
	   |           | restante  | 4     |
	   | T2        | planejado | 2     |
	   |           | restante  | 0     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 4        |          |
	   | 02/08 | 0        |          |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.57 - Criar tarefa quando houverem tarefas canceladas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  9    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Cancelada    | 10         | 01/08  | 0        |	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T2           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 01/08  |
	   | 2  | T1  		| Cancelada    | 10         | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 |
	   | T1        | planejado | 10    |
	   |           | restante  | 0     |
	   | T2        | planejado | 7     |
	   |           | restante  | 7     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 7        |          |
	   | 02/08 | 0        |          |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 01/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.58 - Excluir tarefa quando houverem tarefas canceladas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
       | 2  | T2        | Cancelada    | 3          | 02/08  | 0        |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 |
	   | T1        | planejado | 0     |
	   |           | restante  | 0     |
	   | T2        | planejado | 5     |
	   |           | restante  | 0     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 0        |          |
	   | 02/08 | 0        |          |	   
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.59 - Excluir tarefa cancelada 
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
       | 2  | T2        | Cancelada    | 5          | 01/08  | 0        |
       | 3  | T3        | Nao iniciado | 2          | 02/08  | 2        |
	   | 4  | T4        | Nao iniciado | 3          | 02/08  | 3        |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 5          | 01/08  |
       | 3  | T3        | Nao iniciado | 2          | 02/08  |
	   | 4  | T4        | Nao iniciado | 3          | 02/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	   | T2        | planejado |       | 3     |
	   |           | restante  |       | 3     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais |
	   | 01/08 | 5        |          |
	   | 02/08 | 5        |          |	   
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	 
	 
										#// ----------------------------------------------- //
										#// CENARIOS DE TAREFAS COM A SITUACAO EM ANDAMENTO //
										#// ----------------------------------------------- //	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.60 - Mover tarefa para o inicio da lista quando houverem tarefas em andamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 2          | 01/08  | 4        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  | 1        |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Nao iniciado | 1          | 01/08  | 1        |
       | 2  | T1        | Em andamento | 2          | 01/08  | 4        |
       | 3  | T2        | Nao iniciado | 3          | 02/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |          | 5             |
	   | 02/08 | 2        |          | 2             |
	   | 03/08 | 1        |          | 1             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T3        | planejado | 1     |       |       |
	   |           | restante  | 1     |       |       |
	   | T1        | planejado | 2     |       |       |
	   |           | restante  | 4     |       |       |
	   | T2        | planejado |       | 2     | 1     |
	   |           | restante  |       | 2     | 1     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T2        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 01/08  |	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.61 - Mover tarefa para o inicio da lista quando houverem tarefas em andamento que consomem o dia inteiro
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 2          | 01/08  | 2        |
       | 2  | T2        | Em andamento | 2          | 01/08  | 2        |
	   | 3  | T3        | Nao iniciado | 2          | 02/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 01/08 | T1     | Em andamento | 1            | 2        | Maria       | 7:00      | 8:00   |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Nao iniciado | 2          | 01/08  | 2        |
       | 2  | T1        | Em andamento | 2          | 01/08  | 2        |
       | 3  | T2        | Em andamento | 2          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 4        | 1         | 4             |
	   | 02/08 | 2        |           |               |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 2     |       |
	   |           | realizado | 1     |       |
	   |           | restante  | 2     |       |
	   | T2        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	   | T3        | planejado | 2     |       |
	   |           | restante  | 2     |       |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 01/08  |	 
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.62 - Mover uma tarefa em andamento para o inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Em andamento | 3          | 02/08  | 4        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao       | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T3     | Em andamento   | 1            | 4        | Maria       | 7:00      | 8:00   |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Em andamento | 3          | 01/08  | 4        |
       | 2  | T1        | Nao iniciado | 2          | 01/08  | 2        |
       | 3  | T2        | Nao iniciado | 3          | 03/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        | 1         |               |
	   | 03/08 |          |           | 3             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T3        | planejado | 3     |       |       |
	   |           | realizado |       | 1     |       |
	   |           | restante  | 4     |       |       |
	   | T1        | planejado | 1     | 1     |       |
	   |           | restante  | 1     | 1     |       |
	   | T2        | planejado |       |       | 3     |
	   |           | restante  |       |       | 3     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T2        | 3     |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 01/08  |	 	   
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.63 - Aumentar a estimativa inicial de uma tarefa que possua tarefas em andamento na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 4          | 01/08  | 4        |
       | 2  | T2        | Em andamento | 3          | 01/08  | 3        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T2     | Em andamento | 3            | 3        | Jose        | 7:00      | 10:00  |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '6'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 6          | 01/08  |
       | 2  | T2        | Em andamento | 3          | 01/08  |	
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 2     | 3     | 1     |
	   |           | realizado |       |       |       |
	   |           | restante  | 2     | 3     | 1     |
	   | T2        | planejado | 3     | 0     |       |
	   |           | realizado | 3     |       |       |
	   |           | restante  | 0     |       | 3     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        | 3         | 2             |
	   | 02/08 | 3        |           | 3             |
	   | 03/08 |          |           | 4             |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T1        |  1    |
	   | T2        |  3    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	   
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.63 - Reduzir a estimativa inicial de uma tarefa que possua tarefas em andamento na sequencia
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
       | 2  | T2        | Em andamento | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 3          | 01/08  | 3        |
       | 2  | T2        | Em andamento | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 3     |       |
	   |           | restante  | 3     |       |
	   | T2        | planejado | 2     | 1     |
	   |           | restante  | 2     | 1     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        |           | 1             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.64 - Reduzir a estimativa restante de uma tarefa em andamento
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 5          | 01/08  | 5        |
       | 2  | T2        | Nao iniciado | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa restante alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 5          | 01/08  | 3        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 3     |       |
	   | T2        | planejado | 2     | 1     |
	   |           | restante  | 2     | 1     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        |           | 1             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.65 - Aumentar a estimativa restante de uma tarefa em andamento
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 3          | 01/08  | 3        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa restante alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 3          | 01/08  | 5        |
       | 2  | T2        | Nao iniciado | 3          | 02/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 3     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       | 3     |
	   |           | restante  |       | 3     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |           | 5             |
	   | 02/08 | 3        |           | 3             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.66 - Criar tarefa quando houverem tarefas em andamento na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  9    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 10         | 01/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao       | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T1     | Em andamento   | 10           | 2        | Jose        | 8:00      | 19:00  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T2           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	   | 2  | T1  		| Em andamento | 10         | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 10    |       |
	   |           | realizado | 10    |       |
	   |           | restante  | 0     | 2     |
	   | T2        | planejado |       | 7     |
	   |           | restante  |       | 7     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 10       | 10        | 0             |
	   | 02/08 | 7        | 0         | 9             |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.67 - Criar uma tarefa nao iniciada para apos uma tarefa em andamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  2    |
	   | 02/08 |  5    |
	   | 03/08 |  1    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
	   | 2  | T2        | Em andamento | 4          | 02/08  | 1        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T2     | Em andamento | 4            | 1        | Maria       | 7:00      | 11:00  |
	   | 03/08 | T2     | Em andamento | 1            | 1        | Maria       | 7:00      | 8:00   |	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 2      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
	   | 2  | T2        | Em andamento | 4          | 02/08  | 1        |
	   | 3  | T3        | Nao iniciado | 2          | 03/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 2        |           | 2             |
	   | 02/08 | 4        | 4         | 0             |
	   | 03/08 | 0        | 1         | 1             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 | 04/08 |
	   | T1        | planejado | 2     |       |       |       |
	   |           | restante  | 2     |       |       |       |
	   | T2        | planejado |       | 4     |       |       |
	   |           | realizado |       | 4     |       |       |
	   |           | restante  |       | 1     |       |       |
	   | T3        | planejado |       |       | 1     | 1     |
	   |           | restante  |       |       | 1     | 1     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 04/08 |
	   | T3        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.68 - Excluir tarefa quando houverem tarefas em andamento na sequencia com esforco real lancado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Em andamento | 3          | 02/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T3     | Em andamento | 3            | 2        | Jose        | 8:00      | 11:00  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T2  		| Em andamento | 3          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado |       |       |
	   |           | restante  |       |       |
	   | T2        | planejado |       | 3     |
	   |           | realizado |       | 3     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 0        |           |               |
	   | 02/08 | 3        | 3         | 2             |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.69 - Excluir tarefa quando houverem tarefas em andamento na sequencia sem esforco real existente
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Em andamento | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T2  		| Em andamento | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado |       |       |
	   |           | restante  |       |       |
	   | T2        | planejado | 3     |       |
	   |           | realizado |       |       |
	   |           | restante  | 3     |       |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |           | 3             |
	   | 02/08 |          |           |               |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   
										#// ----------------------------------------------- //
										#// CENARIOS DE TAREFAS ALTERNANDO ENTRE SITUACOES  //
										#// ----------------------------------------------- //	 	   
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.70 - Alterar a situacao de uma tarefa, sem esforco informado e com a situacao em andamento, para nao iniciado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Em andamento | 3          | 02/08  | 5        |
	   | 3  | T3        | Nao iniciada | 2          | 03/08  | 2        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Nao iniciada'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Nao iniciada | 5          | 02/08  | 5        |
	   | 3  | T3        | Nao iniciada | 2          | 03/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 5     |       |       |
	   |           | restante  | 5     |       |       |
	   | T2        | planejado |       | 5     |       |
	   |           | restante  |       | 5     |       |
	   | T3        | planejado |       |       | 2     |
	   |           | restante  |       |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 5        |           | 5             |
	   | 03/08 | 2        |           | 2             |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T3        | 2     |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	   
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.71 - Alterar a situacao de uma tarefa, sem esforco informado e com a situacao impedido, para nao iniciado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Impedida     | 3          | 02/08  | 2        |
	   | 3  | T3        | Nao iniciada | 2          | 02/08  | 2        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Nao iniciada'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Nao iniciada | 2          | 02/08  | 2        |
	   | 3  | T3        | Nao iniciada | 2          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	   | T3        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 4        |           | 4             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	   	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.72 - Alterar a situacao de uma tarefa, sem esforco informado e com a situacao cancelada, para nao iniciado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Em andamento | 5          | 01/08  | 4        |
       | 2  | T2        | Cancelada    | 5          | 02/08  | 0        |
	   | 3  | T3        | Em andamento | 2          | 01/08  | 3        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Nao iniciada'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 4        |
       | 2  | T2        | Nao iniciada | 5          | 01/08  | 5        |
	   | 3  | T3        | Em andamento | 2          | 02/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 5     |       |       |
	   |           | restante  | 4     |       |       |
	   | T2        | planejado | 1     | 4     |       |
	   |           | restante  | 1     | 4     |       |
	   | T3        | planejado |       | 1     | 2     |
	   |           | restante  |       | 1     | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 6        |           | 5             |
	   | 02/08 | 5        |           | 5             |
	   | 03/08 |          |           | 2             |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T3        | 2     |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   | T3  	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.73 - Alterar a situacao de uma tarefa pronta para em andamento
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Pronto       | 4          | 02/08  | 0        |
	   | 3  | T3        | Nao iniciada | 2          | 02/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 02/08 | T2     | Pronto       | 1            | 0        | Jose        | 8:00      | 11:00  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Em andamento'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Em andamento | 4          | 02/08  | 4        |
	   | 3  | T3        | Nao iniciada | 2          | 03/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 5     |       |       |
	   |           | restante  | 5     |       |       |
	   | T2        | planejado | 0     | 4     |       |
	   |           | realizado |       | 1     |       |
	   |           | restante  |       | 4     |       |
	   | T3        | planejado |       |       | 2     |
	   |           | restante  |       |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 5        | 1         | 4             |
	   | 03/08 |          |           | 2             |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T3        | 2     |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 03/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.75 - Alterar a situacao de uma tarefa, sem esforco informado e com a situacao impedido, para cancelado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Impedida     | 3          | 02/08  | 5        |
	   | 3  | T3        | Nao iniciada | 2          | 03/08  | 2        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Cancelada'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Cancelada    | 3          | 02/08  | 0        |
	   | 3  | T3        | Nao iniciada | 2          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       |       |
	   |           | restante  |       |       |
	   | T3        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 2        |           | 2             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 02/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.76 - Alterar a situacao de uma tarefa, com esforco informado e com a situacao impedido, para cancelado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Impedida     | 3          | 02/08  | 3        |
	   | 3  | T3        | Nao iniciada | 2          | 03/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 02/08 | T2     | Impedida     | 2            | 3        | Jose        | 8:00      | 10:00  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2' com a situacao alterada para 'Cancelada'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Cancelada    | 3          | 02/08  | 0        |
	   | 3  | T3        | Nao iniciada | 2          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       |       |
	   |           | realizado |       | 2     |
	   |           | restante  |       |       |
	   | T3        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 2        | 3         | 2             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T3  	   | 02/08  |	   
	   
										#// ------------------------------------------------- //
										#// CENARIOS DE TAREFAS COM A SITUACAO DE IMPEDIMENTO //
										#// ------------------------------------------------- //	 
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.77 - Mover tarefa para o inicio da lista quando houverem tarefas impedidas
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 2          | 01/08  | 4        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Nao iniciado | 1          | 02/08  | 1        |	 
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Nao iniciado | 1          | 01/08  | 1        |
       | 2  | T1        | Impedido     | 2          | 01/08  | 4        |
       | 3  | T2        | Nao iniciado | 3          | 02/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |          | 5             |
	   | 02/08 | 2        |          | 2             |
	   | 03/08 | 1        |          | 1             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T3        | planejado | 1     |       |       |
	   |           | restante  | 1     |       |       |
	   | T1        | planejado | 2     |       |       |
	   |           | restante  | 4     |       |       |
	   | T2        | planejado |       | 2     | 1     |
	   |           | restante  |       | 2     | 1     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T2        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 01/08  |

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.78 - Mover tarefa para o inicio da lista quando houverem tarefas impedidas que consomem o dia inteiro
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 2          | 01/08  | 2        |
       | 2  | T2        | Impedido     | 2          | 01/08  | 2        |
	   | 3  | T3        | Nao iniciado | 2          | 02/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 01/08 | T1     | Impedido     | 1            | 2        | Maria       | 7:00      | 8:00   |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Nao iniciado | 2          | 01/08  | 2        |
       | 2  | T1        | Impedido     | 2          | 01/08  | 2        |
       | 3  | T2        | Impedido     | 2          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 4        | 1         | 4             |
	   | 02/08 | 2        |           |               |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 2     |       |
	   |           | realizado | 1     |       |
	   |           | restante  | 2     |       |
	   | T2        | planejado |       | 2     |
	   |           | restante  |       | 2     |
	   | T3        | planejado | 2     |       |
	   |           | restante  | 2     |       |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   | T3  	   | 01/08  |	

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.79 - Mover uma tarefa impedida para o inicio da lista
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  2    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	   | 3  | T3        | Impedido     | 3          | 02/08  | 4        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao       | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T3     | Impedido       | 1            | 4        | Maria       | 7:00      | 8:00   |
Quando o cronograma 'C1' tiver a tarefa 'T3' movida para a posicao '1' pelo colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T3        | Impedido     | 3          | 01/08  | 4        |
       | 2  | T1        | Nao iniciado | 2          | 01/08  | 2        |
       | 3  | T2        | Nao iniciado | 3          | 03/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        | 1         |               |
	   | 03/08 |          |           | 3             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T3        | planejado | 3     |       |       |
	   |           | realizado |       | 1     |       |
	   |           | restante  | 4     |       |       |
	   | T1        | planejado | 1     | 1     |       |
	   |           | restante  | 1     | 1     |       |
	   | T2        | planejado |       |       | 3     |
	   |           | restante  |       |       | 3     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T2        | 3     |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 03/08  |
	   | T3  	   | 01/08  |	

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.80 - Aumentar a estimativa inicial de uma tarefa que possua tarefas impedidas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 4          | 01/08  | 4        |
       | 2  | T2        | Impedido     | 3          | 01/08  | 3        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T2     | Impedido     | 3            | 3        | Jose        | 7:00      | 10:00  |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '6'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio |
       | 1  | T1        | Nao iniciado | 6          | 01/08  |
       | 2  | T2        | Impedido     | 3          | 01/08  |	
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 |
	   | T1        | planejado | 2     | 3     | 1     |
	   |           | realizado |       |       |       |
	   |           | restante  | 2     | 3     | 1     |
	   | T2        | planejado | 3     | 0     |       |
	   |           | realizado | 3     |       |       |
	   |           | restante  | 0     |       | 3     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        | 3         | 2             |
	   | 02/08 | 3        |           | 3             |
	   | 03/08 |          |           | 4             |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 03/08 |
	   | T1        |  1    |
	   | T2        |  3    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.81 - Reduzir a estimativa inicial de uma tarefa que possua tarefas impedidas na sequencia
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 5          | 01/08  | 5        |
       | 2  | T2        | Impedido     | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa inicial alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 3          | 01/08  | 3        |
       | 2  | T2        | Impedido     | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 3     |       |
	   |           | restante  | 3     |       |
	   | T2        | planejado | 2     | 1     |
	   |           | restante  | 2     | 1     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        |           | 1             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |	 

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.82 - Reduzir a estimativa restante de uma tarefa impedida
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 5          | 01/08  | 5        |
       | 2  | T2        | Nao iniciado | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa restante alterada para '3'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 5          | 01/08  | 3        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 5     |       |
	   |           | restante  | 3     |       |
	   | T2        | planejado | 2     | 1     |
	   |           | restante  | 2     | 1     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 5        |           | 5             |
	   | 02/08 | 1        |           | 1             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.83 - Aumentar a estimativa restante de uma tarefa impedida
   Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  3    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 3          | 01/08  | 3        |
       | 2  | T2        | Nao iniciado | 3          | 01/08  | 3        |
Quando o cronograma 'C1' tiver a tarefa 'T1' com a estimativa restante alterada para '5'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 3          | 01/08  | 5        |
       | 2  | T2        | Nao iniciado | 3          | 02/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 3     |       |
	   |           | restante  | 5     |       |
	   | T2        | planejado |       | 3     |
	   |           | restante  |       | 3     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |           | 5             |
	   | 02/08 | 3        |           | 3             |
	 E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 02/08  |
	   
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.84 - Criar tarefa quando houverem tarefas impedidas na sequencia
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  10   |
	   | 02/08 |  9    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Impedido     | 10         | 01/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao       | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T1     | Impedido       | 10           | 2        | Jose        | 8:00      | 19:00  |
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
	   | posicao	| 1			   |
       | descricao  | T2           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 7      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	   | 2  | T1  		| Impedido     | 10         | 01/08  |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado | 10    |       |
	   |           | realizado | 10    |       |
	   |           | restante  | 0     | 2     |
	   | T2        | planejado |       | 7     |
	   |           | restante  |       | 7     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 10       | 10        | 0             |
	   | 02/08 | 7        | 0         | 9             |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T2  		| Nao iniciado | 7          | 02/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio	   
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.85 - Criar uma tarefa nao iniciada para apos uma tarefa impedida
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  2    |
	   | 02/08 |  5    |
	   | 03/08 |  1    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
	   | 2  | T2        | Impedido     | 4          | 02/08  | 1        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
       | 02/08 | T2     | Impedido     | 4            | 1        | Maria       | 7:00      | 11:00  |
	   | 03/08 | T2     | Impedido     | 1            | 1        | Maria       | 7:00      | 8:00   |	 
Quando o cronograma 'C1' tiver uma tarefa criada pelo colaborador 'Joao' conforme a seguir:
       | descricao  | T3           | 
	   | situacao   | Nao iniciado | 
	   | estimativa | 2      	   |
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciado | 2          | 01/08  | 2        |
	   | 2  | T2        | Impedido     | 4          | 02/08  | 1        |
	   | 3  | T3        | Nao iniciado | 2          | 03/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 2        |           | 2             |
	   | 02/08 | 4        | 4         | 0             |
	   | 03/08 | 0        | 1         | 1             |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 | 03/08 | 04/08 |
	   | T1        | planejado | 2     |       |       |       |
	   |           | restante  | 2     |       |       |       |
	   | T2        | planejado |       | 4     |       |       |
	   |           | realizado |       | 4     |       |       |
	   |           | restante  |       | 1     |       |       |
	   | T3        | planejado |       |       | 1     | 1     |
	   |           | restante  |       |       | 1     | 1     |
	 E o cronograma 'C1' deve possuir tarefas causando desvio conforme a seguir:
	   | descricao | 04/08 |
	   | T3        |  1    |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que foram criadas as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio |
	   | 1  | T3  		| Nao iniciado | 1          | 03/08  |
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio

@pbi_3.02.02.01, @rf_3.03
Cenario: 01.86 - Excluir tarefa quando houverem tarefas impedidas na sequencia com esforco real lancado
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Impedido     | 3          | 02/08  | 2        |
	 E que o cronograma 'C1' possui o esforco realizado a seguir:
	   | data  | tarefa | situacao     | esforco real | restante | colaborador | hr inicio | hr fim |
	   | 01/08 | T3     | Impedido     | 3            | 2        | Jose        | 8:00      | 11:00  |	 
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T2  		| Impedido     | 3          | 02/08  | 2        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado |       |       |
	   |           | restante  |       |       |
	   | T2        | planejado |       | 3     |
	   |           | realizado |       | 3     |
	   |           | restante  |       | 2     |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 0        |           |               |
	   | 02/08 | 3        | 3         | 2             |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' nao deve ser comunicado que houveram mudancas de data de inicio
	 
@pbi_3.02.02.01, @rf_3.03
Cenario: 01.87 - Excluir tarefa quando houverem tarefas impedidas na sequencia sem esforco real existente
  Dado que o cronograma 'C1' possui a capacidade de planejamento a seguir:
       |  dia  | horas |
       | 01/08 |  5    |
	   | 02/08 |  5    |
	 E que o cronograma 'C1' possui as seguintes tarefas:
	   | id | descricao | situacao     | estimativa | inicio | restante |
       | 1  | T1        | Nao iniciada | 5          | 01/08  | 5        |
       | 2  | T2        | Impedido     | 3          | 02/08  | 3        |
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' deve possuir as tarefas a seguir:
       | id | descricao | situacao     | estimativa | inicio | restante |
	   | 1  | T2  		| Impedido     | 3          | 01/08  | 3        |
	 E o cronograma 'C1' deve possuir a alocacao de horas por tarefa conforme a seguir:
	   | tarefa    | tipo      | 01/08 | 02/08 |
	   | T1        | planejado |       |       |
	   |           | restante  |       |       |
	   | T2        | planejado | 3     |       |
	   |           | realizado |       |       |
	   |           | restante  | 3     |       |
	 E o cronograma 'C1' deve possuir a alocacao de horas por dia conforme a seguir:
	   | dia   | hrs plan | hrs reais | hrs restantes |
	   | 01/08 | 3        |           | 3             |
	   | 02/08 |          |           |               |
     E o cronograma 'C1' nao deve possuir tarefas causando desvio
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve ser comunicado que houveram as mudancas de data a seguir:
       | descricao | inicio |
	   | T2  	   | 01/08  |
	 
	 # /*Tema 2: Calculo do início planejado das tarefas 
	 # [Ok] - Ao criar uma nova tarefa no fim do cronograma
	 # [Ok] Ao criar uma tarefa no início do cronograma
	  #[Ok] Ao criar uma tarefa meio do cronograma
	  #[Ok] Ao excluir uma tarefa
	  #[Ok] Ao mo1ver uma tarefa para o início
	  #[Ok] Ao mover uma tarefa para o meio
	  #[Ok] Ao mover uma tarefa para o fim
	  #[Ok] Ao alterar a estimativa inicial de uma tarefa	

	  #[Ok] Ao criar uma nova tarefa quando houver tarefas impactadas prontas
	  #[Ok] Ao excluir uma tarefa quando houver tarefas impactadas prontas
	  #[Ok] Ao mover uma tarefa para início, meio e fim quando houver tarefas impactadas prontas
	  #[Ok] Ao alterar a estimativa inicial de uma tarefa impactando em tarefas que ja estao prontas
	  #[Ok] Mover uma tarefa pronta
	  #[Ok] Mover a tarefa para apos a tarefa pronta

	 #[Ok] Ao criar uma nova tarefa quando houver tarefas impactadas canceladas
	 #[Ok] Ao criar uma tarefa no início ou meio do cronograma quando houver tarefas impactadas canceladas
	 #[Ok] Ao excluir uma tarefa quando houver tarefas impactadas canceladas
	 #[Ok] Ao mover uma tarefa para início, meio e fim quando houver tarefas impactadas canceladas
	 #[Ok] Mover uma tarefa cancelada e verificar se a data de inicio sera mantida
	 #[Ok] Excluir uma tarefa cancelada
	
	 #[Ok] Ao criar uma nova tarefa quando houver tarefas impactadas em andamento
	 #[Ok] Ao criar uma tarefa no início ou meio do cronograma quando houver tarefas impactadas em andamento
	 #[Ok] Ao excluir uma tarefa quando houver tarefas impactadas em andamento
	 #[Ok] Ao mover uma tarefa para início, meio e fim quando houver tarefas impactadas em andamento
	 #[Ok] Ao alterar a estimativa inicial de uma tarefa impactando em tarefas que ja estao em andamento

	 #[Ok] Alterar a situacao para nao iniciado (Cancelado, Pronto, Em Andamento, Impedido).
	
	 #[Ok] Ao criar uma nova tarefa quando houver tarefas impactadas impedidas
	 #[Ok] Ao criar uma tarefa no início ou meio do cronograma quando houver tarefas impactadas impedidas
	 #[Ok] Ao excluir uma tarefa quando houver tarefas impactadas impedidas
	 #[Ok] Ao mover uma tarefa para início, meio e fim quando houver tarefas impactadas impedidas
	 #[Ok] Ao alterar a estimativa inicial de uma tarefa impactando em tarefas que ja estao impedidas
	
	 #[Ok] Ao criar uma nova tarefa no fim do cronograma que caia em um dia nao util
	 #[Ok] Ao criar uma tarefa no início ou meio do cronograma que caia em um dia nao util
	 #[Ok] Ao excluir uma tarefa que impacte em tarefas puxadas de dias nao uteis
	 #[Ok] Ao mover uma tarefa para início, meio e fim que caia em um dia nao util
	 #[Ok] Ao alterar a estimativa inicial de uma tarefa que caia em um dia nao util
	
	 #Ao criar uma nova tarefa com data fixa no fim do cronograma
	 #Ao criar uma tarefa com data fixa no início ou meio do cronograma
	 #Ao excluir uma tarefa com data fixa 
	 #Ao mover uma tarefa com data fixa para início, meio e fim
	 #Ao alterar a estimativa inicial de uma tarefa com data fixa
	 #Ao alterear uma tarefa normal para data fixa

	 #Ao criar uma nova tarefa em um dia que o horario seja diferenciado
	 #Ao criar uma tarefa no início ou meio do cronograma em um dia que o horario seja diferenciado
	 #Ao excluir uma tarefa de um dia que o horario seja diferenciado
	 #Ao mover uma tarefa para início, meio e fim a partir de um dia em que o horario seja diferenciado
	 #Ao mover uma tarefa para início, meio e fim para de um dia em que o horario seja diferenciado
	
	 #Com milestone - nao influencia no calculo*/

