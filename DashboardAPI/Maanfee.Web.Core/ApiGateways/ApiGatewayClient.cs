using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
	public class ApiGatewayClient : IApiGatewayClient
	{
		public ApiGatewayClient(HttpClient http)
		{
			Http = http;
		}

		private readonly HttpClient Http;

		public async Task<CallbackResult<TResponse>> PostAsync<TData, TResponse>(string Url, TData data)
		{
			try
			{
				var PostResult = await Http.PostAsync(Url,
					new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
				if (PostResult.IsSuccessStatusCode)
				{
					var JsonResult = JsonConvert.DeserializeObject<TResponse>(await PostResult.Content.ReadAsStringAsync());
					if (JsonResult != null)
					{
						return new CallbackResult<TResponse>(JsonResult, null);
					}
					else
					{
						return new CallbackResult<TResponse>(default, new ExceptionError(JsonResult.ToString()));
					}
				}
				else
				{
					return new CallbackResult<TResponse>(default, new ExceptionError(await PostResult.Content.ReadAsStringAsync()));
				}
			}
			catch (Exception ex)
			{
				return new CallbackResult<TResponse>(default, new ExceptionError(ex.Message));
			}
		}

		private static string UrlCombine(string baseUrl, params string[] segments)
				=> string.Join("/", new[] { baseUrl.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
	}
}
