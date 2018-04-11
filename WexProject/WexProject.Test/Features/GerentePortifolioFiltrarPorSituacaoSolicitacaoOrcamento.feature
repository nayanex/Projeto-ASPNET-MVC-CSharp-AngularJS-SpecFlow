#language: pt-br

@pbi_9.17
Funcionalidade: Gerente de Portifólio selecionar nos filtros de Responsavel e Situação apenas aquelas que possuem pelo menos 1 ocorrencia na lista de SEOTs cadastradas
	Como um Gerente de Portifolio
	Eu quero selecionar nos filtros de Responsavel e Situação apenas aquelas que possuem pelo menos 1 ocorrencia na lista de SEOTs cadastradas
	Para que eu possa encontrar mais rápido os itens do filtro

Cenário: 01 - Definir as opcoes que devem aparecer no filtro de responsavel (RF_9.17)
		Dado uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E a(s) SEOT(s) com os seguintes valor(es):
	| SEOT    | Responsavel    | Situacao    |
	| 'SEOT1' | 'Colaborador1' | 'Situacao1' |
	| 'SEOT2' | 'Colaborador2' | 'Situacao1' |
	| 'SEOT3' | 'Colaborador3' | 'Situacao1' |
	| 'SEOT4' | 'Colaborador3' | 'Situacao1' |
	Quando verificar se as opções do filtro de responsavel são: 'Colaborador1','Colaborador2','Colaborador3'
	Entao apenas o(s) colaborador(es) 'Colaborador1', 'Colaborador2', 'Colaborador3' devem ser opções no filtro de responsavel

Cenário: 02 - Definir as opcoes que devem aparecer no filtro de situação (RF_9.17)
		Dado uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E a(s) SEOT(s) com os seguintes valor(es):
	| SEOT    | Responsavel    | Situacao    |
	| 'SEOT1' | 'Colaborador1' | 'Situacao1' |
	| 'SEOT2' | 'Colaborador1' | 'Situacao2' |
	| 'SEOT3' | 'Colaborador1' | 'Situacao3' |
	| 'SEOT4' | 'Colaborador1' | 'Situacao3' |
	Quando verificar as opções do filtro de situacao são: 'Situacao1', 'Situacao2', 'Situacao3'
	Entao apenas as situacoes 'Situacao1', 'Situacao2', 'Situacao3' devem ser opções no filtro de situação

Cenário: 03 - Definir as opções do filtro de situação e responsavel com um colaborador sem SEOT (RF_9.17)
		Dado uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E a(s) SEOT(s) com os seguintes valor(es):
	| SEOT    | Responsavel    | Situacao    |
	| 'SEOT1' | 'Colaborador1' | 'Situacao1' |
	| 'SEOT2' | 'Colaborador1' | 'Situacao2' |
	| 'SEOT3' | 'Colaborador1' | 'Situacao3' |
	| 'SEOT4' | 'Colaborador1' | 'Situacao3' |
	E a situação 'SituacaoX' sem SEOT associada a ela 
	Quando verificar as opções do filtro de responsavel é:'Colaborador1'  e situacao são: 'Situacao1', 'Situacao2', 'Situacao3'
	Entao apenas as situacoes 'Situacao1', 'Situacao2', 'Situacao3' devem ser opções no filtro de situação e apenas o colaborador 'Colaborador1' deve ser opção no filtro de responsavel e a situacao 'SituacaoX' não deve aparecer
	
Cenário: 04 - Definir as opções do filtro de situação e responsavel com uma situacao sem SEOT  (RF_9.17)
		Dado uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E a(s) SEOT(s) com os seguintes valor(es):
	| SEOT    | Responsavel    | Situacao    |
	| 'SEOT1' | 'Colaborador1' | 'Situacao1' |
	| 'SEOT2' | 'Colaborador2' | 'Situacao1' |
	| 'SEOT3' | 'Colaborador3' | 'Situacao1' |
	| 'SEOT4' | 'Colaborador3' | 'Situacao1' |
	E o colaborador 'ColaboradorX' sem SEOT associada a ele
	Quando verificar se as opções do filtro de responsavel são 'Colaborador1', 'Colaborador2', 'Colaborador3'  e situacao é 'Situacao1'
	Entao apenas a situacao 'Situacao1' deve ser opção no filtro de situação e os colaboradores 'Colaborador1', 'Colaborador2', 'Colaborador3' devem ser opções no filtro de responsavel e o colaborador 'ColaboradorX' não deve aparecer como opção
