using Bgs.Live.Common.Entities;
using Bgs.Live.Dal.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bgs.Live.Dal
{
    public class ImageRepository : IImageRepository
    {
        private static List<UrlListObject> UrlList = new List<UrlListObject>();
        public async Task AddImage(string name, string url)
        {
            var item = new UrlListObject();

            item.Name = name;
            item.Url = url;
            item.Id = GetNextId();
            item.IsPrimary = false;

            UrlList.Add(item);
        }

        public async Task DeleteImage(UrlListObject obj)
        {
            UrlList.Remove(obj);
        }

        public async Task<IEnumerable<UrlListObject>> GetAll()
        {
            return UrlList;
        }

        public async Task<UrlListObject> GetObjById(int id)
        {
            return UrlList.Find(i => i.Id == id);
        }

        public async Task SetPrimary(UrlListObject obj)
        {
            obj.IsPrimary = true;
        }

        private int GetNextId()
        {
            int maxId = 0;
            foreach (var item in UrlList)
            {
                if (item.Id > maxId)
                {
                    maxId = item.Id;
                }
            }
            return maxId + 1;
        }
    }
}
