using F_ckCreditSlavery.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace F_ckCreditSlavery.Entities.Configurations;

public class CreditAccountConfiguration : IEntityTypeConfiguration<CreditAccount>
{
    public void Configure(EntityTypeBuilder<CreditAccount> builder)
    {
        builder.HasData
        (
            new CreditAccount
            {
                Id = 1,
                Name = "Tinkoff",
                InitialBalance = 1000000,
                CurrentDebtBalance = -50000,
                MonthlyPaymentValue = 1000,
                MonthlyPaymentDate = default,
                StartDate = default,
                EndDate = default
            },
            new CreditAccount
            {
                Id = 2,
                Name = "Sber",
                InitialBalance = 500000,
                CurrentDebtBalance = -5000,
                MonthlyPaymentValue = 1000,
                MonthlyPaymentDate = default,
                StartDate = default,
                EndDate = default
            },
            new CreditAccount
            {
                Id = 3,
                Name = "VTB",
                InitialBalance = 1000,
                CurrentDebtBalance = -500,
                MonthlyPaymentValue = 100,
                MonthlyPaymentDate = default,
                StartDate = default,
                EndDate = default
            }
        );
    }
}