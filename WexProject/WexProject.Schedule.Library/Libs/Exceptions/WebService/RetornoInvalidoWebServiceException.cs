using System;

namespace WexProject.Schedule.Library.Libs.Exceptions.WebService
{
	[Serializable]
	public class RetornoInvalidoWebServiceException : Exception
	{
		public RetornoInvalidoWebServiceException()
		{
		}

		public RetornoInvalidoWebServiceException( string message )
			: base( message )
		{
		}

		public RetornoInvalidoWebServiceException( string message, Exception inner )
			: base( message, inner )
		{
		}

		protected RetornoInvalidoWebServiceException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context )
			: base( info, context ) { }
	}
}