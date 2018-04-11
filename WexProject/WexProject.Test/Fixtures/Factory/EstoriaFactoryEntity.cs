using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Escopo;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Shared.Domains.Escopo;

namespace WexProject.Test.Fixtures.Factory
{
    public class EstoriaFactoryEntity
    {
        public static Estoria Criar(  WexDb contexto, Modulo modulo, Beneficiado beneficiado, string titulo, uint tamanho, string emAnalise )
        {
            Estoria estoria = new Estoria();
            //Projeto projeto = modulo.Projeto;
            estoria.OidModulo = modulo.Oid;
            estoria.OidBeneficiado = beneficiado.Oid;
            //estoria.OidCiclo = projeto.CicloDesenvs.FirstOrDefault(o=>o.NbCiclo )
            estoria.TxTitulo = titulo;
            estoria.NbTamanho = tamanho;

            if(emAnalise.ToUpper().Equals( "SIM" ))
            {
                estoria.CsEmAnalise = true;
                estoria.CsSituacao = Convert.ToInt32( CsEstoriaDomain.EmAnalise );
            }
            else
            {
                estoria.CsEmAnalise = false;
                estoria.CsSituacao = Convert.ToInt32( CsEstoriaDomain.NaoIniciado );
            }
            EstoriaDAO.SalvarEstoria( contexto, estoria );
            return estoria;
        }

        public static Estoria Criar( WexDb contexto, Modulo modulo, Beneficiado beneficiado, string titulo, uint tamanho, string emAnalise, Estoria estoriaPai )
        {
            Estoria estoria = new Estoria();
            estoria.OidModulo = modulo.Oid;
            estoria.OidEstoriaPai = estoriaPai.Oid;
            estoria.OidBeneficiado = beneficiado.Oid;
            estoria.TxTitulo = titulo;
            estoria.NbTamanho = tamanho;

            if(emAnalise.ToUpper().Equals( "SIM" ))
            {
                estoria.CsEmAnalise = true;
                estoria.CsSituacao = Convert.ToInt32( CsEstoriaDomain.EmAnalise );
            }
            else
            {
                estoria.CsEmAnalise = false;
                estoria.CsSituacao = Convert.ToInt32( CsEstoriaDomain.NaoIniciado );
            }

            EstoriaDAO.SalvarEstoria( contexto, estoria );

            return estoria;
        }
    }
}
