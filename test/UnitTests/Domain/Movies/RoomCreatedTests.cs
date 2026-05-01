using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class RoomCreatedTests
{
    [Fact]
    public void Constructor_WithValidInput_ShouldInitializeProperties()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        string name = "Cinema Room A";
        int capacity = 150;

        // Act
        RoomCreated @event = new(roomId, name, capacity);

        // Assert
        Assert.Equal(roomId, @event.RoomId);
        Assert.Equal(name, @event.Name);
        Assert.Equal(capacity, @event.Capacity);
    }

    [Fact]
    public void Constructor_ShouldHaveFQDN()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());

        // Act
        RoomCreated @event = new(roomId, "Cinema Room", 100);

        // Assert
        Assert.Equal("Howestprime.Movies.Room.RoomCreated", @event.FQDN);
    }

    [Fact]
    public void Constructor_WithDifferentValues_ShouldStoreCorrectly()
    {
        // Arrange
        var roomIdValue = Guid.NewGuid();
        RoomId roomId = new(roomIdValue);
        string name = "Premium Cinema";
        int capacity = 300;

        // Act
        RoomCreated @event = new(roomId, name, capacity);

        // Assert
        Assert.Equal(roomIdValue, @event.RoomId.Value);
        Assert.Equal("Premium Cinema", @event.Name);
        Assert.Equal(300, @event.Capacity);
    }

    [Fact]
    public void RoomCreated_Properties_ShouldBeStoredCorrectly()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        string name = "Cinema Room A";
        int capacity = 150;

        var @event = new RoomCreated(roomId, name, capacity);

        // Act & Assert - Just verify the properties are stored correctly
        Assert.Equal(roomId, @event.RoomId);
        Assert.Equal(name, @event.Name);
        Assert.Equal(capacity, @event.Capacity);
    }

    [Fact]
    public void RoomCreated_WithMultipleDifferentNames_ShouldMaintainInequality()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());

        var event1 = new RoomCreated(roomId, "Cinema A", 150);
        var event2 = new RoomCreated(roomId, "Cinema B", 150);

        // Act & Assert
        Assert.NotEqual(event1, event2);
    }

    [Fact]
    public void RoomCreated_WithMultipleDifferentCapacities_ShouldMaintainInequality()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        string name = "Cinema Room";

        var event1 = new RoomCreated(roomId, name, 100);
        var event2 = new RoomCreated(roomId, name, 200);

        // Act & Assert
        Assert.NotEqual(event1, event2);
    }
}



