using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Models.Planejamento;
using DevExpress.Xpo;
using WexProject.BLL.Shared.Domains.Geral;
using WexProject.BLL.Models.Geral;

namespace WexProject.Test.Fixtures.Factory
{
    public class PaisFactory:BaseFactory
    {

        public static Pais Criar(Session session, string nome,string mascara = "(xx)xxxx-xxxx", CsSituacaoDomain situacao = CsSituacaoDomain.Ativo, bool save = true) 
        {
            Pais pais = new Pais(session)
            {
                CsSituacao = situacao,
                TxMascara = mascara
            };

            pais.Country.Name = nome;
            pais.Country.PhoneCode = "55";

            if(save)
                pais.Save();

            return pais;
        }
    }
}