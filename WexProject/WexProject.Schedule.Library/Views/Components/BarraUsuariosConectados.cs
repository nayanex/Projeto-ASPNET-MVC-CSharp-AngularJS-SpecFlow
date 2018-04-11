using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.XtraEditors;

namespace WexProject.Schedule.Library.Views.Components
{
    public partial class BarraUsuariosConectados : PanelControl
    {
        /// <summary>
        /// Tooltip único para setar e modificar o hint dos usuários
        /// </summary>
        private ToolTip hintControl;
        /// <summary>
        /// Armazenar o tamanho do panel Container dos panel colorido
        /// </summary>
        private Size tamanhoP1;
        /// <summary>
        /// Dicionário que armazena o nome do usuário e seu respectivo panel
        /// </summary>
        private Dictionary<string,Panel> usuariosEmPanel;

        /// <summary>
        /// Delegate com assinatura responsável por métodos que envolvem usuarios
        /// </summary>
        /// <param name="usuario">nome do usuario</param>
        public delegate void UsuariosEventHandler(string usuario);
        /// <summary>
        /// Delegate com assinatura responsável por métodos que envolvem cores
        /// </summary>
        /// <param name="cor">cor escolhida</param>
        public delegate void CoresEventHandler(Color cor);
        /// <summary>
        /// Evento responsável por alertar que uma cor já existe
        /// </summary>
        public event CoresEventHandler ExisteCor;
        /// <summary>
        /// Evento responsável por alertar que um usuário já existe no panel
        /// </summary>
        public event UsuariosEventHandler JaExisteEsteUsuarioNoPanel;
        /// <summary>
        /// Evento responsável por alertar que um usuário que seria removido, não existe no
        /// panel
        /// </summary>
        public event UsuariosEventHandler NaoExisteEsteUsuarioParaSerRemovido;

        /// <summary>
        /// Eventos responsável por disparar métodos quando necessitar a visibilidade da a barra seja alterada
        /// Visível - Quando Possuir Usuários
        /// Invisível - Quando Não possuir Usuários
        /// </summary>
        public event AoMudarEstadoDeVisibilidadeDaBarraEventHandler AoMudarEstadoDeVisibilidadeDaBarra;

        /// <summary>
        /// Delegate com assinatura responsável pelo eventos envolvendo a barra de usuários conectados
        /// </summary>
        public delegate void AoMudarEstadoDeVisibilidadeDaBarraEventHandler(bool estadoBarra);
        /// <summary>
        /// Construtor sem indicação de container
        /// </summary>
        public BarraUsuariosConectados()
        {
            Inicializacao();
            InitializeComponent();
        }

        /// <summary>
        /// Construtor com indicação do componente container no qual foi alocado
        /// </summary>
        /// <param name="container">componente pai</param>
        public BarraUsuariosConectados(IContainer container)
        {
            container.Add(this);
            Inicializacao();
            InitializeComponent();
            
        }

        /// <summary>
        /// Responsável pelas configurações de inicialização do componente
        /// </summary>
        private void Inicializacao()
        {
            hintControl = new ToolTip();
            usuariosEmPanel = new Dictionary<string,Panel>();
            Controls.Clear();
            Width = 28;
            Padding = new Padding(4);
            tamanhoP1 = new Size(20,20);
        }

        /// <summary>
        /// Adicionar um usuário no panel
        /// </summary>
        /// <param name="usuario">usuario conectado</param>
        /// <param name="atividade">ultima atividade executada pelo usuario</param>
        /// <param name="cor">cor escolhida para o usuario</param>
        public void AdicionarUsuarioConectadoAoPanel(string usuario,Color cor)
        {
            if (string.IsNullOrEmpty(usuario) || ValidarJaExisteEstaCorNoPanel(cor) || ValidarJaExisteEsteUsuarioNoPanel(usuario))
                return;
            // Panel principal dockado
            Panel p = new Panel();
            p.Dock = DockStyle.Top;
            p.Padding = new Padding(4);
            p.Size = tamanhoP1;
            //Sub Panel - Colorido do usuário
            Panel p2 = new Panel() { BackColor = cor, Size = new Size(12,12), Visible = false};
            p.Controls.Add(p2);
            AdicionarHint(p2,usuario,DateTime.Now);
            usuariosEmPanel.Add(usuario,p);
            Controls.Add(p);
            p2.Visible = true;
        }

        /// <summary>
        /// Recalcular a altura do painel conforme estão sendo adicionados os usuários
        /// </summary>
        private void CalcularNovaAlturaPanel()
        {
            Height = usuariosEmPanel.Sum(o=>o.Value.Height) + Padding.Vertical;
        }

        /// <summary>
        /// Responsável por verificar a existencia de uma determinada cor na hash de usuarios conectados no panel
        /// </summary>
        /// <param name="cor"> cor escolhida</param>
        /// <returns>
        /// True - Ja existe esta sendo utilizada a cor
        /// False - Nao esta sendo utilizada a cor
        /// </returns>
        private bool JaExisteCor(Color cor)
        {
            return usuariosEmPanel.Count(o => o.Value.BackColor == cor) > 0;
        }

        /// <summary>
        /// Responsável por verificar a existencia de uma determinada cor na hash de usuarios conectados no panel
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        private bool ExisteUsuario(string usuario)
        {
            return usuariosEmPanel.ContainsKey(usuario);
        }

        /// <summary>
        /// Responsável por validar se uma cor ja esta em utilização
        /// </summary>
        /// <param name="cor">cor escolhida</param>
        /// <returns>
        /// True - Ja existe esta sendo utilizada a cor
        /// False - Nao esta sendo utilizada a cor
        /// </returns>
        private bool ValidarJaExisteEstaCorNoPanel(Color cor)
        {
            bool possui = JaExisteCor(cor);
            if (possui)
            {
                if (ExisteCor != null)
                    ExisteCor(cor);
            }
            return possui;
        }

        /// <summary>
        /// Responsável por validar se ja existe um determinado usuário inserido no panel
        /// Opcional:
        ///  - Caso Necessário disparar alguma mensagem de aviso utilizar os eventos:
        ///    - JaExisteEsteUsuarioNoPanel - avisar que usuário já existe
        ///    - NaoExisteEsteUsuarioParaSerRemovido - avisar que o usuário que devera ser removido não existe
        /// </summary>
        /// <param name="usuario">Usuario a ser procurado</param>
        /// <returns>
        /// True - Existe no Panel
        /// False - Não Existe no Panel
        /// </returns>
        private bool ValidarJaExisteEsteUsuarioNoPanel(string usuario)
        {
            bool possui = ExisteUsuario(usuario);
            if (possui)
            {
                if (JaExisteEsteUsuarioNoPanel != null)
                    JaExisteEsteUsuarioNoPanel(usuario);
            }
            else
            {
                if (NaoExisteEsteUsuarioParaSerRemovido != null)
                    NaoExisteEsteUsuarioParaSerRemovido(usuario);
            }

            return possui;
        }

        /// <summary>
        /// Responsável por remove um usuário do painel de usuários conectados
        /// </summary>
        /// <param name="usuario"></param>
        public void RemoverUsuarioConectadoAoPanel(string usuario)
        {
            if (string.IsNullOrEmpty(usuario))
                return;

            if (!ExisteUsuario(usuario))
            {
                if (NaoExisteEsteUsuarioParaSerRemovido != null)
                    NaoExisteEsteUsuarioParaSerRemovido(usuario);
                return;
            }

            Panel p = usuariosEmPanel[usuario];
            usuariosEmPanel.Remove(usuario);
            Controls.Remove(p);
        }

        /// <summary>
        /// Responsável por reinicializar o componente, removendo todos usuários do painel
        /// </summary>
        public void RemoverTodosUsuariosDoPanel()
        {
            Inicializacao();
        }

        /// <summary>
        /// Responsável por verificar a visibilidade do componente, sempre visivel enquanto possuir usuário conectado
        /// e não visível quando não possuir usuário conectado
        /// </summary>
        public void VerificarSeExibePanel()
        {
            if (usuariosEmPanel.Count > 0)
            {
                CalcularNovaAlturaPanel();
                if (AoMudarEstadoDeVisibilidadeDaBarra != null)
                AoMudarEstadoDeVisibilidadeDaBarra(true);
            }
            else
            {
                if (AoMudarEstadoDeVisibilidadeDaBarra != null)
                AoMudarEstadoDeVisibilidadeDaBarra(false);
            }
        }

        /// <summary>
        /// Responsável por adicionar um hint a um determinado panel
        /// </summary>
        /// <param name="p">panel que receberá o hint</param>
        /// <param name="usuario">login do colaborador acrescentado ao hint</param>
        /// <param name="atividade">última atividade realizada pelo usuário</param>
        private void AdicionarHint(Panel p,string usuario,DateTime dataInicio, string atividade = null)
        {
            if (!string.IsNullOrEmpty(usuario))
                if(atividade == null)
                    hintControl.SetToolTip(p,String.Format("Nome: {0} \nNenhuma atividade recente!",usuario,dataInicio));
                else
                    hintControl.SetToolTip(p,String.Format("Nome: {0}\nAtividade:{1}\nQuando:{2}",usuario,atividade,dataInicio));
        }

        /// <summary>
        /// Responsável por localizar e modificar o hint de um determindo panel
        /// </summary>
        /// <param name="login">usuário conectado que terá o hint modificado</param>
        /// <param name="atividade">última atividade realizada pelo usuário</param>
        public void ModificarHintUsuario(string login,string atividade,DateTime dataInicio)
        {
            if (!usuariosEmPanel.ContainsKey(login))
                return;
            Panel p = (Panel)usuariosEmPanel[login].Controls[0];
            AdicionarHint(p,login,dataInicio,atividade);
        }
    }
}
