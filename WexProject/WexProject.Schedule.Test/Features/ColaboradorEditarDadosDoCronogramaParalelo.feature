#language: pt-br
@CronogramaPresenter
Funcionalidade: O colaborador editar os dados do cronograma
			gerenciar a edição dos dados do cronograma

Contexto:
	@Dto 
	Dado que exista o servidor 'localhost' com a porta '8000'
	Dado que exista(m) o(s) cronograma(s) 
	| nome | inicio     | final      |
	| C1   | 05/06/2014 | 09/06/2014 |
	| C2   | 04/06/2014 | 08/06/2014 |
	E que existam os usuarios no cronograma 'C1':
	| nome     | login         |
	| gabriel  | gabriel.matos |
	| anderson | anderson.lins |
	E que o cronograma 'C1' esta sendo utilizado pelo usuario 'gabriel'

Cenario: 01.01 O usuario iniciar a edicao de dados do cronograma
	Quando o cronograma atual iniciar a edicao de dados do cronograma
	 Entao o cronograma atual devera solicitar a permissao de edicao dos dados

Cenario: 02.01 O usuario iniciar a edicao de dados do cronograma e receber a permissao de edicao enquanto estiver editando
	Quando o cronograma atual iniciar a edicao de dados do cronograma
		 E o cronograma atual recebeu a permissao de edicao dos dados
	 Entao o cronograma atual deve se manter em edicao
	Quando o cronograma atual encerrar a edicao dos dados
	 Entao o cronograma atual devera comunicar automaticamente o fim da edicao dos dados

Cenario: 02.02 O usuario iniciar a edicao de dados do cronograma e receber a permissao de edicao quando tiver encerrado a edicao
	Quando o cronograma atual iniciar a edicao de dados do cronograma
		 E o cronograma atual encerrar a edicao dos dados
	     E o cronograma atual recebeu a permissao de edicao dos dados
	 Entao o cronograma atual devera comunicar automaticamente o fim da edicao dos dados
	
Cenario: 03.01 O usuario iniciar a edicao de dados do cronograma e for recusado enquanto estiver editando
	Quando o cronograma atual iniciar a edicao de dados do cronograma
	     E o cronograma atual recebeu a recusa de edicao dos dados
	 Entao o cronograma atual deve forcar o fim da edicao
		 E o cronograma devera atualizar os dados a partir do servico

Cenario: 03.02 O usuario iniciar a edicao de dados do cronograma e for recusado quando tiver encerrado a edicao
	Quando o cronograma atual iniciar a edicao de dados do cronograma
		 E o cronograma atual encerrar a edicao dos dados
	     E o cronograma atual recebeu a recusa de edicao dos dados
	 Entao o cronograma devera atualizar os dados a partir do servico
	

#Cenario: 03.02 O usuario iniciar a edicao de dados do cronograma e for recusado quando tiver encerrado a edicao
#	Quando o cronograma 'C1' tiver os dados alterados pelo usuario 'gabriel'
#		 E o cronograma 'C1' ainda esta com os dados em edicao pelo usuario 'gabriel'
#		 E o cronograma 'C1' recebeu a recusa de edicao para o usuario 'gabriel'
#	 Entao o cronograma 'C1' devera encerrar a edicao automaticamente retornando os valores anteriores
	
	
