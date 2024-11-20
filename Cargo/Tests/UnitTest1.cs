using Xunit;

public class Tests
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        Assert.NotEqual(1, 2);
    }
}
