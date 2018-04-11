#language: pt-br

@Custos @OrcamentoAprovado @pbi_01.09.02
Funcionalidade: Gerente de projetos calcular o orçamento restante p/ rubricas

Contexto:
 Dado que existem os seguintes tipos de rubricas:
      | nome               | classe          |
      | Viagens            | Desenvolvimento |
      | RH MDireta         | Desenvolvimento |
      | Custo Fixo         | Administrativo  |
    E que existem os projetos a seguir:
      | nome | inicio planejado | inicio real | situacao     |
      | P1   | 03/02/14         | 31/12/14    | Em Andamento |
    E que existem os aditivos a seguir:
      | descricao    | projeto | inicio   | termino  | qtde meses | orcamento aprovado |
      | adP1         | P1      | 03/02/14 | 31/12/14 | 11         | 100.000,00         |
    E que o aditivo 'adP1' do projeto 'P1' possui o seguinte planejamento para uso do orcamento aprovado:
      | rubrica    | Total |
      | Viagens    | 0     |
      | RH MDireta | 0     |
      | Custo Fixo | 0     |
	E que o aditivo 'adP1' do projeto 'P1' esta sendo planejado nesse momento
      
Cenario: 01 - Deve calcular o orcamento restante igual o valor aprovado quando nao houver nenhum orcamento de rubricas planejados
	Quando o orcamento aprovado do aditivo 'adP1' do projeto 'P1' tiver o valor restante para rubricas recalculado
	Entao devo encontrar no orcamento aprovado do projeto um valor restante de '100.000,00'

Cenario: 02 - Deve calcular o orcamento restante subtraindo o valor do orcamento das rubricas planejados
	Quando o orcamento aprovado do aditivo 'adP1' do projeto 'P1' receber o valor de '10.000,00' na rubrica 'Viagens'
	E o orcamento aprovado do aditivo 'adP1' do projeto 'P1' tiver o valor restante para rubricas recalculado
	Entao devo encontrar no orcamento aprovado do projeto um valor restante de '90.000,00'
   
Cenario: 03 - Deve calcular o orcamento restante quando o valores planejados nas rubricas somados for maior que o orcamento aprovado do aditivo
    Dado que o orcamento aprovado do aditivo 'adP1' do projeto 'P1' foi replanejado conforme a seguir:
      | rubrica     | Total     |
	  | Viagens     | 50.000,00 |
	  | RH MDireta  | 30.000,00 |
	Quando o orcamento aprovado do aditivo 'adP1' do projeto 'P1' receber o valor de '70.000,01' na rubrica 'Viagens'
	E o orcamento aprovado do aditivo 'adP1' do projeto 'P1' tiver o valor restante para rubricas recalculado
	Entao devo encontrar no orcamento aprovado do projeto um valor restante de '-0,01'
   
Cenario: 04 - Deve calcular o orcamento restante quando o valores planejados das rubricas somados for igual ao orcamento aprovado do aditivo
    Dado que o orcamento aprovado do aditivo 'adP1' do projeto 'P1' foi replanejado conforme a seguir:
      | rubrica     | Total     |
	  | Viagens     | 69.999,99 |
	  | RH MDireta  | 30.000,00 |
	Quando o orcamento aprovado do aditivo 'adP1' do projeto 'P1' receber o valor de '70.000,00' na rubrica 'Viagens'
	E o orcamento aprovado do aditivo 'adP1' do projeto 'P1' tiver o valor restante para rubricas recalculado
	Entao devo encontrar no orcamento aprovado do projeto um valor restante de '0,00'
   
Cenario: 05 - Deve calcular o orcamento restante subtraindo o valor do orcamento das rubricas administrativas planejadas
	Quando o orcamento aprovado do aditivo 'adP1' do projeto 'P1' receber o valor de '5.000,00' na rubrica 'Custo Fixo'
    E o orcamento aprovado do aditivo 'adP1' do projeto 'P1' receber o valor de '10.000,01' na rubrica 'RH MDireta'
    E o orcamento aprovado do aditivo 'adP1' do projeto 'P1' tiver o valor restante para rubricas recalculado
	Entao devo encontrar no orcamento aprovado do projeto um valor restante de '84.999,99'
   