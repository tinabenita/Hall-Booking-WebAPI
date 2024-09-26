using System.Collections.Generic;
using System.IO;
//using System.Text.Json;
using Newtonsoft.Json;


public static class JsonFileHelper
{
    private static readonly string JsonFilePath = "C://Users//tiber//source//repos//BanquetHallProject//Data//HallData.json";

    public static List<T> ReadFromJsonFile<T>()
    {
        using StreamReader file = File.OpenText(JsonFilePath);
        JsonSerializer serializer = new JsonSerializer();
        return (List<T>)serializer.Deserialize(file, typeof(List<T>));
    }

    public static void WriteToJsonFile<T>(List<T> data)
    {
        using StreamWriter file = File.CreateText(JsonFilePath);
        JsonSerializer serializer = new JsonSerializer
        {
            Formatting = Formatting.Indented // pretty print the JSON 
        };
        serializer.Serialize(file, data);
    }
}