using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WexProject.Library.Libs.SemaforoPorIntervalo
{
    public class SemaforoPorIntervalo
    {
        #region Atributos

        /// <summary>
        /// Atributo responsável por guardar o semáforo correspondente a um determinado intervalo
        /// </summary>
        public Semaphore semaforo;

        /// <summary>
        /// Atributo responsável por guardar o início do intervalo
        /// </summary>
        public short inicio;

        /// <summary>
        /// Atributo responsável por guardar o final do intervalo
        /// </summary>
        public short final;

        /// <summary>
        /// Atributo responsável por guardar o intervalo.
        /// </summary>
        public List<short> intervalo;

        /// <summary>
        /// Atributo responsável por guardar quantas threads se encontram em espera em determinado semáforo.
        /// </summary>
        public short emEspera;

        /// <summary>
        /// Atributo responsável por guardar o semáforo correspondente ao controle do atributo emEspera
        /// </summary>
        public Semaphore semaforoEmEspera;

        /// <summary>
        /// Atributo responsável por Oid
        /// </summary>
        public Guid Oid;

        #endregion

        #region Regras de Negócios

        /// <summary>
        /// Método responsável por validar a necessidade de criar novos semáforos de acordo com os novos intervalos,
        /// ou se apenas deve-se reaproveitar o semáforo existentes.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Semáforo que deseja comparar para verificar se há necessidade de criar novos semáforos</param>
        /// <param name="inicio">ponto mínimo do intervalo atual</param>
        /// <param name="final">ponto máximo do intervalo atual</param>
        /// <returns>Lista de semáforos que se deve esperar</returns>
        public static void VerificarSemaforosImpactados( ref List<SemaforoPorIntervalo> semaforosImpactadosAguardar,
                                                         ref List<SemaforoPorIntervalo> semaforosImpactadosNovos,
                                                         ref List<SemaforoPorIntervalo> semaforosExistentes,
                                                         SemaforoPorIntervalo semaforoPorIntervalo, short inicio, short final )
        {
            if(!( inicio <= final ))
                return;

            //Verifica se o intervalo está contido no intervalo em verificação atual.
            if(RnVerificarInicioFinalIntervaloEstaContidoEmOutroIntervalo( inicio, final, semaforoPorIntervalo ))
            {
                if(RnValidarAdicionarSemaforoParaAguarde( semaforosImpactadosAguardar, semaforosImpactadosNovos, semaforoPorIntervalo ))
                {
                    SemaforoPorIntervalo.IncrementarContadorSemaforoEmEspera( semaforoPorIntervalo );    
                    
                    semaforosImpactadosAguardar.Add( semaforoPorIntervalo );
                }

                return;
            }

            //verifica se intervalo irá impactar no intervalo já existente
            //Se não impactar irá querer criar um novo semáforo.
            if(!( RnVerificarIntervaloImpactara( inicio, final, semaforoPorIntervalo ) ))
            {
                //Verifica se intervalo não irá impactar nos intervalos já existentes ou nos novos intervalos já existentes 
                if(!( RnVerificarIntervaloImpactara( inicio, final, semaforosImpactadosNovos )
                       || RnVerificarIntervaloImpactara( inicio, final, semaforosImpactadosAguardar )
                       || RnVerificarIntervaloImpactara( inicio, final, semaforosExistentes ) ))
                {
                    semaforosImpactadosNovos.Add( new SemaforoPorIntervalo( inicio, final, 1 ) );
                    semaforosExistentes.Add( new SemaforoPorIntervalo( inicio, final, 1 ) );
                }
            }
            //Se impactar vai adicionar o semáforo atual na lista de aguarde
            //e verifica onde irá impactar para realizar as validações para então decidir se irá criar outro semáforo ou não.
            else
            {
                if(RnValidarAdicionarSemaforoParaAguarde( semaforosImpactadosAguardar, semaforosImpactadosNovos, semaforoPorIntervalo ))
                {
                    SemaforoPorIntervalo.IncrementarContadorSemaforoEmEspera( semaforoPorIntervalo );
                    
                    semaforosImpactadosAguardar.Add( semaforoPorIntervalo );
                }

                //Verifica se o intervalo incorpora outro intervalo dentro dele (o intervalo que está sendo verificado) 
                if(RnVerificarInicioFinalCobremIntervaloAtual( inicio, final, semaforoPorIntervalo ))
                {
                    RnConstruirSemaforoQueAbrangeSemaforoAtual( inicio, final, semaforoPorIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );
                }
                //Se não quer dizer que ou o início ou final estão impactando em algum semáforo.
                else
                {
                    RnConstruirSemaforoQueAbrangeInicioOuFinalSemaforoAtual( inicio, final, semaforoPorIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );
                }
            }
        }

        /// <summary>
        /// Método responsável por analisar a possibilidade de construir um semáforo e construí-lo quando um intervalo abrangir 
        /// outro semáforo por completo, mas ainda assim existir intervalo não incorporados em outro.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforoPorIntervalo">Semáforo atual</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforos criados recentemente</param>
        public static void RnConstruirSemaforoQueAbrangeSemaforoAtual( short inicio, short final, SemaforoPorIntervalo semaforoPorIntervalo,
                                                                       ref List<SemaforoPorIntervalo> semaforosExistentes,
                                                                       ref List<SemaforoPorIntervalo> semaforosImpactadosNovos )
        {
            //Criando início
            int novoFinalIntervalo = semaforoPorIntervalo.inicio - 1;

            List<short> novoIntervalo = CriarIntervalo( inicio, short.Parse( novoFinalIntervalo.ToString() ) );

            RnCriarIntervaloNaoAbrangente( novoIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );

            //Criando final
            int novoInicioIntervalo = semaforoPorIntervalo.final + 1;

            novoIntervalo = CriarIntervalo( short.Parse( novoInicioIntervalo.ToString() ), final );

            RnCriarIntervaloNaoAbrangente( novoIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );
        }

        /// <summary>
        /// Método responsável por analisar a possibilidade de construir um semáforo e construí-lo quando início ou final de um intervalo não estiverem incorporados em outro.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforoPorIntervalo">Semáforo atual</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforos criados recentemente</param>
        public static void RnConstruirSemaforoQueAbrangeInicioOuFinalSemaforoAtual( short inicio, short final, SemaforoPorIntervalo semaforoPorIntervalo,
                                                                                    ref List<SemaforoPorIntervalo> semaforosExistentes,
                                                                                    ref List<SemaforoPorIntervalo> semaforosImpactadosNovos )
        {
            //Verifica se o início está contido no semáforo em comparação, mas o final não está.
            if(RnVerificarInicioEstaContido( inicio, semaforoPorIntervalo ) && final > semaforoPorIntervalo.final)
            {
                int novoInicioIntervalo = semaforoPorIntervalo.final + 1;

                List<short> novoIntervalo = CriarIntervalo( short.Parse( novoInicioIntervalo.ToString() ), final );

                RnCriarIntervaloNaoAbrangente( novoIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );
            }
            //Verifica se o final está contido no semáforo em comparação, mas o início não está.
            else if(inicio < semaforoPorIntervalo.inicio && RnVerificarFinalEstaContido( final, semaforoPorIntervalo ))
            {
                int novoFinalIntervalo = semaforoPorIntervalo.inicio - 1;

                List<short> novoIntervalo = CriarIntervalo( inicio, short.Parse( novoFinalIntervalo.ToString() ) );

                RnCriarIntervaloNaoAbrangente( novoIntervalo, ref semaforosExistentes, ref semaforosImpactadosNovos );
            }
        }

        /// <summary>
        /// Método responsável por criar um novo semáforo caso existe um intervalo não incorporado em outro.
        /// </summary>
        /// <param name="novoIntervalo">Intervalo a ser criado</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforos criados recentemente</param>
        public static void RnCriarIntervaloNaoAbrangente( List<short> novoIntervalo, ref List<SemaforoPorIntervalo> semaforosExistentes, ref List<SemaforoPorIntervalo> semaforosImpactadosNovos )
        {
            List<short> novoIntervaloNaoAbrangente = RnVerificarAbrangenciaIntervalo( novoIntervalo, semaforosExistentes, semaforosImpactadosNovos );

            if(novoIntervaloNaoAbrangente.Count > 0)
            {
                semaforosImpactadosNovos.Add( new SemaforoPorIntervalo( novoIntervaloNaoAbrangente.Min(), novoIntervaloNaoAbrangente.Max(), 1 ) );
                semaforosExistentes.Add( new SemaforoPorIntervalo( novoIntervaloNaoAbrangente.Min(), novoIntervaloNaoAbrangente.Max(), 1 ) );
            }
        }

        /// <summary>
        /// Método responsável por verificar se um determinado intervalo já está contido em algum semáforo
        /// </summary>
        /// <param name="intervalo">Intervalo para verificar</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforo criados recentemente</param>
        /// <returns>Lista referente ao intervalo</returns>
        public static List<short> RnVerificarAbrangenciaIntervalo( List<short> intervalo, List<SemaforoPorIntervalo> semaforosExistentes, List<SemaforoPorIntervalo> semaforosImpactadosNovos )
        {
            List<SemaforoPorIntervalo> intervalosExistentes = new List<SemaforoPorIntervalo>( semaforosImpactadosNovos.Union( semaforosExistentes ) );
            List<short> intervalos = new List<short>();

            for(int i = 0; i < intervalosExistentes.Count; i++)
                intervalos = intervalos.Union( intervalosExistentes[i].intervalo ).ToList();

            return intervalo.Except( intervalos ).ToList();
        }

        /// <summary>
        /// Método responsável por verificar se um determinado intervalo já está contido em algum semáforo
        /// </summary>
        /// <param name="intervalo">Intervalo para verificar</param>
        /// <param name="semaforosExistentes">Lista de semáforos existentes</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforo criados recentemente</param>
        /// <returns>Lista referente ao intervalo</returns>
        public static List<short> RnVerificarAbrangenciaIntervalo( List<short> intervalo,
                                                                   List<SemaforoPorIntervalo> semaforosExistentes,
                                                                   List<SemaforoPorIntervalo> semaforosImpactadosNovos,
                                                                   List<SemaforoPorIntervalo> semaforosImpactadosAguardar )
        {
            List<SemaforoPorIntervalo> intervalosExistentes = new List<SemaforoPorIntervalo>( semaforosImpactadosNovos.Union( semaforosExistentes.Union( semaforosImpactadosAguardar ) ) );

            List<short> intervalos = new List<short>();

            for(int i = 0; i < intervalosExistentes.Count; i++)
                intervalos = intervalos.Union( intervalosExistentes[i].intervalo ).ToList();

            return intervalo.Except( intervalos ).ToList();
        }

        /// <summary>
        /// Método responsável por aguardar todos os semáforos que a atual reordenação irá impactar.
        /// </summary>
        /// <param name="semaforosImpactados">Lista contendo todos os semáforos que serão impactados.</param>
        public static void AguardarSemaforosImpactados( List<SemaforoPorIntervalo> semaforosImpactados )
        {
            for(int i = 0; i < semaforosImpactados.Count; i++)
                semaforosImpactados[i].semaforo.WaitOne();
        }

        #endregion

        #region Validações

        /// <summary>
        /// Método que verifica se o intervalo atual se já está adicionado em alguma lista de aguarde ou de novos semáforos para criar.
        /// </summary>
        /// <param name="semaforosImpactadosAguardar">Lista de semáforos que irá aguardar</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforos que irá criar</param>
        /// <param name="semaforoPorIntervalo">Intervalo que está sendo verificado</param>
        /// <returns>Confirmação se já está adicionado ou não.</returns>
        public static bool RnValidarSemaforosIguaisPorLista( List<SemaforoPorIntervalo> semaforosImpactadosAguardar,
                                                             List<SemaforoPorIntervalo> semaforosImpactadosNovos,
                                                             SemaforoPorIntervalo semaforoPorIntervalo )
        {
            bool adicionado;

            SemaforoPorIntervalo semaforoExistente = semaforosImpactadosAguardar.Where( o => semaforoPorIntervalo.inicio == o.inicio && semaforoPorIntervalo.final == o.final ).FirstOrDefault();
            SemaforoPorIntervalo semaforoNovo = semaforosImpactadosNovos.Where( o => semaforoPorIntervalo.inicio == o.inicio && semaforoPorIntervalo.final == o.final ).FirstOrDefault();

            if(semaforoExistente == null && semaforoNovo == null)
                adicionado = false;
            else
                adicionado = true;

            return adicionado;
        }

        /// <summary>
        /// Método que verifica se o intervalo atual é o mesmo que está sendo comparado, verifica se são iguais.
        /// </summary>
        /// <param name="inicio">Início do intervalo atual</param>
        /// <param name="final">Final do intervalo atual</param>
        /// <param name="semaforoPorIntervalo">Semáforo existente que está sendo comparado no momento</param>
        /// <returns>Confirmação se são ou não iguais</returns>
        public static bool RnValidarIntervalosIguais( short inicio, short final, SemaforoPorIntervalo semaforoPorIntervalo )
        {
            if(inicio == semaforoPorIntervalo.inicio && final == semaforoPorIntervalo.final)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Método que verifica se um intervalo pode ser adicionado na lista de aguarde ou não. 
        /// Caso ele não exista em nenhuma lista, então poderá adicionar.
        /// </summary>
        /// <param name="inicio">Início do intervalo atual</param>
        /// <param name="final">Final do intervalo atual</param>
        /// <param name="semaforosImpactadosAguardar">Lista de semáforos que irá aguardar</param>
        /// <param name="semaforosImpactadosNovos">Lista de semáforos que irá criar</param>
        /// <param name="semaforoPorIntervalo">Semáforo existente que está sendo comparado no momento</param>
        /// <returns>Confirmação se pode adicionar ou não o semáforo na lista de aguarde</returns>
        public static bool RnValidarAdicionarSemaforoParaAguarde( List<SemaforoPorIntervalo> semaforosImpactadosAguardar,
                                                                  List<SemaforoPorIntervalo> semaforosImpactadosNovos,
                                                                  SemaforoPorIntervalo semaforoPorIntervalo )
        {
            bool adicionar;

            if(!RnValidarSemaforosIguaisPorLista( semaforosImpactadosAguardar, semaforosImpactadosNovos, semaforoPorIntervalo ))
                adicionar = true;
            else
                adicionar = false;

            return adicionar;
        }

        /// <summary>
        /// Método responsável por verificar se um intervalo está totalmente contido num semáforo já existente.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforo">Semáforo já existente, o qual desejasse comparar se está contido</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarInicioFinalIntervaloEstaContidoEmOutroIntervalo( short inicio, short final, SemaforoPorIntervalo semaforo )
        {
            if(semaforo != null)
                if(inicio >= semaforo.inicio && final <= semaforo.final)
                    return true;

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se um intervalo está contido em uma lista de semáforo já existentes.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforos">Lista de Semáforos já existente, o qual desejasse comparar se está contido</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarIntervaloEstaContidoEmOutroIntervalo( short inicio, short final, List<SemaforoPorIntervalo> semaforos )
        {
            SemaforoPorIntervalo semaforo = null;

            if(semaforos.Count > 0)
            {
                semaforo = semaforos.Where( o => inicio >= o.inicio && final <= o.final ).FirstOrDefault();

                if(semaforo != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se intervalo está contido no semáforo com intervalo já existente.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforo">Semáforo já existente</param>
        /// <returns>Confirmação se irá ou não impactar</returns>
        public static bool RnVerificarIntervaloImpactara( short inicio, short final, SemaforoPorIntervalo semaforo )
        {
            if(semaforo != null)
            {
                if(inicio >= semaforo.inicio && inicio <= semaforo.final)
                    return true;

                if(final >= semaforo.inicio && final <= semaforo.final)
                    return true;

                if(inicio <= semaforo.inicio && final >= semaforo.final)
                    return true;
                if(inicio >= semaforo.inicio && final <= semaforo.inicio)
                    return true;
                else
                    return false;
            }
            return false;
        }

        /// <summary>
        /// Método responsável por verificar se intervalo está contido na lista de semáforos com intervalos já existentes.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforos">Lista de Semáforos já existentes</param>
        /// <returns>Confirmação se irá ou não impactar</returns>
        public static bool RnVerificarIntervaloImpactara( short inicio, short final, List<SemaforoPorIntervalo> semaforos )
        {
            SemaforoPorIntervalo semaforo = null;

            if(semaforos.Count > 0)
            {
                semaforo = semaforos.Where( o => inicio >= o.inicio && inicio <= o.final ).FirstOrDefault();

                if(semaforo != null)
                    return true;

                semaforo = semaforos.Where( o => final >= o.inicio && final <= o.final ).FirstOrDefault();

                if(semaforo != null)
                    return true;

                semaforo = semaforos.Where( o => inicio <= o.inicio && final >= o.final ).FirstOrDefault();

                if(semaforo != null)
                    return true;
                else
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se início do intervalo está contido no semáforo atual.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="semaforo">Semáforo já existente</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarInicioEstaContido( short inicio, SemaforoPorIntervalo semaforo )
        {
            if(semaforo != null)
                if(inicio >= semaforo.inicio && inicio <= semaforo.final)
                    return true;

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se início do intervalo está contido na lista de semáforos já existentes.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="semaforos">Semáforos já existentes</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarInicioEstaContido( short inicio, List<SemaforoPorIntervalo> semaforos )
        {
            SemaforoPorIntervalo semaforo = null;

            if(semaforos.Count > 0)
            {
                semaforo = semaforos.Where( o => inicio >= o.inicio && inicio <= o.final ).FirstOrDefault();

                if(semaforo != null)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se final do intervalo está contido no semáforo atual.
        /// </summary>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforo">Semáforo já existente</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarFinalEstaContido( short final, SemaforoPorIntervalo semaforo )
        {
            if(semaforo != null)
                if(final >= semaforo.inicio && final <= semaforo.final)
                    return true;

            return false;
        }

        /// <summary>
        /// Método responsável por verificar se final do intervalo está contido na lista de semáforos já existentes.
        /// </summary>
        /// <param name="final">Final do intervalo</param>
        /// <param name="semaforos">Semáforos já existentes</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarFinalEstaContido( short final, List<SemaforoPorIntervalo> semaforos )
        {
            SemaforoPorIntervalo semaforo = null;

            if(semaforos.Count > 0)
            {
                semaforo = semaforos.Where( o => final >= o.inicio && final <= o.final ).FirstOrDefault();

                if(semaforo != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Verificar se um determinado intervalo incorpora outro intervalo, mas ainda contém pontos do intervalo não abrangidos. 
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="SemaforoPorIntervalo">semáforo para validar</param>
        /// <returns>Confirmação se está ou não contido</returns>
        public static bool RnVerificarInicioFinalCobremIntervaloAtual( short inicio, short final, SemaforoPorIntervalo semaforo )
        {
            if(semaforo != null)
                if(inicio < semaforo.inicio && final > semaforo.final)
                    return true;

            return false;
        }


        #endregion

        #region Utilitários

        /// <summary>
        /// Método responsável por ordenar os semáforos de acordo com os intervalos.
        /// </summary>
        /// <param name="semaforosNovos">Lista de semáforos recentemente criados</param>
        /// <param name="semaforosParaAguardar">Lista de semáforos que devem aguardar</param>
        /// <returns>Lista de semáforos ordenados</returns>
        public static List<SemaforoPorIntervalo> OrdenarSemaforos( List<SemaforoPorIntervalo> semaforosNovos, List<SemaforoPorIntervalo> semaforosParaAguardar = null )
        {
            List<SemaforoPorIntervalo> semaforos;

            if(semaforosParaAguardar != null)
                semaforos = semaforosNovos.Union( semaforosParaAguardar ).ToList();
            else
                semaforos = semaforosNovos;

            var semaforosOrdenados = ( from objeto in semaforos
                                       orderby objeto.inicio ascending
                                       select objeto ).ToList();

            return semaforosOrdenados;
        }

        /// <summary>
        /// Método responsável por gerar um intervalo
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <returns></returns>
        public static List<short> CriarIntervalo( short inicio, short final )
        {
            List<short> novoIntervalo = new List<short>();

            SemaforoPorIntervalo.ValidarParametrosCriarIntervalo( ref inicio, ref final );

            int contador = final - inicio;

            for(int i = 0; i <= contador; i++)
            {
                novoIntervalo.Add( inicio );
                inicio++;
            }

            return novoIntervalo;
        }

        /// <summary>
        /// Método responsável por validar o início e final
        /// Caso o final seja menor que o início, o método realiza a troca.
        /// </summary>
        /// <param name="inicio">o ínicio da range</param>
        /// <param name="final">o final da range</param>
        public static void ValidarParametrosCriarIntervalo( ref short inicio, ref short final )
        {
            short auxiliar;

            if(final < inicio)
            {
                auxiliar = final;
                final = inicio;
                inicio = auxiliar;
            }
        }

        /// <summary>
        /// Método responsável por atualizar o contador que indica se há um thread esperando em determinado semáforo.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Objeto (Struct) que contém os dados do semáforo</param>
        public static void IncrementarContadorSemaforoEmEspera( SemaforoPorIntervalo semaforoPorIntervalo )
        {
            semaforoPorIntervalo.semaforoEmEspera.WaitOne();

            semaforoPorIntervalo.emEspera += 1;

            semaforoPorIntervalo.semaforoEmEspera.Release();
        }

        /// <summary>
        /// Método responsável por atualizar o contador que indica se há um thread esperando em determinado semáforo.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Objeto que contém os dados do semáforo</param>
        public static void DecrementarContadorSemaforoEmEspera( SemaforoPorIntervalo semaforoPorIntervalo )
        {
            semaforoPorIntervalo.semaforoEmEspera.WaitOne();

            semaforoPorIntervalo.emEspera -= 1;

            semaforoPorIntervalo.semaforoEmEspera.Release();
        }

        /// <summary>
        /// Método responsável por atualizar o contador que indica se há um thread esperando em determinado semáforo.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Objeto que contém os dados do semáforo</param>
        public static void DecrementarContadorSemaforoEmEspera( List<SemaforoPorIntervalo> semaforosPorIntervalo, int indice = 0 )
        {
            for(int i = indice; i < semaforosPorIntervalo.Count; i++)
            {
                semaforosPorIntervalo[i].semaforoEmEspera.WaitOne();

                semaforosPorIntervalo[i].emEspera -= 1;

                semaforosPorIntervalo[i].semaforoEmEspera.Release();
            }
        }

        /// <summary>
        /// Método responsável por liberar o semáforo.
        /// </summary>
        /// <param name="semaforo">Semáforo que deseja efeturar a liberação (release)</param>
        /// <param name="posicoesLiberadas">Quantidade de posições que serão liberadas no semáforo</param>
        public static void LiberarSemaforo( SemaforoPorIntervalo semaforoPorIntervalo )
        {
            semaforoPorIntervalo.semaforo.Release();
        }

        /// <summary>
        /// Método responsável por liberar um semáforo que está em espera e diminuir o atributo que indicar que ele está em espera.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Semáforo para ser liberado</param>
        public static void LiberarSemaforoEDecrementarContadorSemaforoEmEspera(SemaforoPorIntervalo semaforoPorIntervalo)
        {
            SemaforoPorIntervalo.LiberarSemaforo( semaforoPorIntervalo );
            SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforoPorIntervalo );
        }

        /// <summary>
        /// Método responsável por liberar os semáforos a partir do índice atual dos semáforos.
        /// </summary>
        /// <param name="semaforosPorIntervalo">Lista de semáforos</param>
        /// <param name="indice">Indice da posicao do semáforo</param>
        public static void LiberarSemaforo( List<SemaforoPorIntervalo> semaforosPorIntervalo, int indice )
        {
            for(int i = indice; i < semaforosPorIntervalo.Count; i++)
                SemaforoPorIntervalo.LiberarSemaforo( semaforosPorIntervalo[i] );
        }

        /// <summary>
        /// Método responsável por verificar se o semaforoPorIntervalo1 é diferente do semaforoPorIntervalo2
        /// </summary>
        /// <param name="semaforoPorIntervalo1">Objeto semáforo 1</param>
        /// <param name="semaforoPorIntervalo2">Objeto semáforo 2</param>
        /// <returns>True (Se forem iguais), False (Se não forem iguais)</returns>
        public static bool VerificarSemaforosSaoIguais( SemaforoPorIntervalo semaforoPorIntervalo1, SemaforoPorIntervalo semaforoPorIntervalo2 )
        {
            if(semaforoPorIntervalo1.Oid != semaforoPorIntervalo2.Oid)
                return false;

            return true;
        }

        /// <summary>
        /// Método responsável por liberar recursos do semáforo criado que não possua mais ninguém em espera.
        /// </summary>
        /// <param name="semaforoIntervalo">Objeto SemaforoPorIntevalo</param>
        public static void DestruirSemaforo( SemaforoPorIntervalo semaforoIntervalo )
        {
            if(semaforoIntervalo.emEspera == 0)
            {
                semaforoIntervalo.semaforo.Dispose();
                semaforoIntervalo.semaforo = null;
                semaforoIntervalo = null;
            }
        }

        /// <summary>
        /// Método responsável por Liberar um semáforo e decrementar o contador, que indica se um semáforo está em espera, dos demais semáforos da lista que seria usada.
        /// </summary>
        /// <param name="semaforoPorIntervalo">Semáforo para liberar</param>
        /// <param name="semaforosPorIntervalo">Lista de semáforos para decrementar o contador em espera</param>
        /// <param name="indice">índice em que a lista irá começar a decrementar</param>
        public static void LiberarSemaforoEDecrementarContadoresEmEsperaDosDemaisSemaforos( SemaforoPorIntervalo semaforoPorIntervalo, List<SemaforoPorIntervalo> semaforosPorIntervalo, int indice = 0 )
        {
            SemaforoPorIntervalo.LiberarSemaforo( semaforoPorIntervalo );
            SemaforoPorIntervalo.DecrementarContadorSemaforoEmEspera( semaforosPorIntervalo, indice );
        }

        #endregion

        #region Construtor

        /// <summary>
        /// Construtor que recebe parâmetros e cria semáforo correspondente a determinado intervalo.
        /// </summary>
        /// <param name="inicio">Início do intervalo</param>
        /// <param name="final">Final do intervalo</param>
        /// <param name="posicoesDisponiveis">Posições disponíveis para acesso</param>
        /// <param name="posicoesLimiteAcesso">Limite de posições disponíveis em um semáforo</param>
        public SemaforoPorIntervalo( short inicio, short final, short posicoesDisponiveis = 1, short posicoesLimiteAcesso = 1 )
        {
            semaforo = new Semaphore( posicoesDisponiveis, posicoesLimiteAcesso );
            semaforoEmEspera = new Semaphore( posicoesDisponiveis, posicoesLimiteAcesso );
            Oid = Guid.NewGuid();
            this.inicio = inicio;
            this.final = final;
            intervalo = CriarIntervalo( inicio, final );

            //toda vez que se cria um semáforo siginifica que alguém está usando-o. 
            //Portanto incrementasse o atributo emEspera também. (*ele é incrementado também quando um semáforo irá aguardar um determinado semáforo).
            emEspera = 1;
        }

        #endregion

    }
}
