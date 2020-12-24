using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.ErrorCodes
{
    public enum WebApiErrorCodes
    {
        UsernameOrPasswordIncorrect = 1,
        UsernameAlreadyExists,
        OldPasswordIsIncorrect,
        PersonalNumberIsAlreadyUsed,
        NotEnoughBalance,
        CouldNotUploadAvatar
    }
}
