using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WexProject.MultiAccess.Library.Libs
{
    /// <summary>
    /// Classe Criada para padronizar as contantes que serão indices das propriedades armazenadas em hashtables presentes em Dtos
    /// </summary>
    public static class Constantes
    {
        public const string ID_REQUISICAO = "idRequisicao";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma coleção de tarefas impactadas
        /// </summary>
        public const string DATAHORA_ACAO = "dataHoraAcao"; 

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma coleção de tarefas impactadas
        /// </summary>
        public const string TAREFAS_IMPACTADAS = "tarefasImpactadas"; 

        /// <summary>
        /// 
        /// </summary>
        public const string TAREFAS_EXCLUIDAS = "tarefasExcluidas";
        /// <summary>
        /// Armazena uma string indice na hashtable propriedades, responsável por representar um dicionário de tarefas reordenadas 
        /// como chave o oidTarefa e como valor o nbId da tarefa atual.
        /// </summary>
        public const string TAREFAS_REORDENADAS = "tarefasReordenadas";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o oidCronograma atual
        /// </summary>
        public const string OIDCRONOGRAMA = "oidCronograma";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar uma mensagem de comunicação auto-gerada 
        /// (Que não foi gerada pela ação de outro usuário ou resposta a uma ação solicitada pelo usuário).
        /// </summary>
        public const string MOTIVO = "motivo";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar um vetor de usuários (mensagens que podem ser resumidas)
        /// </summary>
        public const string USUARIOS = "usuarios";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o autor da ação que gerou uma mensagem de comunicação.
        /// </summary>
        public const string AUTOR_ACAO = "AutorAcao";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de uma tarefaa atual.
        /// </summary>
        public const string OIDTAREFA = "oidTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o nbId da tarefa (posição da tarefa no grid).
        /// </summary>
        public const string IDTAREFA = "nbIdTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar a descrição de uma nova tarefa
        /// </summary>
        public const string DESCRICAOTAREFA = "txDescricaoTarefa";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o Oid de um usuário.
        /// </summary>
        public const string OIDUSUARIO = "oidUsuario";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar um vetor de oidTarefas
        /// </summary>
        public const string TAREFAS = "tarefas";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar um vetor de oidTarefas
        /// </summary>
        public const string TAREFAS_NAO_EXCLUIDAS = "tarefasNaoExcluidas";

        /// <summary>
        /// Armazena um indice padrão da hashtable propriedades, responsável por representar o login do colaborador proprietário de AccessClientAtual
        /// </summary>
        public const string LOGIN_WEX_CLIENT = "loginWexClientAtual";

        /// <summary>
        /// Armazena um indice padrão na hashtable propriedades da mensagemDto, responsável por representar o dicionário de tarefas que sofreram ação de um colaborador
        /// tendo como key a login do colaborador e como chave o oid da tarefa atual
        /// </summary>
        public const string AUTORES_ACAO = "autoresAcao";

        /// <summary>
        /// Armazena um indice padrão na hashtable propriedades da mensagemDto, responsável por representar as tarefas que se encontram em edição quando um novo usuário se conectar
        /// </summary>
        public const string EDICOES_CRONOGRAMA = "edicoesCronograma";

        /// <summary>
        /// Armazena um indice padrão na hashtable propriedades da mensagemDto, responsável por representar se o login do usuário que está editando o nome do cronograma 
        /// </summary>
        public const string LOGIN_AUTOR_EDICAO_NOME_CRONOGRAMA = "loginAutorEdicaoNomeCronograma";

        /// <summary>
        /// Armazena o indice padrão para a hashtable de propriedade da mensagemDto,
        /// responsável por representar se uma mensagem de usuário desconectado foi solicitada ou forcada
        /// </summary>
        public const string FORCAR_ATUALIZACAO = "forcarAtualizacao";
    }
}
