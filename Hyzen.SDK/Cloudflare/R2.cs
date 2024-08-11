using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Hyzen.SDK.SecretManager;

namespace Hyzen.SDK.Cloudflare;

public class R2
{
    private static readonly string AccessKey = HyzenSecret.GetSecret("CLOUDFLARE-ACCESS-KEY");
    private static readonly string SecretKey = HyzenSecret.GetSecret("CLOUDFLARE-SECRET-KEY");
    private static readonly string AccountId = HyzenSecret.GetSecret("CLOUDFLARE-ACCOUNT-ID");
    private static readonly string ServiceUrl = $"https://{AccountId}.r2.cloudflarestorage.com";
    private readonly AmazonS3Client _client;
    private static readonly Dictionary<string, R2> Instance = new();
    private string BucketName { get; set; }

    private R2(string bucketName)
    {
        BucketName = bucketName;
        AmazonS3Config config = new AmazonS3Config { ServiceURL = ServiceUrl };
        BasicAWSCredentials credentials = new BasicAWSCredentials(AccessKey, SecretKey);
        _client = new AmazonS3Client(credentials, config);
        AWSConfigsS3.UseSignatureVersion4 = true;
    }
    
    public static R2 Get(string bucketName)
    {
        if (Instance.TryGetValue(bucketName, out var client))
            return client;

        client = new R2(bucketName);
        Instance.Add(bucketName, client);
        
        return client;
    }
    
    public async Task<List<string>> ListBuckets()
    {
        ListBucketsResponse response = await _client.ListBucketsAsync();
        List<string> listBuckets = new List<string>();
        
        foreach (var bucket in response.Buckets)
            listBuckets.Add(bucket.BucketName);
        
        return listBuckets;
    }

    public async Task<List<string>> ListObjects()
    {
        ListObjectsV2Request request = new() { BucketName = BucketName };
        ListObjectsV2Response response = await _client.ListObjectsV2Async(request);
        List<string> listObjects = new List<string>();
        
        foreach (var item in response.S3Objects)
            listObjects.Add(item.Key);
        
        return listObjects;
    }

    public async Task<PutObjectResponse> PutObject(string objectName, byte[] bytes)
    {
        using MemoryStream inputStream = new MemoryStream(bytes);
        
        var request = new PutObjectRequest
        {
            Key = objectName,
            InputStream = inputStream,
            BucketName = BucketName,
            DisablePayloadSigning = true
        };

        return await _client.PutObjectAsync(request);
    }
    
    public async Task<GetObjectResponse> GetObject(string objectName)
    { 
        return await _client.GetObjectAsync(BucketName, objectName);
    }

    public async Task<DeleteObjectResponse> DeleteObject(string objectName)
    {
        return await _client.DeleteObjectAsync(BucketName, objectName);
    }

    public Task<string> GeneratePresignedUrl(string objectName, DateTime expirationDate = default)
    {
        if (expirationDate == default)
            expirationDate = DateTime.Now.AddDays(7);
        
        var presign = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = objectName,
            Verb = HttpVerb.GET,
            Expires = expirationDate,
        };
        
        return Task.FromResult(_client.GetPreSignedURL(presign));
    }
}