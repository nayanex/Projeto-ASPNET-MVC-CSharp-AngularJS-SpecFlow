using System;
using System.Collections.Generic;
using System.Linq;
using WexProject.Library.Libs.Delegates;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Geral;
using System.Drawing;
using WexProject.BLL.Shared.Domains.Planejamento;
using System.Collections;
using WexProject.Schedule.Library.ServiceUtils.Interfaces;
using System.Windows.Forms;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.Stubs
{
    public class PlanejamentoServiceUtilStub : IPlanejamentoServiceUtil
    {
        #region Atributos auxiliares (Testes)
        public  List<SituacaoPlanejamentoDTO> situacoesPlanejamentoAtivas;
        public  List<SituacaoPlanejamentoDTO> situacoesPlanejamentoInativas;
        public  SituacaoPlanejamentoDTO situacaoPlanejamentoPadrao;
        public  ColaboradorDto colaboradorLogado;
        public  CronogramaDto cronogramaSelecionado;
        public  List<CronogramaColaboradorConfigDto> colaboradoresConfig;
        public  List<CronogramaTarefaGridItem> tarefas;
        public  List<ColaboradorDto> colaboradoresResponsaveis;
        public  List<CronogramaDto> cronogramas;
        public static bool CriarCronogramaSeNomeNaoExistir { get; set; }

        #region Variáveis para gerenciar as teclas de atalho das situações planejamento
        /// <summary>
        /// armazenar o valor numérico da primeira tecla de atalho das situações planejamento
        /// </summary>
        public const int seedShortcut = 196657; //Valor do atalho Ctrl+Shift+1

        /// <summary>
        /// armazenar a quantidade de atalhos atribuidos
        /// </summary>
        public static int contadorAtalhos = 0; 
        #endregion

        #endregion

        #region Propriedades Publicas
        /// <summary>
        /// Propriedade que deve ser preenchida para simular o retorno de um inicializador de estimativa
        /// </summary>
        public InicializadorEstimativaDto RetornoInicializadorEstimativa { get; set; }

        /// <summary>
        /// Propriedade que deve ser preenchida para simular o retorno de um inicializador de estimativa
        /// </summary>
        public InicializadorEstimativaDto RetornoUltimaTarefaDiaColaborador { get; set; }

        /// <summary>
        /// Propriedade que deve ser preenchida para simular o retorno do ultimo historico trabalhado pelo colaborador
        /// </summary>
        public TarefaHistoricoTrabalhoDto RetornoUltimaTarefaHistoricoTrabalho { get; set; } 
        #endregion

        public  void InicializarDados()
        {
            situacoesPlanejamentoAtivas = new List<SituacaoPlanejamentoDTO>();
            situacoesPlanejamentoInativas = new List<SituacaoPlanejamentoDTO>();
            CriarSituacoesPlanejamento();
            SelecionarSituacaoPlanejamentoPadrao();
            CriarCronogramas();
            SelecionarCronograma( 1 );
            tarefas = new List<CronogramaTarefaGridItem>();
            CriarColaboradores();
            CriarCronogramaColaboradoresConfig();
            RetornoInicializadorEstimativa = new InicializadorEstimativaDto();
            RetornoUltimaTarefaHistoricoTrabalho = new TarefaHistoricoTrabalhoDto();
            RetornoUltimaTarefaDiaColaborador = new InicializadorEstimativaDto();
        }

        #region Métodos auxiliares (Testes)
        /// <summary>
        /// Criar situações planejamento
        /// </summary>
        public  void CriarSituacoesPlanejamento()
        {
            situacoesPlanejamentoAtivas = new List<SituacaoPlanejamentoDTO>();
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento("Não Iniciado",CsTipoPlanejamento.Planejamento));
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( "Em Andamento", CsTipoPlanejamento.Execução ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( "Cancelado", CsTipoPlanejamento.Cancelamento ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( "Impedido", CsTipoPlanejamento.Impedimento ) );
            situacoesPlanejamentoAtivas.Add( CriarSituacaoPlanejamento( "Pronto", CsTipoPlanejamento.Encerramento ) );
        }
        /// <summary>
        /// Criar os colaboradores
        /// </summary>
        public  void CriarColaboradores()
        {
            string[] colaboradores = new string[] { "gabriel.matos", "anderson.lins", "anderlan.castro", "alexandre.amorim" };
            colaboradoresResponsaveis = new List<ColaboradorDto>();
            foreach(string colaborador in colaboradores)
            {
                colaboradoresResponsaveis.Add( CriarColaboradorDto( colaborador ) );
            }
            colaboradorLogado = colaboradoresResponsaveis.FirstOrDefault();
        }

        /// <summary>
        /// Método para alterar o colaborador logado do ambiente de teste
        /// </summary>
        /// <param name="login"></param>
        public void SelecionarColaboradorLogado(string login) 
        {
            colaboradorLogado = colaboradoresResponsaveis.FirstOrDefault( o => o.Login.Equals( login ) );
        }

        /// <summary>
        /// Criar as configuracoes dos colaboradores para o cronograma selecionado
        /// </summary>
        public  void CriarCronogramaColaboradoresConfig()
        {
            colaboradoresConfig = new List<CronogramaColaboradorConfigDto>();

            foreach(ColaboradorDto colaborador in colaboradoresResponsaveis)
            {
                colaboradoresConfig.Add( CriarConfig( colaborador ) );
            }
        }

        /// <summary>
        /// Criar um cronograma colaborador config
        /// </summary>
        /// <param name="colaborador"></param>
        /// <returns></returns>
        public  CronogramaColaboradorConfigDto CriarConfig( ColaboradorDto colaborador )
        {
            Random rd = new Random();
            CronogramaColaboradorConfigDto config = new CronogramaColaboradorConfigDto()
            {
                OidCronograma = cronogramaSelecionado.Oid,
                OidColaborador = colaborador.OidColaborador,
                Login = colaborador.Login,
                NomeCompletoColaborador = colaborador.TxNomeCompletoColaborador,
                Cor = Color.FromArgb( rd.Next( 255 ), rd.Next( 255 ), rd.Next( 255 ) ).ToArgb().ToString()
            };
            return config;
        }

        /// <summary>
        /// Selecionar um cronograma 
        /// </summary>
        /// <param name="num"></param>
        public  void SelecionarCronograma( int num )
        {
            num--;
            if(num >= cronogramas.Count)
                num = cronogramas.Count - 1;
            if(num < 0)
                num = 0;
            cronogramaSelecionado = cronogramas[num];
        }

        /// <summary>
        /// Selecionar uma situação planejamento padrao
        /// </summary>
        public  void SelecionarSituacaoPlanejamentoPadrao()
        {
            situacaoPlanejamentoPadrao = situacoesPlanejamentoAtivas.FirstOrDefault(o=>o.CsTipo.Equals(CsTipoPlanejamento.Planejamento));
        }

        /// <summary>
        /// Criar cronogramas
        /// </summary>
        public  void CriarCronogramas()
        {
            cronogramas = new List<CronogramaDto>();
            for(int i = 0; i < 5; i++)
            {
                cronogramas.Add( CriarCronograma( i + 1 ) );
            }
        }

        /// <summary>
        /// Criar uma nova situação planejamento
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static  SituacaoPlanejamentoDTO CriarSituacaoPlanejamento(string Descricao, CsTipoPlanejamento tipo, bool ativo = true )
        {
            Random random = new Random();
            CsTipoSituacaoPlanejamento statusSituacao;
            
            if(ativo)
                statusSituacao = CsTipoSituacaoPlanejamento.Ativo;
            else
                statusSituacao = CsTipoSituacaoPlanejamento.Inativo;
            SituacaoPlanejamentoDTO situacao = new SituacaoPlanejamentoDTO()
            {
                Oid = Guid.NewGuid(),
                TxDescricao = Descricao,
                CsPadrao = (CsPadraoSistema)random.Next( 0, 1 ),
                CsSituacao = statusSituacao,
                CsTipo = tipo,
                TxKeys = ((Shortcut)(contadorAtalhos + seedShortcut )).ToString(),
                KeyPress = (Shortcut)(contadorAtalhos + seedShortcut ),
                BlImagemSituacaoPlanejamento = null
            };
            contadorAtalhos++;
            return situacao;
        }

        /// <summary>
        /// Criar um novo cronograma
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public  CronogramaDto CriarCronograma( int num )
        {
            string nomeCronograma = string.Format( "WexCronograma {0}", num );
            return CriarCronograma( nomeCronograma );
        }

        /// <summary>
        /// Criar um novo cronograma
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public  CronogramaDto CriarCronograma( string nomeCronograma )
        {
            return CriarCronograma( nomeCronograma, DateTime.Now );
        }

        /// <summary>
        /// Criar um novo cronograma
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public CronogramaDto CriarCronograma( string nomeCronograma , DateTime? dataInicio = null, DateTime? dataTermino = null )
        {
            var dtInicio = dataInicio.HasValue ? dataInicio.Value : DateTime.Now;
            var dtFinal = dataTermino.HasValue ? dataTermino.Value.Date <= dtInicio.Date ? dtInicio.Date.AddDays( 15 ) : dataTermino.Value : DateTime.Now.AddDays( 15 );
            CronogramaDto cronograma = new CronogramaDto()
            {
                DtInicio = dtInicio,
                DtFinal = dtFinal,
                Oid = Guid.NewGuid(),
                OidSituacaoPlanejamento = situacaoPlanejamentoPadrao.Oid,
                TxDescricao = nomeCronograma,
                TxDescricaoSituacaoPlanejamento = situacaoPlanejamentoPadrao.TxDescricao
            };
            cronogramas.Add( cronograma );
            return cronograma;
        }




        /// <summary>
        /// Criar um novo colaborador
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public  ColaboradorDto CriarColaboradorDto( string login ,bool adicionarAutomaticoAHash = false)
        {
            ColaboradorDto colaborador = new ColaboradorDto()
            {
                Login = login,

            };
            string[] nomes = login.Split( '.' );
            foreach(string nome in nomes)
            {
                colaborador.TxNomeCompletoColaborador += nome + " ";
            }
            colaborador.OidUsuario = Guid.NewGuid();
            colaborador.OidColaborador = Guid.NewGuid();
            colaborador.TxMatriculaColaborador = new Random().Next( 100, 999 ).ToString();
            colaborador.TxNomeCompletoColaborador.Trim();
            if(adicionarAutomaticoAHash)
                colaboradoresResponsaveis.Add( colaborador );
            return colaborador;
        }

        /// <summary>
        /// Método para criar uma nova tarefa
        /// </summary>
        /// <param name="oidCronograma"></param>
        /// <param name="oidSituacaoPlanejamento"></param>
        /// <param name="login"></param>
        /// <param name="CsLinhaBase"></param>
        /// <returns></returns>
        public CronogramaTarefaGridItem CriarTarefa(string descricao = null)
        {
            if(descricao == null)
                descricao = string.Format( "Tarefa {0}",(tarefas.Count + 1) ) ;
            CronogramaTarefaGridItem tarefa = new CronogramaTarefaGridItem()
            {
                TxDescricaoTarefa = descricao,
                TxDescricaoColaborador = "",
                NbEstimativaInicial = 3,
                NbEstimativaRestante = new TimeSpan(2,0,0).Ticks,
                NbID = 1,
                NbRealizado = new TimeSpan( 1, 0, 0 ).Ticks,
                OidCronograma = cronogramaSelecionado.Oid,
                OidSituacaoPlanejamentoTarefa = situacaoPlanejamentoPadrao.Oid,
                OidTarefa = Guid.NewGuid(),
                OidCronogramaTarefa = Guid.NewGuid(),
                TxAtualizadoPor = colaboradorLogado.Login,
                CsLinhaBaseSalva = false,
                DtInicio = DateTime.Now
            };
            tarefas.Add( tarefa );
            return tarefa;
        }

        public static TarefaHistoricoTrabalhoDto CriarTarefaHistoricoTrabalho() 
        {
            TarefaHistoricoTrabalhoDto historico = new TarefaHistoricoTrabalhoDto() 
            {
                DtRealizado = DateTime.Now, 
                NbHoraFinal = new TimeSpan( 0 ), 
                NbHoraInicio = new TimeSpan( 0 ), 
                NbRestante = new TimeSpan( 0 ), 
                OidTarefaHistorico = Guid.NewGuid(),
                OidTarefa = Guid.NewGuid()
            };
            return historico;
        }

        #endregion

        public PlanejamentoServiceUtilStub()
        {
            InicializarDados();
        }

        #region Comportamentos stub

        #region Eventos
        public event RespostaAsyncServiceHandler AoCompletarMovimentacaoTarefa;

        public event RespostaAsyncServiceHandler AoCompletarSolicitacaoCriarNovaTarefa;

        public event RespostaAsyncServiceHandler AoCompletarSolicitacaoExclusaoTarefas;
        #endregion

        #region Métodos stub serviço
        public List<ColaboradorDto> ListarColaboradores()
        {
            return colaboradoresResponsaveis;
        }

        public CronogramaDto ConsultarCronogramaPorNome( string nomeCronograma)
        {
            CronogramaDto cronograma = cronogramas.FirstOrDefault( o => o.TxDescricao.Equals( nomeCronograma ) );
            if(CriarCronogramaSeNomeNaoExistir)
            {
                CriarCronogramaSeNomeNaoExistir = false;
                if(cronograma == null)
                    return CriarCronograma( nomeCronograma );
            }
            return cronograma;
        }

        public List<CronogramaDto> ListarCronogramas()
        {
            return cronogramas;
        }

        public CronogramaTarefaGridItem ConsultarCronogramaTarefaPorOid( Guid oidCronogramaTarefa )
        {
            return tarefas.FirstOrDefault( o => o.OidCronogramaTarefa.Equals( oidCronogramaTarefa ) );
        }

        public CronogramaTarefaGridItem GetCronogramaTarefaPorDescricao(string descricao,Guid? oidCronograma = null) 
        {
            if(tarefas == null)
                return null;
            if(oidCronograma == null)
                oidCronograma = cronogramaSelecionado.Oid;
            return tarefas.FirstOrDefault(o=>o.OidCronograma.Equals(oidCronograma) && o.TxDescricaoTarefa.ToLower().Contains(descricao.ToLower()));
        }

        public List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOid( List<string> oidCronogramaTarefas )
        {
            return tarefas.Where( o => oidCronogramaTarefas.Contains( o.OidCronogramaTarefa.ToString() ) ).ToList();
        }

        public List<CronogramaTarefaGridItem> ConsultarCronogramaTarefasPorOidCronograma( Guid oidCronograma )
        {
            return tarefas.Where( o => o.OidCronograma.Equals( oidCronograma ) ).ToList();
        }

        public InicializadorEstimativaDto ConsultarHorarioUltimaTarefaDiaColaborador( string login, DateTime data )
        {
            return RetornoUltimaTarefaDiaColaborador;
        }

        public InicializadorEstimativaDto ConsultarInicializadorEstimativaInicialColaborador( string login )
        {
            return RetornoInicializadorEstimativa;
        }

        public SituacaoPlanejamentoDTO ConsultarSituacaoPlanejamentoPadrao()
        {
            return situacaoPlanejamentoPadrao;
        }

        public List<SituacaoPlanejamentoDTO> ConsultarSituacoesInativas()
        {
            return situacoesPlanejamentoInativas;
        }

        public List<SituacaoPlanejamentoDTO> ConsultartSituacoesPlanejamento()
        {
            return situacoesPlanejamentoAtivas;
        }

        public List<SituacaoPlanejamentoDTO> ConsultarSituacoesPlanejamentoTipadas()
        {
            return situacoesPlanejamentoAtivas;
        }

        public TarefaHistoricoTrabalhoDto ConsultarTarefaHistoricoTrabalhoAtual( Guid oidTarefa )
        {
            return RetornoUltimaTarefaHistoricoTrabalho;
        }

        public List<TarefaLogAlteracaoDto> ConsultarTarefaLogAlteracaoPorOid( string oidTarefa )
        {
            throw new NotImplementedException();
        }

        public CronogramaDto ConsultarUltimoCronogramaSelecionado( string login )
        {
            return cronogramaSelecionado;
        }

        public bool EditarCronograma( CronogramaDto dto )
        {
            if(cronogramas == null)
                return false;
            CronogramaDto cronograma = cronogramas.FirstOrDefault( o => o.Oid == dto.Oid );
            if(cronograma == null)
                return false;
            cronograma.TxDescricao = dto.TxDescricao;
            return true;
        }

        public CronogramaDto CriarCronogramaPadrao()
        {
            throw new NotImplementedException();
        }

        public void CriarHistoricoTarefa( Guid oidTarefa, string login, TimeSpan nbHoraRealizado, DateTime dtRealizado, TimeSpan nbHoraInicial, TimeSpan nbHoraFinal, string txComentario, TimeSpan nbHoraRestante, Guid oidSituacaoPlanejamento, string txJustificativaReducao )
        {
            throw new NotImplementedException();
        }

        public void CriarNovaTarefa( Guid oidCronograma, string txDescricao = "", string oidSituacao = "", DateTime dtInicio = new DateTime(), string responsaveis = "", string login = "", string txObservacao = "", Int16 EstimativaInicialHora = 0, short nbIDReferencia = 0 )
        {
            throw new NotImplementedException();
        }

        public Hashtable EditarTarefa( CronogramaTarefaDto tarefaEditada )
        {
            Hashtable hash = new Hashtable();
            CronogramaTarefaGridItem tarefa = tarefas.FirstOrDefault( o => o.OidCronogramaTarefa.ToString().Equals( tarefaEditada.OidTarefa ) );
            if(tarefa != null) 
            {
                hash.Add( "EdicaoStatus", true );
                hash.Add( "DtAtualizadoEm", DateTime.Now );
                hash.Add( "TxAtualizadoPor", tarefaEditada.TxAtualizadoPor );
                tarefa.TxDescricaoTarefa = tarefaEditada.TxDescricaoTarefa;
                tarefa.OidSituacaoPlanejamentoTarefa = tarefaEditada.OidSituacaoPlanejamentoTarefa;
                tarefa.TxAtualizadoPor = tarefaEditada.TxAtualizadoPor;
                tarefa.TxObservacaoTarefa = tarefaEditada.TxObservacaoTarefa;
                tarefa.TxDescricaoColaborador = tarefaEditada.TxDescricaoColaborador;
                tarefa.NbRealizado = tarefaEditada.NbRealizado;
                tarefa.NbEstimativaRestante = tarefaEditada.NbEstimativaRestante;
                tarefa.NbEstimativaInicial = tarefaEditada.NbEstimativaInicial;
                tarefa.CsLinhaBaseSalva = tarefaEditada.CsLinhaBaseSalva;
				tarefa.DtInicio = tarefaEditada.DtInicio;
            }
            else
                hash.Add( "EdicaoStatus", true );
            return hash;
        }

        public bool ExcluirCronograma( string oidCronograma )
        {
            throw new NotImplementedException();
        }

        public void ExcluirTarefas( List<Guid> oidTarefas, Guid oidCronograma )
        {
            throw new NotImplementedException();
        }

        public void MoverTarefa( Guid oidCronogramaTarefaSelecionada, short nbIDDestino )
        {
            throw new NotImplementedException();
        }

        public void SalvarUltimoCronogramaSelecionado( string login, string oidCronograma )
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion


        public List<CronogramaColaboradorConfigDto> ConsultarConfigUsuariosConectados( string[] logins, string oidCronograma )
        {
            return colaboradoresConfig.Where(o=> logins.Contains(o.Login)).ToList();
        }

        public void EscolherCorColaborador( string login, string oidCronograma )
        {
        }

        public BurndownGraficoDto ConsultarDadosGraficoBurndown( Guid oidCronograma )
        {
            //Implementar  a funcionalidade quando necessário no ambiente de testes unitários
            return null;
        }
    }
}
