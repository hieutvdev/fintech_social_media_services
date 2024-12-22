using Article.Domain.Models;
using Article.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Article.Infrastructure.Data.EFConfigurations;

public class ProcessingStepConfiguration : IEntityTypeConfiguration<ProcessingStep>
{
    public void Configure(EntityTypeBuilder<ProcessingStep> builder)
    {
        builder.HasKey(pc => pc.Id);
        
        builder.Property(r => r.Id).HasConversion(
            processingStepId => processingStepId.Value, dbId => ProcessingStepId.Of(dbId));
    }
}