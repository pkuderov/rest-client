using System;
using RestSharp;

namespace MP.Shared.RestClient.Exceptions
{
	/// <summary>
	/// When IRestResponse has ResponseStatus != Completed
	/// </summary>
	public class RestResponseResponseStatusException : RestResponseException
	{
		const string MessageFormat = "Error retrieving response for {0} . Response status is {1}";

		public RestResponseResponseStatusException(Uri requestUrl, ResponseStatus responseStatus)
			: base(string.Format(MessageFormat, requestUrl, responseStatus))
		{
		}
	}
}