using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bgs.Live.Bll.Abstract
{
    public interface IInternalUserService
    {
        public Task<InternalUser> AuthenticateUser(string email, string password);
    }
}
