using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using WexProject.BLL.BOs.RH;
using WexProject.BLL.Shared.DTOs.Rh;

namespace WexProject.HttpViews.Controllers
{
	public class ColaboradoresController : Controller
	{
		[HttpGet]
		public ActionResult Index( string login = null )
		{
			if(!String.IsNullOrWhiteSpace( login ))
			{
				ColaboradorDto colaborador = ColaboradorBo.ConsultarColaboradorPorLogin( Convert.ToString( login ) );
				return Json( colaborador, JsonRequestBehavior.AllowGet );
			}
			else
			{
				List<ColaboradorDto> colaboradores = ColaboradorBo.ConsultarColaboradores();
				return Json( colaboradores, JsonRequestBehavior.AllowGet );
			}
		}

		[HttpPut]
		public ActionResult Index( string login, string extensaoEmail )
		{
			if(!String.IsNullOrWhiteSpace( login ) || !String.IsNullOrWhiteSpace( extensaoEmail ))
			{
				ColaboradorBo.CriarColaborador( login, extensaoEmail );
				return new HttpStatusCodeResult( HttpStatusCode.OK );
			}

			return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
		}

		//GET: {base_url}/Colaboradores/UltimoProjetoSelecionado/{id}
		[HttpGet]
		[ActionName( "UltimoProjetoSelecionado" )]
		public ActionResult ConsultarUltimoProjetoSelecionado( Guid? id )
		{
			if(id.HasValue)
			{
				Guid oidUltimoProjetoSelecionado = ColaboradorUltimoFiltroBO.ConsultarUltimoProjetoSelecionadoPorColaborador( id.Value );

				return Json( oidUltimoProjetoSelecionado, JsonRequestBehavior.AllowGet );
			}

			return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
		}

		//PUT: {base_url}/Colaboradores/UltimoProjetoSelecionado/
        [AcceptVerbs( "POST", "PUT" )]
		[ActionName( "UltimoProjetoSelecionado" )]
		public ActionResult SalvarUltimoProjetoSelecionado( Guid? oidColaborador, Guid? oidProjeto )
		{
			if(oidProjeto.HasValue && oidColaborador.HasValue)
			{
				ColaboradorUltimoFiltroBO.SalvarUltimoProjetoSelecionado( oidColaborador.Value, oidProjeto.Value );

				return new HttpStatusCodeResult( HttpStatusCode.OK );
			}

			return new HttpStatusCodeResult( HttpStatusCode.BadRequest );
		}
	}
}
