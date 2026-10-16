using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Abstractions.Identity
{
    public interface ICurrentUser
    {
        public string UserId { get; }
    }
}
