using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WexProject.Library.Libs.Extensions.Test
{
    /// <summary>
    /// Classe com métodos de extensão para auxiliar no ambiente de testes efetuando a invocação de métodos privados
    /// </summary>
    public static class PrivateTestHelperExtension
    {
        /// <summary>
        /// Método genérico que invoca um método privado com retorno
        /// </summary>
        /// <typeparam name="TResult">Tipo de retorno</typeparam>
        /// <param name="instancia">instancia atual</param>
        /// <param name="nomeMetodo">Nome do método a ser executado</param>
        /// <param name="parametros">parametros passados para o método</param>
        /// <returns>Retorna o resultado da execução do método</returns>
        public static TResult InvocarMetodoPrivado<TResult>( this object instancia, string nomeMetodo, params object[] parametros )
        {
            Type tipo = instancia.GetType();

            if(string.IsNullOrWhiteSpace(nomeMetodo))
                throw new  ArgumentException("O nome do método passado é inválido.");

            MethodInfo metodo = tipo.GetMethod( nomeMetodo, BindingFlags.Instance | BindingFlags.NonPublic );
            if(metodo == null)
                throw new NullReferenceException(string.Format("Não existe o método privado '{0}' no Objeto do tipo '{1}'",nomeMetodo,tipo.Name));
            return (TResult)metodo.Invoke( instancia, parametros );
        }



        /// <summary>
        /// Método genérico que invoca um método privado SEM retorno
        /// </summary>
        /// <param name="instancia">instancia atual</param>
        /// <param name="nomeMetodo">Nome do método a ser executado</param>
        /// <param name="parametros">parametros passados para o método</param>
        public static void InvocarMetodoPrivado( this object instancia, string nomeMetodo, params object[] parametros )
        {
            Type tipo = instancia.GetType();
            if(string.IsNullOrWhiteSpace( nomeMetodo ))
                throw new ArgumentException( "O nome do método passado é inválido." );

            MethodInfo metodo = tipo.GetMethod( nomeMetodo, BindingFlags.Instance | BindingFlags.NonPublic );
            if(metodo == null)
                throw new NullReferenceException( string.Format( "Não existe o método privado '{0}' no Objeto de tipo '{1}'", nomeMetodo, tipo.Name ) );
            metodo.Invoke( instancia, parametros );
        }

        /// <summary>
        /// Método que permite injetar um valor em uma propriedade privada de um objeto
        /// </summary>
        /// <typeparam name="TObject">Tipo do Objeto</typeparam>
        /// <typeparam name="TProperty">Tipo da propriedade que será injetada</typeparam>
        /// <param name="instancia">instancia do objeto que terá uma propriedade privada setada</param>
        /// <param name="propertyName">nome da propriedade que deverá ser preenchida</param>
        /// <param name="propertyValue">valor da propriedade que devera ser preenchido</param>
        public static void SetPrivateProperty<TObject, TProperty>( this TObject instancia, string propertyName, TProperty propertyValue )
        {
            Type tipo = typeof( TObject );
            var propriedade = tipo.GetProperty( propertyName, BindingFlags.Instance | BindingFlags.NonPublic );

            if(propriedade == null)
                throw new ArgumentException( string.Format( "O objeto do tipo {0} não possui a propriedade {1}", tipo.Name, propertyName ) );
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance;
            tipo.InvokeMember( propertyName, flag, null, instancia, new object[] { propertyValue } );
        }

		/// <summary>
		/// Método que permite capturar um valor de uma propriedade privada de um objeto para auxiliar no ambiente de testes
		/// </summary>
		/// <typeparam name="TObject">tipo do objeto</typeparam>
		/// <param name="instancia">instância atual do objeto</param>
		/// <param name="propertyName" >nome da propriodade selecionada</param>
		/// <returns></returns>
        public static object GetPrivateProperty<TObject>( this TObject instancia, string propertyName )
        {
            Type tipo = typeof( TObject );
            var propriedade = tipo.GetProperty( propertyName, BindingFlags.Instance | BindingFlags.NonPublic );

            if(propriedade == null)
                throw new ArgumentException( string.Format( "O objeto do tipo {0} não possuir a propriedade {1}", tipo.Name, propertyName ) );
            BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance;
            return propriedade.GetValue( instancia, flag, null, new object[] { }, new System.Globalization.CultureInfo( "pt-BR" ) );
        }
    }
}
