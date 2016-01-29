using System;

namespace MP.Shared.RestClient.Exceptions
{
	public abstract class RestResponseException : ApplicationException
	{
		protected RestResponseException(string message)
			: base(message)
		{ }

		protected RestResponseException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}