using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Serializing;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Ribbon;
using System.Linq;
using System.IO;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using WexProject.Schedule.Library.Presenters;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.ComponentModel;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Schedule.Library.Views.Interfaces;
using WexProject.Schedule.Library.Domains;
using WexProject.Schedule.Library.Views.Components;
using WexProject.Schedule.Library.Properties;
using DevExpress.XtraGrid;
using WexProject.Schedule.Library.Libs.CrontroleMovimentacao;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using WexProject.Library.Libs.Img;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.Schedule.Library.Libs.ExtensionMethods;
using WexProject.Schedule.Library.Helpers;
using WexProject.Library.Libs.Logger;
using WexProject.Schedule.Library.Libs.Configuracoes;
using WexProject.Library.Libs.DataHora.Extension;
using DevExpress.XtraCharts;

namespace WexProject.Schedule.Library.Views.Forms
{
    /// <summary>
    /// Tela de Visualização do Cronograma
    /// </summary>
    public partial class CronogramaView : XtraForm, ICronogramaView
    {
        #region Atributos

        /// <summary>
        /// utilizado pelo tarefaHistoricoPresenter para autorizar ou não salvar o histórico
        /// </summary>
        public static bool autorizarSalvarEdicao;

        private string BURNDOWN_LAYOUT_CONFIG_PATH;
        private string GRID_LAYOUT_CONFIG_PATH;

        private const string BURNDOWN_LAYOUT_CONFIG_FILENAME = "burndownConfig.xml";
        private const string GRID_LAYOUT_CONFIG_FILENAME = "gridTarefasConfig.xml";

        /// <summary>
        /// Propriedade responsável por sinalizar se o encerramento da aplicação deve ou não solicitar a confirmação 
        /// (necessário quando a aplicação precisar ser encerrada pela regra de négocio)
        /// </summary>
        public bool ExibeConfirmacaoAoFechar { get; set; }

        /// <summary>
        /// responsável por armazenar as informaçõs do ultimo click do usuário
        /// </summary>
        private GridHitInfo downHitInfo = null;

        /// <summary>
        /// Responsável por armazenar as cores dos usuários conectados atualmente
        /// </summary>
        private Dictionary<string, Color> coresUsuarios;

        /// <summary>
        /// Responsável por armazenar as tarefas que se encontram atualmente em edição tal como a cor definida do usuário que está
        /// editando a tarefa
        /// </summary>
        private Dictionary<CronogramaTarefaGridItem, Color> coresTarefasEmEdicao;

        /// <summary>
        /// Objeto provisório para guardar valor de célula
        /// </summary>
        private TimeSpan valorCelulaRealizado;

        ///<summary>
        /// Objeto provisório para guardar valor de célula
        /// </summary>
        private TimeSpan valorCelulaInicial;

        /// <summary>
        /// atributo responsável pela execução das regras de negócios da tela
        /// </summary>
        private CronogramaPresenter presenter;

        /// <summary>
        /// responsável por armazenas as tarefas do cronograma
        /// </summary>
        private BindingList<CronogramaTarefaGridItem> tarefasCronograma;

        /// <summary>
        /// Objeto que auxilia o bloqueio quando estiver ordenando e retirando ordenação o grid.
        /// </summary>
        private readonly object bloquearThreadOrdenacao = new object();

        /// <summary>
        /// responsável por armazenar se uma tarefa acabou de ser criada
        /// </summary>
        private bool tarefaRecentementeCriada;

        /// <summary>
        /// responsável por armazenar se a janela da view foi inicializada
        /// </summary>
        private bool janelaInicializada;

        /// <summary>
        /// Armazenar os atalhos para a galeria de situações planejamento
        /// </summary>
        private Dictionary<string, string> atalhosGaleriaSituacaoPlanejamento;

        /// <summary>
        /// Armazenar se é permitido ocorrer edição na tela
        /// Usado quando a o gerenciador de comandos está atualizando a view
        /// </summary>
        bool permitidoEditar = true;

        /// <summary>
        /// Armazena os filtros da situação planejamento
        /// </summary>
        public Dictionary<string, string> FiltrosSituacao { get; set; }
        #endregion

        #region Eventos

        /// <summary>
        /// delegate sem parametros utilizado para invocar algum elemento da tela
        /// Obs:
        ///   Devido a utilização de threads na aplicação, quando uma thread do MultiAccessClient dispara
        ///   um evento, caso este evento utilize algum recurso da tela, ocorrerá um erro de operação inválida entre threads
        ///   para que não haja tal erro, deve-se fazer um invoke de forma que o invoke ative a chamada da tela
        /// </summary>
        delegate void InvocarComportamentoTelaHandler();

        /// <summary>
        /// Delegador utilizado para invocar comportamentos de tela solicitados pela thread do MultiAccessClient referentes
        /// a utilização da barra de usuários conectados
        /// </summary>
        /// <param name="usuario"></param>
        delegate void ControleDePainelHandler( string usuario );

        #endregion

        #region Propriedades

        /// <summary>
        /// Responsável por armazenar a descricao do cronograma
        /// </summary>
        public string NomeCronograma
        {
            get
            {
                return barraTxDescricaoCronograma.EditValue as string;
            }
            set
            {
                barraTxDescricaoCronograma.EditValue = value;
            }
        }

        /// <summary>
        /// Responsável por armazenar a situação do cronograma
        /// </summary>
        public string SituacaoCronograma
        {
            get { return barraTxDescricaoSituacaoPlanejmaneto.EditValue.ToString(); }
            set { barraTxDescricaoSituacaoPlanejmaneto.EditValue = value; }
        }

        /// <summary>
        /// Propriedade para disponibilizar as tarefas do cronogram ao presenter
        /// </summary>
        public BindingList<CronogramaTarefaGridItem> TarefasCronograma
        {
            get { return tarefasCronograma; }

            set
            {
                tarefasCronograma = value;
                ExibirTarefas( tarefasCronograma );
            }
        }

        /// <summary>
        /// Atributo responsável por armazenar as linhas selecionadas para exclusão.
        /// </summary>
        public List<Guid> LinhasParaExcluir
        {
            get;
            set;
        }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor recebendo a Sessão do Banco
        /// </summary>
        public CronogramaView()
        {
            InitializeComponent();
            InicializacaoVariaveis();
            InicializarLayoutConfigPaths();
            LimparGrafico();
        }

        /// <summary>
        /// Método responsável por inicializar o caminho dos arquivos de configuração de layout
        /// </summary>
        private void InicializarLayoutConfigPaths()
        {
            BURNDOWN_LAYOUT_CONFIG_PATH = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, BURNDOWN_LAYOUT_CONFIG_FILENAME );
            GRID_LAYOUT_CONFIG_PATH = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, GRID_LAYOUT_CONFIG_FILENAME );
        }

        /// <summary>
        /// Método Auxiliar responsável por efetuar a inicialização de todas váriaveis necessárias;
        /// </summary>
        private void InicializacaoVariaveis()
        {
            coresUsuarios = new Dictionary<string, Color>();
            coresTarefasEmEdicao = new Dictionary<CronogramaTarefaGridItem, Color>();
            LinhasParaExcluir = new List<Guid>();
            FiltrosSituacao = new Dictionary<string, string>();
            presenter = new CronogramaPresenter( this, MultiAccessClient );
            ExibeConfirmacaoAoFechar = true;
            CronogramaTarefaGridItem.AoAlterarIconeTarefa += CronogramaTarefaGridItem_AoAlterarIconeTarefa;
        }

        /// <summary>
        /// Método respresentando o comportamento da aplicação quando houver alteração no icone da tarefa
        /// </summary>
        /// <param name="sender">CronogramaTarefaGridItem</param>
        /// <param name="e"></param>
        void CronogramaTarefaGridItem_AoAlterarIconeTarefa( object sender, EventArgs e )
        {
            CronogramaTarefaGridItem gridItem = sender as CronogramaTarefaGridItem;
            if(gridItem != null)
            {
                AtualizarTarefaEmSelecao( gridItem.OidCronogramaTarefa );
            }
        }

        #endregion

        #region Ações da Tela

        /// <summary>
        /// Responsável pelo comportamento do botão de fechar localizado na Barra de Menu /// </summary>
        /// <param name="sender">objeto ativador do evento</param>
        /// <param name="e">paramentros de click de mouse relativos ao evento</param>
        private void barraBotao_Fechar_ItemClick( object sender, ItemClickEventArgs e )
        {
            Close();
        }

        /// <summary>
        /// Método Responsável pelo comportamento de tela necessário enquanto o formulário estiver fechando
        /// </summary>
        /// <param name="sender">objeto ativador do evento</param>
        /// <param name="e">paramentros relativos ao evento de fechamento do formulário</param>
        private void TelaCronograma_FormClosing( object sender, FormClosingEventArgs e )
        {
            if(ExibeConfirmacaoAoFechar)
            {
                DialogResult resultado = XtraMessageBox.Show( Resources.Alerta_Fechar_Cronograma, Resources.Caption_Fechar, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2 );
                if(resultado == DialogResult.No)
                {
                    e.Cancel = true;

                }
                else
                {
                    SalvarConfiguracoesLayout();
                    presenter.EncerrarConexao();
                }
            }
        }

        /// <summary>
        /// Método responsável por salvar as configurações de layout
        /// </summary>
        private void SalvarConfiguracoesLayout()
        {
            SalvarLayoutConfig( dockManager, BURNDOWN_LAYOUT_CONFIG_PATH );
            SalvarLayoutConfig( GridView, GRID_LAYOUT_CONFIG_PATH );
        }

        /// <summary>
        /// método responsável por salvar as configurações de um layout em xml
        /// </summary>
        /// <param name="component">componente que terá suas configurações salvas em xml</param>
        /// <param name="nomeArquivoXmlLayoutConfig">caminho do arquivo xml que armazena as configurações</param>
        private void SalvarLayoutConfig( ISupportXtraSerializer component, string nomeArquivoXmlLayoutConfig )
        {
            if(component == null || string.IsNullOrEmpty( ( nomeArquivoXmlLayoutConfig ) ))
                return;
            component.SaveLayoutToXml( nomeArquivoXmlLayoutConfig );
        }

        /// <summary>
        /// Método responsável por carregar as configurações de layout de um componente a partir do arquivo de xml
        /// </summary>
        /// <param name="component"></param>
        /// <param name="nomeArquivoXmlLayoutConfig"></param>
        private void CarregarLayoutConfig( ISupportXtraSerializer component, string nomeArquivoXmlLayoutConfig )
        {
            if(component == null || string.IsNullOrEmpty( ( nomeArquivoXmlLayoutConfig ) ))
                return;

            var fileInfo = new FileInfo( nomeArquivoXmlLayoutConfig );
            if(!fileInfo.Exists)
            {
                SalvarLayoutConfig( component, nomeArquivoXmlLayoutConfig );
                return;
            }

            component.RestoreLayoutFromXml( nomeArquivoXmlLayoutConfig );
        }

        /// <summary>
        /// Método responsável por inicializar as configurações dos layouts salvos
        /// </summary>
        private void InicializarConfiguracoesLayout()
        {
            CarregarLayoutConfig( dockManager, BURNDOWN_LAYOUT_CONFIG_PATH );
            CarregarLayoutConfig( GridView, GRID_LAYOUT_CONFIG_PATH );
        }



        /// <summary>
        /// método responsável por efetuar comportamentos ao formulário carregar
        /// </summary>
        /// <param name="sender">objeto ativador do evento</param>
        /// <param name="e">paramentros relativos ao evento de carregamento do formulário</param>
        private void CronogramaView_Load( object sender, EventArgs e )
        {
            InicializarConfiguracoesLayout();
            tarefaHistoricoView1.AoSalvarHistoricoTarefa += AtualizarDadosTarefa;
            tarefaHistoricoView1.AoFecharJanela += ClosePopUp;
            presenter.InicializarCronograma();
            janelaInicializada = true;
        }



        /// <summary>
        /// Responsável por executar ação quando for solicitado o fechamento do formulário
        /// </summary>
        /// <param name="sender">Objeto que solicitou o fechamento</param>
        /// <param name="e"> Argumentos do evento de click</param>
        private void barraBotao_Fechar_ItemClick_1( object sender, ItemClickEventArgs e )
        {
            ExibeConfirmacaoAoFechar = true;
            Close();
        }

        /// <summary>
        /// Evento acionado quando o valor do combo box de cronogramas é modificado.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void barraCronogramaSelecionado_EditValueChanged( object sender, EventArgs e )
        {
            if(presenter == null)
                return;

            BarEditItem comboCronogramaSelecionado = sender as BarEditItem;

            string descricaoCronograma = comboCronogramaSelecionado.EditValue.ToString();

            if(!string.IsNullOrEmpty( descricaoCronograma ))
                presenter.SelecionarCronograma( descricaoCronograma );
        }

        #endregion

        #region Regras de Tela

        /// <summary>
        /// Método responsável por reinicializar a barra de usuário conectados nas trocas de cronograma
        /// </summary>
        public void ReinicializarPainelUsuariosConectados()
        {
            coresUsuarios = new Dictionary<string, Color>();
            barraUsuariosConectados.RemoverTodosUsuariosDoPanel();
        }

        #endregion

        #region Criar Cronograma

        /// <summary>
        /// Método acionado quando usuário clica no Botão de novo cronograma
        /// Deve criar um novo cronograma.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void barraBotaoNovoCronograma_ItemClick( object sender, ItemClickEventArgs e )
        {
            presenter.IncluirNovoCronograma();
        }


        #endregion

        #region Combo Situação e Colaboradores

        /// <summary>
        /// Regra que preenche todas as situações de planejamento ativas na barra de ferramentas (ribbom).
        /// </summary>
        /// <param name="imagens">Coleção de imagens</param>
        /// <param name="situacoes">List de situações planejamento Dto</param>
        private void PreencherComboSituacoesPlanejamentoBarra( ImageCollection imagens, List<SituacaoPlanejamentoDTO> situacoesPlanejamento )
        {
            if(atalhosGaleriaSituacaoPlanejamento == null)
                atalhosGaleriaSituacaoPlanejamento = new Dictionary<string, string>();

            if(ribbonGaleriaImagensSituacaoPlanejamento.Gallery.Groups.Count == 0)
            {
                ribbonGaleriaImagensSituacaoPlanejamento.Gallery.Images = imagens;
                ribbonGaleriaImagensSituacaoPlanejamento.Gallery.HoverImages = imagens;
                ribbonGaleriaImagensSituacaoPlanejamento.Gallery.AllowHoverImages = true;
                ribbonGaleriaImagensSituacaoPlanejamento.ShortcutKeyDisplayString = Keys.A.ToString();

                GalleryItemGroup grupoItens = new GalleryItemGroup();

                ribbonGaleriaImagensSituacaoPlanejamento.Gallery.Groups.Add( grupoItens );
                grupoItens.Items.Clear();

                for(int i = 0; i < situacoesPlanejamento.Count; i++)
                {
                    GalleryItem item = new GalleryItem();

                    item.ImageIndex = item.HoverImageIndex = i;
                    item.Hint = situacoesPlanejamento[i].TxDescricao;
                    item.Caption = situacoesPlanejamento[i].TxDescricao;
                    item.Description = situacoesPlanejamento[i].KeyPress.ToString();
                    atalhosGaleriaSituacaoPlanejamento.Add( item.Description, situacoesPlanejamento[i].TxKeys );
                    grupoItens.Items.Add( item );
                }

                ribbonGaleriaImagensSituacaoPlanejamento.Gallery.ColumnCount = 3;
            }
        }

        #endregion

        #region MultiAccessClient

        /// <summary>
        /// Responsável por desabilitar os campos do grid quando ocorre a perda de conexão com o MultiAccessManager
        /// </summary>
        private void DesabilitarColunasGrid()
        {
            if(janelaInicializada)
            {
                try
                {
                    BeginInvoke( new Action( () =>
                    {
                        foreach(GridColumn coluna in GridView.Columns)
                        {
                            if(coluna == gridColunaObs)
                            {
                                coluna.OptionsColumn.AllowEdit = true;
                            }
                            else
                            {
                                coluna.OptionsColumn.AllowEdit = false;
                            }
                        }
                    } ) );
                }
                catch(Exception excessao)
                {
                    presenter.accessClient_LogarAoOcorrerException( excessao, new EventArgs() );
                }
            }
        }

        /// <summary>
        /// Método responsável por desabilitar a view de tarefas;
        /// </summary>
        public void DesabilitarViewTarefas( bool parcial = false )
        {
            if(parcial)
                DesabilitarColunasGrid();
            else
                this.SafeInvoke( o =>
                {
                    permitidoEditar = false;
                } );
        }

        /// <summary>
        /// Método responsável por habilitar a view das tarefas
        /// </summary>
        public void HabilitarViewTarefas()
        {
            BeginInvoke( new Action( () =>
            {
                if(!permitidoEditar)
                {
                    permitidoEditar = true;
                    return;
                }

                foreach(GridColumn coluna in GridView.Columns)
                {
                    if(coluna != gridColunaAtualizadoEm && coluna != gridColunaAtualizadoPor && coluna != gridColunaIcone && coluna != gridColunaRealizado)
                        coluna.OptionsColumn.AllowEdit = true;
                }
            } ) );
        }

        /// <summary>
        /// Método associado ao Evento de Edição de uma coluna evitando que ela seja modificada
        /// </summary>
        /// <param name="sender">objeto que disparou o evento</param>
        /// <param name="e">parametros gerenciamento do evento</param>
        private void RealColumnEdit_EditValueChanging( object sender, ChangingEventArgs e )
        {
            e.Cancel = true;
        }
        #endregion

        #region Validações

        /// Método responsável por validar qual aba será exibida como padrão 
        /// quando o usuário acessar o cronograma
        /// </summary>
        private void RnValidarAba()
        {
            //Caso tenha alguma tarefa no Grid, ativa como padrão a aba Tarefas
            if(cronogramaTarefaGridControl.DefaultView.RowCount > 0)
            {
                Tarefas.Ribbon.ShowExpandCollapseButton = DefaultBoolean.True;
                Tarefas.Visible = true;
                Menu.SelectedPage = Tarefas;
            }
            //Caso não tenha alguma tarefa no Grid, ativa como padrão a aba Propriedades
            else
            {
                Planejamento.Category.Expanded = true;
                Planejamento.Ribbon.Select();
                Menu.SelectedPage = Planejamento;
            }
        }

        #endregion

        ///<summary>
        /// Responsável por habilitar ou desabilitar os componentes de tela
        /// </summary>
        /// <param name="estado">
        /// True - Habilitado
        /// False - Desabilitado
        /// </param>
        public void HabilitarBotoes( bool estado )
        {
            if(janelaInicializada)
                BeginInvoke( new Action( () =>
                {
                    barraBtNovoCronograma.Enabled = estado;
                    barraBtExcluirCronograma.Enabled = estado;
                    barraBtAtualizarCronograma.Enabled = estado;
                    barraTxDescricaoCronograma.Enabled = estado;
                    barraBtNovaTarefa.Enabled = estado;
                    barraBtExcluirTarefa.Enabled = estado;
                    ribbonGaleriaImagensSituacaoPlanejamento.Enabled = estado;
                } ) );
        }

        /// <summary>
        /// Método responsável por atualizar a visibilidade dos botões 
        /// caso exista ou não cronograma selecionado.
        /// </summary>
        /// <param name="status">Booleano que determinado se botões são visíveis ou não.</param>
        public void AtualizarVisibilidadeBotoesBarra( bool status )
        {
            Tarefas.Visible = status;
            cronogramaTarefaGridControl.Enabled = status;
            barraBtExcluirCronograma.Enabled = status;
            barraBtAtualizarCronograma.Enabled = status;
            barraTxDescricaoCronograma.Enabled = status;
            Menu.ShowToolbarCustomizeItem = status;
        }

        /// <summary>
        /// Inicializar o popup da tarefa historico view
        /// </summary>
        public void InicializarFormularioTarefaHistoricoView( Guid oidSituacao )
        {
            using(XtraForm formulario = new XtraForm())
            {
                using(TarefaHistoricoView tarefaHistoricoView = new TarefaHistoricoView())
                {
                    formulario.Name = "formSituacaoPopup";
                    formulario.Controls.Add( tarefaHistoricoView );
                    formulario.Width = tarefaHistoricoView.Width + 20;
                    formulario.Height = tarefaHistoricoView.Height + 40;
                    formulario.StartPosition = FormStartPosition.CenterParent;
                    formulario.ShowIcon = false;
                    formulario.MaximizeBox = false;
                    formulario.MinimizeBox = false;
                    formulario.FormClosed += formulario_FormClosed;
                    formulario.Text = "Estimativa de esforço realizado";
                    CronogramaTarefaGridItem tarefaGridItem = GridView.GetFocusedRow() as CronogramaTarefaGridItem;
                    tarefaHistoricoView.AoSalvarHistoricoTarefa += AtualizarDadosTarefa;
                    tarefaHistoricoView.AoFecharJanela += ClosePopUp;
                    tarefaHistoricoView.popupComboImagensSituacaoPlanejamento.Properties.Items.AddRange( comboGridSituacaoPlanejamento.Items );
                    tarefaHistoricoView.popupComboImagensSituacaoPlanejamento.Properties.SmallImages = comboGridSituacaoPlanejamento.SmallImages;
                    tarefaHistoricoView.InicializarSituacoesPlanejamento( presenter.SituacoesPlanejamento );
                    tarefaHistoricoView.InicializarJanela( tarefaGridItem, presenter.ColaboradorLogado.Login, oidSituacao );
                    if(formulario.ShowDialog() == DialogResult.OK)
                    {
                        AtualizarTarefaEmSelecao( tarefaGridItem.OidCronogramaTarefa );
                    }
                }
            }
        }

        /// <summary>
        /// Método disparado quando o formulário de situação planejamento for fechado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formulario_FormClosed( object sender, FormClosedEventArgs e )
        {
            XtraForm form = sender as XtraForm;
            if(form.Name.Equals( "formSituacaoPopup" ))
            {
                ForcarFimEdicao();
            }
        }

        /// <summary>
        /// Método associado ao Evento de Edição de célula do grid efetuando o inicio da edição da tarefa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ShownEditor( object sender, EventArgs e )
        {
            if(!presenter.Conectado)
                return;
            presenter.SolicitarInicioEdicaoTarefa();
        }

        /// <summary>
        /// método responsável sinalizar o fim de edição e iniciar a execução dos comandos de tela pendentes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_HiddenEditor( object sender, EventArgs e )
        {
            if(!presenter.Conectado)
                return;

            presenter.TarefaSaiuDeEdicao();
        }

        /// <summary>
        /// Método responsável por efetuar o carregamento do novo cronograma realizando o carregamento de dados
        /// de alteração uma determinada tarefa no cronograma (TarefaLogAlteracao)
        /// </summary>
        /// <param name="sender">objeto acionador do evento</param>
        /// <param name="e">paramentros relativos ao evento do click do mouse </param>
        private void HistoricoBarButton_ItemClick( object sender, ItemClickEventArgs e )
        {
            CronogramaTarefaGridItem cronogramaTarefa = ConsultarTarefaSelecionada();

            if(cronogramaTarefa != null)
            {
                try
                {
                    using(TarefaLogView form = new TarefaLogView( cronogramaTarefa.OidTarefa ))
                    {
                        form.ShowDialog();
                    }
                }
                catch(Exception ex)
                {
                    NotificarErro( Resources.Caption_Erro, ex.Message );
                }
            }
            else
            {
                NotificarAlerta( Resources.Caption_Atencao, "Não existe tarefa selecionada para que o histórico de atualização possa ser exibido." );
            }
        }

        /// <summary>
        /// Efetuar as atualizações da tela quando for comunicado das tarefas que foram editadas
        /// </summary>
        /// <param name="autoresEdicoes"></param>
        public void NotificarInicioEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem, int> autoresEdicoes )
        {
            BeginInvoke( new Action( () =>
            {
                foreach(var item in autoresEdicoes)
                {
                    if(!coresTarefasEmEdicao.ContainsKey( item.Key ))
                    {
                        coresTarefasEmEdicao.Add( item.Key, Color.FromArgb( item.Value ) );
                        AtualizarTarefaEmSelecao( item.Key.OidCronogramaTarefa );
                    }
                }
            } ) );

        }

        /// <summary>
        /// Método responsável pelo comportamento da linha do grid que está em edição, não permitindo sua edição
        /// enquanto a tarefa encontrar-se em edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ShowingEditor( object sender, CancelEventArgs e )
        {
            if(!presenter.Conectado || !permitidoEditar)
                return;

            CronogramaTarefaGridItem tarefaSelecionada = ConsultarTarefaSelecionada();
            if(TarefasCronograma == null || tarefaSelecionada == null)
                return;

            if(presenter.AguardandoCriacaoTarefa)
            {
                e.Cancel = true;
                return;
            }

            if(tarefaSelecionada.Cor != null)
            {
                e.Cancel = true;
                NotificarAlerta( Resources.Caption_Atencao, "A tarefa já se encontra em edição por outro colaborador." );
                presenter.RemoverTarefaDeEdicao( tarefaSelecionada.OidCronogramaTarefa );
            }

            SituacaoPlanejamentoDTO situacaoPlanejamento = presenter.SituacoesPlanejamento.FirstOrDefault( o => o.Oid == tarefaSelecionada.OidSituacaoPlanejamentoTarefa );
            if(GridView.FocusedColumn != gridColunaSituacao &&
                GridView.FocusedColumn != gridColunaDescricaoTarefa &&
                GridView.FocusedColumn != gridColunaObs &&
                situacaoPlanejamento.CsTipo == CsTipoPlanejamento.Cancelamento)
            {
                e.Cancel = true;
                return;
            }

            if(!TarefaSelecionadaForUmaNovaTarefa() && presenter.ExisteTarefaEmEdicao() && !presenter.TarefaEmEdicao.Oid.Equals( tarefaSelecionada.OidCronogramaTarefa ))
            {
                e.Cancel = true;
            }

            if(GridView.FocusedColumn == gridColunaEstimativaInicial && tarefaSelecionada != null
                && tarefaSelecionada.CsLinhaBaseSalva == true)
                e.Cancel = true;

            if(GridView.FocusedColumn == gridColunaEstimativaRestante)
            {
                if(situacaoPlanejamento == null)
                    e.Cancel = true;

                if(tarefaSelecionada.OidCronograma == new Guid())
                    e.Cancel = true;

                if(situacaoPlanejamento.CsTipo == CsTipoPlanejamento.Encerramento || situacaoPlanejamento.CsTipo == CsTipoPlanejamento.Cancelamento)
                    e.Cancel = true;

                if(!PermitidaEdicaoHoraRestante( tarefaSelecionada ))
                {
                    NotificarAlerta( Resources.Caption_Atencao, Resources.Alerta_DevePossuirDuracaoTarefa );
                    e.Cancel = true;
                    presenter.RemoverTarefaDeEdicao( tarefaSelecionada.OidCronogramaTarefa );
                }
            }
        }

        /// <summary>
        /// Método Responsável pelo comportamento da tela quando uma tecla for pressionada com grid em foco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_KeyDown( object sender, KeyEventArgs e )
        {
            if(!presenter.Conectado)
                return;

            //Caso Seja Pressionado o Ctrl + Delete Enquanto o Grid estiver em foco
            if(e.KeyCode == Keys.Control && e.Modifiers == Keys.Delete)
            {
                SolicitarExclusao();
                return;
            }
            if(!presenter.AguardandoCriacaoTarefa)
                if(e.KeyCode == Keys.Escape)
                {
                    presenter.EsperarLeituraDataSource();
					CronogramaTarefaGridItem tarefa = presenter.ConsultarTarefaPodOidCronogramaTarefaNoDataSource( new Guid() );
                    presenter.LiberarLeituraDataSource();

                    presenter.viewEditandoTarefa = false;
                    presenter.RemoverTarefaDataSource( tarefa );
                }

            KeyShortcut atalho = new KeyShortcut( e.KeyData );
            if(atalho != null)
            {
                presenter.ProcessarTeclaDeAtalhoSituacaoPlanejamento( atalho.ToString() );
            }
        }

        /// <summary>
        /// Método executado nas solicitações de exclusão de linhas
        /// </summary>
        private void SolicitarExclusao()
        {
            if(!presenter.Conectado)
                return;

            presenter.EsperarLeituraDataSource();

            int[] indicesParaExclusao = GridView.GetSelectedRows();

            if(indicesParaExclusao.Length > 0)
            {
                List<CronogramaTarefaGridItem> tarefasParaExclusao = ConsultarTarefaPosicaoSelecionada( indicesParaExclusao.ToList() );

                presenter.LiberarLeituraDataSource();

                CronogramaTarefaGridItem tarefaSemOidValido = tarefasParaExclusao.FirstOrDefault( o => o.OidCronogramaTarefa == new Guid() );

                if(tarefaSemOidValido != null)
                {
                    tarefasParaExclusao.Remove( tarefaSemOidValido );
                    presenter.RemoverTarefaDataSource( tarefaSemOidValido );

                    if(tarefasParaExclusao.Count == 0)
                    {
                        return;
                    }
                }

                presenter.SolicitarExclusaoTarefas( tarefasParaExclusao );
            }
            else
            {
                presenter.LiberarLeituraDataSource();

                NotificarAlerta( Resources.Caption_Atencao, Resources.Alerta_TarefaNaoSelecionadaParaExclusao );
            }
        }

        /// <summary>
        /// Método responsável por efetuar a exclusão das tarefas selecionadas
        /// </summary>
        /// <param name="oidTarefasExcluidas">oid das tarefas a removidas da lista do grid</param>
        public void RemoverTarefas( List<Guid> oidTarefasExcluidas )
        {
            if(!presenter.Conectado)
                return;

            BeginInvoke( new Action( () =>
            {
                for(int i = 0; i < oidTarefasExcluidas.Count; i++)
                {
                    presenter.EsperarLeituraDataSource();
					CronogramaTarefaGridItem tarefa = presenter.ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidTarefasExcluidas[i] );
                    presenter.LiberarLeituraDataSource();

                    if(tarefa != null)
                    {
                        presenter.EsperarEscritaDataSource();
                        presenter.RemoverTarefaDataSource( tarefa );
                        presenter.LiberarEscritaDataSource();
                    }
                }
            } ) );
        }

        /// <summary>
        /// Evento disparado ao carregar o popup na tela de CronogramaView, 
        /// busca a linha que está em foco e seta alguns valores na tela de TarefaHistoricoView para ser usado na inicialização do popup.
        /// </summary>
        /// <param name="sender">GridView</param>
        /// <param name="e"></param>
        private void repositorioPopUpContainer_Popup( object sender, EventArgs e )
        {
            InicializarTarefaHistoricoTrabalho();
        }

        /// <summary>
        /// Método responsável por inicializar a janela de reestimativa
        /// </summary>
        private void InicializarTarefaHistoricoTrabalho()
        {
            CronogramaTarefaGridItem tarefaGridItem = GridView.GetFocusedRow() as CronogramaTarefaGridItem;
            tarefaHistoricoView1.InicializarSituacoesPlanejamento( presenter.SituacoesPlanejamento );
            tarefaHistoricoView1.InicializarJanela( tarefaGridItem, presenter.ColaboradorLogado.Login );
        }

        /// <summary>
        /// Método responsável por realizar a validação do campo Tarefa no GridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ValidatingEditor( object sender, BaseContainerValidateEditorEventArgs e )
        {
            if(!presenter.Conectado)
                return;

            Regex exp;
            if(GridView.FocusedColumn == gridColunaDescricaoTarefa)
            {
                if(e.Value == null || string.IsNullOrWhiteSpace( e.Value.ToString() ))
                {
                    e.Valid = false;
                    e.ErrorText = "Preencha a Descrição da Tarefa";
                }
                return;
            }

            if(GridView.FocusedColumn == gridColunaEstimativaInicial)
            {
                if(e.Value == null || string.IsNullOrEmpty( e.Value.ToString() ))
                {
                    e.Valid = false;
                    e.ErrorText = "Preencha o valor da Estimativa Inicial";
                }
                else
                {
                    exp = new Regex( @"^\d{1,2}h$" );
                    if(!exp.IsMatch( e.Value.ToString() ))
                    {
                        e.Valid = false;
                        e.ErrorText = "Preencha as horas no formato correto! (0 - 99)";
                    }
                }
                return;
            }
        }

        /// <summary>
        /// Método responsável por atualizar uma linha do gridView
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid Cronograma Tarefa</param>
        public void AtualizarTarefaEmSelecao( Guid oidCronogramaTarefa )
        {
            if(!presenter.Conectado)
                return;

            this.SafeInvoke( o =>
            {
                CronogramaTarefaGridItem tarefa = null;

                try
                {
                    o.presenter.EsperarLeituraDataSource();

                    tarefa = presenter.ConsultarTarefaPodOidCronogramaTarefaNoDataSource( oidCronogramaTarefa );
                }
                catch(Exception excecao)
                {
                    WexLogger.Error( "", excecao );
                }
                finally
                {
                    o.presenter.LiberarLeituraDataSource();
                }

                if(tarefa != null)
                {
                    try
                    {
                        o.presenter.EsperarEscritaDataSource();
						int posicaoDataSource = presenter.BuscarIndiceDaTarefaNoDataSource( tarefa );
						int posicaoView = GridView.GetRowHandle( posicaoDataSource );
                        o.presenter.LiberarEscritaDataSource();

						o.cronogramaTarefaBindingSource.ResetItem( posicaoDataSource );
						o.GridView.RefreshRow( posicaoView );
                    }
                    catch(Exception excecao)
                    {
                        WexLogger.Error( "", excecao );
                    }
                }
            } );
        }

        /// <summary>
        /// Método responsável por atualizar varias linhas do grid
        /// </summary>
        /// <param name="oidCronogramaTarefas">lista de tarefas a serem atualizadas</param>
        public void AtualizarTarefaEmSelecao( List<Guid> oidCronogramaTarefas )
        {
            if(!presenter.Conectado)
                return;

            if(oidCronogramaTarefas == null)
                return;

            foreach(Guid oidTarefa in oidCronogramaTarefas)
            {
                AtualizarTarefaEmSelecao( oidTarefa );
            }
        }

        /// <summary>
        /// Forçar atualização de todas linhas do grid
        /// </summary>
        public void AtualizarGridView()
        {
            if(!presenter.Conectado)
                return;

            cronogramaTarefaBindingSource.ResetBindings( false );
        }

        /// <summary>
        /// Método responsável pelo comportamento da tela quando uma tarefa é solicitada para exclusão
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_CustomDrawCell( object sender, RowCellCustomDrawEventArgs e )
        {
            if(e.Column == gridColunaIcone)
            {
                CronogramaTarefaGridItem tarefa = ConsultarTarefaPorPosicaoSelecionada( e.RowHandle ) as CronogramaTarefaGridItem;
                if(tarefa != null)
                {
                    if(LinhasParaExcluir.Contains( tarefa.OidCronogramaTarefa ))
                    {
                        tarefa.AdicionarIconeExcluir();
                        e.Appearance.Font = new Font( e.Appearance.Font, FontStyle.Strikeout );
                    }
                    else
                    {
                        e.Appearance.BackColor = Color.Transparent;
                    }
                }
            }
        }

        /// <summary>
        /// Desenhar a posição final da tarefa na seta que indica a movimentação
        /// </summary>
        /// <param name="oidTarefaAtual">oid da tarefa movimentada</param>
        /// <param name="posicaoTarefa">nova posição da tarefa</param>
        private void PreencherSeta( Guid oidTarefaAtual, int posicaoTarefa )
        {
            if(presenter.TarefasMovidas.Count > 0)
            {
                TarefaMovida tarefaMovida = presenter.TarefasMovidas.FirstOrDefault( o => o.OidTarefa == oidTarefaAtual );
                if(tarefaMovida != null && tarefaMovida.OidTarefa != new Guid())
                {
                    Image imagem;
                    if(tarefaMovida.Movimento == CsTipoSituacaoLinhaGrid.MovidaAcima)
                        imagem = Resources.seta_acima3264;
                    else
                        imagem = Resources.seta_abaixo3264;
                    using(Graphics graphics = Graphics.FromImage( imagem ))
                    {
                        graphics.TextRenderingHint = TextRenderingHint.AntiAlias; //I also tried ClearTypeGridFit
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.CompositingMode = CompositingMode.SourceOver;
                        using(Font font = new Font( "Tahoma", 9, FontStyle.Bold ))
                        {
                            graphics.DrawString( tarefaMovida.PosicaoFinal.ToString(), font, Brushes.DarkBlue, new PointF( 22, 6 ) );
                        }
                    }
                    GridView.SetRowCellValue( posicaoTarefa, gridColunaIcone, imagem );
                }
            }
            else
                GridView.SetRowCellValue( posicaoTarefa, gridColunaIcone, Resources.None );
        }


        /// <summary>
        /// Método do drag and drop das tarefas no cronograma
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void CronogramaTarefaGrid_DragDrop( object sender, DragEventArgs e )
        {
            if(!presenter.Conectado)
                return;

            presenter.EsperarLeituraDataSource();

            GridControl grid = sender as GridControl;
            GridView view = grid.MainView as GridView;

            int posicaoInicial = view.FocusedRowHandle;
            int posicaoFinal = view.CalcHitInfo( grid.PointToClient( new Point( e.X, e.Y ) ) ).RowHandle;

            int posicaoMaxima = ConsultarMaiorPosicaoTarefaNoGrid();

            if(posicaoInicial >= 0 && posicaoFinal >= 0)
                if(posicaoFinal >= posicaoMaxima)
                {
                    presenter.LiberarLeituraDataSource();
                    NotificarAlerta( "Erro", "Movimentação inválida! Fora da quantidade de tarefas cadastradas" );
                    return;
                }
                else
                {
                    CronogramaTarefaGridItem tarefaSelecionada = ConsultarTarefaPorPosicaoSelecionada( posicaoInicial );
                    CronogramaTarefaGridItem tarefaDestino = ConsultarTarefaPorPosicaoSelecionada( posicaoFinal );

                    presenter.LiberarLeituraDataSource();

                    this.SafeInvoke( o =>
                    {
                        presenter.SolicitarMovimentacaoTarefa( tarefaSelecionada, tarefaDestino );
                    } );
                }
        }

        /// <summary>
        /// Método responsável por buscar a maior posição existente de uma tarefa no datasource
        /// </summary>
        /// <returns></returns>
        private int ConsultarMaiorPosicaoTarefaNoGrid()
        {
            try
            {
                int posicaoMaxima = (int)TarefasCronograma.Max( o => o.NbID );
                return posicaoMaxima;
            }
            catch(Exception)
            {
                return 0;
            }
        }



        /// <summary>
        /// Método de largar um objeto em um drag and drop
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void CronogramaTarefaGrid_DragOver( object sender, DragEventArgs e )
        {
            GridControl grid = sender as GridControl;
            GridView view = grid.MainView as GridView;
            GridHitInfo hitInfo = view.CalcHitInfo( grid.PointToClient( new Point( e.X, e.Y ) ) );
            if(hitInfo.InRow && hitInfo.RowHandle != GridControl.NewItemRowHandle)
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Método que guarda a movimentação do mouse quando larga um componente (tarefa)
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void CronogramaTarefaGrid_MouseDown( object sender, MouseEventArgs e )
        {
            GridControl grid = sender as GridControl;
            GridView view = grid.MainView as GridView;
            downHitInfo = null;
            GridHitInfo hitInfo = view.CalcHitInfo( new Point( e.X, e.Y ) );
            if(Control.ModifierKeys != Keys.None)
                return;
            if(e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;
        }

        /// <summary>
        /// Método que guarda e seta os movimentos feitos pelo usuário com o mouse
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumentos</param>
        private void CronogramaTarefaGrid_MouseMove( object sender, MouseEventArgs e )
        {
            if(e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle( new Point( downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2 ), dragSize );
                if(!dragRect.Contains( new Point( e.X, e.Y ) ))
                {
                    CronogramaTarefaGridItem row = ConsultarTarefaPorPosicaoSelecionada( downHitInfo.RowHandle ) as CronogramaTarefaGridItem;

                    if(row.OidCronogramaTarefa == new Guid())
                    {
                        return;
                    }

                    GridView.GridControl.DoDragDrop( row, DragDropEffects.Move );
                    downHitInfo = null;
                    DXMouseEventArgs.GetMouseArgs( e ).Handled = true;
                }
            }
        }

        /// <summary>
        /// Método responsável pelo controe do comportamento da estilo de renderização da formatação das células
        /// </summary>
        /// <param name="sender">objeto que disparou o evento</param>
        /// <param name="e">argumentos relativos ao evento disparado</param>
        private void GridView_RowCellStyle( object sender, RowCellStyleEventArgs e )
        {
            CronogramaTarefaGridItem tarefaAtual;

            if(TarefasCronograma == null)
                return;

            tarefaAtual = ConsultarTarefaPorPosicaoSelecionada( e.RowHandle );

            if(tarefaAtual == null)
                return;

            if(e.Column == gridColunaAtualizadoEm)
                e.Appearance.BackColor = Color.LightCyan;

            if(e.Column == gridColunaAtualizadoPor)
                e.Appearance.BackColor = Color.LightCyan;

            if(tarefaAtual.Cor != null)
            {
                Color cor = Color.FromArgb( (int)tarefaAtual.Cor );
                e.Appearance.BackColor = cor;
                e.Appearance.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( cor );
            }

            if(e.Column == gridColunaEstimativaInicial)
            {
                if(tarefaAtual.CsLinhaBaseSalva)
                    e.Appearance.BackColor = Color.LightCyan;
                else
                    e.Appearance.BackColor = Color.White;
            }

            if(e.Column == gridColunaEstimativaRestante)
            {
                if(tarefaAtual.CsLinhaBaseSalva)
                    e.Appearance.BackColor = Color.White;
                else
                    e.Appearance.BackColor = Color.LightCyan;
            }

            if(e.Column == gridColunaRealizado)
            {
                valorCelulaRealizado = tarefaAtual.NbRealizado.ToTimeSpan();
                valorCelulaInicial = tarefaAtual.EstimativaInicial;

                if(valorCelulaRealizado > valorCelulaInicial)
                    e.Appearance.BackColor = Color.LightSalmon;
            }

            //Quando existir linhas em exclusão. Risca a linha.
            if(LinhasParaExcluir.Contains( tarefaAtual.OidCronogramaTarefa ))
                e.Appearance.Font = new Font( e.Appearance.Font, FontStyle.Strikeout );
        }

        /// <summary>
        /// Método que desabilita uma célula HoraRestante quando
        /// </summary>
        /// <param name="view">GridView</param>
        /// <param name="linha">linha</param>
        /// <returns>se é realmente aquele célula</returns>
        private static bool PermitidaEdicaoHoraRestante( CronogramaTarefaGridItem tarefa )
        {
            if(tarefa == null)
                return false;

            TimeSpan tempo = tarefa.EstimativaInicial;
            return tempo.Ticks > 0;
        }

        /// <summary>
        /// Método para fechar o popup com um clique
        /// </summary>
        private void ClosePopUp( TarefaHistoricoView tarefaHistoricoView )
        {
            if(popupContainerControl1.Visible)
                popupContainerControl1.Hide();

            ForcarFimEdicao();
        }

        /// Método responsável pelo comportamento do cronogramaView quando houver alteração no nome do cronograma
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AoEncerrarEdicaoDadosCronograma( object sender, ItemClickEventArgs e )
        {
			presenter.FimEdicaoDadosCronograma();
        }

        /// <summary>
        /// Efetuar a Cópia da descrição do cronograma durante a edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AoIniciarEdicaoDadosCronograma( object sender, ItemClickEventArgs e )
        {
			presenter.InicioEdicaoDadosCronograma();
        }

        /// <summary>
        /// Evento acionado quando usuário clicar no botão excluir tarefa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barraBtExcluirTarefa_ItemClick( object sender, ItemClickEventArgs e )
        {
            List<CronogramaTarefaGridItem> tarefasSelecionadas;

            presenter.EsperarLeituraDataSource();

            int[] indicesTarefasParaExclusao = GridView.GetSelectedRows();

            if(indicesTarefasParaExclusao.Length > 0)
            {
                tarefasSelecionadas = ConsultarTarefaPosicaoSelecionada( indicesTarefasParaExclusao.ToList() );

                presenter.LiberarLeituraDataSource();

                CronogramaTarefaGridItem tarefaSemOidValido = tarefasSelecionadas.FirstOrDefault( o => o.OidCronogramaTarefa == new Guid() );

                if(tarefaSemOidValido != null)
                {
                    tarefasSelecionadas.Remove( tarefaSemOidValido );
                    presenter.RemoverTarefaDataSource( tarefaSemOidValido );

                    if(tarefasSelecionadas.Count == 0)
                    {
                        return;
                    }
                }

                List<CronogramaTarefaGridItem> tarefasQuePossuemHistorico = tarefasSelecionadas.Where( o => o.NbRealizado > 0 ).ToList();

                if(tarefasQuePossuemHistorico.Count > 0)
                {
                    string mensagem = "A(s) tarefa(s) ";

                    for(int i = 0; i < tarefasQuePossuemHistorico.Count; i++)
                    {
                        tarefasSelecionadas.Remove( tarefasQuePossuemHistorico[i] );

                        if(tarefasQuePossuemHistorico[i].TxDescricaoTarefa.Length > 10)
                        {
                            mensagem += String.Format( "{0}, ", tarefasQuePossuemHistorico[i].TxDescricaoTarefa.Substring( 0, 10 ) );
                        }
                        else
                        {
                            mensagem += String.Format( "{0}, ", tarefasQuePossuemHistorico[i].TxDescricaoTarefa );
                        }
                    }

                    mensagem = mensagem.Trim( ',' );

                    mensagem += " não poderão ser excluídas, pois possuem esforço de horas estimado.";

                    NotificarMensagem( Resources.Caption_Atencao, mensagem );

                    if(tarefasSelecionadas.Count == 0)
                        return;
                }

                presenter.SolicitarExclusaoTarefas( tarefasSelecionadas );
            }
            else
            {
                int indice = TarefasCronograma.IndexOf( GridView.GetFocusedRow() as CronogramaTarefaGridItem );

                int[] indiceTarefasParaExclusao = { indice };

                tarefasSelecionadas = ConsultarTarefaPosicaoSelecionada( indiceTarefasParaExclusao.ToList() );

                if(tarefasSelecionadas.FirstOrDefault() != null)
                {
                    if(tarefasSelecionadas.FirstOrDefault().OidCronogramaTarefa == new Guid())
                    {
                        presenter.RemoverTarefaDataSource( tarefasSelecionadas.FirstOrDefault() );
                        presenter.LiberarLeituraDataSource();
                        return;
                    }
                }

                List<CronogramaTarefaGridItem> tarefasQuePossuemHistorico = tarefasSelecionadas.Where( o => o.NbRealizado > 0 ).ToList();

                if(tarefasQuePossuemHistorico.Count > 0)
                {
                    for(int i = 0; i < tarefasQuePossuemHistorico.Count; i++)
                    {
                        tarefasSelecionadas.Remove( tarefasQuePossuemHistorico[i] );
                    }

                    if(tarefasSelecionadas.Count == 0)
                    {
                        presenter.LiberarLeituraDataSource();
                        return;
                    }
                }

                if(tarefasSelecionadas.Count > 0 || tarefasQuePossuemHistorico.Count > 0)
                {
                    presenter.LiberarLeituraDataSource();
                    presenter.SolicitarExclusaoTarefas( tarefasSelecionadas );
                }
                else
                {
                    presenter.LiberarLeituraDataSource();
                }
            }
        }

        /// <summary>
        /// método responsável por efetuar uma notificação no popup lateral
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensagem"></param>
        public void NotificarMensagem( string titulo, string mensagem )
        {
            this.SafeInvoke( o =>
            {
                o.MensagemPopup.Show( o, titulo, mensagem );
            } );
        }

        /// <summary>
        /// Método responsável pelo comportamento da aplicação quando um ou mais usuários se conectem ao servidor no mesmo cronograma
        /// </summary>
        /// <param name="coresColaboradores"></param>
        public void AdicionarNovosUsuariosConectados( List<CronogramaColaboradorConfigDto> configs )
        {
            this.SafeInvoke( o =>
            {
                for(int i = 0; i < configs.Count; i++)
                {
                    barraUsuariosConectados.AdicionarUsuarioConectadoAoPanel( configs[i].NomeCompletoColaborador, Color.FromArgb( Convert.ToInt32( configs[i].Cor ) ) );
                }
            } );
        }

        /// <summary>
        /// Método responsável pelo comportamento da aplicação quando um ou mais usuários se desconectarem
        /// </summary>
        /// <param name="usuariosDesconectados"></param>
        public void RemoverUsuariosDesconectados( List<string> usuariosDesconectados )
        {
            this.SafeInvoke( o =>
           {
               foreach(var usuario in usuariosDesconectados)
               {
                   o.barraUsuariosConectados.RemoverUsuarioConectadoAoPanel( usuario );
               }
           } );
        }

        /// <summary>
        /// Método responsável por executar o comportamento da tela assim que o cronograma for conectado;
        /// </summary>
        public void ExecutarAoConectar()
        {
            this.SafeInvoke( o =>
                {
                    o.lbColaboradorConectado.Caption = "Usuário: " + presenter.ColaboradorLogado.TxNomeCompletoColaborador;
                    o.lbEstadorServidor.Caption = Resources.Info_Conectado;
                } );
        }

        /// <summary>
        /// Método responsável por executar o comportamento da tela assim que o cronograma for desconectado
        /// </summary>
        public void ExecutarAoDesconectar()
        {
            if(janelaInicializada)
                this.SafeInvoke( o =>
                {
                    o.barraUsuariosConectados.RemoverTodosUsuariosDoPanel();
                    o.lbEstadorServidor.Caption = Resources.Info_Desconectado;
                } );
        }

        /// <summary>
        /// Método responsável por efetuar a notificação de uma mensagem de alerta
        /// </summary>
        /// <param name="mensagem"></param>
        public void NotificarAlerta( string caption, string mensagem )
        {
            this.SafeInvoke( o =>
                {
                    XtraMessageBox.Show( LookAndFeel, mensagem, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                } );
        }

        /// <summary>
        /// Método responsável por efetuar a chamada de encerramento da aplicação
        /// </summary>
        public void Fechar()
        {
            Close();
        }


        /// <summary>
        /// Método para preencher o Combo de cronogramas
        /// </summary>
        /// <param name="cronogramas"></param>
        /// <param name="descricaoCronogramaSelecionado"></param>
        public void ListarCronogramas( List<CronogramaDto> cronogramas, string descricaoCronogramaSelecionado )
        {
            barraRepositorioComboCronogramaSelecionado.Items.Clear();

            if(cronogramas != null)
            {
                //preenche o combo de cronogramas
                foreach(CronogramaDto cronograma in cronogramas.Where( cronograma => cronograma != null && cronograma.TxDescricao != null ))
                {
                    barraRepositorioComboCronogramaSelecionado.Items.Add( cronograma.TxDescricao );
                }
                barraComboCronogramaSelecionado.Enabled = true;
            }

            barraComboCronogramaSelecionado.EditValue = !string.IsNullOrEmpty( descricaoCronogramaSelecionado ) ? descricaoCronogramaSelecionado : string.Empty;
        }

        /// <summary>
        /// Método responsável por carregar o nome do cronograma selecionado
        /// </summary>
        /// <param name="DescricaoCronogramaSelecionado"></param>
        public void SetarNomeCronograma( string DescricaoCronogramaSelecionado )
        {
            if(!presenter.Conectado)
                return;

            this.SafeInvoke( o => o.barraTxDescricaoCronograma.EditValue = DescricaoCronogramaSelecionado );
        }

        /// <summary>
        /// Método responsável por carregar o combo de situação planejamento
        /// </summary>
        /// <param name="situacoes">lista de situaçoes de planejamento</param>
        /// <param name="situacaoPadrao">situação planejamento padrao</param>
        public void ListarSituacoesPlanejamento( List<SituacaoPlanejamentoDTO> situacoes, SituacaoPlanejamentoDTO situacaoPadrao )
        {
            this.SafeInvoke( o =>
            {
                Image imagem;
                MemoryStream stream;
                o.comboGridSituacaoPlanejamento.Items.Clear();
                ImageCollection imagens = new ImageCollection();
                foreach(SituacaoPlanejamentoDTO situacao in situacoes)
                {
                    stream = new MemoryStream( situacao.BlImagemSituacaoPlanejamento );
                    imagem = Image.FromStream( stream );
                    imagens.AddImage( imagem );
                    o.comboGridSituacaoPlanejamento.Items.Add( new ImageComboBoxItem( situacao.TxDescricao, situacao.Oid, comboGridSituacaoPlanejamento.Items.Count ) );
                }
                o.comboGridSituacaoPlanejamento.SmallImages = imagens;
                o.PreencherComboSituacoesPlanejamentoBarra( imagens, situacoes );
                //Constroi o combo situação planejamento do popup de estimativa
                o.tarefaHistoricoView1.popupComboImagensSituacaoPlanejamento.Properties.Items.Clear();
                o.tarefaHistoricoView1.popupComboImagensSituacaoPlanejamento.Properties.Items.AddRange( comboGridSituacaoPlanejamento.Items );
                o.tarefaHistoricoView1.popupComboImagensSituacaoPlanejamento.Properties.SmallImages = comboGridSituacaoPlanejamento.SmallImages;
            } );
        }

        /// <summary>
        /// Método responsável por efetuar o carregamento da situação do cronograma atual
        /// </summary>
        /// <param name="situacao"></param>
        public void SetarSituacaoCronogramaAtual( string situacao )
        {
            if(!presenter.Conectado)
                return;

            if(!string.IsNullOrEmpty( situacao ))
                SituacaoCronograma = situacao;
        }

        /// <summary>
        /// Método responsável por efetuar o carregamento dos colaboradores no combo do grid
        /// </summary>
        /// <param name="colaboradores"></param>
        public void ExibirColaboradoresResponsaveis( List<ColaboradorDto> colaboradores )
        {
            this.SafeInvoke( o =>
            {
                if(colaboradores == null || colaboradores.Count == 0)
                    return;

                o.comboColaboradoresResponsaveis.Items.Clear();
                foreach(ColaboradorDto colaboradorDto in colaboradores)
                {
                    if(!String.IsNullOrEmpty( colaboradorDto.TxNomeCompletoColaborador ))
                        o.comboColaboradoresResponsaveis.Items.Add( colaboradorDto.TxNomeCompletoColaborador, colaboradorDto.TxNomeCompletoColaborador );
                }
            } );
        }

        /// <summary>
        /// Método responsável por efetuar a notificação de um erro
        /// </summary>
        /// <param name="caption">Titulo da janela de mensagem do alerta</param>
        /// <param name="mensagem">mensagem da janela de a alerta</param>
        public void NotificarErro( string caption, string mensagem )
        {
            this.SafeInvoke( o =>
            {
                XtraMessageBox.Show( o.LookAndFeel, mensagem, caption, MessageBoxButtons.OK, MessageBoxIcon.Error );
            } );
        }

        /// <summary>
        /// Evento que é acionado quando um usuário clica no botão de excluir cronograma.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barraBtExcluirCronograma_ItemClick( object sender, ItemClickEventArgs e )
        {
            presenter.ExcluirCronograma();
        }

        /// <summary>
        /// Método responsável por retornar a tarefa selecionada
        /// </summary>
        /// <returns></returns>
        public CronogramaTarefaGridItem ConsultarTarefaSelecionada()
        {
            if(GridView.DataRowCount > 0 && GridView.FocusedRowHandle >= 0)
                return GridView.GetFocusedRow() as CronogramaTarefaGridItem;
            else
                return null;
        }

        /// <summary>
        /// Método responsável por validar o campo Situação Planejamento quando o usuário trocar a situação para Tipo Encerramento diretamente no Grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboGridSituacaoPlanejamento_Closed( object sender, ClosedEventArgs e )
        {
            switch(e.CloseMode)
            {
                case PopupCloseMode.ButtonClick:
                case PopupCloseMode.Cancel:
                case PopupCloseMode.Immediate:
                    presenter.RetornarValoresAnterioresTarefa( ConsultarTarefaSelecionada().OidCronogramaTarefa );
                    break;
            }

            ForcarFimEdicao();
        }

        /// <summary>
        /// Método acionado quando criar uma nova tarefa no Grid.
        /// Cria uma linha no grid sem ID.
        /// </summary>
        /// <param name="sender">Objeto quem acionou o evento</param>
        /// <param name="e">Objeto acionado</param>
        private void NovaTarefa_ItemClick( object sender, ItemClickEventArgs e )
        {
            if(GridView.IsFindPanelVisible)
            {
                GridView.ApplyFindFilter( String.Empty );
            }

            if(presenter.viewEditandoTarefa)
            {
				ForcarFimEdicao();
				//NotificarAlerta( Resources.Caption_Atencao, "Você precisa sair de edição para criar uma tarefa." );
				//return;
            }

            if(VerificarSeFiltroOuAgrupamentoDeTarefasEstaAtivo())
                AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Todas );

            presenter.PermitirGerenciadorComandosAtualizarView( false );
            RetirarOrdenacao();

            CronogramaTarefaDto tarefaCriada = presenter.NovaTarefaCriada( ConsultarTarefaSelecionada() );
            ForcarEdicaoDescricaoTarefaAtual( tarefaCriada );
        }

        /// <summary>
        /// Método responsável por retornar a tarefa da linha selecionada
        /// </summary>
        /// <param name="posicao">posicao da tarefa que será selecionada</param>
        /// <returns></returns>
        public CronogramaTarefaGridItem ConsultarTarefaPorPosicaoSelecionada( int posicao )
        {
            if(posicao >= 0 && posicao < GridView.DataRowCount)
                return GridView.GetRow( posicao ) as CronogramaTarefaGridItem;
            return null;
        }

        /// <summary>
        /// Método responsável por retornar a lista de tarefas selecionadas
        /// </summary>
        /// <param name="posicoes">posicoes das tarefas selecionada</param>
        /// <returns>lista de tarefas selecionadas</returns>
        public List<CronogramaTarefaGridItem> ConsultarTarefaPosicaoSelecionada( List<int> posicoes )
        {
            List<CronogramaTarefaGridItem> cronogramaTarefas = new List<CronogramaTarefaGridItem>();

            for(int i = 0; i < posicoes.Count; i++)
                if(posicoes[i] >= 0 && posicoes[i] < GridView.DataRowCount)
                {
                    object cronogramaTarefa = GridView.GetRow( posicoes[i] );
                    if(cronogramaTarefa != null)
                        cronogramaTarefas.Add( cronogramaTarefa as CronogramaTarefaGridItem );
                }

            return cronogramaTarefas;
        }

        /// <summary>
        /// Método responsável por inserir uma nova tarefa na posição desejada
        /// </summary>
        /// <param name="tarefa">nova tarefa</param>
        /// <param name="posicao">posicao da tarefa</param>
        public void InserirTarefaPadrao( CronogramaTarefaGridItem tarefa, int posicao )
        {
            tarefaRecentementeCriada = true;

            presenter.EsperarEscritaDataSource();
            GridView.ClearSelection();

            if(posicao < 0)
            {
                TarefasCronograma.Add( tarefa );
            }
            else
            {
                TarefasCronograma.Insert( posicao, tarefa );
            }
            presenter.LiberarEscritaDataSource();
        }

        /// <summary>
        /// Método responsável por efetuar o carregamento das tarefas de um determinado cronograma
        /// </summary>
        /// <param name="tarefas">Exibir as tarefas de um cronograma</param>
        public void ExibirTarefas()
        {
            this.SafeInvoke( o =>
            {
                Menu.SelectedPage = Tarefas;
                cronogramaTarefaGridControl.DataSource = tarefasCronograma;
            } );
        }

        /// <summary>
        /// Método que verifica se a tarefa atual é uma nova tarefa
        /// </summary>
        /// <returns></returns>
        public bool TarefaSelecionadaForUmaNovaTarefa()
        {
            CronogramaTarefaGridItem tarefa = ConsultarTarefaSelecionada();
            return TarefaSelecionadaForUmaNovaTarefa( tarefa );
        }

        /// <summary>
        /// Método que verifica se a tarefa atual é uma nova tarefa
        /// </summary>
        /// <returns></returns>
        public bool TarefaSelecionadaForUmaNovaTarefa( int posicao )
        {
            CronogramaTarefaGridItem tarefa = ConsultarTarefaPorPosicaoSelecionada( posicao );
            return TarefaSelecionadaForUmaNovaTarefa( tarefa );
        }

        /// <summary>
        /// Método que verifica se a tarefa atual é uma nova tarefa
        /// </summary>
        /// <returns></returns>
        public bool TarefaSelecionadaForUmaNovaTarefa( CronogramaTarefaGridItem tarefaSelecionada )
        {
            if(tarefaSelecionada == null)
                return false;

            return tarefaSelecionada.OidCronogramaTarefa == new Guid();
        }

        /// <summary>
        /// Método responsável por atualizar a view  após a execução dos comandos pendentes
        /// </summary>
        public void AtualizarView()
        {
            this.SafeInvoke( o =>
            {
                o.presenter.EsperarEscritaDataSource();
                o.cronogramaTarefaGridControl.Enabled = true;
                o.cronogramaTarefaBindingSource.ResetBindings( false );
                Ordenar();
                o.presenter.LiberarEscritaDataSource();
                o.GridView.RefreshData();
            } );
        }

        /// <summary>
        /// Método utilizado para atualizar o nome do cronograma na view
        /// </summary>
        /// <param name="NovoNome">novo do cronograma</param>
        public void AtualizarNomeCronograma( string NovoNome )
        {
            if(!presenter.Conectado)
                return;

            this.SafeInvoke( o =>
            {
                o.NomeCronograma = NovoNome;
                o.barraComboCronogramaSelecionado.EditValue = NovoNome;
            } );
        }

        /// <summary>
        /// Método responsável por remover tarefas de edição na tela e comunicar as alterações
        /// </summary>
        /// <param name="tarefasEditadas">
        /// As tarefas que foram editadas indexadas pelo login do colaborador que as editou
        /// Key   - Tarefa Editada
        /// Value - Mensagem com as modificações
        /// </param>
        public void NotificarFimEdicaoTarefaExterna( Dictionary<CronogramaTarefaGridItem, string> tarefasEditadas )
        {
            this.SafeInvoke( o =>
            {
                List<CronogramaTarefaGridItem> tarefas = tarefasEditadas.Keys.ToList();
                foreach(CronogramaTarefaGridItem tarefa in tarefas)
                {
                    RemoverCorEdicao( tarefa.OidCronogramaTarefa );
                    AtualizarTarefaEmSelecao( tarefa.OidCronogramaTarefa );
                }
            } );
        }

        /// <summary>
        /// Método para remover a cor da tarefa que se encontrava em edição
        /// </summary>
        /// <param name="oid"></param>
        public void RemoverCorEdicao( Guid oid )
        {
            CronogramaTarefaGridItem tarefa = coresTarefasEmEdicao.Keys.FirstOrDefault( o => o.OidCronogramaTarefa == oid );
            if(tarefa != null)
                tarefa.Cor = null;
        }

        /// <summary>
        /// Método para remover a cor da tarefa que se encontrava em edição
        /// </summary>
        /// <param name="cor"></param>
        public void RemoverCorEdicao( int? cor )
        {
            if(cor == null)
                return;

            var tarefas = tarefasCronograma.Where( o => o.Cor.Equals( cor ) );
            if(tarefas != null && ( tarefas.Count() > 0 ))
            {
                foreach(var item in tarefas)
                {
                    item.Cor = null;
                }
            }
        }

        /// <summary>
        /// Método responsável por atualizar a ultima ação ocorrida no cronograma
        /// </summary>
        /// <param name="notificacao">mensagem da ultima ação ocorrida no cronograma</param>
        public void AtualizarUltimaAlteracaoCronograma( string notificacao )
        {
            this.SafeInvoke( o => o.lbUltimaAlteracao.Caption = notificacao );
        }

        /// <summary>
        /// Método para atualizar o hint de um colaborador
        /// </summary>
        /// <param name="login"></param>
        /// <param name="acao"></param>
        /// <param name="quando"></param>
        public void AtualizarHintColaborador( string login, string acao, DateTime quando )
        {
            this.SafeInvoke( o => o.barraUsuariosConectados.ModificarHintUsuario( login, acao, quando ) );
        }

        /// <summary>
        /// Método para criar ordenação no grid
        /// </summary>
        public void Ordenar()
        {
            this.SafeInvoke(
                 o =>
                 {
                     lock(bloquearThreadOrdenacao)
                     {
                         TarefasCronograma.OrderBy( x => x.NbID );
                     }
                 }
             );

        }

        /// <summary>
        /// Método para remover a ordenação
        /// </summary>
        public void RetirarOrdenacao()
        {
            this.SafeInvoke( o =>
            {
                lock(bloquearThreadOrdenacao)
                {
                    o.gridColunaId.SortOrder = DevExpress.Data.ColumnSortOrder.None;
                }
            } );
        }

        /// <summary>
        /// Método utilizado ao fechar o combo dos colaboradores responsáveis pela tarefa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboColaboradoresResponsaveis_Closed( object sender, ClosedEventArgs e )
        {
            //forçar o fim da edição do comb
            GridView.CloseEditor();
        }

        /// <summary>
        /// Método responsável por perguntar ao usuário se ele deseja realmente excluir as tarefas selecionadas.
        /// </summary>
        /// <returns></returns>
        public bool ConfirmarExclusaoTarefas()
        {
            return XtraMessageBox.Show( LookAndFeel, "Deseja Excluir a(s) linha(s) selecionada(s) ?", "Excluir Linha", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3 ) == DialogResult.Yes;
        }

        /// <summary>
        /// Método responsável por perguntar ao usuário se ele deseja realmente excluir o cronograma.
        /// </summary>
        /// <returns></returns>
        public bool ConfirmarExclusaoCronograma()
        {
            return XtraMessageBox.Show( LookAndFeel, "Deseja Excluir o cronograma ?", "Excluir Cronograma", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3 ) == DialogResult.Yes;
        }

        /// <summary>
        /// Método usado para forçar o fim da edição data de inicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtInicioEdit_Closed( object sender, ClosedEventArgs e )
        {
            GridView.CloseEditor();
        }

        /// <summary>
        /// Método usado para forçar o fim da edição da observação que fechar o popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextAreaObservacaoTarefa_Closed( object sender, ClosedEventArgs e )
        {
            GridView.CloseEditor();
        }

        /// <summary>
        /// Método responsável por atualizar os dados da tarefa
        /// </summary>
        /// <param name="oidCronogramaTarefa"></param>
        public void AtualizarDadosTarefa( Guid oidCronogramaTarefa )
        {
            if(!presenter.Conectado)
                return;

            presenter.AtualizarDadosTarefa( oidCronogramaTarefa );
        }

        /// <summary>
        /// Método utilizado para remover o foco das linhas selecionadas
        /// </summary>
        public void RemoverFocoTarefas()
        {
            int[] linhasSelecionadas = GridView.GetSelectedRows();
            foreach(int indiceLinha in linhasSelecionadas)
            {
                GridView.UnselectRow( indiceLinha );
            }
        }

        /// <summary>
        /// Método responsável por remover o foco sobre a tarefa em que a edição foi recusada
        /// </summary>
        /// <param name="oid"></param>
        public void RemoverTarefaRecusadaDeEdicao( Guid oid )
        {
            List<CronogramaTarefaGridItem> lista = new List<CronogramaTarefaGridItem>( GridView.DataSource as IEnumerable<CronogramaTarefaGridItem> );
            CronogramaTarefaGridItem tarefa = lista.FirstOrDefault( o => o.OidCronogramaTarefa == oid );
            if(tarefa != null)
            {
                this.SafeInvoke( o =>
                {
                    int posicao = lista.IndexOf( tarefa );
                    o.GridView.UnselectRow( posicao );
                    CronogramaTarefaGridItem tarefaEmFoco = (CronogramaTarefaGridItem)o.GridView.GetFocusedRow();
                    if(tarefaEmFoco.OidCronogramaTarefa == tarefa.OidCronogramaTarefa)
                        ForcarFimEdicao();
                } );
            }
        }

        ///<summary>
        /// Notificar a edição do nome do cronograma utilizando o cor do colaborador que está editando;
        /// </summary>
        /// <param name="cor">cor selecionado do colaborador que está editando</param>
        public void NotificarInicioEdicaoDadosCronograma( Color cor )
        {
            this.SafeInvoke( o =>
            {
                o.barraTxDescricaoCronograma.Enabled = false;
                o.barraRepositorioTxDescricaoCronograma.AppearanceDisabled.BackColor = cor;
                o.barraRepositorioTxDescricaoCronograma.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( cor );

				o.barraDtInicioCronograma.Enabled = false;
				o.barraDtInicioCronograma.Edit.AppearanceDisabled.BackColor = cor;
				o.barraDtInicioCronograma.Edit.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( cor );

				o.barraDtTerminoCronograma.Enabled = false;
				o.barraDtTerminoCronograma.Edit.AppearanceDisabled.BackColor = cor;
				o.barraDtTerminoCronograma.Edit.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( cor );
            } );
        }


        /// <summary>
        /// Remover cor da edição do nome do cronograma
        /// </summary>
        public void NotificarFimEdicaoDadosCronograma()
        {
            this.SafeInvoke( o =>
            {
				o.barraTxDescricaoCronograma.Enabled = true;
                o.barraRepositorioTxDescricaoCronograma.AppearanceDisabled.BackColor = Color.White;
                o.barraRepositorioTxDescricaoCronograma.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( Color.White );
                
				o.barraDtInicioCronograma.Enabled = true;
				o.barraDtInicioCronograma.Edit.AppearanceDisabled.BackColor = Color.White;
				o.barraDtInicioCronograma.Edit.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( Color.White );

				o.barraDtTerminoCronograma.Enabled = true;
				o.barraDtTerminoCronograma.Edit.AppearanceDisabled.BackColor = Color.White;
				o.barraDtTerminoCronograma.Edit.AppearanceDisabled.ForeColor = ImageUtil.DefinirCorFonteEmConstraste( Color.White );
            } );
        }

        /// <summary>
        /// Método responsável pelo comportamento de tela quando for clicado em ir para descrição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonGoDesc_ItemClick( object sender, ItemClickEventArgs e )
        {
            ForcarEdicaoDescricaoTarefaAtual();
        }

        /// <summary>
        /// Método utilizado para forçar a edição da descrição da tarefa atual
        /// </summary>
        public void ForcarEdicaoDescricaoTarefaAtual( CronogramaTarefaDto tarefa )
        {
            if(!presenter.Conectado)
                return;

            if(tarefa != null)
            {
                this.SafeInvoke( o =>
                {
                    presenter.viewEditandoTarefa = true;

                    presenter.EsperarLeituraDataSource();

                    CronogramaTarefaGridItem tarefacronograma = TarefasCronograma.FirstOrDefault( x => x == tarefa );
                    int posicaoDataSource = TarefasCronograma.IndexOf( tarefacronograma );
                    int posicaoView = GridView.GetRowHandle( posicaoDataSource );
                    GridView.FocusedRowHandle = posicaoView;
                    GridView.FocusedColumn = gridColunaDescricaoTarefa;
                    GridView.SelectRow( GridView.FocusedRowHandle );
                    GridView.ShowEditor();

                    presenter.LiberarLeituraDataSource();
                } );
            }
        }

        /// <summary>
        /// Método utilizado para forçar a edição da descrição da tarefa atual
        /// </summary>
        private void ForcarEdicaoDescricaoTarefaAtual()
        {
            ForcarEdicaoDescricaoTarefa( GridView.FocusedRowHandle );
        }

        /// <summary>
        /// Método utilizado para forçar a edição da descrição da tarefa atual
        /// </summary>
        /// <param name="rowHandle"></param>
        private void ForcarEdicaoDescricaoTarefa( int rowHandle )
        {
            if(!presenter.Conectado)
                return;

            GridView.FocusedColumn = gridColunaDescricaoTarefa;
            GridView.SelectRow( rowHandle );
            GridView.ShowEditor();
        }

        /// <summary>
        /// Método responsável pelo comportamento de tela quando for clicado em pesquisa de tarefas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonGoSearch_ItemClick( object sender, ItemClickEventArgs e )
        {
            if(GridView.IsFindPanelVisible)
                GridView.HideFindPanel();
            else
                GridView.ShowFindPanel();
        }

        /// <summary>
        /// Método responsável pelo comportamento de tela quando for clicado em ir para observação
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonGoObs_ItemClick( object sender, ItemClickEventArgs e )
        {
            GridView.FocusedColumn = gridColunaObs;
            GridView.SelectRow( GridView.FocusedRowHandle );
            GridView.ShowEditor();
            MemoExEdit editor = GridView.ActiveEditor as MemoExEdit;
            if(editor != null)
                editor.ShowPopup();
        }

        /// <summary>
        /// Método responsável pelo comportamento de tela quando for clicado em ir para estimativa (restante)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonGoEst_ItemClick( object sender, ItemClickEventArgs e )
        {
            GridView.FocusedColumn = gridColunaEstimativaRestante;
            GridView.SelectRow( GridView.FocusedRowHandle );
            GridView.ShowEditor();
            PopupBaseEdit editor = GridView.ActiveEditor as PopupBaseEdit;
            if(editor != null)
                editor.ShowPopup();
        }

        /// <summary>
        /// Responsável por mostrar ao esconder os campos atualizadoPor e atualizadoEm das tarefas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExibirHistoricoCheck_CheckedChanged( object sender, ItemClickEventArgs e )
        {
            BarCheckItem checkButton = sender as BarCheckItem;
            if(!checkButton.Checked)
            {
                gridColunaAtualizadoEm.Visible = false;
                gridColunaAtualizadoPor.Visible = false;
            }
            else
            {
                gridColunaAtualizadoEm.Visible = true;
                gridColunaAtualizadoPor.Visible = true;
            }
        }

        /// <summary>
        /// Método responsável por notificar uma mensagem contendo uma imagem
        /// </summary>
        /// <param name="titulo">titulo da mensagem</param>
        /// <param name="mensagem">mensagem de texto</param>
        /// <param name="foto">imagem de acompanhamento da mensagem</param>
        public void NotificarMensagemComFoto( string titulo, string mensagem, byte[] foto )
        {
            try
            {
                this.SafeInvoke( o =>
                {
                    Image imagem = null;
                    if(foto != null)
                    {
                        MemoryStream stream = new MemoryStream( foto );
                        imagem = Image.FromStream( stream );
                        imagem = ImageUtil.RedimensionarImagem( imagem, 64, 64 );
                    }
                    else
                        imagem = Resources.imagemPadrao;
                    o.MensagemPopup.Show( this, titulo, mensagem, imagem );
                } );
            }
            catch(Exception excessao)
            {
                throw excessao;
            }
        }

        /// <summary>
        /// Método responsável pelo comportamento quando for clicado sobre uma célula do grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_RowCellClick( object sender, RowCellClickEventArgs e )
        {
            //Verificar se a célula do grid clicada com o botão direito foi da coluna id
            if(e.Button == MouseButtons.Right && e.Column == gridColunaId)
            {
                //abrir o popup no local de clique do mouse
                popupMenu1.ShowPopup( Control.MousePosition );
                //setar o foco sobre o campo
                txEditIdPopup.Links[0].Focus();
            }
        }

        /// <summary>
        /// Método responsável pelo comportamento da tela quando o popup do menu de contexto sobre o grid for fechado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popupMenu1_CloseUp( object sender, EventArgs e )
        {
            if(!presenter.Conectado)
                return;

            //verificar se o campo do menu de contexto teve o valor modificado
            if(!string.IsNullOrEmpty( txEditIdPopup.EditValue as string ))
            {
                presenter.EsperarLeituraDataSource();

                //capturar o valor do campo do menu de contexto
                string num = txEditIdPopup.EditValue.ToString();
                int posicaoFinal = Convert.ToInt32( num );
                txEditIdPopup.EditValue = null;

                CronogramaTarefaGridItem tarefaSelecionada = ConsultarTarefaPorPosicaoSelecionada( GridView.FocusedRowHandle );
                CronogramaTarefaGridItem tarefaDestino = ConsultarTarefaPorPosicaoSelecionada( posicaoFinal - 1 );

                presenter.LiberarLeituraDataSource();

                this.SafeInvoke( o =>
                {
                    //enviar a posição selecionada para a movimentação
                    presenter.SolicitarMovimentacaoTarefa( tarefaSelecionada, tarefaDestino );
                } );
            }
        }

        /// <summary>
        /// Método responsável por atualizar a ultima ação do próprio usuário no cronograma
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="atualizacao"></param>
        public void AtualizarUltimaAcao( string atualizacao )
        {
            if(string.IsNullOrWhiteSpace( atualizacao ))
                atualizacao = string.Empty;

            this.SafeInvoke( o => { o.lbEstadoAcoes.Caption = atualizacao; } );
        }

        /// <summary>
        /// Método responsável por exibir a as tarefas no gridView de tarefa
        /// </summary>
        /// <param name="tarefas"></param>
        public void ExibirTarefas( BindingList<CronogramaTarefaGridItem> tarefas )
        {
            this.SafeInvoke( o =>
            {
                o.cronogramaTarefaBindingSource.DataSource = tarefas;
                cronogramaTarefaGridControl.DataSource = cronogramaTarefaBindingSource;
            } );
        }

        /// <summary>
        /// Método responsável por forçar a edição de uma tarefa no gridview
        /// </summary>
        public void ForcarFimEdicao()
        {
            this.SafeInvoke( o =>
            {
                o.GridView.CloseEditor();
            } );
        }

		/// <summary>
		/// Método responsável por forçar o fim da edição dos campos nome do cronograma, data de inicio e data de termino
		/// </summary>
		public void ForcarFimEdicaoDadosCronograma()
		{
			this.SafeInvoke( o =>
			{
                if(Menu.Manager.ActiveEditItemLink != null)
                    Menu.Manager.ActiveEditItemLink.CancelEditor();    
			} );
		}

        /// <summary>
        /// Seta o foco para uma tarefa
        /// </summary>
        /// <param name="posicao">posicao do foco</param>
        public void SetarFocoTarefa( int posicao )
        {
            if(!presenter.Conectado)
                return;

            this.SafeInvoke( o =>
            {
                if(posicao < 0 || posicao > tarefasCronograma.Count - 1)
                {
                    posicao = tarefasCronograma.Count - 1;
                }
                GridView.FocusedRowHandle = posicao;
            } );
        }

        /// <summary>
        /// Seta o foco para uma tarefa
        /// </summary>
        /// <param name="posicao">posicao do foco</param>
        public void AtribuirFocoTarefa( CronogramaTarefaGridItem cronogramaTarefaGridItem )
        {
            if(!presenter.Conectado)
                return;

            this.SafeInvoke( o =>
            {
                o.presenter.EsperarLeituraDataSource();

                o.GridView.ClearSelection();
                int posicao = presenter.BuscarIndiceDaTarefaNoDataSource( cronogramaTarefaGridItem );

                o.presenter.LiberarLeituraDataSource();

                o.GridView.SelectRow( posicao );
            } );
        }

        /// <summary>
        /// Método para alterar a situação planejamento via atalho na barra de atalho de situação planejamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ribbonGaleriaImagensSituacaoPlanejamento_GalleryItemClick( object sender, GalleryItemClickEventArgs e )
        {
            Menu.SelectedPage = Tarefas;
            string atalho = atalhosGaleriaSituacaoPlanejamento[e.Item.Description];
            presenter.ProcessarTeclaDeAtalhoSituacaoPlanejamento( atalho );
        }

        /// <summary>
        /// Método que é acionado quando clica no combo de cronogramas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barraComboCronogramaSelecionado_ShowingEditor( object sender, ItemCancelEventArgs e )
        {
            presenter.CarregarDadosCronograma();
        }

        /// <summary>
        /// Método responsável por habilitar a view quando usuário conectar.
        /// </summary>
        public void HabilitarAoConectar()
        {
            this.SafeInvoke( o =>
            {
                o.barraTxDescricaoCronograma.Enabled = true;
                o.cronogramaTarefaGridControl.Enabled = true;
                o.barraBtNovaTarefa.Enabled = true;
                o.barraBtExcluirTarefa.Enabled = true;
                o.HistoricoBarButton.Enabled = true;
                o.HabilitarViewTarefas();
            } );
        }

        /// <summary>
        /// Método responsável por verificar se filtro ou agrupamento de tarefas está ativo.
        /// </summary>
        /// <returns>Retorna a confirmação de que está ou não ativo</returns>
        public bool VerificarSeFiltroOuAgrupamentoDeTarefasEstaAtivo()
        {
            ViewFilter filtro = GridView.ActiveFilter;

            if(GridView.GroupCount > 0 || !filtro.IsEmpty)
                return true;

            return false;
        }

        /// <summary>
        /// Setar erro em uma determinada coluna
        /// </summary>
        /// <param name="coluna"></param>
        /// <param name="mensagem"></param>
        public void SetarErroColuna( string coluna, string mensagem )
        {
            switch(coluna.ToLower())
            {
                case "descricao":
                    GridView.SetColumnError( gridColunaDescricaoTarefa, mensagem );
                    ForcarEdicaoDescricaoTarefaAtual();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Método que remove uma nova tarefa considerada inválida
        /// </summary>
        private void RemoverNovaTarefaInvalida()
        {
            if(tarefasCronograma == null)
                return;

            if(tarefaRecentementeCriada)
            {
                tarefaRecentementeCriada = false;
                return;
            }

            var novaTarefa = tarefasCronograma.FirstOrDefault( o => o.OidCronogramaTarefa == new Guid() );
            var tarefaEmFoco = GridView.GetFocusedRow() as CronogramaTarefaGridItem;

            if(TarefaSelecionadaForUmaNovaTarefa( novaTarefa ))
                return;

            bool existeTarefaNova = novaTarefa != null;
            bool descricaoPreenchida = !string.IsNullOrWhiteSpace( novaTarefa.TxDescricaoTarefa );
            bool estaNaColunaDescricao = GridView.FocusedColumn == gridColunaDescricaoTarefa;

            if(!existeTarefaNova)
                return;

            if(!descricaoPreenchida)
            {
                presenter.RemoverTarefaDataSource( novaTarefa );
                ForcarFimEdicao();

                NotificarMensagem( Resources.Caption_Atencao, "Para criar uma tarefa você deve preencher o campo 'Tarefa', caso contrário a tarefa será descartada." );
            }
            else
            {
                if(!estaNaColunaDescricao && !presenter.AguardandoCriacaoTarefa)
                {
                    presenter.RemoverTarefaDataSource( novaTarefa );
                    ForcarFimEdicao();

                    NotificarMensagem( Resources.Caption_Atencao, "Para criar uma tarefa você deve preencher o campo 'Tarefa', caso contrário a tarefa será descartada." );
                }
            }
        }


        /// <summary>
        /// Método responsável pelo comportamento do filtro no grid, quando forem pressionados um dos checkedits
        /// de filtro de tarefas por situação planejamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filtroSituacao_ItemClick( object sender, ItemClickEventArgs e )
        {
            var filtro = e.Item as BarCheckItem;
            if(filtro != null)
            {
                switch(filtro.Caption.ToLower())
                {
                    case "todas":
                        AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Todas );
                        break;
                    case "pendentes":
                        AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Pendentes );
                        break;
                    case "encerradas":
                        AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Encerradas );
                        break;
                    case "personalizado":
                        AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Personalizado );
                        break;
                }
            }
        }

        /// <summary>
        /// Ao modificar um filtro de situação planejamento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_ColumnFilterChanged( object sender, EventArgs e )
        {
            if(GridView.ActiveFilterString == FiltrosSituacao["pendentes"]
                || GridView.ActiveFilterString == FiltrosSituacao["encerradas"] ||
                string.IsNullOrWhiteSpace( GridView.ActiveFilterString ))
            {
                return;
            }
            filtroSituacaoCustom.Checked = true;
            CronogramaPresenter.FiltroSituacaoPersonalizado = GridView.ActiveFilterString;
            AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento.Personalizado, GridView.ActiveFilterString );

        }

        /// <summary>
        /// Aplicar um filtro de seleção para as situações planejamento das tarefas
        /// </summary>
        /// <param name="filtro">filtro aplicado</param>
        /// <param name="filtroCustom">filtro costumizado</param>
        public void AplicarFiltroSituacao( CsFiltroSituacaoPlanejamento filtro, string filtroCustom = null )
        {
            switch(filtro)
            {
                case CsFiltroSituacaoPlanejamento.Todas:
                    GridView.ActiveFilter.Clear();
                    GerenciadorConfiguracao.SalvarFiltroSituacao( CsFiltroSituacaoPlanejamento.Todas );
                    filtroSituacaoTodas.Checked = true;
                    break;
                case CsFiltroSituacaoPlanejamento.Pendentes:
                    GridView.ActiveFilterString = FiltrosSituacao["pendentes"];
                    GerenciadorConfiguracao.SalvarFiltroSituacao( CsFiltroSituacaoPlanejamento.Pendentes );
                    filtroSituacaoPendentes.Checked = true;
                    break;
                case CsFiltroSituacaoPlanejamento.Encerradas:
                    GridView.ActiveFilterString = FiltrosSituacao["encerradas"];
                    GerenciadorConfiguracao.SalvarFiltroSituacao( CsFiltroSituacaoPlanejamento.Encerradas );
                    filtroSituacaoEncerradas.Checked = true;
                    break;
                case CsFiltroSituacaoPlanejamento.Personalizado:
                    GridView.ActiveFilterString = CronogramaPresenter.FiltroSituacaoPersonalizado;
                    GerenciadorConfiguracao.SalvarFiltroSituacaoCustom( GridView.ActiveFilterString );
                    filtroSituacaoCustom.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Comportamento executado quando fechado o combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboEstimativaInicial_Closed( object sender, ClosedEventArgs e )
        {
            ForcarFimEdicao();
        }


        public void ForcarEdicaoSituacaoPlanejamentoTarefa()
        {
            if(!presenter.Conectado)
                return;

            GridView.FocusedColumn = gridColunaSituacao;
            GridView.SelectRow( GridView.FocusedRowHandle );
            GridView.ShowEditor();
            GridView.CloseEditor();
        }

        /// <summary>
        /// Responsável pelo comportamento do dock do Burndown quando solicitado o fechamento
        /// </summary>
        /// <param name="sender">DockPanel</param>
        /// <param name="e">Parametros de fechamentod o panel</param>
        private void dockPanelBurndown_ClosingPanel( object sender, DockPanelCancelEventArgs e )
        {
            e.Cancel = true;
            if(e.Panel.Visibility != DockVisibility.AutoHide)
                e.Panel.Visibility = DockVisibility.AutoHide;
            e.Panel.HideImmediately();
        }

        /// <summary>
        /// Método responsável pelo comportamento do dockPanel ao receber um click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dockPanelBurndown_Click( object sender, EventArgs e )
        {
            var panel = sender as DockPanel;
            if(panel != null)
                panel.HideSliding();
        }

        /// <summary>
        /// Método responsável pelo comportamento quando for necessário atualizar os dados do cronograma
        /// </summary>
        /// <param name="dto">dto contendo dados do cronograma</param>
        public void AtualizarCronogramaSelecionado( CronogramaDto dto )
        {
            if(dto == null)
                return;

			this.SafeInvoke( o =>
			{
				barraTxDescricaoCronograma.EditValue = !string.IsNullOrEmpty( dto.TxDescricao ) ? dto.TxDescricao : string.Empty;
				barraComboCronogramaSelecionado.EditValue = !string.IsNullOrEmpty( dto.TxDescricao ) ? dto.TxDescricao : string.Empty;
				barraDtInicioCronograma.EditValue = dto.DtInicio;
				barraDtTerminoCronograma.EditValue = dto.DtFinal;
			} );
        }

        /// <summary>
        /// Atualizar o gráfico de burndown com os dados informados
        /// </summary>
        /// <param name="graficoDto">dados para renderizar o gráfico</param>
        public void AtualizarGraficoBurndown( BurndownGraficoDto graficoDto )
        {
            this.SafeInvoke( o =>
            {
                LimparGrafico( );
                PreencherTituloGrafico( graficoDto.Desvio );
				o.chartControl1.Series[0].Points.AddRange( graficoDto.DadosBurndown.Where( dados => dados.CsTipo == CsTipoBurndown.Planejado ).Select( CriarSeriePoint ).ToArray() );
				o.chartControl1.Series[1].Points.AddRange( graficoDto.DadosBurndown.Where( dados => dados.CsTipo == CsTipoBurndown.Realizado ).Select( CriarSeriePoint ).ToArray() );
            } );
        }

        /// <summary>
        /// Método responsável por preencher o titulo do gráfico de burndown
        /// </summary>
        /// <param name="valor">quantidade de horas de desvio</param>
        private void PreencherTituloGrafico( double valor )
        {
            Action<string,Color?> preencher = (msg,cor) =>  
            {
                chartControl1.Titles[0].Text = msg;
                if(cor.HasValue)
                    chartControl1.Titles[0].TextColor = cor.Value;
            };
            if(valor > 0)
            {
				preencher( string.Format( "Adiantado: {0:n2}h" , valor ) , Color.ForestGreen );
            }
            else
            {
                if(valor < 0)
                {
                    valor = -valor;
					preencher( string.Format( "Desvio: -{0:n2}h" , valor ) , Color.DarkRed );
                }
                else
                    preencher( "Em dia", Color.LightSlateGray );
            }
        }

        /// <summary>
        /// Método responsável por converter um BurndownDadosDto para um série point
        /// </summary>
        /// <param name="dados">dados do burndown</param>
        /// <returns>SeriesPoint que será plotado no gráfico</returns>
        private SeriesPoint CriarSeriePoint( BurndownDadosDto dados )
        {
            return new SeriesPoint( dados.Dia.ToString(@"dd/MMM"), dados.QtdeHoras.GetValueOrDefault() );
        }

        /// <summary>
        /// Método que efetua a remoçãod os pontos desenhados no gráfico
        /// </summary>
        private void LimparGrafico()
        {
            chartControl1.Series[0].Points.Clear();
            chartControl1.Series[1].Points.Clear();
        }

        /// <summary>
        /// Propriedade que armazena a visibilidade do gráfico de burndown
        /// </summary>
        public bool BurndownVisivel
        {
            get 
            { 
                return dockPanelBurndown.Visibility != DockVisibility.Hidden; 
            }
            set {} // não deve possuir implementação
        }

        private void dockPanelBurndown_VisibilityChanged( object sender, VisibilityChangedEventArgs e )
        {
            DockPanel panel = sender as DockPanel;
            if(e.Visibility != DockVisibility.Hidden) 
            {
                presenter.AtualizarGraficoBurndown();
            }
        }

		public DateTime DataInicio
		{
			get { return SelecionarDataDoComponente( barraDtInicioCronograma ); }
			set { barraDtInicioCronograma.EditValue = value; }
		}

		public DateTime DataTermino
		{
			get { return SelecionarDataDoComponente( barraDtTerminoCronograma ); }
			set { barraDtTerminoCronograma.EditValue = value; }
		}

		private DateTime SelecionarDataDoComponente(BarEditItem item) 
		{
			return ( (DateTime?)item.EditValue ).GetValueOrDefault();
		}
	}
}