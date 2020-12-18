using Bgs.Live.Bll.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace BgsLiveBackend.Multimedia.Api.Controllers
{
    public class ImageController : ControllerBase
    {
        private readonly string _fileSystemPath;
        private readonly IMultimediaService _multimediaService;

        public ImageController(IConfiguration configuration, IMultimediaService multimediaService)
        {
            _fileSystemPath = configuration["FileSystemPath"];
            _multimediaService = multimediaService;
        }

        [HttpPost("add")]
        public IActionResult Add(IFormFile file)
        {
            var attachmentUrl = _multimediaService.AddImage(file);

            return Ok(attachmentUrl);
        }

        [HttpGet("get")]
        public IActionResult GetAttachment(string fileName)
        {
            var filePath = Path.Combine($"{_fileSystemPath}/UploadedImages", fileName);

            return PhysicalFile(filePath, MediaTypeNames.Image.Jpeg);
        }
    }
}
