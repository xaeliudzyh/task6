using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Execution;
using System.Diagnostics;

namespace Task2
{
    /// <summary>
    /// невозможно создать файл выходной
    /// </summary>
    class InabilityOfCreateFile : Exception
    {
        public InabilityOfCreateFile(string fileName) : base($"Impossible to create file {fileName}") { }
    }

    /// <summary>
    /// отсутствие входного файла
    /// </summary>
    class AbsenceOfInputFile : Exception
    {
        public AbsenceOfInputFile(string fileName) : base($"{fileName} is not found") { }
    }

    /// <summary>
    /// неподходящий формат строк в inpt файле
    /// </summary>
    class IncorrectFormatOfString : Exception
    {
        public IncorrectFormatOfString(string line, int numberOfString) : base($"Incorrect format in line {numberOfString}: ${line}") { }
    }

    public class Task2
    {
        public static int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            return GCD(b, a % b);
        }
        
        private static readonly ILogger<Task2> Logger =
            LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<Task2>();
        public static int defaultCount = (int)1e5;
        public static string defaultFile = "../../../data/nums.txt";
        private static readonly string logFileName = "../../../data/logFile.txt";
        
        public static (int, string) ArgsParsing(string[] args)
        {
            int amount;
            string file;
            try
            {
                amount = int.Parse(args[0]);
            }
            catch (Exception)
            {
                Logger.LogError("Wrong count! Default value used");
                amount = defaultCount;
            }
            try
            {
                file = args[1];
            }
            catch (Exception)
            {
                file = defaultFile;
            }
            return (amount, file);
        }
        
        internal static void CreateFile(string fileName, int length = 100)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            StreamWriter file;
            try
            {
                file = new StreamWriter(fileName);
            }
            catch (DirectoryNotFoundException)
            {
                throw new InabilityOfCreateFile(fileName);
            }
            for (var i = 0; i < length; i++)
            {
                var num1 = rand.Next((int)1e9);
                var num2 = rand.Next((int)1e9);

                file.WriteLine($"{num1} {num2}");
            }
            file.Close();
        }
        
        public static void CountGCD(string fileName)
        {
            string[] data;
            var logFile = new StreamWriter(logFileName);

            try
            {
                data = File.ReadAllLines(fileName);
            }
            catch (FileNotFoundException)
            {
                throw new AbsenceOfInputFile(fileName);
            }

            for (var i = 0; i < data.Length; i++)
            {
                List<int> nums;
                try
                {
                    nums = data[i].Split(' ').Select(int.Parse).ToList();
                }
                catch (Exception)
                {
                    throw new IncorrectFormatOfString(data[i], i);
                }

                var startDate = DateTime.Now.Ticks * 100;

                GCD(nums[0], nums[1]);

                var finishDate = DateTime.Now.Ticks * 100;
                logFile.WriteLine(finishDate - startDate);
            }

            logFile.Close();
        }
        
        public static void LogAnalizing()
        {
            var logFile = File.ReadAllLines(logFileName);

            var times = logFile.Select(double.Parse);

            try
            {
                double avgTime = times.Sum() / times.Count();
                Logger.LogInformation($"Average time: {avgTime} nanoseconds");
            }
            catch (DivideByZeroException)
            {
                Logger.LogError("Logfile is empty");
            }
        }
        public static void Main(string[] args)
        {
            Logger.LogInformation("program started");

            var (count, file) = ArgsParsing(args);
            CreateFile(file, count);
            CountGCD(file);
            LogAnalizing();

            Logger.LogInformation("program completed");
        }
    }
}