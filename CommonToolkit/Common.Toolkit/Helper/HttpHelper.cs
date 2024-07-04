using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Formatting = Newtonsoft.Json.Formatting;
using Parameter = RestSharp.Parameter;

namespace Common.Toolkit.Helper
{

    public static class HttpHelper
    {
        #region Get
        public static async Task<TResult> GetAsync<TResult>(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            var result = await GetAsync(url, parameter, Headers, timeout, statusErrorHandle, readBodyWhenErr);
            return JsonConvert.DeserializeObject<TResult>(result ?? string.Empty);
        }
        public static async Task<string> GetAsync(string url, Dictionary<string, object> parameter = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            RestClient client;
            RestRequest request;
            GetInit(url, parameter, Headers, timeout, out client, out request);

            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        private static void GetInit(string url, Dictionary<string, object> parameter, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient(new RestClientOptions
            {
                Timeout = timeout
            });
            request = new RestRequest(url, Method.Get);
            //request.AddHeader("Content-Type", "application/json");
            request.Timeout = timeout;

            GenerateHeader(request, Headers);

            if (parameter != null)
            {
                foreach (KeyValuePair<string, object> item in parameter)
                {
                    Parameter para = Parameter.CreateParameter(item.Key, item.Value, ParameterType.GetOrPost);
                    request.AddParameter(para);
                }
            }
        }
        #endregion

        #region Post
        public static async Task<TResult> PostAsync<TResult>(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            var result = await PostAsync(url, obj, Headers, timeout, statusErrorHandle, readBodyWhenErr);
            return JsonConvert.DeserializeObject<TResult>(result ?? string.Empty);
        }
        public static async Task<string> PostAsync(string url, object obj = null, Dictionary<string, string> Headers = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            RestClient client;
            RestRequest request;
            PostInit(url, obj, Headers, timeout, out client, out request);

            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        public static async Task<TResult> PostFormDataAsync<TResult>(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            var result = await PostFormDataAsync(url, Headers, parameters, timeout, statusErrorHandle, readBodyWhenErr);
            return JsonConvert.DeserializeObject<TResult>(result ?? string.Empty);
        }

        public static async Task<string> PostFormDataAsync(string url, Dictionary<string, string> Headers = null, Dictionary<string, object> parameters = null, int timeout = 10000, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            RestClient client;
            RestRequest request;
            PostInit(url, null, Headers, timeout, out client, out request);

            foreach (var item in parameters)
            {
                request.AddParameter(item.Key, item.Value, ParameterType.GetOrPost);//, ParameterType.RequestBody);
                //request.AddParameter(item.Key, item.Value);
            }

            return await GetResult(client, request, statusErrorHandle, readBodyWhenErr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="statusErrorHandle"></param>
        /// <param name="readBodyWhenErr">http code 非200时是否将body中的内容作为正常数据返回, 如果为true则需要业务中自行处理异常逻辑</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        private static async Task<string> GetResult(RestClient client, RestRequest request, bool statusErrorHandle = true, bool readBodyWhenErr = false)
        {
            RestResponse response = await client.ExecuteAsync(request);

            if (!statusErrorHandle || response.IsSuccessful)
            {
                return response.Content;
            }

            if (readBodyWhenErr)
            {
                return response.Content;
            }

            if (response.ErrorException == null)
            {
                if (!string.IsNullOrWhiteSpace(response.ErrorMessage))
                {
                    throw new System.Exception(response.ErrorMessage);
                }

                throw new System.Exception($"Get response error. {request.Resource}");
            }


            if (response.ErrorException is HttpRequestException requestException)
            {
                throw new System.Exception($"{(int)(requestException?.StatusCode ?? 0)}:{requestException?.Message ?? string.Empty}");
            }
            throw response.ErrorException;
        }

        private static void PostInit(string url, object obj, Dictionary<string, string> Headers, int timeout, out RestClient client, out RestRequest request)
        {
            client = new RestClient(new RestClientOptions
            {
                Timeout = timeout
            });
            var setting = new JsonSerializerSettings()
            {
                ContractResolver = null,//new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            client.UseNewtonsoftJson(setting);


            request = new RestRequest(url, Method.Post);
            //request.AddHeader("Content-Type", "application/json");
            request.Timeout = timeout;

            GenerateHeader(request, Headers);

            if (obj != null)
            {
                request.AddBody(obj);
                //request.AddJsonBody(obj);
            }
        }

        /// <summary>
        /// 将header添加到请求中
        /// </summary>
        private static void GenerateHeader(RestRequest request, Dictionary<string, string> Headers)
        {
            if (Headers != null && Headers.Count > 0)
            {
                request.AddOrUpdateHeaders(Headers);
            }
        }
        #endregion
    }
}

