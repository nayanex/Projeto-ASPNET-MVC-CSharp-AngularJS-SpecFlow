#language: pt-br

@pbi_9.02
Funcionalidade: O Gerente de Portifólio poder Acompanhar Histórico de uma Solicitação de Orçamento
            Dado um Gerente de Portifólio
			Gostaria de acompanhar o histórico de alteração das situações da Solicitação de Orçamento
            Então poder visualizar altereções no Documento

@bug
Cenário: 01 - Colaborador modificar uma SEOT. O histórico deve considerar que o usuário logado é o responsável pela alteração.
			Dado os seguintes tipos de solicitacao de orcamento:
			| descrição                 |
			| P&D de Lei de Informática |
			E as seguintes configuracoes de documento:
			| documento            |
			| SolicitacaoOrcamento |
			E as seguintes situacoes de configuracao de documento:
			| documento            | situação     | cc                                       | cco                                      | padrão? |
			| SolicitacaoOrcamento | Não Iniciada | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | false   |
			E ter colaborador(es) 'colaborador01','colaborador03'
			E os seguintes paises:
			| nome   | máscara de telefone | situação | país padrão? |
			| Brasil | 55 00 0000-0000     | Ativo    | True         | 
			E as seguintes empresas/instituicoes:
			| sigla | nome                   | email         | pais   | fone/fax        |
			| FPF   | Fundação Paulo Feitoza | fpf@email.com | Brasil | 55 92 0000-0000 |
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E as seguintes solicitacoes de orcamento:
			| responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente |
			| colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 30/11/2012 | FPF     |
			E data atual for '22/02/2012 22:00:00'
			Quando as solicitações de orçamento a seguir forem modificadas:
			| código      | responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente | comentário                 |
			| FPF.01/2012 | colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 15/11/2012 | FPF     | Alteração no Prazo da SEOT |
			Entao o historico da solicitacao de orcamento 'FPF.01/2012' deve ser:
            | data e hora       | responsavel   | situação     | comentário                 | atualizado por |
            | 20/02/12 22:00:00 | colaborador03 | Não Iniciada | Criação do Documento       | colaborador01  |
            | 22/02/12 22:00:00 | colaborador03 | Não Iniciada | Alteração no Prazo da SEOT | colaborador01  |

Cenário: 02 - Colaborador alterar uma SEOT. Guardar último comentário feito para exibição.
			Dado os seguintes tipos de solicitacao de orcamento:
			| descrição                 |
			| P&D de Lei de Informática |
			E as seguintes configuracoes de documento:
			| documento            |
			| SolicitacaoOrcamento |
			E as seguintes situacoes de configuracao de documento:
			| documento            | situação     | cc                                       | cco                                      | padrão? |
			| SolicitacaoOrcamento | Não Iniciada | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | false   |
			E ter colaborador(es) 'colaborador01','colaborador03'
			E os seguintes paises:
			| nome   | máscara de telefone | situação | país padrão? |
			| Brasil | 55 00 0000-0000     | Ativo    | True         | 
			E as seguintes empresas/instituicoes:
			| sigla | nome                   | email         | pais   | fone/fax        |
			| FPF   | Fundação Paulo Feitoza | fpf@email.com | Brasil | 55 92 0000-0000 |
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E as seguintes solicitacoes de orcamento:
			| responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente |
			| colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 30/11/2012 | FPF     |
			E data atual for '22/02/2012 22:00:00'
			Quando as solicitações de orçamento a seguir forem modificadas:
			| código      | responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente | comentário                 |
			| FPF.01/2012 | colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 15/11/2012 | FPF     | Alteração no Prazo da SEOT |
			Entao o último comentário de alteração das SEOTs devem ser os seguintes:
            | código      | último histórico           |
            | FPF.01/2012 | Alteração no Prazo da SEOT |

Cenário: 03 - Colaborador criar uma SEOT. O comentário deve ser 'Criação do Documento'.
			Dado os seguintes tipos de solicitacao de orcamento:
			| descrição                 |
			| P&D de Lei de Informática |
			E as seguintes configuracoes de documento:
			| documento            |
			| SolicitacaoOrcamento |
			E as seguintes situacoes de configuracao de documento:
			| documento            | situação     | cc                                       | cco                                      | padrão? |
			| SolicitacaoOrcamento | Não Iniciada | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | false   |
			E ter colaborador(es) 'colaborador01','colaborador03'
			E os seguintes paises:
			| nome   | máscara de telefone | situação | país padrão? |
			| Brasil | 55 00 0000-0000     | Ativo    | True         | 
			E as seguintes empresas/instituicoes:
			| sigla | nome                   | email         | pais   | fone/fax        |
			| FPF   | Fundação Paulo Feitoza | fpf@email.com | Brasil | 55 92 0000-0000 |
			E usuario logado for 'colaborador01'
			Quando as solicitações de orçamento a seguir forem criadas:
			| responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente |
			| colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 30/11/2012 | FPF     |
			Entao o último comentário de alteração das SEOTs devem ser os seguintes:
            | código      | último histórico     |
            | FPF.01/2012 | Criação do Documento |

Cenário: 04 - Colaborador modificar uma SEOT. Deve ser o responsável pela atualização.
			Dado os seguintes tipos de solicitacao de orcamento:
			| descrição                 |
			| P&D de Lei de Informática |
			E as seguintes configuracoes de documento:
			| documento            |
			| SolicitacaoOrcamento |
			E as seguintes situacoes de configuracao de documento:
			| documento            | situação     | cc                                       | cco                                      | padrão? |
			| SolicitacaoOrcamento | Não Iniciada | email@email.com.br;emailtwo@email.com.br | emailtwo@email.com.br;email@email.com.br | false   |
			E ter colaborador(es) 'colaborador01','colaborador03'
			E os seguintes paises:
			| nome   | máscara de telefone | situação | país padrão? |
			| Brasil | 55 00 0000-0000     | Ativo    | True         | 
			E as seguintes empresas/instituicoes:
			| sigla | nome                   | email         | pais   | fone/fax        |
			| FPF   | Fundação Paulo Feitoza | fpf@email.com | Brasil | 55 92 0000-0000 |
			E data atual for '20/02/2012 22:00:00'
			E usuario logado for 'colaborador01'
			E as seguintes solicitacoes de orcamento:
			| responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente |
			| colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 30/11/2012 | FPF     |
			E data atual for '22/02/2012 22:00:00'
			Quando usuario logado for 'colaborador03'
			E as solicitações de orçamento a seguir forem modificadas:
			| código      | responsável   | situação     | tipo de solicitação       | prioridade | título                         | prazo      | cliente | comentário                 |
			| FPF.01/2012 | colaborador03 | Não Iniciada | P&D de Lei de Informática | Alta       | Desenvolvimento de Aplicativos | 15/11/2012 | FPF     | Alteração no Prazo da SEOT |
			Entao o historico da solicitacao de orcamento 'FPF.01/2012' deve ser:
            | data e hora       | responsavel   | situação     | comentário                 | atualizado por |
            | 20/02/12 22:00:00 | colaborador03 | Não Iniciada | Criação do Documento       | colaborador01  |
            | 22/02/12 22:00:00 | colaborador03 | Não Iniciada | Alteração no Prazo da SEOT | colaborador03  |