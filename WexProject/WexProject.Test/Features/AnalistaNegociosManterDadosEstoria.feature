#language: pt-br

@pbi_2.11
Funcionalidade: Manter os dados da estoria ao clicar no botão salvar e novo
		Como um Analista de Negócios
		Eu quero manter os alguns dados da estoria selecionada após clicar em salvar e novo
		Para que seja agilizado o processo de digitação de estoorias relacionadas

@rn
Cenário: 01 - Ao clicar no botão Salvar e Novo, manter os dados dos campos 'Como um', 'Solicitante', 'Módulo' e 'Estoria Pai'
			Dado o módulo '2 - Escopo'
		E o módulo '1 - Teste'
		E benefeciado 'Analista de Negocios'
		E benefeciado 'Desenvolvedor'
		E parte interessada 'Alexandre Amorim'
		E a estoria 'estoria00' com as seguintes propriedades:
			   | Field             | Value                    |
		  | Titulo            |				             |
		  | EstoriaPai        |					         |
		  | ComoUm            | Desenvolvedor	         |
		  | Solicitante       |				             |
		  | Modulo            | 1 - Teste                |
		E a estoria 'estoria01' com as seguintes propriedades:
		  | Field             | Value                    |
			   | Titulo            | Estoria_01               |
		  | EstoriaPai        | estoria00                |
			   | ComoUm            | Analista de Negocios     |
			   | Solicitante       | Alexandre Amorim         |
			   | Modulo            | 2 - Escopo               |
			Quando clicar em salvar e novo da estoria 'estoria01' 
		Entao a nova estoria deve estar com as seguintes propiedades: 
			   | Field             | Value                    |
		  | Titulo            |                          |
		  | EstoriaPai        | estoria00                |
			   | ComoUm            | Analista de Negocios     |
			   | Solicitante       | Alexandre Amorim         |
		  | Modulo            | 2 - Escopo               |