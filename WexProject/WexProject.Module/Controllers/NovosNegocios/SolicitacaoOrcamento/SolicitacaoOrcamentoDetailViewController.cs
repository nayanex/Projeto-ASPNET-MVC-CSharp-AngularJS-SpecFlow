using System;
using System.Collections.Generic;
using DevExpress.ExpressApp;
using WexProject.BLL.Models.NovosNegocios;

namespace WexProject.Module.Lib.ControllerPraWinWeb.NovosNegocios
{
    public partial class SolicitacaoOrcamentoDetailViewController : ViewController
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public SolicitacaoOrcamentoDetailViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        /// <summary>
        /// Quando o controller for ativado
        /// </summary>
        protected override void OnActivated()
        {
            LimparComentarioSolicitacaoOrcamento();

            base.OnActivated();
        }

        /// <summary>
        /// Limpeza do comentário da SEOT
        /// </summary>
        private void LimparComentarioSolicitacaoOrcamento()
        {
            SolicitacaoOrcamento seot = (View.CurrentObject as SolicitacaoOrcamento);
            if (!seot.Oid.Equals(new Guid()))
            {
                seot.TxUltimoComentario = string.Empty;
            }
        }
    }
}
