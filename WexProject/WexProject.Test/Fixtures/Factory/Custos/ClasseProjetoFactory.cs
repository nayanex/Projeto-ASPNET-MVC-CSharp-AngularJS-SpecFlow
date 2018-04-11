using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Contexto;
using WexProject.BLL.DAOs.Custos;
using WexProject.BLL.Models.Custos;
using WexProject.Test.UnitTest;
using System.Data.Entity.Migrations;

namespace WexProject.Test.Fixtures.Factory
{
    public class ClasseProjetoFactory : BaseEntityFrameworkTest
    {
        public static ClasseProjeto CriarClasseProjeto(int ClasseProjetoId, string TxNome)
        {
            var classeProjeto = new ClasseProjeto { ClasseProjetoId = ClasseProjetoId, TxNome = TxNome };
            using (WexDb contexto = ContextFactoryManager.CriarWexDb())
            {                
                contexto.ClassesProjetos.AddOrUpdate(classeProjeto);
                contexto.SaveChanges();
            }
            return classeProjeto;
        }
    }
}
