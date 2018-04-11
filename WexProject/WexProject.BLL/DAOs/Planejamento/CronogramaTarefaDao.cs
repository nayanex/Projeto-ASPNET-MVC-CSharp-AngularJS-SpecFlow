using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.Library.Libs.Concorrencia;
using System.Data.Entity;
using WexProject.BLL.BOs.Planejamento;
using WexProject.BLL.Entities;
using WexProject.BLL.Contexto;
using System.Linq.Expressions;
using WexProject.BLL.Extensions.Entities;
using System.Data;

namespace WexProject.BLL.DAOs.Planejamento
{
    public class CronogramaTarefaDao
    {
        #region Consultas

        /// <summary>
        /// Método responsável por retornar o somatório das estimativas iniciais da tarefas de um determinado cronograma
        /// </summary>
        /// <param name="oidCronograma">oid de identificalção do cronograma</param>
        /// <returns></returns>
        public static double ConsultarTotalHorasPlanejadasCronogramaPorOid( Guid oidCronograma )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                var cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( oidCronograma, o => o.Tarefa );

                if(cronogramaTarefas == null || !cronogramaTarefas.Any())
                    return 0;

                return cronogramaTarefas.Sum( o => o.Tarefa.NbEstimativaInicial );
            }
        }

        /// <summary>
        /// Método responsável por buscar as tarefas que estão em determinado intervalo de NbIds.
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="oidCronograma">Oid do cronograma que se deseja encontrar os NbIds</param>
        /// <param name="nbIdInicio">NbId início do intervalo</param>
        /// <param name="nbIdFinal">NbId final do intervalo</param>
        /// <returns>Lista das tarefas que estão no intervalo</returns>
        public static List<CronogramaTarefa> ConsultarCronogramaTarefasPorIntervaloNbIDs( Guid oidCronograma, short nbIdInicio, short nbIdFinal, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(nbIdInicio <= 0 || nbIdFinal <= 0)
                    throw new ArgumentException( "Parâmetros inválidos." );
                return contexto.CronogramaTarefa.MultiploInclude( includes ).Where( o => o.NbID >= nbIdInicio && o.NbID <= nbIdFinal && o.OidCronograma == oidCronograma && !o.CsExcluido ).OrderBy( o => o.NbID ).ToList();
            }
        }

        /// <summary>
        /// Método responsável por buscar todas as tarefas que tiverem o NbID maior do que o ID que foi informado como referência.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma que se deseja encontrar as tarefas</param>
        /// <param name="nbIdReferencia">nbId referencia para encontrar as tarefas</param>
        /// <param name="includes">relacionamentos a serem carregados na consulta</param>
        /// <returns>Lista de tarefas que foram encontradas</returns>
        public static List<CronogramaTarefa> ConsultarCronogramaTarefasMaiorQueNbID( Guid oidCronograma, short nbIdReferencia, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(contexto == null || nbIdReferencia < 0)
                    throw new ArgumentException( "Parâmetros inválidos." );

                return contexto.CronogramaTarefa.MultiploInclude( includes ).Where( o => o.NbID > nbIdReferencia && o.OidCronograma == oidCronograma && !o.CsExcluido ).OrderBy( o => o.NbID ).ToList();
            }
        }

        /// <summary>
        /// Método responsável por buscar um CronogramaTarefa pelo NbID
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma que se deseja encontrar a tarefa</param>
        /// <param name="nbId">NbId da tarefa que se quer encontrar</param>
        /// <returns>Tarefa encontrada com aquele Id</returns>
        public static CronogramaTarefa ConsultarCronogramaTarefaPorNbId( Guid oidCronograma, short nbId, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(oidCronograma == new Guid() || oidCronograma == null || nbId < 0)
                    throw new ArgumentException( "Parâmetros inválidos." );
                return contexto.CronogramaTarefa.MultiploInclude( includes ).FirstOrDefault( o => o.NbID == nbId && o.OidCronograma == oidCronograma && !o.CsExcluido );
            }
        }

        /// <summary>
        /// Método responsável por buscar CronogramaTarefas pelo NbIds informados
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma que se deseja encontrar a tarefa</param>
        /// <param name="nbIds">NbIds das tarefas que se quer encontrar</param>
        /// <returns>Tarefas encontradas com aqueles nbIds</returns>
        public static List<CronogramaTarefa> ConsultarCronogramaTarefaPorNbID( Guid oidCronograma, List<short> nbIds, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(oidCronograma == new Guid() || oidCronograma == null || nbIds == null)
                    throw new ArgumentException( "Os parâmetros são inválidos." );

                return contexto.CronogramaTarefa.MultiploInclude( includes ).Where( o => o.OidCronograma == oidCronograma && nbIds.Contains( o.NbID ) && !o.CsExcluido ).OrderBy( o => o.NbID ).ToList();
            }
        }

        /// <summary>
        /// Método responsável por buscar as tarefas que serão impactadas
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma em que será pesquisado</param>
        /// <param name="nbIdInicio">ID início</param>
        /// <param name="nbIdFinal">ID final</param>
        /// <returns>Lista de tarefas que serão impactadas na reordenação e/ou recalculo</returns>
        public static List<CronogramaTarefa> ConsultarTarefasImpactadas( Guid oidCronograma, short nbIdInicio, short nbIdFinal = 0 )
        {
            if(oidCronograma == new Guid() || oidCronograma == null || nbIdInicio < 0)
                throw new ArgumentException( "Os parâmetros são inválidos." );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(nbIdFinal > 0)
                {
                    CronogramaTarefaBo.ValidarValoresNbID( ref nbIdInicio, ref nbIdFinal );
                    return contexto.CronogramaTarefa.MultiploInclude(
                        o => o.Cronograma,
                        o => o.Tarefa.SituacaoPlanejamento
                        ).Where( o => o.NbID >= nbIdInicio && o.NbID <= nbIdFinal && o.OidCronograma == oidCronograma && !o.CsExcluido ).OrderBy( o => o.NbID ).ToList();
                }
                else
                {
                    return contexto.CronogramaTarefa.MultiploInclude( o => o.Cronograma, o => o.Tarefa.SituacaoPlanejamento ).Where( o => o.NbID >= nbIdInicio && o.OidCronograma == oidCronograma && !o.CsExcluido ).OrderBy( o => o.NbID ).ToList();
                }
            }
        }

        /// <summary>
        /// Método responsável por buscar todas as tarefas de um determindado cronograma, ordenando-as pelo NbId.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma em que será pesquisado</param>
        /// <returns>Lista com as tarefas existentes naquele cronograma</returns>
        public static List<CronogramaTarefa> ConsultarCronogramaTarefasPorOidCronograma( Guid oidCronograma, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            if(oidCronograma == new Guid())
                new ArgumentException( "Os parâmetros estão inválidos." );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.CronogramaTarefa.MultiploInclude( includes )
                                            .Where( o => o.OidCronograma == oidCronograma && !o.CsExcluido )
                                            .OrderBy( o => o.NbID ).ToList();
            }
        }

        /// <summary>
        /// Método responsável por buscar as tarefas de um cronograma.
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="oidCronogramaTarefas">Lista de Oids das tarefas que se quer procurar.</param>
        /// <returns>Lista das tarefas encontradas</returns>
        public static List<CronogramaTarefa> ConsultarCronogramaTarefasPorOid( List<Guid> oidCronogramaTarefas, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            if(oidCronogramaTarefas == null)
                new ArgumentException( "Os parâmetros estão inválidos." );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.CronogramaTarefa.MultiploInclude( includes )
                                            .Where( o => oidCronogramaTarefas.Contains( o.Oid ) && !o.CsExcluido )
                                            .OrderBy( o => o.NbID ).ToList();
            }
        }

        /// <summary>
        /// Responsável por buscar uma lista de CronogramaTarefas converter para Dto
        /// </summary>
        /// <param name="oidCronogramaTarefas">Lista de oids das tarefas para pesquisar</param>
        /// <returns>retorna lista das tarefas convertidas para Dto</returns>
        public static List<CronogramaTarefaDto> ConsultarCronogramaTarefaPorOidDto( List<Guid> oidCronogramaTarefas )
        {

            List<CronogramaTarefa> cronogramaTarefas = ConsultarCronogramaTarefasPorOid( oidCronogramaTarefas, o => o.Cronograma, o => o.Tarefa.AtualizadoPor.Usuario.Person.Party, o => o.Tarefa.SituacaoPlanejamento );

            DateTime dataHoraAcao = DateTime.Now;

            return cronogramaTarefas == null ? new List<CronogramaTarefaDto>() : cronogramaTarefas.Select( cronogramaTarefa => CronogramaTarefaBo.DtoFactory( cronogramaTarefa, dataHoraAcao ) ).ToList();
        }

        /// <summary>CronogramaTarefaBo.ControlarCriacaoCronogramaTarefas( contexto,
        /// Método usado para buscar todas as tarefas a partir da descrição de um cronograma, é usado pelo serviço e acionado pela tela de cronograma.
        /// </summary>
        /// <param name="oidCronograma">oid do cronograma</param>
        /// <returns>Lista das tarefas consultadas e convertidas para dto</returns>
        public static List<CronogramaTarefaDto> ConsultarCronogramaTarefasPorOidCronogramaDto( Guid oidCronograma )
        {
            List<CronogramaTarefaDto> cronogramaTarefasDto = new List<CronogramaTarefaDto>();

            List<CronogramaTarefa> cronogramaTarefas = CronogramaTarefaDao.ConsultarCronogramaTarefasPorOidCronograma( oidCronograma, o => o.Cronograma, o => o.Tarefa.AtualizadoPor.Usuario.Person, o => o.Tarefa.SituacaoPlanejamento );
            DateTime dataHoraAcao = DateTime.Now;

            if(cronogramaTarefas == null)
                return cronogramaTarefasDto;

            for(int i = 0; i < cronogramaTarefas.Count; i++)
                cronogramaTarefasDto.Add( CronogramaTarefaBo.DtoFactory( cronogramaTarefas[i], dataHoraAcao ) );

            return cronogramaTarefasDto;
        }

        /// <summary>
        /// Método responsável por buscar tarefas em cronogramaTarefa através do Oid.
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da tarefa a ser pesquisada</param>
        /// <returns>Tarefa encontrada</returns>
        public static CronogramaTarefa ConsultarCronogramaTarefaPorOid( Guid oidCronogramaTarefa, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(oidCronogramaTarefa == new Guid())
                    new ArgumentException( "Parâmetros inválidos." );
                return contexto.CronogramaTarefa.MultiploInclude( includes ).FirstOrDefault( o => o.Oid == oidCronogramaTarefa && !o.CsExcluido );
            }
        }

        /// <summary>
        /// Método responsável por buscar uma tarefa pelo Oid e Converter para Dto
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da tarefa a ser procurada</param>
        /// <returns>Tarefa encontrada</returns>
        public static CronogramaTarefaDto ConsultarCronogramaTarefaPorOidDto( Guid oidCronogramaTarefa )
        {
            CronogramaTarefa cronogramaTarefa = CronogramaTarefaDao.ConsultarCronogramaTarefaPorOid( oidCronogramaTarefa, o => o.Tarefa.AtualizadoPor.Usuario.Person, o => o.Tarefa.SituacaoPlanejamento, o => o.Cronograma );
            DateTime dataHoraAcao = DateTime.Now;
            return CronogramaTarefaBo.DtoFactory( cronogramaTarefa, dataHoraAcao );
        }

        /// <summary>
        /// Método responsável por buscar os NbID de uma tarefa.
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid da tarefa a ser procurada</param>
        /// <returns>tarefa encontrada</returns>
        public static short ConsultarNbIDCronogramaTarefa( Guid oidCronogramaTarefa )
        {
            if(oidCronogramaTarefa == null)
                new ArgumentException( "Os parâmetros estão inválidos." );

            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                CronogramaTarefa tarefa = contexto.CronogramaTarefa.FirstOrDefault( o => o.Oid == oidCronogramaTarefa && !o.CsExcluido );
                if(tarefa != null)
                    return tarefa.NbID;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Responsável por encontrar o valor do maior ID da tarefa de um determinado cronograma
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma que será procurado</param>
        /// <returns>Maior NbId</returns>
        public static short ConsultarMaxNbIDPorCronograma( Guid oidCronograma )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(contexto == null || oidCronograma == null)
                    new ArgumentException( "Os parâmetros estão inválidos." );

                CronogramaTarefa cronogramaTarefa = contexto.CronogramaTarefa.FirstOrDefault( o => o.OidCronograma == oidCronograma && !o.CsExcluido );

                if(cronogramaTarefa != null)
                    return contexto.CronogramaTarefa.Where( o => o.OidCronograma == oidCronograma && !o.CsExcluido ).Select( o => o.NbID ).Max();
                else
                    return 0;
            }
        }

        /// <summary>
        /// Método responsável por ConsultarIncluindoRelacionamentos um objeto CronogramaTarefa pelo Oid do Objeto Tarefa
        /// </summary>
        /// <param name="oidTarefa">Oid da tarefa que será procurada</param>
        /// <returns>Tarefa encontrada</returns>
        public static CronogramaTarefa ConsultarCronogramarTarefaPorTarefaOid( Guid oidTarefa, params Expression<Func<CronogramaTarefa, object>>[] includes )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                return contexto.CronogramaTarefa.MultiploInclude( includes ).FirstOrDefault( o => o.Tarefa.Oid == oidTarefa && !o.CsExcluido );
            }
        }

        /// <summary>
        /// Método para consultar se existe um objeto CronogramaTarefa no banco de dados
        /// </summary>
        /// <param name="oidCronogramaTarefa">Oid do Objeto</param>
        /// <returns>Se existe ou não no banco</returns>
        public static bool ConsultarSeCronogramaTarefaExisteNoBancoDeDados( Guid oidCronogramaTarefa )
        {
            using(WexDb contexto = ContextFactoryManager.CriarWexDb())
            {
                if(contexto.CronogramaTarefa.Existe( o => o.Oid == oidCronogramaTarefa ))
                    return true;
            }

            return false;
        }

        #endregion

        #region Salvar

        /// <summary>
        /// Método responsável por salvar um CronogramaTarefa no banco de dados.
        /// Caso gere exception, tenta novamente realizar a ação de salvar.
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="cronogramaTarefa">CronogramaTarefa que será salvo.</param>
        /// <param name="numeroTentativas">quantidade de vezes que irá tentar salvar</param>
        /// <returns>Tarefa que foi salva/atualizada</returns>
        public static CronogramaTarefa Salvar( CronogramaTarefa cronogramaTarefa )
        {

            CronogramaTarefa copia = cronogramaTarefa.Clone();
            copia.Cronograma = null;
            copia.Tarefa = null;

            try
            {
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                {
                    if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefa.Oid ))
                    {
                        contexto.CronogramaTarefa.Attach( copia );
                        contexto.Entry( copia ).State = EntityState.Modified;
                    }
                    else
                    {
                        contexto.CronogramaTarefa.Add( copia );
                    }
                    contexto.SaveChanges();
                }
            }
            catch(Exception)
            {
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                {
                    if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefa.Oid ))
                    {
                        if(contexto.CronogramaTarefa.ExisteLocalmente( o => o.Oid == cronogramaTarefa.Oid ))
                        {
                            contexto.Entry<CronogramaTarefa>( copia ).Reload();
                            contexto.CronogramaTarefa.Attach( copia );
                            contexto.Entry( copia ).State = EntityState.Modified;
                        }
                        else
                        {
                            contexto.CronogramaTarefa.Attach( copia );
                            contexto.Entry<CronogramaTarefa>( copia ).Reload();
                            contexto.CronogramaTarefa.Attach( copia );
                            contexto.Entry( copia ).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        contexto.CronogramaTarefa.Add( copia );
                    }
                    contexto.SaveChanges();
                }
            }

            return cronogramaTarefa;
        }

        #endregion

        #region Excluir

        /// <summary>
        /// Método responsável por realizar a exclusão de um objeto CronogramaTarefa
        /// obs: caso ele não consiga, ele atualiza o objeto (caso tenha sido alterado por outra thread) e realiza novamente a tentativa de exclusão.
        /// <param name="cronogramaTarefa">Tarefa que será excluida</param>
        /// <param name="numeroTentativas">número de tentivas para tentar excluir</param>
        public static void Excluir( CronogramaTarefa cronogramaTarefa )
        {
            Tarefa copiaTarefa = cronogramaTarefa.Tarefa.Clone();

            cronogramaTarefa.CsExcluido = true;
            cronogramaTarefa.Tarefa = null;
            cronogramaTarefa.Cronograma = null;

            copiaTarefa.CsExcluido = true;
            copiaTarefa.AtualizadoPor = null;
            copiaTarefa.LogsAlteracao = null;
            copiaTarefa.SituacaoPlanejamento = null;
            copiaTarefa.TarefaHistoricoTrabalhos = null;
            copiaTarefa.TarefaResponsaveis = null;

            try
            {
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                {
                    if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefa.Oid ))
                    {
                        contexto.CronogramaTarefa.Attach( cronogramaTarefa );
                        contexto.Tarefa.Attach( copiaTarefa );
                        contexto.Entry( cronogramaTarefa ).State = EntityState.Modified;
                        contexto.Entry( copiaTarefa ).State = EntityState.Modified;
                    }

                    contexto.SaveChanges();
                }
            }
            catch(Exception)
            {
                using(WexDb contexto = ContextFactoryManager.CriarWexDb())
                {
                    if(contexto.CronogramaTarefa.Existe( o => o.Oid == cronogramaTarefa.Oid ))
                    {
                        contexto.CronogramaTarefa.Attach( cronogramaTarefa );
                        contexto.Tarefa.Attach( copiaTarefa );
                        contexto.Entry<CronogramaTarefa>( cronogramaTarefa ).Reload();
                        cronogramaTarefa.CsExcluido = true;
                        contexto.Entry<Tarefa>( copiaTarefa ).Reload();
                        copiaTarefa.CsExcluido = true;
                        contexto.CronogramaTarefa.Attach( cronogramaTarefa );
                        contexto.Tarefa.Attach( copiaTarefa );
                        contexto.Entry( cronogramaTarefa ).State = EntityState.Modified;
                        contexto.Entry( copiaTarefa ).State = EntityState.Modified;
                    }

                    contexto.SaveChanges();
                }
            }
        }

        #endregion

        #region Factories

        /// <summary>
        /// Método responsável por converter as tarefas impactadas após uma movimentação e a data e hora da ação para um Dto.
        /// </summary>
        /// <param name="impactadas">Lista contendo as tarefas impactadas</param>
        /// <param name="dataHoraAcao">Data e hora da ação</param>
        /// <returns>Dto de TarefaMovidasDto</returns>
        public static TarefasMovidasDto TarefasMovidasDtoFactory( List<CronogramaTarefa> impactadas, DateTime dataHoraAcao, Guid oidCronogramaTarefaMovida, short nbIDAtualizadoTarefaMovida, Guid oidCronograma )
        {
            var tarefasMovidasDto = new TarefasMovidasDto
            {
                TarefasImpactadas = impactadas.Distinct().ToDictionary( o => o.Oid.ToString(), o => o.NbID ),
                DataHoraAcao = dataHoraAcao,
                OidCronogramaTarefaMovida = oidCronogramaTarefaMovida,
                NbIDTarefaMovida = nbIDAtualizadoTarefaMovida,
                OidCronograma = oidCronograma
            };
            return tarefasMovidasDto;
        }

        /// <summary>
        /// Método responsável por converter as tarefas impactadas após uma movimentação, a data e hora da ação, as tarefas nao excluidas e tarefas excluidas para um Dto.
        /// </summary>
        /// <param name="impactadas">Tarefas impactadas (reordenadas)</param>
        /// <param name="oidsNaoExcluidos">Oids das Tarefas nao excluidas</param>
        /// <param name="excluidas">Tarefas excluidas</param>
        /// <param name="dataHoraAcao">Data e hora da acao que foi realizada</param>
        /// <returns>Objeto TarefasExcluidasDto</returns>
        public static TarefasExcluidasDto TarefasExcluidasDtoFactory( List<CronogramaTarefa> impactadas, List<Guid> oidsNaoExcluidos, List<CronogramaTarefa> excluidas, DateTime dataHoraAcao, Guid oidCronograma )
        {
            TarefasExcluidasDto tarefasExcluidasDto = new TarefasExcluidasDto()
            {
                TarefasImpactadas = impactadas.Distinct().ToDictionary( o => o.Oid.ToString(), o => o.NbID ),
                TarefasExcluidas = excluidas.Distinct().Select( o => o.Oid ).ToList(),
                TarefasNaoExcluidas = oidsNaoExcluidos,
                DataHoraAcao = dataHoraAcao,
                OidCronograma = oidCronograma
            };
            return tarefasExcluidasDto;
        }

        #endregion
    }
}
