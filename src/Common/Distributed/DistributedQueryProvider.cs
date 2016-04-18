using Newtonsoft.Json;
using System.Net;
using System.Text;

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
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                stringResult = webClient.UploadString(queryUrl, "POST", data);
            }

            return JsonConvert.DeserializeObject<TResult>(stringResult);
        }

        protected void MakePostQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, data);
            }
        }

        protected void MakePutQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, "PUT", data);
            }
        }

        protected void MakePatchQuery(string uri, string data)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                webClient.UploadString(queryUrl, "PATCH", data);
            }
        }

        protected TResult MakeGetQuery<TResult>(string uri)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            string stringResult = "";

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                stringResult = webClient.DownloadString(queryUrl);
            }

            return JsonConvert.DeserializeObject<TResult>(stringResult);
        }

        protected void MakeGetQuery(string uri)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                webClient.DownloadString(queryUrl);
            }
        }

        protected QueryResult MakeDeleteQuery(string uri)
        {
            var queryUrl = string.Concat(BackendUri, uri);
            string stringResult = "";

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.GetEncoding("UTF-8");
                stringResult = webClient.UploadString(queryUrl, "DELETE", "");
            }

            return JsonConvert.DeserializeObject<QueryResult>(stringResult);
        }

        protected virtual string EntityToString(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }

        private static string StringEncodingConvert(string strText, string strSrcEncoding, string strDestEncoding)
        {
            System.Text.Encoding srcEnc = System.Text.Encoding.GetEncoding(strSrcEncoding);
            System.Text.Encoding destEnc = System.Text.Encoding.Default; //System.Text.Encoding.GetEncoding(strDestEncoding);
            byte[] bData = srcEnc.GetBytes(strText);
            return destEnc.GetString(bData);
        }
    }
}