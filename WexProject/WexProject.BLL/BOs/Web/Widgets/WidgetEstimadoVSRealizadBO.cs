using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.BOs.Escopo;
using WexProject.BLL.Models.Web;

namespace WexProject.BLL.BOs.Web.Widgets
{
    /// <summary>
    /// Widget para Relatório de Realizado vs Planejado.
    /// </summary>
    public class WidgetEstimadoVSRealizadBO : WidgetAbstractBO
    {
        /// <summary>
        ///  Método responsável pela recuperação do json de dados para geração dos dados (gráfico, lista, etc) do Widget.
        /// </summary>
        /// <param name="id"> ID do objeto.</param>
        /// <returns>Json com o resultado do gráfico.</returns>
        public override string GetDados(object idDashboardWidget)
        {
            Guid oidDashboardWidget = new Guid(idDashboardWidget.ToString());
            try
            {
                using (SessionConnection sessionConnection = new SessionConnection())
                {
                    Session session = sessionConnection.getSession();
                    String grafico = string.Empty;
                    DashboardWidgetFiltro dashboardWigdetFiltro = 
                        session.FindObject<DashboardWidgetFiltro>
                        (CriteriaOperator.Parse("DashboardWidget.Oid = ?", idDashboardWidget));
                                        
                    Guid idProjeto = new Guid(dashboardWigdetFiltro.TxValor);

                    grafico = JsonConvert.SerializeObject(GraficoEstimadoRealizadoBO.CalcularGraficoEstimadoVsRealizadoProjeto(idProjeto, session));
                    
                    sessionConnection.Dispose();
                    return grafico;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
