using System;
using System.Net;

namespace MP.Shared.RestClient.Exceptions
{
	/// <summary>
	/// When IRestResponse has unexpected StatusCode
	/// </summary>
	public class RestResponseStatusCodeException : RestResponseException
	{
		const string MessageFormat = "Error retrieving response for {0} . Status code is {1}: {2}";

		public HttpStatusCode StatusCode { get; set; }
		public string StatusDescription { get; set; }

		public RestResponseStatusCodeException(Uri requestUrl, HttpStatusCode statusCode, string statusDescription)
			: base(string.Format(MessageFormat, requestUrl, statusCode, statusDescription))
		{
			StatusCode = statusCode;
			StatusDescription = statusDescription;
		}
	}
}