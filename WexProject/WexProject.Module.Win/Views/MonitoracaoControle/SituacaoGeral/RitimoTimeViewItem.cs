using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;

namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    /// <summary>
    /// Interface para utilizar uma view fora do padrão do XAF
    /// </summary>
    public interface IWinUIContainerRitimoTimeViewItem : IModelDashboardViewItem
    {
    }

    [ViewItemAttribute(typeof(IWinUIContainerRitimoTimeViewItem))]
    public class RitimoTimeViewItem : DashboardViewItem
    {

        public RitimoTimeViewItem(IModelDashboardViewItem model, Type objectType)
            : base(model, objectType)
        {

        }

        /// <summary>
        /// Método criador de view fora do padrão do XAF
        /// </summary>
        /// <returns>Uma instância da view</returns>
        protected override object CreateControlCore()
        {
            return new ChartRitimoTime();
        }

    }
}
