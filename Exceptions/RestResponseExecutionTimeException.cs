using System;
using RestSharp;

namespace MP.Shared.RestClient.Exceptions
{
	/// <summary>
	/// When IRestResponse has ErrorException property != null
	/// </summary>
	public class RestResponseExecutionTimeException : RestResponseException
	{
		const string MessageFormat = "Error retrieving response for {0} . Response status is {1}. Check inner details for more info";
		public ResponseStatus ResponseStatus { get; set; }

		public RestResponseExecutionTimeException(Uri requestUrl, ResponseStatus responseStatus, Exception innerException)
			: base(string.Format(MessageFormat, requestUrl, responseStatus), innerException)
		{
			ResponseStatus = responseStatus;
		}
	}
}