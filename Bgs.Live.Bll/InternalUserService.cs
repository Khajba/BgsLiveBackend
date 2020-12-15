using Bgs.Dal.Abstract;
using Bgs.Live.Bll.Abstract;
using Bgs.Live.Common.Entities;
using Bgs.Live.Common.ErrorCodes;
using Bgs.Live.Core.Exceptions;
using Bgs.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Bll
{
    public class InternalUserService: IInternalUserService
    {
        private readonly IInternalUserRepository _internalUserRepository;

        public InternalUserService(IInternalUserRepository internalUserRepository)
        {
            _internalUserRepository = internalUserRepository;
        }

        public InternalUser AuthenticateUser(string email, string password)
        {
            var user = _internalUserRepository.GetUserByCredentials(email, password.ToSHA256(email));

            if (user == null)
            {
                throw new BgsException((int)AdminApiErrorCodes.EmailOrPasswordIncorrect);
            }
            else
            {
                return user;
            }
        }
    }
}

