using System;
using System.Collections.Generic;
using WexProject.BLL.Models.Execucao;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace WexProject.Module.Win.TelasForaPadrao.Execucao.UserControl
{
    /// <summary>
    /// User Control de Itens Pendentes
    /// </summary>
    public partial class ListaItensPendentesCicloUserControl : XtraUserControl
    {
        /// <summary>
        /// Indica se está no MouseDown de Prioridades
        /// </summary>
        private bool lvPrioridades_mdown = false;

        /// <summary>
        /// Indica se está no MouseDown de Próximo Ciclo
        /// </summary>
        private bool lvProximoCiclo_mdown = false;

        /// <summary>
        /// Último controle usado
        /// </summary>
        private Control lastControl;

        /// <summary>
        /// Estórias selecionadas na lista
        /// </summary>
        private List<CicloDesenvEstoria> selecionadas;

        /// <summary>
        /// Ciclo atual
        /// </summary>
        public CicloDesenv Ciclo
        {
            get;
            set;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public ListaItensPendentesCicloUserControl()
        {
            InitializeComponent();
            LostFocusControls();
        }

        /// <summary>
        /// DragEnter de Lista de Prioridades
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstPrioridade_DragEnter(object sender, DragEventArgs e)
        {
            if (selecionadas.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// DragDrop de Lista de Prioridades
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstPrioridade_DragDrop(object sender, DragEventArgs e)
        {
            if (Ciclo != null)
            {
                Ciclo.RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(selecionadas);

                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                lvPrioridades_mdown = false;
                lvProximoCiclo_mdown = false;

                selecionadas = new List<CicloDesenvEstoria>();
            }
        }

        /// <summary>
        /// Mover o mouse na Lista de Prioridades
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstPrioridade_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lvPrioridades_mdown || e.Button == MouseButtons.Right) return;

            SelecionaItens(LstPrioridade);

            if (selecionadas.Count <= 0) return;

            LstPrioridade.DoDragDrop(selecionadas, DragDropEffects.Move);
        }

        /// <summary>
        /// MouseDown na Lista de Prioridades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstPrioridade_MouseDown(object sender, MouseEventArgs e)
        {
            lvPrioridades_mdown = true;
        }

        /// <summary>
        /// DragEnter da Lista de Próximo Ciclo
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstProximoCiclo_DragEnter(object sender, DragEventArgs e)
        {
            if (selecionadas.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// DragDrop na Lista de Próximo Ciclo
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstProximoCiclo_DragDrop(object sender, DragEventArgs e)
        {
            if (Ciclo != null)
            {
                Ciclo.RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(selecionadas);

                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                lvPrioridades_mdown = false;
                lvProximoCiclo_mdown = false;

                selecionadas = new List<CicloDesenvEstoria>();
            }
        }

        /// <summary>
        /// Mover o mouse na Lista de Próximo Ciclo
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void LstProximoCiclo_MouseMove(object sender, MouseEventArgs e)
        {
            if (!lvProximoCiclo_mdown || e.Button == MouseButtons.Right) return;

            SelecionaItens(LstProximoCiclo);

            if (selecionadas.Count <= 0) return;

            LstProximoCiclo.DoDragDrop(selecionadas, DragDropEffects.Move);
        }

        /// <summary>
        /// MouseDown na Lista de Próximo Ciclo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstProximoCiclo_MouseDown(object sender, MouseEventArgs e)
        {
            lvProximoCiclo_mdown = true;
        }

        /// <summary>
        /// Passa os itens selecionados da Lista atual para um coleção interna
        /// </summary>
        /// <param name="lst">Objeto de ListBoxControl</param>
        private void SelecionaItens(ListBoxControl lst)
        {
            selecionadas = new List<CicloDesenvEstoria>();

            for (int position = 0; position < lst.SelectedItems.Count; position++)
            {
                selecionadas.Add(lst.SelectedItems[position] as CicloDesenvEstoria);
            }
        }

        /// <summary>
        /// Solicitação de subir itens selecionados
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void SubirBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                List<CicloDesenvEstoria> selecionadas = new List<CicloDesenvEstoria>();

                if (lastControl.Equals(LstPrioridade) && LstPrioridade.SelectedItems.Count > 0)
                {
                    for (int position = 0; position < LstPrioridade.SelectedItems.Count; position++)
                    {
                        selecionadas.Add(LstPrioridade.SelectedItems[position] as CicloDesenvEstoria);
                    }

                    Ciclo.RnTrocarPosicoesListaPendentes(selecionadas, 1, true);
                    LstPrioridade.SelectedItem = selecionadas[0];
                }
                else if (lastControl.Equals(LstProximoCiclo) && LstProximoCiclo.SelectedItems.Count > 0)
                {
                    for (int position = 0; position < LstProximoCiclo.SelectedItems.Count; position++)
                    {
                        selecionadas.Add(LstProximoCiclo.SelectedItems[position] as CicloDesenvEstoria);
                    }

                    Ciclo.RnTrocarPosicoesListaPendentes(selecionadas, 0, true);
                    LstProximoCiclo.SelectedItem = selecionadas[0];
                }

                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Solicitação de descer itens selecionados
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void DescerBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                List<CicloDesenvEstoria> selecionadas = new List<CicloDesenvEstoria>();

                if (lastControl.Equals(LstPrioridade) && LstPrioridade.SelectedItems.Count > 0)
                {
                    for (int position = 0; position < LstPrioridade.SelectedItems.Count; position++)
                    {
                        selecionadas.Add(LstPrioridade.SelectedItems[position] as CicloDesenvEstoria);
                    }

                    Ciclo.RnTrocarPosicoesListaPendentes(selecionadas, 1, false);
                    LstPrioridade.SelectedItem = selecionadas[0];
                }
                else if (lastControl.Equals(LstProximoCiclo) && LstProximoCiclo.SelectedItems.Count > 0)
                {
                    for (int position = 0; position < LstProximoCiclo.SelectedItems.Count; position++)
                    {
                        selecionadas.Add(LstProximoCiclo.SelectedItems[position] as CicloDesenvEstoria);
                    }

                    Ciclo.RnTrocarPosicoesListaPendentes(selecionadas, 0, false);
                    LstProximoCiclo.SelectedItem = selecionadas[0];
                }

                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Solicitação de mover itens para a Lista à direita
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void MoverDireitaBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                List<CicloDesenvEstoria> selecionadas = new List<CicloDesenvEstoria>();

                for (int position = 0; position < LstPrioridade.SelectedItems.Count; position++)
                {
                    selecionadas.Add(LstPrioridade.SelectedItems[position] as CicloDesenvEstoria);
                }

                Ciclo.RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(selecionadas);
                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Solicitação de mover itens para a Lista à esquerda
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void MoverEsquerdaBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                List<CicloDesenvEstoria> selecionadas = new List<CicloDesenvEstoria>();

                for (int position = 0; position < LstProximoCiclo.SelectedItems.Count; position++)
                {
                    selecionadas.Add(LstProximoCiclo.SelectedItems[position] as CicloDesenvEstoria);
                }

                Ciclo.RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(selecionadas);
                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Solicitação de mover todos os itens para à direita
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void MoverTodosDireitaBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                Ciclo.RnEnviarEstoriasSelecionadasDePrioridadesParaProximoCiclo(Ciclo._ListaPrioridades);
                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Solicitação de mover todos os itens para à esquerda
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void MoverTodosEsquerdaBtn_Click(object sender, EventArgs e)
        {
            if (Ciclo != null)
            {
                Cursor = Cursors.WaitCursor;

                Ciclo.RnEnviarEstoriasSelecionadasDeProximoCicloParaPrioridades(Ciclo._ListaProximoCiclo);
                LstProximoCiclo.Refresh();
                LstPrioridade.Refresh();

                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// LostFocus de um Controle
        /// </summary>
        /// <param name="sender">Objeto</param>
        /// <param name="e">Evento</param>
        private void AllLostFocus(object sender, EventArgs e)
        {
            lastControl = (Control)sender;
        }

        /// <summary>
        /// Indentificação do último controle usado
        /// </summary>
        private void LostFocusControls()
        {
            LstPrioridade.LostFocus += new EventHandler(AllLostFocus);
            LstProximoCiclo.LostFocus += new EventHandler(AllLostFocus);
        }

        /// <summary>
        /// Verifica em que posição está o cursor para habilitar ou não os botões.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">args</param>
        private void LstPrioridade_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstPrioridade.SelectedIndex == 0)
                SubirBtn.Enabled = false;
            else
                SubirBtn.Enabled = true;

            if (LstPrioridade.SelectedIndex == (LstPrioridade.ItemCount - 1))
                DescerBtn.Enabled = false;
            else
                DescerBtn.Enabled = true;

            LstProximoCiclo.SelectedIndex = -1;
        }

        /// <summary>
        /// Verifica em que posição está o cursor para habilitar ou não os botões.
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">args</param>
        private void LstProximoCiclo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LstProximoCiclo.SelectedIndex == 0)
                SubirBtn.Enabled = false;
            else
                SubirBtn.Enabled = true;

            if (LstProximoCiclo.SelectedIndex == (LstProximoCiclo.ItemCount - 1))
                DescerBtn.Enabled = false;
            else
                DescerBtn.Enabled = true;

            LstPrioridade.SelectedIndex = -1;
        }

        /// <summary>
        /// Verificar se existe itens na lista para mostrar o enviar todos
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">args</param>
        private void LstProximoCiclo_Click(object sender, EventArgs e)
        {
            if (LstProximoCiclo.ItemCount == 0)
                MoverTodosEsquerdaBtn.Enabled = false;
            else
                MoverTodosEsquerdaBtn.Enabled = true;
        }

        /// <summary>
        /// Verificar se existe itens na lista para mostrar o enviar todos
        /// </summary>
        /// <param name="sender">objeto sender</param>
        /// <param name="e">args</param>
        private void LstPrioridade_Click(object sender, EventArgs e)
        {
            if (LstPrioridade.ItemCount == 0)
                MoverTodosDireitaBtn.Enabled = false;
            else
                MoverTodosDireitaBtn.Enabled = true;
        }
    }
}