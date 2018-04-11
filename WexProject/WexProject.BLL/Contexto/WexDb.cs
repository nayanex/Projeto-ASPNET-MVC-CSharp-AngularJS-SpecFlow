using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects;
using WexProject.BLL.Entities.Escopo;
using WexProject.BLL.Entities.Execucao;
using WexProject.BLL.Entities.Geral;
using WexProject.BLL.Entities.Mapping;
using WexProject.BLL.Entities.NovosNegocios;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.Qualidade;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Models.Custos;

namespace WexProject.BLL.Contexto
{

    public partial class WexDb : DbContext
    {

        public WexDb()
            : base( "Name=ConnectionString" )
        {
           // WexProject.BLL.Migrations.WexAutomaticMigration.Instance.ExecuteOnce();
        }

        public WexDb( string connectionString )
            : base( connectionString )
        {
        }

        /// <summary>         
        /// Construtor utilizado em testes unitários.         
        /// DbConnection seria a conexao fake para banco em memória.         
        /// </summary>         
        /// <param name="conexao"></param>         
        public WexDb( DbConnection conexao )
            : base( conexao, true )
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Beneficiado> Beneficiados { get; set; }
        public DbSet<Calendario> Calendarios { get; set; }
        public DbSet<CasoTeste> CasoTestes { get; set; }
        public DbSet<CasoTestePasso> CasoTestePassoes { get; set; }
        public DbSet<CasoTestePassoResultadoEsperado> CasoTestePassoResultadoEsperadoes { get; set; }
        public DbSet<CasoTestePassoResultadoEsperadoAnexo> CasoTestePassoResultadoEsperadoAnexoes { get; set; }
        public DbSet<CasoTestePassoResultadoEsperadoInformacaoAdicional> CasoTestePassoResultadoEsperadoInformacaoAdicionals { get; set; }
        public DbSet<CasoTestePreCondicao> CasoTestePreCondicaos { get; set; }
        public DbSet<CasoTestePreCondicaoAnexo> CasoTestePreCondicaoAnexoes { get; set; }
        public DbSet<CasoTestePreCondicaoInformacaoAdicional> CasoTestePreCondicaoInformacaoAdicionals { get; set; }
        public DbSet<CicloDesenv> CicloDesenvs { get; set; }
        public DbSet<CicloDesenvEstoria> CicloDesenvEstorias { get; set; }
        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<ColaboradorAfastamento> ColaboradorAfastamentoes { get; set; }
        public DbSet<ColaboradorPeriodoAquisitivo> ColaboradorPeriodoAquisitivoes { get; set; }
        public DbSet<ColaboradorUltimoFiltro> ColaboradorUltimoFiltroes { get; set; }
        public DbSet<Configuracao> Configuracaos { get; set; }
        public DbSet<ConfiguracaoDocumento> ConfiguracaoDocumentoes { get; set; }
        public DbSet<ConfiguracaoDocumentoSituacao> ConfiguracaoDocumentoSituacaos { get; set; }
        public DbSet<ConfiguracaoDocumentoSituacaoEmail> ConfiguracaoDocumentoSituacaoEmails { get; set; }
        public DbSet<ConfiguracaoDocumentoSituacaoEmailCc> ConfiguracaoDocumentoSituacaoEmailCcs { get; set; }
        public DbSet<ConfiguracaoDocumentoSituacaoEmailCco> ConfiguracaoDocumentoSituacaoEmailCcoes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Cronograma> Cronograma { get; set; }
        public DbSet<CronogramaTarefa> CronogramaTarefa { get; set; }
        public DbSet<Estoria> Estorias { get; set; }
        public DbSet<EstoriaCasoTeste> EstoriaCasoTestes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ferias> Ferias { get; set; }
        public DbSet<FeriasPlanejamento> FeriasPlanejamentoes { get; set; }
        public DbSet<FileData> FileDatas { get; set; }
        //public DbSet<ItemDeTrabalho> ItemDeTrabalho { get; set; }
        public DbSet<NotaFiscal> NotaFiscal { get; set; }
		public DbSet<LoteMaoDeObra> Lotes { get; set; }
		public DbSet<MaoDeObra> MaosDeObra { get; set; }
        public DbSet<ModalidadeFeria> ModalidadeFerias { get; set; }
        public DbSet<ModuleInfo> ModuleInfoes { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<MotivoCancelamento> MotivoCancelamentoes { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Pai> Pais { get; set; }
        public DbSet<Papel> Papels { get; set; }
        public DbSet<ParteInteressada> ParteInteressadas { get; set; }
        public DbSet<Party> Party { get; set; }
        public DbSet<PersistentPermission> PersistentPermissions { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }
        public DbSet<ProjetoColaboradorConfig> ProjetoColaboradorConfigs { get; set; }
        public DbSet<ProjetoParteInteressada> ProjetoParteInteressadas { get; set; }
        public DbSet<ProjetoUltimoFiltro> ProjetoUltimoFiltroes { get; set; }
        public DbSet<Requisito> Requisitoes { get; set; }
        public DbSet<RequisitoCasoTeste> RequisitoCasoTestes { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceResources_EventEvents> ResourceResources_EventEvents { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleBase> RoleBases { get; set; }
        public DbSet<Rubrica> Rubricas { get; set; }
        public DbSet<SituacaoPlanejamento> SituacaoPlanejamento { get; set; }
        public DbSet<SolicitacaoOrcamento> SolicitacaoOrcamentoes { get; set; }
        public DbSet<SolicitacaoOrcamentoHistorico> SolicitacaoOrcamentoHistoricoes { get; set; }
        public DbSet<Solicitante> Solicitantes { get; set; }
        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<TarefaHistoricoTrabalho> TarefaHistoricoTrabalho { get; set; }
        public DbSet<TarefaLogAlteracao> TarefaLogAlteracao { get; set; }
        public DbSet<TarefaResponsaveis> TarefaResponsaveis { get; set; }
        public DbSet<TipoAfastamento> TipoAfastamentoes { get; set; }
        public DbSet<TipoEscopo> TipoEscopoes { get; set; }
        public DbSet<TipoSolicitacao> TipoSolicitacaos { get; set; }
        public DbSet<User> Usuario { get; set; }
        public DbSet<UserUsers_RoleRoles> UserUsers_RoleRoles { get; set; }
        public DbSet<XPObjectType> XPObjectTypes { get; set; }
        public DbSet<CronogramaUltimaSelecao> CronogramaUltimaSelecao { get; set; }
        public DbSet<CronogramaColaboradorConfig> CronogramaColaboradorConfig { get; set; }
        public DbSet<CronogramaCapacidadePlan> CronogramaCapacidadePlan { get; set; }
        public DbSet<Projeto> Projetos { get; set; }
        public DbSet<ClasseProjeto> ClassesProjetos { get; set; }
        public DbSet<TipoProjeto> TiposProjetos { get; set; }
        public DbSet<Aditivo> Aditivos { get; set; }
        public DbSet<EmpresaInstituicao> EmpresasInstituicoes { get; set; }
        public DbSet<ProjetoCliente> ProjetosClientes { get; set; }
        public DbSet<AditivoPatrocinador> AditivosPatrocinadores { get; set; }
        public DbSet<AditivoCentroCusto> AditivosCentrosCusto { get; set; }
        public DbSet<CentroCusto> CentrosCusto { get; set; }
        public DbSet<TipoRubrica> TiposRubrica { get; set; }
        public DbSet<RubricaMes> RubricaMeses { get; set; }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<TarefaHistoricoEstimativa> TarefaHistoricoEstimativa { get; set; }

        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add( new AddressMap() );
            modelBuilder.Configurations.Add( new BeneficiadoMap() );
            modelBuilder.Configurations.Add( new CalendarioMap() );
            modelBuilder.Configurations.Add( new CargoMap() );
            modelBuilder.Configurations.Add( new CasoTesteMap() );
            modelBuilder.Configurations.Add( new CasoTestePassoMap() );
            modelBuilder.Configurations.Add( new CasoTestePassoResultadoEsperadoMap() );
            modelBuilder.Configurations.Add( new CasoTestePassoResultadoEsperadoAnexoMap() );
            modelBuilder.Configurations.Add( new CasoTestePassoResultadoEsperadoInformacaoAdicionalMap() );
            modelBuilder.Configurations.Add( new CasoTestePreCondicaoMap() );
            modelBuilder.Configurations.Add( new CasoTestePreCondicaoAnexoMap() );
            modelBuilder.Configurations.Add( new CasoTestePreCondicaoInformacaoAdicionalMap() );
            modelBuilder.Configurations.Add( new CicloDesenvMap() );
            modelBuilder.Configurations.Add( new CicloDesenvEstoriaMap() );
            modelBuilder.Configurations.Add( new ColaboradorMap() );
            modelBuilder.Configurations.Add( new ColaboradorAfastamentoMap() );
            modelBuilder.Configurations.Add( new ColaboradorPeriodoAquisitivoMap() );
            modelBuilder.Configurations.Add( new ColaboradorUltimoFiltroMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoDocumentoMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoDocumentoSituacaoMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoDocumentoSituacaoEmailMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoDocumentoSituacaoEmailCcMap() );
            modelBuilder.Configurations.Add( new ConfiguracaoDocumentoSituacaoEmailCcoMap() );
            modelBuilder.Configurations.Add( new CountryMap() );
            modelBuilder.Configurations.Add( new CronogramaMap() );
            modelBuilder.Configurations.Add( new CronogramaTarefaMap() );
            modelBuilder.Configurations.Add( new EmpresaInstituicaoMap() );
            modelBuilder.Configurations.Add( new EstoriaMap() );
            modelBuilder.Configurations.Add( new EstoriaCasoTesteMap() );
            modelBuilder.Configurations.Add( new EventMap() );
            modelBuilder.Configurations.Add( new FeriasMap() );
            modelBuilder.Configurations.Add( new FeriasPlanejamentoMap() );
            modelBuilder.Configurations.Add( new FileDataMap() );
            //modelBuilder.Configurations.Add(new ItemDeTrabalhoMap());
            modelBuilder.Configurations.Add( new ModalidadeFeriaMap() );
            modelBuilder.Configurations.Add( new ModuleInfoMap() );
            modelBuilder.Configurations.Add( new ModuloMap() );
            modelBuilder.Configurations.Add( new MotivoCancelamentoMap() );
            modelBuilder.Configurations.Add( new NoteMap() );
            modelBuilder.Configurations.Add( new PaiMap() );
            modelBuilder.Configurations.Add( new PapelMap() );
            modelBuilder.Configurations.Add( new ParteInteressadaMap() );
            modelBuilder.Configurations.Add( new PartyMap() );
            modelBuilder.Configurations.Add( new PersistentPermissionMap() );
            modelBuilder.Configurations.Add( new PersonMap() );
            modelBuilder.Configurations.Add( new PhoneNumberMap() );
            modelBuilder.Configurations.Add( new PhoneTypeMap() );
            modelBuilder.Configurations.Add( new ProjetoMap() );
            modelBuilder.Configurations.Add( new ProjetoClienteMap() );
            modelBuilder.Configurations.Add( new ProjetoColaboradorConfigMap() );
            modelBuilder.Configurations.Add( new ProjetoParteInteressadaMap() );
            modelBuilder.Configurations.Add( new ProjetoUltimoFiltroMap() );
            modelBuilder.Configurations.Add( new RequisitoMap() );
            modelBuilder.Configurations.Add( new RequisitoCasoTesteMap() );
            modelBuilder.Configurations.Add( new ResourceMap() );
            modelBuilder.Configurations.Add( new ResourceResources_EventEventsMap() );
            modelBuilder.Configurations.Add( new RoleMap() );
            modelBuilder.Configurations.Add( new RoleBaseMap() );
            modelBuilder.Configurations.Add( new SituacaoPlanejamentoMap() );
            modelBuilder.Configurations.Add( new SolicitacaoOrcamentoMap() );
            modelBuilder.Configurations.Add( new SolicitacaoOrcamentoHistoricoMap() );
            modelBuilder.Configurations.Add( new SolicitanteMap() );
            modelBuilder.Configurations.Add( new TarefaMap() );
            modelBuilder.Configurations.Add( new TarefaHistoricoTrabalhoMap() );
            modelBuilder.Configurations.Add( new TarefaLogAlteracaoMap() );
            //modelBuilder.Configurations.Add(new TarefaResponsaveisMap());
            modelBuilder.Configurations.Add( new TipoAfastamentoMap() );
            modelBuilder.Configurations.Add( new TipoEscopoMap() );
            modelBuilder.Configurations.Add( new TipoSolicitacaoMap() );
            modelBuilder.Configurations.Add( new UserMap() );
            modelBuilder.Configurations.Add( new UserUsers_RoleRolesMap() );
            modelBuilder.Configurations.Add( new XPObjectTypeMap() );
        }

        /// <summary>
        /// Retornar o ObjectContext do contexto atual
        /// </summary>
        /// <returns></returns>
        public ObjectContext GetObjectContext()
        {
            return ( (IObjectContextAdapter)this ).ObjectContext;
        }

        /// <summary>
        /// Retornar o ObjectStateManager do contexto atual
        /// </summary>
        /// <returns></returns>
        public ObjectStateManager GetObjectStateManager()
        {
            return GetObjectContext().ObjectStateManager;
        }
    }
}