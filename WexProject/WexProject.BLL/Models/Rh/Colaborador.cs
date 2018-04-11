using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security;
using DevExpress.Data.Filtering;
using System.Collections;
using WexProject.BLL.Shared.Domains.Rh;
using DevExpress.Xpo.DB;
using System.Drawing;
using WexProject.BLL.Models.Geral;
using WexProject.Library.Libs.DataHora;
using WexProject.Library.Libs.Str;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.DAOs.Geral;
using DevExpress.Xpo.Metadata;
using WexProject.BLL.Models.Planejamento;

namespace WexProject.BLL.Models.Rh
{
    /// <summary>
    /// Colaborador
    /// </summary>
    [DeferredDeletion( false )]
    [DefaultClassOptions]
    [Custom( "Caption", "Colaborador" )]
    [OptimisticLocking( false )]
    public class Colaborador : BaseObject
    {
        #region Atributos

        /// <summary>
        /// Atributo de Usuario
        /// </summary>
        private User usuario;

        /// <summary>
        /// Atributo de TxMatricula
        /// </summary>
        private string txMatricula;

        /// <summary>
        /// Atributo de DtAdmissao
        /// </summary>
        private DateTime dtAdmissao;

        /// <summary>
        /// Atributo de Coordenador
        /// </summary>
        private Colaborador coordenador;

        /// <summary>
        /// Atributo para informar o cargo ocupado pelo colaborador
        /// </summary>
        private Cargo cargo;

        /// <summary>
        /// Texto de Planejamento de Férias atual
        /// </summary>
        private string planoFeriasAtual = null;

        /// <summary>
        /// Situação geral das férias
        /// </summary>
        private CsSituacaoFerias _csSituacaoFerias;

        /// <summary>
        /// Dados antigos do Colaborador
        /// </summary>
        private Colaborador colaboradorOld = null;

        /// <summary>
        /// IsActive do User antigo
        /// </summary>
        private bool colaboradorUserActiveOld;

        /// <summary>
        /// Último filtro do Colaborador
        /// </summary>
        private ColaboradorUltimoFiltro colaboradorUltimoFiltro;

        #endregion

        #region Propriedades

        /// <summary>
        /// Usuário referente ao Colaborador
        /// </summary>
        [Browsable( false )]
        public User Usuario
        {
            get
            {
                return usuario;
            }
            set
            {
                SetPropertyValue<User>( "Usuario", ref usuario, value );
            }
        }

        /// <summary>
        /// Matrícula do colaborador
        /// </summary>
        [Custom( "Caption", "Matrícula" )]
        [RuleUniqueValue( "Colaborador_TxMatricula_Unique", DefaultContexts.Save,
        "Já existe um colaborador com essa matrícula!" )]
        [RuleRequiredField( "Colaborador_TxMatricula_Required", DefaultContexts.Save, "É necessário inserir uma matrícula para o colaborador." )]
        public string TxMatricula
        {
            get
            {
                return txMatricula;
            }
            set
            {
                txMatricula = StrUtil.RetirarEspacoVazio( txMatricula );
                SetPropertyValue<string>( "TxMatricula", ref txMatricula, value );
            }
        }

        /// <summary>
        /// Data de admissão do colaborador
        /// </summary>
        [Custom( "Caption", "Admissão" )]
        [RuleRequiredField( "Colaborador_DtAdmissao_Required", DefaultContexts.Save, "É necessário inserir uma data de admissão para o colaborador." )]
        public DateTime DtAdmissao
        {
            get
            {
                return dtAdmissao;
            }
            set
            {
                SetPropertyValue<DateTime>( "DtAdmissao", ref dtAdmissao, value );
            }
        }

        ///// <summary>
        ///// Atributo que guarda as tarefas de um colaborador
        ///// </summary>
        //[Association( "TarefaResponsaveis", typeof( Tarefa ), UseAssociationNameAsIntermediateTableName = true )]
        //public XPCollection<Tarefa> Tarefas
        //{
        //    get
        //    {
        //        return GetCollection<Tarefa>( "Tarefas" );
        //    }
        //}

        /// <summary>
        /// Coordenador do colaborador
        /// </summary>
        [Indexed]
        [Custom( "Caption", "Superior Imediato" )]
        public Colaborador Coordenador
        {
            get
            {
                return coordenador;
            }
            set
            {
                SetPropertyValue<Colaborador>( "Coordenador", ref coordenador, value );
            }
        }

        /// <summary>
        /// Propiedade que guarda o cargo do colaborador
        /// </summary>
        [RuleRequiredField( "Colaborador_Cargo_Required", DefaultContexts.Save, "É necessário inserir um cargo para o colaborador." )]
        public Cargo Cargo
        {
            get
            {
                return cargo;
            }
            set
            {
                SetPropertyValue<Cargo>( "Cargo", ref cargo, value );
            }
        }

        /// <summary>
        /// Coleção de períodos aquisitivos
        /// </summary>
        [Custom( "Caption", "Períodos Aquisitivos" )]
        [Association( "Colaborador_ColaboradorPeriodoAquisitivo", typeof( ColaboradorPeriodoAquisitivo ) ), Aggregated]
        public XPCollection<ColaboradorPeriodoAquisitivo> PeriodosAquisitivos
        {
            get
            {
                return GetCollection<ColaboradorPeriodoAquisitivo>( "PeriodosAquisitivos" );
            }
        }

        /// <summary>
        /// Coleção de afastamentos
        /// </summary>
        [Custom( "Caption", "Afastamentos" )]
        [Association( "Colaborador_ColaboradorAfastamento", typeof( ColaboradorAfastamento ) ), Aggregated]
        public XPCollection<ColaboradorAfastamento> Afastamentos
        {
            get
            {
                return GetCollection<ColaboradorAfastamento>( "Afastamentos" );
            }
        }

        /// <summary>
        /// Último filtro do Colaborador
        /// </summary>
        [Browsable( false )]
        public ColaboradorUltimoFiltro ColaboradorUltimoFiltro
        {
            get
            {
                return colaboradorUltimoFiltro;
            }
            set
            {
                if(colaboradorUltimoFiltro == value)
                    return;

                SetPropertyValue<ColaboradorUltimoFiltro>( "ColaboradorUltimoFiltro", ref colaboradorUltimoFiltro, value );
            }
        }

        #endregion

        #region Objetos Não Persistentes

        /// <summary>
        /// Propriedade que traz as atividades do usuário
        /// </summary>
        [NonPersistent]
        public XPCollection<Role> _Roles
        {
            get
            {
                return Usuario.Roles;
            }
        }

        /// <summary>
        /// Propriedades que verifica as permissões do usuário
        /// </summary>
        [NonPersistent]
        public IList<IPermission> _Permissions
        {
            get
            {
                return Usuario.Permissions;
            }
        }

        /// <summary>
        /// Resgata todos os phonenumbers
        /// </summary>
        [NonPersistent]
        public XPCollection<PhoneNumber> _PhoneNumber
        {
            get
            {
                return Usuario.PhoneNumbers;
            }
        }

        /// <summary>
        /// Atributo de email
        /// </summary>
        [NonPersistent, Custom( "Caption", "Endereço" )]
        public Address _Endereco
        {
            get
            {
                return Usuario.Address1;
            }
            set
            {
                Usuario.Address1 = value;
            }
        }

        /// <summary>
        /// Propriedade para ver uma view identificando a mesma
        /// </summary>
        [NonPersistent, Browsable( false )]
        public string GetViewId
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Nome completo do Colaborador
        /// </summary>
        [NonPersistent, Custom( "Caption", "Nome Completo" )]
        public string _NomeCompleto
        {
            get
            {
                string middle = string.Empty, complete;

                if(!string.IsNullOrEmpty( Usuario.MiddleName ))
                {
                    middle = Usuario.MiddleName + ' ';
                }

                complete = String.Format( "{0} {1}{2}", Usuario.FirstName, middle, Usuario.LastName );

                return complete.Trim();
            }
        }

        /// <summary>
        /// Texto do Plano de Férias atual
        /// </summary>
        [NonPersistent, Custom( "Caption", "Plano de Férias Atual" )]
        [VisibleInDetailView( false )]
        public string _PlanoFeriasAtual
        {
            get
            {
                XPCollection<FeriasPlanejamento> planejamentos = new XPCollection<FeriasPlanejamento>( Session,
                    CriteriaOperator.Parse( "(CsSituacao = ? OR CsSituacao = ?) AND Periodo.Colaborador = ?",
                    CsSituacaoFerias.EmAtraso, CsSituacaoFerias.Planejado, this ) );

                return ColaboradorPeriodoAquisitivo.CalculoTextoPlanejamentoFerias( planejamentos, ref planoFeriasAtual, ref _csSituacaoFerias );
            }
        }

        /// <summary>
        /// Situação geral das férias
        /// </summary>
        [NonPersistent, VisibleInDetailView( false )]
        public CsSituacaoFerias _CsSituacaoFerias
        {
            get
            {
                return _csSituacaoFerias;
            }
            set
            {
                SetPropertyValue<CsSituacaoFerias>( "_CsSituacaoFerias", ref _csSituacaoFerias, value );
            }
        }

        [NonPersistent, VisibleInListView( false )]
        public Image _Foto
        {
            get
            {
                return Usuario.Photo;
            }
            set
            {
                Usuario.Photo = value;
            }
        }

        #endregion

        #region Regras de Negócio

        [RuleFromBoolProperty( "ValidacaoDadosColaborador_Colaborador", DefaultContexts.Save,
            CustomMessageTemplate = @"Alguns dados do Colaborador precisam ser definidos." )]
        [NonPersistent, Browsable( false )]
        public bool _ValidacaoDadosColaborador
        {
            get
            {
                if(Cargo == null || string.IsNullOrEmpty( TxMatricula ))
                    return false;

                return true;
            }
        }

        /// <summary>
        /// Validação do nome completo
        /// </summary>
        [RuleFromBoolProperty( "ValidacaoUsuarioRede_Colaborador", DefaultContexts.Save,
            CustomMessageTemplate = @"É necessário definir o usuário de rede." )]
        [NonPersistent, Browsable( false )]
        public bool _ValidacaoUsuarioRede
        {
            get
            {
                if(Usuario.UserName != null)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Validação do nome completo
        /// </summary>
        [RuleFromBoolProperty( "ValidacaoNomeCompleto_Colaborador", DefaultContexts.Save,
            CustomMessageTemplate = @"É necessário definir pelo menos primeiro nome e um sobrenome" )]
        [NonPersistent, Browsable( false )]
        public bool _ValidacaoNomeCompleto
        {
            get
            {
                if(Usuario.FullName != null)
                {
                    if(Usuario.FullName.Split( ' ' ).Length < 2)
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Validação da data de admissão do colaborador
        /// </summary>
        [RuleFromBoolProperty( "ValidacaoDtAdmissao_Colaborador", DefaultContexts.Save,
            CustomMessageTemplate = @"A Data de Admissão precisa ser menor que todos os Planejamentos de Férias feitos para o Colaborador." )]
        [NonPersistent, Browsable( false )]
        public bool _ValidacaoDtAdmissao
        {
            get
            {
                int qtde;

                PeriodosAquisitivos.Filter = CriteriaOperator.Parse( "Planejamentos[_DtRetorno > ?]", DtAdmissao.Date );
                qtde = PeriodosAquisitivos.Count;

                PeriodosAquisitivos.Filter = null;

                if(qtde > 0 && colaboradorOld != null && DtAdmissao != colaboradorOld.DtAdmissao)
                    return false; // inválido

                return true;
            }
        }

        /// <summary>
        /// Validação da data de admissão
        /// </summary>
        [RuleFromBoolProperty( "ValidacaoDtAdmissaoMaiorHoje_Colaborador", DefaultContexts.Save,
            CustomMessageTemplate = @"A Data de Admissão não pode ser maior que a Data de Hoje." )]
        [NonPersistent, Browsable( false )]
        public bool _ValidacaoDtAdmissaoMaiorHoje
        {
            get
            {
                if(DtAdmissao.Date > DateTime.Now.Date)
                {
                    return false; // inválido
                }

                return true;
            }
        }

        /// <summary>
        /// Propriedade do email
        /// </summary>
        [NonPersistent, Custom( "Caption", "Email" )]
        [RuleUniqueValue( "Colaborador_Email_Unique", DefaultContexts.Save,
            "Já existe um colaborador com esse email!" )]
        public string _Email
        {
            get
            {
                return Usuario.Email;
            }
            set
            {
                Usuario.Email = value;
            }
        }

        /// <summary>
        /// Método responsável por verificar se colaborador existe, senão deve criá-lo.
        /// É usado como verificação pelo AD
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="login">Login do usuário</param>
        /// <param name="extensaoEmail">Extensão de email da instituição</param>
        public static void RnAutenticarColaborador( Session session, string login, string extensaoEmail )
        {
            if(session == null || login == null)
                throw new Exception( "Os objetos Sessão e Login não podem ser nulos." );

            Colaborador colaborador = GetColaboradorPorLogin( session, login );
            if(colaborador == null)
                colaborador = Colaborador.RnCriarColaborador( session, login, extensaoEmail );
            UsuarioDAO.CurrentUser = colaborador.Usuario;
        }

        /// <summary>
        /// Método responsável por criar o colaborador caso não exista.
        /// É usado pelo AD
        /// <param name="extensaoEmail">Extensao do email da empresa</param>
        /// <param name="session">Sessão</param>
        /// <param name="login">Login do usuário</param>
        /// </summary>
        public static Colaborador RnCriarColaborador( Session session, string login, string extensaoEmail )
        {
            if(session == null || String.IsNullOrEmpty( login ) == true || String.IsNullOrEmpty( extensaoEmail ) == true)
                throw new Exception( "Os parâmetros Sessão, Login e ExtensaoEmail não podem ser nulos." );

            string firstName;
            string lastName;
            string fullName;
            DateTime dtAdmissaoCriada;

            Colaborador colaboradorPesq = Colaborador.GetColaboradorPorLogin( session, login );

            if(colaboradorPesq == null)
            {
                if(login.Contains( "." ))
                {
                    firstName = login.Split( '.' )[0];
                    lastName = login.Split( '.' )[1];
                    fullName = String.Format( "{0} {1}", StrUtil.UpperCaseFirst( firstName ), StrUtil.UpperCaseFirst( lastName ) );
                }
                else
                {
                    firstName = login;
                    lastName = "";
                    fullName = String.Format( "{0}", StrUtil.UpperCaseFirst( login ) );
                }

                Colaborador colaboradorCriado = new Colaborador( session );

                dtAdmissaoCriada = DateUtil.ConsultarDataHoraAtual();
                firstName = StrUtil.UpperCaseFirst( firstName );
                lastName = StrUtil.UpperCaseFirst( lastName );
                colaboradorCriado.Usuario.ChangePasswordOnFirstLogon = false;
                colaboradorCriado.Usuario.FirstName = firstName;
                colaboradorCriado.Usuario.LastName = lastName;
                colaboradorCriado.DtAdmissao = dtAdmissaoCriada;
                colaboradorCriado.Usuario.UserName = login;
                colaboradorCriado.Usuario.Email = login + extensaoEmail;

                Role roleDefault = GetRolePorNome( session, "Default" );
                colaboradorCriado.Usuario.Roles.Add( roleDefault );

                colaboradorCriado.Save();

                UsuarioDAO.CurrentUser = colaboradorCriado.Usuario;

                return colaboradorCriado;
            }

            UsuarioDAO.CurrentUser = colaboradorPesq.Usuario;

            return colaboradorPesq;
        }

        #endregion

        #region Pesquisa

        /// <summary>
        /// Método responsável por buscar um colaborador de acordo com seu Oid.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <param name="oidColaborador">Oid de colaborador (ID)</param>
        /// <returns>Objeto colaborador</returns>
        public static Colaborador GetColaboradorPorOid( Session session, Guid oidColaborador )
        {
            return session.FindObject<Colaborador>( CriteriaOperator.Parse( "Oid = ?", oidColaborador ) );
        }

        /// <summary>
        /// Método responsável por resgatar uma role default
        /// <param name="session">Sessão</param>
        /// <param name="nome">nome da role default(pode passar o nome como "Default")</param>
        /// </summary>
        public static Role GetRolePorNome( Session session, string nome )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Role>(),
                CriteriaOperator.Parse( string.Format( "Name = '{0}'", nome ) ), null, 1, false, true );

            Role role = null;

            foreach(Role rl in collection)
                role = rl;

            return role;
        }

        /// <summary>
        /// Resgata o objeto de colaborador através do Login
        /// </summary>
        /// <param name="session">secao atual do banco de dados</param>
        /// <param name="login">Login do usuario no sistema</param>
        /// <returns>Objeto do Colaborador</returns>
        public static Colaborador GetColaboradorPorLogin( Session session, string login )
        {
            try
            {
                return session.FindObject<Colaborador>( CriteriaOperator.Parse( "Usuario.UserName = ?", login ) );
            }
            catch(Exception e)
            {
                Exception excessao = new Exception(String.Format("Mensagem: {0} - StackTrace: {1}", e.Message, e.StackTrace));
                throw excessao;
            }
        }

        /// <summary>
        /// Método responsável por acessar a classe colaborador e buscar o colaborador corrente e retornar para o serviço
        /// </summary>
        /// <param name="session">Sessão Corrente</param>
        /// <param name="login">Login do colaborador</param>
        /// <returns>Retorna um DTO de colaborador</returns>
        public static ColaboradorDto GetColaboradorPorLoginDto( Session session, string login )
        {
            var  colaborador = GetColaboradorPorLogin( session, login );
            if(colaborador != null)
                return colaborador.DtoFactory();
            else
                return null;
        }

        /// <summary>
        /// Resgata o Colaborador Logado
        /// </summary>
        /// <param name="session">Session</param>
        /// <returns>Lista de Colaboradores</returns>
        public static XPCollection<Colaborador> GetColaboradores( Session session )
        {
            SortingCollection sortCollection = new SortingCollection();
            sortCollection.Add( new SortProperty( "Usuario.FullName", SortingDirection.Ascending ) );
            XPCollection<Colaborador> result = new XPCollection<Colaborador>( session );
            result.Sorting.Add( sortCollection );
            return result;
        }

        /// <summary>
        /// Método responsável por acessar a classe Colaborador e buscar todos os colaboradores e retornar pelo serviço.
        /// </summary>
        /// <param name="session">Sessão corrente</param>
        /// <returns>Retorna uma lista do tipo dto de colaboradores</returns>
        public static List<ColaboradorDto> GetColaboradoresDto( Session session )
        {
            List<ColaboradorDto> colaboradoresDto = new List<ColaboradorDto>();

            //pega todos os colaboradores e transforma numa lista, pois ele vem como xpCollection
            using(XPCollection<Colaborador> colaboradores = Colaborador.GetColaboradores( session ))
            {
                if(colaboradores == null || colaboradores.Count <= 0)
                    return colaboradoresDto;
                foreach(Colaborador colaborador in colaboradores)
                    colaboradoresDto.Add( colaborador.DtoFactory() );
            }

            return colaboradoresDto;
        }

        /// <summary>
        /// Resgata o objeto de colaborador do corrente usuário
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="user">CurrentUser</param>
        /// <returns>Objeto do Colaborador</returns>
        public static Colaborador GetColaboradorCurrent( Session session, User user )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Colaborador>(),
                CriteriaOperator.Parse( string.Format( "Usuario = '{0}'", user.Oid ) ), null, 1, false, true );

            Colaborador result = null;

            foreach(Colaborador ct in collection)
                result = ct;

            return result;
        }

        /// <summary>
        /// Resgata o objeto de colaborador do corrente usuário
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="user">CurrentUser</param>
        /// <returns>Objeto do Colaborador</returns>
        public static Colaborador GetColaboradorCurrent( Session session, Guid user )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Colaborador>(),
                CriteriaOperator.Parse( String.Format( "Usuario = '{0}'", user ) ), null, 1, false, true );

            Colaborador result = null;

            foreach(Colaborador ct in collection)
                result = ct;

            return result;
        }

        /// <summary>
        /// Resgata o objeto de colaborador do corrente usuário
        /// </summary>
        /// <param name="session">Session</param>
        /// <param name="user">CurrentUser</param>
        /// <returns>Objeto do Colaborador</returns>
        public bool IsSuperiorImediato( Session session, User user )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Colaborador>(),
                CriteriaOperator.Parse( string.Format( "Coordenador.Usuario.Oid = '{0}'", user.Oid ) ), null, 1, false, true );

            if(collection.Count > 0)
                return true;

            return false;
        }

        /// <summary>
        /// Resgata o objeto de colaborador do usuário logado
        /// </summary>
        /// <param name="session">session</param>
        /// <returns>Colaborador logado</returns>
        public static Colaborador GetColaboradorCurrent( Session session )
        {
            return GetColaboradorCurrent( session, UsuarioDAO.GetUsuarioLogado( session ) );
        }

        /// <summary>
        /// Regra que salva a última SEOT para o usuário
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="user">Colaborador que será salvo</param>
        /// <param name="colaborador">Colaborador logado</param>
        public static void RnSalvarUsuarioUltimaSEOT( Session session, Guid user, Colaborador colaborador )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Colaborador>(),
                CriteriaOperator.Parse( String.Format( "Usuario = '{0}'", colaborador.usuario.Oid ) ), null, 1, false, true );

            Colaborador result = null;

            foreach(Colaborador ct in collection)
                result = ct;

            result.ColaboradorUltimoFiltro.LastUsuarioFilterSeot = user;

            result.Save();

            if(session.ConnectionString.Equals( string.Empty ))
            {
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Regra que salva a última SEOT para a situação
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="situacao">Colaborador que será salvo</param>
        /// <param name="colaborador">Colaborador logado</param>
        public static void RnSalvarSituacaoUltimaSEOT( Session session, Guid situacao, Colaborador colaborador )
        {
            ICollection collection = session.GetObjects( session.GetClassInfo<Colaborador>(),
                CriteriaOperator.Parse( String.Format( "Usuario = '{0}'", colaborador.usuario.Oid ) ), null, 1, false, true );

            Colaborador result = null;

            foreach(Colaborador ct in collection)
                result = ct;

            result.ColaboradorUltimoFiltro.LastSituacaoFilterSeot = situacao;

            result.Save();

            if(session.ConnectionString.Equals( string.Empty ))
            {
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Salvar o último período selecionado no planejamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="index">Index do Item selecionado</param>
        public static void RnSalvarPeriodoUltimoPlanejamentoFerias( Session session, Colaborador colaborador, int index )
        {
            colaborador.ColaboradorUltimoFiltro.LastPeriodoFilterPlanejamentoFerias = index;
            colaborador.Save();

            if(session.ConnectionString.Equals( string.Empty ))
            {
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Salvar a última situação selecionada no planejamento
        /// </summary>
        /// <param name="session">Sessão</param>
        /// <param name="colaborador">Colaborador</param>
        /// <param name="index">Index do Item selecionado</param>
        public static void RnSalvarPeriodoUltimaSituacaoFerias( Session session, Colaborador colaborador, int index )
        {
            colaborador.ColaboradorUltimoFiltro.LastSituacaoFilterPlanejamentoFerias = index;
            colaborador.Save();

            if(session.ConnectionString.Equals( string.Empty ))
            {
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Seta o ultimo filtro que o usuario selecionou
        /// </summary>
        /// <param name="ultimoSelecionado">O objeto selecionado pelo usuario</param>
        public static void RnSetUltimoFiltroSuperiorImediato( Session session, string ultimoSelecionado, Colaborador colaborador )
        {
            if(Colaborador.GetColaboradorCurrent( session ).Oid == colaborador.Oid)
            {
                colaborador.ColaboradorUltimoFiltro.LastSuperiorImediatoFilterPlanejamentoFerias = ultimoSelecionado;
                session.UpdateSchema();
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Seta o ultimo filtro que o usuario selecionou para situação de ferias
        /// </summary>
        /// <param name="ultimoSelecionado">O objeto selecionado pelo usuario</param>
        public static void RnSetUltimoFiltroSituacaoFerias( Session session, string ultimoSelecionado, Colaborador colaborador )
        {
            if(Colaborador.GetColaboradorCurrent( session ).Oid == colaborador.Oid)
            {
                colaborador.ColaboradorUltimoFiltro.LastSituacaoFeriasFilterPlanejamentoFerias = ultimoSelecionado;
                session.UpdateSchema();
                session.CommitTransaction();
            }
        }

        /// <summary>
        /// Lista de Todos os Superiores Imediatos do Sistema
        /// </summary>
        /// <param name="session">Sessão atual</param>
        /// <returns>List de Colaborador com todos os superiores imediatos do Siatema</returns>
        public static List<Colaborador> GetAllSuperioresImediatos( Session session )
        {
            List<Colaborador> superiores = new List<Colaborador>();

            using(XPCollection<Colaborador> collection = new XPCollection<Colaborador>( session, CriteriaOperator.Parse( "Coordenador is not null" ) ))
            {
                var list = ( from item in collection select item.Coordenador ).Distinct();

                superiores.AddRange( list.ToList() );
            }

            return superiores;
        }

        public static List<Colaborador> GetColaboradorBySuperiores( Session session, string[] superiores_oid )
        {

            List<Colaborador> colaboradores = new List<Colaborador>();

            List<CriteriaOperator> criterias = new List<CriteriaOperator>();

            for(int i = 0; i < superiores_oid.Length; i++)
            {
                criterias.Add( CriteriaOperator.Parse( "Coordenador=?", new Guid( superiores_oid[i] ) ) );
            }

            using(XPCollection<Colaborador> collection = new XPCollection<Colaborador>( session, CriteriaOperator.Or( criterias ) ))
            {
                colaboradores.AddRange( collection );
            }

            return colaboradores;
        }

        #endregion

        #region Factories

        /// <summary>
        /// Método responsável por criar um objeto ColaboradorDto
        /// </summary>
        /// <param name="colaborador">Objeto Colaborador</param>
        /// <returns>Objeto ColaboradorDto</returns>
        public ColaboradorDto DtoFactory()
        {
            ColaboradorDto colaboradorDto = new ColaboradorDto()
            {
                OidColaborador = Oid,
                OidUsuario = Usuario.Oid,
                TxMatriculaColaborador = TxMatricula,
                Login = Usuario.UserName,
                TxNomeCompletoColaborador = _NomeCompleto
            };

            return colaboradorDto;
        }

        #endregion

        #region Utilitários

        /// <summary>
        /// Quando estiver persistindo o objeto
        /// </summary>
        protected override void OnSaving()
        {
            // Verifica a criação/recriação de períodos aquisitivos
            if(IsCriarPeriodosAquisitivos())
            {
                CriarPeriodosAquisitivos();
            }

            ColaboradorUltimoFiltro.Save();

            base.OnSaving();
        }

        /// <summary>
        /// Quando tiver apagando o objeto
        /// </summary>
        protected override void OnDeleting()
        {
            Usuario.Delete();
            ColaboradorUltimoFiltro.Delete();

            base.OnDeleting();
        }

        /// <summary>
        /// Indica se é para criar Período Aquisitivo
        /// </summary>
        /// <returns>True se for para criar e false se não for</returns>
        private bool IsCriarPeriodosAquisitivos()
        {
            return ( Oid == Guid.Empty && Usuario != null && Usuario.IsActive ) || ( colaboradorOld != null &&
                ( colaboradorOld.DtAdmissao != DtAdmissao || !colaboradorUserActiveOld && Usuario.IsActive ) );
        }

        /// <summary>
        /// Criação/recriação dos períodos aquisitivos
        /// </summary>
        public void CriarPeriodosAquisitivos()
        {
            if(DtAdmissao == DateTime.MinValue)
            {
                return;
            }

            // Períodos existentes
            Hashtable periodosHash = new Hashtable();
            foreach(ColaboradorPeriodoAquisitivo periodo in PeriodosAquisitivos)
            {
                string key = String.Format( "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", periodo.DtInicio, periodo.DtTermino );

                if(!periodosHash.ContainsKey( key ))
                {
                    periodosHash.Add( key, periodo );
                }
            }

            // Afastamentos não-remunerados
            Afastamentos.Filter = CriteriaOperator.Parse( "TipoAfastamento.IsRemunerado = false" );

            // Ordenação decrescente dos Afastamentos não-remunerados
            Afastamentos.Sorting.Add( new SortProperty( "DtInicio", SortingDirection.Descending ) );

            List<ColaboradorAfastamento> ausencias = new List<ColaboradorAfastamento>( Afastamentos );

            // Retirada da ordenação
            Afastamentos.Sorting.RemoveAt( Afastamentos.Sorting.Count - 1 );

            // Retirada do filtro
            Afastamentos.Filter = null;

            ColaboradorAfastamento ausencia = null;
            DateTime dtTerminoPeriodo, dtInicioProximoPeriodo = DtAdmissao, dtNow = DateUtil.ConsultarDataHoraAtual();

            // criação dos novos períodos aquisitivos
            while(dtInicioProximoPeriodo.Date <= dtNow.Date)
            {
                string key = string.Empty;

                dtTerminoPeriodo = dtInicioProximoPeriodo.Date.AddYears( 1 ).AddDays( -1 ); // data de término do período

                if(ausencia == null && ausencias.Count > 0)
                {
                    ausencia = ausencias[ausencias.Count - 1];
                }

                // Se houver ausência para o período atual, calcula o mesmo com retirada da mesma
                if(ausencia != null && ausencia.DtInicio.Date >= dtInicioProximoPeriodo.Date &&
                    ausencia.DtInicio.Date <= dtTerminoPeriodo.Date)
                {
                    DateTime dtInicio, dtTermino, dtTerminoAusencia = DateTime.MinValue;
                    int qtdeAusencia = 0;

                    while(ausencia != null && ausencia.DtInicio.Date >= dtInicioProximoPeriodo.Date &&
                            ausencia.DtInicio.Date <= dtTerminoPeriodo.Date)
                    {
                        dtTerminoAusencia = ausencia.DtTermino;
                        qtdeAusencia += ausencia.DtTermino.Subtract( ausencia.DtInicio ).Days + 1;

                        dtInicio = dtInicioProximoPeriodo.Date;
                        dtTermino = ausencia.DtInicio.Date.AddDays( -1 );

                        if(dtTermino > dtInicio)
                        {
                            key = String.Format( "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", dtInicio, dtTermino );

                            if(!periodosHash.ContainsKey( key ))
                            {
                                new ColaboradorPeriodoAquisitivo( Session )
                                {
                                    Colaborador = this,
                                    DtInicio = dtInicio,
                                    DtTermino = dtTermino,
                                    NbFeriasPlanejadas = 0
                                }.Save();
                            }
                            else
                            {
                                periodosHash.Remove( key );
                            }

                            dtInicioProximoPeriodo = dtTermino.AddDays( 1 );
                        }

                        // Remoção do Afastamento
                        ausencias.Remove( ausencia );

                        if(ausencias.Count > 0)
                        {
                            ausencia = ausencias[ausencias.Count - 1];
                            dtInicioProximoPeriodo = dtTerminoAusencia.Date.AddDays( 1 );
                        }
                        else
                        {
                            ausencia = null;
                        }
                    }

                    // Término da ausência
                    dtInicio = dtTerminoAusencia.Date.AddDays( 1 );
                    dtTermino = dtTerminoPeriodo.AddYears( qtdeAusencia / 360 ).AddDays( qtdeAusencia % 360 );
                    if(dtTermino > dtInicio)
                    {
                        key = String.Format( "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", dtInicio, dtTermino );

                        if(!periodosHash.ContainsKey( key ))
                        {
                            new ColaboradorPeriodoAquisitivo( Session )
                            {
                                Colaborador = this,
                                DtInicio = dtInicio,
                                DtTermino = dtTermino,
                                NbFeriasPlanejadas = 0
                            }.Save();
                        }
                        else
                        {
                            periodosHash.Remove( key );
                        }

                        dtInicioProximoPeriodo = dtTermino.AddDays( 1 );
                    }
                }
                // se não houver ausência, o término é 1 ano depois do início
                else
                {
                    key = String.Format( "{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", dtInicioProximoPeriodo, dtTerminoPeriodo );

                    if(!periodosHash.ContainsKey( key ))
                    {
                        new ColaboradorPeriodoAquisitivo( Session )
                        {
                            Colaborador = this,
                            DtInicio = dtInicioProximoPeriodo.Date,
                            DtTermino = dtTerminoPeriodo,
                            NbFeriasPlanejadas = 0
                        }.Save();
                    }
                    else
                    {
                        periodosHash.Remove( key );
                    }

                    dtInicioProximoPeriodo = dtInicioProximoPeriodo.AddYears( 1 );
                }
            }

            // Apagando os períodos que ficaram no lixo
            foreach(DictionaryEntry periodoDic in periodosHash)
            {
                ( (ColaboradorPeriodoAquisitivo)periodoDic.Value ).Delete();
            }
        }

        /// <summary>
        /// Cria roles pro usuário
        /// </summary>
        public void AssociacaoRoleUser()
        {
            XPCollection<Role> roles = new XPCollection<Role>( Session );


            if(roles.Count > 0)
            {
                Usuario.Roles.Add( roles[0] );
            }
            else
            {
                Usuario.Roles.Add( new Role( Session ) );
            }
        }

        /// <summary>
        /// Lista de possíveis superiores imediatos usados
        /// no combo do detail
        /// </summary>
        [Browsable( false ), NonPersistent]
        public XPCollection<Colaborador> _GetListaSuperioresImediatosColaborador
        {
            get
            {
                return new XPCollection<Colaborador>( Session, CriteriaOperator.Parse( "Oid != ? AND Usuario.IsActive = true", Oid ) );
            }
        }

        /// <summary>
        /// Ao terminar de dar load no objeto
        /// </summary>
        protected override void OnLoaded()
        {
            base.OnLoaded();

            if(colaboradorOld == null)
            {
                colaboradorOld = MemberwiseClone() as Colaborador;
                colaboradorUserActiveOld = colaboradorOld.Usuario.IsActive;
            }

            if(ColaboradorUltimoFiltro == null)
            {
                ColaboradorUltimoFiltro = new ColaboradorUltimoFiltro( Session )
                {
                    LastPeriodoFilterPlanejamentoFerias = -1,
                    LastSituacaoFilterPlanejamentoFerias = -1,
                    LastSituacaoFilterSeot = new Guid(),
                    LastUsuarioFilterSeot = new Guid(),
                    LastSuperiorImediatoFilterPlanejamentoFerias = " ",
                    LastSituacaoFeriasFilterPlanejamentoFerias = " "
                };
            }
        }

        /// <summary>
        /// Conversão para String
        /// </summary>
        /// <returns>Nome Completo</returns>
        public override string ToString()
        {
            if(Usuario != null)
            {
                if(Usuario.FullName != null && !Usuario.FullName.Trim().Equals( string.Empty ))
                {
                    return Usuario.FullName;
                }
                else
                {
                    return Usuario.UserName;
                }
            }

            return string.Empty;
        }

        #endregion

        #region Construtores

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="session">Sessão</param>
        public Colaborador( Session session )
            : base( session )
        {
            // This constructor is used when an object is loaded from a persistent storage.
            // Do not place any code here or place it only when the IsLoading property is false:
            // if (!IsLoading){
            //    It is now OK to place your initialization code here.
            // }
            // or as an alternative, move your initialization code into the AfterConstruction method.
        }

        /// <summary>
        /// Chamado depois de construir o objeto
        /// </summary>
        public override void AfterConstruction()
        {
            base.AfterConstruction();

            // Setando o novo usuário
            Usuario = new User( Session )
            {
                ChangePasswordOnFirstLogon = true, // Escolher a senha no primeiro login
                IsActive = true // Usuário Ativo
            };

            ColaboradorUltimoFiltro = new ColaboradorUltimoFiltro( Session )
            {
                LastPeriodoFilterPlanejamentoFerias = -1,
                LastSituacaoFilterPlanejamentoFerias = -1,
                LastSituacaoFilterSeot = new Guid(),
                LastUsuarioFilterSeot = new Guid()
            };

            AssociacaoRoleUser();
        }

        #endregion
    }
}