using System;
using MP.Shared.RestClient.Exceptions;
using RestSharp;

namespace MP.Shared.RestClient
{
	public static class RestResponseExtensions
	{
		public static IRestResponse Validate(this IRestResponse response, Uri requestUrl)
		{
			if (response.ErrorException != null)
				throw new RestResponseExecutionTimeException(requestUrl, response.ResponseStatus, response.ErrorException);

			if (response.ResponseStatus != ResponseStatus.Completed)
				throw new RestResponseResponseStatusException(requestUrl, response.ResponseStatus);

			if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300 )
			{
				throw new RestResponseStatusCodeException(requestUrl, response.StatusCode, response.StatusDescription);
			}

			return response;
		}
	}
}