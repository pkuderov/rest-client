using System;
using System.Threading.Tasks;
using RestSharp;

namespace MP.Shared.RestClient
{
	/*
	 * Maybe it's better to separate IEntityClient from IRestClient
	 */

	public interface IEntityClient : IRestClient
	{
		string ExecuteRawJsonRequest(IRestRequest request);
		T ExecuteEntityRequest<T>(IRestRequest request) where T: new ();

		Task<string> ExecuteRawJsonRequestAsync(IRestRequest request);
		Task<T> ExecuteEntityRequestAsync<T>(IRestRequest request);
	}
	
	public delegate IEntityClient EntityClientFactory(Uri baseUrl);
}