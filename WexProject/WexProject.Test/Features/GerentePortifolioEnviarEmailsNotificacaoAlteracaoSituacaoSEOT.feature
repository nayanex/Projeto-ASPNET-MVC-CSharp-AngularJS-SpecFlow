#language: pt-br

@pbi_9.26
Funcionalidade: Gerente de Portifólio enviar automaticamente emails de notificação sobre alteração da situação da solicitação de orçamento
            Dado um Gerente de Portifólio
			Gostaria de enviar automaticamente emails de notificação sobre alteração da situação da solicitação de orçamento
            Então poder acompanhar através do e-mail a evolução da concepção do documento e me manter informado

@bug
Cenário: 01 - Criar uma nova Solicitação e a mesma deverá poder ser salva com sucesso quando os dados estiverem corretos.
			Dado os seguintes tipos de solicitacao de orcamento:
			| descrição                 |
			| P&D de Lei de Informática |
			E as seguintes configuracoes de documento:
			| documento            |
			| SolicitacaoOrcamento |
			E as seguintes situacoes de configuracao de documento:
			| documento            | situação     | cc                                       | cco                                      | padrão? |
			| SolicitacaoOrcamento | Não Iniciada | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | false   |
			E ter colaborador(es) 'colaborador01'
			E os seguintes paises:
			| nome   | máscara de telefone | situação | país padrão? |
			| Brasil | 55 00 0000-0000     | Ativo    | True         | 
			E as seguintes empresas/instituicoes:
			| sigla | nome                   | email         | pais   | fone/fax        |
			| FPF   | Fundação Paulo Feitoza | fpf@email.com | Brasil | 55 92 0000-0000 |
			E usuario logado for 'colaborador01'
			Quando as solicitações de orçamento a seguir forem criadas:
			| responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente |
			| colaborador01 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 30/11/2012 | FPF     |
			Entao a solicitação de orçamento estará apta a salvar