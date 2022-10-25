using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public static void GCDTest()
    {
        That(GCD(10, 1340), Is.EqualTo(10));
        That(GCD(12, 48), Is.EqualTo(12));
        That(GCD(35, 150), Is.EqualTo(5));
        That(GCD(10, 80), Is.EqualTo(10));
        Catch<AbsenceOfInputFile>(() => { CountGCD("file.txt"); });
    }
    
    [Test]
    public static void ArgsParsingTest()
    {
        var data = new string[] { "1000", "../../../data/y.txt"};
        var errorData = new string[] { "a" };

        That(ArgsParsing(data), Is.EqualTo((int.Parse(data[0]), data[1])));
        That(ArgsParsing(errorData), Is.EqualTo((defaultCount, defaultFile)));
    }

    [Test]
    public static void CreateFileTest()
    {
        CreateFile(defaultFile, 567);
        That(File.ReadAllLines(defaultFile).Length, Is.EqualTo(567));
        Catch<InabilityOfCreateFile>(() => { CreateFile("y/y.txt", 100); });
    }
}