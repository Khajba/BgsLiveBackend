﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Bgs.Live.Common.ErrorCodes
{
    public enum WebApiErrorCodes
    {
        EmailOrPasswordIncorrect = 1,
        EmailAlreadyExists,
        OldPasswordIsIncorrect,
        CouldNotUploadAvatar,
        UsernameAlreadyExists
    }
}
