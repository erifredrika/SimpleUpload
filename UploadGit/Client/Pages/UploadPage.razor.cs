using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UploadGit.Shared;

namespace UploadGit.Client.Pages
{
    public partial class UploadPage
    {
        [Inject] public HttpClient HttpClient { get; set; }
        public FileRequest FileRequest = new();
        public string Message { get; set; }

        public async Task UploadFile()
        {
            var result = await HttpClient.PostAsJsonAsync<FileRequest>("api/upload/single-file", FileRequest);

            if (result.IsSuccessStatusCode)
            {
                Message = result.Content.ReadAsStringAsync().Result;
                StateHasChanged();
            }           
        }
    }
}
