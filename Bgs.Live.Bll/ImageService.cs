using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Entities;
using Bgs.Live.Common.ErrorCodes;
using Bgs.Live.Core.Exceptions;
using Bgs.Live.Dal.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _ImageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _ImageRepository = imageRepository;
        }
        public async Task AddImage(string name, string url)
        {
            await _ImageRepository.AddImage(name, url);
        }

        public async Task DeleteImage(int id)
        {
            var obj = await GetObjById(id);
            await _ImageRepository.DeleteImage(obj);

        }

        public async Task<IEnumerable<UrlListObject>> GetAll()
        {
            return await _ImageRepository.GetAll();
        }

        public async Task<UrlListObject> GetObjById(int id)
        {
            var obj = await _ImageRepository.GetObjById(id);

            if (obj == null)
            {
                throw new BgsException((int)WebApiErrorCodes.ImageNotFound);
            }

            return obj;
        }

        public async Task SetPrimary(int id)
        {
            var obj = await GetObjById(id);
            await _ImageRepository.SetPrimary(obj);
        }
    }
}
