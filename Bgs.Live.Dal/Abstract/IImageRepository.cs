using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Dal.Abstract
{
    public interface IImageRepository
    {
        public Task AddImage(string name, string url);
        public Task DeleteImage(UrlListObject obj);
        public Task SetPrimary(UrlListObject obj);
        public Task<IEnumerable<UrlListObject>> GetAll();
        public Task<UrlListObject> GetObjById(int id);
    }
}

