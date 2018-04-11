using System;
using System.Collections.Generic;
using WexProject.BLL.Models.Execucao;
using DevExpress.XtraEditors;

namespace WexProject.Module.Win.TelasForaPadrao.Execucao
{
    /// <summary>
    /// Formulário de decisão de destino dos itens
    /// </summary>
    public partial class CicloDecisaoEstoriasPendentesForm : XtraForm
    {
        /// <summary>
        /// Objeto de Ciclo
        /// </summary>
        private CicloDesenv ciclo;

        /// <summary>
        /// Indica se foi cancelado o salvamento dos itens
        /// </summary>
        public bool IsCancel
        {
            get;
            set;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="ciclo">Objeto de Ciclo</param>
        public CicloDecisaoEstoriasPendentesForm(CicloDesenv ciclo)
        {
            InitializeComponent();

            this.ciclo = ciclo;
            IsCancel = true;

            if (ciclo != null && ListaItensPendentesCicloUserControl != null)
            {
                // Preenchimento dos DataSources
                ListaItensPendentesCicloUserControl.LstPrioridade.DataSource = ciclo._ListaPrioridades;
                ListaItensPendentesCicloUserControl.LstProximoCiclo.DataSource = ciclo._ListaProximoCiclo;

                ListaItensPendentesCicloUserControl.Ciclo = ciclo;
            }
        }

        /// <summary>
        /// Ao salvar o destino dos itens
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void SalvarBtn_Click(object sender, EventArgs e)
        {
            SaveClick();
        }

        /// <summary>
        /// Clique do botão salvar
        /// </summary>
        private void SaveClick()
        {
            IsCancel = false; // Indica que não foi cancelado

            ciclo.RnSalvarDestinoEstoriasPendentes();
            Close();
        }
    }
}