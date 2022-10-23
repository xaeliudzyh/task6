using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Task1
{
    
    /// <summary>
    /// некорректность схемы преобразования
    /// </summary>
    class UnsupportedOperation : Exception
    {
        public UnsupportedOperation(char op) : base($"Operation '{op}' is unsupported")
        {
        }
    }

    
    /// <summary>
    /// несоответствие количества чисел в соответствующей строке второго файла выполняемому преобразованию:
    /// 1. Мало
    /// 2. Много
    /// </summary>
    class NotEnoughNumbers : Exception
    {
        public NotEnoughNumbers(int available, string line) : base($"More than {available} numbers required in ${line}")
        {
        }
    }
    class TooManyNumbers : Exception
    {
        public TooManyNumbers(int available, string line) : base($"Less than {available} numbers required in ${line}")
        {
        }
    }

    /// <summary>
    /// Отсутствие входного файла
    /// </summary>
    class AbsenceOfFile : Exception
    {
        public AbsenceOfFile(string fileName) : base($"File {fileName} is missed")
        {
            
        }
    }

    /// <summary>
    /// разное кол-во строк в входных файлах
    /// </summary>
    class DifferentNumberOfLines : Exception
    {
        public DifferentNumberOfLines() : base($"Files have different numbers of line")
        {
            
        }
    }

    class IncorrectStringFormat : Exception
    {
        public IncorrectStringFormat(string fileName) : base($"{fileName} have incorrect format of string")
        {
            
        }
    }

    class DiscrepancyofNumbersand : Exception
    {
        public DiscrepancyofNumbersand(int availableNumbers, int availableSymbols) : base(
            $"Discrepancy of {availableNumbers} numbers and {availableSymbols} symbols")
        {
            
        }
    }

    class InabilityOfCreatingTheFile : Exception
    {
        public InabilityOfCreatingTheFile(string fileName) : base(
            $"it is not possible to create an output file {fileName} (or a file with the same name already exists)")
        {
            
        }
    }

    public class Task1
    {
        private static readonly ILogger<Task1> Logger =
            LoggerFactory.Create(builder => { builder.AddSimpleConsole(); }).CreateLogger<Task1>();

        internal static int ApplyOperation(char op, int arg1, int arg2)
        {
            return TODO<int>();
        }

        private static Func<List<int>, int> ApplySchema(string schema)
        {
            var ops = schema.ToCharArray();
            return args =>
            {
                var res = 0;
                TODO();
                return res;
            };
        }

        private static string FormatLhs(string schema, string[] numbers)
        {
            return TODO<string>();
        }

        internal static string ProcessString(string schema, string input)
        {
            var transformation = ApplySchema(schema);
            var numbers = input.Split(",");
            var result = transformation(numbers.Select(int.Parse).ToList());
            return FormatLhs(schema, numbers) + $"={result}";
        }

        internal static void ProcessFiles(string schemasFile, string dataFile, string outputFile)
        {
            TODO();
        }

        public static void Main(string[] args)
        {
            Logger.LogInformation("program started");
            TODO();
            Logger.LogInformation("program completed");
        }

        private static void TODO()
        {
            throw new NotImplementedException();
        }

        private static T TODO<T>()
        {
            throw new NotImplementedException();
        }
    }
}