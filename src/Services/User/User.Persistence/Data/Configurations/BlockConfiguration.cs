using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Models;
using User.Domain.ValuesObjects;

namespace User.Persistence.Data.Configurations;

public class BlockConfiguration : IEntityTypeConfiguration<Block>
{
    public void Configure(EntityTypeBuilder<Block> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(blockId => blockId.Value, dbId => BlockId.Of(dbId));

        builder.HasIndex(r => r.BlockedId);

        builder.HasIndex(r => r.BlockerId);
        
        builder.Property(r => r.BlockedId).IsRequired();
        
        builder.Property(r => r.BlockerId).IsRequired();
    }
}