using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web.ASPxGridView;
using WexProject.BLL.Models.NovosNegocios;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.ExpressApp.SystemModule;
using System.ComponentModel;

namespace WexProject.Module.Lib
{
    /// <summary>
    /// Classe
    /// </summary>
    public partial class SolicitacaoOrcamentoViewController : ViewController
    {

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public SolicitacaoOrcamentoViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Quando os controladores da view são criados
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            if (View is ListView) 
            {
                ModifyGridViewSettings();
            }

            base.OnViewControlsCreated();            
        }

        #endregion

        #region Events

        /// <summary>
        /// Evento para colocar cor de background nas células da coluna de situação
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">CustomizeAppearanceEventArgs</param>
        public void Editor_CustomizeAppearance(object sender, DevExpress.ExpressApp.Editors.CustomizeAppearanceEventArgs e)
        {
            if ((e.Name.Equals("Situacao") || e.Name.Equals("_Situacao")) && e.ContextObject is SolicitacaoOrcamento)
            {
                var seot = e.ContextObject as SolicitacaoOrcamento;
                if (Application is DevExpress.ExpressApp.Win.WinApplication)
                {
                    if (seot != null)
                    {
                        ((RowCellStyleEventArgs)e.Item.Data).Appearance.BackColor = seot.Situacao._ClSituacao;
                    }
                }
                else
                {
                    if (e.Item.Data is DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell && seot != null)
                    {
                        ((DevExpress.Web.ASPxGridView.Rendering.GridViewTableDataCell)e.Item.Data).BackColor = seot.Situacao._ClSituacao;
                    }
                }
            }
        }

        #endregion

        #region Utils

        /// <summary>
        /// Método para modificaras colunas do grid view do web e win
        /// </summary>
        private void ModifyGridViewSettings() 
       {
            if (!(Application is DevExpress.ExpressApp.Win.WinApplication))
            {
               ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
               editor.CustomizeAppearance += Editor_CustomizeAppearance;

               if (editor != null)
               {
                   GridViewDataColumn column = editor.Grid.Columns["_Responsavel"] as GridViewDataColumn;
                   if (column != null)
                   {
                       column.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column2 = editor.Grid.Columns["_TipoSolicitacao"] as GridViewDataColumn;
                   if (column2 != null)
                   {
                       column2.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column3 = editor.Grid.Columns["TxCodigo"] as GridViewDataColumn;
                   if (column3 != null)
                   {
                       column3.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column4 = editor.Grid.Columns["TxTitulo"] as GridViewDataColumn;
                   if (column4 != null)
                   {
                       column4.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column5 = editor.Grid.Columns["_Situacao"] as GridViewDataColumn;
                   if (column5 != null)
                   {
                       column5.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column6 = editor.Grid.Columns["_DiasGastos"] as GridViewDataColumn;
                   if (column6 != null)
                   {
                       column6.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column7 = editor.Grid.Columns["DtEmissao"] as GridViewDataColumn;
                   if (column7 != null)
                   {
                       column7.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column8 = editor.Grid.Columns["DtPrazo"] as GridViewDataColumn;
                   if (column8 != null)
                   {
                       column8.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column9 = editor.Grid.Columns["CsPrioridade"] as GridViewDataColumn;
                   if (column9 != null)
                   {
                       column9.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }

                   GridViewDataColumn column10 = editor.Grid.Columns["DtConclusao"] as GridViewDataColumn;
                   if (column10 != null)
                   {
                       column10.Settings.AllowHeaderFilter = DevExpress.Utils.DefaultBoolean.False;
                   }
                }
            }
            else
            {
                GridListEditor gView = ((ListView)View).Editor as GridListEditor;

                // Força a entrar no evento para pintar a célula
                gView.Columns[7].Caption = gView.Columns[0].Caption;
                gView.CustomizeAppearance += Editor_CustomizeAppearance;
            }
        }
        
        #endregion

    }
}
