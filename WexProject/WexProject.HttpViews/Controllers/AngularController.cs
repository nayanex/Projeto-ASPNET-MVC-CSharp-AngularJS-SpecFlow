using System.Linq;
using System.Web.Mvc;

namespace WexProject.HttpViews.Controllers
{
    public class AngularController : Controller
    {
        //
        // GET: /Angular/

        public ActionResult despesas()
        {
            return PartialView();
        }

		public ActionResult despesa()
		{
			return PartialView();
		}

		public ActionResult rubricas()
		{
			return PartialView();
		}

		public ActionResult patrocinadores()
		{
			return PartialView();
		}

		public ActionResult centrosCusto()
		{
			return PartialView();
		}

		public ActionResult aditivo()
		{
			return PartialView();
		}

		public ActionResult termoAditivo()
		{
			return PartialView();
		}

        public ActionResult template(string id)
        {
			// TODO: Tem que melhorar esse método pra detectar automaticamente o caminho do template
			if (id.Contains('/'))
			{
				return PartialView("/Views" + id + ".cshtml");
			}
			else
			{
				return PartialView(id);
			}
        }

    }
}
