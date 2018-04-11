#language: pt-br

@pbi_4.07
Funcionalidade: Decidir o que fazer com as estórias pendentes do ciclo
       Como um Líder de Projeto
       Eu quero poder informar se uma estória não concluída será devolvida para lista de prioridades ou movida para o próximo ciclo quando eu encerrar um ciclo
       Para que eu possa ganhar tempo no planejamento dos próximos ciclos

Cenário: 01 - Definir um ciclo sem estórias pendentes como concluído (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Pronto', 'estória 02' - situação 'Pronto', 'estória 03' - situação 'Pronto'
		Quando definir a situação do ciclo 'ciclo 01' como 'Concluído'
		Entao não deverá exibir a janela de destino das estórias pendentes do ciclo 'ciclo 01'

Cenário: 02 - Por padrão, estórias pendentes irão para a lista de 'Próximo Ciclo' (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Pronto', 'estória 02' - situação 'Não Iniciado', 'estória 03' - situação 'Em Desenvolvimento'
		Quando definir a situação do ciclo 'ciclo 01' como 'Concluído'
		Entao deverá exibir a janela de destino das estórias pendentes do ciclo 'ciclo 01'
		E as estórias pendentes do ciclo 'ciclo 01' deverão ser sugeridas na lista de 'Próximo Ciclo'

Cenário: 03 - Subir a posição de estórias selecionadas (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Concluído' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento', 'estória 04' - situação 'Pronto', 'estória 05' - situação 'Não Iniciado', 'estória 06' - situação 'Em Desenvolvimento'
		Quando selecionar as estórias do ciclo 'ciclo 01': 'estória 02', 'estória 03'
		E solicitar subir a posição das estórias selecionadas do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas do ciclo 'ciclo 01' deverão subir uma posição

Cenário: 04 - Subir a posição de estórias selecionadas (quando uma das estiver na primeira posição) (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Concluído' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento', 'estória 04' - situação 'Em Desenvolvimento', 'estória 05' - situação 'Não Iniciado', 'estória 06' - situação 'Em Desenvolvimento'
		Quando selecionar as estórias do ciclo 'ciclo 01': 'estória 01', 'estória 03', 'estória 05'
		E solicitar subir a posição das estórias selecionadas do ciclo 'ciclo 01'
		Entao a(s) estória(s) 'estória 03', 'estória 05' do ciclo 'ciclo 01' deverá(ão) subir uma posição
		E a(s) estória(s) 'estória 01' do ciclo 'ciclo 01' deverá(ão) permenecer na mesma posição, 'subindo'

Cenário: 05 - Descer a posição de estórias selecionadas (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Concluído' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		Quando selecionar as estórias do ciclo 'ciclo 01': 'estória 01', 'estória 02'
		E solicitar descer a posição das estórias selecionadas do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas do ciclo 'ciclo 01' deverão descer uma posição

Cenário: 06 - Descer a posição de estórias selecionadas (quando uma das estiver na última posição) (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Concluído' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento', 'estória 04' - situação 'Em Desenvolvimento'
		Quando selecionar as estórias do ciclo 'ciclo 01': 'estória 01', 'estória 02', 'estória 04'
		E solicitar descer a posição das estórias selecionadas do ciclo 'ciclo 01'
		Entao a(s) estória(s) 'estória 01', 'estória 02' do ciclo 'ciclo 01' deverá(ão) descer uma posição
		E a(s) estória(s) 'estória 04' do ciclo 'ciclo 01' deverá(ão) permenecer na mesma posição, 'descendo'

Cenário: 07 - Enviar estórias selecionadas da lista de 'Próximo Ciclo' para a lista de 'Prioridades' (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		E todas as estórias do ciclo 'ciclo 01' estiverem na lista de 'Próximo Ciclo'
		Quando selecionar as estórias da lista de 'Próximo Ciclo' do ciclo 'ciclo 01': 'estória 01', 'estória 02'
		E solicitar enviar estórias da lista de 'Próximo Ciclo' para a lista de 'Prioridades' do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas deverão sair da lista de 'Próximo Ciclo' e irão para a lista de 'Prioridades' do ciclo 'ciclo 01'

Cenário: 08 - Enviar estórias selecionadas da lista de 'Prioridades' para a lista de 'Próximo Ciclo' (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		E todas as estórias do ciclo 'ciclo 01' estiverem na lista de 'Prioridades'
		Quando selecionar as estórias da lista de 'Prioridades' do ciclo 'ciclo 01': 'estória 01', 'estória 02'
		E solicitar enviar estórias da lista de 'Prioridades' para a lista de 'Próximo Ciclo' do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas deverão sair da lista de 'Prioridades' e irão para a lista de 'Próximo Ciclo' do ciclo 'ciclo 01'

Cenário: 09 - Enviar todas as estórias da lista de 'Próximo Ciclo' para a lista de 'Prioridades' (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		E todas as estórias do ciclo 'ciclo 01' estiverem na lista de 'Próximo Ciclo'
		Quando solicitar enviar todas as estórias da lista de 'Próximo Ciclo' para a lista de 'Prioridades' do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas deverão sair da lista de 'Próximo Ciclo' e irão para a lista de 'Prioridades' do ciclo 'ciclo 01'
		E a lista de 'Próximo Ciclo' do ciclo 'ciclo 01' deve estar vazia

Cenário: 10 - Enviar todas as estórias da lista de 'Prioridades' para a lista de 'Próximo Ciclo' (RF_4.02)
		Dado ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		E todas as estórias do ciclo 'ciclo 01' estiverem na lista de 'Prioridades'
		Quando solicitar enviar todas as estórias da lista de 'Prioridades' para a lista de 'Próximo Ciclo' do ciclo 'ciclo 01'
		Entao todas as estórias selecionadas deverão sair da lista de 'Prioridades' e irão para a lista de 'Próximo Ciclo' do ciclo 'ciclo 01'
		E a lista de 'Prioridades' do ciclo 'ciclo 01' deve estar vazia

Cenário: 11 - Salvar o destino das estórias pendentes (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		Quando definir a situação do ciclo 'ciclo 01' como 'Concluído'
		E a(s) estória(s) 'estória 01' estiver(em) na lista de 'Prioridades' do ciclo 'ciclo 01'
		E a(s) estória(s) 'estória 02', 'estória 03' estiver(em) na lista de 'Próximo Ciclo' do ciclo 'ciclo 01'
		E salvar o destino das estórias pentendes do ciclo 'ciclo 01'
		Entao a(s) estória(s) 'estória 01' deve(m) ser movidas para o backlog
		E a(s) estória(s) 'estória 02', 'estória 03' deve(m) estar como 'Replanejado' no ciclo 'ciclo 01'
		E a(s) estória(s) 'estória 02', 'estória 03' deve(m) estar como 'Não Iniciado' no ciclo 'ciclo 02'

Cenário: 12 - Salvar o destino das estórias pendentes (último ciclo) (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 03' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		Quando definir a situação do ciclo 'ciclo 03' como 'Concluído'
		Entao não deverá exibir a janela de destino das estórias pendentes do ciclo 'ciclo 03'
		E a(s) estória(s) 'estória 01', 'estória 02' deve(m) estar como 'Replanejado' no ciclo 'ciclo 03'
		E a(s) estória(s) 'estória 01', 'estória 02', 'estória 03' deve(m) ser movidas para o backlog

Cenário: 13 - Salvar o destino das estórias pendentes (próximo ciclo concluído) (RF_4.02)
		Dado um projeto 'projeto 01' com o(s) ciclo(s) 'ciclo 01', 'ciclo 02', 'ciclo 03'
		E ciclo 'ciclo 01' na situação 'Em andamento' com as estórias: 'estória 01' - situação 'Não Iniciado', 'estória 02' - situação 'Em Desenvolvimento', 'estória 03' - situação 'Em Desenvolvimento'
		E ciclo 'ciclo 02' na situação 'Concluído' com as estórias: 'estória 04' - situação 'Pronto', 'estória 05' - situação 'Pronto'
		Quando definir a situação do ciclo 'ciclo 01' como 'Concluído'
		E a(s) estória(s) 'estória 02', 'estória 03' estiver(em) na lista de 'Próximo Ciclo' do ciclo 'ciclo 01'
		E a(s) estória(s) 'estória 01' estiver(em) na lista de 'Prioridades' do ciclo 'ciclo 01'
		E salvar o destino das estórias pentendes do ciclo 'ciclo 01'
		Entao a(s) estória(s) 'estória 01' deve(m) ser movidas para o backlog
		E a(s) estória(s) 'estória 02', 'estória 03' deve(m) estar como 'Replanejado' no ciclo 'ciclo 01'
		E a(s) estória(s) 'estória 02', 'estória 03' deve(m) estar como 'Não Iniciado' no ciclo 'ciclo 03'