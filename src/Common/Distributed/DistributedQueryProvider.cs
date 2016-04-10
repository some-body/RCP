using Newtonsoft.Json;
using System.Net;

namespace Distributed
{
    public class DistributedQueryProvider
    {
        public string BackendUri { get; private set; }

        public DistributedQueryProvider(string backendUri)
        {
            BackendUri = backendUri;
        }

        protected TResult MakePostQuery<TResult>(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            string stringResult = "";

            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                data = StringEncodingConvert(data, "WINDOWS-1251", "UTF-8");
                stringResult = webClient.UploadString(queryUrl, "POST", data);
            }

            stringResult = StringEncodingConvert(stringResult, "WINDOWS-1251", "UTF-8");
            return JsonConvert.DeserializeObject<TResult>(stringResult);
        }

        protected void MakePostQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, data);
            }
        }

        protected void MakePutQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, "PUT", data);
            }
        }

        protected void MakePatchQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, "PATCH", data);
            }
        }

        protected TResult MakeGetQuery<TResult>(string uri)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            string stringResult = "";

            using (var webClient = new WebClient())
                stringResult = webClient.DownloadString(queryUrl);

            stringResult = StringEncodingConvert(stringResult, "WINDOWS-1251", "UTF-8");
            return JsonConvert.DeserializeObject<TResult>(stringResult);
        }

        protected void MakeGetQuery(string uri)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
                webClient.DownloadString(queryUrl);
        }

        public static string StringEncodingConvert(string strText, string strSrcEncoding, string strDestEncoding)
        {
            System.Text.Encoding srcEnc = System.Text.Encoding.GetEncoding(strSrcEncoding);
            System.Text.Encoding destEnc = System.Text.Encoding.GetEncoding(strDestEncoding);
            byte[] bData = srcEnc.GetBytes(strText);
            return destEnc.GetString(bData);
        }
    }
}