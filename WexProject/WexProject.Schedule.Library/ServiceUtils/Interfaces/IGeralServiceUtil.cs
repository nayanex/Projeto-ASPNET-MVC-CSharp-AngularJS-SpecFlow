using System;
using WexProject.BLL.Shared.DTOs.Rh;
using System.Collections.Generic;
using WexProject.BLL.Shared.DTOs.Geral;
namespace WexProject.Schedule.Library.ServiceUtils.Interfaces
{
    public interface IGeralServiceUtil
    {
        ColaboradorDto ConsultarColaboradorLogado( string login );

        SemanaTrabalhoDto ConsultarSemanaDeTrabalhoPadrao();
    }
}
