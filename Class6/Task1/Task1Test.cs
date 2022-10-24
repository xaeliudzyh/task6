using NUnit.Framework;
using System.IO.Enumeration;
using static NUnit.Framework.Assert;
using static Task1.Task1;

namespace Task1;

public class Tests
{

    [Test]
    public void ApplyOperationTest()
    {
        That(ApplyOperation('*', 5, 2), Is.EqualTo(10));
        That(ApplyOperation('/', 10, 5), Is.EqualTo(2));
        Catch<DivideByZeroException>(() => { ApplyOperation('/', 1, 0); });
    }

    [Test]
    public void FormatLhsTest()
    {
        That(FormatLhs("**/", new string[] { "1", "2", "3", "4" }), Is.EqualTo("1*2*3/4"));
    }

    [Test]
    public void ProcessStringTest()
    {
        That(ProcessString("**", "1,2,3"), Is.EqualTo("1*2*3=6"));
        That(ProcessString("**/", "1,2,3,4"), Is.EqualTo("1*2*3/4=1"));
        Catch<NotEnoughNumbers>(() => { ProcessString("***", "12"); });
    }

    [Test]
    public void ReadFileTest()
    {
        That(ReadFile("../../../schemaTest.txt"), Is.EqualTo(new string[] { "765", "5", "226" }));
        Catch<AbsenceOfFile>(() => { ReadFile("a.txt"); });
    }

    [Test]
    public void WriteFileTest()
    {
        var fileName = "../../../schemTest.txt";
        var data = new List<string> { "765", "5", "226" };

        WriteFile(fileName, data);
        That(ReadFile(fileName), Is.EqualTo(data));

        Catch<InabilityOfCreatingTheFile>(() => { WriteFile("E/a.txt", new List<string> { "a" }); });
    }

    [Test]
    public void ProcessFilesTest()
    {
        var schemaFile = "../../../data/schema.txt";
        var dataFile1 = "../../../data/data.txt";
        var outputFile = "../../../data/output.txt";
        var dataFile2 = "../../../data/data2.txt";
        var dataFile3 = "../../../data/data3.txt";

        ProcessFiles(schemaFile, dataFile1, outputFile);
        That(ReadFile(outputFile), Is.EqualTo(new string[] { "1*2/6=0", "1*7*8*5=280", "1/2*3*4*5=0", "7/9*4=0", "9/9*15*2=30", "5*2=10", "5/0*6 *** DIVBYZERO" }));
        Catch<IncorrectStringFormat>(() => { ProcessFiles(schemaFile, dataFile2, outputFile); });
        Catch<DifferentNumberOfLines>(() => { ProcessFiles(schemaFile, dataFile3, outputFile); });
    }

    [Test]
    public void ParseArgsTest()
    {
        That(ArgsParsing(new string[] { "1", "2", "3", "4" }), Is.EqualTo(("1", "2", "3")));
        Catch<NotEnoughArguments>(() => { ArgsParsing(new string[] { "a.txt" }); });
    }
}