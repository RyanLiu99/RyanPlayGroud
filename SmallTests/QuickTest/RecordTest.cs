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

        var r11With = r11 with { Name = r11Old.Name };
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

    [Test]
    public void EquateTwoTuplesWithSameContent()
    {
        var t1 = Tuple.Create("S");
        var t2 = Tuple.Create(t1.Item1);
        Assert.IsTrue(t1.Equals(t2));
        Assert.IsFalse(t1 == t2);  //!!!
        Assert.IsFalse(Object.ReferenceEquals(t1, t2));
    }

    [Test]
    public void EquateTwoTuplesWithSameContentDifferentType()
    {
        var t1 = Tuple.Create("S");
        var t2 = Tuple.Create((object)t1.Item1);
        Assert.IsFalse(t1.Equals(t2));
        // Assert.IsFalse(t1 == t2); // Won't even compile, since they are different type
        Assert.IsFalse(Object.ReferenceEquals(t1, t2));

    }

    [Test]
    public void EquateTwoValueTuplesWithSameContent()
    {
        var t1 = ValueTuple.Create("S");
        var t2 = ValueTuple.Create(t1.Item1);
        Assert.IsTrue(t1.Equals(t2));
        // Assert.IsFalse(t1 == t2);  //!!! won't even compile even same type, worked with Tuple
        Assert.IsFalse(Object.ReferenceEquals(t1, t2));
    }

    [Test]
    public void EquateTwoValueTuplesWithSameContentDifferentType()
    {
        var t1 = ValueTuple.Create("S");
        var t2 = ValueTuple.Create((object)t1.Item1);
        Assert.IsFalse(t1.Equals(t2));
        // Assert.IsFalse(t1 == t2); // Won't even compare, since different type
        Assert.IsFalse(Object.ReferenceEquals(t1, t2));

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