using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string oldFilePath = null);
        Task DeleteFileAsync(string filePath); // Asenkron silme metodu

        Task<(string pdfUrl, string videoUrl, string imageUrl)> UploadFilesAsync(IFormFile pdfFile, IFormFile videoFile, IFormFile imageFile); // Çoklu dosya yükleme metodu
    }
}
