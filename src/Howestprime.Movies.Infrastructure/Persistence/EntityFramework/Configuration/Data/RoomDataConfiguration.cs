using Howestprime.Movies.Application.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Data;

public class RoomDataConfiguration : IEntityTypeConfiguration<RoomData>
{
    public void Configure(EntityTypeBuilder<RoomData> builder)
    {
        builder.ToTable("Rooms");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id);
        builder.Property(r => r.Name);
        builder.Property(r => r.Capacity);
    }
}