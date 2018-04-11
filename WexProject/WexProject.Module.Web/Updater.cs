using System;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace WexProject.Module.Web
{
    /// <summary>
    /// Updater do Model
    /// </summary>
    public class Updater : ModuleUpdater
    {
        /// <summary>
        /// Construtor do Model Updater
        /// </summary>
        /// <param name="objectSpace">objectSpace</param>
        /// <param name="currentDBVersion">currentDBVersion</param>
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion)
        {
        }
        /// <summary>
        /// Constructor do Model
        /// </summary>
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
