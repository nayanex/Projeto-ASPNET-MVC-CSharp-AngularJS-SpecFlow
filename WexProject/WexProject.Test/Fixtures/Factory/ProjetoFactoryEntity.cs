using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Geral;
using WexProject.BLL.Entities.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    public class ProjetoFactoryEntity
    {
        public static Projeto Criar( WexDb contexto, UInt32 nbTamanhoTotal = 0, String txNome = "" )
        {
            Projeto projeto = new Projeto()
            {
                EmpresaInstituicao = EmpresaInstituiçãoFactoryEntity.Criar( contexto, "EmpInst" ).Oid,
                NbTamanhoTotal = Convert.ToInt32( nbTamanhoTotal ),
            };

            ProjetoUltimoFiltro projUltimoFiltro = new ProjetoUltimoFiltro()
            {
                Projeto = projeto.Oid
            };

            ProjetoDao.Instancia.SalvarProjeto( contexto, projeto );

            return projeto;
        }

        public static Projeto CriarProjetoRitmo( WexDb contexto, int nbTamanhoTotal = 0, String txNome = "", int totalCiclos = 1, int ritmo = 1 )
        {
            Projeto projeto = new Projeto()
            {
                TxNome = txNome,
                EmpresaInstituicao = EmpresaInstituiçãoFactoryEntity.Criar( contexto ).Oid,
                NbTamanhoTotal = nbTamanhoTotal,
                NbRitmoTime = ritmo,
                NbCicloTotalPlan = totalCiclos
            };

            ProjetoUltimoFiltro projUltimoFiltro = new ProjetoUltimoFiltro()
            {
                Projeto = projeto.Oid
            };

            contexto.Projetos.Add(projeto);

            contexto.ProjetoUltimoFiltroes.Add(projUltimoFiltro);

            for(int i = 0; i < projeto.NbCicloTotalPlan; i++)
            {
                CicloFactoryEntity.Criar( contexto, projeto, i.ToString() );
            }

            return projeto;
        }
    }
}
