using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UploadGit.Server.Utilities;
using UploadGit.Shared;

namespace UploadGit.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        public UploadController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Route("single-file")]
        public async Task<IActionResult> UploadSingleFile([FromBody]FileRequest fileRequest)
        {
            string destinationPath = $"{_hostEnvironment.ContentRootPath}/Uploads/{fileRequest.FileName}";

            try
            {
                using var targetStream = System.IO.File.Create(destinationPath);
                await targetStream.WriteAsync(fileRequest.Byte);
                await targetStream.DisposeAsync();

                var git = new GitCaller("cmd.exe", $@"{_hostEnvironment.ContentRootPath}/Uploads");

                var response = "";
                response = git.Run($@"/c git add {fileRequest.FileName} & git status");
                response = git.Run($@"/c git commit -m'uploaded-{fileRequest.FileName}'");
            
                return Ok(response);
              
            }
            catch (Exception e)
            {
                return BadRequest($"Something went wrong trying to upload" +
                    $" '{fileRequest.FileName}'." +
                    $" \n Error Message: {e.Message}");
            }
        }
    }
}
