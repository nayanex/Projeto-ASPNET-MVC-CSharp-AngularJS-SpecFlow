using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WexProject.Schedule.Library.Views.Forms;

namespace WexProject.Module.Win.Controllers.Planejamento
{
    public class CronogramaController
    {
        /// <summary>
        /// 
        /// </summary>
        public static void FecharFormAplicacaoCronograma()
        {
            if(CronogramaController.VerificarSeFormCronogramaEstaAberto())
            {
                CronogramaView cronograma = Application.OpenForms.OfType<CronogramaView>().FirstOrDefault();
                cronograma.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool VerificarSeFormCronogramaEstaAberto()
        {
            if(Application.OpenForms.OfType<CronogramaView>().Count() > 0)
                return true;

            return false;
        }
    }
}
