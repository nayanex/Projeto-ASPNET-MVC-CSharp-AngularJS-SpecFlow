using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.Models.Custos;
using System.Data.Entity.Migrations;

namespace WexProject.Test.Fixtures.Factory
{
    public class TipoProjetoFactory
    {
        public static TipoProjeto CriarTipoProjeto(int TipoProjetoId, int ClasseProjetoId, string TxNome)
        {
            var tipoProjeto = new TipoProjeto { TipoProjetoId = TipoProjetoId, ClasseProjetoId = ClasseProjetoId, TxNome = TxNome };
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                contexto.TiposProjetos.AddOrUpdate(tipoProjeto);
                contexto.SaveChanges();
            }
            return tipoProjeto;
        }

    }
}
