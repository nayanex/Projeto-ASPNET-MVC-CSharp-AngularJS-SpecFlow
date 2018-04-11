using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using WexProject.BLL.Models.Planejamento;
using DevExpress.Web.ASPxEditors;
using WexProject.BLL.Models.Geral;

namespace WexProject.Module.Lib.ControllerPraWinWeb.Planejamento
{
    public partial class PaisViewController : ViewController
    {
        #region Properties
        
        /// <summary>
        /// Text Edit da Máscara (WIN)
        /// </summary>
        public TextEdit WinTextEdit { get; set; }
        /// <summary>
        /// Text Edit com a Máscara (WIN)
        /// </summary>
        public StringEdit WinTextEditWMask { get; set; }
        /// <summary>
        /// Text Edit da Máscara (WEB)
        /// </summary>
        public ASPxStringPropertyEditor WebTextEdit { get; set; }
        /// <summary>
        /// Edit Text (WEB)
        /// </summary>
        public ASPxStringPropertyEditor WebTextEditWMask { get; set; }
        #endregion

        #region Constructor
        public PaisViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }
        #endregion

        #region Override

        protected override void OnActivated()
        {
            if (!(Application is DevExpress.ExpressApp.Win.WinApplication))
            {
                if (View is DetailView)
                {
                    // TextEdit da Máscara
                    WebTextEdit = (ASPxStringPropertyEditor)((DetailView)View).FindItem("TxMascara");
                    WebTextEdit.ControlCreated += Control_EditValueChanged;
                    WebTextEdit.ControlValueChanged += Control_EditValueChanged;
                    // TextEdit com a Máscara
                    WebTextEditWMask = (ASPxStringPropertyEditor)((DetailView)View).FindItem("_TxTesteMascara");

                    // Objeto de País                        
                    WebTextEditWMask.EditMask = ((Pais)View.CurrentObject).TxMascara;
                }
            }
            base.OnActivated();
        }

        protected override void OnViewControlsCreated()
        {
            if (Application is DevExpress.ExpressApp.Win.WinApplication)
            {                
                if (View is DetailView)
                {
                    // TextEdit da Máscara
                    WinTextEdit = ((StringPropertyEditor)((DetailView)View).FindItem("TxMascara")).Control;
                    WinTextEdit.Leave += Control_EditValueChanged;

                    // TextEdit com a Máscara
                    WinTextEditWMask = ((StringPropertyEditor)((DetailView)View).FindItem("_TxTesteMascara")).Control as StringEdit;
                    WinTextEditWMask.BackColor = System.Drawing.Color.Yellow;

                    // Objeto de País 
                    WinTextEditWMask.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Simple;   
                    WinTextEditWMask.Properties.Mask.EditMask = ((Pais)View.CurrentObject).TxMascara;
                }
            }
            base.OnViewControlsCreated();
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
            string _txMascara = string.Empty;
            if (Application is DevExpress.ExpressApp.Win.WinApplication) 
                _txMascara = WinTextEdit.Text;
            else
                _txMascara = WebTextEdit.ControlValue == null ? WebTextEdit.PropertyValue.ToString() : WebTextEdit.ControlValue.ToString();
            _txMascara = string.IsNullOrWhiteSpace(_txMascara) ? "55" : _txMascara;
            if (Application is DevExpress.ExpressApp.Win.WinApplication)                 
                    WinTextEditWMask.Properties.Mask.EditMask = _txMascara;
            else
                if (WebTextEditWMask.Editor != null)
                    ((ASPxTextBox)WebTextEditWMask.Editor).MaskSettings.Mask = _txMascara;
                else
                    WebTextEditWMask.EditMask = _txMascara;
            
        }
        #endregion

    }
}
