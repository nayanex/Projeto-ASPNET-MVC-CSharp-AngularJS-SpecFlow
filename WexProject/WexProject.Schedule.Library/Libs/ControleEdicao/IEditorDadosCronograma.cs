using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;

namespace WexProject.Schedule.Library.Libs.ControleEdicao
{
	public interface IEditorDadosCronograma
	{
		void DesfazerEdicaoDadosCronograma( CronogramaDto editado , CronogramaDto original );

		void SalvarEdicaoDadosCronograma( CronogramaDto editado , CronogramaDto original );

		void ComunicarInicioEdicaoDadosCronograma();
	}
}
