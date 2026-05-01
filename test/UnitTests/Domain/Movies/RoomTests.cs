using Howestprime.Movies.Domain.Movies;
using Howestprime.Movies.Domain.Movies.Events;

namespace UnitTests.Domain.Movies;

public sealed class RoomTests
{
    [Fact]
    public void DefaultConstructor_ShouldLeavePropertiesUninitialized()
    {
        // Arrange & Act
        Room room = new();

        // Assert
        Assert.Equal(default, room.Id);
        Assert.Null(room.Name);
        Assert.Equal(0, room.Capacity);
        Assert.Empty(room.DomainEvents);
    }

    [Fact]
    public void Create_WithValidState_ShouldCreateRoom()
    {
        // Arrange
        string name = "Cinema Room A";
        int capacity = 150;

        // Act
        Room room = Room.Create(name, capacity);

        // Assert
        Assert.NotEqual(default, room.Id);
        Assert.Equal(name, room.Name);
        Assert.Equal(capacity, room.Capacity);
        Assert.Single(room.DomainEvents);

        var domainEvent = Assert.IsType<RoomCreated>(room.DomainEvents.Single());
        Assert.Equal(room.Id, domainEvent.RoomId);
        Assert.Equal(name, domainEvent.Name);
        Assert.Equal(capacity, domainEvent.Capacity);
    }

    [Fact]
    public void Create_WithNullName_ShouldThrow()
    {
        // Act
        Action act = () => Room.Create(null!, 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrow()
    {
        // Act
        Action act = () => Room.Create(string.Empty, 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithWhitespaceName_ShouldThrow()
    {
        // Act
        Action act = () => Room.Create("   ", 100);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithZeroCapacity_ShouldThrow()
    {
        // Act
        Action act = () => Room.Create("Cinema Room A", 0);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithNegativeCapacity_ShouldThrow()
    {
        // Act
        Action act = () => Room.Create("Cinema Room A", -50);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void Create_WithLargeCapacity_ShouldSucceed()
    {
        // Arrange
        string name = "Grand Cinema";
        int largeCapacity = 5000;

        // Act
        Room room = Room.Create(name, largeCapacity);

        // Assert
        Assert.Equal(largeCapacity, room.Capacity);
    }

    [Fact]
    public void CreateWithId_WithValidState_ShouldCreateRoom()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        string name = "Cinema Room B";
        int capacity = 200;

        // Act
        Room room = Room.Create(name, capacity, roomId);

        // Assert
        Assert.Equal(roomId, room.Id);
        Assert.Equal(name, room.Name);
        Assert.Equal(capacity, room.Capacity);
        Assert.Single(room.DomainEvents);

        var domainEvent = Assert.IsType<RoomCreated>(room.DomainEvents.Single());
        Assert.Equal(roomId, domainEvent.RoomId);
        Assert.Equal(name, domainEvent.Name);
        Assert.Equal(capacity, domainEvent.Capacity);
    }

    [Fact]
    public void CreateWithId_WithNullName_ShouldThrow()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());

        // Act
        Action act = () => Room.Create(null!, 100, roomId);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void CreateWithId_WithZeroCapacity_ShouldThrow()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());

        // Act
        Action act = () => Room.Create("Cinema Room", 0, roomId);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void ValidateState_WithValidState_ShouldNotThrow()
    {
        // Arrange
        Room room = Room.Create("Cinema Room", 100);

        // Act
        Action act = () => room.ValidateState();

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void Room_Equality_SameReference_ShouldBeEqual()
    {
        // Arrange
        Room room = Room.Create("Cinema Room", 100);

        // Act
        bool result = room.Equals(room);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Room_Equality_DifferentRoomsSameId_ShouldBeEqual()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        Room room1 = Room.Create("Cinema A", 100, roomId);
        Room room2 = Room.Create("Cinema B", 150, roomId);

        // Act
        bool result = room1.Equals(room2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Room_Equality_DifferentIds_ShouldNotBeEqual()
    {
        // Arrange
        Room room1 = Room.Create("Cinema A", 100);
        Room room2 = Room.Create("Cinema B", 100);

        // Act
        bool result = room1.Equals(room2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Room_Inequality_Operator_ShouldWork()
    {
        // Arrange
        Room room1 = Room.Create("Cinema A", 100);
        Room room2 = Room.Create("Cinema B", 100);

        // Act & Assert
        Assert.True(room1 != room2);
    }

    [Fact]
    public void Room_Equality_Operator_SameReference_ShouldBeTrue()
    {
        // Arrange
        Room room = Room.Create("Cinema A", 100);

        // Act & Assert
        Assert.True(room.Equals(room));
    }

    [Fact]
    public void Room_GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        RoomId roomId = new(Guid.NewGuid());
        Room room1 = Room.Create("Cinema A", 100, roomId);
        Room room2 = Room.Create("Cinema B", 150, roomId);

        // Act & Assert
        Assert.Equal(room1.GetHashCode(), room2.GetHashCode());
    }
}



