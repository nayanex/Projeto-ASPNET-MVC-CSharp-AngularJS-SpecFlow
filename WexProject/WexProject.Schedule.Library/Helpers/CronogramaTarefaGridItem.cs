using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WexProject.BLL.Shared.DTOs.Planejamento;
using System.Drawing;
using WexProject.Schedule.Library.Properties;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using WexProject.Library.Libs.DataHora.Extension;

namespace WexProject.Schedule.Library.Helpers
{
    public class CronogramaTarefaGridItem : CronogramaTarefaDecorator
    {
        /// <summary>
        /// Evento disparado quando há alguma alteração no icone da tarefa
        /// </summary>
        public static event EventHandler AoAlterarIconeTarefa;
        /// <summary>
        /// Armazenar o icone que irá ser apresentado no grid de cronograma tarefa
        /// </summary>
        public Image Icone { get; private set; }

        /// <summary>
        /// Armazear se o texto deverá ser apresentado tachado
        /// </summary>
        public bool EmExclusao { get; private set; }

        /// <summary>
        /// Cor da tarefa
        /// </summary>
        public int? Cor { get; set; }

        /// <summary>
        /// Exibir por extenso o valor da estimativa realizada
        /// </summary>
        public string TxRealizado 
        {
            get { return Realizado.PorExtenso(); }
        }

        /// <summary>
        /// Exibir por extenso o valor da estimativa restante
        /// </summary>
        public string TxRestante 
        {
            get { return EstimativaRestante.PorExtenso(); }
            set {  }
        }

        /// <summary>
        /// Exibir por extenso o valor da estimativa inicial
        /// </summary>
        public string TxEstimativaInicial 
        {
            get 
            {
                return string.Format("{0}h",NbEstimativaInicial);
            }
            set 
            {
                string valorFormatado = value.Replace( 'h','\0' );
                NbEstimativaInicial = Convert.ToInt16( valorFormatado );
            } 
        }

        /// <summary>
        /// Remover o icone adicionado a linha do grid
        /// </summary>
        /// <param name="forcarAtualizacao"> informar se deve ou não forçar a atualização da linha do grid</param>
        public void RemoverIcone( bool forcarAtualizacao = false )
        {
            Icone = null;
            EmExclusao = false;
            if(forcarAtualizacao)
                AoAlterarIconeTarefa( this, new EventArgs() );
        }

        /// <summary>
        /// Método que adiciona um icone de criação
        /// </summary>
        public void AdicionarIconeCriacao()
        {
            Icone = Resources.doc2_16x16;
        }

        /// <summary>
        /// Adicionar icone de exclusão
        /// </summary>
        /// <param name="icone">image que represente uma exclusão</param>
        public void AdicionarIconeExcluir()
        {
            Icone = Resources.excluir;
            EmExclusao = true;
            if(AoAlterarIconeTarefa != null)
                AoAlterarIconeTarefa( this, new EventArgs() );
        }

        /// <summary>
        /// Adicionar o icone de movimentação
        /// </summary>
        /// <param name="posicaoFinal">posição final da tarefa no grid</param>
        /// <param name="movimento">tipo de movimento Acima/Abaixo</param>
        public void AdicionarIconeMovimentacao( int posicaoInicial, int posicaoFinal )
        {
            if(NbID != posicaoFinal)
            {
                Image imagem;
                if(posicaoInicial > posicaoFinal)
                    imagem = Resources.seta_acima3264;
                else
                    imagem = Resources.seta_abaixo3264;

                EscreverPosicaoFinal( posicaoFinal.ToString(), imagem );
                Icone = imagem;
            }
            else
                Icone = null;
            AoAlterarIconeTarefa( this, new EventArgs() );
        }

        /// <summary>
        /// Escreve o texto em uma imagem
        /// </summary>
        /// <param name="texto">texto a ser escrito na imagem</param>
        /// <param name="imagem">imagem</param>
        private static void EscreverPosicaoFinal( string texto, Image imagem )
        {
            using(Graphics graphics = Graphics.FromImage( imagem ))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.CompositingMode = CompositingMode.SourceOver;
                using(Font font = new Font( "Tahoma", 9, FontStyle.Bold ))
                {
                    graphics.DrawString( texto, font, Brushes.DarkBlue, new PointF( 22, 6 ) );
                }
            }
        }

        /// <summary>
        /// Obter uma cópia de valores do item atual
        /// </summary>
        /// <returns></returns>
        public CronogramaTarefaGridItem Clone()
        {
            return base.MemberwiseClone() as CronogramaTarefaGridItem;
        }
    }
}
