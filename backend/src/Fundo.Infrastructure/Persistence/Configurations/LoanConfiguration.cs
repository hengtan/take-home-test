using Fundo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fundo.Infrastructure.Persistence.Configurations;

public class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        entity.Property(e => e.CurrentBalance)
            .IsRequired()
            .HasPrecision(18, 2);

        entity.Property(e => e.ApplicantName)
              .HasMaxLength(150)
              .IsRequired();

        entity.Property(e => e.Status)
              .IsRequired();

        entity.HasData(GetSeedLoans());
    }

    private static IEnumerable<object> GetSeedLoans() => new List<object>
    {
        new {
            Id = Guid.Parse("1a58dcd6-4562-4e1e-9aa9-02129a2f1c01"),
            Amount = 1500m,
            CurrentBalance = 500m,
            ApplicantName = "Maria Silva",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("2b3c934a-78d2-4b56-a5d4-55a83954ae02"),
            Amount = 1000m,
            CurrentBalance = 0m,
            ApplicantName = "João Souza",
            Status = LoanStatus.Paid
        },
        new {
            Id = Guid.Parse("3c1a2d9f-d8f1-4f45-8b90-49a2780b1f03"),
            Amount = 5000m,
            CurrentBalance = 5000m,
            ApplicantName = "Alice Johnson",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("4db4e8ae-d0ff-49fd-ae13-bb3e9f4b3a04"),
            Amount = 7500m,
            CurrentBalance = 2500m,
            ApplicantName = "Michael Smith",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("5c334e60-81a2-42a0-89e2-0f80c4b1e405"),
            Amount = 2000m,
            CurrentBalance = 0m,
            ApplicantName = "Laura Martinez",
            Status = LoanStatus.Paid
        },
        new {
            Id = Guid.Parse("6a223421-38a9-43ff-b7b2-dab9b5e8e206"),
            Amount = 12000m,
            CurrentBalance = 12000m,
            ApplicantName = "Daniel Kim",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("7e21f316-9475-4a3e-9c6d-61c5f1a3b207"),
            Amount = 3000m,
            CurrentBalance = 1500m,
            ApplicantName = "Emma Brown",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("8f42e9a1-3cdd-470b-8779-319ae346f508"),
            Amount = 900m,
            CurrentBalance = 0m,
            ApplicantName = "Lucas Williams",
            Status = LoanStatus.Paid
        },
        new {
            Id = Guid.Parse("9c01fd52-780c-4e84-9ef1-5a3e3c0a5909"),
            Amount = 6500m,
            CurrentBalance = 3000m,
            ApplicantName = "Olivia Davis",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("10ffbc7e-212f-4fc7-a3ee-9437e66e7e10"),
            Amount = 4000m,
            CurrentBalance = 0m,
            ApplicantName = "Ethan Wilson",
            Status = LoanStatus.Paid
        },
        new {
            Id = Guid.Parse("11aaf61e-4de7-4607-b9fd-dbc81d15cb11"),
            Amount = 10000m,
            CurrentBalance = 10000m,
            ApplicantName = "Sophia Moore",
            Status = LoanStatus.Active
        },
        new {
            Id = Guid.Parse("12bbc84e-5ad8-4c6a-b637-e2cc087c5d12"),
            Amount = 2500m,
            CurrentBalance = 500m,
            ApplicantName = "Gabriel Taylor",
            Status = LoanStatus.Active
        }
    };
}