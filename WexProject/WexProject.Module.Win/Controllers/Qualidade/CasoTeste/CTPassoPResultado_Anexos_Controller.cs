using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using WexProject.BLL.Models.Qualidade;
using DevExpress.ExpressApp.Utils;

namespace WexProject.Module.Win.Projeto.Controller_Anexos.Anexos_CasoTestePreCondicoes
{
    /// <summary>
    /// classe CasoTestePassoResultadoEsperado_Anexos_Controller
    /// </summary>
    public partial class CTPassoPResultado_Anexos_Controller : ViewController
    {

        /// <summary>
        /// item do singlechoice action para ser adicionado na listagem.
        /// </summary>
        private ChoiceActionItem setItem;

        /// <summary>
        /// objeto partNumber que obtem o item selecionado no grid.
        /// </summary>
        private CasoTestePassoResultadoEsperado resultadoEsperadoAnexo;

        /// <summary>
        /// caso de teste de controle de anexo
        /// </summary>
        public CTPassoPResultado_Anexos_Controller()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// método acionado para habilitar a action.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">evento args</param>
        private void CasoTestePassoResultadoEsperado_Anexos_Controller_Activated(object sender, EventArgs e)
        {
            this.singleChoiceAction1.Items.Clear();
            this.singleChoiceAction1.Active.SetItemValue("ObjectType", View.Id.Equals("CasoTestePasso_ResultadosEsperados_ListView"));
        }

        /// <summary>
        /// método acionado para habilitar a action quando entrar na tela de bomPartnumber.
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View is ListView && View.Id.Equals("CasoTestePasso_ResultadosEsperados_ListView"))
            {
                View.CurrentObjectChanged += new EventHandler(View_CurrentObjectChanged);
            }
        }

        /// <summary>
        /// método acionado para desabilitar a action.
        /// </summary>
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            if (View is ListView && View.Id.Equals("CasoTestePasso_ResultadosEsperados_ListView"))
            {
                View.CurrentObjectChanged -= new EventHandler(View_CurrentObjectChanged);
            }
        }

        /// <summary>
        /// metodo de execução no view
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">argumento de evento</param>
        private void SingleChoiceAction1_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            string tmp = null;
            if (View.Id.Equals("CasoTestePasso_ResultadosEsperados_ListView"))
            {
                tmp = CasoTestePassoResultadoEsperadoAnexo.GetOpenAnexos(this.resultadoEsperadoAnexo, e.SelectedChoiceActionItem.ToString());
            }

            // por fim, abre o anexo.
            System.Diagnostics.Process.Start(tmp);
        }

        /// <summary>
        /// método para manipular a ação ao escolher um item no singlechoice.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">eventos args</param>
        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {

            this.singleChoiceAction1.Items.Clear();
            if (View.Id.Equals("CasoTestePasso_ResultadosEsperados_ListView") && View.CurrentObject != null)
            {
                resultadoEsperadoAnexo = (CasoTestePassoResultadoEsperado)View.CurrentObject;

                foreach (CasoTestePassoResultadoEsperadoAnexo item in resultadoEsperadoAnexo.ResultadosEsperadosAnexos)
                {
                    if (item.TxDescricao != null)
                    {
                        this.setItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(CasoTestePassoResultadoEsperadoAnexo), item.TxDescricao), null);
                        this.singleChoiceAction1.Items.Add(setItem);
                    }
                }
            }
        }
    }
}
