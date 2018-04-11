#language: pt-br

@pbi_9.15
Funcionalidade: Gerente de Portifólio validar os dados da Empresa/Instituição
		Como um Gerente de Portifólio
		Eu quero validar os dados da Empresa/Instituição
		Para que se evite cadastros problemáticos

Cenário: 01 - Excluir um País que está sendo usado por uma Empresa/Instituição.
		 Dado uma empresa com o nome 'empresa01' e sigla 'emp01'
		 E o(s) país(es) 'Pais01'
		 E o país 'Pais01' que está sendo usado na Empresa/Instituição 'empresa01'
		 Quando tentar excluir o pais 'Pais01'
		 Então deverá exibir a seguinte excessão para o pais 'Pais01': 'O País está sendo usado numa Empresa/Instituição'

Cenário: 02 - Criar um novo País. Verificar se a Máscara do País vem, por padrão, com o valor "55"
		Dado o(s) país(es) 'Pais01'
		Quando criar um pais 'Pais02' com situação 'ativo' e marcado como 'padrão'
		Então a Máscara do País 'Pais02' deve estar com o valor padrão '55' 

Cenário: 03 - Cadastrar mais de um país como "País Padrão" (ambos com situação ativo).
		Dado o(s) país(es) 'Pais01'
		E o pais 'Pais01' marcado como 'padrão'
		E o pais 'Pais01' marcado como 'ativo'
		Quando criar um pais 'Pais02' com situação 'ativo' e marcado como 'padrão'
		Então exibir a janela de modificação de pais padrão para o pais 'Pais02'

Cenário: 04 - Cadastrar mais de um país como "País Padrão" (apenas um com a situação ativo, o resto como inativo)
		Dado o(s) país(es) 'Pais01','Pais02'
		E o pais 'Pais01' marcado como 'padrão'
		E o pais 'Pais01' marcado como 'inativo'
		E o pais 'Pais02' marcado como 'inativo'
		Quando criar um pais 'Pais03' com situação 'ativo' e marcado como 'padrão'
		Então não exibir a janela de modificação de pais padrão para o pais 'Pais03'

Cenário: 05 - Considerar um novo País como "País Padrão" quando já existir um outro ocupando esse cargo. 
				Deve-se considerar o novo País como "País Padrão" e o antigo não deverá mais ser considerado como "País Padrão".
		Dado o(s) país(es) 'Pais01'
		E o pais 'Pais01' marcado como 'padrão'
		E o pais 'Pais01' marcado como 'ativo'
		Quando criar um pais 'Pais02' com situação 'ativo' e marcado como 'padrão'
		E exibir a janela de modificação de pais padrão para o pais 'Pais02'
		E definir que o país padrão agora deve ser o país 'Pais01'
		Entao o pais 'Pais01' deve estar como 'não padrão'
		E o pais 'Pais02' deve estar como 'padrão'

Cenário: 06 - Cadastrar duas empresas/instituições com o mesmo nome.
				 Não deve permitir cadastrar empresas/instituições com mesmo nome, não diferenciando maiúsculas e minúsculas.
		Dado uma empresa com o nome 'EMPRESA 01' e sigla 'EMP01'
		E uma empresa com o nome 'empresa 01' e sigla 'emp01'
		Quando salvar a empresa 'EMPRESA 01'
		Então a empresa 'empresa 01' não pode ser salva e deve exbir a mensagem 'Já existe uma empresa cadastrada com este mesmo nome'

Cenário: 07 - Criação da Lista de Países que virão no cadastro de Empresa/Instituição.
				Não deve listar países inativos no cadastro de empresa/instituição.
		Dado o(s) país(es) 'Pais01','Pais02'
		E o pais 'Pais02' marcado como 'inativo'
		Quando uma empresa for criada com o nome 'empresa01' e sigla 'emp01'
		Então deveria vir na lista de países o(s) país(es) 'Pais01' para a empresa 'empresa01' 

Cenário: 08 - Haver um País definido como "País Padrão".
				Ao criar uma nova empresa/instituição, deve sugerir automaticamente o "País Padrão".
		Dado o(s) país(es) 'Pais01','Pais02'
		E o pais 'Pais02' marcado como 'padrão'
		Quando uma empresa for criada com o nome 'empresa01' e sigla 'emp01'
		Então o pais da empresa 'empresa01' deverá ser 'Pais02'

Cenário: 09 - Existir dois Países cadastrados. Estar no cadastro de nova Empresa. 
				Alterar automaticamente a máscara do telefone de acordo com o país da empresa/instituição selecionado.
		Dado o(s) país(es) 'Pais01','Pais02'
		E o pais 'Pais02' ter como mascara '(xx)xxxx-xxxx'
		E uma empresa com o nome 'empresa01' e sigla 'emp01'
		Quando selecionar o 'Pais02' para a empresa 'empresa01'
		Então o campo 'Telefone' da empresa 'empresa01' deve estar com a máscara '(xx)xxxx-xxxx'

Cenário: 10 - Existir dois Países cadastrados. Estar no cadastro de nova Empresa.
				Limpar a mascara do telefone quando não houver um país selecionado.
		Dado o(s) país(es) 'Pais01','Pais02'
		E o pais 'Pais02' marcado como 'padrão'
		E o pais 'Pais02' ter como mascara '(xx)xxxx-xxxx'
		E uma empresa com o nome 'empresa01' e sigla 'emp01'
		Quando deselecionar o pais 'Pais02' da empresa 'empresa01'
		Então o campo 'Telefone' da empresa 'empresa01' deve estar com a máscara ''