using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Bgs.Utility.Extensions
{
    public static class IFormFileExtensions
    {
        public static MultipartFormDataContent ToHttpContent(this IFormFile file)
        {
            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
            {
                data = br.ReadBytes((int)file.OpenReadStream().Length);
            }

            var bytes = new ByteArrayContent(data);

            return new MultipartFormDataContent
            {
                { bytes, "file", file.FileName }
            };
        }
    }
}
