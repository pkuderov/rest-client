using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace MP.Shared.RestClient
{
	public interface IRestClient
	{
		Uri BuildUri(IRestRequest request);
		CookieContainer CookieContainer { get; }

		IRestResponse ExecuteRestRequest(IRestRequest request);
		IRestResponse<T> ExecuteRestRequest<T>(IRestRequest request) where T: new();

		Task<IRestResponse> ExecuteRestRequestAsync(IRestRequest request);
		Task<IRestResponse<T>> ExecuteRestRequestAsync<T>(IRestRequest request);
	}

	public delegate IRestClient RestClientFactory(Uri baseUrl);
}