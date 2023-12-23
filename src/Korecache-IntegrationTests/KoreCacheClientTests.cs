using KoreCache;
using KoreCache.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;

namespace Korecache_IntegrationTests
{
    [TestClass]
    public class KoreCacheClientTests
    {
        KoreCacheClient _client;

        [TestInitialize()]
        public void Startup()
        {
            var apiKey = TestSetup.GetApiKey();
            var config = new Configuration(apiKey);
            _client = new KoreCacheClient(config, new HttpClient());
        }

        [TestMethod]
        public void GetOverviewStats()
        {
            var overviewStatsResult = _client.GetOverviewStats().Result;
            Assert.IsNotNull(overviewStatsResult);
            Assert.IsTrue(overviewStatsResult.Success);
            Assert.IsTrue(int.Parse(overviewStatsResult.Result.First().Max) > 0);
        }

        [TestMethod]
        public void GetOwner()
        {
            var ownerResult = _client.GetOwner().Result;
            Assert.IsNotNull(ownerResult);
            Assert.IsTrue(ownerResult.Success);
            Assert.IsTrue(ownerResult.Result.OwnerGuid !=  Guid.Empty);
        }

        [TestMethod]
        public void CreateBucket()
        {
            var name = $@"CreateBucket_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
            var createdBucketResult = _client.CreateBucket(name).Result;
            var bucketResult = _client.GetBucket(name).Result;
            Assert.IsNotNull(createdBucketResult);
            Assert.IsTrue(createdBucketResult.Success);
            Assert.IsTrue(createdBucketResult.Result.BucketName == name);
            var deletedBucket = _client.DeleteBucket(bucketResult.Result.Id).Result;
            Assert.IsTrue(deletedBucket.Success);
        }

        [TestMethod]
        public void GetBucket()
        {
            var name = $@"GetBucket_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
            var createdBucketResult = _client.CreateBucket(name).Result;
            var bucketResult = _client.GetBucket(name).Result;
            Assert.IsNotNull(bucketResult);
            Assert.IsTrue(bucketResult.Success);
            Assert.IsTrue(bucketResult.Result.BucketName == name);
            var deletedBucket = _client.DeleteBucket(bucketResult.Result.Id).Result;
            Assert.IsTrue(deletedBucket.Success);
        }

        [TestMethod]
        public void GetBuckets()
        {
            var name = $@"GetBuckets_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
            var createdBucketResult = _client.CreateBucket(name).Result;
            var bucketResult = _client.GetBuckets().Result;
            Assert.IsNotNull(bucketResult);
            Assert.IsTrue(bucketResult.Success);
            Assert.IsTrue(bucketResult.Result.Any(x => x.BucketName == name));
            var deletedBucket = _client.DeleteBucket(bucketResult.Result.First(x => x.BucketName == name).Id).Result;
            Assert.IsTrue(deletedBucket.Success);
        }

        [TestMethod]
        public void BasicKeyOperation()
        {
            var name = $@"BasicKeyOperation_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}".ToLower();
            var createdBucketResult = _client.CreateBucket(name).Result;
            var bucketResult = _client.GetBucket(name).Result;
            Assert.IsNotNull(bucketResult);
            Assert.IsTrue(bucketResult.Success);
            Assert.IsTrue(bucketResult.Result.BucketName == name);

            var keyName = "BasicKeyOperation";
            var keyValue = $@"BasicKeyOperation_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}";
            var setKeyResult = _client.SetKey(bucketResult.Result.Id, keyName, keyValue).Result;
            Assert.IsTrue(setKeyResult.Success);
            var getKeyResult = _client.GetKey(bucketResult.Result.Id, keyName).Result;
            Assert.IsTrue(getKeyResult.Success);
            Assert.IsTrue(getKeyResult.Result.Value == keyValue);
            Assert.IsTrue(getKeyResult.Result.BucketId == bucketResult.Result.Id);
            var deletedKeyResult = _client.ExpireKey(bucketResult.Result.Id, keyName).Result;
            Assert.IsTrue(deletedKeyResult.Success);
            var deletedBucket = _client.DeleteBucket(bucketResult.Result.Id).Result;
            Assert.IsTrue(deletedBucket.Success);
        }
    }
}