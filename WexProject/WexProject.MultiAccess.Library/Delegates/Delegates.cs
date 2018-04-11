using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.MultiAccess.Library.Domains;
using WexProject.MultiAccess.Library.Dtos;

namespace WexProject.MultiAccess.Library.Delegates
{
    /// <summary>
    /// Tipo usado para identificar a assinatura do evento AoFalharConexaoNoServidor
    /// </summary>
    /// <param name="oidCronograma">Nome do cronograma selecionado</param>
    /// <param name="login">Login do colaborador</param>
    public delegate void AoFalharConexaoNoServidorEventHandler( string oidCronograma, string login );

    /// <summary>
    /// Acionar Qualquer Evento que receba por parametro uma MensagemDto
    /// </summary>
    /// <param name="mensagem">MensagemDto utilizada no Evento</param>
    public delegate void MensagemDtoEventHandler( MensagemDto mensagem );

    /// <summary>
    /// Tipo usado para identificar a assinatura do evento AoConectarNovoUsuario
    /// </summary>
    /// <param name="objeto">Mensagem com informações sobre o novo usuário conectado</param>
    /// <param name="login"></param>
    public delegate void AoConectarNovoUsuarioEventHandler( MensagemDto objeto, string login );

    /// <summary>
    /// Responsável por armazenar métodos sem paramentros para chamadas assincronas
    /// </summary>
    public delegate void ChamadaAssincrona();

	/// <summary>
	/// Delegate que representa os eventos de quando forem atualizados a listagem de usuários conectados no servidor
	/// </summary>
	/// <param name="oidCronograma"></param>
	/// <param name="login"></param>
	public delegate void AtualizarUsuariosConectadosEventHandler( string oidCronograma , string login );

	/// <summary>
	/// Delegate que representa eventos de quando forem atualizado a listagem de tarefas em edição
	/// </summary>
	/// <param name="oidCronograma"></param>
	/// <param name="login"></param>
	/// <param name="oidTarefa"></param>
	public delegate void AtualizarEdicoesTarefaEventHandler( string oidCronograma , string login , string oidTarefa );

}
