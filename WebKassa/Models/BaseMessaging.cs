using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using WebKassa.Models.Request;
using WebKassa.Models.Response;

namespace WebKassa.Models
{
    public abstract class BaseMessaging
    {
        private Uri _baseUri;
        private string _apiToken;
        public BaseMessaging(Uri baseUri, string apiToken)
        {
            _baseUri = baseUri;
            _apiToken = apiToken;
        }

        public virtual Uri UriCompare(Uri uri, Dictionary<string, string> uriParts)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var pair in uriParts)
            {
                query.Add(pair.Key, pair.Value);
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        public virtual async Task<U> SendRequestAsync<U, T>(HttpMethod method, T? options, string endPoint, Dictionary<string, string>? parametres = null)
        {
            using HttpClient client = new HttpClient();
            Uri requestUri = new Uri(_baseUri.ToString() + endPoint);

            if (parametres != null)
                requestUri = UriCompare(requestUri, parametres);

            HttpRequestMessage requestMessage = new HttpRequestMessage(method, requestUri);

            string content = string.Empty;

            if (options != null)
            {
                content = JsonSerializer.Serialize(options);
            }

            requestMessage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
            requestMessage.Content = new StringContent(content, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.SendAsync(requestMessage);
                string responseData = await response.Content.ReadAsStringAsync() ?? string.Empty;

                if ((int)response.StatusCode >= 200 & (int)response.StatusCode < 300)
                {
                    return JsonSerializer.Deserialize<U>(responseData);
                }
                else
                {
                    if (!string.IsNullOrEmpty(responseData))
                    {
                        var exception = JsonSerializer.Deserialize<WebKassaException>(responseData);
                        exception.StatusCode = (int)response.StatusCode;
                        throw new Exception($"{exception?.Exception.ErrorMessageForClient}, StatusCode: {exception?.StatusCode}");
                    }
                    throw new Exception($"WebKassa exception. StatusCode: {(int)response.StatusCode}");
                }


            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
