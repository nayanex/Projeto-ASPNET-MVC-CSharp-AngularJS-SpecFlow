using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Outros;

namespace WexProject.BLL.BOs.Outros
{
    public class PartyBO
    {
        /// <summary>
        /// Método responsável por criar um objeto Party padrão
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <returns>Objeto Party</returns>
        public static Party CriarPartyPadrao( WexDb contexto )
        {
		    Party party = new Party()
                {
                    Photo = null,
                    Address1 = null,
                    Address2 = null,
                    OptimisticLockField = 0,
                    GCRecord = null,
                    ObjectType = 1
                };

            contexto.Party.Add( party );
            contexto.SaveChanges();

            return party;
        }
    }
}
