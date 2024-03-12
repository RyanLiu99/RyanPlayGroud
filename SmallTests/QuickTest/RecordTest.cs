using NUnit.Framework.Constraints;

namespace QuickTest;

public class RecordTest
{
    [Test]
    public void TestRC()
    {
        var r11 = new RC1("Liu", 30);
        var r22 = new RC1("Liu", 30);

        Assert.IsTrue(r11 == r22);
        Assert.IsTrue(r11.Equals(r22));

        Assert.IsFalse(object.ReferenceEquals(r11, r22));
    }

    [Test]
    public void TestRS1()
    {
        var r11 = new RS1("Liu", 30);
        RS1 r22 = new RS1("Liu", 30);

        Assert.IsTrue(r11 == r22);
        Assert.IsTrue(r11.Name == "Liu");
        Assert.IsTrue(r11.Equals(r22));

        Assert.IsFalse(object.ReferenceEquals(r11, r22));//make no sense since boxing

        var r11Original = r11; // a new copy
        Assert.IsTrue(r11 == r11Original);  // it is value type

        r11.Name = "Liu2";  // can modify, r11 changed

        Assert.IsTrue(r11.Name == "Liu2");
        Assert.IsTrue(r11 != r11Original); // it is stuct, and value changed

        var r11With = r11 with { Name = r11Original.Name }; // value go back to original
        Assert.That(r11Original, Is.EqualTo(r11With));

        Assert.IsTrue(r11With == r11Original); //just compare value

        Assert.IsTrue(r11With.Equals(r11Original));
        Assert.IsFalse(object.ReferenceEquals(r11With, r11Original)); //Boxing cause them not same

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
    public string Name { get; init; }
    public int Age { get; init; }

}


public struct K<T1, T2>
{
    public T1 Item1;
    public T2 Item2;

    public double d;

    public static bool operator ==(K<T1, T2> t1, K<T1, T2> t2)
    {
        return (t1.Item1?.Equals(t2.Item1)?? false)  // but == and Equals means different thing
        && (t1.Item2?.Equals(t2.Item2) ?? false);
    }

    public static bool operator !=(K<T1, T2> t1, K<T1, T2> t2)
    {
        return !(t1 == t2);
    }
}