using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace WexProject.Schedule.Library.Libs.GerenciadorComandos
{
    /// <summary>
    /// Classe abstrata base para os comandos que deverão ser acumulados durante a edição de uma tarefa
    /// </summary>
    public abstract class Comando 
    {
        /// <summary>
        /// Atributo responsável por armazenar a instancia do gerenciador de comandos
        /// </summary>
        protected GerenciadorComandos gerenciador;

        /// <summary>
        /// Efetuar Execução de um comando armazenado
        /// </summary>
        public abstract void Executar();

    }
}
