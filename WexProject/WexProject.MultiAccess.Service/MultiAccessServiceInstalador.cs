using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace WexProject.MultiAccess.Service
{
    [RunInstaller( true )]
    public partial class MultiAccessServiceInstalador : System.Configuration.Install.Installer
    {
        public MultiAccessServiceInstalador()
        {
            InitializeComponent();
        }
    }
}
