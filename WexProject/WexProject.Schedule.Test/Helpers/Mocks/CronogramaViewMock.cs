using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Rh;
using System.Drawing;
using WexProject.BLL.Shared.DTOs.Geral;
using System.ComponentModel;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.Helpers.Mocks
{
    /// <summary>
    /// Classe simulando Cronogramaview que possa ser mocada para fins de testes com o presenter
    /// </summary>
    public class CronogramaViewMock :  ICronogramaView
    {
		public virtual void SelecionarCorColaborador( string login , string oidCronograma )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void NotificarMensagem( string titulo , string mensagem )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void AtualizarVisibilidadeBotoesBarra( bool visibilidade )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void AdicionarNovosUsuariosConectados( Dictionary<string , int> coresColaboradores )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void RemoverUsuariosDesconectados( List<string> usuariosDesconectados )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ExecutarAoConectar()
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void ExecutarAoDesconectar()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void HabilitarBotoes( bool estado )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual bool ExibeConfirmacaoAoFechar { get; set; }


		public virtual void NotificarAlerta( string caption , string mensagem )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void Fechar()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ListarCronogramas( List<CronogramaDto> cronogramas , string DescricaoCronogramaSelecionado )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void SetarNomeCronograma( string DescricaoCronogramaSelecionado )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ListarSituacoesPlanejamento( List<SituacaoPlanejamentoDTO> situacoes , SituacaoPlanejamentoDTO situacaoPadrao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void ExibirTarefas( List<CronogramaTarefaGridItem> tarefas )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void SetarSituacaoCronogramaAtual( string situacao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ExibirColaboradoresResponsaveis( List<ColaboradorDto> colaboradores )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void NotificarErro( string caption , string mensagem )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public string NomeCronograma
		{
			get;
			set;
		}

		public string SituacaoCronograma
		{
			get;
			set;
		}

		public virtual void ExcluirCronograma()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual CronogramaTarefaGridItem ConsultarTarefaSelecionada()
		{
			//Implementar caso haja necessidade nos testes unitários
			return null;
		}


		public BindingList<CronogramaTarefaGridItem> TarefasCronograma
		{
			get;
			set;
		}

		public virtual CronogramaTarefaGridItem ConsultarTarefaPorPosicaoSelecionada( int posicao )
		{
			//Implementar caso haja necessidade nos testes unitários
			return null;
		}

		public virtual void InserirTarefaPadrao()
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void DesabilitarViewTarefas( bool parcial = false )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public bool Conectado
		{
			get;
			set;
		}


		public virtual void AdicionarNovosUsuariosConectados( List<CronogramaColaboradorConfigDto> configs )
		{
		}


		public virtual void InserirTarefaPadrao( CronogramaTarefaGridItem tarefa , int posicao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ExibirTarefas()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarTarefaEmSelecao( CronogramaTarefaGridItem tarefa )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void AtualizarTarefasImpactadas( Dictionary<string , short> tarefasImpactadas )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarNomeCronograma( string NovoNome )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void SolicitarExclusaoTarefas()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual bool ContemTarefaEmEdicao
		{
			get;
			set;
		}


		public virtual void NotificarInicioEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem , int> autoresEdicoes )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void NotificarFimEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem , string> tarefasEditadas )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarUltimaAlteracaoCronograma( string notificacao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarHintColaborador( string login , string acao , DateTime quando )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void HabilitarViewTarefas()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarView()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void RetirarOrdenacao()
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void Ordenar()
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual List<Guid> LinhasParaExcluir
		{
			get;
			set;
		}

		public virtual bool ConfirmarExclusaoTarefas()
		{
			//Implementar caso haja necessidade nos testes unitários
			return false;
		}

		public virtual void RemoverTarefas( List<Guid> oidTarefasExcluidas )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void RemoverFocoTarefas()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void RemoverCorEdicao( int? cor )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void RemoverCorEdicao( Guid oid )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void RemoverTarefaRecusadaDeEdicao( Guid oid )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void NotificarInicioEdicaoDadosCronograma( Color cor )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void NotificarFimEdicaoDadosCronograma()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void NotificarMensagemComFoto( string titulo , string mensagem , byte[] foto )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarUltimaAcao( string atualizacao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void ExibirTarefas( System.ComponentModel.BindingList<CronogramaTarefaGridItem> tarefas )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public virtual void AtualizarTarefaEmSelecao( Guid oidTarefaSelecionada )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void AtualizarTarefaEmSelecao( List<Guid> oidTarefaSelecionada )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public virtual void SetarTarefaSelecionada( int indiceTarefaSelecionada )
		{

		}

		public virtual void ForcarFimEdicao()
		{
			//Implementar caso haja necessidade nos testes unitários 
		}

		public virtual void InicializarFormularioTarefaHistoricoView( Guid oidSituacaoPlanejamento )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public void SetarFocoTarefa( int posicao )
		{
			//Implementar caso haja necessidade nos testes unitários
		}

		public void AtribuirFocoTarefa( CronogramaTarefaGridItem cronogramaTarefaGridItem )
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public void HabilitarAoConectar()
		{
			//Implementar caso haja necessidade nos testes unitários
		}


		public Dictionary<string , string> FiltrosSituacao
		{
			get;
			set;
		}


		public void AplicarFiltroSituacao( Library.Domains.CsFiltroSituacaoPlanejamento filtro , string filtroCustom )
		{
		}


		public bool ConfirmarExclusaoCronograma()
		{
			return true;
		}


		public void ForcarEdicaoSituacaoPlanejamentoTarefa()
		{
		}


		public void ForcarSaidaDeFoco()
		{

		}


		public void AtualizarCronogramaSelecionado( CronogramaDto dto )
		{
		}


		public void AtualizarGraficoBurndown( BurndownGraficoDto graficoDto )
		{
			//implementar caso necessário no ambiente de testes
		}


		public bool BurndownVisivel
		{
			get { return false; }
			set { } // sem implementação
		}


		public DateTime DataInicio
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public DateTime DataTermino
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public void ForcarFimEdicaoDadosCronograma()
		{
			throw new NotImplementedException();
		}
	}
}
