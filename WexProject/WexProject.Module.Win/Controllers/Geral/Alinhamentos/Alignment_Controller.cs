using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Utils;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Geral;
using WexProject.BLL.Models.NovosNegocios;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;
using System.Drawing;

namespace WexProject.Module.Win.Projeto.Controller_Alignment
{
    /// <summary>
    /// classe Alignment_Controller
    /// </summary>
    public partial class Alignment_Controller : ViewController
    {
        /// <summary>
        /// variavel que guarda a estoria
        /// </summary>
        public Estoria Estoria { get; set; }

        /// <summary>
        /// construtor da classe
        /// </summary>
        public Alignment_Controller()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Quando os controles forem criados
        /// </summary>
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

            GridListEditor listEditor = ((ListView)View).Editor as GridListEditor;
            if (listEditor != null)
            {
                listEditor.GridView.OptionsView.RowAutoHeight = true;
            }
        }

        /// <summary>
        /// Define alinhamento dos textos nas células e tamanho das colunas do grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        private void Alignment_Controller_ViewControlsCreated(object sender, EventArgs e)
        {
            if (!(View is ListView))
                return;

            GridListEditor gle = ((ListView)this.View).Editor as GridListEditor;
            if (gle == null)
                return;
            string id = View.Id.ToString();

            switch (id)
            {
                case "Projeto_ListView":

                    gle.GridView.Columns["NbCicloTotalPlan"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbCicloTotalPlan"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbTamanhoTotal"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbTamanhoTotal"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtInicioReal"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicioReal"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTerminoPlan"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTerminoPlan"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTerminoReal"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTerminoReal"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_NbPerConcluido"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_NbPerConcluido"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "CicloDesenv_ListView":

                    gle.GridView.Columns["NbCiclo"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbCiclo"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTermino"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTermino"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbPontosPlanejados"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbPontosPlanejados"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbPontosRealizados"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbPontosRealizados"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbAlcanceMeta"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbAlcanceMeta"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_TxAlcanceMeta"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_TxAlcanceMeta"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_SituacaoCiclo"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_SituacaoCiclo"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Modulo_ListView":

                    gle.GridView.Columns["TxID"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxID"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbEsforcoPlanejado"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbEsforcoPlanejado"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbPontosTotal"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbPontosTotal"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Estoria_ListView":

                    gle.GridView.Columns["TxID"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxID"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsTipo"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsTipo"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsValorNegocio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsValorNegocio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_NbTamanho"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_NbTamanho"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_TxQuando"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_TxQuando"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsSituacao"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsSituacao"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Requisito_ListView":

                    gle.GridView.Columns["TxID"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxID"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "CasoTeste_ListView":

                    gle.GridView.Columns["TxID"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxID"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "ParteInteressada_ListView":

                    gle.GridView.Columns["TxTelefoneFixo"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxTelefoneFixo"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["TxCelular"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxCelular"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["TxEmail"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxEmail"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Prioridade_ListView":

                    gle.GridView.Columns["NbPrioridade"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbPrioridade"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["TxID"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxID"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsTipo"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsTipo"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsValorNegocio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsValorNegocio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbTamanho"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbTamanho"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Colaborador_PeriodosAquisitivos_ListView":

                    gle.GridView.Columns["DtInicio"].Width = 100;
                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTermino"].Width = 100;
                    gle.GridView.Columns["DtTermino"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTermino"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["NbFeriasPlanejadas"].Width = 105;
                    gle.GridView.Columns["NbFeriasPlanejadas"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["NbFeriasPlanejadas"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_PlanejamentoFerias"].Width = 1120;

                    break;

                case "MotivoCancelamento_ListView":

                    gle.GridView.Columns["TxDescricao"].Width = 1000;

                    gle.GridView.Columns["CsSituacao"].Width = 300;

                    break;


                case "ColaboradorPeriodoAquisitivo_Planejamentos_ListView":

                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].Width = 100;

                    gle.GridView.Columns["_DtRetorno"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_DtRetorno"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_DtRetorno"].Width = 100;

                    gle.GridView.Columns["Vender"].Width = 100;
                    gle.GridView.Columns["Vender"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["Vender"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsSituacao"].Width = 100;
                    gle.GridView.Columns["CsSituacao"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsSituacao"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["Realizadas"].Width = 100;
                    gle.GridView.Columns["Realizadas"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["Realizadas"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns[0].Width = 100;
                    gle.GridView.Columns[0].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns[0].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["TxAtualizado"].Width = 455;
                    gle.GridView.Columns["TxAtualizado"].AppearanceCell.BackColor = Color.LightCyan;

                    gle.GridView.Columns["TxPlanejado"].Width = 453;
                    gle.GridView.Columns["TxPlanejado"].AppearanceCell.BackColor = Color.LightCyan;

                    break;

                case "Colaborador_Afastamentos_ListView":

                    gle.GridView.Columns["DtInicio"].Width = 90;
                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTermino"].Width = 90;
                    gle.GridView.Columns["DtTermino"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTermino"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;


                    gle.GridView.Columns[4].Width = 80;
                    gle.GridView.Columns[4].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns[4].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;


                    gle.GridView.Columns[2].Width = 420;

                    gle.GridView.Columns[3].Width = 400;

                    break;

                case "ColaboradorAfastamento_LookupView":

                    gle.GridView.Columns["DtInicio"].Width = 100;
                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["DtTermino"].Width = 100;
                    gle.GridView.Columns["DtTermino"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtTermino"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;


                    gle.GridView.Columns["TipoAfastamento"].Width = 400;

                    gle.GridView.Columns["TxObservacao"].Width = 400;

                    break;

                case "Colaborador_LookupView":

                    gle.GridView.Columns["UserName"].Width = 200;
                    gle.GridView.Columns["UserName"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["UserName"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    break;

                case "Colaborador_ListView":

                    gle.GridView.Columns["_NomeCompleto"].Width = 470;

                    gle.GridView.Columns[9].Width = 100;
                    gle.GridView.Columns[9].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns[9].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns[10].Width = 230;

                    gle.GridView.Columns["TxMatricula"].Width = 100;
                    gle.GridView.Columns["TxMatricula"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["TxMatricula"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns[2].Width = 200;

                    gle.GridView.Columns["_PlanoFeriasAtual"].Width = 550;

                    break;


                case "FeriasPlanejamento_ListView":

                    gle.GridView.Columns["DtInicio"].Width = 140;
                    gle.GridView.Columns["DtInicio"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["DtInicio"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["_DtRetorno"].Width = 100;
                    gle.GridView.Columns["_DtRetorno"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["_DtRetorno"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["Vender"].Width = 100;
                    gle.GridView.Columns["Vender"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["Vender"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns[1].Width = 100;
                    gle.GridView.Columns[1].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns[1].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["Realizadas"].Width = 120;
                    gle.GridView.Columns["Realizadas"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["Realizadas"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["CsSituacao"].Width = 100;

                    gle.GridView.Columns["CsSituacao"].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                    gle.GridView.Columns["CsSituacao"].AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;

                    gle.GridView.Columns["TxAtualizado"].Width = 395;

                    gle.GridView.Columns["TxPlanejado"].Width = 395;

                    break;

                case "TipoAfastamento_ListView":

                    gle.GridView.Columns["TxDescricao"].Width = 600;

                    gle.GridView.Columns["_IsParaFeriasRealizadas"].Width = 110;

                    gle.GridView.Columns["_IsRemunerado"].Width = 110;

                    gle.GridView.Columns["CsSituacao"].Width = 130;

                    break;


                /* case "SolicitacaoOrcamento_ListView":
                                                                                    ListView lv = (ListView)this.View;
                                                                                    ProxyCollection x = lv.CollectionSource.Collection as ProxyCollection;*/
                
            }
        }
    }
}
