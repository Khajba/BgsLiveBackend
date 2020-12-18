using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Dal.Abstract
{
    public interface IInternalUserRepository
    {
        public Task<InternalUser> GetUserByCredentials(string email, string password);
    }
}
