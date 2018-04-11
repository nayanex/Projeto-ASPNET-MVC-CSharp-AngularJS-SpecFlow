using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors.Mask;
using WexProject.BLL.Models.Planejamento;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web.ASPxEditors;
using WexProject.BLL.Models.Geral;

namespace WexProject.Module.Lib.ControllerPraWinWeb.Planejamento
{
    /// <summary>
    /// Controller para a classe em Empresa/Intituição
    /// </summary>
    public partial class EmpresaInstituicaoViewController : ViewController
    {
        #region Properties
        /// <summary>
        /// LookUp Edit de País (WIN)
        /// </summary>
        public LookupEdit WinLookUpEdit { get; set; }
        /// <summary>
        /// Text Edit da Máscara (WIN)
        /// </summary>
        public TextEdit WinTextEdit { get; set; }
        /// <summary>
        /// Text Edit com a Máscara (WIN)
        /// </summary>
        public StringEdit WinTextEditWMask { get; set; }
        /// <summary>
        /// LookUp Edit de País (WEB)
        /// </summary>
        public ASPxLookupPropertyEditor WebLookUpEdit { get; set; }
        /// <summary>
        /// Text Edit da Máscara (WEB)
        /// </summary>
        public ASPxStringPropertyEditor WebTextEdit { get; set; }
        /// <summary>
        /// Edit Text (WEB)
        /// </summary>
        public ASPxStringPropertyEditor WebTextEditWMask { get; set; }
        /// <summary>
        /// País Atual
        /// </summary>
        public Pais Pais { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Construtor do Controller
        /// </summary>
        public EmpresaInstituicaoViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Manipulação de Objetos e Eventos do Controller
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            if (View is DetailView && Application is DevExpress.ExpressApp.Win.WinApplication) 
            {       // LookUp de País
                    WinLookUpEdit = ((LookupPropertyEditor)((DetailView)View).FindItem("Pais")).Control;
                    WinLookUpEdit.EditValueChanged += Control_EditValueChanged;
                    // TextEdit com a Máscara
                    WinTextEditWMask = ((StringPropertyEditor)((DetailView)View).FindItem("TxFoneFax")).Control as StringEdit;
            }
            base.OnViewControlsCreated();
        }

        /// <summary>
        /// Empilhamento de eventos de inicialização
        /// </summary>
        protected override void OnActivated()
        {
            if (View is DetailView && !(Application is DevExpress.ExpressApp.Win.WinApplication))
            {
                WebLookUpEdit = (ASPxLookupPropertyEditor)((DetailView)View).FindItem("Pais");
                WebTextEditWMask = (ASPxStringPropertyEditor)((DetailView)View).FindItem("TxFoneFax");

                WebLookUpEdit.ControlCreated += Control_EditValueChanged;
                WebLookUpEdit.ControlValueChanged += Control_EditValueChanged;
            }

            base.OnActivated();
        }

        /// <summary>
        /// Desaloca Objetos e Desempilha Eventos
        /// </summary>
        protected override void OnDeactivated()
        {
            if (View is DetailView)
            {
                if (Application is DevExpress.ExpressApp.Win.WinApplication)
                {
                    if (WinLookUpEdit != null)
                    {
                        WinLookUpEdit.EditValueChanged -= Control_EditValueChanged;
                    }
                    else if (WinTextEdit != null)
                    {
                        WinTextEdit.Leave -= Control_EditValueChanged;
                    }
                }
                else
                {
                    if (WebLookUpEdit != null)
                    {
                        WebLookUpEdit.ControlCreated -= Control_EditValueChanged;
                        WebLookUpEdit.ControlValueChanged -= Control_EditValueChanged;
                    }
                    else if (WinTextEdit != null)
                    {
                        WebTextEdit.ControlCreated -= Control_EditValueChanged;
                        WebTextEdit.ControlValueChanged -= Control_EditValueChanged;
                    }
                }
            }

            base.OnDeactivated();
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento disparado quando o LookUpEdit de País muda o valor
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void Control_EditValueChanged(object sender, EventArgs e)
        {
            
            if (Application is DevExpress.ExpressApp.Win.WinApplication)
                Pais = WinLookUpEdit.EditValue as Pais;
            else
                Pais = WebLookUpEdit.ControlValue == null ? WebLookUpEdit.PropertyValue as Pais : WebLookUpEdit.ControlValue as Pais;
            
            if (Pais != null && !string.IsNullOrEmpty(Pais.TxMascara))
                if (Application is DevExpress.ExpressApp.Win.WinApplication)
                {
                    if (WinTextEditWMask != null)
                    {
                        WinTextEditWMask.Properties.Mask.MaskType = MaskType.Simple;
                        WinTextEditWMask.Properties.Mask.EditMask = Pais.TxMascara;
                        //WinTextEditWMask.MaskBox.Mask.EditMask = Pais.TxMascara;
                    }
                }
                else
                    if (WebTextEditWMask.Editor != null)
                        ((ASPxTextBox)WebTextEditWMask.Editor).MaskSettings.Mask = Pais.TxMascara;
                    else
                        WebTextEditWMask.EditMask = Pais.TxMascara;
            else
                if (Application is DevExpress.ExpressApp.Win.WinApplication)
                    WinTextEditWMask.Properties.Mask.EditMask = null;
                else
                    WebTextEditWMask.EditMask = null;
            
        }

        #endregion
    }
}