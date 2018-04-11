using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Execucao;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Entities.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    public class CicloFactoryEntity
    {
        public static CicloDesenv Criar( WexDb contexto, Projeto projeto, string txMeta = "" )
        {
            CicloDesenv ciclo = new CicloDesenv()
            {
                TxMeta = txMeta,
                Projeto = projeto.Oid
            };

            CicloDesenvDAO.SalvarCicloDesenv( contexto, ciclo );
            
            return ciclo;
        }
    }
}
