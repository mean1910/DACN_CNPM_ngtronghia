using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace elearning_b1.Services
{
    public class GoogleDriveService
    {
        private readonly DriveService _driveService;

        public GoogleDriveService(DriveService driveService)
        {
            _driveService = driveService;
        }
        public GoogleDriveService(IConfiguration configuration)
        {
            var credential = GoogleCredential.FromFile("D:/service_account.json") // Đường dẫn tới file JSON
                .CreateScoped(DriveService.ScopeConstants.Drive); 

            _driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "b1elearning"
            });
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = file.FileName,
                Parents = new List<string> { "1kYQuSq0VAE54fLR1T6VYRWR3L7fZ6cS2" } // ID thư mục Google Drive
            };

            FilesResource.CreateMediaUpload request;

            using (var stream = file.OpenReadStream())
            {
                request = _driveService.Files.Create(fileMetadata, stream, file.ContentType);
                request.Fields = "id"; // Chỉ lấy ID sau khi upload
                request.ChunkSize = 256 * 1024; // Kích thước khối 256 KB

                try
                {
                    await request.UploadAsync();
                }
                catch (Exception ex)
                {
                    // In thêm thông tin lỗi để dễ dàng debug
                    Console.WriteLine("Error uploading file: " + ex.Message);
                    throw new Exception("Error uploading file: " + ex.Message, ex);
                }

            }

            var fileId = request.ResponseBody?.Id;

            if (!string.IsNullOrEmpty(fileId))
            {
                // Gọi phương thức MakeFilePublicAsync để public file
                await MakeFilePublicAsync(fileId);
                return fileId; // Trả về ID file đã public
            }

            return null;
        }

        public async Task DeleteFileAsync(string fileId)
        {
            try
            {
                await _driveService.Files.Delete(fileId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting file: " + ex.Message);
            }
        }
        public async Task<string> MakeFilePublicAsync(string fileId)
        {
            var permission = new Google.Apis.Drive.v3.Data.Permission
            {
                Role = "reader",
                Type = "anyone"
            };

            await _driveService.Permissions.Create(permission, fileId).ExecuteAsync();

            return $"https://drive.google.com/uc?id={fileId}";
        }
    }
}
