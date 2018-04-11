using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.Test.UnitTest;

namespace WexProject.Test.Fixtures.Factory
{
    public class AditivoFactory : BaseEntityFrameworkTest
    {
        public static Aditivo CriarAditivo(string nome, decimal orcamento, int duracao, DateTime dtInicio, DateTime dtTermino, Guid projetoOid)
        {
            var aditivo = new Aditivo { TxNome = nome, NbOrcamento = orcamento, NbDuracao = duracao, DtInicio = dtInicio, DtTermino = dtTermino, ProjetoOid = projetoOid };
            AditivoDao.Instance.SalvarAditivo(aditivo);
            return aditivo;
        }
    }
}
