using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bgs.Live.Bll
{
    public class FileSystemMultimediaService : IMultimediaService
    {

        private static Random _random = new Random();

        private readonly string _fileSystemPath;

        public FileSystemMultimediaService(IConfiguration configuration)
        {
            _fileSystemPath = configuration["FileSystemPath"];
        }
        public string AddImage(IFormFile file)
        {
            var fileName = RandomString(10);
            var fileExtension = Path.GetExtension(file.FileName);
            var filePath = Path.Combine($"{_fileSystemPath}/UploadedImages", $"{fileName}{fileExtension}");

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return $"http://localhost:62258/api/image/get?fileName={fileName}{fileExtension}";
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
