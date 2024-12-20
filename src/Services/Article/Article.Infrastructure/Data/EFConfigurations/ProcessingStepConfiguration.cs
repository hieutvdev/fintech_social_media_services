using Article.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ProcessingStepConfiguration : IEntityTypeConfiguration<ProcessingStep>
{
    public void Configure(EntityTypeBuilder<ProcessingStep> builder)
    {
        throw new NotImplementedException();
    }
}