using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using log4net.Config;

namespace WexProject.MultiAccess.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Configurando Log (log4net) por xml
            XmlConfigurator.Configure();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new MultiAccessService() 
			};
            ServiceBase.Run( ServicesToRun );
        }
    }
}
