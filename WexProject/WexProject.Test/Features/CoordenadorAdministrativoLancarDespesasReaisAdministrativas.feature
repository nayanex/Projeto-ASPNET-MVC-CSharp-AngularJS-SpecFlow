#language: pt-br

@Custos @DespesasReais @Administrativo
Funcionalidade: Coordenador Administrativo lancar as despesas reais administrativas

Contexto:
  Dado que existem os projetos a seguir:
       | nome | inicio planejado | inicio real | situacao     |
       | P1   | 03/02/14         | 03/02/14    | Em Andamento |
       | P2   | 02/01/14         | 02/01/14    | Em Andamento |
       | P3   | 01/06/14         | 01/06/14    | Em Andamento |
       | P4   | 01/07/14         | 01/07/14    | Em Andamento | 
     E que existem os aditivos a seguir:
       | descricao  | projeto |  inicio   | termino  | qtde meses | orcamento aprovado |
       | Fase 1     | P1      | 03/02/14  | 31/03/14 |     2      | 93.802,00		   |
       | Fase 2     | P1      | 01/03/14  | 30/06/14 |     3      | 150.000,50         |
       | Aditivo P2 | P2      | 02/01/14  | 31/12/14 |     12     | 350.000,00         |
       | Aditivo P4 | P4      | 01/07/14  | 30/09/14 |     3      | 50.100,00          |
     E que existem os seguintes tipos de rubricas:
       | nome        | classe          |
       | Viagens     | Desenvolvimento |
       | RH MDireta  | Desenvolvimento |
       | RH GDC      | Desenvolvimento |
       | Custo Fixo  | Administrativo  |
       | Taxa de Adm | Administrativo  |
       | FACN        | Administrativo  |
       | Impostos    | Administrativo  |

#// -- REGISTRO DAS DESPESAS REAIS -- //

@Custos @DespesasReais @Administrativo
Cenario: 01.01 - Deve atualizar a despesa real quando o projeto possuir apenas um aditivo
	Dado que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 10000 | 10000     |       |
		| RH MDireta  | 50000 | 25000     | 25000 |
		| Custo Fixo  | 20000 | 10000     | 10000 |
		| Taxa de Adm | 12000 | 6000      | 6000  |
		| Impostos    | 1802  | 901       | 901   |
	Quando as despesas reais do projeto 'P2' forem informadas conforme a seguir:
		| rubrica     | Janeiro | Fevereiro |
		| Viagens     |         | 8000      |
		| RH MDireta  | 23000   | 25000     |
		| Custo Fixo  |         | 9500      |
		| Taxa de Adm | 10000   | 6000      |
		| Impostos    |         | 1200      |
	E os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Fevereiro' de '2014'
	Entao devo encontrar os seguintes custos administrativos no mes de 'Fevereiro' de '2014':
		| rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
		| Custo Fixo  | P2      | 10000              | 10000            | 9500         |
		| Taxa de Adm | P2      | 6000               | -4000            | 6000         |
		| Impostos    | P2      | 901                | 901              | 1200         |


# ------------------ Os cenários abaixo já existiam --------------

@Custos @DespesasReais @Administrativo
Cenario: 01.02 - Deve atualizar a despesa real quando o projeto possuir mais de um aditivo com periodos nao conflitantes
  Dado que o aditivo 'Fase 1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
       | rubrica     | Total | Fevereiro | Marco |
       | Viagens     | 1000  | 1000      |       |
       | RH GDC      | 10000 | 5000      | 5000  |
       | Taxa de Adm | 25000 | 12500     | 12500 |
       | FACN        | 37802 |           | 37802 | 
    E que o aditivo 'Fase 2' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
       | rubrica     | Total    | Abril | Maio  | Junho   |
       | RH MDireta  | 50100    | 16700 | 16700 | 16700   |
       | RH GDC      | 15000    |       |       | 15000   |
       | Custo Fixo  | 18000,50 | 6000  | 6000  | 6000,50 |
       | Taxa de Adm | 51000    | 17000 | 17000 | 17000   |
       | Impostos    | 15900    | 15000 | 900   |         |
	Quando as despesas reais do projeto 'P1' forem informadas conforme a seguir:
       | rubrica     | Marco | Abril | Maio  | Junho   |
       | Custo Fixo  |       |       |       | 6000,50 |
       | Taxa de Adm | 12500 | 10000 | 24000 |         |
       | Impostos    |       | 15000 | 900   |         |
    E os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Junho' de '2014'
	Entao devo encontrar os seguintes custos administrativos no mes de 'Junho' de '2014':
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P1      | 6000,50            | 18000,50         | 6000,50      |
       | Impostos    | P1      | 0                  | 0                | 0            |
       | Taxa de Adm | P1      | 17000              | 29500            | 0            |
       
@Custos @DespesasReais @Administrativo
Cenario: 01.03 - Deve atualizar a despesa real quando o projeto possuir mais de um aditivo com periodos conflitantes
  Dado que o aditivo 'Fase 1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
       | rubrica     | Total | Fevereiro | Marco |
       | Viagens     | 1000  | 1000      |       |
       | RH GDC      | 10000 | 5000      | 5000  |
       | Taxa de Adm | 25000 | 12500     | 12500 |
       | FACN        | 37802 |           | 37802 |  
     E que o aditivo 'Fase 2' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
       | rubrica     | Total | Marco | Abril | Maio  |
       | Custo Fixo  | 18000 | 6000  | 6000  | 6000  |
       | Taxa de Adm | 51000 | 17000 | 17000 | 17000 |
       | Impostos    | 15900 | 15000 | 900   |       |
	Quando as despesas reais do projeto 'P1' forem informadas conforme a seguir:
       | rubrica     | Marco | Abril |
       | Taxa de Adm | 29000 | 100   |
     E os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Maio' de '2014'
	Entao devo encontrar os seguintes custos administrativos no mes de 'Maio' de '2014':
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P1      | 6000               | 18000            | 0            |
       | Impostos    | P1      | 0                  | 15900            | 0            |
       | Taxa de Adm | P1      | 17000              | 46900            | 0            |