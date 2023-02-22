using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maanfee.Web.Core
{
	//public class ApiGatewayClient : IApiGatewayClient
	//{
	//	public ApiGatewayClient(HttpClient http)
	//	{
	//		Http = http;
	//	}

	//	private readonly HttpClient Http;

	//	#region - PostAsJsonAsync Usage -

	//	//var PostResult = await ApiGatewayClient.PostAsJsonAsync<JwtLoginViewModel>("http://localhost:4030/gateway/...", Model.TrimString());
	//	//if (PostResult.IsSuccessStatusCode)
	//	//{
	//	//	var JsonResult = await PostResult.Content.ReadFromJsonAsync<JwtAuthenticationViewModel>();
	//	//	if (JsonResult != null)
	//	//	{
	//	//		Snackbar.Add(JsonResult.Token, Severity.Error);
	//	//	}
	//	//	else
	//	//	{
	//	//		Snackbar.Add(JsonResult.ErrorMessage, Severity.Error);
	//	//	}
	//	//}
	//	//else
	//	//{
	//	//	Snackbar.Add(PostResult.Content.ReadAsStringAsync().Result, Severity.Error);
	//	//}

	//	#endregion

	//	public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string Url, T value)
	//	{
	//		var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
	//		return await Http.PostAsync(Url, stringContent);
	//	}

	//	#region - PostAsJsonAsync Usage -

	//	//var ApiResult = await ApiGatewayClient.PostAsJsonAsync<JwtLoginViewModel, JwtAuthenticationViewModel>("http://localhost:4030/gateway/...", Model.TrimString());

	//	#endregion

	//	public async Task<CallbackResult<TResponse>> PostAsJsonAsync<TData, TResponse>(string Url, TData data)
	//	{
	//		try
	//		{
	//			var PostResult = await Http.PostAsync(Url,
	//				new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"));
	//			if (PostResult.IsSuccessStatusCode)
	//			{
	//				var JsonResult = JsonConvert.DeserializeObject<TResponse>(await PostResult.Content.ReadAsStringAsync());
	//				if (JsonResult != null)
	//				{
	//					return new CallbackResult<TResponse>(JsonResult, null);
	//				}
	//				else
	//				{
	//					return new CallbackResult<TResponse>(default, new ExceptionError(JsonResult.ToString()));
	//				}
	//			}
	//			else
	//			{
	//				return new CallbackResult<TResponse>(default, new ExceptionError(await PostResult.Content.ReadAsStringAsync()));
	//			}
	//		}
	//		catch (Exception ex)
	//		{
	//			return new CallbackResult<TResponse>(default, new ExceptionError(ex.Message));
	//		}
	//	}

	//	#region - PutAsJsonAsync Usage -

	//	//var PustResult = await ApiGatewayClient.PutAsJsonAsync<JwtLoginViewModel>("http://localhost:4030/gateway/...", Model.TrimString());
	//	//if (PutResult.IsSuccessStatusCode)
	//	//{
	//	//    var JsonResult = await PutResult.Content.ReadFromJsonAsync<CallbackResult<SubmitRoleViewModel>>();
	//	//    if (JsonResult.Data != null)
	//	//    {
	//	//        Snackbar.Add(JsonResult.SuccessMessage ?? DashboardResource.MessageSavedSuccessfully, Severity.Success);
	//	//        MudDialog.Close(DialogResult.Ok(SubmitRoleViewModel));
	//	//    }
	//	//    else
	//	//    {
	//	//        Snackbar.Add(MessageHandler.ErrorHandler(JsonResult.Error), Severity.Error);
	//	//    }
	//	//}

	//	#endregion

	//	public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string Url, T value)
	//	{
	//		var stringContent = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");
	//		return await Http.PutAsync(Url, stringContent);
	//	}

	//	#region - GetFromJsonAsync Usage -

	//	//var Callback = await Http.GetFromJsonAsync<CallbackResult<Group>>($"api/Groups/Details/{Id}");

	//	//if (Callback.Data != null)
	//	//{
	//	//	Details = Callback.Data;
	//	//}
	//	//else
	//	//{
	//	//	Snackbar.Add(Callback.Error.ToString(), Severity.Error);
	//	//}

	//	#endregion

	//	public async Task<T> GetFromJsonAsync<T>(string Url)
	//	{
	//		return JsonConvert.DeserializeObject<T>(await Http.GetStringAsync(Url));
	//	}

	//	#region - Usage -



	//	#endregion

	//	// ************************************************

	//	private static string UrlCombine(string baseUrl, params string[] segments)
	//			=> string.Join("/", new[] { baseUrl.TrimEnd('/') }.Concat(segments.Select(s => s.Trim('/'))));
	//}
}
