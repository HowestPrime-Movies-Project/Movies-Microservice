using Howestprime.Movies.Domain.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configuration.Domain;

public class MovieEventConfiguration : IEntityTypeConfiguration<MovieEvent>
{
    public void Configure(EntityTypeBuilder<MovieEvent> builder)
    {
        builder.ToTable("MovieEvents");
        
        builder.HasKey(me => me.Id);
        builder.Property(me => me.Id).ValueGeneratedNever();
        
        builder.Property(me => me.MovieId).IsRequired();
        builder.Property(me => me.RoomId).IsRequired();
        builder.Property(me => me.ShowTime).IsRequired();
        builder.Property(me => me.Capacity).IsRequired();
        builder.Property(me => me.Visitors).IsRequired();
        
        builder.OwnsMany(me => me.Bookings, bookingBuilder =>
        {
            bookingBuilder.ToJson();
        });
        
        // Index for efficient lookup of showtime and room combinations
        builder.HasIndex(me => new { me.RoomId, me.ShowTime });
    }
}


