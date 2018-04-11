using System;
using DevExpress.Xpo;
using WexProject.BLL;
using WexProject.BLL.DAOs.Escopo;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Geral;
using WexProject.Test.Helpers.BDD.Bind;
using WexProject.BLL.Contexto;

namespace WexProject.Test.Fixtures.Factory
{
    /// <summary>
    /// factory modulo
    /// </summary>
    public class ModuloFactory : BaseFactory
    {

        #region DevExpress

        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">Projeto</param>
        /// <param name="txNome">String</param>
        /// <param name="save">bool</param>
        /// <returns>modulo</returns>
        public static WexProject.BLL.Models.Escopo.Modulo Criar( Session session, WexProject.BLL.Models.Geral.Projeto projeto, String txNome = "", bool save = false )
        {
            WexProject.BLL.Models.Escopo.Modulo modulo = new WexProject.BLL.Models.Escopo.Modulo( session );
            if(String.IsNullOrEmpty( txNome ))
                modulo.TxNome = GetDescricao();
            else
                modulo.TxNome = txNome;
            modulo.Projeto = projeto;

            if(save)
                modulo.Save();

            return modulo;
        }

        /// <summary>
        /// método Criar
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">Projeto</param>
        /// <param name="txNome">String</param>
        /// <param name="save">bool</param>
        /// <returns>modulo</returns>
        public static WexProject.BLL.Models.Escopo.Modulo Criar( Session session, WexProject.BLL.Models.Geral.Projeto projeto, String txNome = "", uint tamanho = 1, bool save = false )
        {
            WexProject.BLL.Models.Escopo.Modulo modulo = new WexProject.BLL.Models.Escopo.Modulo( session );
            if(String.IsNullOrEmpty( txNome ))
                modulo.TxNome = GetDescricao();
            else
                modulo.TxNome = txNome;
            modulo.Projeto = projeto;
            modulo.NbPontosTotal = tamanho;
            if(save)
                modulo.Save();

            return modulo;
        }

        /// <summary>
        /// método CriarFilho
        /// </summary>
        /// <param name="contexto">session</param>
        /// <param name="moduloPai">Modulo</param>
        /// <param name="txNome">String</param>
        /// <param name="save">bool</param>
        /// <returns>moduloFilho</returns>
        public static WexProject.BLL.Models.Escopo.Modulo CriarModuloFilho( Session session, WexProject.BLL.Models.Escopo.Modulo moduloPai, String txNome = "", bool save = false )
        {
            WexProject.BLL.Models.Escopo.Modulo moduloFilho = Criar( session, moduloPai.Projeto, txNome, false );
            moduloFilho.ModuloPai = moduloPai;
            if(save)
                moduloFilho.Save();

            return moduloFilho;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="moduloPai"></param>
        /// <param name="txNome"></param>
        /// <param name="tamanho"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public static WexProject.BLL.Models.Escopo.Modulo CriarFilho( Session session, WexProject.BLL.Models.Escopo.Modulo moduloPai, String txNome = "", uint tamanho = 1, bool save = false )
        {
            WexProject.BLL.Models.Escopo.Modulo moduloFilho = Criar( session, moduloPai.Projeto, txNome, false );
            moduloFilho.ModuloPai = moduloPai;
            moduloFilho.NbPontosTotal = tamanho;
            if(save)
                moduloFilho.Save();

            return moduloFilho;
        }

        /// <summary>
        /// método crair sobrescrito
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="projeto">projeto</param>
        /// <param name="txNome">String</param>
        /// <param name="nbPontosNaoIniciado">NbPontosNaoIniciado</param>
        /// <param name="nbPontosEmAnalise">NbPontosEmAnalise</param>
        /// <param name="nbPontosEmDesenv">NbPontosEmDesenv</param>
        /// <param name="nbPontosPronto">NbPontosPronto</param>
        /// <param name="nbPontosDesvio">NbPontosDesvio</param>
        /// <param name="nbPontosTotal">NbPontosTotal</param>
        /// <param name="save">bool</param>
        /// <returns>modulo</returns>
        public static WexProject.BLL.Models.Escopo.Modulo CriarTeste( Session session, WexProject.BLL.Models.Geral.Projeto projeto, String txNome = "", int nbPontosNaoIniciado = 0,
        int nbPontosEmAnalise = 0, int nbPontosEmDesenv = 0, int nbPontosPronto = 0, int nbPontosDesvio = 0,
        int nbPontosTotal = 0, bool save = false )
        //public void Criar(int NbPontosNaoIniciado, int NbPontosEmAnalise, int NbPontosEmDesenv, int NbPontosPronto, int NbPontosDesvio, int NbPontosTotal,  bool save = false)
        {

            //Console.Write(this.Criar);
            WexProject.BLL.Models.Escopo.Modulo modulo = new WexProject.BLL.Models.Escopo.Modulo( session );

            if(String.IsNullOrEmpty( txNome ))
                modulo.TxNome = GetDescricao();

            modulo.NbPontosNaoIniciado = nbPontosNaoIniciado;
            modulo.NbPontosEmAnalise = nbPontosEmAnalise;
            modulo.NbPontosEmDesenv = nbPontosEmDesenv;
            modulo.NbPontosPronto = nbPontosPronto;
            modulo.NbPontosDesvio = nbPontosDesvio;
            modulo.NbPontosTotal = nbPontosTotal;
            modulo.Projeto = projeto;
            if(save)
                modulo.Save();

            return modulo;

        }
        
        #endregion


        #region Entity

        /// <summary>
        /// Método responsável por criar e salvar no banco em memória um objeto Módulo
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="projeto">Objeto  projeto</param>
        /// <param name="txNome">Nome do módulo</param>
        /// <returns>Objeto Módulo</returns>
        public static Modulo Criar( WexDb contexto, Projeto projeto, ModuloBindHelper moduloBindHelper )
        {
            Modulo modulo = new Modulo()
            {
                TxNome = moduloBindHelper.Nome,
                OidProjeto = projeto.Oid,
                NbPontosTotal = moduloBindHelper.Tamanho 
            };

            ModuloDAO.SalvarModulo( contexto, modulo );

            return modulo;
        }

        /// <summary>
        /// Método responsável por criar e salvar no banco em memória um objeto Módulo
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="projeto">Objeto  projeto</param>
        /// <param name="txNome">Nome do módulo</param>
        /// <returns>Objeto Módulo</returns>
        public static Modulo Criar( WexDb contexto, Projeto projeto, String txNome = "", uint tamanho = 1 )
        {
            Modulo modulo = new Modulo()
            {
                TxNome = txNome,
                OidProjeto = projeto.Oid,
                NbPontosTotal = tamanho
            };

            ModuloDAO.SalvarModulo( contexto, modulo );

            return modulo;
        }

        /// <summary>
        /// Método responsável por Criar um modulo filho
        /// </summary>
        /// <param name="contexto">Instância de conexão com o banco</param>
        /// <param name="moduloPai">Módulo Pai</param>
        /// <param name="txNome">Nome do Módulo Filho</param>
        /// <returns>Objeto Módulo Filho criado</returns>
        public static Modulo CriarModuloFilho( WexDb contexto, Modulo moduloPai, ModuloBindHelper moduloBindHelper )
        {
            Modulo moduloFilho = ModuloFactory.Criar( contexto, moduloPai.Projeto, moduloBindHelper );

            moduloFilho.ModuloPai = moduloPai;

            ModuloDAO.SalvarModulo( contexto, moduloFilho );

            return moduloFilho;
        }

        /// <summary>
        /// Método responsável por criar um módulo filho
        /// </summary>
        /// <param name="contexto">Instância do banco</param>
        /// <param name="moduloPai">Módulo Pai</param>
        /// <param name="txNome">Nome módulo filho</param>
        /// <param name="tamanho">tamanho do módulo filho</param>
        /// <returns>Objeto Módulo Filho criado</returns>
        public static Modulo CriarFilho( WexDb contexto, Modulo moduloPai, String txNome = "", uint tamanho = 1 )
        {
            Modulo moduloFilho = ModuloFactory.Criar( contexto, moduloPai.Projeto, txNome );

            moduloFilho.ModuloPai = moduloPai;
            moduloFilho.NbPontosTotal = tamanho;

            ModuloDAO.SalvarModulo( contexto, moduloFilho );

            return moduloFilho;
        }

        #endregion
    }
}
