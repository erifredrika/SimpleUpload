using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UploadGit.Shared;

namespace UploadGit.Client.Shared
{
    public partial class Upload
    {
        [Parameter] public FileRequest FileRequest { get; set; }
        [Parameter] public EventCallback EventCallback { get; set; }
        [Parameter] public string Message { get; set; }

        IBrowserFile SelectedFile;

        private void OnFileSelected(InputFileChangeEventArgs e)
        {
            SelectedFile = e.File;

            StateHasChanged();
        }

        private async Task UploadFile()
        {
            if (SelectedFile != null)
            {             
                Stream stream = SelectedFile.OpenReadStream();
                MemoryStream ms = new();
                await stream.CopyToAsync(ms);

                FileRequest.Byte = ms.ToArray();
                FileRequest.FileName = WebUtility.HtmlEncode(SelectedFile.Name);

                await EventCallback.InvokeAsync();                 
            }
        }
    }
}
