using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.BLL.Migrations
{

    /// <summary>
    /// Classe utilitaria para executar oo Migrations pelo menos uma vez em que a aplicação for inicializada
    /// </summary>
    public class WexAutomaticMigration
    {

        private static WexAutomaticMigration singleton = null;

        private WexAutomaticMigration () {}

        /// <summary>
        /// Singleton
        /// </summary>
        public static WexAutomaticMigration Instance
        {
            get
            {
                if (WexAutomaticMigration.singleton == null)
                {
                    WexAutomaticMigration.singleton = new WexAutomaticMigration();
                }
                return WexAutomaticMigration.singleton;
            }
        }

        /// <summary>
        /// Flag para garantir que o método ExecuteOnce execute as instruções uma única vez.
        /// </summary>
        private bool ExecuteOnceFlag = false;

        /// <summary>
        /// Executa o Update-Database
        /// </summary>
        public void Execute()
        {
            DbMigrationsConfiguration configuration = new Configuration();
            DbMigrator migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        /// <summary>
        /// Executa o Update-Database uma única vez até o momento em que a aplicação for reinicializada.
        /// </summary>
        public void ExecuteOnce()
        {
            if (!ExecuteOnceFlag)
            {
                ExecuteOnceFlag = true;
                Execute();
            }
        }

    }
}
