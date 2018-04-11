# Wex Web Application
Esta solução contém a single page application da solução Web do Wex.

## Usando a aplicação

Instale o Grunt
`npm install -g grunt-cli`

E instale todas as dependências que o repositório necessita:
`npm install`
`bower install`

### Modo de desenvolvimento

Rode o seguinte comando para iniciar o Grunt Server
`grunt server`

Depois rode este comando para visualizar mudanças em diretórios e assets em tempo real
`grunt`

Você pode acessar a solução Web por aqui
`http://localhost:8000`

### Testando

#### Rodando testes

Inicie o servidor para rodar os testes
`grunt server`

Você pode rodar suítes individuais de testes com um dos comandos abaixo:
`grunt test:unit`
`grunt test:midway`
`grunt test:e2e`

Ou todas as suítes, em ordem:
`grunt test`


#### Observando mudanças nos testes
Quando observando mudanças nos testes, qualquer arquivo de spec salvo vai fazer com que o karma rode os testes novamente
para a suíte de testes relacionada.

Inicie o teste para rodar os testes
`grunt server`

Você só pode observer as mudanças de uma suíte de testes por vez.
`grunt autotest:unit`
`grunt autotest:midway`
`grunt autotest:e2e`