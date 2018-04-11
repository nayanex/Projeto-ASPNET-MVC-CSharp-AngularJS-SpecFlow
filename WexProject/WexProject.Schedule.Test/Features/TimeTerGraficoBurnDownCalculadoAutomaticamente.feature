#language: pt-br

@Burndown
Funcionalidade: Time ter grafico de burndown de um planejamento de tarefas calculado automaticamente

Contexto: 
	Dado que exista(m) a(s) situacao(oes) de planejamento a seguir:
		 | tipo         | nome         | situacao | padrao |
		 | Planejamento | Não Iniciado | Ativo    | Sim    |
		 | Cancelamento | Cancelado    | Ativo    | Não    |
		 | Encerramento | Pronto       | Ativo    | Não    |
		 | Impedimento  | Impedido     | Ativo    | Não    |
		 | Execução     | Em Andamento | Ativo    | Não    |

@Burndown
Cenario: 01.01 - Calcular o burndown quando nao houver tarefas planejadas em um periodo sem finais de semana
  Dado que exista(m) o(s) cronograma(s)
         | nome | inicio   | final    |
         | C1   | 05/05/14 | 09/05/14 |
Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 |           |          |
       | 06/05/14 |           |          |
       | 07/05/14 |           |          |
       | 08/05/14 |           |          |
       | 09/05/14 |           |          |
	   
@Burndown
Cenario: 01.02 - Calcular o burndown quando houver tarefas planejadas em um periodo sem finais de semana
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '04/05/14'
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 9          		|
       | 2  | T2        | Planejamento | 7          		|
       | 3  | T3        | Planejamento | 5          		|
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 21        |          |
       | 06/05/14 | 15,75     |          |
       | 07/05/14 | 10,5      |          |
       | 08/05/14 | 5,25      |          |
       | 09/05/14 | 0         |          |

@Burndown
Cenario: 01.03 - Calcular o burndown quando houver tarefas planejadas em um periodo com final de semana
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 12/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10                 |
       | 2  | T2        | Planejamento | 7                  |
       | 3  | T3        | Planejamento | 10                 |
       | 4  | T4        | Planejamento | 3                  |
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 30        | 30       |
       | 06/05/14 | 24        |          |
       | 07/05/14 | 18        |          |
       | 08/05/14 | 12        |          |
       | 09/05/14 | 6         |          |
       | 12/05/14 | 0         |          |
	   
@Burndown
Cenario: 01.04 - Calcular o burndown quando houver tarefas planejadas em um periodo com calendario institucional definindo um sabado como dia util
	Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 12/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10          		|
       | 2  | T2        | Planejamento | 7          		|
       | 3  | T3        | Planejamento | 10          		|
       | 4  | T4        | Planejamento | 3          		|
     E que o calendario institucional possui as seguintes definicoes:
       | Vigencia 			| Data     | Descricao  | Tipo     | Situacao |
       | Por dia, mes e ano | 10/05/14 | Hora Extra | Trabalho | Ativo    |
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 |    30     |    30    |
       | 06/05/14 |    25     |          |
       | 07/05/14 |    20     |          |
       | 08/05/14 |    15     |          |
       | 09/05/14 |    10     |          |
       | 10/05/14 |     5     |          |
       | 12/05/14 |     0     |          |
	   	   
@Burndown
Cenario: 01.05 - Calcular o burndown quando houver tarefas planejadas em um periodo com feriados
	Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '05/05/14'
     E que o calendario institucional possui as seguintes definicoes:
       | Vigencia 			| Data     | Descricao | Tipo     | Situacao |
       | Por dia do mes     | 07/05    | Folga 01  | Folga    | Ativo    |
       | Por dia, mes e ano | 08/05/14 | Folga 02  | Folga    | Inativo  |
       | Por dia, mes e ano | 09/05/14 | Folga 03  | Folga    | Ativo    |
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10          		|
       | 2  | T2        | Planejamento | 7          		|
       | 3  | T3        | Planejamento | 10          		|
       | 4  | T4        | Planejamento | 3          		|
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 30        | 30       |
       | 06/05/14 | 15        |          |
       | 08/05/14 | 0         |          |
	   
@Burndown
Cenario: 01.06 - Calcular o burndown quando nao houver historico de trabalho apos o inicio da execucao
 Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 12/05/14 |
     E que o dia atual seja '05/05/14'
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10                 |
       | 2  | T2        | Planejamento | 7                  |
       | 3  | T3        | Planejamento | 10                 |
       | 4  | T4        | Execução     | 3                  |
     E que o dia atual seja '07/05/14'
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 |    30     |    30    |
       | 06/05/14 |    24     |    30    |
       | 07/05/14 |    18     |    30    |
       | 08/05/14 |    12     |          |
       | 09/05/14 |     6     |          |
       | 12/05/14 |     0     |          |
     E o cronograma 'C1' deve ter um desvio de -12 horas	 

#//TODO: Criar cenário que altere a estimativa inicial sem histórico de trabalho
@Burndown
Cenario: 02.01 - Calcular o burndown quando houver historico de trabalho em um final de semana
 Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 02/05/14 | 12/05/14 |
     E que o dia atual seja '02/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10          		|
       | 2  | T2        | Planejamento | 7          		|
       | 3  | T3        | Planejamento | 10          		|
       | 4  | T4        | Planejamento | 3          		|
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data     | tarefa | esforco realizado | estimativa restante |
       | 03/05/14 | T3     | 2                 | 8                   |
     E que o dia atual seja '05/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 02/05/14 | 30        | 30       |
       | 03/05/14 |           | 28       |
       | 05/05/14 | 25        | 28       |
       | 06/05/14 | 20        |          |
       | 07/05/14 | 15        |          |
       | 08/05/14 | 10        |          |
       | 09/05/14 | 5         |          |
       | 12/05/14 | 0         |          |
		   
@Burndown
Cenario: 02.02 - Calcular o burndown quando houver historico de trabalho em um feriado
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 02/05/14 | 13/05/14 |
     E que o dia atual seja '02/05/14'	   
     E que o calendario institucional possui as seguintes definicoes:
       | Vigencia 			| Data     | Descricao | Tipo     | Situacao |
       | Por dia, mes e ano | 05/05/14 | Folga 01  | Folga    | Ativo    |
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10          		|
       | 2  | T2        | Planejamento | 7          		|
       | 3  | T3        | Planejamento | 10          		|
       | 4  | T4        | Planejamento | 3          		|
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data     | tarefa | esforco realizado | estimativa restante |
       | 05/05/14 | T3     | 2                 | 5                   |
     E que o dia atual seja '06/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 02/05/14 | 30        | 30       |
       | 05/05/14 |           | 25       |
       | 06/05/14 | 25        | 25       |
       | 07/05/14 | 20        |          |
       | 08/05/14 | 15        |          |
       | 09/05/14 | 10        |          |
       | 12/05/14 | 5         |          |
       | 13/05/14 | 0         |          |

#cenário com histórico de trabalho na semana? Cenário feliz
#cenario tarefa cancelada
@alexandre
#cenário quando a sprint estiver o seu primeiro dia todo estimado e com horas realizadas e no segundo dia da sprint novas tarefas sejam criadas @alexandre
		   
@Burndown
Cenario: 03.01 - Calcular o burndown quando houver tarefas com estimativas restantes causando um desvio negativo
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Planejamento | 10                 |
       | 2  | T2        | Planejamento | 7                  |
       | 3  | T3        | Execução     | 10                 |
       | 4  | T4        | Execução     | 3                  |
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data     | tarefa | esforco realizado | estimativa restante |
       | 05/05/14 | T3     | 2                 | 8                   |
       | 06/05/14 | T3     | 8                 | 5                   |
       | 07/05/14 | T4     | 4                 | 1                   |
     E que o dia atual seja '07/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 28        | 28       |
       | 06/05/14 | 21        | 25       |
       | 07/05/14 | 14        | 23       |
       | 08/05/14 | 7         |          |
       | 09/05/14 | 0         |          |
     E o cronograma 'C1' deve ter um desvio de -9 horas	  

@Burndown
Cenario: 03.02 - Calcular o burndown quando houver tarefas com estimativas restantes causando um desvio positivo
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Execução     | 10                 |
       | 2  | T2        | Execução     | 7                  |
       | 3  | T3        | Planejamento | 10                 |
       | 4  | T4        | Planejamento | 3                  |
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data           | tarefa | esforco realizado | estimativa restante |
       | 06/05/14 10:00 | T1     | 1                 | 8                   |
       | 06/05/14 14:30 | T1     | 3                 | 2                   |
       | 06/05/14 8:00  | T2     | 10                | 1                   |
     E que o dia atual seja '06/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 30        | 30       |
       | 06/05/14 | 22,5      | 16       |
       | 07/05/14 | 15        |          |
       | 08/05/14 | 7,5       |          |
       | 09/05/14 | 0         |          |
     E o cronograma 'C1' deve ter um desvio de 6,5 horas

@Burndown
Cenario: 03.03 - Calcular o burndown quando zerar as estimativas restantes antes do ultimo dia
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Encerramento | 10                 |
       | 2  | T2        | Encerramento | 7                  |
       | 3  | T3        | Encerramento | 10                 |
       | 4  | T4        | Encerramento | 3                  |
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data           | tarefa | esforco realizado | estimativa restante |
       | 06/05/14 10:00 | T1     | 1                 | 0                   |
       | 06/05/14 11:01 | T2     | 10                | 0                   |
       | 07/05/14 22:00 | T3     | 5                 | 0                   |
       | 08/05/14 00:01 | T4     | 20                | 0                   |
     E que o dia atual seja '09/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 30        | 30       |
       | 06/05/14 | 22,5      | 13       |
       | 07/05/14 | 15        | 3        |
       | 08/05/14 | 7,5       | 0        |
       | 09/05/14 | 0         |          |
     E o cronograma 'C1' deve ter um desvio de 0 horas

@Burndown
Cenario: 03.04 - Calcular quando ainda houver as estimativas restantes apos o ultimo dia
  Dado que exista(m) o(s) cronograma(s)
       | nome | inicio   | final    |
       | C1   | 05/05/14 | 09/05/14 |
     E que o dia atual seja '05/05/14'	   
     E que o cronograma 'C1' possui as seguintes tarefas:
       | id | descricao | situacao     | estimativa inicial |
       | 1  | T1        | Execução     | 10                 |
       | 2  | T2        | Encerramento | 7                  |
       | 3  | T3        | Encerramento | 10                 |
       | 4  | T4        | Encerramento | 3                  |
     E que o cronograma 'C1' possui o seguinte historico de trabalho:
       | data           | tarefa | esforco realizado | estimativa restante |
       | 06/05/14 10:00 | T1     | 1                 | 5                   |
       | 06/05/14 11:01 | T2     | 10                | 0                   |
       | 07/05/14 22:00 | T3     | 5                 | 0                   |
	   | 08/05/14 00:01 | T4     | 20                | 0                   |
     E que o dia atual seja '10/05/14'	   
 Quando o grafico burndown for calculado para o cronograma 'C1'
 Entao o grafico de burndown deve conter os seguintes valores para o cronograma 'C1':
       | Data     | Planejado | Restante |
       | 05/05/14 | 30        | 30       |
       | 06/05/14 | 22,5      | 18       |
       | 07/05/14 | 15        | 8        |
       | 08/05/14 | 7,5       | 5        |
       | 09/05/14 | 0         | 5        |
     E o cronograma 'C1' deve ter um desvio de -5 horas
