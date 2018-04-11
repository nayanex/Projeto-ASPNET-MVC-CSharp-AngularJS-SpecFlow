using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WexProject.Library.Libs.Concorrencia
{
    public class ConcorrenciaUtil
    {
        /// <summary>
        /// Método responsável por setar os valores atuais e os valores no banco de dados com os valores que estão resolvidos, ou seja, os valores que foram escolhidos para salvar.
        /// </summary>
        /// <param name="exDatabase">Excessão causada por concorrência no banco de dados</param>
        /// <param name="atualizarComDadosDoCliente">Deseja resolver os dados com os dados do cliente ou não (com o do banco de dados)</param>
        public static void AtualizarDadosConcorrentes( DbUpdateConcurrencyException exDatabase, bool atualizarComDadosDoCliente = true )
        {
            var entidades = exDatabase.Entries.Single();
            var valoresAtuais = entidades.CurrentValues;
            var valoresBancoDeDados = entidades.GetDatabaseValues();

            DbPropertyValues valoresResolvidos = null;

            if(atualizarComDadosDoCliente)
            {
                valoresResolvidos = valoresAtuais.Clone();
            }
            else
            {
                valoresResolvidos = valoresBancoDeDados.Clone();
            }

            valoresAtuais.SetValues( valoresResolvidos );
            valoresBancoDeDados.SetValues( valoresResolvidos );

            entidades.OriginalValues.SetValues( valoresBancoDeDados );
            entidades.CurrentValues.SetValues( valoresResolvidos );
        }
    }
}
