using F_ckCreditSlavery.Entities.Enums;
using F_ckCreditSlavery.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F_ckCreditSlavery.Entities.Configurations;

public class CreditAccountChangeConfiguration : IEntityTypeConfiguration<CreditAccountChange>
{
    public void Configure(EntityTypeBuilder<CreditAccountChange> builder)
    {
        builder.HasData
        (
            new CreditAccountChange
            {
                Id = 1,
                CreditAccountId = 1,
                PaymentValue = 1000,
                PaymentType = PaymentType.Incoming,
                PaymentDate = default
            },
            new CreditAccountChange
            {
                Id = 2,
                CreditAccountId = 1,
                PaymentValue = 2000,
                PaymentType = PaymentType.Incoming,
                PaymentDate = default
            },
            new CreditAccountChange
            {
                Id = 3,
                CreditAccountId = 1,
                PaymentValue = 3000,
                PaymentType = PaymentType.Incoming,
                PaymentDate = default
            }
        );
    }
}