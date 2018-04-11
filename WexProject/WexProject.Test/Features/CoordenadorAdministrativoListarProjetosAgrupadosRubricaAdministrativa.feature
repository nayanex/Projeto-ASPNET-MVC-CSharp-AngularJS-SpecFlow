#language: pt-br

@Administrativo
Funcionalidade: Coordenador Administrativo listar projetos agrupados por rubricas administrativas

Contexto:
  Dado que existem os projetos a seguir:
       | nome | inicio planejados | inicio real | situacao     |
       | P1   | 03/02/14          | 03/02/14    | Em Andamento |
       | P2   | 02/02/14          | 02/02/14    | Em Andamento |
       | P3   | 01/06/14          | 01/06/14    | Em Andamento |
       | P4   | 01/07/14          | 01/07/14    | Em Andamento |
       | PX   | 30/01/14          | 20/02/14    | Em Andamento |
     E que existem os aditivos a seguir:
       | descricao  | projeto | inicio   | termino  | qtde meses | orcamento aprovado |
       | Fase 1     | P1      | 01/02/14 | 31/03/14 | 2          | 93.802,00          |
       | Aditivo X  | PX      | 30/01/14 | 20/11/14 | 10         | 1.150.000,50       |
       | Aditivo P2 | P2      | 02/01/14 | 31/12/14 | 12         | 350.000,00         |
       | Aditivo P4 | P4      | 01/07/14 | 30/09/14 | 3          | 50.100,00          |
	E que existem as classes de projeto a seguir:
		| Nome                   |
		| Projeto Patrocinado    |
		| Projeto sem Patrocínio |
		| Setor                  |
	E que existem os tipos de projetos a seguir:
		| Nome                   | ClasseProjeto		  |
		| Projeto Base           | Projeto Patrocinado	  |
		| Setor de Administração | Setor				  |
     E que existem os seguintes tipos de rubricas:
       | nome        | classe          | TipoProjeto  |
       | Viagens     | Desenvolvimento | Projeto Base |
       | RH MDireta  | Desenvolvimento | Projeto Base |
       | RH GDC      | Desenvolvimento | Projeto Base |
       | Custo Fixo  | Administrativo  | Projeto Base |
       | Taxa de Adm | Administrativo  | Projeto Base |
       | FACN        | Administrativo  | Projeto Base |
       | Impostos    | Administrativo  | Projeto Base |

@Administrativo
Cenario: 01 - Deve listar as rubricas administrativa dados mês e ano
	Dado que o aditivo 'Fase 1' do projeto 'P1' possui as seguintes rubricas:
		| nome        |
		| Viagens     |
		| RH GDC      |
		| Custo Fixo  |
		| Taxa de Adm |
	E que o aditivo 'Fase 1' do projeto 'P1' possui as seguintes configurações das rubricas:
		| nome        | Mes       | Ano  |
		| Viagens     | Fevereiro | 2014 |
		| Custo Fixo  | Fevereiro | 2014 |
		| Custo Fixo  | Marco     | 2013 |
		| Taxa de Adm | Fevereiro | 2014 |
	E que o aditivo 'Aditivo P2' do projeto 'P2' possui as seguintes rubricas:
		| nome        |
		| RH MDireta  |
		| RH GDC      |
		| Taxa de Adm |
		| FACN        |
	E que o aditivo 'Aditivo P2' do projeto 'P2' possui as seguintes configurações das rubricas:
		| nome        | Mes		  | Ano  |
		| RH MDireta  | Fevereiro | 2014 |
		| RH GDC      | Fevereiro | 2014 |
		| Taxa de Adm | Fevereiro | 2014 |
		| Taxa de Adm | Marco	  | 2014 |
		| FACN        | Fevereiro | 2014 |

	Quando o usuário listar as rubricas do tipo 'Administrativo' no mês de 'Fevereiro' de '2014'
	Entao devo encontrar as seguintes rubricas administrativas:
       | rubrica     |
       | Custo Fixo  |
       | Taxa de Adm |
       | FACN        |
       | Impostos    | 

@Administrativo
Cenario: 02 - Deve listar os projetos de uma rubrica administrativa dados mês e ano
	Dado que o aditivo 'Fase 1' do projeto 'P1' possui as seguintes rubricas:
		| nome        |
		| Viagens     |
		| RH GDC      |
		| Custo Fixo  |
		| Taxa de Adm |
	E que o aditivo 'Aditivo P2' do projeto 'P2' possui as seguintes rubricas:
		| nome        |
		| RH MDireta  |
		| RH GDC      |
		| Taxa de Adm |
		| FACN        |

	Quando o usuário listar os projetos 'Em Andamento' da rubrica 'Taxa de Adm' no mês de 'Fevereiro' de '2014'
	Entao devo encontrar os seguintes projetos:
		| projeto |
		| P1      |
		| P2      |