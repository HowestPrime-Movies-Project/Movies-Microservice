using Howestprime.Movies.Domain.Shared;

namespace UnitTests.Domain.Shared;

public sealed class AssertsTests
{
    [Fact]
    public void EnsureNotEmpty_WithEmptyString_ShouldThrow()
    {
        // Arrange
        const string value = "";

        // Act
        Action act = () => Asserts.EnsureNotEmpty(value);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureNotEmpty_WithWhitespaceString_ShouldThrow()
    {
        // Arrange
        const string value = "   ";

        // Act
        Action act = () => Asserts.EnsureNotEmpty(value);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureNotEmpty_WithNullObject_ShouldThrow()
    {
        // Arrange
        object? value = null;

        // Act
        Action act = () => Asserts.EnsureNotEmpty(value!);

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    [Fact]
    public void EnsureNotEmpty_WithValidString_ShouldNotThrow()
    {
        // Arrange
        const string value = "ok";

        // Act
        Action act = () => Asserts.EnsureNotEmpty(value);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureNotEmpty_WithValidObject_ShouldNotThrow()
    {
        // Arrange
        object value = new();

        // Act
        Action act = () => Asserts.EnsureNotEmpty(value);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureGreaterThan_WithGreaterValue_ShouldNotThrow()
    {
        // Arrange
        const int value = 11;
        const int threshold = 10;

        // Act
        Action act = () => Asserts.EnsureGreaterThan(value, threshold);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureGreaterThan_WithEqualValue_ShouldThrow()
    {
        // Arrange
        const int value = 10;
        const int threshold = 10;

        // Act
        Action act = () => Asserts.EnsureGreaterThan(value, threshold);

        // Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(act);
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void EnsureLessThan_WithLowerValue_ShouldNotThrow()
    {
        // Arrange
        const int value = 9;
        const int threshold = 10;

        // Act
        Action act = () => Asserts.EnsureLessThan(value, threshold);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureLessThan_WithGreaterValue_ShouldThrow()
    {
        // Arrange
        const int value = 11;
        const int threshold = 10;

        // Act
        Action act = () => Asserts.EnsureLessThan(value, threshold);

        // Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(act);
        Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void EnsureNotNegative_WithPositiveValue_ShouldNotThrow()
    {
        // Arrange
        const int value = 1;

        // Act
        Action act = () => Asserts.EnsureNotNegative(value);

        // Assert
        Assert.Null(Record.Exception(act));
    }

    [Fact]
    public void EnsureNotNegative_WithNegativeValue_ShouldThrow()
    {
        // Arrange
        const int value = -1;

        // Act
        Action act = () => Asserts.EnsureNotNegative(value);

        // Assert
        ArgumentException exception = Assert.Throws<ArgumentException>(act);
        Assert.Equal("value", exception.ParamName);
    }
}
