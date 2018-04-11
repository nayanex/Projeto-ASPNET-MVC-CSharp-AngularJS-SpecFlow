using System;
using System.ComponentModel;
using System.Collections.Generic;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using WexProject.BLL.Models.Escopo;
using WexProject.BLL.Models.Geral;

namespace WexProject.Module.Win.Projeto.Controllers.Escopo
{
    public partial class EstoriaDetailViewController : ViewController
    {

        private static Estoria estoria;

        private DetailViewController detailViewController;

        public EstoriaDetailViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        private void ViewController1_Activated(object sender, EventArgs e)
        {
            detailViewController = Frame.GetController<DetailViewController>();
            detailViewController.SaveAndNewAction.Executing += SaveAndNewAction_Executing;
            Frame.ViewChanged += Frame_ViewChanged;
        }

        void Frame_ViewChanged(object sender, ViewChangedEventArgs e)
        {
            if (estoria != null && View.CurrentObject != null && View.CurrentObject.GetType() == typeof(Estoria))
            {
                Estoria current = (Estoria)View.CurrentObject;

                if(estoria.ComoUmBeneficiado != null)
                {
                    current.ComoUmBeneficiado = current.Session.GetObjectByKey<Beneficiado>( estoria.ComoUmBeneficiado.Oid );    
                }
                current.ProjetoParteInteressada = current.Session.GetObjectByKey<ProjetoParteInteressada>(estoria.ProjetoParteInteressada.Oid);
                current.Modulo = current.Session.GetObjectByKey<Modulo>(estoria.Modulo.Oid);
                if(estoria.EstoriaPai != null)
                {
                    current.EstoriaPai = current.Session.GetObjectByKey<Estoria>( estoria.EstoriaPai.Oid );    
                }

                estoria = null;
            }
        }

        void SaveAndNewAction_Executing(object sender, CancelEventArgs e)
        {
            if (View.CurrentObject.GetType() == typeof(Estoria))
                estoria = (Estoria)View.CurrentObject;
        }


    }
}
