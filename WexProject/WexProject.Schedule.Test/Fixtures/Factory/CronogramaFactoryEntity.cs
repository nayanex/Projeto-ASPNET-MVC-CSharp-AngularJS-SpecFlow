using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WexProject.BLL;
using WexProject.BLL.Contexto;
using WexProject.BLL.Entities.Outros;
using WexProject.BLL.Entities.Planejamento;
using WexProject.BLL.Entities.RH;
using WexProject.BLL.Shared.Domains.Planejamento;
using WexProject.Library.Libs.DataHora;


namespace WexProject.Schedule.Test.Fixtures.Factory
{
    public class CronogramaFactoryEntity
    {


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

        /// <summary>
        /// Factory para criação de cronograma
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="txDescricao">descrição para descrição do cronograma</param>
        /// <param name="situacaoPlanejamento">Setar a situação do cronograma</param>
        /// <param name="dtInicio">data inicio do cronograma</param>
        /// <param name="dtFinal">data final do cronograma</param>
        /// <param name="save">verificação se é necessário salvar o objeto</param>
        /// <returns>Objeto de Cronograma</returns>
        public static Cronograma CriarCronograma(   WexDb contexto, 
                                                    String txDescricao, 
                                                    SituacaoPlanejamento situacaoPlanejamento,
                                                    DateTime dtInicio, 
                                                    DateTime dtFinal, 
                                                    bool save = true )
        {
            Cronograma cronograma = new Cronograma();

            cronograma.TxDescricao = txDescricao;
            cronograma.OidSituacaoPlanejamento = situacaoPlanejamento.Oid;
            cronograma.SituacaoPlanejamento = situacaoPlanejamento;

            if(dtFinal != null)
                cronograma.DtFinal = dtFinal;
            else
                cronograma.DtFinal = DateTime.Now;

            if(dtInicio != null)
                cronograma.DtInicio = dtInicio;
            else
                cronograma.DtInicio = DateTime.Now;

            if(save)
            {
                contexto.Cronograma.Add( cronograma );
                contexto.SaveChanges();
            }

            return cronograma;
        }


        /// <summary>
        /// Factory para criação de situação planejamento
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="txDescricao">descrição para descrição da situação planejamento</param>
        /// <param name="csSituacao">enum de situacao</param>
        /// <param name="csTipo">enum de tipo</param>
        /// <param name="padraoSistema">Verifica se é padrão do sistema</param>
        /// <param name="save">verificação se é necessário salvar o objeto</param>
        /// <returns>Objeto de Situação Planejamento</returns>
        public static SituacaoPlanejamento CriarSituacaoPlanejamento( WexDb contexto, 
                                                                      String txDescricao = "",
                                                                      CsTipoSituacaoPlanejamento csSituacao = CsTipoSituacaoPlanejamento.Ativo, 
                                                                      CsTipoPlanejamento csTipo = CsTipoPlanejamento.Execução,
                                                                      CsPadraoSistema padraoSistema = CsPadraoSistema.Não, 
                                                                      bool save = true )
        {

            SituacaoPlanejamento situacaoPlanejamento = new SituacaoPlanejamento
            {
                TxDescricao = txDescricao,
                CsPadrao = padraoSistema,
                TxKeys = ((Shortcut) (contadorAtalhos + seedShortcut)).ToString(),
                KeyPress = contadorAtalhos + seedShortcut,
                CsSituacao = csSituacao,
                CsTipo = csTipo
            };

            if(save)
            {
                contexto.SituacaoPlanejamento.Add( situacaoPlanejamento );
                contexto.SaveChanges();
            }
            contadorAtalhos++;

            return situacaoPlanejamento;
        }

        /// <summary>
        /// Factory para crição do objeto de criação de tarefas
        /// </summary>
        /// <param name="contexto">contexto do banco</param>
        /// <param name="nbId">descrição para id da tarefa</param>
        /// <param name="txDescricao">descrição para nome da tarefa</param>
        /// <param name="txObservacao">descrição para observação da tarefa</param>
        /// <param name="situacaoPlanejamento">situação da tarefa</param>
        /// <param name="estimativaInicial">estimativa inicial da tarefa</param>
        /// <param name="realizado">Número de horas realizada na tarefa</param>
        /// <param name="colaborador">Colaborador atrelado a tarefa</param>
        /// <param name="cronograma">Cronograma da tarefa</param>
        /// <param name="tarefaSelecionada">verifica se nenhuma tarefa foi selecionada</param>
        /// <param name="save">verificação se é necessário salvar o objeto</param>
        /// <returns>Objeto de Tarefa</returns>
        public static CronogramaTarefa CriarTarefa( WexDb contexto, ushort nbId, String txDescricao, String txObservacao,
            SituacaoPlanejamento situacaoPlanejamento, Int16 estimativaInicial, TimeSpan realizado,
            Colaborador colaborador, Cronograma cronograma, CronogramaTarefa tarefaSelecionada, bool save = false )
        {
            CronogramaTarefa tarefa = new CronogramaTarefa() { Tarefa = new Tarefa() { SituacaoPlanejamento = new SituacaoPlanejamento() } };

            tarefa.Cronograma = cronograma;

            if(situacaoPlanejamento != null)
                tarefa.Tarefa.SituacaoPlanejamento = situacaoPlanejamento;

            tarefa.Tarefa.TxDescricao = txDescricao;
            tarefa.Tarefa.TxObservacao = txObservacao;
            tarefa.Tarefa.DtInicio = new DateTime( 2011, 11, 01 );
            tarefa.Tarefa.NbEstimativaInicial = estimativaInicial;
            tarefa.Tarefa.TxResponsaveis = colaborador.NomeCompleto;
            tarefa.Tarefa.EstimativaRealizadoHora = realizado;
            tarefa.Tarefa.AtualizadoPor = colaborador;

            if(save)
            {
                contexto.CronogramaTarefa.Add( tarefa );
                contexto.SaveChanges();
            }

            return tarefa;
        }

        /// <summary>
        /// Criando Factory para criação de novos usuário no sistema
        /// </summary>
        /// <param name="contexto">Contexto do banco</param>
        /// <param name="txUserName">descrição para nome de usuário</param>
        /// <param name="txFirstName">descrição para primeiro nome do usuário</param>
        /// <param name="txLastName">descrição para último nome do usuário</param>
        /// <param name="txEmail">descrição para email do usuário</param>
        /// <param name="save">verificação se é necessário salvar o objeto</param>
        /// <returns>Objeto de Usuário</returns>
        public static User CriarUsuario( WexDb contexto, String txUserName, String txFirstName, String txLastName, String txEmail, bool save = true )
        {
            User usuario = new User()
            {
               Person = new Person() { Party = new Party() }
            };

            usuario.UserName = txUserName;
            usuario.Person.FirstName = txFirstName;
            usuario.Person.LastName = txLastName;
            usuario.Person.Email = txEmail;

            DateUtil.CurrentDateTime = DateTime.MinValue;
            
            //UserDao.CurrentUser = usuario;

            if(save)
            {
                contexto.Usuario.Add( usuario );
                contexto.SaveChanges();
            }

            return usuario;
        }
    }
}
