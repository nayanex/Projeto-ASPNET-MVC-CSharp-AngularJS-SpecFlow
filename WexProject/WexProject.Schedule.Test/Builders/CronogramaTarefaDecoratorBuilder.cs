using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;
using WexProject.BLL.Shared.DTOs.Rh;
using WexProject.Library.Libs.DataHora;
using WexProject.Schedule.Library.Helpers;

namespace WexProject.Schedule.Test.Builders
{
    public class CronogramaTarefaDecoratorBuilder
    {
        CronogramaTarefaDecorator tarefaAtual;
        public CronogramaTarefaDecoratorBuilder()
        {
            tarefaAtual = new CronogramaTarefaDecorator();
        }

        public CronogramaTarefaDecoratorBuilder( CronogramaTarefaDecorator tarefa )
        {
            tarefaAtual = tarefa;
        }

        public CronogramaTarefaDecoratorBuilder Descricao(string Descricao) 
        {
            tarefaAtual.TxDescricaoTarefa = Descricao;
            return this;
        }

        public CronogramaTarefaDecoratorBuilder LinhaBaseSalva( bool status )
        {
            tarefaAtual.CsLinhaBaseSalva = status;
            return this;
        }
        public CronogramaTarefaDecoratorBuilder AtualizadoEm( DateTime atualizadoEm )
        {
            tarefaAtual.DtAtualizadoEm = atualizadoEm;
            return this;
        }
        public CronogramaTarefaDecoratorBuilder DataDeInicio( DateTime dataInicio )
        {
            tarefaAtual.DtInicio = dataInicio;
            return this;
        }
        public CronogramaTarefaDecoratorBuilder EstimativaInicial( Int16 estimativa )
        {
            tarefaAtual.NbEstimativaInicial = estimativa ;
            return this;
        }
        public CronogramaTarefaDecoratorBuilder EstimativaRestante( string estimativa )
        {
            tarefaAtual.NbEstimativaRestante = ConversorTimeSpan.ConverterHorasDeStringParaTicks(estimativa);
            return this;
        }

        public CronogramaTarefaDecoratorBuilder ID( Int16 id )
        {
            tarefaAtual.NbID = id;
            return this;
        }

        public CronogramaTarefaDecoratorBuilder Realizado( string estimativa )
        {
            tarefaAtual.NbRealizado = ConversorTimeSpan.ConverterHorasDeStringParaTicks(estimativa);
            return this;
        }

        public CronogramaTarefaDecoratorBuilder OidCronograma( Guid oidCronograma )
        {
            tarefaAtual.OidCronograma = oidCronograma;
            return this;
        }

        public CronogramaTarefaDecoratorBuilder OidCronogramaTarefa( Guid oid )
        {
            tarefaAtual.OidCronogramaTarefa = oid;
            return this;
        }

        public CronogramaTarefaDecorator Criar() 
        {
            return tarefaAtual;
        }

        public CronogramaTarefaDecoratorBuilder SituacaoPlanejamento( SituacaoPlanejamentoDTO situacao )
        {
            tarefaAtual.OidSituacaoPlanejamentoTarefa = situacao.Oid;
            tarefaAtual.TxDescricaoSituacaoPlanejamentoTarefa = situacao.TxDescricao;
            return this;
        }

        public CronogramaTarefaDecoratorBuilder AtualizadoPor( ColaboradorDto colaborador )
        {
            tarefaAtual.TxAtualizadoPor = colaborador.Login;
            tarefaAtual.TxDescricaoColaborador = colaborador.TxNomeCompletoColaborador;
            return this;
        }

    }
}
