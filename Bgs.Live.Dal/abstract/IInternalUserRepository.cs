using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Dal.Abstract
{
    public interface IInternalUserRepository
    {
        public InternalUser GetUserByCredentials(string email, string password);
    }
}
