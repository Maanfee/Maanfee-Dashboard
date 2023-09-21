using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Maanfee.Web.Core
{
    public class FilesManagerService :IFilesManagerService
    {
        public FilesManagerService(HttpClient http)
        {
			Http = http;
        }

		private HttpClient Http;

		public async Task<bool> UploadFileChunk(string Url, FileChunk FileChunk)
        {
            try
            {
                var result = await Http.PostAsJsonAsync(Url, FileChunk);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                return Convert.ToBoolean(responseBody);
            }
            catch //(Exception ex)
            {
                return false;
            }
        } 

    }
}
