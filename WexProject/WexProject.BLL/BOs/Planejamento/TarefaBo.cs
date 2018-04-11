using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Concorrencia;
using WexProject.Library.Libs.DataHora;
using System.Data.Objects;
using WexProject.BLL.DAOs.Planejamento;
using WexProject.BLL.DAOs.RH;
using WexProject.BLL.Contexto;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.BLL.BOs.Planejamento
{
    public class TarefaBo
    {
        #region TarefaConstantes
        /// <summary>
        /// Constante Armazenando o Nome da Tabela TarefasResposnsaveis utilizada nas seções de
        /// </summary>
        public const string NOME_TABELA_RESPONSAVEIS = "TarefaResponsaveis";
        #endregion

        /// <summary>
        /// Método responsável por excluir uma tarefa.
        /// </summary>
        /// <param name="contexto">Sessão Corrente</param>
        /// <param name="oidTarefas">Lista com os oids das tarefas a serem excluidas</param>
        /// <returns>Lista contendo os oids das tarefas que foram excluidas.</returns>
        public static List<Guid> ExcluirTarefa( WexDb contexto, List<Guid> oidTarefas )
        {
            List<Guid> oidTarefasExcluidas = new List<Guid>();

            foreach(Guid oidTarefa in oidTarefas)
            {
                if(TarefaDao.ExcluirTarefa( oidTarefa ))
                {
                    oidTarefasExcluidas.Add( oidTarefa );
                }
            }
            return oidTarefasExcluidas;
        }

        /// <summary>
        /// Método responsável por excluir uma tarefa.
        /// </summary>
        /// <param name="contexto">Sessão Corrente</param>
        /// <param name="oidTarefa">Lista com os oids das tarefas a serem excluidas</param>
        /// <returns>Lista contendo os oids das tarefas que foram excluidas.</returns>
        public static Guid ExcluirTarefa( WexDb contexto, Guid oidTarefa )
        {
            return TarefaDao.ExcluirTarefa( oidTarefa ) ? oidTarefa : new Guid();
        }

        /// <summary>
        /// Método responsável por editar uma tarefa
        /// </summary>
        /// <param name="contexto">Sessão corrente</param>
        /// <param name="oidCronogramaTarefa">Oid (Guid) da tarefa a ser editada</param>
        /// <param name="txDescricao">Descrição da tarefa alterada</param>
        /// <param name="oidSituacaoPlanejamento">Oid (Guid) da tarefa editada</param>
        /// <param name="dataInicio">Data de Inicio da tarefa editada</param>
        /// <param name="login">Login do usuário que editou a tarefa</param>
        /// <param name="txObservacao">Observação da tarefa editada</param>
        /// <param name="responsaveis">array de responsáveis pela tarefa</param>
        /// <param name="nbEstimativaInicial">Estimativa inicial da tarefa</param>
        /// <param name="nbEstimativaRestante">Estimativa restante da tarefa</param>
        /// <param name="nbRealizado">Horas realizadas da tarefa</param>
        /// <param name="csLinhaBaseSalva">Boolean afirmando se a tarefa foi salva a linda de base</param>
        /// <returns>Retorna uma Hash contendo dados de atualizado em, atualizado por e confirmação da edição</returns>
        public static Hashtable EditarTarefa( string oidCronogramaTarefa, string txDescricao, string oidSituacaoPlanejamento,
                                              string login, string txObservacao, string responsaveis,
                                              Int16 nbEstimativaInicial, TimeSpan nbEstimativaRestante, TimeSpan nbRealizado, bool csLinhaBaseSalva, DateTime dataInicio )
        {
            if(oidCronogramaTarefa == null || txDescricao == null || oidSituacaoPlanejamento == null ||
                login == null)
                throw new ArgumentException( "Os parâmetros OidCronogramaTarefa, TxDescricao, DataInício, SituacaoPlanejamento, login e TxObservação são obrigatórios." );

            Hashtable dadosEdicaoTarefa = new Hashtable();
            CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( Guid.Parse( oidCronogramaTarefa ), o => o.Tarefa.SituacaoPlanejamento, o => o.Tarefa.AtualizadoPor.Usuario.Person );

            if(cronogramaTarefa == null)
            {
                dadosEdicaoTarefa.Add( "EdicaoStatus", false );
                return dadosEdicaoTarefa;
            }

            Tarefa tarefaAntiga = cronogramaTarefa.Tarefa.Clone();
            Colaborador colaborador = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario.Person );
            SituacaoPlanejamento situacaoPlanejamento = SituacaoPlanejamentoDAO.ConsultarSituacaoPlanejamentoPorOid( Guid.Parse( oidSituacaoPlanejamento ) );

            if(colaborador != null)
            {
                cronogramaTarefa.Tarefa.OidAtualizadoPor = colaborador.Oid;
                cronogramaTarefa.Tarefa.AtualizadoPor = null;
            }

            if(situacaoPlanejamento != null)
            {
                cronogramaTarefa.Tarefa.OidSituacaoPlanejamento = situacaoPlanejamento.Oid;
                cronogramaTarefa.Tarefa.SituacaoPlanejamento = null;
            }


            cronogramaTarefa.Tarefa.CsLinhaBaseSalva = csLinhaBaseSalva;
            cronogramaTarefa.Tarefa.NbEstimativaInicial = nbEstimativaInicial;
            cronogramaTarefa.Tarefa.EstimativaRealizadoHora = nbRealizado;
            cronogramaTarefa.Tarefa.TxDescricao = txDescricao;
            cronogramaTarefa.Tarefa.TxObservacao = txObservacao;
            cronogramaTarefa.Tarefa.DtAtualizadoEm = DateTime.Now;
            cronogramaTarefa.Tarefa.TxResponsaveis = responsaveis;
            cronogramaTarefa.Tarefa.DtInicio = dataInicio;

            EstimarHorasRestantesSugeridaPorTipoSituacaoPlanejamento( cronogramaTarefa.Tarefa, nbEstimativaRestante.Ticks, situacaoPlanejamento );
            TarefaDao.SalvarTarefa( cronogramaTarefa.Tarefa );
            TarefaHistoricoEstimativaBo.CriarHistorico( DateUtil.ConsultarDataHoraAtual(), cronogramaTarefa.Tarefa.Oid, cronogramaTarefa.Tarefa.NbEstimativaRestante );
            TarefaLogAlteracaoBo.CriarLogTarefa( cronogramaTarefa.Tarefa, tarefaAntiga );

            dadosEdicaoTarefa.Add( "EdicaoStatus", true );
            dadosEdicaoTarefa.Add( "DtAtualizadoEm", cronogramaTarefa.Tarefa.DtAtualizadoEm );
            dadosEdicaoTarefa.Add( "TxAtualizadoPor", colaborador.NomeCompleto );

            return dadosEdicaoTarefa;
        }

        /// <summary>
        /// Método responsável por estimar e sugerir horas restantes por tipo de situação planejamento
        /// </summary>
        /// <param name="tarefa">tarefa em edição</param>
        /// <param name="horaRestantesTicks"> horas restantes</param>
        /// <param name="situacao">situação de planejamento da tarefa</param>
        private static void EstimarHorasRestantesSugeridaPorTipoSituacaoPlanejamento( Tarefa tarefa, double horaRestantesTicks, SituacaoPlanejamento situacao, bool criacaoHistoricoTrabalho = false )
        {
            if(tarefa == null || situacao == null)
                return;

            switch(situacao.CsTipo)
            {
                case CsTipoPlanejamento.Planejamento:
                    if(criacaoHistoricoTrabalho)
                    {
                        tarefa.NbEstimativaRestante = horaRestantesTicks;
                        return;
                    }
                        tarefa.NbEstimativaRestante = tarefa.NbEstimativaInicial.ToTicks();
                        break;
                case CsTipoPlanejamento.Cancelamento:
                    tarefa.NbEstimativaRestante = 0;
                    break;
                default:
                    tarefa.NbEstimativaRestante = horaRestantesTicks;
                    break;
            }
        }

        /// <summary>
        /// Método responsável por criar uma tarefa. Obs: Ela pode ser independente de projeto ou não.
        /// É usado pela classe CronogramaTarefa ou pode ser avulsa.
        /// </summary>
        /// <param name="txDescricaoTarefa">descrição da tarefa</param>
        /// <param name="txObservacaoTarefa">observação da tarefa</param>
        /// <param name="situacaoPlanejamento">situação da tarefaClone</param>
        /// <param name="responsavel">responsáveis pela tarefaClone</param>
        /// <param name="estimativaInicial">estimativa inicial para tarefa</param>
        /// <param name="dtInicio">data de início da tarefa</param>
        public static Tarefa CriarTarefa( string txDescricaoTarefa, SituacaoPlanejamento situacaoPlanejamento, DateTime dtInicio, string login, string txObservacaoTarefa = "", string responsaveis = null, Int16 estimativaInicial = 0 )
        {
            if(txDescricaoTarefa == null ||
                dtInicio == null ||
                situacaoPlanejamento == null ||
                login == null)
                throw new ArgumentException( "Os parâmetros txDescricao, dtInício, situacaoPlanejamento e login são obrigatórios." );

            var novaTarefa = new Tarefa
            {
                TxDescricao = txDescricaoTarefa,
                TxObservacao = String.Empty,
                TxResponsaveis = responsaveis,
                NbEstimativaInicial = estimativaInicial,
                NbEstimativaRestante = estimativaInicial.ToTicks(),
                DtInicio = dtInicio,
                OidSituacaoPlanejamento = situacaoPlanejamento.Oid,
                SituacaoPlanejamento = situacaoPlanejamento
            };

            Colaborador colaborador = ColaboradorDAO.ConsultarColaborador( login, o => o.Usuario.Person );

            if(colaborador != null)
            {
                novaTarefa.OidAtualizadoPor = colaborador.Oid;
                novaTarefa.AtualizadoPor = colaborador;
            }

            novaTarefa.DtAtualizadoEm = DateUtil.ConsultarDataHoraAtual();

            TarefaDao.SalvarTarefa( novaTarefa );
            TarefaHistoricoEstimativaBo.CriarHistorico( DateUtil.ConsultarDataHoraAtual(), novaTarefa.Oid, novaTarefa.NbEstimativaRestante );
            TarefaLogAlteracaoBo.CriarLogTarefa( novaTarefa, null );

            return novaTarefa;
        }

        /// <summary>
        /// Método responsável por, atribuir um colaborador à tarefa, caso ele não esteja como responsável pela mesma,
        /// atualizar a situação planejamento da tarefa e atualizar as estimativas da tarefa.
        /// </summary>
        /// <param name="oidTarefa">oid da tarefa</param>
        /// <param name="colaborador">Objeto Colaborador</param>
        /// <param name="situacaoPlanejamento">Objeto Situação Planejamento</param>
        /// <param name="nbHoraRealizado">Hora realizado da atividade</param>
        /// <param name="nbHoraRestante">Hora restante da atividade</param>
        public static void AtualizarDadosTarefa( Guid oidTarefa, Colaborador colaborador, SituacaoPlanejamento situacaoPlanejamento, TimeSpan nbHoraRealizado, TimeSpan nbHoraRestante, bool CsLinhaBaseSalva )
        {
            if(oidTarefa == null || oidTarefa == new Guid())
                throw new ArgumentException( "O parâmetro oidTarefa é  inválido" );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                Tarefa tarefa = TarefaDao.ConsultarTarefaPorOid( oidTarefa );

                if(tarefa == null)
                    return;
                //Atualiza a situação planejamento da tarefa e estimativas
                if(situacaoPlanejamento != null && !tarefa.OidSituacaoPlanejamento.Equals( situacaoPlanejamento.Oid ))
                {
                    tarefa.SituacaoPlanejamento = null;
                    tarefa.OidSituacaoPlanejamento = situacaoPlanejamento.Oid;
                }

                if(colaborador != null && !tarefa.OidAtualizadoPor.Equals( colaborador.Oid ))
                {
                    tarefa.AtualizadoPor = null;
                    tarefa.OidAtualizadoPor = colaborador.Oid;
                }
                EstimarHorasRestantesSugeridaPorTipoSituacaoPlanejamento( tarefa, nbHoraRestante.Ticks, situacaoPlanejamento, true );
                tarefa.EstimativaRealizadoHora += nbHoraRealizado;
                tarefa.DtAtualizadoEm = DateUtil.ConsultarDataHoraAtual();
                tarefa.CsLinhaBaseSalva = CsLinhaBaseSalva;

                //Atualiza responsáveis tarefa
                if(nbHoraRealizado.Ticks > 0)
                {
                    if(!String.IsNullOrEmpty( tarefa.TxResponsaveis ) || !String.IsNullOrWhiteSpace( tarefa.TxResponsaveis ))
                    {
                        List<string> ResponsaveisTarefa = new List<string>( tarefa.TxResponsaveis.Split( ',' ) );

                        if(colaborador != null)
                            if(!ResponsaveisTarefa.Contains( colaborador.NomeCompleto ))
                                tarefa.TxResponsaveis += "," + colaborador.NomeCompleto;
                    }
                    else
                        tarefa.TxResponsaveis += colaborador.NomeCompleto;
                }

                contexto.Tarefa.Attach( tarefa );
                contexto.Entry( tarefa ).State = EntityState.Modified;

                contexto.SaveChanges();

                TarefaHistoricoEstimativaBo.CriarHistorico( DateUtil.ConsultarDataHoraAtual(), tarefa.Oid, tarefa.NbEstimativaRestante );
            }
        }

        /// <summary>
        /// método responsável por salvar a linha de base de uma tarefa
        /// </summary>
        /// <param name="tarefa">tarefa a ter linha de base salva</param>
        public static void SalvarLinhaBaseTarefa( Tarefa tarefa )
        {
            if(tarefa.CsLinhaBaseSalva)
                return;
            tarefa.CsLinhaBaseSalva = true;
        }

        #region Factories

        /// <summary>
        /// Método responsável por construir um objeto TarefaEdicaoDto a partir de dados vindos de uma hash.
        /// </summary>
        /// <param name="dadosTarefa">Hash contendo os dados relevantes de uma tarefa que foi editada</param>
        /// <returns>Retorna um DTO com o data da atualização e quem atualizou a tarefa</returns>
        public static TarefaEdicaoDto EdicaoDtoFactory( Hashtable dadosTarefa )
        {
            TarefaEdicaoDto tarefaEdicao = new TarefaEdicaoDto()
            {
                EdicaoStatus = (bool)dadosTarefa["EdicaoStatus"],
                DtAtualizadoEm = (DateTime)dadosTarefa["DtAtualizadoEm"],
                TxAtualizadoPor = dadosTarefa["TxAtualizadoPor"] as string
            };

            return tarefaEdicao;
        }

        /// <summary>
        /// Método responsável por construir um objeto novaTarefaDto a partir de dados vindos de uma hash.
        /// </summary>
        /// <param name="dadosTarefa">Hash contendo os dados relevantes de uma tarefa que foi criada</param>
        /// <returns>Retorna um DTO com o Oid, NbId, lista de tarefas impactadas, data da atualização e quem atualizou a tarefa</returns>
        public static TarefaCriadaDto TarefaCriadaDtoFactory( CronogramaTarefa novaTarefa, List<CronogramaTarefa> impactadas, DateTime dataHoraAcao )
        {
            TarefaCriadaDto tarefa = new TarefaCriadaDto()
            {
                OidCronogramaTarefa = novaTarefa.Oid,
                OidCronograma = (Guid)novaTarefa.OidCronograma,
                OidTarefa = novaTarefa.Tarefa.Oid,
                NbIdTarefa = novaTarefa.NbID,
                TarefasImpactadas = impactadas.ToDictionary( o => o.Oid.ToString(), o => o.NbID ),
                CsLinhaBaseSalva = novaTarefa.Tarefa.CsLinhaBaseSalva,
                DtInicio = (DateTime)novaTarefa.Tarefa.DtInicio,
                NbEstimativaInicial = novaTarefa.Tarefa.NbEstimativaInicial,
                NbEstimativaRestante = novaTarefa.Tarefa.NbEstimativaRestante,
                NbRealizado = novaTarefa.Tarefa.NbRealizado,
                OidSituacaoPlanejamentoTarefa = novaTarefa.Tarefa.SituacaoPlanejamento.Oid,
                TxDescricaoColaborador = novaTarefa.Tarefa.TxResponsaveis,
                TxDescricaoSituacaoPlanejamentoTarefa = novaTarefa.Tarefa.SituacaoPlanejamento.TxDescricao,
                TxDescricaoTarefa = novaTarefa.Tarefa.TxDescricao,
                TxObservacaoTarefa = novaTarefa.Tarefa.TxObservacao,
                DtAtualizadoEm = (DateTime)novaTarefa.Tarefa.DtAtualizadoEm,
                TxAtualizadoPor = novaTarefa.Tarefa.AtualizadoPor.NomeCompleto,
                dataHoraAcao = dataHoraAcao,
            };

            return tarefa;
        }

        #endregion
    }
}
