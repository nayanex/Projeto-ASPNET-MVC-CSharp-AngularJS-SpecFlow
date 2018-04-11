#language: pt-br

@pbi_3.1
Funcionalidade: O Gestor do Produto definir as entregas de um ciclo de desenvolvimento
		Como um Gestor do Produto
		Eu quero identificar quais entregas priorizadas farão parte de um ciclo de desenvolvimento
		Para que eu possa informar ao time de desenvolvimento a meta do ciclo, de forma que as entregas agreguem valor ao negócio da minha empresa.

@rf_4.02 @bug
Cenário: [BUG] - Um colaborador troca o status de uma estória de pronto para replanejado
     Dado um projeto 'Teste' com o(s) ciclo(s) 'ciclo 1', 'ciclo 2'
	    E ciclo '1' do projeto 'Teste' na situação 'Concluido' com as estórias:
          | estória  | situação     | pontos | tipo				| valor do negócio	|
          | Teste 01 | Pronto       | 5      | EscopoContratado	| Mandatorio		|
          | Teste 02 | Replanejado  | 3      | EscopoContratado	| Mandatorio		|
		E ciclo '2' do projeto 'Teste' na situação 'NaoPlanejado' com as estórias:
          | estória  | situação     | pontos | tipo				| valor do negócio	|
          | Teste 03 | NaoIniciado  | 5      | EscopoContratado	| Mandatorio		|
   Quando mudar a situacao do ciclo 'ciclo 1' para 'Em Andamento'
	      | estória  | situação    |
	      | Teste 01 | Replanejado |
      Então as estorias do projeto 'Teste' devem estar com a situacao
	      | estória  | situação    |
	      | Teste 01 | Replanejado |
		  | Teste 02 | Replanejado |