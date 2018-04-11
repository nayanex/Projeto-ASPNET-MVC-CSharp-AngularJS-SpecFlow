#language: pt-br

@pbi_1.02
Funcionalidade:UsuarioTerProjetoSelecionadoAutomaticamente

Contexto:
	Dado que exista(m) o(s) projeto(s) a seguir:
         | nome       | tamanho | total de ciclos | ritmo do time |
         | safira	  | 30      | 3               | 10            |
		 | wex        | 30      | 3               | 10            |
	   E que existam os usuarios:
		 | login			 |
		 | roberto.sousa	 |
		 | alexandre.amorim  |

@rn
Cenário: 01 - Ao selecionar um projeto essa informação deve ser armazenada no sistema vinculada ao usuário atual
    Dado usuario logado for 'roberto.sousa'
  Quando o projeto 'safira' for selecionado pelo usuario 'roberto.sousa'
   Então o projeto 'safira' deveria ter sido salvo como ultimo projeto selecionado para o usuario 'roberto.sousa'

@rn
Cenário: 02 - Ao selecionar outro projeto o sistema deve armazená-lo vinculada ao usuário atual
    Dado usuario logado for 'roberto.sousa'
	   E que o projeto 'safira' tenha sido selecionado anteriormente pelo usuario 'roberto.sousa'
  Quando o projeto 'wex' for selecionado pelo usuario 'roberto.sousa'
   Então o projeto 'wex' deveria ter sido salvo como ultimo projeto selecionado para o usuario 'roberto.sousa'

@rn
Cenário: 03 - Quando o usuário autenticar no sistema o filtro de projeto deve vim preenchido com o ultimo registro selecionado
    Dado que o projeto 'safira' tenha sido selecionado anteriormente pelo usuario 'roberto.sousa'
  Quando usuario logado for 'roberto.sousa'
   Então o projeto 'safira' deve ficar preenchido automaticamente

@rn
Cenário: 04 - Quando o usuário autenticar no sistema e nao tiver selecionado nenhum projeto anteriormente
	Dado que nenhum projeto tenha sido selecionado anteriormente pelo usuario 'alexandre.amorim'
  Quando usuario logado for 'alexandre.amorim'
   Então o filtro de projeto deve ficar em branco

@rn
Cenário: 05 - O sistema nao deve preencher automaticamente projetos selecionados por outros usuários
    Dado que o projeto 'safira' tenha sido selecionado anteriormente pelo usuario 'roberto.sousa'
	   E que o projeto 'wex' tenha sido selecionado anteriormente pelo usuario 'alexandre.amorim'
  Quando usuario logado for 'roberto.sousa'
   Então o projeto 'safira' deve ficar preenchido automaticamente



	