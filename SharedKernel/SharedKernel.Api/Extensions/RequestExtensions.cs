using System.IO;
using System.Net.Http;
using System.Web;

namespace SharedKernel.Api.Extensions
{
    public static class RequestExtensions
    {
        public const string ROOT_FILE_UPLOADS = "~/App_Data/Temp/FileUploads";

        public static byte[] GetFileBytes(this HttpRequestMessage request, string fileName)
        {
            var root = HttpContext.Current.Server.MapPath(ROOT_FILE_UPLOADS);

            Directory.CreateDirectory(root);

            var provider = new MultipartFormDataStreamProvider(root);
            var result = request.Content.ReadAsMultipartAsync(provider).Result;

            byte[] foto = null;

            foreach (var file in result.FileData)
            {
                if (!file.Headers.ContentDisposition.Name.Contains(fileName)) continue;

                foto = File.ReadAllBytes(file.LocalFileName);
                File.Delete(file.LocalFileName);
                break;
            }

            return foto;
        }
    }
}
