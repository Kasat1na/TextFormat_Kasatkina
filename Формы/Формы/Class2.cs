using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

//class CsvFileManager
//{
//    public List<T> ReadFromCsv<T>(string filePath)
//    {
//        using (var reader = new StreamReader(filePath))
//        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
//        {
//            return csv.GetRecords<T>().ToList();
//        }
//    }

//    public void WriteToCsv<T>(List<T> data, string filePath)
//    {
//        using (var writer = new StreamWriter(filePath))
//        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
//        {
//            csv.WriteRecords(data);
//        }
//    }
//}