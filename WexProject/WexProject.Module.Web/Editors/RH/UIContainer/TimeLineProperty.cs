using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;

namespace WexProject.Module.Web.Editors.RH.UIContainer
{
    public interface ITimeLineProperty : IModelViewItem
    {
    }
    public class TimeLineProperty : ViewItem
    {

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="objectType">Type</param>
        /// <param name="id">string</param>
        public TimeLineProperty(Type objectType, string id)
            : base(objectType, id)
        {
        }
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="model">ITimeLineProperty</param>
        /// <param name="objectType">Type</param>
        public TimeLineProperty(ITimeLineProperty model, Type objectType)
            : base(objectType, model.Id)
        {
        }
        /// <summary>
        /// Criação do controller
        /// </summary>
        /// <returns>object</returns>
        protected override object CreateControlCore()
        {
            return WebWindow.CurrentRequestPage.LoadControl("TelasForaPadrao/RH/TimeLineWebUserControl.ascx");
        }

    }
}
