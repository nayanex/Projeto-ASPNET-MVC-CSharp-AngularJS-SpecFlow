using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections;
using WexProject.Library.Libs.Logger;

namespace WexProject.Library.Libs.SemaforoPorIntervalo
{
    public class SemaforoSingleton
    {
        #region Atributos

        /// <summary>
        /// Atributo estático responsável por guarda a instância da classe.
        /// </summary>
        public static SemaforoSingleton instanciaSemaforoSingleton;

        /// <summary>
        /// Atributo responsável por controlar o acesso de leitura e escrita ao dicionário de cronogramas.
        /// </summary>
        public static ReaderWriterLockSlim lockerCronogramas = new ReaderWriterLockSlim();

        /// <summary>
        /// Atributo estático responsável por armazenar os cronogramas, sua respectiva lista contendo vários semáforos.
        /// </summary>
        public static Dictionary<Guid, SemaforosControle> semaforosPorCronograma = new Dictionary<Guid, SemaforosControle>();

        /// <summary>
        /// Dicionário responsável por armazenar o contador de acesso de cada cronograma, 
        /// possibilitando saber se alguma thread está acessando informações de semáforos de um cronograma em específico.
        /// </summary>
        public static Dictionary<Guid, ContadorAcesso> contadoresAcessoPorCronograma = new Dictionary<Guid, ContadorAcesso>();

        /// <summary>
        /// Dicionário responsável por armazenar os semáforos para controlar a exclusão dos semáforos inativos por cronograma.
        /// </summary>
        public static Dictionary<Guid, Semaphore> semaforosControleExclusaoSemaforosPorCronograma = new Dictionary<Guid, Semaphore>();

        /// <summary>
        /// Semáforo responsável por controlar a incrementação e decrementação da variável contadorAcesso;
        /// </summary>
        public static Semaphore semaforoContadorAcesso = new Semaphore( 1, 1 );

        /// <summary>
        /// Atributo responsável por controlar a exclusão de semáforos que já não estão disponíveis.
        /// </summary>
        public static Semaphore semaforoControleExclusaoSemaforos = new Semaphore( 1, 1 );

        #endregion

        #region Constantes

        /// <summary>
        /// Constante responsável por indicar que existe apenas uma thread acessando.
        /// </summary>
        public const int MINIMO_THREAD_EMACESSO = 1;

        /// <summary>
        /// Constante responsável por servir como índice na hash para indicar o índice de Semáforos novos.
        /// </summary>
        public const string SEMAFOROS_NOVOS = "semaforosNovos";

        /// <summary>
        /// Constante responsável por servir como índice na hash para indicar o índice de Semáforos que deverão ser aguardados.
        /// </summary>
        public const string SEMAFOROS_AGUARDAR = "semaforosAguardar";

        #endregion

        #region Struct

        /// <summary>
        /// Struct responsável por armazenar a lista de semáforos e seu respectivo Semáforo (ReaderWriterLockSlim) para cada cronograma.
        /// </summary>
        public struct SemaforosControle
        {
            /// <summary>
            /// Lista de semáforos de um cronograma.
            /// </summary>
            public List<SemaforoPorIntervalo> semaforos;

            /// <summary>
            /// Semáforo (ReaderWriterLockSlim) que controla o acesso a lista de semáforos de um determinado cronograma.
            /// </summary>
            public ReaderWriterLockSlim lockerSemaforos;
        }

        /// <summary>
        /// Struct responsável por armazenar o atributo para contabilizar quantos acessos existem daquele cronograma, 
        /// assim como um semáforo para controlar incrementação e decrementação.
        /// </summary>
        public struct ContadorAcesso
        {
            /// <summary>
            /// Atributo para contar o número de threads que estão em processo em determinado momento.
            /// </summary>
            public int contadorAcesso;

            /// <summary>
            /// Semáforo para controlar a incrementação e decrementação do atributo contador de acesso.
            /// </summary>
            public Semaphore semaforoContadorAcesso;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método responsável controlar o acesso de threads simultaneamente enquanto leem e escrevem tanto no dicionário de cronogramas quanto na lista de semáforos de cada cronograma.
        /// Valida o acesso por cronograma e intervalo.
        /// </summary>
        /// <param name="oidCronograma">Oid cronograma</param>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        public virtual Hashtable ControlarSemaforos( Guid oidCronograma, short inicio, short final )
        {
            Hashtable semaforos = new Hashtable();

            ControlarIncrementacaoContadorAcessoPorCronograma( oidCronograma );

            ControlarExclusaoSemaforosInativosPorCronograma( oidCronograma );

            if(ValidarCronograma( oidCronograma ))
            {
                semaforos = ConsultarSemaforosImpactadosPorCronograma( oidCronograma, inicio, final, semaforos );

                ControlarExclusaoCronogramasInexistentes();
            }
            else
            {
                EsperarEscritaCronogramas();

                if(semaforosPorCronograma.ContainsKey( oidCronograma ))
                {
                    LiberarEscritaCronogramas();

                    semaforos = ConsultarSemaforosImpactadosPorCronograma( oidCronograma, inicio, final, semaforos );

                    ControlarExclusaoCronogramasInexistentes();

                    ControlarDecrementacaoContadorAcessoPorCronograma( oidCronograma );

                    return semaforos;
                }
                else
                {
                    //cria um objeto SemaforoPorCronograma que armazena um semáforo e a lista de semáforos de um cronograma
                    SemaforosControle semaforosControle = new SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };

                    //cria um semáforo
                    SemaforoPorIntervalo novoSemaforo = new SemaforoPorIntervalo( inicio, final );

                    //cria uma lista para retornar na Hash 
                    List<SemaforoPorIntervalo> semaforosNovos = new List<SemaforoPorIntervalo>();

                    semaforosNovos.Add( novoSemaforo );

                    //Adiciona o semáforo novo criado para retornar na hash
                    semaforos.Add( SEMAFOROS_NOVOS, semaforosNovos );

                    //adiciona o semáforo à lista de semáforo por cronograma.
                    semaforosControle.semaforos.Add( novoSemaforo );

                    //adiciona o cronograma e sua lista ao dicionário de cronogramas ativos.
                    semaforosPorCronograma.Add( oidCronograma, semaforosControle );
                }

                ControlarExclusaoCronogramasInexistentes();

                LiberarEscritaCronogramas();
            }

            ControlarDecrementacaoContadorAcessoPorCronograma( oidCronograma );

            return semaforos;
        }

        /// <summary>
        /// Método responsável por consultar os semáforos que uma determinada thread de reordenação deverá esperar para então executar sua própria reordenação.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma (índice do dicionário de cronogramas)</param>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <returns>Retorna uma lista contendo os semáforos que deve-se esperar</returns>
        public Hashtable ConsultarSemaforosImpactadosPorCronograma( Guid oidCronograma, short inicio, short final, Hashtable semaforos )
        {
            List<SemaforoPorIntervalo> semaforosImpactadosAguardar = new List<SemaforoPorIntervalo>();
            List<SemaforoPorIntervalo> semaforosImpactadosNovos = new List<SemaforoPorIntervalo>();

            lock(semaforosPorCronograma[oidCronograma].semaforos)
            {
                EsperarLeituraSemaforos( oidCronograma );
                
                if(semaforosPorCronograma[oidCronograma].semaforos.Count == 0)
                {
                    semaforosImpactadosNovos.Add( new SemaforoPorIntervalo( inicio, final ) );
                }
                else
                {
                    for(int i = 0; i < semaforosPorCronograma[oidCronograma].semaforos.Count; i++)
                    {
                        SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosAguardar, ref semaforosImpactadosNovos, ref semaforosImpactadosNovos,
                            semaforosPorCronograma[oidCronograma].semaforos[i], inicio, final );

                        if(semaforosImpactadosNovos.Count > 0)
                        {
                            if(semaforosImpactadosAguardar.Count > 0)
                                for(int indice = 0; indice < semaforosImpactadosAguardar.Count; indice++)
                                    SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosImpactadosAguardar[indice] );
                            break;
                        }
                    }
                }

                LiberarLeituraSemaforos( oidCronograma );

                if(semaforosImpactadosNovos.Count > 0)
                {
                    EsperarEscritaSemaforos( oidCronograma );

                    semaforosImpactadosAguardar.Clear();
                    semaforosImpactadosNovos.Clear();

                    List<SemaforoPorIntervalo> semaforosExistentes = semaforosPorCronograma[oidCronograma].semaforos;

                    if(semaforosPorCronograma[oidCronograma].semaforos.Count == 0)
                    {
                        SemaforoPorIntervalo novoSemaforo = new SemaforoPorIntervalo( inicio, final );

                        semaforosPorCronograma[oidCronograma].semaforos.Add( novoSemaforo );

                        semaforosImpactadosNovos.Add( novoSemaforo );
                    }
                    else
                    {
                        for(int i = 0; i < semaforosPorCronograma[oidCronograma].semaforos.Count; i++)
                        {
                            if(semaforosPorCronograma[oidCronograma].semaforos[i].semaforo != null)
                            {
                                SemaforoPorIntervalo.IncrementarContadorSemaforoEmEspera( semaforosPorCronograma[oidCronograma].semaforos[i] );

                                SemaforoPorIntervalo.VerificarSemaforosImpactados( ref semaforosImpactadosAguardar, ref semaforosImpactadosNovos, ref semaforosExistentes,
                                                                                   semaforosPorCronograma[oidCronograma].semaforos[i], inicio, final );

                                SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosPorCronograma[oidCronograma].semaforos[i] );
                            }
                            else
                            {
                                semaforosPorCronograma[oidCronograma] = new SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };
                                    
                                //cria um semáforo
                                SemaforoPorIntervalo novoSemaforo = new SemaforoPorIntervalo( inicio, final );

                                semaforosImpactadosNovos.Add( novoSemaforo );

                                semaforosPorCronograma[oidCronograma].semaforos.Add( novoSemaforo );
                            }
                        }
                    }

                    LiberarEscritaSemaforos( oidCronograma );
                }
            }

            semaforos.Add( SEMAFOROS_AGUARDAR, semaforosImpactadosAguardar );
            semaforos.Add( SEMAFOROS_NOVOS, semaforosImpactadosNovos );

            return semaforos;
        }

        /// <summary>
        /// Método responsável por controlar a exclusão dos semáforo inativos por cronograma, validando também a existencia do cronograma 
        /// </summary>
        /// <param name="oidCronograma"></param>
        public static void ControlarExclusaoSemaforosInativosPorCronograma( Guid oidCronograma )
        {
            //controla a exclusão através de um semáforo para várias threads não tentarem excluir vários semáforos ao mesmo tempo
            semaforoControleExclusaoSemaforos.WaitOne();

            if(!semaforosControleExclusaoSemaforosPorCronograma.ContainsKey( oidCronograma ))
                semaforosControleExclusaoSemaforosPorCronograma.Add( oidCronograma, new Semaphore( 1, 1 ) );

            //libera o semáforo 
            semaforoControleExclusaoSemaforos.Release();

            ExcluirSemaforosInativosPorCronograma( oidCronograma );

            ControlarLixoDicionarioExclusaoSemaforosPorCronograma( oidCronograma );
        }

        /// <summary>
        /// Método responsável por controlar a incrementação do atributo contador de acesso para determinado cronograma.
        /// Valida também a existencia dele no dicionário de contadores de acesso por cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid Cronograma</param>
        public static void ControlarIncrementacaoContadorAcessoPorCronograma( Guid oidCronograma )
        {
            semaforoContadorAcesso.WaitOne();

            if(!contadoresAcessoPorCronograma.ContainsKey( oidCronograma ))
                contadoresAcessoPorCronograma.Add( oidCronograma, new ContadorAcesso() { contadorAcesso = 0, semaforoContadorAcesso = new Semaphore( 1, 1 ) } );

            semaforoContadorAcesso.Release();

            IncrementarContadorAcessoPorCronograma( oidCronograma );
        }

        /// <summary>
        /// Método responsável por controlar a decrementação do atributo contador de acesso para determinado cronograma.
        /// Valida também se o contador de acesso está nulo, se estiver retira ele do dicionário para nao acumular lixo.
        /// </summary>
        /// <param name="oidCronograma">Oid Cronograma</param>
        public static void ControlarDecrementacaoContadorAcessoPorCronograma( Guid oidCronograma )
        {
            DecrementarContadorAcessoPorCronograma( oidCronograma );

            semaforoContadorAcesso.WaitOne();

            if(contadoresAcessoPorCronograma.ContainsKey( oidCronograma ))
                if(contadoresAcessoPorCronograma[oidCronograma].contadorAcesso == 0)
                {
                    contadoresAcessoPorCronograma[oidCronograma].semaforoContadorAcesso.Dispose();
                    contadoresAcessoPorCronograma.Remove( oidCronograma );
                }

            semaforoContadorAcesso.Release();
        }

        /// <summary>
        /// Método responsável por Excluir os cronogramas que não possuem semáforos ativos.
        /// </summary>
        public static void ControlarExclusaoCronogramasInexistentes()
        {
            int qtdAcessos = 0;

            semaforoContadorAcesso.WaitOne();

            foreach(KeyValuePair<Guid, ContadorAcesso> item in contadoresAcessoPorCronograma)
                if(item.Value.contadorAcesso > 0)
                    qtdAcessos += 1;

            if(qtdAcessos == MINIMO_THREAD_EMACESSO)
            {
                List<Guid> cronogramasExcluir = ValidarCronogramasInexistentes( ref semaforosPorCronograma );

                ExcluirCronogramasInexistentes( cronogramasExcluir, ref semaforosPorCronograma );
            }

            semaforoContadorAcesso.Release();
        }


        #endregion

        #region Utilitários

        /// <summary>
        /// Método responsável por excluir os cronogramas inativos de um determinado cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        public static void ExcluirSemaforosInativosPorCronograma( Guid oidCronograma )
        {
            //controla a exclusão através de um semáforo para várias threads não tentarem excluir vários semáforos ao mesmo tempo
            semaforosControleExclusaoSemaforosPorCronograma[oidCronograma].WaitOne();

            if(semaforosPorCronograma.ContainsKey( oidCronograma ))
                //lock na lista de semáforos daquele cronograma para que nenhuma outra thread adicione mais semáforos a esta lista.
                lock(semaforosPorCronograma[oidCronograma].semaforos)
                {
                    List<SemaforoPorIntervalo> semaforosExistentes = semaforosPorCronograma[oidCronograma].semaforos;

                    List<SemaforoPorIntervalo> semaforosParaExcluir = ValidarSemaforosInativos( semaforosExistentes );

                    ExcluirSemaforosInativos( ref semaforosParaExcluir, ref semaforosExistentes );
                }

            //libera o semáforo 
            semaforosControleExclusaoSemaforosPorCronograma[oidCronograma].Release();
        }

        /// <summary>
        /// Método responsável por retirar da lista todos os semáforos que existem para que um nova seja criada.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        /// <param name="excluirSemaforos">Atributo que autoriza a exclusão de todos os semáforos.</param>
        public static void ExcluirSemaforosInativosPorCronograma( Guid oidCronograma, bool excluirSemaforos )
        {
            if(excluirSemaforos)
            {
                if(semaforosPorCronograma.ContainsKey( oidCronograma ))
                    //lock na lista de semáforos daquele cronograma para que nenhuma outra thread adicione mais semáforos a esta lista.
                    lock(semaforosPorCronograma[oidCronograma].semaforos)
                    {
                        if(semaforosPorCronograma[oidCronograma].semaforos.Count > 0)
                            semaforosPorCronograma[oidCronograma] = new SemaforosControle() { lockerSemaforos = new ReaderWriterLockSlim(), semaforos = new List<SemaforoPorIntervalo>() };
                    }
            }
        }

        /// <summary>
        /// Método responsável por excluir os cronogramas inexistentes no dicionário dos cronogramas existentes atualmente.
        /// </summary>
        /// <param name="cronogramasExcluir">Lista de Guid que deverão ser excluídos</param>
        /// <param name="cronogramasExistentes">Dicionário de cronogramas existentes atualmente</param>
        public static void ExcluirCronogramasInexistentes( List<Guid> cronogramasExcluir, ref Dictionary<Guid, SemaforosControle> cronogramasExistentes )
        {
            for(int i = 0; i < cronogramasExcluir.Count; i++)
                cronogramasExistentes.Remove( cronogramasExcluir[i] );
        }

        /// <summary>
        /// Método responsável por excluir os semáforos inutilizados da lista de semáforos existentes.
        /// </summary>
        /// <param name="semaforosParaExcluir">Lista de semáforos que deverão ser excluídos</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes atualmente</param>
        public static void ExcluirSemaforosInativos( ref List<SemaforoPorIntervalo> semaforosParaExcluir, ref List<SemaforoPorIntervalo> semaforosExistentes )
        {
            //excluindo semáforos da lista de semaforos por cronograma
            for(int i = 0; i < semaforosParaExcluir.Count; i++)
            {
                SemaforoPorIntervalo.DestruirSemaforo( semaforosParaExcluir[i] );
                semaforosExistentes.Remove( semaforosParaExcluir[i] );
            }
        }

        /// <summary>
        /// Método responsável por verificar quais cronogramas não possuem lista de semáforos ativos e que devem ser excluídos.
        /// </summary>
        /// <param name="cronogramasExistentes">Dicionário de cronogramas existentes atualmente</param>
        /// <returns>Lista de Guid dos cronogramas que devem ser excluídos</returns>
        public static List<Guid> ValidarCronogramasInexistentes( ref Dictionary<Guid, SemaforosControle> cronogramasExistentes )
        {
            List<Guid> cronogramasExcluir = new List<Guid>();

            foreach(KeyValuePair<Guid, SemaforosControle> objeto in cronogramasExistentes)
                if(objeto.Value.semaforos.Count == 0)
                    cronogramasExcluir.Add( objeto.Key );

            return cronogramasExcluir;
        }

        /// <summary>
        /// Método responsável por verificar quais semáforos devem ser excluídos.
        /// </summary>
        /// <param name="semaforosExistentes">Lista de semáforos existentes atualmente</param>
        /// <returns>Lista de semáforos que devem ser excluídos</returns>
        public static List<SemaforoPorIntervalo> ValidarSemaforosInativos( List<SemaforoPorIntervalo> semaforosExistentes )
        {
            List<SemaforoPorIntervalo> semaforosParaExcluir = new List<SemaforoPorIntervalo>();

            //verificando quais semaforos nao são mais necessários.
            for(int i = 0; i < semaforosExistentes.Count; i++)
            {
                if(semaforosExistentes[i].semaforo == null || semaforosExistentes[i].emEspera <= 0)
                    semaforosParaExcluir.Add( semaforosExistentes[i] );
            }
            
            return semaforosParaExcluir;
        }

        /// <summary>
        /// Método responsável por validar se um determinado cronograma já existe no dicionário de cronogramas
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma que se deseja validar</param>
        /// <returns>Confirmação de que existe ou não determinado cronograma no dicionário de cronogramas</returns>
        public bool ValidarCronograma( Guid oidCronograma )
        {
            bool cronogramaExistente;

            EsperarLeituraCronogramas();

            if(semaforosPorCronograma.ContainsKey( oidCronograma ))
                cronogramaExistente = true;
            else
                cronogramaExistente = false;

            LiberarLeituraCronogramas();

            return cronogramaExistente;
        }

        /// <summary>
        /// Método responsável por solicitar a leitura do dicionário de cronogramas.
        /// </summary>
        public virtual void EsperarLeituraCronogramas()
        {
            lockerCronogramas.EnterReadLock();
        }

        /// <summary>
        /// Método responsável por solicitar a escrita do dicionário de cronogramas.
        /// </summary>
        public virtual void EsperarEscritaCronogramas()
        {
            lockerCronogramas.EnterWriteLock();
        }

        /// <summary>
        /// Método responsável por liberar a leitura ao dicionário de cronogramas.
        /// </summary>
        public virtual void LiberarLeituraCronogramas()
        {
            lockerCronogramas.ExitReadLock();
        }

        /// <summary>
        /// Método responsável por liberar a escrita ao dicionário de cronogramas.
        /// </summary>
        public virtual void LiberarEscritaCronogramas()
        {
            lockerCronogramas.ExitWriteLock();
        }

        /// <summary>
        /// Método responsável por solicitar a leitura da lista de semáforos contidos em um cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        public virtual void EsperarLeituraSemaforos( Guid oidCronograma )
        {
            semaforosPorCronograma[oidCronograma].lockerSemaforos.EnterReadLock();
        }

        /// <summary>
        /// Método responsável por liberar a leitura da lista de semáforos contidos em um cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        public virtual void LiberarLeituraSemaforos( Guid oidCronograma )
        {
            semaforosPorCronograma[oidCronograma].lockerSemaforos.ExitReadLock();
        }

        /// <summary>
        /// Método responsável por solicitar a escrita da lista de semáforos contidos em um cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        public virtual void EsperarEscritaSemaforos( Guid oidCronograma )
        {
            semaforosPorCronograma[oidCronograma].lockerSemaforos.EnterWriteLock();
        }

        /// <summary>
        /// Método responsável por liberar a escrita da lista de semáforos contidos em um cronograma.
        /// </summary>
        /// <param name="oidCronograma">Oid do cronograma</param>
        public virtual void LiberarEscritaSemaforos( Guid oidCronograma )
        {
            semaforosPorCronograma[oidCronograma].lockerSemaforos.ExitWriteLock();
        }

        /// <summary>
        /// Método responsável por incrementar a variável que controla o acesso ao dicionário de cronogramas
        /// </summary>
        public static void IncrementarContadorAcessoPorCronograma( Guid oidCronograma )
        {
            contadoresAcessoPorCronograma[oidCronograma].semaforoContadorAcesso.WaitOne();

            ContadorAcesso objetoContadorAcesso = contadoresAcessoPorCronograma[oidCronograma];

            objetoContadorAcesso.contadorAcesso += 1;

            contadoresAcessoPorCronograma[oidCronograma] = objetoContadorAcesso;

            contadoresAcessoPorCronograma[oidCronograma].semaforoContadorAcesso.Release();
        }

        /// <summary>
        /// Método responsável por decrementar a variável que controla o acesso ao dicionário de cronogramas
        /// </summary>
        public static void DecrementarContadorAcessoPorCronograma( Guid oidCronograma )
        {
            contadoresAcessoPorCronograma[oidCronograma].semaforoContadorAcesso.WaitOne();

            ContadorAcesso objetoContadorAcesso = contadoresAcessoPorCronograma[oidCronograma];

            objetoContadorAcesso.contadorAcesso -= 1;

            contadoresAcessoPorCronograma[oidCronograma] = objetoContadorAcesso;

            contadoresAcessoPorCronograma[oidCronograma].semaforoContadorAcesso.Release();
        }

        /// <summary>
        /// Método responsável por avaliar se o índice no dicionário de semaforosControleExclusaoSemaforosPorCronograma ainda será utilizado, caso não ele remove para não acumular lixo.
        /// </summary>
        /// <param name="oidCronograma">Guid oid cronograma</param>
        private static void ControlarLixoDicionarioExclusaoSemaforosPorCronograma( Guid oidCronograma )
        {
            semaforoContadorAcesso.WaitOne();

            foreach(KeyValuePair<Guid, Semaphore> item in semaforosControleExclusaoSemaforosPorCronograma)
                if(item.Key != oidCronograma)
                    if(contadoresAcessoPorCronograma.ContainsKey( item.Key ))
                        if(contadoresAcessoPorCronograma[item.Key].contadorAcesso == 0)
                            if(semaforosControleExclusaoSemaforosPorCronograma.ContainsKey( item.Key ))
                                semaforosControleExclusaoSemaforosPorCronograma.Remove( item.Key );        
            
            semaforoContadorAcesso.Release();
        }

        
        #endregion

        #region Consultas


        /// <summary>
        /// Método responsável por retornar a instância da classe Semaforo.
        /// </summary>
        /// <returns>Instância da classe</returns>
        public static SemaforoSingleton GetInstancia()
        {
            if(instanciaSemaforoSingleton == null)
            {
                instanciaSemaforoSingleton = new SemaforoSingleton();
            }

            return instanciaSemaforoSingleton;
        }


        #endregion
    }
}
