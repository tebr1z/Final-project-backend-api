using LmsApiApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LmsApiApp.Application.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly string _uploadPath;

        public FileUploadService()
        {
            _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file, string oldFilePath = null)
        {
            if (file == null || file.Length == 0) return null;

            // Eski dosya varsa sil
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                await DeleteFileAsync(oldFilePath); // Asenkron silme
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName; // Yüklenen dosyanın adı döner
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            var fullPath = Path.Combine(_uploadPath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath); // Dosyayı sil
            }
            else
            {
                // Dosya yoksa bir log bırakabilirsiniz
                Console.WriteLine("File does not exist: " + fullPath);
            }

            await Task.CompletedTask;
        }


        public async Task<(string pdfUrl, string videoUrl, string imageUrl)> UploadFilesAsync(IFormFile pdfFile, IFormFile videoFile, IFormFile imageFile)
        {
            string pdfUrl = await UploadFileAsync(pdfFile);
            string videoUrl = await UploadFileAsync(videoFile);
            string imageUrl = await UploadFileAsync(imageFile);

            return (pdfUrl, videoUrl, imageUrl);
        }
    }
}
