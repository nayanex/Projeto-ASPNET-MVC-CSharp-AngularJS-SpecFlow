#language: pt-br

@pbi_01.02.02
Funcionalidade: Gerente de Projetos informar manualmente o orçamento aprovado por meio de rubricas

@rn
Cenário: 01 - Adicionar aditivo com descrição em branco
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome             |
        | projeto 01       |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome  | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | vazio | "15/04/2013"      | "15/07/2013"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então deve ser exibida a mensagem de erro "Nome inválido!"


Cenário: 02 - Adicionar data de ínicio do projeto em branco
    Dado que exita(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "15/04/2013"      | "15/07/2013"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então deve ser exibida a seguinte mensagem de erro:
            | erro normal      |
            | "Data inválida!" |


Cenário: 03 - Adicionar data de término do projeto em branco
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "15/04/2013"      | "dd/mm/aaaa"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então deve ser exibida a seguinte mensagem de erro:
            | erro normal      |
            | "Data inválida!" |


Cenário: 04 - Adicionar aditivo com data de término do projeto menor que a data de início
    Dado que exita(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "15/07/2013"      | "15/07/2013"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então deve ser exibida a seguinte mensagem de erro:
            | erro normal         |
            | "Período inválido!" |


Cenário: 05 - Calcular Qtde de Meses baseado na data de início e término do projeto
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "18/06/2013"      | "13/11/2013"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então os dados do(s) projeto(s) deve(m) ser:
        | Qtde de Meses |
        | 5             |


Cenário: 06- Adicionar orçamento aprovado em branco
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "18/06/2013"      | "13/11/2013"       | "1.000.000,00"     |
    Quando houver tentativa de salvar
     Então deve ser exibida a seguinte mensagem de erro:
        | erro normal       |
        | "Valor inválido!" |


Cenário: 07 - Um aditivo pode ter mais de um centro de custos
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    Quando houver tentativa de adicionar o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 02 |
     Então salvar o projeto com sucesso


Cenário: 08 - Um aditivo pode ter mais de um patrocinador
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    Quando houver tentativa de adicionar o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 02 |
     Então salvar o projeto com sucesso


Cenário: 09 - Excluir um aditivo caso não haja despesas reais
# Não implementado

Cenário: 10 - Excluir um centro de custo a qualquer momento
    Dado que exista(m) o(s) projeto(s) a seguir:
         | nome           |
         | projeto 01     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    Quando houver tentativa de excluir o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
     Então salvar o projeto com sucesso


Cenário: 11 - Excluir um patrocinador a qualquer momento
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    Quando houver tentativa de excluir o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
     Então salvar o projeto com sucesso


Cenário: 12 - Exibir rubricas padrões para o primeiro ano do aditivo
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "18/06/2013"      | "13/11/2013"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    Quando houver tentativa de carregar os detalhes do aditivo 'aditivo 01'
     Então carregar as rubricas padrões


Cenário: 13 - Exibir rubricas padrões para o ano escolhido
# Cenário falhando
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "18/06/2013"      | "13/11/2014"       | "1.000.000,00"     |
    E que exista(m) o(s) centro(s) de custos a seguir:
        | nome               |
        | centro de custo 01 |
    E que exista(m) o(s) patrocinador(es) a seguir:
        | nome            |
        | patrocinador 01 |
    Quando houver tentativa de carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de carregar as rubricas planejadas para o ano '2014'
     Então carregar as rubricas para o ano escolhido

Cenário: 14 - Adicionar um gasto planejado para o projeto
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
    Quando carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de adicionar a(s) seguinte(s) rubrica(s) no ano '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "662,20" | "662,20" | "662,20" | "662,20" | "662,20" |
     Então salvar o projeto com sucesso


Cenário: 14 - Redistribuir valor de uma rubrica entre todos os meses do projeto
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
    Quando carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de redistribuir o valor da(s) seguinte(s) rubrica(s) no ano '2013':
        | nome                      | tipo              | Total      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" |
     Então o valor por mês deve ser exatamente '662,20'


Cenário: 15 - Adicionar rubrica acima do valor restante
# Cenário falhando
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
    Quando carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de adicionar o valor da(s) seguinte(s) rubrica(s) no ano '2013':
        | nome                      | tipo              | Total            |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "200.000.000,00" |
    E houver tentativa de salvar
     Então deve ser exibida o valor restante em vermelho, entre parênteses

Cenário: 16 - Notificar ao adicionar valores que não batam com o total da rubrica
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
    Quando carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de adicionar o valor da(s) seguinte(s) rubrica(s) no ano '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "663,20" | "662,20" | "662,20" | "662,20" | "662,20" |
    E houver tentativa de salvar
     Então deve ser exibida a seguinte mensagem de erro:
            | erro normal                            |
            | "Soma dos meses é diferente do total!" |


Cenário: 17 - Recalcular o Valor Total do planejamento de um aditivo
    Dado que exista(m) o(s) projeto(s) a seguir:
        | nome       |
        | projeto 01 |
    E que exista(m) o(s) aditivo(s) a seguir:
        | nome       | Início do Projeto | Término do Projeto | Orçamento Aprovado |
        | aditivo 01 | "09/07/2013"      | "18/11/2013"       | "1.000.000,00"     |
     Então o valor total listado deverá ser '1.000.000,00'


Cenário: 18 - Recalcular o Valor Restante do planejamento de um aditivo
# Cenário falhando
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
    Quando carregar os detalhes do aditivo 'aditivo 01'
    E houver tentativa de adicionar o valor da(s) seguinte(s) rubrica(s) no ano '2013':
        | nome                      | tipo              | Total      | Jul      | Ago      | Set      | Out      | Nov      |
        | "Viagens e Deslocamentos" | "Desenvolvimento" | "3.311,00" | "663,20" | "662,20" | "662,20" | "662,20" | "662,20" |
     Então o valor restante listado deverá ser '996.689,00'
