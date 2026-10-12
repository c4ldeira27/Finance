

using Finance.Domain.Exceptions;

namespace Finance.Domain.Entities
{
    public class Category : BaseEntity
    {
        public const int NameMaxLength = 100;
        public string Name { get; private set; } = string.Empty;
        public string UserId { get; private set; } = null!;

        public Category(string name, string userId)
        {
            Name = ValidateName(name);

            if(string.IsNullOrWhiteSpace(userId))
                throw new DomainException("User is required.");

            UserId = userId;
        }

        private Category()
        {

        }

        public void Rename(string name)
        {
            Name = ValidateName(name);
        }

        private static string ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name is required.");

            var trimmedName = name.Trim();

            if (trimmedName.Length > NameMaxLength)
                throw new DomainException($"Category name must be at most {NameMaxLength} characters.");

            return trimmedName;
        }

    }
}
