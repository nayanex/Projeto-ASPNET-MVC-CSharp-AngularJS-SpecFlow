#language: pt-br

Funcionalidade: Gerente de projetos associar notas fiscais a despesa real de uma rubrica

Contexto:
  Dado que existem os projetos a seguir:
       | nome | inicio planejado | inicio real | situacao     |
       | P2   | 02/01/14         | 02/01/14    | Em Andamento |
     E que existem os aditivos a seguir:
       | descricao | projeto | inicio   | termino  | qtde meses | orcamento aprovado |
       | AdP2      | P2      | 02/01/14 | 31/12/14 | 12         | 350.000,00         |
     E que existem os seguintes tipos de rubricas:
       | nome              | classe          |
       | Viagens           | Desenvolvimento |
       | RH MDireta        | RecursosHumanos |
       | RH GDC            | RecursosHumanos |
       | RH TI             | RecursosHumanos |
       | RH Designer       | RecursosHumanos |
       | RH Testes         | RecursosHumanos |
       | RH Qualidade      | RecursosHumanos |
       | RH DiretoDissidio | RecursosHumanos |

Cenario: 01.01 - Deve atualizar a despesa real ao associar notas fiscais a uma rubrica em um periodo informado
	Dado que o aditivo 'AdP2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 10000 | 10000     |       |
		| RH MDireta  | 50000 | 25000     | 25000 |
	E que existam as despesas reais no aditivo 'AdP2' do projeto 'P2' conforme informadas a seguir:
		| rubrica     | Fevereiro | Marco |
		| Viagens     | 8000      | 2000  |
		| RH MDireta  | 23000     | 25000 |
	E que existam as seguintes notas fiscais pendentes de associacao do aditivo 'AdP2' do projeto 'P2' no mes de 'Fevereiro' de '2014'
		| descricao      | valor |
		| 13 salario     | 20,00 |
		| passagem aerea | 30,00 |
		| transporte     | 20,00 |
	Quando as notas fiscais abaixo forem associadas com a rubrica 'Viagens' do aditivo 'AdP2' do projeto 'P2' no mes de 'Fevereiro' de '2014':	
		| descricao      |
		| 13 salario     |
		| passagem aerea |
		| transporte     |
	Entao as despesas reais seguintes devem ser encontradas para o aditivo 'AdP2' do projeto 'P2' na rubrica 'Viagens' no mes de 'Fevereiro' de '2014':
		| rubrica | despesa real |
		| Viagens | 70           |

Cenario: 01.02 - Deve atualizar a despesa real ao desassociar notas fiscais de uma rubrica em um periodo informado
	Dado que o aditivo 'AdP2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 10000 | 10000     |       |
		| RH MDireta  | 50000 | 25000     | 25000 |
	E que existam as despesas reais no aditivo 'AdP2' do projeto 'P2' conforme informadas a seguir:
		| rubrica     | Fevereiro | Marco |
		| Viagens     | 8000      | 2000  |
		| RH MDireta  | 23000     | 25000 |
	E que existam as seguintes notas fiscais associadas a rubrica 'Viagens' do aditivo 'AdP2' do projeto 'P2' no mes de 'Fevereiro' de '2014'
		| descricao      | valor |
		| 13 salario     | 20,00 |
		| passagem aerea | 30,00 |
		| transporte     | 20,00 |
	Quando as notas fiscais abaixo forem desassociadas da rubrica 'Viagens' do aditivo 'AdP2' do projeto 'P2' no mes de 'Fevereiro' de '2014':	
		| descricao      |
		| 13 salario     |
		| passagem aerea |
	Entao as despesas reais seguintes devem ser encontradas para o aditivo 'AdP2' do projeto 'P2' na rubrica 'Viagens' no mes de 'Fevereiro' de '2014':
		| rubrica | despesa real |
		| Viagens | 20           |

Cenario: 02.01 - Deve mostrar rubricas de desenvolvimento, com excecao de rubricas de RH, ao acessar secao Associar notas fiscais do mes
	Dado que o aditivo 'AdP2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
        | rubrica      | Total | Fevereiro | Marco |
        | Viagens      | 20000 | 10000     | 1000  |
        | RH MDireta   | 50000 | 25000     | 25000 |
        | RH GDC       | 20000 | 10000     | 10000 |
        | RH TI        | 12000 | 6000      | 6000  |
        | RH Designer  | 1802  | 901       | 901   |
        | RH Testes    | 5000  | 2500      | 2500  |
	E que existem os seguintes tipos de rubricas:
		| nome         | classe          |
		| Treinamentos | Desenvolvimento |
	Quando as despesas reais do projeto 'P2' e aditivo 'AdP2' tiverem as rubricas para associar notas fiscais listadas 
	Entao devo encontrar nas despesas reais as seguintes rubricas de desenvolvimento para vincular as notas fiscais:
		| rubrica      |
		| Viagens      |
		


