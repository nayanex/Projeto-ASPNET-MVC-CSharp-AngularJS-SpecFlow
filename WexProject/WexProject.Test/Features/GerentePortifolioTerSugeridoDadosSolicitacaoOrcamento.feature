#language: pt-br

@pbi_9.19
Funcionalidade: Gerente de Portifólio ter sugerido automaticamente dados da solicitação de orçamento
       Como um Gerente de Portfólio
       Eu quero poder ter sugerido automaticamente dados da solicitação de orçamento
       Para que eu possa facilitar diversos cadastro de Solicitação de Orçamento

Cenário: 01 - Obter a data sugerida para o prazo com 9 dias úteis após a data atual
		Dado a data atual for '20/03/2012'
		Quando for criado uma nova Solicitação de Orçamento 'seot01' (codigo 'CLI01.02/2012')
		Entao o campo "Prazo" da 'CLI01.02/2012' a ser sugerido deve ser 9 dias úteis após a data atual, sendo '02/04/2012'

Cenario: 02 - Sugerir o último cliente utilizado pelo usuário logado
		Dado uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E ter colaborador(es) 'colaborador01','colaborador02','colaborador03'
		E usuario logado for 'colaborador01'
		E a(s) SEOT(s) com os seguintes valor(es):
        | SEOT     | Responsavel    | Situacao    | Cliente    | Codigo          | TipoSolicitacao |
        | 'seot01' | 'Colaborador1' | 'Situacao1' | 'Cliente1' | 'CLI01.01/2012' | Tipo01          |
		Quando for criado uma nova Solicitação de Orçamento 'seot02' (codigo 'CLI01.02/2012')
		Então o campo 'Cliente' deve estar preechido com 'Empresa01' para a seot 'CLI01.02/2012'

Cenário: 03 - Sugerir o último Tipo de Solicitação utilizado pelo usuário logado
		Dado o(a) Tipo(s) de Solicitação de Orçamento com os seguintes valores:
		| Descrição |
		| Tipo01  |
		E uma empresa com o nome 'Empresa01' e sigla 'emp01'
		E ter colaborador(es) 'colaborador01','colaborador02','colaborador03'
		E usuario logado for 'colaborador01'
		E a(s) SEOT(s) com os seguintes valor(es):
        | SEOT    | Responsavel    | Situacao    | Cliente    | Codigo         | TipoSolicitacao |
        | 'seot01' | 'Colaborador1' | 'Situacao1' | 'Cliente1' | 'CLI01.01/2012'|Tipo01        |
		Quando for criado uma nova Solicitação de Orçamento 'seot02' (codigo 'CLI01.02/2012')
		Então o campo 'TipoSolicitacao' deve estar preechido com 'Tipo01' para a seot 'CLI01.02/2012'

Cenário: 04 - Quando definir uma situação inicial em configuração de documento trocar a Situacao Inicial definida como padrão
		Dado as seguintes configuracoes de documento:
		| documento            |
		| SolicitacaoOrcamento |
		E as seguintes situacoes de configuracao de documento:
		| documento            | situação    | cc                                       | cco                                      | padrão? |
		| SolicitacaoOrcamento | Descricao01 | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | true    |
		Quando criar uma nova Configuracao de Documento de Situacao com o(s) seguinte(s) valores:
		| Descrição | Cor | SituacaoInicial |
		| Descricao02 | 255; 255; 0 | true |
		Então a Configuração de Documento de Situação definida como padrão é 'Descricao02'

Cenário: 05 - Sugerir a situação de documento definido como situação inicial
		Dado as seguintes configuracoes de documento:
		| documento            |
		| SolicitacaoOrcamento |
		E as seguintes situacoes de configuracao de documento:
		| documento            | situação    | cc                                       | cco                                      | padrão? |
		| SolicitacaoOrcamento | Descricao01 | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | true    |
		| SolicitacaoOrcamento | Descricao02 | email@email.com.br                       |                                          | false   |
		Quando for criado uma nova Solicitação de Orçamento 'seot01' (codigo 'CLI01.01/2012')
		Então o campo 'Situacao' deve estar preechido com 'Descricao01'