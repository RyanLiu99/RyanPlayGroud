using NUnit.Framework.Constraints;

namespace QuickTest;

public class Tests
{

    [Test]
    public void TestRC()
    {
        var r11 = new RC1("Liu", 30);
        var r22 = new RC1("Liu", 30);

        Assert.IsTrue(r11 == r22);

    }

    [Test]
    public void TestRS1()
    {
        var r11 = new RS1("Liu", 30);
        RS1 r22 = new RS1("Liu", 30);

        Assert.IsTrue(r11 == r22);
        Assert.IsTrue(r11.Name == "Liu");

        var r11Old = r11;
        Assert.IsTrue(r11 == r11Old);

        r11.Name = "Liu2";  // can modify

        Assert.IsTrue(r11.Name == "Liu2");
        Assert.IsTrue(r11 != r11Old);

        var r11With = r11 with { Name = r11Old.Name};
        Assert.That(r11Old, Is.EqualTo(r11With));
        Assert.IsTrue(r11 != r11Old);
        Assert.IsTrue(r11With == r11Old);

    }

    [Test]
    public void TestRS2()
    {
        var r11 = new RS2() { Name = "Liu", Age = 20 };
        RS2 r22 = new RS2() { Name = "Liu", Age = 20 };

        Assert.IsTrue(r11 == r22);
        Assert.IsTrue(r22.Name == "Liu");
    }
}


public record class RC1
{
    public RC1(string Name, int Age)
    {

    }
}

public record struct RS1(string Name, int Age) //primary constructor
{

}

public record struct RS2
{
    public required string Name { get; init; }
    public required int Age { get; init; }

}