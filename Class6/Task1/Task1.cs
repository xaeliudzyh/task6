using System.ComponentModel;
using System.Text;
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
    class NotEnoughArguments : Exception
    {
        public NotEnoughArguments(int amountOfArgs) : base($"It needs than {amountOfArgs} arguments")
        {
            
        }
    }
    
    class NotEnoughNumbers : Exception
    {
        public NotEnoughNumbers(int amountOfNumbers) : base(
            $"It needs than {amountOfNumbers} nubmbers") { }
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
        public DifferentNumberOfLines(string fileName1, string fileName2) : base(
            $"Files {fileName1} and {fileName2} have different numbers of line")
        {
            
        }
    }

    
    /// <summary>
    /// неправильный формат ввода
    /// </summary>
    class IncorrectStringFormat : Exception
    {
        public IncorrectStringFormat(string line) : base($"Incorrect format of string: {line} ")
        {
            
        }
    }

    
    /// <summary>
    /// енсоответствие количество чисел и знаков
    /// </summary>
    class DiscrepancyofNumbersAndSymbols : Exception
    {
        public DiscrepancyofNumbersAndSymbols(int availableNumbers, int availableSymbols) : base(
            $"Discrepancy of {availableNumbers} numbers and {availableSymbols} symbols")
        {
            
        }
    }

    
    /// <summary>
    /// енвозможно создать выходной файл или файл с таким именем уже существует
    /// </summary>
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

        /// <summary>
        /// выполнение операции
        /// </summary>
        /// <param name="op"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        internal static int ApplyOperation(char op, int arg1, int arg2)
        {
            var operations = new Dictionary<char, Func<int, int, int>>
            {
                { '*', (a, b) => a * b },
                { '/', (a, b) => a / b }
            };
            try
            {
                return operations[op](arg1, arg2);
            }
            catch (KeyNotFoundException)
            {
                throw new UnsupportedOperation(op);
            }
        }

        /// <summary>
        /// применяем схему
        /// </summary>
        /// <param name="schema"></param>
        /// <returns></returns>
        private static Func<List<int>, int> ApplySchema(string schema)
        {
            var ops = schema.ToCharArray();
            return args =>
            {
                int res = args[0];

                for (var i = 0; i < schema.Length; i++)
                {
                    res = ApplyOperation(ops[i], res, args[i + 1]);
                }

                return res;
            };
        }

        
        /// <summary>
        /// конструируем вывод
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="numbers"></param>
        /// <returns></returns>
        public static string FormatLhs(string schema, string[] numbers)
        {
            var equation = new StringBuilder();
            equation.Append(numbers[0]);
            for (int i = 0; i < schema.Length; i++)
            {
                equation.Append(schema[i]);
                equation.Append(numbers[i + 1]);
            }

            return equation.ToString();
        }

        
        internal static string ProcessString(string schema, string input )
        {
            var transformation = ApplySchema(schema);
            var numbers = input.Split(",");
            var result = "";

            try
            {
                result = $"={transformation(numbers.Select(int.Parse).ToList())}";
            }

            catch (DivideByZeroException)
            {
                result = " *** DIVBYZERO";
            }

            catch(ArgumentOutOfRangeException)
            {
                throw new NotEnoughNumbers(numbers.Length);
            }

            catch (FormatException)
            {
                throw new IncorrectStringFormat(input);
            }


            return FormatLhs(schema, numbers) + $"{result}";
        }

        public static string[] ReadFile(string fileName)
        {
            try
            {
                var data = File.ReadAllLines(fileName);
                return data;
            }
            catch (FileNotFoundException)
            {
                throw new AbsenceOfFile(fileName);
            } 
        }
        
        /// <summary>
        /// проверка на схожесть длин файлов
        /// </summary>
        /// <param name="data"></param>
        /// <param name="scheme"></param>
        /// <param name="dataName"></param>
        /// <param name="schemeName"></param>
        /// <exception cref="DifferentNumberOfLines"></exception>
        public static void LengthChecking(string[] schema, string schemaName, string[] data, string dataName)
        {
            if (data.Length != schema.Length)
                throw new DifferentNumberOfLines(dataName, schemaName);
        }
        
        /// <summary>
        /// записываем данные в файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <exception cref="InabilityOfCreatingTheFile"></exception>
        public static void WriteFile(string fileName, List<string> data)
        {
            try
            {
                var file = new StreamWriter(fileName);
                foreach (var line in data)
                    file.WriteLine(line);
                file.Close();
            }

            catch (DirectoryNotFoundException)
            {
                throw new InabilityOfCreatingTheFile(fileName);
            }
        }

        
        /// <summary>
        /// прогоняем файлы
        /// </summary>
        /// <param name="schemaFile"></param>
        /// <param name="dataFile"></param>
        /// <param name="outputFile"></param>
        
        internal static void ProcessFiles(string schemaFile, string dataFile, string outputFile)
        {
            var schema = ReadFile(schemaFile);
            var data = ReadFile(dataFile);
            var result = new List<string>();
            LengthChecking(schema, schemaFile, data, dataFile);
            for (int i = 0; i < schema.Length; i++)
            {
                result.Add(ProcessString(schema[i], data[i]));
            }
            WriteFile(outputFile, result);
        }
        public static (string, string, string) ArgsParsing(string[] args)
        {
            try
            {
                var schemaFile = args[0];
                var dataFile = args[1];
                var outputFile = args[2];
                return (schemaFile, dataFile, outputFile);
            }

            catch (IndexOutOfRangeException)
            {
                throw new NotEnoughArguments(args.Length);
            }
        }



        public static void Main(string[] args)
        {
            Logger.LogInformation("program started");
            try
            {
                var (schemaFile, dataFile, outputFile) = ArgsParsing(args);
                ProcessFiles(schemaFile, dataFile, outputFile);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                Logger.LogInformation("program completed");
            }
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