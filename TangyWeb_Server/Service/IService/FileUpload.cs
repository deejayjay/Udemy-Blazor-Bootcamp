using Microsoft.AspNetCore.Components.Forms;

namespace TangyWeb_Server.Service.IService
{
    public class FileUpload : IFileUpload
    {
        #region Constructor
        
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        } 

        #endregion

        public Task<bool> DeleteFile(string filePath)
        {
            var fullPath = _webHostEnvironment.WebRootPath + filePath;
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            FileInfo fileInfo = new(file.Name);
            var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
            var folderDirectory = $"{_webHostEnvironment.WebRootPath}\\images\\product";
            if (!Directory.Exists(folderDirectory))
            {
                Directory.CreateDirectory(folderDirectory);
            }
            var filePath = Path.Combine(folderDirectory, fileName);

            await using FileStream fs = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);

            var fullPath = $"/images/product/{fileName}";
            return fullPath;
        }
    }
}
