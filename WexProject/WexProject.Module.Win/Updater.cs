using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp;

namespace WexProject.Module.Win
{
    /// <summary>
    /// classe de update
    /// </summary>
    public class Updater : ModuleUpdater
    {
        /// <summary>
        /// contrutor da classe
        /// </summary>
        /// <param name="objectSpace">objectSpace</param>
        /// <param name="currentDBVersion">currentDBVersion</param>
        public Updater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion)
        {
        }
        /// <summary>
        /// Atualizar esquema
        /// </summary>
        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
