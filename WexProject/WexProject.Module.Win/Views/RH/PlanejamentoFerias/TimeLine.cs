using System;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraScheduler;
using WexProject.Module.Win.TelasForaPadrao.RH.PlanejamentoFerias.UserControls;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using WexProject.BLL.Models.Rh;
using DevExpress.XtraNavBar;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraBars;
using WexProject.BLL.Shared.Domains.Geral;
using DevExpress.XtraScheduler.Drawing;
using System.Drawing;
using WexProject.Module.Win.Properties;
using System.Windows.Forms;
using WexProject.Library.Libs.Enumerator;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.DAOs;
using WexProject.Library.Libs.Img;
using WexProject.BLL.DAOs.Geral;

namespace WexProject.Module.Win.TelasForaPadrao.RH.TimeLineView
{
    /// <summary>
    /// Formulário de Visualização das Férias Planejadas dos Colaboradores Alocados e Não-Alocados nos Projetos
    /// </summary>
    public partial class TimeLine : DevExpress.XtraEditors.XtraForm
    {
        #region Properties
        /// <summary>
        /// Flag indicando se o form está sendo construido
        /// obs.: serve para filtrador para o método TimeLine
        /// não ser chamado mais do que 2 vezes assim que o form
        /// é construído.
        /// </summary>
        private bool IsInConstructor { get; set; }
        /// <summary>
        /// Colaborador Atual logado no sistema
        /// </summary>
        public Colaborador CurrentColaborador { get; set; }
        /// <summary>
        /// Dicionario de fotos dos colaboradores
        /// </summary>
        public Dictionary<Guid,Image> FotosColaboradores { get; set; }
        /// <summary>
        /// Determina o tamanho da fonte que deverá aparecer para o usuário
        /// </summary>
        public Font FonteAppointment { get; set; }
        /// <summary>
        /// Determina o tamanho do retangulo do nome do colaborador no appoitment
        /// </summary>
        public Rectangle retanguloAppointment = new Rectangle(400, 700, 39, 150);
        #endregion

        #region Constructors
        /// <summary>
        /// Construtor do Formulário
        /// </summary>
        /// <param name="session">Sessão Atual</param>
        public TimeLine(Session session)
        {
            InitializeComponent();
            FotosColaboradores = new Dictionary<Guid, Image>();
            FonteAppointment = new Font(FontFamily.GenericSansSerif, 6);
            navBarControlProjetos.LargeImages = new List<Image>() { Resources.projetox16, Resources.BO_Customer };
            navBarControlProjetos.GroupBackgroundImage = Resources.projetox16;
            navBarControlProjetos.SmallImages = new List<Image>() { Resources.projetox16, Resources.BO_Customer };
            Session = session;
            Ferias.Session = session;
            CurrentColaborador = Colaborador.GetColaboradorCurrent(Session);            
            PopulateFiltersSuperiorImediato();
            PopulateFilterSituacaoFerias();
            PopulateFilterPeriodos();
        }
        #endregion

        #region Utils
        /// <summary>
        /// Método de Criação dos Grupos e Itens de Grupo de Projeto
        /// </summary>
        private void PopulateNavBar()
        {
            navBarControlProjetos.SuspendLayout();
            string[] oids;
            //if para verificar opção já previamente selecionada
            if (barEditItemSuperiorImediato.EditValue != null)
            {
                oids = barEditItemSuperiorImediato.EditValue.ToString().Split(',');
                if (oids.Contains(""))
                    oids = new string[0];
                }
            else
                oids = new string[0];
            var projetos = ProjetoParteInteressada.GetProjetosBySuperioresImediatos(oids);
            /*if (barEditItemSuperiorImediato.EditValue == null || barEditItemSuperiorImediato.EditValue.ToString() == "")
                projetos = ProjetoParteInteressada.GetProjetosByNoSuperioresImediatos(Session, oids);*/
            var groups = projetos.GroupBy(g => g.ToString().Split(',')[1]);
            if (navBarControlProjetos.Groups.Count > 0)
                navBarControlProjetos.Groups.Clear();
            foreach (var projeto in groups)
            {
                NavBarGroup navGroup = new NavBarGroup()
                {
                    Caption = projeto.Key.Split('=')[1].Trim(),
                    Expanded = true
                };
                navBarControlProjetos.Groups.Add(navGroup);
                navGroup.Appearance.Options.UseImage = true;
                navGroup.GroupCaptionUseImage = NavBarImage.Small;
                navGroup.SmallImageIndex = 0;
                navGroup.LargeImage = Resources.projetox16;
                navGroup.SmallImage = Resources.projetox16;
                string[] participantes_oids = new string[projeto.Count()];
                int index = 0;
                foreach (var participante in projeto)
                {
                    NavBarItem nvItem = new NavBarItem()
                    {
                        Caption = participante.ToString().Split(',')[2].Split('=')[1],//nome do colaborador
                        Tag = new Guid(participante.ToString().Split(',')[3].Split('=')[1])//Oid do colaborador
                    };
                    nvItem.Appearance.Options.UseImage = true;
                    nvItem.SmallImageIndex = 1;
                    nvItem.LargeImage = Resources.BO_Customer;
                    nvItem.SmallImage = Resources.BO_Customer;
                    participantes_oids[index] = participante.ToString().Split(',')[3].Split('=')[1];//Oid do colaborador
                    navGroup.ItemLinks.Add(nvItem);
                    index++;
                }
                navGroup.Tag = participantes_oids;
            }
            navBarControlProjetos.ResumeLayout();
        }
        /// <summary>
        /// Método de População do Filtro de Superior Imediato
        /// </summary>
        private void PopulateFiltersSuperiorImediato()
        {
            foreach (Colaborador superior_imediato in Colaborador.GetAllSuperioresImediatos(Session))
                repositoryItemCheckedComboBoxEditSuperiorImediato.Items.Add(new CheckedListBoxItem(superior_imediato.Oid, superior_imediato.Usuario.FullName));
            if (CurrentColaborador.ColaboradorUltimoFiltro.LastSuperiorImediatoFilterPlanejamentoFerias != null)
            {
                if (Colaborador.GetAllSuperioresImediatos(Session).Contains(CurrentColaborador) && CurrentColaborador.ColaboradorUltimoFiltro.LastSuperiorImediatoFilterPlanejamentoFerias.Equals(""))
                {
                    barEditItemSuperiorImediato.EditValue = CurrentColaborador.Oid.ToString();
            }
                else
            {
                    barEditItemSuperiorImediato.EditValue = CurrentColaborador.ColaboradorUltimoFiltro.LastSuperiorImediatoFilterPlanejamentoFerias;
            }
        }
            else
            {
                if (CurrentColaborador.IsSuperiorImediato(Session, UsuarioDAO.GetUsuarioLogado(Session)))
                    barEditItemSuperiorImediato.EditValue = CurrentColaborador.Oid.ToString();
                else
                    barEditItemSuperiorImediato.EditValue = string.Empty;
                if (Colaborador.GetAllSuperioresImediatos(Session).Contains(CurrentColaborador))
                    barEditItemSuperiorImediato.EditValue = CurrentColaborador.Oid.ToString();               
            }
        }
        /// <summary>
        /// Método de População do Filtro de Situação de Férias
        /// </summary>
        private void PopulateFilterSituacaoFerias()
        {
            string[] listaDescricao = { "Com férias planejadas", "Com férias em atraso", "Com férias realizadas", "Com licenças não renumeradas", " Com licenças renumeradas" };
            string[] listaValues = { "CsSituacao = 'Planejado'", "CsSituacao = 'EmAtraso'", "CsSituacao = 'Realizado'", "Afastamento.TipoAfastamento.IsRemunerado = 'false'","Afastamento.TipoAfastamento.IsRemunerado = 'true'" };
            for (int i = 0; i < listaValues.Length; i++)
                repositoryItemCheckedComboBoxEditSituacaoFerias.Items.Add(new CheckedListBoxItem(listaValues[i], listaDescricao[i]));
            if (CurrentColaborador.ColaboradorUltimoFiltro.LastSituacaoFeriasFilterPlanejamentoFerias != null)
                barEditItemSituacaoFerias.EditValue = CurrentColaborador.ColaboradorUltimoFiltro.LastSituacaoFeriasFilterPlanejamentoFerias;
            else
                barEditItemSituacaoFerias.EditValue = string.Empty;
            }
        /// <summary>
        /// Método de População do Filtro de Períodos
        /// </summary>
        private void PopulateFilterPeriodos()
        {
            repositoryItemComboBoxMesInicial.Items.Clear();
            repositoryItemComboBoxMesInicial.Items.AddRange(Enum.GetNames(typeof(CsMesDomain)));
            barEditItemMesInicial.EditValue = Enum.GetName(typeof(CsMesDomain), CsMesDomain.Janeiro);
            repositoryItemComboBoxMesFinal.Items.Clear();
            repositoryItemComboBoxMesFinal.Items.AddRange(Enum.GetNames(typeof(CsMesDomain)));
            barEditItemMesFinal.EditValue = Enum.GetName(typeof(CsMesDomain), CsMesDomain.Dezembro);
            DateTime max = DateTime.Now;
            DateTime min = DateTime.Now;
            using (XPCollection<FeriasPlanejamento> ferias = new XPCollection<FeriasPlanejamento>(Session))
            {
                ferias.Sorting.Add(new SortProperty("DtInicio", DevExpress.Xpo.DB.SortingDirection.Ascending));
                if (ferias.Count > 0)
                {
                    min = ferias[0].DtInicio;
                    max = ferias[ferias.Count - 1]._DtRetorno;
                }
            }
            int interval = max.Year - min.Year;
            repositoryItemComboBoxAnoFinal.Items.Clear();
            repositoryItemComboBoxAnoInicial.Items.Clear();
            repositoryItemComboBoxAnoFinal.Items.Add(max.Year);
            repositoryItemComboBoxAnoInicial.Items.Add(max.Year);
            for (int i = 0; i < interval; i++)
            {
                repositoryItemComboBoxAnoFinal.Items.Add(max.Year - i - 1);
                repositoryItemComboBoxAnoInicial.Items.Add(max.Year - i - 1);
            }
            barEditItemAnoInicial.EditValue = max.Year;
            barEditItemAnoFinal.EditValue = max.Year;
        }
        /// <summary>
        /// Método de População da View da Linha do Tempo/Mês
        /// </summary>
        private void PopulateTimeLine()
        {
            DateTime inicio = new DateTime(DateTime.Now.Year, 01, 01);
            DateTime final = new DateTime(DateTime.Now.Year, 12, 31);
            if (barEditItemAnoInicial.EditValue != null && barEditItemMesInicial.EditValue != null)
                inicio = new DateTime((int)barEditItemAnoInicial.EditValue, (int)EnumUtil.ValueEnum(typeof(CsMesDomain), barEditItemMesInicial.EditValue.ToString()), 01);
            if (barEditItemAnoFinal.EditValue != null && barEditItemMesFinal.EditValue != null)
                final = new DateTime((int)barEditItemAnoFinal.EditValue, (int)EnumUtil.ValueEnum(typeof(CsMesDomain), barEditItemMesFinal.EditValue.ToString()), 28);
            string[] superiores_oids;
            if (barEditItemSuperiorImediato.EditValue != null)
            {
                var array = from item in barEditItemSuperiorImediato.EditValue.ToString().Split(',')
                            where
                            !item.Equals("")
                            select item.Trim();
                superiores_oids = array.Count() == 0 ? new string[0] : array.ToArray<string>();
            }
            else
            {
                superiores_oids = new string[0];
            }
            string[] situacoes_ferias;
            if (barEditItemSituacaoFerias.EditValue != null)
            {
                var array = from item in barEditItemSituacaoFerias.EditValue.ToString().Split(',')
                            where
                            !item.Equals("")
                            select item.Trim();
                situacoes_ferias = array.Count() == 0 ? new string[0] : array.ToArray<string>();
            }
            else
            {
                situacoes_ferias = new string[0];
            }
            schedulerStorage1.Appointments.Clear();
            Ferias.AddRange(FeriasPlanejamento.GetPlanejamentoFerias(Session, superiores_oids, situacoes_ferias, inicio, final));
            schedulerControl1.RefreshData();
            PopulateItemNoSuperior();
                }
        /// <summary>
        /// Método para criar o form de edição de planejamento de férias
        /// </summary>
        /// <param name="Appointment">Appointment</param>
        /// <returns>PlanejamentoFeriasModelViewForm</returns>
        private PlanejamentoFeriasModelViewForm CreatePopupPlanejamentoFerias(Appointment Appointment)
        {
            PlanejamentoFeriasModelViewForm modal = new PlanejamentoFeriasModelViewForm(Session, Appointment);
            modal.LookAndFeel.ParentLookAndFeel = schedulerControl1.LookAndFeel;
            DialogResult result = modal.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                PopulateFilterPeriodos();
                PopulateTimeLine();
            }
            return modal;
        }
        /// <summary>
        /// Método de filtro de superior imediato
        /// </summary>
        private void PopulateItemNoSuperior()
        {
            List<CriteriaOperator> criterias = new List<CriteriaOperator>();
            foreach (NavBarGroup navGroupItens in navBarControlProjetos.Groups)
                if (navGroupItens.Expanded)
                    foreach (NavBarItemLink itemColaborador in navGroupItens.ItemLinks)
                        criterias.Add(CriteriaOperator.Parse("Periodo.Colaborador.Oid = ?", itemColaborador.Item.Tag));
            Ferias.Filter = CriteriaOperator.Or(criterias);
            schedulerStorage1.RefreshData();
            schedulerControl1.RefreshData();
        }
        /// <summary>
        /// Verifica se a Data de Inicio de Periodo é maior
        /// que Data de Fim do Período no Filtro de Períodos
        /// </summary>        
        private void VerifyDatesInicioFinalPeriodo() 
        {
            DateTime inicio = new DateTime((int)barEditItemAnoInicial.EditValue, (int)EnumUtil.ValueEnum(typeof(CsMesDomain), barEditItemMesInicial.EditValue.ToString()), 01);
            DateTime final = new DateTime((int)barEditItemAnoFinal.EditValue, (int)EnumUtil.ValueEnum(typeof(CsMesDomain), barEditItemMesFinal.EditValue.ToString()), 28);
            if (inicio.CompareTo(final)>0)
                throw new DevExpress.ExpressApp.UserFriendlyException("Data Inicial no Filtro de Períodos não pode ser maior que a Data Final");
        }
        #endregion

        #region Events
        /// <summary>
        /// Evento disparado quando um Group do Barra de Navegação é reduzido
        /// </summary>
        /// <param name="sender">NavBarGroup</param>
        /// <param name="e">NavBarGroupEventArgs</param>
        private void navBarControlProjetos_GroupCollapsed(object sender, NavBarGroupEventArgs e)
        {
            List<CriteriaOperator> criterias = new List<CriteriaOperator>();
            foreach (NavBarGroup navGroupItens in e.Group.Collection)
                if (navGroupItens.Expanded)
                    foreach (NavBarItemLink itemColaborador in navGroupItens.ItemLinks)
                        criterias.Add(CriteriaOperator.Parse("Periodo.Colaborador.Oid = ?", itemColaborador.Item.Tag));
            if (criterias.Count > 0)
                Ferias.Filter = CriteriaOperator.Or(criterias);
            else
                //colocando 1=2 não traz nada
                Ferias.Filter = CriteriaOperator.Parse("1=2");
            schedulerStorage1.RefreshData();
            schedulerControl1.RefreshData();
        }
        /// <summary>
        /// Evento disparado quendo um Group da Barra de Navegação é expandido
        /// </summary>
        /// <param name="sender">NavBarGroup</param>
        /// <param name="e">NavBarGroupEventArgs</param>
        private void navBarControlProjetos_GroupExpanded(object sender, NavBarGroupEventArgs e)
        {
            List<CriteriaOperator> criterias = new List<CriteriaOperator>();
            foreach (NavBarGroup navGroupItens in e.Group.Collection)
                if (navGroupItens.Expanded)
                    foreach (NavBarItemLink itemColaborador in navGroupItens.ItemLinks)
                        criterias.Add(CriteriaOperator.Parse("Periodo.Colaborador.Oid = ?", itemColaborador.Item.Tag));
            Ferias.Filter = CriteriaOperator.Or(criterias);
            schedulerStorage1.RefreshData();
            schedulerControl1.RefreshData();
        }
        /// <summary>
        /// Evento chamado quando se clica em um item do navbar
        /// </summary>
        /// <param name="sender">Objeto selecionado</param>
        /// <param name="e">evento</param>
        private void navBarControlProjetos_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            if (e.Link != null)
            {
                try
                 {
                    Colaborador colab = Session.FindObject<Colaborador>(CriteriaOperator.Parse("Oid=?", new Guid(e.Link.Item.Tag.ToString())));
                    colab.PeriodosAquisitivos.Sorting.Add(new SortProperty("DtInicio", DevExpress.Xpo.DB.SortingDirection.Descending));
                    var periodo = colab.PeriodosAquisitivos.First(single => single.Planejamentos.Count > 0);
                    periodo.Planejamentos.Sorting.Add(new SortProperty("DtInicio", DevExpress.Xpo.DB.SortingDirection.Ascending));
                    FeriasPlanejamento ferias = periodo.Planejamentos[0];
                    schedulerControl1.TimelineView.SelectAppointment(schedulerStorage1.Appointments.GetAppointmentById(ferias.Oid));
            }
                catch
                {                 
        }
            }
        }
        /// <summary>
        /// Evento chamado quando se muda um item do CheckedComboBox
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento</param>
        private void barEditItemSuperiorImediato_EditValueChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            PopulateNavBar();
            if (IsInConstructor)
            {
                PopulateTimeLine();
                BarEditItem combo = sender as BarEditItem;
                if (combo != null)
                    Colaborador.RnSetUltimoFiltroSuperiorImediato(Session, combo.EditValue.ToString(), CurrentColaborador);
                    }
            ResumeLayout(true);
        }
        /// <summary>
        /// Evento chamado quando se muda um item do checkedComboBox de situação de ferias
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">evento</param>
        private void barEditItemSituacaoFerias_EditValueChanged(object sender, EventArgs e)
        {           
            PopulateTimeLine();
            if (IsInConstructor)
            {
                BarEditItem combo = sender as BarEditItem;
                if (combo != null)
                    Colaborador.RnSetUltimoFiltroSituacaoFerias(Session, combo.EditValue.ToString(), CurrentColaborador);
                    }
                }
        /// <summary>
        /// Evento disparado 
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">AppointmentFormEventArgs</param>
        private void schedulerControl1_EditAppointmentFormShowing(object sender, AppointmentFormEventArgs e)
        {
            Appointment apt = e.Appointment;
            e.DialogResult = CreatePopupPlanejamentoFerias(apt).DialogResult;
            e.Handled = true;
        }
        /// <summary>
        /// Muda a back color dos appointments de acordo com as férias do colaborador
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">AppointmentViewInfoCustomizingEventArgs</param>
        private void schedulerControl1_AppointmentViewInfoCustomizing(object sender, AppointmentViewInfoCustomizingEventArgs e)
        {
            string situacao = e.ViewInfo.Appointment.Description.Split(',')[6];
            Color cor_apt;
            switch (situacao)
            {
                case "EmAtraso": cor_apt = Color.FromArgb(255, 106, 106); break;
                case "Realizado": cor_apt = Color.FromArgb(110, 163, 217); break;
                case "Planejado": cor_apt = Color.FromArgb(255, 255, 98); break;
                case "EmAndamento": cor_apt = Color.LimeGreen; break;
                case "Vendida": cor_apt = Color.LightGreen; break;
                default: cor_apt = Color.FromArgb(191,177,207);//outras licenças
                    break;
            }
            e.ViewInfo.Appearance.BackColor = cor_apt;
        }
        /// <summary>
        /// Evento chamado para colocar imagens nos appointments
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">AppointmentImagesEventArgs</param>
        private void schedulerControl1_InitAppointmentImages(object sender, AppointmentImagesEventArgs e)
        {
            SuspendLayout();
            FeriasPlanejamento ferias = Session.FindObject<FeriasPlanejamento>(CriteriaOperator.Parse("Oid=?", new Guid(e.Appointment.Id.ToString())));
            e.Appointment.Duration = ferias._DuracaoFerias;
            Image img;
            if (ferias.Periodo.Colaborador._Foto != null)
                img = ImageUtil.ResizeImage(ferias.Periodo.Colaborador._Foto, 32, 32);
            else
                img = ImageUtil.ResizeImage(Resources.no_image,32,32);
            if (!FotosColaboradores.Keys.Contains(ferias.Oid))
                FotosColaboradores.Add(ferias.Oid, img);
            ResumeLayout();
            }
        /// <summary>
        /// Evento para posionar a imagem do colaborador no centro do Appointment e o nome do mesmo embaixo da foto
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CustomDrawObjectEventArgs</param>
        private void schedulerControl1_CustomDrawAppointment(object sender, CustomDrawObjectEventArgs e) 
            {
            TimeLineAppointmentViewInfo time = e.ObjectInfo as TimeLineAppointmentViewInfo;            
            if (time != null) 
            {
                Image image = FotosColaboradores[new Guid(time.Appointment.Id.ToString())];
                int width = time.InnerBounds.Width;                
                int x = time.InnerBounds.X; 
                e.Cache.Paint.DrawImage(e.Graphics, image, new Point(x + (width/2) - (image.Width/2), time.InnerBounds.Top));
                string textColaborador = time.Appointment.Subject;
                if (FonteAppointment.Size >= 9)
                    if (textColaborador.Length > 15)
                        retanguloAppointment.X = x + (width / 2) - (image.Width) - textColaborador.Length - 14;
                    else
                        retanguloAppointment.X = x + (width / 2) - (image.Width) - textColaborador.Length + 3;
                else
                    textColaborador = textColaborador.Replace(" ", "\n");
                    retanguloAppointment.X = x + (width / 2) - (image.Width / 2);
                retanguloAppointment.Y = time.InnerBounds.Top + image.Height + 5;
                e.Cache.DrawString(textColaborador, FonteAppointment, new SolidBrush(Color.Black), retanguloAppointment, StringFormat.GenericDefault);
                e.Handled = true;
        }
        }
        /// <summary>
        /// Evento chamado quando o edit value do mês inicial é mudado
        /// repopulando assim o time line view
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void barEditItemMesInicial_EditValueChanged(object sender, EventArgs e)
        {
            if (IsInConstructor)
                try
            {
                    VerifyDatesInicioFinalPeriodo();
                PopulateTimeLine();
            }
                catch (DevExpress.ExpressApp.UserFriendlyException ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message);
        }
        }
        /// <summary>
        /// Evento chamado quando o edit value do mês final é mudado
        /// repopulando assim o time line view
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void barEditItemMesFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (IsInConstructor)
                try
            {
                    VerifyDatesInicioFinalPeriodo();
                PopulateTimeLine();
            }
                catch (DevExpress.ExpressApp.UserFriendlyException ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message);
        }
        }
        /// <summary>
        /// Evento chamado quando o edit value do ano inicial é mudado
        /// repopulando assim o time line view
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void barEditItemAnoInicial_EditValueChanged(object sender, EventArgs e)
        {
            if (IsInConstructor)
                try
            {
                    VerifyDatesInicioFinalPeriodo();
                PopulateTimeLine();
                    //PopulateFilterPeriodos();
            }
                catch (DevExpress.ExpressApp.UserFriendlyException ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message);
        }
            
        }
        /// <summary>
        /// Evento chamado quando o edit value do ano final é mudado
        /// repopulando assim o time line view
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void barEditItemAnoFinal_EditValueChanged(object sender, EventArgs e)
        {
            if (IsInConstructor)
                try
            {
                    VerifyDatesInicioFinalPeriodo();
                PopulateTimeLine();
            }
                catch (DevExpress.ExpressApp.UserFriendlyException ex)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(ex.Message);
        }
        }
        /// <summary>
        /// Evento do hint
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ToolTipControllerShowEventArgs</param>
        private void toolTipController1_BeforeShow(object sender, DevExpress.Utils.ToolTipControllerShowEventArgs e)
        {
            e.AllowHtmlText = DevExpress.Utils.DefaultBoolean.True;
            TimeLineAppointmentViewInfo info = e.SelectedObject as TimeLineAppointmentViewInfo;
            if (info != null)
            {
                e.Title = string.Format("{0} ({1} dias)", info.Appointment.Subject, info.Appointment.Duration.Days + 1);
                e.IconType = DevExpress.Utils.ToolTipIconType.Information;
                string[] properties = info.Appointment.Description.Split(',');
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                builder.Append("- Inicio: ").Append(string.Format("{0:dd/MM/yyyy}", info.Appointment.Start.Date)).Append("\n");
                builder.Append("- Término: ").Append(string.Format("{0:dd/MM/yyyy}", info.Appointment.End.Date)).Append("\n");
                builder.Append("- Venda de férias: ").Append(properties[7]).Append("\n");
                builder.Append("- Remunerado: ").Append(properties[8]).Append("\n");
                builder.Append("- Situação: ").Append(properties[6]).Append("\n");
                if (!string.IsNullOrEmpty(properties[4]))
                builder.Append("- Planejado por: ").Append(properties[4]).Append("\n");
                if (!string.IsNullOrEmpty(properties[5]))
                builder.Append("- Atualizado por: ").Append(properties[5]).Append("\n");
                e.ToolTip = builder.ToString();
            }
        }
        /// <summary>
        /// evento para criar um novo planejamento
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">args</param>
        private void barButtonItemNovo_ItemClick(object sender, ItemClickEventArgs e)
        {
            CreatePopupPlanejamentoFerias(new Appointment());
            }
        /// <summary>
        /// Evento para editar um planejamento antigo
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">ItemClickEventArgs</param>
        private void barButtonItemEditar_ItemClick(object sender, ItemClickEventArgs e)
            {
            if (schedulerControl1.SelectedAppointments.Count > 0)
                CreatePopupPlanejamentoFerias(schedulerControl1.SelectedAppointments.OrderBy(o => o.Start).ToList()[0]);
            }
        /// <summary>
        /// Evento chamado quando o form está visivel
        /// e para settar uma flag para true para q entre nos filtros
        /// de períodos
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void TimeLine_Shown(object sender, EventArgs e)
        {
            IsInConstructor = true;
        }
        /// <summary>
        /// Quando houver o clique deverá aumentar o tamanho da fonte para o apointment
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void viewNavigatorZoomInItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            retanguloAppointment.Width = retanguloAppointment.Width + 50;
            FonteAppointment = new Font(FontFamily.GenericSansSerif, FonteAppointment.Size + 1);
        }
        /// <summary>
        /// Quando houver o clique deverá diminuir o tamanho da fonte para o apointment
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void viewNavigatorZoomOutItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            FonteAppointment = new Font(FontFamily.GenericSansSerif, FonteAppointment.Size - 1);
        }
        #endregion
        
    }
}