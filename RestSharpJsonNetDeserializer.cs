using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace MP.Shared.RestClient
{
	public class RestSharpJsonNetDeserializer : IDeserializer
	{
		public T Deserialize<T>(IRestResponse response)
		{
			return JsonConvert.DeserializeObject<T>(response.Content);
		}

		/// <summary>
		/// Unused for JSON Deserialization
		/// </summary>
		public string DateFormat { get; set; }
		/// <summary>
		/// Unused for JSON Deserialization
		/// </summary>
		public string RootElement { get; set; }
		/// <summary>
		/// Unused for JSON Deserialization
		/// </summary>
		public string Namespace { get; set; }
	}
}