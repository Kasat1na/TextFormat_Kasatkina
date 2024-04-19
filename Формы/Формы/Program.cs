using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using CsvHelper;
using System.Xml.Serialization;
using YamlDotNet.Serialization;
using System.Globalization;

namespace Формы
{
    public class FileHandler<T>
    {
        public static void WriteToFile(T data, string fileName, FileType fileType)
        {
            try
            {
                switch (fileType)
                {
                    case FileType.CSV:
                        using (var writer = new StreamWriter(fileName))
                        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csv.WriteRecords(new List<T> { data });
                        }
                        break;
                    case FileType.JSON:
                        File.WriteAllText(fileName, JsonConvert.SerializeObject(data));
                        break;
                    case FileType.XML:
                        var serializer = new XmlSerializer(typeof(T));
                        using (var writer = new StreamWriter(fileName))
                        {
                            serializer.Serialize(writer, data);
                        }
                        break;
                    case FileType.YAML:
                        var serializerYaml = new Serializer();
                        var yaml = serializerYaml.Serialize(data);
                        File.WriteAllText(fileName, yaml);
                        break;
                    default:
                        Console.WriteLine("Выбран некорректный тип файла.");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при записи файла: " + e.Message);
            }
        }

        public static T ReadFromFile(string fileName, FileType fileType)
        {
            try
            {
                switch (fileType)
                {
                    case FileType.CSV:
                        using (var reader = new StreamReader(fileName))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            var records = csv.GetRecords<T>();
                            return records.FirstOrDefault();
                        }
                    case FileType.JSON:
                        var json = File.ReadAllText(fileName);
                        return JsonConvert.DeserializeObject<T>(json);
                    case FileType.XML:
                        var serializer = new XmlSerializer(typeof(T));
                        using (var reader = new StreamReader(fileName))
                        {
                            var obj = (T)serializer.Deserialize(reader);
                            return obj;
                        }
                    case FileType.YAML:
                        var deserializer = new Deserializer();
                        var yaml = File.ReadAllText(fileName);
                        return deserializer.Deserialize<T>(yaml);
                    default:
                        Console.WriteLine("Выбран некорректный тип файла.");
                        return default(T);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка при чтении файла: " + e.Message);
                return default(T);
            }
        }
    }

    public enum FileType
    {
        CSV,
        JSON,
        XML,
        YAML
    }

    public class Model1
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            return $"Имя: {Name}, Возраст: {Age}";
        }
    }

    public class Model2
    {
        public string City { get; set; }
        public string Country { get; set; }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Доступные действия:");
            Console.WriteLine("1. Записать данные в файл");
            Console.WriteLine("2. Прочитать данные из файла");
            Console.Write("Выберите действие (1 или 2): ");

            int choice = Int32.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.Write("Введите имя файла для записи: ");
                    string writeFileName = Console.ReadLine();
                    Console.WriteLine("Выберите тип файла (CSV, JSON, XML, YAML): ");
                    FileType writeFileType = (FileType)Enum.Parse(typeof(FileType), Console.ReadLine(), true);
                    Console.WriteLine("Выберите модель данных (Model1, Model2): ");
                    string modelType = Console.ReadLine();

                    if (modelType == "Model1")
                    {
                        Model1 dataModel1 = new Model1 { Name = "Anastasia", Age = 18 };
                        FileHandler<Model1>.WriteToFile(dataModel1, writeFileName, writeFileType);
                      
                    }
                    else if (modelType == "Model2")
                    {
                        Model2 dataModel2 = new Model2 { City = "New York", Country = "USA" };
                        FileHandler<Model2>.WriteToFile(dataModel2, writeFileName, writeFileType);
                    }
                    else
                    {
                        Console.WriteLine("Некорректная модель данных.");
                    }
                    break;

                case 2:
                    Console.Write("Введите имя файла для чтения: ");
                    string readFileName = Console.ReadLine();
                    Console.WriteLine("Выберите тип файла (CSV, JSON, XML, YAML): ");
                    FileType readFileType = (FileType)Enum.Parse(typeof(FileType), Console.ReadLine(), true);
                    Console.WriteLine("Выберите модель данных (1-Model1, 2-Model2): ");
                    string modelType2 = Console.ReadLine();

                    if (modelType2 == "1")
                    {

                        var dataReadModel1 = FileHandler<Model1>.ReadFromFile(readFileName, readFileType);
                        Console.WriteLine(dataReadModel1);
                    }

                    else if (modelType2 == "2")
                    {
                        var dataReadModel2 = FileHandler<Model2>.ReadFromFile(readFileName, readFileType);
                    }
                    else
                    {
                        Console.WriteLine("Некорректная модель данных.");
                    }
                    break;

                default:
                    Console.WriteLine("Некорректный выбор действия.");
                    break;
            }
        }
    }
}