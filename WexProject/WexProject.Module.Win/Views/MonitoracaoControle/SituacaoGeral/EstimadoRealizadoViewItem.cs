using System;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Collections.Generic;

namespace WexProject.Module.Win.Views.MonitoracaoControle.SituacaoGeral
{
    /// <summary>
    /// Interface para utilizar uma view fora do padrão do XAF
    /// </summary>
    public interface IWinUIContainerEstimadoRealizadoViewItem : IModelDashboardViewItem
    {
    }

    /// <summary>
    /// Classe utilizada para exibir uma view fora do padrão do XAF
    /// </summary>
    [ViewItemAttribute(typeof(IWinUIContainerEstimadoRealizadoViewItem))]
    public class WinUIContainerEstimadoRealizadoViewItem : DashboardViewItem
    {

        public WinUIContainerEstimadoRealizadoViewItem(IModelDashboardViewItem model, Type objectType)
            : base(model, objectType)
        {
            
        }

        /// <summary>
        /// Método criador de view fora do padrão do XAF
        /// </summary>
        /// <returns>Uma instância da view</returns>
        protected override object CreateControlCore()
        {
            return new ChartEstimadoRealizado();
        }
    }
}

