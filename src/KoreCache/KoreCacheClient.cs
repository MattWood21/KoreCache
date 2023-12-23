using KoreCache.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace KoreCache
{
    public class KoreCacheClient
    {
        private Configuration _configuration;
        private HttpClient _httpClient;

        private string _koreCacheEndpoint;
        public string KoreCacheEndpoint => _koreCacheEndpoint;

        public KoreCacheClient(Configuration configuration, string koreToolsEnvironment = null)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(koreToolsEnvironment ?? "https://prod.us-west-1.kore-tools.com");
        }

        public KoreCacheClient(Configuration configuration, HttpClient httpClient, string koreToolsEnvironment = null)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(koreToolsEnvironment ?? "https://prod.us-west-1.kore-tools.com");
        }

        #region internal handling
        private string _serialize(object thing)
        {
            return JsonSerializer.Serialize(thing, JsonHelper.CompactJsonSerializerOptions);
        }

        private async Task<ApiResult<T>> _handleRequest<T>(HttpMethod method, string route, string body = null)
        {
            var result = new ApiResult<T>();

            try
            {
                var request = new HttpRequestMessage(method, route);
                if (!string.IsNullOrEmpty(_configuration?.ApiKey))
                {
                    request.Headers.Add("X-KoreTools-ApiKey", $@"ApiKey {_configuration.ApiKey}");
                }

                if (!string.IsNullOrWhiteSpace(body))
                {
                    var content = new StringContent(body, Encoding.UTF8, "application/json");
                    request.Content = content;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var response = _httpClient.SendAsync(request).Result;
                stopWatch.Stop();
                var responseContent = await response?.Content?.ReadAsStringAsync();
                result.Success = response.IsSuccessStatusCode;
                result.StatusCode = response.StatusCode;
                result.ResponseContent = responseContent;
                result.ExecutionTime = stopWatch.ElapsedMilliseconds;

                if (result.Success && !string.IsNullOrWhiteSpace(responseContent))
                {
                    result.Result = JsonSerializer.Deserialize<T>(responseContent, JsonHelper.CompactJsonSerializerOptions);
                }
                else if (!result.Success)
                {
                    if ((int)response.StatusCode == 401)
                    {
                        result.ErrorReason = new ApiErrorReason() { Error = "Unauthorized, API Key is invalid" };
                    }
                    else if ((int)response.StatusCode == 429)
                    {
                        result.ErrorReason = new ApiErrorReason() { Error = "API rate limit exceeded; wait and try this operation again later, or consider upgrading your Kore Tools plan" };
                    }
                    else if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600)
                    {
                        result.ErrorReason = new ApiErrorReason() { Error = "System unavailable, please try again later" };
                    }
                    else
                    {
                        result.ErrorReason = JsonSerializer.Deserialize<ApiErrorReason>(responseContent, JsonHelper.CompactJsonSerializerOptions);
                        if (string.IsNullOrWhiteSpace(result.ErrorReason?.Error))
                        {
                            result.ErrorReason = new ApiErrorReason()
                            {
                                Error = "API encountered an unexpected error without additional details"
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = ex;
            }

            return result;
        }
        #endregion


        #region Stats
        public async Task<ApiResult<IEnumerable<OverviewStat>>> GetOverviewStats()
        {
            return await _handleRequest<IEnumerable<OverviewStat>>(HttpMethod.Get, "/korecache/api/overview");
        }
        #endregion

        #region Owner
        public async Task<ApiResult<CreateOwnerResult>> CreateOwner(string email, string firstName, string lastName, string source)
        {
            var createOwner = new CreateOwner()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Source = source
            };

            return await _handleRequest<CreateOwnerResult>(HttpMethod.Post, "/korecache/api/owner", _serialize(createOwner));
        }

        public async Task<ApiResult<Owner>> GetOwner()
        {
            return await _handleRequest<Owner>(HttpMethod.Get, "/korecache/api/owner");
        }
        #endregion

        #region Buckets
        public async Task<ApiResult<Bucket>> CreateBucket(string bucketName)
        {
            var createBucket = new Bucket()
            {
                BucketName = bucketName
            };

            return await _handleRequest<Bucket>(HttpMethod.Post, "/korecache/api/bucket", _serialize(createBucket));
        }

        public async Task<ApiResult<Bucket>> GetBucket(string bucketname)
        {
            return await _handleRequest<Bucket>(HttpMethod.Get, $@"/korecache/api/bucket/name?bucketName={bucketname}");
        }

        public async Task<ApiResult<List<Bucket>>> GetBuckets()
        {
            return await _handleRequest<List<Bucket>>(HttpMethod.Get, $@"/korecache/api/bucket");
        }

        public async Task<ApiResult<Bucket>> DeleteBucket(long bucketId)
        {
            return await _handleRequest<Bucket>(HttpMethod.Delete, $@"/korecache/api/bucket/{bucketId}");
        }
        #endregion

        #region Keys
        public async Task<ApiResult<KoreCacheKey>> GetKey(long bucketId, string key)
        {
            return await _handleRequest<KoreCacheKey>(HttpMethod.Get, $@"/korecache/api/cache/{bucketId}/{key}");
        }

        public async Task<ApiResult<KoreCacheKey>> SetKey(long bucketId, string key, string value)
        {
            var setKey = new
            {
                bucketId,
                key,
                value
            };

            return await _handleRequest<KoreCacheKey>(HttpMethod.Post, $@"/korecache/api/cache", _serialize(setKey));
        }

        public async Task<ApiResult<KoreCacheKey>> ExpireKey(long bucketId, string key)
        {
            var expireKey = new
            {
                bucketId,
                key
            };

            return await _handleRequest<KoreCacheKey>(HttpMethod.Delete, $@"/korecache/api/cache", _serialize(expireKey));
        }
        #endregion

        #region API Keys
        public async Task<ApiResult<List<ApiKey>>> GetApiKeys()
        {
            return await _handleRequest<List<ApiKey>>(HttpMethod.Get, $@"/korecache/api/apikey/list");
        }

        public async Task<ApiResult<ApiKey>> CreateApiKey(string name)
        {
            return await _handleRequest<ApiKey>(HttpMethod.Post, $@"/korecache/api/apikey?apiKeyName={name}");
        }

        public async Task<ApiResult<ApiKey>> DeleteApiKey(long id)
        {
            return await _handleRequest<ApiKey>(HttpMethod.Delete, $@"/korecache/api/apikey/{id}");
        }

        #endregion
    }
}
