using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.DAOs.Execucao;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Shared.Domains.Execucao;
using WexProject.Test.Features.StepDefinition;
using WexProject.Test.UnitTest;
using WexProject.BLL.Extensions.Entities;
using System.Data;
using WexProject.BLL.Shared.Domains.Escopo;
using WexProject.BLL.Contexto;

namespace WexProject.Test.Fixtures.Factory
{
    public class CicloDesenvEstoriaFactoryEntity : BaseEntityFrameworkTest
    {
        public static CicloDesenvEstoria Criar( WexDb contexto, CicloDesenv ciclo, Estoria estoria, CsSituacaoEstoriaCicloDomain situacaoEstoria )
        {
            CicloDesenvEstoria cicloDesenvEstoria = new CicloDesenvEstoria()
            {
                Ciclo = ciclo.Oid,
                Estoria = estoria.Oid,
                CsSituacao = (int)situacaoEstoria
            };

            ClicloDesenvEstoriaDAO.Salvar( contexto, cicloDesenvEstoria );

            if(!contexto.Estorias.ExisteLocalmente( o=>o.Oid == estoria.Oid )) 
            {
                contexto.Estorias.Attach( estoria );
            }

            AlterarEstadoEstoria( contexto, estoria, situacaoEstoria );
            return cicloDesenvEstoria;
        }

        private static void AlterarEstadoEstoria( WexDb contexto, Estoria estoria, CsSituacaoEstoriaCicloDomain situacaoEstoria ) 
        {
            switch(situacaoEstoria)
            {
                case CsSituacaoEstoriaCicloDomain.NaoIniciado:
                    estoria.CsSituacao = (int)CsEstoriaDomain.NaoIniciado;
                    break;
                case CsSituacaoEstoriaCicloDomain.EmDesenv:
                    estoria.CsSituacao = (int)CsEstoriaDomain.EmDesenv;
                    break;
                case CsSituacaoEstoriaCicloDomain.Pronto:
                    estoria.CsSituacao = (int)CsEstoriaDomain.Pronto;
                    break;
                case CsSituacaoEstoriaCicloDomain.Replanejado:
                    estoria.CsSituacao = (int)CsEstoriaDomain.Replanejado;
                    break;
                default:
                    break;
            }

            EntityState estado;
            if(!contexto.Estorias.Existe( o => o.Oid == estoria.Oid ))
            {
                estado = EntityState.Added;
            }
            else
            {
                estado = EntityState.Modified;
            }
            contexto.Entry( estoria ).State = estado;
            contexto.SaveChanges();
        }
    }
}
