using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using WexProject.BLL.BOs;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Contexto;
using WexProject.BLL.Shared.DTOs.Geral;
using WexProject.BLL.Extensions.Entities;
using System.Data;

namespace WexProject.BLL.DAOs.Geral
{
	public class EmpresaInstituicaoDAO
	{
		private static EmpresaInstituicaoDAO instancia;

		public static EmpresaInstituicaoDAO Instancia
		{
			get
			{
				if (instancia == null) {
					instancia = new EmpresaInstituicaoDAO();
				}

				return instancia;
			}
		}

		private EmpresaInstituicaoDAO()
		{
		}

		/// <summary>
		/// Lista Empresas/Instituições do Banco de Dados.
		/// </summary>
		/// <returns>Uma lista de EmpresaInstituição.</returns>
		public List<EmpresaInstituicao> ListarEmpresasInstituicoes()
		{
			List<EmpresaInstituicao> empresasInstituicoes;

			using (var _db = new WexDb())
			{
				empresasInstituicoes = (from ei in _db.EmpresasInstituicoes
										orderby ei.TxNome
										select ei)
										.ToList();
			}

			return empresasInstituicoes;
		}

        /// <summary>
        /// Método responsável por criar ou salvar uma alteração em Empresa Instituição
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="empresaInstituicao">Objeto Empresa Instituição</param>
        public void Salvar(WexDb contexto, EmpresaInstituicao empresaInstituicao )
        {
            if(!contexto.EmpresasInstituicoes.ExisteLocalmente( o => o.Oid == empresaInstituicao.Oid ))
                contexto.EmpresasInstituicoes.Attach( empresaInstituicao );

            if(contexto.EmpresasInstituicoes.Existe( o => o.Oid == empresaInstituicao.Oid ))
                contexto.Entry( empresaInstituicao ).State = EntityState.Modified;
            else
                contexto.Entry( empresaInstituicao ).State = EntityState.Added;
        
            contexto.SaveChanges();
        }
	}
}
