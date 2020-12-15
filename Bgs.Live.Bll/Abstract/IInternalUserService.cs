using Bgs.Live.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Bll.Abstract
{
    public interface IInternalUserService
    {
        public InternalUser AuthenticateUser(string email, string password);
    }
}
