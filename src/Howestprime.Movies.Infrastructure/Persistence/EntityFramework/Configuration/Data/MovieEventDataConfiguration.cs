using Howestprime.Movies.Application.Contracts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class MovieEventDataConfiguration : IEntityTypeConfiguration<MovieEventData>
{
    public void Configure(EntityTypeBuilder<MovieEventData> builder)
    {
        builder.ToTable("MovieEvents");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id);
        builder.Property(m => m.ShowTime);
        builder.Property(m => m.Capacity);
        
        builder.HasOne(m => m.Movie)
            .WithMany()
            .HasForeignKey("MovieId");
            
        builder.HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey("RoomId");

    }
}