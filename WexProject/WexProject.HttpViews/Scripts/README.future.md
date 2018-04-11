# Wex Web Application
Esta solu��o cont�m a single page application da solu��o Web do Wex.

## Usando a aplica��o

Instale o Grunt
`npm install -g grunt-cli`

E instale todas as depend�ncias que o reposit�rio necessita:
`npm install`
`bower install`

### Modo de desenvolvimento

Rode o seguinte comando para iniciar o Grunt Server
`grunt server`

Depois rode este comando para visualizar mudan�as em diret�rios e assets em tempo real
`grunt`

Voc� pode acessar a solu��o Web por aqui
`http://localhost:8000`

### Testando

#### Rodando testes

Inicie o servidor para rodar os testes
`grunt server`

Voc� pode rodar su�tes individuais de testes com um dos comandos abaixo:
`grunt test:unit`
`grunt test:midway`
`grunt test:e2e`

Ou todas as su�tes, em ordem:
`grunt test`


#### Observando mudan�as nos testes
Quando observando mudan�as nos testes, qualquer arquivo de spec salvo vai fazer com que o karma rode os testes novamente
para a su�te de testes relacionada.

Inicie o teste para rodar os testes
`grunt server`

Voc� s� pode observer as mudan�as de uma su�te de testes por vez.
`grunt autotest:unit`
`grunt autotest:midway`
`grunt autotest:e2e`