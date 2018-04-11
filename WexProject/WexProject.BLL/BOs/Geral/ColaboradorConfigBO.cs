using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WexProject.BLL.BOs.Geral
{
    public class ColaboradorConfigBo
    {
        /// <summary>
        /// Array de cores pré-definidas.
        /// </summary>
        private static Color[] cores = {
                                           Color.Aqua,
                                           Color.Beige,
                                           Color.BlueViolet,
                                           Color.Blue,
                                           Color.Brown,
                                           Color.BurlyWood,
                                           Color.CadetBlue,
                                           Color.Chartreuse,
                                           Color.Chocolate,
                                           Color.Crimson,
                                           Color.DarkCyan,
                                           Color.DarkGreen,
                                           Color.DarkRed,
                                           Color.DeepSkyBlue,
                                           Color.YellowGreen,
                                           Color.Yellow,
                                           Color.Wheat,
                                           Color.Violet,
                                           Color.Turquoise,
                                           Color.Tomato,
                                           Color.Thistle,
                                           Color.SlateGray,
                                           Color.Silver,
                                           Color.RosyBrown,
                                           Color.Plum,
                                           Color.Peru,
                                           Color.PeachPuff,
                                           Color.PaleVioletRed,
                                           Color.PaleGoldenrod 
                                       };

        /// <summary>
        /// Retornar a lista de cores para seleção
        /// </summary>
        public static Color[] Cores
        {
            get { return cores; }
        }

        /// <summary>
        /// Método responsável por atribuir cores unicas a seleção da nova cor
        /// </summary>
        /// <param name="coresSelecionadas">lista de cores selecionadas</param>
        /// <returns>nova cor atribuida</returns>
        public static string SelecionarCor( List<string> coresSelecionadas )
        {
            int quantidadeColaboradores = coresSelecionadas.Count;
            string cor;
            if(quantidadeColaboradores <= cores.Length - 1)
            {
                cor = cores[quantidadeColaboradores].ToArgb().ToString();
            }
            else
            {
                Random randomGenerate = new Random();
                Color randomColor;

                do
                {
                    randomColor = Color.FromArgb( randomGenerate.Next( 255 ), randomGenerate.Next( 255 ), randomGenerate.Next( 255 ) );
                    cor = randomColor.ToArgb().ToString();
                } while(coresSelecionadas.Contains( cor ));
            }

            return cor;
        }

    }
}
