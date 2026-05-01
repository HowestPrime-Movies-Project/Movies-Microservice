using Howestprime.Movies.Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Domain;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();
        
        builder.Property(r => r.Name).IsRequired();
        builder.Property(r => r.Capacity).IsRequired();
        
        // Index for room name uniqueness
        builder.HasIndex(r => r.Name).IsUnique();
    }
}

