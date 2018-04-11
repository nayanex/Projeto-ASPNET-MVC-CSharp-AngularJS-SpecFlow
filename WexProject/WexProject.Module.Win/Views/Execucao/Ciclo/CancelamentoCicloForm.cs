using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using WexProject.BLL.Models.Execucao;
using DevExpress.XtraEditors;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Shared.Domains.Execucao;

namespace WexProject.Module.Win.TelasForaPadrao.Projeto.Ciclo
{
    /// <summary>
    /// Form de Cancelamento de Ciclo
    /// </summary>
    public partial class CancelamentoCicloForm : XtraForm
    {
        #region Properties
        
        /// <summary>
        /// Sessão Ataul
        /// </summary>
        private Session Session { get; set; }

        /// <summary>
        /// Ciclo de Desenvolvimento
        /// </summary>
        private CicloDesenv Ciclo { get; set; }

        /// <summary>
        /// Indica se o form foi cancelado
        /// </summary>
        public bool IsCancel
        {
            get;
            set;
        }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <param name="ciclo">Ciclo de Desenvolvimento</param>
        public CancelamentoCicloForm(CicloDesenv ciclo)
        {
            Session = ciclo.Session;
            Ciclo = ciclo;
            IsCancel = true;

            InitializeComponent();

            PopulateItems();
        }   


        #endregion

        #region Métodos

        /// <summary>
        /// Método que popula a lista de motivos e delimita o aparecer ou não do 'inicio do próximo ciclo'
        /// </summary>
        private void PopulateItems()
        {
            XPCollection<MotivoCancelamento> motivos = MotivoCancelamento.GetMotivosAtivos(Session);

            lookUpEditMotivo.Properties.DataSource = motivos;

            if (Ciclo.Projeto.UltimoFiltro != null) 
            {
                if (Ciclo.Projeto.UltimoFiltro.MotivoCancelamentoCiclo != null) 
                {
                    lookUpEditMotivo.EditValue = Ciclo.Projeto.UltimoFiltro.MotivoCancelamentoCiclo;
                }
            }

            if (Ciclo.CriarListasItensPendentes(false))
                ListaItensPendentesCicloUserControl.Enabled = true;
            else
                ListaItensPendentesCicloUserControl.Enabled = false;

            ListaItensPendentesCicloUserControl.LstPrioridade.DataSource = Ciclo._ListaPrioridades;
            ListaItensPendentesCicloUserControl.LstProximoCiclo.DataSource = Ciclo._ListaProximoCiclo;
            ListaItensPendentesCicloUserControl.Ciclo = Ciclo;

            if (Ciclo.RnMostrarInicioProximoCiclo())
            {
                DtInicioProxCiclo.Enabled = true;
                DtInicioProxCiclo.EditValue = Ciclo._DataProximoCiclo;
            }
        }
    
        #endregion

        #region Eventos

        /// <summary>
        /// Salvamento do Cancelamento do Ciclo
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (lookUpEditMotivo.EditValue != null)
            {
                ProjetoUltimoFiltro.RnSetUltimoMotivoCancelamento(Session, Ciclo.Projeto,
                    (MotivoCancelamento)lookUpEditMotivo.EditValue);
            }
            else
            {
                XtraMessageBox.Show(CicloDesenv.RnValidarMotivoCancelamento(null), "Erro",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error,
                    System.Windows.Forms.MessageBoxDefaultButton.Button1);

                return;
            }

            DateTime data = DateTime.MinValue;
            string erro;

            if (DtInicioProxCiclo.Visible && DtInicioProxCiclo.Enabled)
            {
                data = DtInicioProxCiclo.DateTime;
                erro = Ciclo.RnDataProximoCiclo(data);

                if (!string.IsNullOrEmpty(erro))
                {
                    XtraMessageBox.Show(erro, "Erro",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error,
                        System.Windows.Forms.MessageBoxDefaultButton.Button1);

                    return;
                }
            }
            Ciclo.CsSituacaoCiclo = CsSituacaoCicloDomain.Cancelado;
            IsCancel = false; // Indica que não foi cancelado
            Ciclo.RnCancelarCiclo(lookUpEditMotivo.EditValue as MotivoCancelamento, data);
            Close();
        }
        
        #endregion
    }
}