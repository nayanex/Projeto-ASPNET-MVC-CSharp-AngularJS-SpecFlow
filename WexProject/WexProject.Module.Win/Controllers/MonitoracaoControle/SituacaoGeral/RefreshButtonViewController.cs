using DevExpress.ExpressApp.SystemModule;
using WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral;

namespace WexProject.Module.Win.Projeto.Controllers.Dashboard
{
    public partial class RefreshButtonViewController : RefreshController
    {
        public RefreshButtonViewController()
        {
            InitializeComponent();
            RegisterActions(components);
        }

        protected override void OnActivated()   
        {
            base.OnActivated();
        }

        protected override void Refresh()
        {
            MontaGraficos();
            base.Refresh();
        }

        static void MontaGraficos()
        {
            ChartEstimadoRealizado.MontaSeries();
            ChartRitimoTime.MontaSeries();
            ChartEscopoCompletude.MontaSeries();
        }
    }
}
