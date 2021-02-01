using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface IImageService
    {
        public Task AddImage(string name, string url);
        public Task DeleteImage(int id);
        public Task SetPrimary(int id);
        public Task<UrlListObject> GetObjById(int id);
        public Task<IEnumerable<UrlListObject>> GetAll();
    }
}
