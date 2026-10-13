using Finance.Domain.Entities;
using Finance.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Infrastructure.Persistence.Configuration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(Category.NameMaxLength).IsRequired();
            builder.Property(x => x.UserId).IsRequired();


            builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(x => x.UserId);

            builder.HasIndex(x => new
            {
                x.UserId,
                x.Name,
            }).IsUnique();
        }
    }
}
