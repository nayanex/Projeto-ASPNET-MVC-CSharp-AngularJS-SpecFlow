using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WexProject.Library.Libs.Str
{
    /// <summary>
    /// Classe utilitária com funções para String.
    /// </summary>
    public class StrUtil
    {
        /// <summary>
        /// Expressão regular para detectar se uma string é um
        /// número inteiro.
        /// </summary>
        private static Regex numberExpr = new Regex(@"^\d+$");

        /// <summary>
        /// Copia X caracteres de uma string a partir do início.
        /// </summary>
        /// <param name="str">String de origem</param>
        /// <param name="length">Quantidade de caracteres que será copiada</param>
        /// <returns>Cópia da string até a quantidade de caracteres 
        /// especificados em length    </returns>
        public static string Copy(string str, int length)
        {
            if (str == null)
                return "";

            if (str.Length > length)
                return str.Substring(0, length);
            else
                return str;
        }
        /// <summary>
        /// Retorna verdadeiro o conteúdo da string
        /// passada como parâmetro for um número inteiro.
        /// </summary>
        /// <param name="strInt">String contendo o número inteiro</param>
        /// <returns>Verdadeiro se for um número inteiro</returns>
        public static bool IsInteger(string strInt)
        {
            Match m = numberExpr.Match(strInt);
            return m.Success;
        }

        /// <summary>
        /// Coloca a primeira letra da palavra em maiúsculo
        /// </summary>
        /// <param name="palavra">Palavra</param>
        /// <returns>Palavra com a primeira letra maiúscula</returns>
        public static string UpperCaseFirst(string palavra)
        {
            if (string.IsNullOrEmpty(palavra))
                return string.Empty;

            char[] letras = palavra.ToCharArray();
            letras[0] = char.ToUpper(letras[0]);

            return new string(letras);
        }

        /// <summary>
        /// Coloca a primeira letra da palavra em minúsculo
        /// </summary>
        /// <param name="palavra">Palavra</param>
        /// <returns>Palavra com a primeira letra minúsculo</returns>
        public static string LowerCaseFirst(string palavra)
        {
            if (string.IsNullOrEmpty(palavra))
                return string.Empty;

            char[] letras = palavra.ToCharArray();
            letras[0] = char.ToLower(letras[0]);

            return new string(letras);
        }

        /// <summary>
        /// Verifica se a string possui caracteres em branco no começo ou final e retira os espaços em branco
        /// </summary>
        /// <param name="texto">Texto</param>
        /// <returns>Um texto sem espaços no começo ou final ou em branco</returns>
        public static string RetirarEspacoVazio(string texto)
        {
            if (texto != null)
            {
                texto = texto.TrimStart().TrimEnd();
            }
            else
            {
                texto = "";
            }
            return texto;
        }

        /// <summary>
        /// Caso o texto seja maior que o tamanho da coluna
        /// limita o tamanho do texto à coluna 
        /// </summary>
        /// <param name="tamanhoColuna">TamanhoColuna</param>
        /// <param name="texto">texto</param>
        /// <returns>Retorna uma string q possui no máximo o tamanho da coluna e no final acrescenta reticiencias</returns>
        public static string LimitarTamanhoColuna(int tamanhoColuna, string texto)
        {
            string[] textoArray = texto.Split('\n');
            texto = "";
            for (int i = 0; i < textoArray.Length; i++)
            {
                if (textoArray[i].Length > tamanhoColuna)
                {
                    texto += textoArray[i].Substring(0, (tamanhoColuna - 3));
                    texto += "...";
                }
                else
                {
                    texto += textoArray[i];
                }
            }

            return texto;
        }

        /// <summary>
        /// Retirada de conteúdo desnecessário de texto que possui separador
        /// </summary>
        /// <param name="texto">Texto a ser retirado o conteúdo desnecessário</param>
        /// <param name="separador">Separador do texto</param>
        /// <returns>Texto sem o conteúdo desnecessário</returns>
        public static string RetirarConteudoDesnecessarioStringComSeparador(string texto, char separador)
        {
            string novoTexto = string.Empty;

            foreach(string parte in texto.Split(separador))
            {
                if (!string.IsNullOrEmpty(parte.Trim()))
                {
                    novoTexto += String.Format("{0}{1}", parte.Trim(), separador);
                }
            }

            if (novoTexto.Length > 0)
            {
                novoTexto = novoTexto.Substring(0, novoTexto.Length - 1);
            }

            return novoTexto;
        }

		/// <summary>
		/// Insere espaços entre palavras em uma string em Pascal Case
		/// </summary>
		/// <param name="texto">Texto em Pascal Case sem espaço entra as palavras</param>
		/// <returns>Texto com palavras separadas por espaços</returns>
		public static string InserirEspacosStringPascalCase(string texto)
		{
			string novoTexto = "";

			foreach (char letra in texto)
			{
				if (Char.IsUpper(letra) && novoTexto.Length != 0)
				{
					novoTexto += " " + letra;
				}
				else
				{
					novoTexto += letra;
				}
			}

			return novoTexto;
		}

        public static string NormalizarAcentuacao(string palavra)
        {
            Dictionary<string, string[]> filtroCaracteres = new Dictionary<string, string[]>
            {
                {"i", new[] {"í", "ì"}},
                {"a", new[] {"á", "à", "â", "ã"}},
                {"e", new[] {"é", "ê"}},
                {"o", new[] {"ó", "ô", "õ", "ò"}},
                {"u", new[] {"ú"}}
            };
            palavra = palavra.ToLower();
            return filtroCaracteres.Aggregate(palavra, (current1, vogal) => vogal.Value.Aggregate(current1, (current, caracter) => current.Replace(caracter, vogal.Key)));
        }

    }
}