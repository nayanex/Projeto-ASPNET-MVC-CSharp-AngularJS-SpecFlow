using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WexProject.Library.Libs.Img
{
    public class ImageUtil
    {

        /// <summary>
        /// Metodo de Redimensionamento de foto
        /// </summary>
        /// <param name="original">foto original</param>
        /// <returns>foto redimensionada</returns>
        public static Image ResizeImage( Image original, int altura, int largura )
        {
            try
            {
                Bitmap modificada = new Bitmap( original, largura, altura );

                Rectangle rect = new Rectangle( 0, 0, largura, altura );

                modificada = modificada.Clone( rect, System.Drawing.Imaging.PixelFormat.Format32bppArgb );

                return modificada;
            }
            catch(Exception e)
            {
                System.Console.WriteLine( e.Message );
            }

            return original;
        }

        /// <summary>
        /// Método utilizado para redimensionar imagens
        /// </summary>
        /// <param name="original">Imagem original</param>
        /// <param name="altura">Nova altura</param>
        /// <param name="largura">Nova largura</param>
        /// <returns></returns>
        public static Image RedimensionarImagem( Image original, int altura, int largura )
        {
            if(original == null)
                 return null;
            Bitmap novaImagem = new Bitmap( largura, altura );
            using(Graphics gr = Graphics.FromImage( novaImagem ))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage( original, new Rectangle( 0, 0, largura, altura ) );
            }
            return novaImagem;
        }

        /// <summary>
        /// Método responsável por converter uma imagem para array de bytes
        /// </summary>
        /// <param name="imagem">Imagem que deseja converter</param>
        /// <returns>Array de Bytes que contém a imagem</returns>
        public static byte[] ConverterImagemParaBytes( Image imagem )
        {
            if(imagem == null)
                return null;
            MemoryStream memoryStream = new MemoryStream();
            ImageFormat imagemFormato = imagem.RawFormat;
            imagem.Save( memoryStream, imagemFormato );

            return memoryStream.ToArray();
        }

        /// <summary>
        /// Método para conversão para escala de cinza
        /// </summary>
        /// <param name="source">imagem</param>
        /// <returns></returns>
        public static Bitmap ConvertToGrayscale( Bitmap source )
        {
            ColorMatrix matrix = new ColorMatrix( new float[][]{
                new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                new float[] {     0,      0,      0, 1, 0},
                new float[] {     0,      0,      0, 0, 0}
            } );

            //Create our image to convert.
            Image image = (Bitmap)source.Clone();
           
            //Create the ImageAttributes object and apply the ColorMatrix
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix( matrix );
           
            //Create a new Graphics object from the image.
            Graphics graphics = Graphics.FromImage( image );
           
            //Draw the image using the ImageAttributes we created.
            graphics.DrawImage( image,new Rectangle( 0, 0, image.Width, image.Height ), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes );
           
            //Dispose of the Graphics object.
            graphics.Dispose();
            return (Bitmap)image;
        }

        /// <summary>
        /// Método responsável por uma cor de fonte em contraste com a cor de fundo
        /// </summary>
        /// <param name="corDeFundo">Cor de fundo</param>
        /// <returns></returns>
        public static Color DefinirCorFonteEmConstraste( Color corDeFundo)
        {
            return DefinirCorFonteEmConstraste( corDeFundo, Color.White, Color.Black );
        }

        /// <summary>
        /// Método responsável por uma cor de fonte em contraste com a cor de fundo
        /// </summary>
        /// <param name="corDeFundo">Cor de fundo</param>
        /// <param name="corFonteClara">Cor de fonte quando fundo for escuro</param>
        /// <param name="corFonteEscuro">Cor de fonte quando fundo for claro</param>
        /// <returns></returns>
        public static Color DefinirCorFonteEmConstraste( Color corDeFundo ,Color corFonteClara,Color corFonteEscuro)
        {
            //Counting luminance
            double a = 1 - ( 0.299 * corDeFundo.R + 0.587 * corDeFundo.G + 0.114 * corDeFundo.B ) / 255;
            if(a < 0.5)
                return corFonteEscuro;
            else
                return corFonteClara;
        }
    }
}
