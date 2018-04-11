#language: pt-br

@pbi_10.01.03 
Funcionalidade: Analista de RH Visualizar Férias Planejadas Para Determinado Periodo
		Como um Analista de RH
		Quero que seja possível visualizar, cadastrar, alterar e excluír planejamento de férias para os colaboradores.
		Deve ser possível filtrar os dados por um ou mais superior imediato e por período.

Contexto:
     Dado existam as modalidades de ferias:
		| dias | pode vender | ativo |
		| 30   | nao         | sim   |
		| 90   | nao         | sim   |
		E que exista(m) o(s) projeto(s) a seguir:
        | nome       | tamanho | total de ciclos | ritmo do time |
        | Projeto01  | 50      | 5               | 10            |
		| Projeto02  | 50      | 5               | 10            |

Cenário: 01 - Listar os planejamentos de ferias filtrados por periodo.
	Dado que existam os colaboradores:
	    | colaborador | superior | admissao   |
	    | colab 1     |          | 01/01/2011 |
	    | colab 2     |          | 01/01/2011 |
	    | colab 1.1   | colab 1  | 01/01/2011 |
	    | colab 2.1   | colab 2  | 01/01/2011 |
	  E existam as seguintes partes interessadas para o projeto 'Projeto01':
		| colaborador | cargo    |
		| colab 1.1   | Analista | 
	  E que existam os seguintes planejamentos de ferias:
	    | colaborador | data inicial | modalidade |
	    | colab 1.1   | 01/01/2012   | 30	      |
	    | colab 2.1   | 01/01/2012   | 30	      |
  Quando consultar o planejamento de ferias no periodo de '01/2012' a '12/2012':
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto	  |
       | colab 1.1   | colab 1  | Projeto01   |
       | colab 2.1   | colab 2  | Sem Projeto |
      E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/01/2012   | 30         |

Cenário: 02 - Listar os planejamentos de ferias filtrados por periodo e superior imediato.
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   | colab 2     |          | 01/01/2011 |
	   | colab 1.1   | colab 1  | 01/01/2011 |
	   | colab 2.1   | colab 2  | 01/01/2011 |
	   E existam as seguintes partes interessadas para o projeto 'Projeto01':
	   | colaborador | cargo    |
	   | colab 1.1   | Analista | 
	   E que existam os seguintes planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/01/2012   | 30         |
  Quando consultar o planejamento de ferias no periodo de '01/2012' a '02/2012' filtrando pelos superiores imediatos 'colab 1'
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto    |
       | colab 1.1   | colab 1  | projeto 01 |
       E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |

Cenário: 03 - Listar os planejamentos de ferias filtrados por periodo e varios superiores imediatos.
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   | colab 2     |          | 01/01/2011 |
	   | colab 3     |          | 01/01/2011 |
	   | colab 1.1   | colab 1  | 01/01/2011 |
	   | colab 2.1   | colab 2  | 01/01/2011 | 
	   | colab 3.1   | colab 3  | 01/01/2011 | 
	   E existam as seguintes partes interessadas para o projeto 'Projeto01':
	   | colaborador | cargo    |
	   | colab 1.1   | Analista | 
	   E que existam os seguintes planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/01/2012   | 30         |
	   | colab 3.1   | 01/01/2012   | 30         |  
  Quando consultar o planejamento de ferias no periodo de '01/2012' a '02/2012' filtrando pelos superiores imediatos 'colab 1','colab 2'
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto     |
       | colab 1.1   | colab 1  | projeto 01  |
	   | colab 2.1   | colab 2  | Sem Projeto |
       E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/01/2012   | 30         |

Cenário: 04 - Listar os planejamentos de ferias filtrados por periodo.
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   | colab 2     |          | 01/01/2011 |
	   | colab 3     |          | 01/01/2011 |
	   | colab 1.1   | colab 1  | 01/01/2011 |
	   | colab 2.1   | colab 2  | 01/01/2011 |
	   | colab 3.1   | colab 3  | 01/01/2011 |
	   E existam as seguintes partes interessadas para o projeto 'Projeto01':
	   | colaborador | cargo    |
	   | colab 1.1   | Analista | 
	   E que existam os seguintes planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/02/2012   | 30         |
	   | colab 3.1   | 01/03/2012   | 30         |  
  Quando consultar o planejamento de ferias no periodo de '01/2012' a '02/2012':
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto     |
       | colab 1.1   | colab 1  | projeto 01  |
	   | colab 2.1   | colab 2  | Sem Projeto |
       E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/02/2012   | 30         |
	   
Cenário: 05 - Listar os planejamentos de ferias dos colaboradores visiveis.
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   | colab 2     |          | 01/01/2011 |
	   | colab 3     |          | 01/01/2011 |
	   | colab 1.1   | colab 1  | 01/01/2011 |
	   | colab 2.1   | colab 2  | 01/01/2011 | 
	   | colab 2.2   | colab 2  | 01/01/2011 | 
	   | colab 3.1   | colab 3  | 01/01/2011 | 
	   E existam as seguintes partes interessadas para o projeto 'Projeto01':
	   | colaborador | cargo    |
	   | colab 1.1   | Analista | 
	   | colab 2.1   | Analista |
	   E existam as seguintes partes interessadas para o projeto 'Projeto02':
	   | colaborador | cargo    |
	   | colab 3.1   | Analista | 
	   E que existam os seguintes planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/01/2012   | 30         |
	   | colab 3.1   | 01/01/2012   | 30         |  
  Quando consultar o planejamento de ferias no periodo de '01/2012' a '12/2012':
       E ocultar os dados do 'Projeto02'
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto     |
       | colab 1.1   | colab 1  | projeto 01  |
	   | colab 2.1   | colab 2  | projeto 01  |
       E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 01/01/2012   | 30         |
	   | colab 2.1   | 01/02/2012   | 30         |


Cenário: 06 - Listar os planejamentos de ferias todos os periodos.
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   | colab 2     |          | 01/01/2011 |
	   | colab 3     |          | 01/01/2011 |
	   | colab 1.1   | colab 1  | 01/01/2011 |
	   | colab 2.1   | colab 2  | 01/01/2011 | 
	   | colab 2.2   | colab 2  | 01/01/2011 | 
	   | colab 3.1   | colab 3  | 01/01/2011 | 
	   | colab 3.2   | colab 3  | 01/01/2011 | 
	   E existam as seguintes partes interessadas para o projeto 'Projeto01':
	   | colaborador | cargo    |
	   | colab 1.1   | Analista | 
	   | colab 2.1   | Analista |
	   E existam as seguintes partes interessadas para o projeto 'Projeto02':
	   | colaborador | cargo    |
	   | colab 2.2   | Analista | 
	   | colab 3.1   | Analista | 
	   E que existam os seguintes planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 20/02/2012   | 30         |
	   | colab 2.1   | 01/03/2012   | 30         |
	   | colab 2.2   | 20/03/2012   | 30         |  
	   | colab 3.1   | 01/02/2012   | 90         |  
	   | colab 3.2   | 01/01/2012   | 30         |  
  Quando consultar o planejamento de ferias no periodo de '03/2012' a '03/2012': 
   Entao devem ser listados no plenejamento de ferias os colaboradores:
       | colaborador | superior | projeto     |
       | colab 1.1   | colab 1  | projeto 01  |
	   | colab 2.1   | colab 1  | projeto 01  |
	   | colab 2.2   | colab 2  | projeto 02  |
	   | colab 3.1   | colab 3  | projeto 02  |
       E devem ser listados os planejamentos de ferias:
	   | colaborador | data inicial | modalidade |
	   | colab 1.1   | 20/02/2012   | 30         |
	   | colab 2.1   | 01/03/2012   | 30         |
	   | colab 2.2   | 20/03/2012   | 30         |  
	   | colab 3.1   | 01/02/2012   | 90         |


Cenário: 07 - Criar planejamento de férias no passado
    Dado que existam os colaboradores:
	   | colaborador | superior | admissao   |
	   | colab 1     |          | 01/01/2011 |
	   E data atual for '20/07/2012'
  Quando criar planejamento de ferias para o colaborador 'colab 1' iniciado '15/07/2012' na modalidade '30'
   Entao o planejamento de férias para o colaborador 'colab 1' iniciado '15/07/2012' estará apto a salvar