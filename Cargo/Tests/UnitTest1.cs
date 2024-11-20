using api;
using NUnit.Framework;

public class Tests
{
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Test]
    public void Test2()
    {
        Assert.AreNotEqual(1, 2);
    }
}