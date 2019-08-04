using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApplication1
{
    public class DropBoxManager
    {
        private readonly DropboxClient dropboxClient;

        public DropBoxManager(string AccessToken)
        {
            dropboxClient = new DropboxClient(AccessToken);
        }

        public async Task<byte[]> Download(string folder, string fileName)
        {
            byte[] file; 
            using (var response = await dropboxClient.Files.DownloadAsync(folder + "/" + fileName))
            {
                file = await response.GetContentAsByteArrayAsync();
            }
            return file;
        }

        public async Task Upload(string folder, string fileName, byte[] content)
        {
            using (var mem = new MemoryStream(content))
            {
                var updated = await dropboxClient.Files.UploadAsync(
                    "/" + folder + "/" + fileName,
                    WriteMode.Overwrite.Instance,
                    body: mem);
            }

        }

        public async Task<string> Upload(string folder, string fileName, IFormFile content)
        {
            byte[] byteArray;
            string Url;
            using (var ms = new MemoryStream())
            {
                content.CopyTo(ms);
                byteArray = ms.ToArray();
            }

            using (var mem = new MemoryStream(byteArray))
            {
                //await content.CopyToAsync(mem);

                var updated = await dropboxClient.Files.UploadAsync(
                    "/" + folder + "/" + fileName,
                    WriteMode.Overwrite.Instance,
                    body: mem);
                var a = await dropboxClient.Sharing.CreateSharedLinkWithSettingsAsync("/" + folder + "/" + fileName);
                Url = (a).Url;
            }
            return Url.Substring(0, Url.Length - 1) + "1";

        }

        public async Task Upload(string folder, string fileName, string content)
        {
            using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var updated = await dropboxClient.Files.UploadAsync(
                    "/" + folder + "/" + fileName,
                    WriteMode.Overwrite.Instance,
                    body: mem);
            }

        }
    }
}
