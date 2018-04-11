using System;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace WexProject.Module.Web
{
    /// <summary>
    /// Modulo Base
    /// </summary>
    [ToolboxItemFilter("Xaf.Platform.Web")]
    public sealed partial class WexProjectAspNetModule : ModuleBase
    {
        /// <summary>
        /// WexProjectAspNetModule
        /// </summary>
        public WexProjectAspNetModule()
        {
            InitializeComponent();
        }
    }
}
