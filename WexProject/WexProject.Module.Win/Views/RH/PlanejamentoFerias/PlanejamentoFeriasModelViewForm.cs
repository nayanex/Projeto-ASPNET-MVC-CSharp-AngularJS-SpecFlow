using System;
using System.Collections.Generic;
using DevExpress.Xpo;
using WexProject.BLL.Models.Rh;
using DevExpress.XtraScheduler;
using DevExpress.Data.Filtering;
using WexProject.BLL.Shared.Domains.Rh;
using DevExpress.ExpressApp;
using DevExpress.Xpo.DB;
using System.Text;
using WexProject.Library.Libs.Enumerator;
using WexProject.BLL.Shared.Domains.Geral;

namespace WexProject.Module.Win.TelasForaPadrao.RH.PlanejamentoFerias.UserControls
{
    public partial class PlanejamentoFeriasModelViewForm : DevExpress.XtraEditors.XtraForm
    {
        #region Atributtes
        private Session Session { get; set; }

        private Appointment Appointment { get; set; }

        public ColaboradorPeriodoAquisitivo PeriodoAquisitivo { get; set; }

        private Colaborador Colaborador { get; set; }

        private FeriasPlanejamento Planejamento { get; set; }
        #endregion
        
        #region Constructor
        /// <summary>
        /// Construtor do modal 
        /// </summary>
        /// <param name="session">Sess�o</param>
        /// <param name="apt">appointment com os valores do registro selecionado</param>
        /// <param name="feirasPlanejamentos">Cole��o com as ferias planejadas</param>
        public PlanejamentoFeriasModelViewForm(Session session, Appointment apt)
        {
            InitializeComponent();
            Session = session;
            Appointment = apt;

            vendaFeriascomboBoxEdit.Enabled = false;
            vendaFeriascomboBoxEdit.EditValue = null;

            PopulateComponents();
        }
        #endregion
        
        #region Populates
        /// <summary>
        /// M�todo que popula o modal com os valores do registro selecionado
        /// </summary>
        private void PopulateComponents()
        {
            vendaFeriascomboBoxEdit.Properties.Items.Add(EnumUtil.DescricaoEnum(CsSimNao.Sim));
            vendaFeriascomboBoxEdit.Properties.Items.Add(EnumUtil.DescricaoEnum(CsSimNao.N�o));

            if (!(string.IsNullOrWhiteSpace(Appointment.Description)))
            {
                string[] values = Appointment.Description.Split(',');

                this.Text = String.Format("Planejamento de ferias - {0}", values[10]);

                //popula os campos do area de periodo aquisitivo=
                lookUpEditColaborador.EditValue = Session.FindObject<Colaborador>(CriteriaOperator.Parse("Oid = ?",values[0]));
               // lookUpEditColaborador.Text = lookUpEditColaborador.EditValue.ToString();
                lookUpEditPeriodo.EditValue = Session.FindObject<ColaboradorPeriodoAquisitivo>(CriteriaOperator.Parse("Oid = ?", values[1]));
                spinEditPeriodoAquisitivoFeriasPlanejadas.EditValue = values[2];

                //Popula a area de planejamento de ferias
                dateEditPLFeriasInicio.EditValue = Appointment.Start;
                textBoxDtTermino.Text = Appointment.End.ToShortDateString();
                
                //insere a modalidade do registro selecionado
                lookUpEditModalidade.EditValue = Session.FindObject<ModalidadeFerias>(CriteriaOperator.Parse("Oid = ?", values[3]));

                vendaFeriascomboBoxEdit.Enabled = true;

                if (values[7].Equals(CsSimNao.Sim))
                {
                    vendaFeriascomboBoxEdit.EditValue = EnumUtil.DescricaoEnum(CsSimNao.Sim);
                }
                else
                {
                    vendaFeriascomboBoxEdit.EditValue = EnumUtil.DescricaoEnum(CsSimNao.N�o);
                }

                textEditAtualizadoPor.EditValue = values[5];
                textEditPlanejadoPor.EditValue = values[4];

                if (values[6].Equals("EmAtraso"))
                {
                    //insere o valor da situa��o do item selecionado se a situa��o for 'em atraso'
                    comboBoxEdit1.EditValue = EnumUtil.DescricaoEnum(CsSituacaoFerias.EmAtraso);
                }
                else
                {
                    //insere o valor da situa��o do item selecionado
                    comboBoxEdit1.EditValue = values[6];
                }
            }

            if (!(string.IsNullOrWhiteSpace(Appointment.Description)))
            {
                lookUpEditColaborador.Properties.ReadOnly = true;
                lookUpEditPeriodo.Properties.ReadOnly = true;
            }
            else
            {
                //insere as modalidades no lookup de modalidades
                lookUpEditColaborador.EditValue = Colaborador.GetColaboradorCurrent(Session);
                lookUpEditPeriodo.EditValue = Colaborador.GetColaboradorCurrent(Session).PeriodosAquisitivos[Colaborador.GetColaboradorCurrent(Session).PeriodosAquisitivos.Count - 1];

                textEditAtualizadoPor.Visible = false;
                textEditPlanejadoPor.Visible = false;
                labelPlanejadoPor.Visible = false;
                labelControlAtualizadoPor.Visible = false;
            }

                //Pega todas as modalidades de ferias que est�o ativas
                XPCollection<ModalidadeFerias> modalidades = new XPCollection<ModalidadeFerias>(Session, CriteriaOperator.Parse("CsSituacao = 'Ativo'"));
                //insere as modalidades no lookup de modalidades
                if (modalidades.Count > 0)
                {
                    lookUpEditModalidade.Properties.DataSource = modalidades;
                }
                else
                {
                    EnableComponents(false);
                    throw new UserFriendlyException("Voc� deve primeiro cadastrar uma modalidade de f�rias.");
                }
                //insere os valores no comboBox de situa��o das ferias
                comboBoxEdit1.Properties.Items.Add(CsSituacaoFeriasPlanejamento.Planejado);
                comboBoxEdit1.Properties.Items.Add(CsSituacaoFeriasPlanejamento.Realizado);

                spinEditPeriodoAquisitivoFeriasPlanejadas.Properties.ReadOnly = true;

                //muda os campos de planejamento de ferias para ReadOnly                
                textEditAtualizadoPor.Properties.ReadOnly = true;
                textEditPlanejadoPor.Properties.ReadOnly = true;
        }
        #endregion

        #region Util
        /// <summary>
        /// Metodo que edita e atualiza o registro de ferias planejamento
        /// </summary>
        private void EditValues()
        {
            string[] values = Appointment.Description.Split(',');

            string ferias_oid = values.Length > 1 ?  values[9] : "00000000-0000-0000-0000-000000000000" ;

            if (new Guid(ferias_oid) != Guid.Empty)
            {
                Planejamento = Session.FindObject<FeriasPlanejamento>(CriteriaOperator.Parse("Oid=?",new Guid(ferias_oid)));
            }
            else
            {
                Planejamento = new FeriasPlanejamento(Session);
            }
            
            if (Colaborador != null)
            {
                    Planejamento.Periodo = lookUpEditPeriodo.EditValue as ColaboradorPeriodoAquisitivo;
            }

            if (values.Length > 1)
            {
                Planejamento.DtInicio = (DateTime)dateEditPLFeriasInicio.EditValue;

                Planejamento.Modalidade = lookUpEditModalidade.EditValue as ModalidadeFerias;

                if (comboBoxEdit1.EditValue.ToString().Equals("Planejado"))
                {
                    Planejamento.CsSituacaoFerias = CsSituacaoFeriasPlanejamento.Planejado;
                }
                else
                {
                    Planejamento.CsSituacaoFerias = CsSituacaoFeriasPlanejamento.Realizado;
                }

                Planejamento.Vender = vendaFeriascomboBoxEdit.EditValue.Equals("Sim") ? CsSimNao.Sim : CsSimNao.N�o;

            }
            else
            {
                if (Colaborador == null)
                {
                    throw new UserFriendlyException("O preenchimento do campo Colaborador � obrigat�rio.");
                }
                
                Planejamento.Modalidade = lookUpEditModalidade.EditValue as ModalidadeFerias;

                if (Planejamento.Periodo != null)
                {
                    if (Planejamento.Modalidade != null)
                    {
                        Planejamento.Periodo.NbFeriasPlanejadas = Planejamento.Modalidade.NbDias;
                    }
                }
                if (dateEditPLFeriasInicio.EditValue != null)
                {
                    Planejamento.DtInicio = (DateTime)dateEditPLFeriasInicio.EditValue;
                }

                if (comboBoxEdit1.EditValue != null)
                {
                    if (comboBoxEdit1.EditValue.ToString().Equals("Planejado"))
                    {
                        Planejamento.CsSituacaoFerias = CsSituacaoFeriasPlanejamento.Planejado;
                    }
                    else
                    {
                        Planejamento.CsSituacaoFerias = CsSituacaoFeriasPlanejamento.Realizado;
                    }
                }

                if (textEditAtualizadoPor.EditValue != null)
                {
                    Planejamento.TxPlanejado = textEditAtualizadoPor.EditValue.ToString();
                }

                if (textEditPlanejadoPor.EditValue != null)
                {
                    Planejamento.TxAtualizado = textEditPlanejadoPor.EditValue.ToString();
                }
            }

            if (vendaFeriascomboBoxEdit.EditValue != null &&
                vendaFeriascomboBoxEdit.EditValue.ToString().Equals(CsSimNao.Sim))
            {
                Planejamento.Vender = CsSimNao.Sim;
        }
            else
            {
                Planejamento.Vender = CsSimNao.N�o;
            }
        }
        /// <summary>
        /// M�todo que verifica se as regras est�o validas
        /// </summary>
        private void ValidateFields()
        {

            StringBuilder builder = new StringBuilder();

            if (!(Planejamento._ValidacaoDtInicio))
            {
                builder.Append("- A Data de In�cio do Planejamento n�o pode ser menor que a Data atual e nem da Data de Admiss�o do Colaborador.").Append("\n");
            }
            if (!(Planejamento._ValidacaoPeriodoFeriasConflitantes))
            {
                builder.Append("- O per�odo do Planejamento de F�rias n�o pode conflitar com nenhum outro j� existente para o Per�odo Aquisitivo.").Append("\n");
            }
            if (!(Planejamento._ValidacaoPeriodoFerias))
            {
                builder.Append("- O Planejamento n�o pode ultrapassar o limite m�ximo para tirar F�rias definido na Configura��o e nem pode ser menor que a Data de In�cio do Per�odo Aquisitivo.").Append("\n");
            }
            if (!(Planejamento._ValidacaoVendaFerias))
            {
                builder.Append("- A quantidade de dias a serem Vendidos n�o pode ser maior que o definido na Configura��o.").Append("\n");
            }
            if (Planejamento.Periodo.DtInicio == null)
            {
                builder.Append("- O preenchimento do campo In�cio do per�odo aquisitivo � obrigat�rio.").Append("\n");
            }
            if (Planejamento.Periodo.DtTermino == null)
            {
                builder.Append("- O preenchimento do campo T�rmino do per�odo aquisitivo � obrigat�rio.").Append("\n");
            }
            if (Planejamento.DtInicio == null)
            {
                builder.Append("- O preenchimento do campo In�cio do planejamento de ferias � obrigat�rio.").Append("\n");
            }
            if (Planejamento.Modalidade == null)
            {
                builder.Append("- O preenchimento do campo Modalidade do planejamento de ferias � obrigat�rio.").Append("\n");
            }
            
            if (!string.IsNullOrWhiteSpace(builder.ToString()))
            {
                builder = new StringBuilder("Ocorreram problemas de valida��o. Por favor leia os erros abaixo para tentar resolv�-los.\n\n").Append(builder.ToString());
                throw new UserFriendlyException(builder.ToString());
            }
            else
            {
                Planejamento.Save();
                Session.CommitTransaction();
            }
        }
        /// <summary>
        /// Seta a Data de T�rmino da F�rias
        /// </summary>
        private void SetDtTermino() 
        {
            if (Planejamento != null)
            {                
                textBoxDtTermino.Text = Planejamento._DtRetorno.ToShortDateString();
            }
            else
            {
                ModalidadeFerias modalidade = lookUpEditModalidade.EditValue as ModalidadeFerias;
                if (modalidade != null && dateEditPLFeriasInicio.EditValue != null)
                {
                    textBoxDtTermino.Text = dateEditPLFeriasInicio.DateTime.AddDays(modalidade.NbDias - 1).Date.ToShortDateString();
                }
            }
        }
        /// <summary>
        /// habilita e desabilita alguns componentes do form
        /// </summary>
        /// <param name="enable">booleano de ativa��o dos componentes</param>
        private void EnableComponents(bool enable)
        {
            dateEditPLFeriasInicio.Enabled = enable;
            lookUpEditModalidade.Enabled = enable;            
            comboBoxEdit1.Enabled = enable;
            textEditPlanejadoPor.Enabled = enable;
            textEditAtualizadoPor.Enabled = enable;
            buttonOk.Enabled = enable;
        }
        #endregion

        #region Eventos
        /// <summary>
        /// Evento chamado quando se clica no bot�o cancelar
        /// </summary>
        /// <param name="sender">objeto button</param>
        /// <param name="e">evento</param>
        private void buttonCancelar_Click(object sender, EventArgs e)
        {            
            Dispose(); 
        }
        /// <summary>
        /// Evento chamado quando se clica no bot�o ok
        /// </summary>
        /// <param name="sender">objeto button</param>
        /// <param name="e">evento</param>
        private void buttonOk_Click(object sender, EventArgs e)
        {
            EditValues();

            ValidateFields();
            
            DialogResult = System.Windows.Forms.DialogResult.OK;

            Dispose(); 
        }
        /// <summary>
        /// Evento chamado quando modifica uma modalidade mo lookup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (Planejamento != null)
            {
                spinEditPeriodoAquisitivoFeriasPlanejadas.EditValue = Planejamento.Periodo._PlanejamentoFerias;
                spinEditPeriodoAquisitivoFeriasPlanejadas.Properties.ReadOnly = true;                
            }

            if (lookUpEditModalidade.EditValue == null)
            {
                vendaFeriascomboBoxEdit.Enabled = false;
                vendaFeriascomboBoxEdit.EditValue = null;

                return;
            }

            if(((ModalidadeFerias)lookUpEditModalidade.EditValue).PodeVender)
            {
                vendaFeriascomboBoxEdit.Enabled = true;
                vendaFeriascomboBoxEdit.EditValue = EnumUtil.DescricaoEnum(CsSimNao.Sim);
            }
            else
            {
                vendaFeriascomboBoxEdit.Enabled = false;
                vendaFeriascomboBoxEdit.EditValue = EnumUtil.DescricaoEnum(CsSimNao.N�o);
            }

            SetDtTermino();
            this.Refresh();
        }
        /// <summary>
        /// Evento chamado quando se edita um colaborador no lookupEdit de colaborador
        /// </summary>
        /// <param name="sender">Objeto selecionado(LookUpEdit)</param>
        /// <param name="e">evento</param>
        private void lookUpEditColaborador_EditValueChanged(object sender, EventArgs e)
        {
            Colaborador = lookUpEditColaborador.EditValue as Colaborador;

            if (Colaborador != null)
            {
                XPCollection<Colaborador> colaboradores = new XPCollection<Colaborador>(Session);

                colaboradores.Sorting.Add(new SortProperty("Usuario.FullName", SortingDirection.Ascending));

                lookUpEditColaborador.Properties.DataSource = colaboradores;
                lookUpEditColaborador.EditValue = Colaborador;

                lookUpEditPeriodo.Properties.DataSource = null;

                //popula o lookup de periodos aquisitivos
                if (Colaborador.PeriodosAquisitivos.Count > 0)
                {

                    EnableComponents(true);

                    lookUpEditPeriodo.Properties.DataSource = Colaborador.PeriodosAquisitivos;

                    Session.FindObject<Colaborador>(CriteriaOperator.Parse("Oid = ?", Colaborador.Oid)).PeriodosAquisitivos.Sorting.Add(new SortProperty("DtInicio", SortingDirection.Ascending));

                    FeriasPlanejamento dadosFerias = Session.FindObject<FeriasPlanejamento>(CriteriaOperator.Parse(String.Format("Periodo.Colaborador = '{0}'", Colaborador.Oid)));

                    if (dadosFerias != null)
                    {
                        spinEditPeriodoAquisitivoFeriasPlanejadas.EditValue = dadosFerias.Modalidade.NbDias;
                        textEditAtualizadoPor.EditValue = String.Format("{0} - {1}", Colaborador.GetColaboradorCurrent(Session), DateTime.Now);
                        textEditPlanejadoPor.EditValue = String.Format("{0} - {1}", Colaborador.GetColaboradorCurrent(Session), DateTime.Now);
                    }
                    else
                    {
                        spinEditPeriodoAquisitivoFeriasPlanejadas.EditValue = Colaborador.PeriodosAquisitivos[Colaborador.PeriodosAquisitivos.Count - 1]._PlanejamentoFerias;
                        textEditAtualizadoPor.EditValue = String.Format("{0} - {1}", Colaborador.GetColaboradorCurrent(Session), DateTime.Now);
                        textEditPlanejadoPor.EditValue = String.Format("{0} - {1}", Colaborador.GetColaboradorCurrent(Session), DateTime.Now);
                    }
                }
                else
                {
                    EnableComponents(false);
                    throw new UserFriendlyException("O Colaborador n�o possui per�odos aquisitivos. Por favor encaminhe - se ao cadastro de colaborador para cadastrar um per�odo aquisitivo.");
                }
            }
        }
        /// <summary>
        /// Evento para se houver mudan�a na data inicial do Planejamento de F�rias
        /// recalcular a data de t�rmino
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void dateEditPLFeriasInicio_EditValueChanged(object sender, EventArgs e)
        {
            SetDtTermino();
        }
        #endregion

    }
}
