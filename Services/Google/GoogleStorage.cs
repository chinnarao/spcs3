using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Services.Google
{
    public class GoogleStorage : IGoogleStorage
    {
        public void UploadObject(string bucketName, Stream stream, string objectName, string contentType)
        {
            #region validation
            if (string.IsNullOrEmpty(bucketName))       throw new ArgumentException(nameof(bucketName));
            if (stream == null || stream.Length <= 0)   throw new ArgumentException(nameof(stream));
            if (string.IsNullOrEmpty(objectName))       throw new ArgumentException(nameof(objectName));
            if (string.IsNullOrEmpty(contentType))      throw new ArgumentException(nameof(contentType));
            #endregion

            // If you don't specify credentials when constructing the client, the
            // client library will look for credentials in the environment.
            GoogleCredential googleCredential = GoogleCredential.GetApplicationDefault();
            if (googleCredential == null) throw new ArgumentNullException(nameof(GoogleCredential));

            var storage = StorageClient.Create(googleCredential);
            storage.UploadObject(bucketName, objectName, contentType, stream);
        }

        public async Task UploadObjectAsync(string bucketName, Stream stream, string objectName, string contentType)
        {
            #region MyRegion
            if (string.IsNullOrEmpty(bucketName))       throw new ArgumentException(nameof(bucketName));
            if (stream == null || stream.Length <= 0)   throw new ArgumentException(nameof(stream));
            if (string.IsNullOrEmpty(objectName))       throw new ArgumentException(nameof(objectName));
            if (string.IsNullOrEmpty(contentType))      throw new ArgumentException(nameof(contentType));
            #endregion

            var storage = StorageClient.Create();
            await storage.UploadObjectAsync(bucketName, objectName, contentType, stream);
        }
    }

    public interface IGoogleStorage
    {
        void UploadObject(string bucketName, Stream stream, string objectName, string contentType);
        Task UploadObjectAsync(string bucketName, Stream stream, string objectName, string contentType);
    }
}
