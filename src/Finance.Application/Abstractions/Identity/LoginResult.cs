using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Abstractions.Identity
{
    public enum LoginResult
    {
        Success = 1,
        InvalidCredentials = 2,
        LockedOut = 3
    }
}
