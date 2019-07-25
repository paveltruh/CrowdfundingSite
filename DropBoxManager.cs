using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace WebApplication1
{
    public class DropBoxManager
    {
        DropboxClient dropboxClient;
        public DropBoxManager()
        {
            dropboxClient = new DropboxClient("JAIXvLr3ULAAAAAAAAAAJBPze-dji8m4LY43vlgjaI2zzfddDcILCJbckSW1Aagu");
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
