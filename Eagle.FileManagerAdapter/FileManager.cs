using Eagle.FileManagerAdapter.Domain;
using Elk.Core;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace Eagle.FileManagerAdapter
{
    public class FileManager
    {
        public static IResponse<FMDownloadResponse> Download(string baseFileManagerUrl, Guid fileId, FileType fileType)
        {
            try
            {
                using var http = new HttpClient();
                var call = http.GetAsync($"{baseFileManagerUrl}/file/GetFileUrl?FileID={fileId.ToString()}&FileType={fileType.ToString()}").Result;
                if (!call.IsSuccessStatusCode) new Response<FMDownloadResponse> {Message = "file manager not respond" };
                var rep = JsonConvert.DeserializeObject<OldActionResponse<FMDownloadResponse>>(call.Content.ReadAsStringAsync().Result);
                return new Response<FMDownloadResponse>
                {
                    IsSuccessful = rep.IsSuccessfull,
                    Result = rep.Result
                };

            }
            catch (Exception e)
            {
                return new Response<FMDownloadResponse> { Message = e.Message };

            }
        }

        public static IResponse<FMUploadResponse> Upload(string baseFileManagerUrl, string fmUn, string fmPw, IFormFile file)
        {
            var content = new MultipartFormDataContent();
            var un = new StringContent(fmUn, Encoding.UTF8, "text/plain");
            content.Add(un, "username");

            var pw = new StringContent(fmPw, Encoding.UTF8, "text/plain");
            content.Add(pw, "password");
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var byteArrayContent = new ByteArrayContent(fileBytes);
                byteArrayContent.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName, Name = "postedFile" };
                content.Add(byteArrayContent, "postedFile");
                using (var _httpClient = new HttpClient())
                {
                    var callResult = _httpClient.PostAsync($"{baseFileManagerUrl}/file/UploadHttpFile", content).Result;
                    if (callResult.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var rep = callResult.Content.ReadAsStringAsync().Result;
                        var repModel = rep.DeSerializeJson<OldActionResponse<FMUploadResponse>>();
                        return new Response<FMUploadResponse>
                        {
                            Result = repModel.Result,
                            IsSuccessful = repModel.IsSuccessfull,
                            Message = repModel.Message
                        };
                    }
                    else
                    {
                        return new Response<FMUploadResponse> { Message = "can't upload file" };
                    }
                }

            }

        }

    }
}
