#language: pt-br

@Custos @DespesasReais @Administrativo
Funcionalidade: Coordenador Administrativo Calcular Automaticamente Saldo Disponivel Por Rubrica Administrativa

Contexto:
  Dado que existem os projetos a seguir:
       | nome | inicio planejado | inicio real | situacao     |
       | P1   | 03/02/14         | 03/02/14    | Em Andamento |
       | P2   | 02/01/14         | 02/01/14    | Em Andamento |
       | P3   | 01/06/14         | 01/06/14    | Em Andamento |
       | P4   | 01/07/14         | 01/07/14    | Concluido    |
     E que existem os aditivos a seguir:
       | descricao    | projeto | inicio   | termino  | qtde meses | orcamento aprovado |
       | Fase 1       | P1      | 03/02/14 | 31/03/14 | 2          | 93.802,00          |
       | Fase 2       | P1      | 01/04/14 | 30/06/14 | 3          | 150.000,50         |
       | Aditivo P2   | P2      | 02/01/14 | 31/12/14 | 12         | 350.000,00         |
       | Aditivo P4   | P4      | 01/07/14 | 30/09/14 | 3          | 50.100,00          |
     E que existem os seguintes tipos de rubricas:
       | nome               | classe          |
       | Viagens            | Desenvolvimento |
       | RH MDireta         | Desenvolvimento |
       | RH GDC             | Desenvolvimento |
       | Custo Fixo         | Administrativo  |
       | Taxa de Adm        | Administrativo  |
       | FACN               | Administrativo  |
       | Impostos           | Administrativo  |
       | Apoio a Clientes   | Administrativo  |
       | Apoio a Clientes 2 | Administrativo  |

#// -- 01 - LISTAGEM -- //

@Custos @DespesasReais @Administrativo
Cenario: 01.01 - Deve listar as rubricas administrativas e projetos vigentes no mes informado com saldo disponivel dos meses anteriores
	Dado que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Janeiro | Fevereiro | Marco |
		| Viagens     | 2000  |         | 2000      |       |
		| RH MDireta  | 45000 | 15000   | 15000     | 15000 |
		| Custo Fixo  | 30000 | 10000   | 10000     | 10000 |
		| Taxa de Adm | 10000 |         | 10000     |       |
		| Impostos    | 2500  |         | 2500      |       |
	E que o aditivo 'Fase 1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 1000  | 1000      |       |
		| RH MDireta  | 20000 | 10000     | 10000 |
		| RH GDC      | 10000 | 5000      | 5000  |
		| Taxa de Adm | 25000 | 12500     | 12500 |
		| FACN        | 37802 |           | 37802 |
Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Fevereiro' de '2014' 
	Entao os custos administrativos no mes de 'Fevereiro' de '2014' devem ser encontrados:
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P2      | 10000              | 20000            | 0            |
       | Impostos    | P2      | 2500               | 2500             | 0            |
       | Taxa de Adm | P1      | 12500              | 12500            | 0            |
       | Taxa de Adm | P2      | 10000              | 10000            | 0            |

@Custos @DespesasReais @Administrativo
Cenario: 01.02 - Deve listar as rubricas administrativas e projetos vigentes no mes informado quando houver despesas reais ja lancadas
	Dado que o aditivo 'Fase 1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 1000  | 1000      |       |
		| RH MDireta  | 20000 | 10000     | 10000 |
		| RH GDC      | 10000 | 5000      | 5000  |
		| Taxa de Adm | 25000 | 12500     | 12500 |
		| FACN        | 37802 |           | 37802 |   
    E que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Janeiro | Fevereiro | Marco |
		| Viagens     | 2000  |         |           | 2000  |
		| RH MDireta  | 45000 | 15000   | 15000     | 15000 |
		| Custo Fixo  | 30000 | 10000   | 10000     | 10000 |
		| Taxa de Adm | 10000 |         | 10000     |       |
		| Impostos    | 2500  |         | 2500      |       |
	E que o aditivo 'Aditivo P2' do projeto 'P2' possua as seguintes despesas reais informadas:
		| rubrica     | Fevereiro | Marco |
		| Custo Fixo  | 550       |       |
		| Taxa de Adm | 1500      |       |
		| RH MDireta  |           | 25000 |
		| Impostos    |           | 10    |
	Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Fevereiro' de '2014'
	Entao os custos administrativos no mes de 'Fevereiro' de '2014' devem ser encontrados:
			| rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
			| Custo Fixo  | P2      | 10000              | 20000            | 550          |
			| FACN        | P1      | 0                  | 0                | 0            |
			| Impostos    | P2      | 2500               | 2500             | 0            |
			| Taxa de Adm | P1      | 12500              | 12500            | 0            |
			| Taxa de Adm | P2      | 10000              | 10000            | 1500         |

#// -- 02 - CALCULO DO SALDO QUANDO HOUVER DESPESAS REAIS -- //

@Custos @DespesasReais @Administrativo
Cenario: 02.01 - Deve calcular saldo disponivel quando houver despesas reais lancadas em meses anteriores
  Dado que o aditivo 'Fase 1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Fevereiro | Marco |
		| Viagens     | 1000  | 1000      |       |
		| RH GDC      | 10000 | 5000      | 5000  |
		| Taxa de Adm | 25000 | 12500     | 12500 |
		| FACN        | 37802 |           | 37802 |
	E que o aditivo 'Fase 1' do projeto 'P1' possua as seguintes despesas reais informadas:
		| rubrica     | Fevereiro | Marco |
		| Viagens     | 1500      |       |
		| Taxa de Adm |           | 650   |
		| FACN        |           | 802   |
     E que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
       | rubrica     | Total  | Janeiro | Fevereiro | Marco | Abril | Maio  | Junho | Julho | Agosto | Setembro | Outubro | Novembro |
       | RH MDireta  | 165000 | 15000   | 15000     | 15000 | 15000 | 15000 | 15000 | 15000 | 15000  | 15000    | 15000   | 15000    |
       | Custo Fixo  | 110000 | 10000   | 10000     | 10000 | 10000 | 10000 | 10000 | 10000 | 10000  | 10000    | 10000   | 10000    |
       | Taxa de Adm | 50000  |         | 10000     |       | 10000 |       | 10000 |       | 10000  |          | 10000   |          |
       | Impostos    | 5000   |         | 2500      |       |       |       | 2500  |       |        |          |         |          |
     E que o aditivo 'Aditivo P2' do projeto 'P2' possua as seguintes despesas reais informadas:
		| rubrica  | Marco |
		| Impostos | 1800  |
Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Outubro' de '2014'
 Entao os custos administrativos no mes de 'Outubro' de '2014' devem ser encontrados:
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P2      | 10000              | 100000           | 0            |
       | Impostos    | P2      | 0                  | 3200             | 0            |
       | Taxa de Adm | P2      | 10000              | 50000            | 0            |
Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Marco' de '2014'
 Entao os custos administrativos no mes de 'Marco' de '2014' devem ser encontrados:
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P2      | 10000              | 30000            | 0            |
       | FACN        | P1      | 37802              | 37802            | 802          |
       | Impostos    | P2      | 0                  | 2500             | 1800         |
       | Taxa de Adm | P1      | 12500              | 25000            | 650          |
       | Taxa de Adm | P2      | 0                  | 10000            | 0            |
	
@Custos @DespesasReais @Administrativo
Cenario: 02.02 - Deve calcular saldo disponivel quando houver despesas reais lancadas em meses anteriores sem saldo disponivel
  Dado que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
  		| rubrica     | Total | Janeiro | Fevereiro | Marco |
  		| RH MDireta  | 45000 | 15000   | 15000     | 15000 |
  		| Custo Fixo  | 3000  | 10000   | 10000     | 10000 |
  		| Taxa de Adm | 10000 |         | 10000     |       |
  		| Impostos    | 2500  |         | 2500      |       | 
	 E que o aditivo 'Aditivo P2' do projeto 'P2' possua as seguintes despesas reais informadas:
		| rubrica     | Janeiro | Fevereiro |
		| Custo Fixo  | 10000   | 10000     |
		| Taxa de Adm | 10000   |           |
		| Impostos    |         | 2500      |
	Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Marco' de '2014'
	Entao os custos administrativos no mes de 'Marco' de '2014' devem ser encontrados:
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P2      | 10000              | 10000            | 0            |
       | Impostos    | P2      | 0                  | 0                | 0            |
       | Taxa de Adm | P2      | 0                  | 0                | 0            |

@Custos @DespesasReais @Administrativo
Cenario: 02.03 - Deve calcular saldo disponivel quando houver despesas reais lancadas em meses anteriores e com saldo disponivel negativo
  Dado que o aditivo 'Aditivo P2' do projeto 'P2' possui o seguinte planejamento para uso do orcamento aprovado:
		| rubrica     | Total | Janeiro | Fevereiro | Marco |
		| RH MDireta  | 45000 | 15000   | 15000     | 15000 |
		| Custo Fixo  | 3000  | 10000   | 10000     | 10000 |
		| Taxa de Adm | 10000 |         | 10000     |       |
		| Impostos    | 2500  |         | 2500      |       |
	E que o aditivo 'Aditivo P2' do projeto 'P2' possua as seguintes despesas reais informadas:
		| rubrica     | Janeiro | Fevereiro |
		| Custo Fixo  | 10000   | 15000     |
		| Taxa de Adm | 20000   |           |
		| Impostos    |         | 2501      |
Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Marco' de '2014'
	Entao os custos administrativos no mes de 'Marco' de '2014' devem ser encontrados:
       | rubrica     | projeto | orcamento aprovado | saldo disponivel | despesa real |
       | Custo Fixo  | P2      | 10000              | 5000			   | 0            |
       | Impostos    | P2      | 0                  | -1               | 0            |
       | Taxa de Adm | P2      | 0                  | -10000           | 0            |

@Custos @DespesasReais @Administrativo @Bug
Cenario: 02.04 - Deve calcular saldo disponivel quando houver despesas reais lancadas em meses anteriores e com saldo disponivel descosiderando valores fora do periodo do aditivo
	Dado que existem os projetos a seguir:
       | nome | inicio planejado | inicio real | situacao     |
       | P5   | 02/01/14         | 02/01/14    | Em Andamento |
	E que existem os aditivos a seguir:
       | descricao | projeto | inicio   | termino  | qtde meses | orcamento aprovado |
       | Aditivo 1 | P5      | 02/03/14 | 31/12/14 | 10         | 860105.00          |
	E que o aditivo 'Aditivo 1' do projeto 'P5' possui o seguinte planejamento para uso do orcamento aprovado no ano de '2014':
		| rubrica          | Total     | Marco    | Abril    | Maio     | Junho    | Julho    | Agosto   | Setembro | Outubro  | Novembro | Dezembro |
		| Custo Fixo       | 21614.01  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,16  | 1801,25  |
		| Taxa de Adm      | 162399,98 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,33 | 13533,35 |
		| Apoio a Clientes | 25803,12  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  | 2150,26  |
	E que o aditivo 'Aditivo 1' do projeto 'P5' possui o seguinte planejamento para uso do orcamento aprovado no ano de '2013':
		| rubrica          | Total    | Janeiro  | Fevereiro |
		| Custo Fixo       | 3602,32  | 1801,16  | 1801,16   |
		| Taxa de Adm      | 27066,66 | 13533,33 | 13533,33  |
		| Apoio a Clientes | 4050,00  | 2025,00  | 2025,00   |
	E que o aditivo 'Aditivo 1' do projeto 'P5' possua as seguintes despesas reais informadas no ano de '2014':
       | rubrica          | Marco    | Abril   | Maio     | Junho    | Julho   | Agosto  | Setembro | Outubro | Novembro | Dezembro |
       | Custo Fixo       | 1801,16  | 0,00    | 3602,16  | 1801,16  |         |         |          |         |          |          |
       | Taxa de Adm      | 13533,33 | 0,00    | 27066,66 | 13533,33 |         |         |          |         |          |          |
       | Apoio a Clientes | 2150,26  | 2150,26 | 2150,26  | 2150,26  | 2150,26 | 2150,26 | 2150,26  | 2150,26 | 2150,26  | 2150,26  |
	E que o aditivo 'Aditivo 1' do projeto 'P5' possua as seguintes despesas reais informadas no ano de '2013':
       | rubrica          | Janeiro  | Fevereiro |
       | Custo Fixo       | 1801,16  | 1801,16   |
       | Taxa de Adm      | 13533,33 | 13533,33  |
       | Apoio a Clientes | 2025,00  | 2025,00   |
	Quando os custos administrativos dos projetos 'Em Andamento' forem calculados para o mes de 'Maio' de '2014' 
	Entao os custos administrativos no mes de 'Maio' de '2014' devem ser encontrados:
		   | rubrica          | projeto | orcamento aprovado | saldo disponivel | despesa real |
		   | Custo Fixo       | P5      | 1801,16            | 3602,32          | 3602,16      |
		   | Taxa de Adm      | P5      | 13533,33           | 27066,66         | 27066,66     |
		   | Apoio a Clientes | P5      | 2150,26            | 2150,26          | 2150,26      |

