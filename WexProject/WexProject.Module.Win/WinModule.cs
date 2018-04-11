using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using DevExpress.ExpressApp;

namespace WexProject.Module.Win
{
    /// <summary>
    /// Criação da classe
    /// </summary>
    [ToolboxItemFilter("Xaf.Platform.Win")]
    public sealed partial class WexProjectWindowsFormsModule : ModuleBase
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public WexProjectWindowsFormsModule()
        {
            InitializeComponent();
        }
    }
}
