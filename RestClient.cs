using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;

namespace MP.Shared.RestClient
{
	/// <summary>
	/// Facade wrapper around RestSharp.RestClient and partiotioned 
	/// by response type to rest/entity clients.
	/// To create an instance use fluent fabric inner class 
	/// or subtly DefaultFactory which is all-in client
	/// </summary>
	public class RestClient : IEntityClient
	{
		private readonly RestSharp.RestClient _client;
		private Action<IRestRequest> _onBeforeRequestExecution;

		private RestClient()
		{
			_client = new RestSharp.RestClient();
		}

		#region IRestClient implementation

		public Uri BuildUri(IRestRequest request)
		{
			return _client.BuildUri(request);
		}

		public CookieContainer CookieContainer
		{
			get { return _client.CookieContainer; }
		}

		public IRestResponse ExecuteRestRequest(IRestRequest request)
		{
			if (_onBeforeRequestExecution != null)
				_onBeforeRequestExecution(request);

			var requestUrl = _client.BuildUri(request);
			var response = _client.Execute(request);
			response.Validate(requestUrl);

			return response;
		}

		public IRestResponse<T> ExecuteRestRequest<T>(IRestRequest request) 
			where T: new()
		{
			if (_onBeforeRequestExecution != null)
				_onBeforeRequestExecution(request);

			var requestUrl = _client.BuildUri(request);
			request.OnBeforeDeserialization = responseBeforeDeserialization => responseBeforeDeserialization.Validate(requestUrl);
			return _client.Execute<T>(request);
		}

		public async Task<IRestResponse> ExecuteRestRequestAsync(IRestRequest request)
		{
			if (_onBeforeRequestExecution != null)
				_onBeforeRequestExecution(request);

			var requestUrl = _client.BuildUri(request);
			var response = await _client.ExecuteTaskAsync(request).ConfigureAwait(false);
			response.Validate(requestUrl);

			return response;
		}

		public async Task<IRestResponse<T>> ExecuteRestRequestAsync<T>(IRestRequest request)
		{
			if (_onBeforeRequestExecution != null)
				_onBeforeRequestExecution(request);

			var requestUrl = _client.BuildUri(request);
			request.OnBeforeDeserialization = responseBeforeDeserialization => responseBeforeDeserialization.Validate(requestUrl);
			return await _client.ExecuteTaskAsync<T>(request).ConfigureAwait(false);
		}

		#endregion

		#region IEntityClient implementation

		public string ExecuteRawJsonRequest(IRestRequest request)
		{
			var response = ExecuteRestRequest(request);
			return response.Content;
		}

		public T ExecuteEntityRequest<T>(IRestRequest request)
			where T: new()
		{
			var response = ExecuteRestRequest<T>(request);
			return response.Data;
		}

		public async Task<string> ExecuteRawJsonRequestAsync(IRestRequest request)
		{
			var response = await ExecuteRestRequestAsync(request).ConfigureAwait(false);
			return response.Content;
		}

		public async Task<T> ExecuteEntityRequestAsync<T>(IRestRequest request)
		{
			var response = await ExecuteRestRequestAsync<T>(request).ConfigureAwait(false);
			return response.Data;
		}

		#endregion

		/// <summary>
		/// Every returned factory owns unique cookie container. 
		/// Container's shared only between clients generated by this factory object
		/// </summary>
		public static EntityClientFactory GetDefaultFactory()
		{
			var sharedByFactoryCookieContainer = new CookieContainer();
			return baseUrl => new RestClientFluentFactory(baseUrl)
				.UseJsonNetDeserializer()
				.UseJsonNetSerializer()
				.UseCookieContainer(sharedByFactoryCookieContainer)
				.Build();
		}

		public class RestClientFluentFactory
		{
			/// <summary>
			/// It's supposed to be thread safe. Let's believe it.
			/// </summary>
			private static readonly RestSharpJsonNetSerializer JsonNetSerializer = new RestSharpJsonNetSerializer();
			/// <summary>
			/// It's supposed to be thread safe. Let's believe it.
			/// </summary>
			private static readonly RestSharpJsonNetDeserializer JsonNetDeserializer = new RestSharpJsonNetDeserializer();

			private readonly RestClient _restClient;

			public RestClientFluentFactory()
			{
				_restClient = new RestClient();
			}

			public RestClientFluentFactory(string baseUrl)
				: this()
			{
				WithBaseUrl(baseUrl);
			}

			public RestClientFluentFactory(Uri baseUrl)
				: this()
			{
				WithBaseUrl(baseUrl);
			}

			public RestClient Build()
			{
				return _restClient;
			}

			public RestClientFluentFactory WithBaseUrl(string url)
			{
				_restClient._client.BaseUrl = new Uri(url);
				return this;
			}

			public RestClientFluentFactory WithBaseUrl(Uri url)
			{
				_restClient._client.BaseUrl = url;
				return this;
			}

			public RestClientFluentFactory UseCookieContainer(CookieContainer cookieJar)
			{
				_restClient._client.CookieContainer = cookieJar;
				return this;
			}

			public RestClientFluentFactory UseJsonNetDeserializer()
			{
				_restClient._client.AddHandler("application/json", JsonNetDeserializer);
				return this;
			}

			public RestClientFluentFactory UseJsonNetSerializer()
			{
				_restClient._onBeforeRequestExecution = request =>
				{
					request.RequestFormat = DataFormat.Json;
					request.JsonSerializer = JsonNetSerializer;
				};
				return this;
			}
		}
	}
}