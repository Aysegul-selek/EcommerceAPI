using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Application.Interfaces.Services;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class S3Service
    {
        private readonly IAwsSettingsService _awsSettingsService;

        public S3Service(IAwsSettingsService awsSettingsService)
        {
            _awsSettingsService = awsSettingsService;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            // DB'den ayarları al
            var settingsResponse = await _awsSettingsService.GetAsync();
            if (settingsResponse == null)
                throw new Exception("AWS ayarları bulunamadı.");

            var aws = settingsResponse; // Dto dönüyor

            using var s3Client = new AmazonS3Client(
                aws.Data.AccessKey,
                aws.Data.SecretKey,
                RegionEndpoint.GetBySystemName(aws.Data.Region)
            );

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = fileName,
                BucketName = aws.Data.BucketName,
                ContentType = contentType
                // CannedACL = S3CannedACL.PublicRead // eğer public URL istiyorsan açabilirsin
            };

            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);

            return $"https://{aws.Data.BucketName}.s3.{s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";
        }
    }
}
