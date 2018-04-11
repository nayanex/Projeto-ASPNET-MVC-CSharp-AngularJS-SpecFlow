#language: pt-br

@pbi_4.05
Funcionalidade: O Time de Desenvolvimento poder editar o cronograma em paralelo
             Como um Time de Desenvolvimento
             Gostaria de poder editar o cronograma em paralelo
             Então, eu poderei ter várias pessoas editando o mesmo cronograma ao mesmo tempo com um comportamento
             semelhante ao do GoogleDocs

Contexto:
Dado que exista(m) a(s) situacao(oes) de planejamento a seguir:
	 | tipo         | nome         | situacao | padrao |
	 | Planejamento | Não Iniciado | Ativo    | Sim    |
	 | Cancelamento | Cancelado    | Ativo    | Não    |
	 | Encerramento | Pronto       | Ativo    | Não    |
	 | Impedimento  | Impedido     | Ativo    | Não    |
	 | Execução     | Em Andamento | Ativo    | Não    |
   E que exista(m) o(s) cronograma(s) 
	 | nome |
	 | C1   |
	 | C2   |
   E que existam os colaboradores:
	 | colaborador | superior | admissao   |
	 | Joao        |          | 01/01/2011 | 
	 | Maria       |          | 01/01/2011 |
	 | Jose        |          | 01/01/2011 |
	 | Pedro       |          | 01/01/2011 |

@rf_3.03 @rn
Cenario: 01 - Um usuario tentar abrir um cronograma com servidor desligado
  Dado que o servidor esta desligado
Quando o cronograma 'C1' for aberto pelo colaborador 'Joao'
 Entao o servidor nao deve possuir o colaborador 'Joao'  em sua lista de usuarios conectados ao cronograma 'C1'
     E o cronograma 'C1' ser comunicado de que nao houve sucesso na conexao com o servidor para o colaborador 'Joao' 	 

@rf_3.03 @rn @rn_11.1.5 @rn_11.1.2
Cenario: 02 - Um usuario abrir um cronograma e os outros usuarios ja conectados no cronograma serem comunicados
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Maria', 'Jose' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' for aberto pelo colaborador 'Joao' 	 
 Entao o servidor deve ser comunicado que que o(s) colaborador(es) 'Joao' esta(o) conectado(s) ao cronograma 'C1'
     E o servidor devera conter na lista de colaboradores conectados o colaborador 'Joao' no cronograma 'C1'
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve(m) ser comunicado(s) que o colaborador 'Joao' ficou online

@rf_3.03 @rn @rn_11.1.5 @rn_11.1.2
Cenario: 03 - Um usuario se desconectar do cronograma
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria', 'Jose' conectado(s) no cronograma 'C1'
	 E que o servidor contenha o(s) colaborador(es) 'Pedro' conectado(s) no cronograma 'C2'
Quando o cronograma 'C1' tiver o(s) colaborador(es) 'Joao' desconectado(s)
 Entao o servidor deve ser comunicado que o colaborador 'Joao' se desconectou
	 E o servidor nao devera ter em sua lista de colaboradores online no cronograma 'C1' o(s) colaborador(es) 'Joao'
	 E o cronograma 'C1' do(s) colaborador(es) 'Maria', 'Jose' deve(m) ser comunicado(s) que o colaborador 'Joao' se desconectou
	 E o cronograma 'C2' do(s) colaborador(es) 'Pedro' nao deve(m) ser comunicado(s) que o colaborador 'Joao' se desconectou
     
@rf_3.0.3 @rn @rn_11.1.5 @rn_11.1.2
Cenario: 04 - Um usuario estar com um cronograma aberto e o servidor cair
   Dado que o servidor esta ligado
   E que o servidor contenha o(s) colaborador(es) 'Joao','Pedro' conectado(s) no cronograma 'C1'
 Quando o servidor desconectar
  Então o cronograma 'C1' do(s) colaborador(es) 'Joao','Pedro' deve(m) ser comunicado(s) que o servidor desconectou

@rf_3.03 @rn @rn_11.1.1 @rn_11.1.6
Cenario: 05 - Dois usuarios criarem tarefas no mesmo cronograma
    Dado que o servidor esta ligado
	   E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
  Quando o cronograma 'C1' tiver uma tarefa 'T1' criada pelo colaborador 'Joao' 
	   E o cronograma 'C1' tiver uma tarefa 'T2' criada pelo colaborador 'Maria'
   Entao o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a tarefa 'T1' foi criada com o ID '1'
	   E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Joao' de que a tarefa 'T2' foi criada com o ID '2'

@rf_3.03, @rn @rn_11.1.1 @rn_11.1.6
Cenario: 06 - Dois usuarios criarem tarefas em cronogramas diferentes
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao' conectado(s) no cronograma 'C1'
	 E que o servidor contenha o(s) colaborador(es) 'Maria' conectado(s) no cronograma 'C2'
     E que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
	 | 2  | T2        |
	 | 3  | T3        |
	 | 4  | T4        |
	   E que o cronograma 'C2' possui as seguintes tarefas criadas pelo colaborador 'Maria':
     | id | descricao |
     | 1  | T1        |
	 | 2  | T2        |
	 | 3  | T3        |
	 | 4  | T4        |
Quando o cronograma 'C1' tiver uma tarefa 'T5' criada pelo colaborador 'Joao' na posicao '2'
	 E o cronograma 'C2' tiver uma tarefa 'T6' criada pelo colaborador 'Maria' na posicao '3'
 Entao o cronograma 'C2' nao deve comunicar ao(s) colaborador(es) 'Maria' de que no cronograma 'C1' foi criada a tarefa 'T5'
     E o cronograma 'C2' devera ter as tarefas visualizadas pelo colaborador 'Maria' na seguinte ordem:
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T6        |
     | 4  | T3        |
	 | 5  | T4        |
	 E o cronograma 'C1' nao deve comunicar ao(s) colaborador(es) 'Joao' de que no cronograma 'C2' foi criada a tarefa 'T6'
	 E o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Joao' na seguinte ordem:
     | id | descricao |
     | 1  | T1        |
     | 2  | T5        |
     | 3  | T2        |
     | 4  | T3        |
	 | 5  | T4        |

@rf_3.03, @rn @rn_11.1.4 @rn_11.1.7
Cenario: 07 - O usuario comecar a editar uma tarefa em um cronograma
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
 Entao o servidor deve ser comunicado que a tarefa 'T1' do cronograma 'C1' esta sendo editada pelo colaborador 'Joao'
 E o cronograma 'C1' deve comunicar o colaborador 'Joao' de que a tarefa 'T1' foi permitida para edicao
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a tarefa 'T1' esta sendo editada pelo colaborador 'Joao'

@rf_3.03 @rn @rn_11.1.4 @rn_11.1.7
Cenario: 08 - O usuario ser comunicado de que outro usuario esta editando tarefas diferentes
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
     E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Maria'
 Entao o servidor deve ser comunicado que a tarefa 'T2' do cronograma 'C1' esta sendo editada pelo colaborador 'Maria'
	 E o cronograma 'C1' deve comunicar o colaborador 'Maria' de que a tarefa 'T2' foi permitida para edicao
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Joao' de que a tarefa 'T2' esta sendo editada pelo colaborador 'Maria'
	 
@rf_3.03 @rn @rn_11.1.4 @rn_11.1.7
Cenario: 09 - O usuário tentar editar uma tarefa que ja esta sendo editada por outro usuario
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Maria'
 Entao o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que foi recusada a edicao da a tarefa 'T1' pois a mesma esta sendo editada pelo colaborador 'Joao'
	 E o cronograma 'C1' deve comunicar o colaborador 'Joao' de que a tarefa 'T1' foi permitida para edicao

@rf_3.03 @rn @rn_11.1.4 @rn_11.1.7
Cenario: 10 - Dois usuarios concluirem a edicao de tarefas diferentes no mesmo cronograma
   Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        | 
     | 2  | T2        |
     E que o servidor esta ligado
     E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T1' editada pelo colaborador 'Joao' 
     E o cronograma 'C1' tiver a tarefa 'T2' editada pelo colaborador 'Maria'
 Entao o servidor devera ser comunicado de que a(s) tarefa(s) 'T1', 'T2' estao liberadas para edicao
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Joao' de que a(s) tarefa(s) 'T2' recebeu(ram) atualizacao(oes)
	 E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a(s) tarefa(s) 'T1' recebeu(ram) atualizacao(oes)

@rf_3.03 @rn @rn_11.1.4 @rn_11.1.7
Cenario: 11 - Dois usuarios editarem tarefas em cronogramas diferentes
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o cronograma 'C2' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 2  | T2        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao' conectado(s) no cronograma 'C1'
	 E que o servidor contenha o(s) colaborador(es) 'Maria' conectado(s) no cronograma 'C2'
Quando o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C2' tiver a tarefa 'T2' em edicao pelo colaborador 'Maria'
 Entao o servidor deve ser comunicado que a tarefa 'T1' do cronograma 'C1' esta sendo editada pelo colaborador 'Joao'
	 E o servidor deve ser comunicado que a tarefa 'T2' do cronograma 'C2' esta sendo editada pelo colaborador 'Maria'
	 E o servidor nao deve comunicar ao(s) colaborador(es) 'Joao' de que a(s) tarefa(s) 'T2' recebeu(ram) atualizacao(oes) no cronograma 'C2'
	 E o servidor nao deve comunicar ao(s) colaborador(es) 'Maria' de que a(s) tarefa(s) 'T1' recebeu(ram) atualizacao(oes) no cronograma 'C1'

@rf_3.03 @rn @rn_11.1.9
Cenario: 12 - Dois usuarios excluirem tarefas diferentes no mesmo cronograma
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
	 | 3  | T3        |
	 | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1','T3' solicitada(s) para exclusao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a(s) tarefa(s) 'T2' solicitada(s) para exclusao pelo colaborador 'Maria'
 Entao o servidor devera receber a solicitacao para excluir a(s) tarefa(s) 'T1','T2','T3'  no cronograma 'C1'
	 E o cronograma 'C1' devera comunicar o servidor de que a(s) tarefa(s) 'T1','T3' foram excluidas pelo colaborador 'Joao' e que a(s) tarefas foram reorganizadas na seguinte ordem:
	 | id | descricao |
     | 1  | T2        |
	 | 2  | T4        |
	  E o cronograma 'C1' devera comunicar o servidor de que a(s) tarefa(s) 'T2' foram excluidas pelo colaborador 'Maria' e que a(s) tarefas foram reorganizadas na seguinte ordem:
	 | id | descricao |
     | 1  | T4        |
	 E o cronograma 'C1' devera comunicar ao colaborador 'Maria' da exclusao da(s) tarefa(s) 'T1','T3'
	 E o cronograma 'C1' devera comunicar ao colaborador 'Joao' da exclusao da(s) tarefa(s) 'T2'

@rf_3.03 @rn @rn_11.1.9
Cenario: 13 - Dois usuários excluirem a mesma tarefa em um cronograma
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao', 'Maria','Jose' conectado(s) no cronograma 'C1'
	 E que o cronograma 'C1' tiver a(s) tarefa(s) 'T1' solicitada(s) para exclusao pelo colaborador 'Maria'
     E que o cronograma 'C1' tiver a(s) tarefa(s) 'T1' solicitada(s) para exclusao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' excluida(s) pelo(s) colaborador 'Maria'
 Entao o servidor devera receber a confirmacao de que o colaborador 'Maria' excluiu a(s) tarefa(s) 'T1' do cronograma 'C1'
     E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Jose', 'Joao' de que a(s) tarefa(s) 'T1' foi excluida(s) pelo colaborador 'Maria'
	 
@rf_3.03 @rn @rn_11.1.9
Cenario: 14 - Dois usuários excluirem tarefas em cronogramas diferentes
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o cronograma 'C2' possui as seguintes tarefas criadas pelo colaborador 'Maria':
     | id | descricao |
     | 2  | T2        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao' conectado(s) no cronograma 'C1'
	 E que o servidor contenha o(s) colaborador(es) 'Maria' conectado(s) no cronograma 'C2'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' solicitada(s) para exclusao pelo colaborador 'Joao'
	 E o cronograma 'C2' tiver a(s) tarefa(s) 'T2' solicitada(s) para exclusao pelo colaborador 'Maria'
 Entao o servidor devera receber a solicitacao para excluir a(s) tarefa(s) 'T1' no cronograma 'C1'
	 E o servidor devera receber a solicitacao para excluir a(s) tarefa(s) 'T2' no cronograma 'C2'
	 E o cronograma 'C2' nao deve comunicar ao(s) colaborador(es) 'Maria' de que esta(o) sendo excluida(s) tarefa(s) no cronograma 'C1'
	 E o cronograma 'C1' nao deve comunicar ao(s) colaborador(es) 'Joao' de que esta(o) sendo excluida(s) tarefa(s) no cronograma 'C2'


@rf_3.03 @rn @rn_11.1.9
Cenario: 15 - O usuario tentar excluir uma tarefa que esta sendo editada por outro usuario
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
     E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1' solicitada(s) para exclusao pelo colaborador 'Maria'
 Entao o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a(s) tarefa(s) 'T1' nao podera(o) ser excluida(s)
	 E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria','Jose' de que a tarefa 'T1' esta sendo editada pelo colaborador 'Joao'
# Devemos atualizar protótipo para representar isso. Na tela, a ideia é, quando o usuário estiver editando uma tarefa, exibir um icone (de um X 
# vermelho) na primeira coluna das tarefas que foram excluidas por outros usuários e pintar essas linhas excluídas de cinza claro ate que a edicao 
# acabe. Quando a edicao da tarefa acabar, a lista de tarefas do usuario eh atualizada.

@rf_3.03 @rn @rn_11.1.9
Cenario: 16 - Excluir varias tarefas, de uma vez so, quando outros usuarios estiverem editando tarefas impactadas pela exclusao
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
     E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
     E o cronograma 'C1' tiver a tarefa 'T3' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T1','T2','T3','T4' solicitada(s) para exclusao pelo colaborador 'Jose'
	 E o cronograma 'C1' tiver a(s) tarefa(s) 'T2','T4' excluida(s) pelo(s) colaborador 'Jose'
 Entao o servidor devera receber a solicitacao para excluir a(s) tarefa(s) 'T1', 'T2', 'T3', 'T4' no cronograma 'C1'
	 E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Jose' de que a(s) tarefa(s) 'T1','T3' nao podera(o) ser excluida(s)
	 E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao', 'Maria' da exclusao da(s) tarefa(s) 'T2', 'T4'

#antes de realizar este cenario, verificar o possível bug que ocorreu quando movimentar a tarefa de índice as observações foram apagadas.
@rf_3.03 @rn @rn_11.1.8
Cenario: 17 - Mover uma tarefa para o inicio do cronograma quando outros usuarios estiverem editando tarefas que serao afetadas pela ordenacao
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
     E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T3' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T4' movida para a posicao '1' pelo colaborador 'Jose'
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Jose' na seguinte ordem:
     | id | descricao |
     | 1  | T4        |
     | 2  | T1        |
     | 3  | T2        |
     | 4  | T3        |
     E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao','Maria' de que a(s) tarefa(s) sofreram reordenacao conforme a seguir:
	 | id | descricao |
     | 2  | T1        |
     | 3  | T2        |
     | 4  | T3        |

	 #TODO: adicionar na tabela acima a quantidade de posições que uma tarefa subiu ou desceu.

# OBS1: Na tela, a ideia é, enquanto o usuario estiver editando uma tarefa e ocorrer alteracao de ordem, colocar em uma primeira coluna das tarefas um 
# ícone (com uma seta para cima ou para baixo) sinalizando que houve mudança na ordem das tarefas. Somente quando a edicao da tarefa for concluida, eh
# que o grid do usuario sera atualizado com as novas ordens de tarefa.
# Devemos atualizar protótipo para representar isso.

@rf_3.03 @rn @rn_11.1.8
Cenario: 18 - Mover uma tarefa para o fim do cronograma quando outros usuarios estiverem editando tarefas que serao afetadas pela ordenacao
     Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T3' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '4' pelo colaborador 'Jose'
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Jose' na seguinte ordem:
     | id | descricao |
     | 1  | T2        |
     | 2  | T3        |
     | 3  | T4        |
     | 4  | T1        |
	 E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao','Maria' de que a(s) tarefa(s) sofreram reordenacao conforme a seguir:
	 | id | descricao |
     | 1  | T2        |
     | 2  | T3        |
     | 3  | T4        |


@rf_3.03 @rn @rn_11.1.8
Cenario: 19 - Mover para cima uma tarefa e para o meio do cronograma quando outros usuarios estiverem editando tarefas que serao afetadas pela ordenacao
   Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T3' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T4' movida para a posicao '2' pelo colaborador 'Jose'
Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Jose' na seguinte ordem:
     | id | descricao |
     | 1  | T1        |
     | 2  | T4        |
     | 3  | T2        |
     | 4  | T3        |
	 E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao','Maria' de que a(s) tarefa(s) sofreram reordenacao conforme a seguir:
	 | id | descricao |
	 | 3  | T2        |
     | 4  | T3        |

@rf_3.03 @rn @rn_11.1.8
Cenario: 20 - Mover para baixo uma tarefa e para o meio do cronograma quando outros usuarios estiverem editando tarefas que serao afetadas pela ordenacao
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T3' em edicao pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Jose'
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Jose' na seguinte ordem:
     | id | descricao |
     | 1  | T2        |
     | 2  | T3        |
     | 3  | T1        |
     | 4  | T4        |
 	 E o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao','Maria' de que a(s) tarefa(s) sofreram reordenacao conforme a seguir:
	 | id | descricao |
	 | 1  | T2        |
     | 2  | T3        |

@rf_3.03 @RT? @ignore
Cenario: 21 - O usuario concluir a edicao de uma tarefa apos uma operacao de mover ter ocorrido
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Joao'
     E o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Maria'
Quando o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Joao'
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Joao' na seguinte ordem:
     | id | descricao |
     | 1  | T2        |
     | 2  | T3        |
     | 3  | T1        |
     | 4  | T4        |

@rf_3.03 @RT? @ignore
Cenario: 22 - O usuario visualizar tarefas sendo movidas quando nao estiver editando nenhuma tarefa
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria','Jose' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' tiver a tarefa 'T1' movida para a posicao '3' pelo colaborador 'Maria'
 Entao o cronograma 'C1' devera comunicar ao(s) colaborador(es) 'Joao' de que a(s) tarefa(s) sofreram movimentacao conforme a seguir:
     | descricao | movido para |
     | T1        | baixo       |
     | T2        | cima        |
     | T3        | cima        |
	 E apos 0.5 segundo(s)
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Joao' na seguinte ordem:
     | id | descricao |
     | 1  | T2        |
     | 2  | T3        |
     | 3  | T1        |
     | 4  | T4        |

@rf_3.03 @rn @rn_3.2.9
Cenario: 23 - Salvar historico de uma tarefa
   Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao |
     | 1  | T1        |
     | 2  | T2        |
     | 3  | T3        |
     | 4  | T4        |
     E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria' conectado(s) no cronograma 'C1'
	 E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver o historico da tarefa 'T1' salvo pelo colaborador 'Joao' com os seguintes atributos:
	 | hora realizado | data realizado | hora inicial | hora final | comentario         | hora restante | situacao planejamento |
	 | 01:00          | 12/03/2013     | 08:00        | 09:00      | Tarefa realizada   | 02:00         | Em Andamento          |
	 | 02:00          | 15/03/2013     | 09:00        | 11:00      | Tarefa realizada 1 | 01:00         | Em Andamento          |
	 | 05:00          | 23/05/2013     | 17:30        | 22:30      | Tarefa finalizada  | 00:00         | Encerramento          |
     E o cronograma 'C1' tiver a tarefa 'T1' editada pelo colaborador 'Joao' 
 Entao o cronograma 'C1' tera a linha de base da tarefa 'T1' salva
	 E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a(s) tarefa(s) 'T1' recebeu(ram) atualizacao(oes)

@rf_3.03 @rn @rn_11.1.5
Cenario: 27 - O cronograma ser encerrado inesperadamente e nao comunicar o servidor que o usuario se desconectou
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' tiver o(s) colaborador(es) 'Joao' desconectado(s) inesperadamente
 Entao o servidor deve detectar de que o(s) colaborador(es) 'Joao' foram desconectado(s)

@rf_3.03 @rn_11.1.8
Cenario: 29 - A ordenacao do cronograma ser alterada apos a exclusao
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
	 | id | descricao |
	 | 1  | T1        |
	 | 2  | T2        |
	 | 3  | T3        |
	 | 4  | T4        |
	 | 5  | T5        |
	 | 6  | T6        |
	 E que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria' conectado(s) no cronograma 'C1'
Quando o cronograma 'C1' tiver a(s) tarefa(s) 'T2','T4','T6' excluida(s) pelo(s) colaborador 'Joao'
 Entao o cronograma 'C1' devera ter as tarefas visualizadas pelo colaborador 'Joao' na seguinte ordem:
	 | id | descricao |
     | 1  | T1        |
     | 2  | T3        |
	 | 3  | T5        |

#cenario cancelado, pois a funcionalidade está sendo realizada na tela
@ignore
Cenario: 30 - O usuario editar tarefa alterando a situacao da mesma.
  Dado que exista(m) a(s) situacoes de planejamento a seguir:
	 | situacao     | 
	 | Em Andamento | 
	 | Cancelado    | 
	 E que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
     | id | descricao | observacao | situacao     | estimativa | 
     | 1  | T1        |            | Em andamento | 1          | 
	 E que o servidor esta ligado
     E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s)
Quando tarefa 'T1' tiver os seguintes atributos alterados pelo colaborador 'Maria':
	 | campo                 | valor     |
	 | SituacaoPlanejamento  | Cancelado |
 Entao o servidor deve detectar que a tarefa 'T1' do cronograma 'C1' recebeu atualizacoes em 'situacao' pelo colaborador 'Joao'
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a tarefa 'T1' recebeu atualizacoes em 'situacao' pelo colaborador 'Joao'

#cenario cancelado, pois a funcionalidade está sendo realizada na tela
@ignore
Cenario: 31 - O usuario editar tarefa alterando estimativa.     
  Dado que exista(m) a(s) situacoes de planejamento a seguir:
	 | situacao     | 
	 | Não Iniciado | 
	 E que o cronograma 'C1' possui as seguintes tarefas:
     | id | descricao | observacao | situacao     | estimativa |
     | 1  | T1        |            | Não iniciado | 1          |
     E que o servidor esta ligado
     E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s)
Quando tarefa 'T1' tiver os seguintes atributos alterados:
	 | campo                 | valor |
	 | EstimativaInicial     | 4     | 
 Entao o servidor deve detectar que a tarefa 'T1' do cronograma 'C1' recebeu atualizacoes em 'estimativa' pelo colaborador 'Joao'
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a tarefa 'T1' recebeu atualizacoes em 'estimativa' pelo colaborador 'Joao'

#cenario cancelado, pois a funcionalidade está sendo realizada na tela
@ignore
Cenario: 32 - O usuario editar tarefa alterando situacao e estimativa.     
  Dado que exista(m) a(s) situacoes de planejamento a seguir:
	 | situacao     | 
	 | Em Andamento | 
	 | Não Iniciado | 
     E que o cronograma 'C1' possui as seguintes tarefas:
     | id | descricao | observacao | situacao     | estimativa |
     | 1  | T1        |            | Não iniciado | 1          |
     E que o servidor esta ligado
     E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao', 'Maria' conectado(s)
Quando tarefa 'T1' tiver os seguintes atributos alterados:
	 | campo                 | valor        |
	 | SituacaoPlanejamento  | Em Andamento | 
	 | EstimativaInicial     | 4            | 
 Entao o servidor deve ser comunicado que a tarefa 'T1' do cronograma 'C1' recebeu atualizacoes em 'situacao', 'estimativa' pelo colaborador 'Joao'
     E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a tarefa 'T1' recebeu atualizacoes em 'situacao', 'estimativa' pelo colaborador 'Joao'

@ignore
@rf_1.02
Cenario: 33 - O usuario conectar ao projeto pela primeira vez no projeto e o escolher uma cor automaticamente.
	Dado que o servidor esta ligado
	   E que exista(m) o(s) projeto(s) a seguir:
         | nome | tamanho | total de ciclos | ritmo do time |
         | P1   |  30     | 5               | 10            |
  Quando o projeto 'P1' escolher uma cor aleatoria para o colaborador 'Joao'
   Entao o projeto 'P1' devera ter escolhido uma cor aleatoria para o colaborador 'Joao'

@ignore	    
@rf_1.02
Cenario: 34 - O usuario conectar ao projeto pela segunda vez e o reaproveitar a cor escolhida anteriormente para o projeto.
	Dado que o servidor esta ligado
	   E que exista(m) o(s) projeto(s) a seguir:
         | nome | tamanho | total de ciclos | ritmo do time | 
         | P1   |  30     | 5               | 10            |
	   E que o projeto 'P1' tenha escolhido a cor 'preto' para o colaborador 'Joao'
  Quando o projeto 'P1' selecionar a cor escolhida anteriormente para o colaborador 'Joao'
   Entao o projeto 'P1' devera ter escolhido a cor 'preto' para o colaborador 'Joao'

@ignore
@rf_1.02
Cenario: 35 - Mais de um usuario conectar ao projeto pela primeira vez cores distintas serem escolhidas automaticamente.
	Dado que o servidor esta ligado
	   E que exista(m) o(s) projeto(s) a seguir:
         | nome | tamanho | total de ciclos | ritmo do time |
         | P1   |  30     | 5               | 10            |
	   E que o projeto 'P1' tenha escolhido a cor 'preto' para o colaborador 'Joao'
  Quando o projeto 'P1' escolher uma cor aleatoria para o colaborador 'Maria'
   Entao o projeto 'P1' nao deve ter escolhido a cor 'preto' para o colaborador 'Maria'

#este cenário foi está sem numeração pois é equivalente ao cenário 15, porém pode necessitar de alguns ajustes
@ignore
Cenario: 36 - Um usuario tentar editar uma tarefa que esta em processo de exclusao
  Dado que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
       | id | descricao |
       | 1  | T1        |
	   | 2  | T2        |
	 E que o servidor esta ligado
     E que o cronograma 'C1' tenha o(s) colaborador(es) 'Joao' conectado(s)
     E que o cronograma 'C1' tenha o(s) colaborador(es) 'Maria' conectado(s)
     E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
Quando o cronograma 'C1' tiver a tarefa 'T1' solicitada para exclusao pelo colaborador 'Maria'
 Entao o servidor deve ser comunicado de que a(s) tarefa(s) 'T1' esta(o) sendo editada(s)
	 E o servidor deve ser comunicado de que a(s) tarefa(s) 'T1' foram solicitadas para exclusao
	 E o cronograma 'C1' deve comunicar ao(s) colaborador(es) 'Maria' de que a(s) tarefa(s) 'T1' nao pode(m) ser excluida(s)

Cenario: 38 - Quando um usuário se conectar e outros usuários já estiverem editando tarefas.
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Maria' conectado(s) no cronograma 'C1'
	 E que o servidor contenha o(s) colaborador(es) 'Jose','Marcos' conectado(s) no cronograma 'C2'
	 E que o cronograma 'C1' possui as seguintes tarefas criadas pelo colaborador 'Joao':
       | id | descricao |
       | 1  | T1        |
       | 2  | T2        |
       | 3  | T3        |
	 E que o cronograma 'C2' possui as seguintes tarefas criadas pelo colaborador 'Jose':
       | id | descricao |
       | 1  | T1        |
       | 2  | T2        |
       | 3  | T3        |
	 E o cronograma 'C1' tiver a tarefa 'T1' em edicao pelo colaborador 'Joao'
	 E o cronograma 'C1' tiver a tarefa 'T2' em edicao pelo colaborador 'Maria'
	 E o cronograma 'C2' tiver a tarefa 'T3' em edicao pelo colaborador 'Jose'
Quando o cronograma 'C1' for aberto pelo colaborador 'Pedro'
 Entao o servidor devera comunicar o colaborador 'Pedro' que no cronograma 'C1' as seguintes tarefa(s) estao em edicao:
	   | Autor | Tarefa |
	   | Joao  | T1     |
	   | Maria | T2     |

Cenario: 39 - Quando usuarios alterarem simultaneamente o nome do cronograma
  Dado que o servidor esta ligado
	 E que o servidor contenha o(s) colaborador(es) 'Joao','Jose' conectado(s) no cronograma 'C1'
	 E que o colaborador 'Joao' tenha solicitado do servidor permissao para alterar o nome do cronograma 'C1'
Quando o servidor receber a solicitacao de alteracao do nome do cronograma 'C1' pelo colaborador 'Joao'
     E o colaborador 'Jose' solicitar do servidor permissao para alterar nome do cronograma 'C1'
	 E o servidor receber a solicitacao de alteracao do nome do cronograma 'C1' pelo colaborador 'Jose'
 Entao o servidor devera permitir a edicao do nome do cronograma 'C1' para o colaborador 'Joao'
	 E o servidor devera recusar a edicao do nome do cronograma 'C1' para o colaborador 'Jose'

Cenario: 40 - Ao Abrir Janela de Estimativa do esforco realizado pela primeira vez e a situacao da tarefa mudar para Execucao
  Dado que o servico foi inicializado
	 E que o colaborador 'Joao' esteja logado
     E que exista a tarefa :
	    | Field              | Value     |
	    | Descricao          | Tarefa 01 |
	    | Estimativa Inicial | 05:00     |
	    | Restante           | 05:00     |
	    | Realizado          | 00:00     |
	 E que o dia de trabalho para o colaborador 'Joao' no dia '30/08/2013' contenha os periodos:
		| Hora inicial | Hora final |
		| 8:00         | 12:00      |
		| 13:00        | 18:00      |
	 E que o ultimo esforco estimado pelo colaborador seja no dia '30/08/2013' as '08:30'
Quando for aberto o a janela de estimativa de esforco realizado
	 E o colaborador alterar a situacao da tarefa para o tipo 'Execução'
 Entao a situacao planejamento devera ser do tipo 'Execução'


 Cenario: 41 - Ao abrir a janela de estimativa do esforco realizado e zerar a hora restante sem estimar esforco realizado
  Dado que o servico foi inicializado
	 E que o colaborador 'Joao' esteja logado
     E que exista a tarefa :
	    | Field              | Value     |
	    | Descricao          | Tarefa 01 |
	    | Estimativa Inicial | 05:00     |
	    | Restante           | 05:00     |
	    | Realizado          | 00:00     |
	 E que o dia de trabalho para o colaborador 'Joao' no dia '30/08/2013' contenha os periodos:
		| Hora inicial | Hora final |
		| 8:00         | 12:00      |
		| 13:00        | 18:00      |
	 E que o ultimo esforco estimado pelo colaborador seja no dia '30/08/2013' as '08:30'
	 E que foi aberto a janela de estimativa de esforco realizado
Quando o colaborador alterar a hora restante para '00:00'
 Entao a situacao planejamento devera ser do tipo 'Planejamento' pois 'Não deve alterar a situação quando reduzir não estimar realização'
	 E deveria ter habilitado o campo de justificativa de reducao

Cenario: 42 - Ao abrir a janela de estimativa do esforco realizado e estimar as horas realizadas
  Dado que o servico foi inicializado
	 E que o colaborador 'Joao' esteja logado
     E que exista a tarefa :
	    | Field              | Value     |
	    | Descricao          | Tarefa 01 |
	    | Estimativa Inicial | 05:00     |
	    | Restante           | 05:00     |
	    | Realizado          | 00:00     |
	 E que o dia de trabalho para o colaborador 'Joao' no dia '30/08/2013' contenha os periodos:
		| Hora inicial | Hora final |
		| 8:00         | 12:00      |
		| 13:00        | 18:00      |
	 E que o ultimo esforco estimado pelo colaborador seja no dia '30/08/2013' as '08:30'
	 E que foi aberto a janela de estimativa de esforco realizado
Quando o colaborador estimar a o esforco realizado como '02:00'
 Entao a situacao planejamento devera ser do tipo 'Execução' pois 'está ocorrendo uma estimativa, que ainda possui horas restantes a serem trabalhadas'
	 E os horarios esperados deverao ser hora realizado '02:00',hora restante '03:00' e hora final '10:30'
	 E nao deveria ter habilitado o campo de justificativa de reducao

Cenario: 43 Ao abrir a janela de estimativa do esforco realizado e alterada a situacao diretamente para pronto quando houver horas restantes
 Dado que o servico foi inicializado
	 E que o colaborador 'Joao' esteja logado
     E que exista a tarefa :
	    | Field              | Value     |
	    | Descricao          | Tarefa 01 |
	    | Estimativa Inicial | 05:00     |
	    | Restante           | 05:00     |
	    | Realizado          | 00:00     |
	 E que o dia de trabalho para o colaborador 'Joao' no dia '30/08/2013' contenha os periodos:
		| Hora inicial | Hora final |
		| 8:00         | 12:00      |
		| 13:00        | 18:00      |
	 E que o ultimo esforco estimado pelo colaborador seja no dia '30/08/2013' as '08:30'
	 E que foi aberto a janela de estimativa de esforco realizado
Quando o colaborador alterar a situacao da tarefa para o tipo 'Encerramento'
 Entao os horarios esperados deverao ser hora realizado '05:00',hora restante '00:00' e hora final '14:30'

 Cenario: 44 Ao abrir a janela de estimativa do esforco realizado e a situacao for alterado para andamento quando nao houver mais horas restantes e nao houver estimativa anterior
 Dado que o servico foi inicializado
	 E que o colaborador 'Joao' esteja logado
     E que exista a tarefa :
	    | Field              | Value     |
	    | Descricao          | Tarefa 01 |
	    | Estimativa Inicial | 05:00     |
	    | Restante           | 02:00     |
	    | Realizado          | 03:00     |
	 E que o dia de trabalho para o colaborador 'Joao' no dia '30/08/2013' contenha os periodos:
		| Hora inicial | Hora final |
		| 8:00         | 12:00      |
		| 13:00        | 18:00      |
	 E que o ultimo esforco estimado pelo colaborador seja no dia '30/08/2013' as '08:00'
	 E que foi aberto a janela de estimativa de esforco realizado
	 E que o colaborador alterou a hora restante para '00:00'
Quando o colaborador alterar a situacao da tarefa para o tipo 'Execução'
 Entao o colaborador nao devera poder salvar as alterações na tarefa

	
	