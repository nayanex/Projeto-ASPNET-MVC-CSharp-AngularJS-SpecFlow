#language: pt-br

@pbi_01.02.03
Funcionalidade: Gerente de Projetos informar manualmente as despesas reais do projeto

@rn
Cenario: 01 - Distribuicao das rubricas planejadas, mes a mes, para o primeiro ano
    Dado que o(s) projeto(s) a seguir exista(m):
        | nome       |
        | 01         |
    E que o(s) aditivo(s) a seguir exista(m) para o projeto "01":
        | nome       | Inicio do Projeto | Termino do Projeto | Orcamento Aprovado |
        | 01         | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que o(s) centro(s) de custos a seguir exista(m) para o projeto "01":
        | nome |
        | 01   |
    E que o(s) patrocinador(es) a seguir exista(m) para o projeto "01":
        | nome |
        | 01   |
    E que o orcamento aprovado seguinte exista para o ano de '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "663,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    Quando a acao de salvar for disparada
     Entao distribuir na(s) rubrica(s) correspondente(s) o(s) seguinte(s) valor(es):
        | nome                      | tipo              | Total Planejado     | Planejado Jul | Planejado Ago | Planejado Set | Planejado Out | Planejado Nov |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00"          | "663,20"      | "662,20"      | "662,20"      | "662,20"      | "662,20"      |


Cenario: 02 - Somar aditivos em meses conflitantes
    Dado que o(s) projeto(s) a seguir exista(m):
        | nome |
        | 01   |
    E que o(s) aditivo(s) a seguir exista(m) para o projeto "01":
        | nome | Inicio do Projeto | Termino do Projeto | Orcamento Aprovado |
        | 01   | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
        | 02   | "09/07/2013"      | "18/11/2014"       | "2.000.000,00"     |
    E que o(s) centro(s) de custos a seguir exista(m) para o projeto "01":
        | nome |
        | 01   |
    E que o(s) patrocinador(es) a seguir exista(m) para o projeto "01":
        | nome |
        | 01   |
    E que o orcamento aprovado seguinte exista para o ano de '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "663,20" | "662,20" | "662,20" | "662,20" | "662,20" |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "663,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    Quando a acao de salvar for disparada
     Entao distribuir na(s) rubrica(s) correspondente(s) o(s) seguinte(s) valor(es):
        | nome                      | tipo              | Total Planejado     | Planejado Jul | Planejado Ago | Planejado Set | Planejado Out | Planejado Nov |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "6.622,00"          | "1326,40"     | "1324,40"     | "1324,40"     | "1324,40"     | "1324,40"     |


Cenário: 03 - Informar os gastos reais de uma rubrica
    #
    # As 4 primeiras pre-condicoes se repetem em quase todos os cenarios.
    # Nao seria melhor coloca-las como pre-condicoes de contexto e enxugar todas 
    # as especificacoes? Se nao souber como faz vem aqui que ti mostro.
    # 
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        # remover as aspas duplas dos dados
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome |
        | C01  | # nao usar o nome da entidade no dado.
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome |
        | P01  | # nao usar o nome da entidade no dado.
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    # 1. se o teste eh para testar informar as despesas reais entao o mesmo deveria esar no quando. O BDD instancia os objetos, preenche e salva.
    # 2. comecar a frase pelo requisito que esta sendo testado.    
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
     # 1. O entao normalmente tem um exemplo que sera o assert verificado. Neste caso, precisa ter. O BDD consulta e verifica se os dados foram salvos.
     Então salvar o projeto com sucesso


Cenário: 04 - Replanejar gastos de uma rubrica
# Quais efeitos imediatos ao replanejar um gasto?
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    # A coluna total calculada. Nao vejo necessidade de estar como entrada.
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    # 1. comecar a frase com a entidade a ser testada
    # 2. identificar o projeto.
    # 3. remover as " do conteudo das tabelas. Eles ja chegarao como string.
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jul      | Ago      | Set      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" | "662,20" | "662,20" |
    # 1. identificar o projeto
    # 2. Esse cenario acredito que impacta no campo "diferenca" tambem, pois os valores replanejados nao sao exatamente o somatorio do que tinha planejado.
    Quando informar os seguintes gastos replanejados:
        | nome                      | tipo              | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "0,00"   | "500,00" |
    # 1. identificar o projeto
    # 2. Esse atributo entre " esta sendo selecionado dinamicamente? Ou esta fixo? Posso reutilizar esse entao em outro lugar mudando o nome do campo?
    #    Usar o nome do atributo como parametro (as vezes) nao eh vantajoso pois voce perde auto-complete. 
    Então o "Total Realizado + Previsão" deverá ser de "2486,60"


Cenário: 05 - Assumir baseline caso um valor replanejado não seja informado
# Na planilha, é necessário repetir a baseline caso informe um único replanjamento.
# Caso um replanejamento seja informado e os outros não, assumir a baseline.

# Devemos checar melhor esse cenario. O campo replanejamento so deve ser informado onde nao existe gastos real.
# Caso exista gasto real, ele sera nulo.
# Inclusive, pode ser algo legal pra tratarmos em nivel de tela. Zerar e desabilitar campo do replanejado quando
# for inofrmado gastos reais.
# O replanejado sera usado apenas para meses futuros ainda nao informados.
# Obs: A intensao do titulo do cenario eh boa. So que o exemplo nao diz o comportamento correto.

    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" | "662,20" | "500,20" | "662,20" | "662,20" | "662,20" |
    Quando informar os seguintes gastos replanejados:
        | nome                      | tipo              | Ago      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "500,00" |
    Então o "Total Realizado" deverá ser de "3811,00"

Cenário: 06 - Recalcular o total planejado caso existam novos aditivos
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jun      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" |"662,20" |
    Quando adicionar o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 02 | "19/11/2013"      | "18/11/2014"       | "1.000.000,00"     |
    E que seja(m) adicionado(s) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total     | Dez      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "4635,40" | "662,20" |
    # 1. Seria importante demonstrar o somatorio no mes de dezembro nos asserts.
    #    Entao as despesas reais devem apresentar o orcamento aprovado conforme a seguir:
    Então o "Total Planejado" deverá ser de "4635,40"

Cenário: 07 - Recalcular o total realizado de cada rubrica
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" | "662,20" | "501,20" | "662,20" | "662,20" | "662,20" |
    Quando informar os seguintes gastos replanejados
        | nome                      | tipo              | Ago      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "500,00" |
    # Talvez ficasse melhor assim:
    # Entao as despesas reais terao a rubrica "Viagens e Deslocamentos" conforme a seguir:
    #       | total planejado             | Xx,xx |
    #       | total realizado             | xx,xx |
    #       | total realizado + provicoes | xx,xx |
    #       | diferenca                   | xx,xx |
    # Voce poderia reaproveitar esse entao para varios desses cenarios.
Então o "Total Realizado" deverá ser de "3812,00"


Cenário: 08 - Recalcular a diferença de cada rubrica
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jun      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" |
    # Esta confuso. Onde esta o [Qaundo] deste cenario?
    # Nao consegui identificar qual regra esse cenario testa :).
    # Fica muito estranho um cenario sem um evento disparando a acao a ser testada.
    # O titulo do cenario diz que fara o calculo da [diferenca] mas estamos checando [total planejado]
     Então o "Total Planejado" deverá ser de "3311,00"


Cenário: 09 - Exibir somente rubricas planejadas
# Melhoria (melhor dos mundos) - Não implementada
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    # A especificacao deve ser escrita em um linguagem que um usuario leigo consiga entender.
    # [Renderizar a tela] nao parece ser um termo adequado.
    # Ex: Quando as despesas reais forem listadas
    Quando renderizar a tela de "Gastos Reais" 
    # Entao as despesas reais do projeto 01 devem ter as seguintes rubricas:
     Então mostrar somente a(s) seguinte(s) rubrica(s):
        | nome                      | tipo              |
        | "Viagens e Deslocamentos" | "Desenvolvimento" |


Cenário: 10 - Permitir adicionar manualmente gasto real para uma rubrica nao planejada
# Melhoria (melhor dos mundos) - Não implementada
# Precisa de mudanças na UI e UX (similar ao adicionar rubrica para planejamento)
# Nao entendi esse cenario.
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    Quando tentar adicionar o(s) seguinte(s) gastost reais:
        | nome           | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Treinamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
     Então salvar os gastos reais com sucesso


Cenário: 11 - Calcular a soma de todos os meses de Rubricas a partir de despesas reais para o ano
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    E que exista(m) o(s) seguinte(s) gastos planejados para o ano de '2013':
        | nome                      | tipo              | Total      | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.973,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E que exista(m) o(s) seguinte(s) gastos reais para o ano de '2013':
        | nome                      | tipo              | Jun      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "662,20" | "662,20" | "501,20" | "662,20" | "662,20" | "662,20" |
    Quando informar os seguintes gastos reais
        | nome                      | tipo              | Ago      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "600,00" |
    # Falta o exemplo a ser verificado.
    # Colocar o requisito testado no comeco da frase.
    Então o total para cada mês em "Totalização das despesas" deverá ser a soma dos meses relacionados
    
Cenario: 12 - Como iremos tratar projetos que atrasam? Ou seja, que tem informar meses que nao foram orcados?

# Senti falta de um cenario em que o projeto esteja no vermelho.
