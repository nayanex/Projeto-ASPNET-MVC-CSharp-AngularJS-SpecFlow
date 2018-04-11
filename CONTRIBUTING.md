## Como reportar um bug?
O seguinte formato deve ser utilizado:

    ## Passos: ##
    _Passos para reproduzir bug_

    1. Primeiro passo para o bug;
    2. Segundo passo;
    3. Último passo.

    ## Observado: ##
    _O que foi observado._

    Eu observei a Matrix.

    ## Esperado: ##
    _O que deveria ocorrer._

    Eu deveria ter observado o Mundo.

## Contribuindo com codificação:

Você deve criar um _fork_ desde repositório e clonar o repositório gerado para trabalhar na feature ou correção de bug desejada. Para saber mais sobre o esquema de _fork_:
https://www.atlassian.com/git/workflows/#!workflow-forking

## Definição de pronto
_Responsável: Time_

**Será considerado pronto quando:**
1. Todos os critérios de aceitação das estórias estarem atendidos;
2. A estória estiver implantada no servidor de produção;
3. Não houver nenhum bug que inviabilize o uso dos critérios de aceitação estabelecidos;
4. Cobertura de testes unitários para JavaScript e C#;
5. A API precisa estar 100% documentada em branchs de features ou em branchs de bugfix caso a interface tenha sido alterada ou incrementada.

## Definição de preparado 
_Responsáveis: Alexandre Amorim e Diogo Riker_

**A estória será considerada preparada/apta para entrar na Sprint quando:**
1. A UX da estória estiver pronta e aprovada pelo PO; 
2. A UI da estoria estiver pronta e aprovada pelo PO;
3. Os critérios de aceitação estiverem estabelecidos pelo PO e acordados com o time;
4. Preferencialmente, a estória deve ter no máximo 3 pontos;
5. Preferencialmente, O BDD deverá ser especificado pelo PO e acordado com o time;

### Não entrarão:
1. Pull requests sem cobertura de teste unitário, mesmo em correções de bugs. Os testes garantirão que um bug não aconteça novamente;
2. Pull requests com o build quebrando;
3. Algo que a critério do revisor seja considerado código não limpo.

Você pode efetuar novos commits no mesmo pull request para resolver algum problema apontado pelo revisor. 

### Você não pode:

1. Commitar diretamente no repositório principal, na branch master, mesmo que você seja um revisor ou mesmo que você seja Deus;
2. Aprovar seus próprios pull requests. Sendo revisor, solicite revisão de um colega;
3. Ser indelicado.

### O revisor não pode:

1. Fechar imediatamente o seu pull request sem justificativa. Em caso de não aprovação inicial, os problemas serão apontados para que você tenha oportunidade de corrigi-los;
2. Em caso de escopo novo de iniciativa voluntárias o seu escopo não pode ser negado a menos que três revisores o recusem igualmente. A ser discutido na issue aberta pelo pull request;
3. Ser indelicado. 

Em caso de dúvidas, mande um e-mail para wex.terminators@fpf.br ou diretamente para ayrton.junior@fpf.br, revisor reponsável.