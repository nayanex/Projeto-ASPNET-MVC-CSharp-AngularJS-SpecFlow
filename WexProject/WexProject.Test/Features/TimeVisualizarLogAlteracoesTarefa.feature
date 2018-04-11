#language: pt-br

@pbi_4.03.12
Funcionalidade: Time de Desenvolvimento visualizar o log de alterações de uma tarefa
            Dado um time de Desenvolvimento
			Quando estiver no cronograma 
            E visualizar o log de atualizações de uma determinada tarefa
            Entao poder visualizar todas as modificações da tarefa selecionada

Cenário: 01 - Um colaborador modificar uma tarefa. Este deverá ser registrado no log de atualizações da tarefa
			Dado um cronograma 'cronograma01'
			E ter colaborador(es) 'colaborador01','colaborador02','colaborador03','colaborador04'
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
			E data atual for '22/02/2012 22:00:00'
			Quando o colaborador 'colaborador01' modificar a tarefa 'tarefa01':
			| campo | valor			|
			| nome  | tarefazeroum  |
			Entao o histórico de log da 'tarefa01' deve estar:
            | data e hora			| responsavel     | alterações												|
			| 22/02/12 22:00:00		| colaborador01   | Descrição alterada de 'tarefa01' para 'tarefazeroum'\n	|
			| 20/02/12 22:00:00		| colaborador01   | Criação da tarefa\n										|
			E a tarefa 'tarefa01' deve estar com os dados de última atualização:
			| campo			| valor					|
			| data			| 22/02/12 22:00		|
			| responsável	| colaborador01			|

Cenário: 02 - Dois colaboradores modificarem uma mesma tarefa. Estes deverão ser registrados no log de atualizações da tarefa
            Dado um cronograma 'cronograma01'
			E ter colaborador(es) 'colaborador01','colaborador02','colaborador03','colaborador04'
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefazeroum			|
			| quando | 22/02/2012 22:00:00	|
			E data atual for '23/02/2012 16:00:00'
			Quando o colaborador 'colaborador02' modificar a tarefa 'tarefa01':
			| campo | valor			|
			| nome  | tarefazeroone |
            Entao o histórico de log da 'tarefa01' deve estar:
			| data e hora			| responsavel     | alterações														|
			| 23/02/12 16:00:00		| colaborador02   | Descrição alterada de 'tarefazeroum' para 'tarefazeroone'\n		|
			| 22/02/12 22:00:00		| colaborador01   | Descrição alterada de 'tarefa01' para 'tarefazeroum'\n			|
			| 20/02/12 22:00:00		| colaborador01   | Criação da tarefa\n												|
			E a tarefa 'tarefa01' deve estar com os dados de última atualização:
			| campo			| valor					|
			| data		 | 23/02/12 16:00	|
			| responsável	| colaborador02			|

Cenário: 03 - Dois colaboradores modificarem tarefas distintas. Estes deveram ser registrado no log de atualizações de cada tarefa
			Dado um cronograma 'cronograma01'
			E ter colaborador(es) 'colaborador01','colaborador02','colaborador03'
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 02			|
			| nome  | tarefa02		|
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefazeroum			|
			| quando | 22/02/2012 22:00:00	|
			E data atual for '23/02/2012 16:00:00'
			Quando o colaborador 'colaborador02' modificar a tarefa 'tarefa02':
			| campo | valor			 |
			| nome  | tarefazerodois |
            Entao o histórico de log da 'tarefa02' deve estar:
			| data e hora			| colaborador     | alterações													|
			| 23/02/12 16:00:00		| colaborador02   | Descrição alterada de 'tarefa02' para 'tarefazerodois'\n	|
			| 20/02/12 22:00:00		| colaborador01   | Criação da tarefa\n											|
            E a tarefa 'tarefa01' deve estar com os dados de última atualização:
			| campo			| valor					|
			| data        | 22/02/12 22:00 |
			| responsável	| colaborador01			|
			E a tarefa 'tarefa02' deve estar com os dados de última atualização:
			| campo			| valor					|
			| data        | 23/02/12 16:00 |
			| responsável	| colaborador02			|

Cenário: 04 - Ter uma tarefa selecionada. O botão de visualizar o Histórico de Alterações deve estar habilitado
			Dado um cronograma 'cronograma01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
			E ter colaborador(es) 'colaborador01'
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefazeroum			|
			| quando | 22/02/2012 22:00:00	|
			Quando selecionar a(s) tarefa(s) 'tarefa01'
			Entao o botão 'Histórico de Atualização' deve estar habilitado

Cenário: 05 - Ter mais de uma tarefa selecionada. O botão de visualizar o Histórico de Alterações não deve estar habilitado
			Dado um cronograma 'cronograma01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 02			|
			| nome  | tarefa02		|
			E ter colaborador(es) 'colaborador01'
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefazeroum			|
			| quando | 22/02/2012 22:00:00	|
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa02':
			| campo  | valor				|
			| nome   | tarefazerodois		|
			| quando | 23/02/2012 22:00:00	|
			Quando selecionar a(s) tarefa(s) 'tarefa01','tarefa02'
			Entao o botão 'Histórico de Atualização' não deve estar habilitado

Cenário: 06 - Ter uma tarefa selecionada e obter o Histórico de Alterações da mesma
			Dado um cronograma 'cronograma01'
			E ter colaborador(es) 'colaborador01','colaborador02','colaborador03'
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E uma tarefa do cronograma 'cronograma01' com os dados:
			| campo | valor			|
			| id	| 01			|
			| nome  | tarefa01		|
            E o colaborador 'colaborador01' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefazeroum			|
			| quando | 22/02/2012 22:00:00	|
			E o colaborador 'colaborador02' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefa			|
			| quando | 10/03/2012 20:00:00	|
			E o colaborador 'colaborador03' ter modificado a tarefa 'tarefa01':
			| campo  | valor				|
			| nome   | tarefaalterada		|
			| quando | 15/04/2012 10:00:00	|
			Quando selecionar a(s) tarefa(s) 'tarefa01'
			Entao o histórico de log da 'tarefa01' deve estar:
			| data e hora			| responsavel		| alterações												|
			| 15/04/12 10:00		| colaborador03		| Descrição alterada de 'tarefa' para 'tarefaalterada'\n	|
			| 10/03/12 20:00		| colaborador02		| Descrição alterada de 'tarefazeroum' para 'tarefa'\n		|
			| 22/02/12 22:00		| colaborador01		| Descrição alterada de 'tarefa01' para 'tarefazeroum'\n	|
			| 20/02/12 22:00		| colaborador01		| Criação da tarefa\n										|

Cenário: 07 - Um colaborador criar uma tarefa. Deverá ser registrado no log de atualizações da tarefa
			Dado um cronograma 'cronograma01'
			E ter colaborador(es) 'colaborador01','colaborador02','colaborador03','colaborador04'
			E usuario logado for 'colaborador01'
			E data atual for '22/02/2012 22:00:00'
			Quando o colaborador logado criar as seguintes tarefas para o cronograma 'cronograma01':
			| id	| nome			|
			| 01	| tarefa01		|
			Entao o histórico de log da 'tarefa01' deve estar:
            | data e hora			| responsavel     | alterações					|
			| 22/02/12 22:00:00		| colaborador01   | Criação da tarefa\n			|
			E a tarefa 'tarefa01' deve estar com os dados de última atualização:
			| campo			| valor					|
			| data			| 22/02/12 22:00		|
			| responsável	| colaborador01			|