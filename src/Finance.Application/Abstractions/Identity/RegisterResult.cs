using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Application.Abstractions.Identity
{
    public record class RegisterResult
    {
        public bool Succeeded { get; }
        public string? UserId { get; }
        public IReadOnlyCollection<AuthError> Errors { get; }

        private RegisterResult(bool succeeded, string? userId, IReadOnlyCollection<AuthError> errors)
        {
            Succeeded = succeeded;
            UserId = userId;
            Errors = errors;
        }

        public static RegisterResult Success(string userId) => new(true, userId, []);

        public static RegisterResult Failure(IReadOnlyCollection<AuthError> errors) => new(false, null, errors);
    }
}
