#language: pt-br

@pbi_4.06
Funcionalidade: Cancelamento de ciclo de desenvolvimento do projeto
       Como um Líder de Projeto
       Eu quero poder cancelar um ciclo de desenvolvimento
       Para que eu possa registrar quando um ciclo não pode ser realizado por interferências externas e terei todos os meus ciclos seguintes replanejados

Cenário: 01 - Excluir um motivo de cancelamento que está sendo usado (RF_3.05)
		Dado um motivo de cancelamento 'motivo 01' usado no cancelamento de um ciclo 'ciclo 01'
		Quando excluir o motivo de cancelamento 'motivo 01'
		Entao exibir a excessão 'O Motivo de Cancelamento está sendo usado por um Cancelamento de Ciclo'

Cenário: 02 - Cancelar um ciclo com a situação diferente de "Não Iniciado" (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		Quando cancelar o ciclo 'ciclo 01'
		Entao exibir mensagem "Não se pode cancelar um Ciclo que não esteja com a situação 'Não Iniciado'" para o cancelamento do ciclo 'ciclo 01'

Cenário: 03 - Obter a data sugerida no cancelamento de Ciclo (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 01'
		Entao o campo "data de início do próximo ciclo" deve ser exibido no cancelamento do ciclo 'ciclo 01'
		E o campo "data de início do próximo ciclo" no cancelamento do ciclo 'ciclo 01' deve estar com um valor '21/02/2012'

Cenário: 04 - Obter, para o cancelamento de ciclo, o Motivo de cancelamento (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E um motivo 'motivo 02'
		E ciclo 'ciclo 01' na situação 'Cancelado - motivo 01', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 02'
		Entao o campo "motivo" no cancelamento do ciclo 'ciclo 02' deve estar com o motivo 'motivo 01'

Cenário: 05 - Cancelar um ciclo quando a data de término do ciclo atual seja menor que a data atual (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '02/05/2012'
		Quando cancelar o ciclo 'ciclo 01' com o motivo 'motivo 01'
		Entao o campo "data de início do próximo ciclo" não deve ser exibido no cancelamento do ciclo 'ciclo 01'
		E o ciclo 'ciclo 01' deve ser cancelado
		E o campo "data de início" do ciclo 'ciclo 01' deve ser '05/03/2012'
		E o campo "data de término" do ciclo 'ciclo 01' deve ser '30/03/2012'

Cenário: 06 - Cancelar um ciclo quando existir, nos itens posteriores ao mesmo, ao menos algum com a situação diferente de "Não Planejado" (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Concluído', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Cancelado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Cancelado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 01' com o motivo 'motivo 01'
		Entao o campo "data de início do próximo ciclo" não deve ser exibido no cancelamento do ciclo 'ciclo 01'
		E o ciclo 'ciclo 01' deve ser cancelado
		E o campo "data de início" do ciclo 'ciclo 01' deve ser '05/03/2012'
		E o campo "data de término" do ciclo 'ciclo 01' deve ser '30/03/2012'

Cenário: 07 - Cancelar o último ciclo do projeto
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 03' com o motivo 'motivo 01'
		Entao o campo "data de início do próximo ciclo" não deve ser exibido no cancelamento do ciclo 'ciclo 03'
		E o ciclo 'ciclo 03' deve ser cancelado
		E o campo "data de início" do ciclo 'ciclo 03' deve ser '02/05/2012'
		E o campo "data de término" do ciclo 'ciclo 03' deve ser '29/05/2012'

Cenário: 08 - Cancelar um ciclo quando todos os itens posteriores ao mesmo estiverem com a situação "Não Iniciado" (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 01' com o motivo 'motivo 01' e data de início do próximo ciclo com '28/03/2012'
		Entao o ciclo 'ciclo 01' deve ser cancelado
		E o campo "data de início" do ciclo 'ciclo 01' deve ser '05/03/2012'
		E o campo "data de término" do ciclo 'ciclo 01' deve ser '27/03/2012'
		E o campo "data de início" do ciclo 'ciclo 02' deve ser '28/03/2012'
		E o campo "data de término" do ciclo 'ciclo 02' deve ser '10/04/2012'
		E o campo "data de início" do ciclo 'ciclo 03' deve ser '11/04/2012'
		E o campo "data de término" do ciclo 'ciclo 03' deve ser '24/04/2012'

Cenário: 09 - Cancelar um ciclo com a "data de início do próximo ciclo" com uma data menor que o início do ciclo atual (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando validar o cancelamento do ciclo 'ciclo 01' passando a data '04/03/2012'
		Entao exibir mensagem para o ciclo 'ciclo 01' 'A data de Início do Próximo Ciclo deve estar entre 06/03/2012 e 03/04/2012' para o cancelamento com a data incorreta

Cenário: 10 - Cancelar um ciclo com a "data de início do próximo ciclo" com uma data igual a do início do ciclo atual (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando validar o cancelamento do ciclo 'ciclo 01' passando a data '05/03/2012'
		Entao exibir mensagem para o ciclo 'ciclo 01' 'A data de Início do Próximo Ciclo deve estar entre 06/03/2012 e 03/04/2012' para o cancelamento com a data incorreta

Cenário: 11 - Cancelar um ciclo com a "data de início do próximo ciclo" com uma data maior que o início do próximo ciclo (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando validar o cancelamento do ciclo 'ciclo 01' passando a data '04/04/2012'
		Entao exibir mensagem para o ciclo 'ciclo 01' 'A data de Início do Próximo Ciclo deve estar entre 06/03/2012 e 03/04/2012' para o cancelamento com a data incorreta

Cenário: 12 - Concatenar a situação do ciclo com o motivo de cancelamento (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '02/05/2012'
		Quando cancelar o ciclo 'ciclo 01' com o motivo 'motivo 01'
		Entao a situação do ciclo 'ciclo 01' deverá ser 'Cancelado - motivo 01'

Cenário: 13 - Definir um ciclo sem estórias pendentes como cancelado (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '02/05/2012'
		Quando cancelar o ciclo 'ciclo 01' com o motivo 'motivo 01'
		Entao não deverá exibir, na janela de cancelamento do ciclo 'ciclo 01', a área de destino de estórias pendentes

Cenário: 14 - Obter a Lista de Motivos Ativos (RF_3.05)
		Dado os motivos de cancelamento 'motivo 01' - status 'Ativo', 'motivo 02' - status 'Ativo', 'motivo 03' - status 'Ativo', 'motivo 04' - status 'Inativo', 'motivo 05' - status 'Inativo', 'motivo 06' - status 'Ativo'
		Quando obter lista de motivos ativos
		Entao os motivos 'motivo 01', 'motivo 02', 'motivo 03', 'motivo 06' devem vir na lista de motivos ativos

Cenário: 15 - Salvar o cancelamento de Ciclo sem informar um Motivo (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E um motivo 'motivo 01'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '20/02/2012'
		Quando cancelar o ciclo 'ciclo 01'
		E validar o cancelamento do ciclo 'ciclo 01' sem passar o motivo
		Entao exibir mensagem para o ciclo 'ciclo 01' 'É necessário informar um Motivo de Cancelamento' para o cancelamento sem motivo

Cenário: 16 - O Ciclo só poderá ser cancelado se seu periodo estiver no passado data atual (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Não Planejado', data de início no dia '05/03/2012' e data de término no dia '30/03/2012'
		E ciclo 'ciclo 02' na situação 'Não Planejado', data de início no dia '03/04/2012' e data de término no dia '30/04/2012'
		E ciclo 'ciclo 03' na situação 'Não Planejado', data de início no dia '02/05/2012' e data de término no dia '29/05/2012'
		E data atual for '28/04/2012'
		Quando cancelar o ciclo 'ciclo 03'
		Entao o 'ciclo 03' não deverá ser cancelado