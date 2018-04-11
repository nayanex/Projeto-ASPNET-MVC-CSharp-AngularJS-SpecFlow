using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WexProject.Schedule.Win.Views.SplashScreen
{
    /// <summary>
    /// Classe do splashScreen
    /// </summary>
    public partial class SplashScreen : Form
    {
        #region Constructors
        /// <summary>
        /// Construtor
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cria os parametros necessários para fundo transparente
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00080000; // Required: set WS_EX_LAYERED extended style
                return cp;
            }
        }

        /// <summary>
        /// Atualiza nosso forme
        /// </summary>
        /// <param name="backgroundImage">imagem de background do form</param>
        public void UpdateFormDisplay(Image backgroundImage)
        {
            IntPtr screenDc = API.GetDC(IntPtr.Zero);
            IntPtr memDc = API.CreateCompatibleDC(screenDc);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr oldBitmap = IntPtr.Zero;

            try
            {
                //Display-image
                Bitmap bmp = new Bitmap(backgroundImage);
                hBitmap = bmp.GetHbitmap(Color.FromArgb(0));  //Set the fact that background is transparent
                oldBitmap = API.SelectObject(memDc, hBitmap);

                //Display-rectangle
                Size size = bmp.Size;
                Point pointSource = new Point(0, 0);
                Point topPos = new Point(this.Left, this.Top);

                //Set up blending options
                API.BLENDFUNCTION blend = new API.BLENDFUNCTION();
                blend.BlendOp = API.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = 255;
                blend.AlphaFormat = API.AC_SRC_ALPHA;

                API.UpdateLayeredWindow(this.Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0, ref blend, API.ULW_ALPHA);

                //Clean-up
                bmp.Dispose();
                API.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    API.SelectObject(memDc, oldBitmap);
                    API.DeleteObject(hBitmap);
                }
                API.DeleteDC(memDc);
            }
            catch (Exception)
            {
            }
        } 

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="sender">objeto</param>
        /// <param name="e">Argumentos de evento</param>
        private void SplashScreen_Load(object sender, EventArgs e)
        {
            UpdateFormDisplay(this.BackgroundImage);
        }

        /// <summary>
        /// Ao pintar
        /// </summary>
        /// <param name="e">opções de argumento</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //Call our drawing function
            UpdateFormDisplay(this.BackgroundImage);
        }
        #endregion

        #region API
        /// <summary>
        /// api necessária
        /// </summary>
        internal class API
        {
            /// <summary>
            /// Constante do fundo
            /// </summary>
            public const byte AC_SRC_OVER = 0x00;

            /// <summary>
            /// Constante de alpha da imagem
            /// </summary>
            public const byte AC_SRC_ALPHA = 0x01;

            /// <summary>
            /// Hexadecimal do alpha
            /// </summary>
            public const Int32 ULW_ALPHA = 0x00000002;

            /// <summary>
            /// Função para o blend da imagem
            /// </summary>
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct BLENDFUNCTION
            {
                /// <summary>
                /// Blend Superior
                /// </summary>
                public byte BlendOp;

                /// <summary>
                /// Estilos de BlendOp
                /// </summary>
                public byte BlendFlags;

                /// <summary>
                /// Fonte do alpha
                /// </summary>
                public byte SourceConstantAlpha;

                /// <summary>
                /// formato do alpha
                /// </summary>
                public byte AlphaFormat;
            }


            /// <summary>
            /// Atualiza o layer da janela
            /// </summary>
            /// <param name="hwnd">IntPtr</param>
            /// <param name="hdcDst">IntPtr</param>
            /// <param name="pptDst">Pontos</param>
            /// <param name="psize">Pontos</param>
            /// <param name="hdcSrc">Header</param>
            /// <param name="pprSrc">Pontos</param>
            /// <param name="crKey">Keys</param>
            /// <param name="pblend">lend</param>
            /// <param name="dwFlags">bandeiras</param>
            /// <returns>true or false</returns>
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

            /// <summary>
            /// Resgatda DC
            /// </summary>
            /// <param name="hWnd">hand</param>
            /// <returns>true or false</returns>
            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            /// <summary>
            /// Cria um DC compatível
            /// </summary>
            /// <param name="hDC">hand DC</param>
            /// <returns>Int Ptr</returns>
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            /// <summary>
            /// Cria a parte do DC
            /// </summary>
            /// <param name="hWnd">hand</param>
            /// <param name="hDC">hander</param>
            /// <returns>inteiro</returns>
            [DllImport("user32.dll", ExactSpelling = true)]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            /// <summary>
            /// Apagar DC
            /// </summary>
            /// <param name="hdc">hand</param>
            /// <returns>true or false</returns>
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool DeleteDC(IntPtr hdc);

            /// <summary>
            /// Selecionador de Objetos
            /// </summary>
            /// <param name="hDC">hand</param>
            /// <param name="hObject">hand Object</param>
            /// <returns>Inteiro|Ponto</returns>
            [DllImport("gdi32.dll", ExactSpelling = true)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            /// <summary>
            /// Apaga o objeto
            /// </summary>
            /// <param name="hObject">hand</param>
            /// <returns>true or false</returns>
            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern bool DeleteObject(IntPtr hObject);
        }
        #endregion
    }
}
