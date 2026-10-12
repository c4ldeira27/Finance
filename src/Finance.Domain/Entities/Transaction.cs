using Finance.Domain.Enums;
using Finance.Domain.Exceptions;

namespace Finance.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        public const int DescriptionMaxLength = 200;

        public string UserId { get; private set; } = null!;
        public Guid CategoryId { get; private set; }
        public TransactionType TransactionType { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public Transaction(string description, TransactionType transactionType, decimal amount, Guid categoryId, string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new DomainException("User is required.");

            SetDetails(description, transactionType, amount, categoryId);

            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        private Transaction()
        {

        }

        public void Update(string description, TransactionType transactionType, decimal amount, Guid categoryId)
        {
            SetDetails(description, transactionType, amount, categoryId);
            UpdatedAt = DateTime.UtcNow;
        }

        private void SetDetails(string description, TransactionType transactionType, decimal amount, Guid categoryId)
        {
            var trimmedDescription = ValidateDescription(description);

            if (!Enum.IsDefined(transactionType))
                throw new DomainException("Transaction type is invalid.");

            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");

            if (categoryId == Guid.Empty)
                throw new DomainException("Category is required.");

            Description = trimmedDescription;
            TransactionType = transactionType;
            Amount = amount;
            CategoryId = categoryId;
        }

        private static string ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Description is required.");

            var trimmedDescription = description.Trim();

            if (trimmedDescription.Length > DescriptionMaxLength)
                throw new DomainException($"Description must be at most {DescriptionMaxLength} characters.");

            return trimmedDescription;
        }
    }
}
