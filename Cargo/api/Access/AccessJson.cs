using Newtonsoft.Json;

public static class AccessJson
{
    public async static Task<List<T>> ReadJson<T>()
    {
        string folderPath = "Data";
        string path = $"{folderPath}/{typeof(T).Name}s.json";

        // Ensure the folder exists, create otherwise
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        StreamReader reader;
        List<T> items = [];

        if (File.Exists(path))
        {
            reader = new(path);
            string content = await reader.ReadToEndAsync();
            items = JsonConvert.DeserializeObject<List<T>>(content)!;
            reader.Close();
            reader.Dispose();
        }
        return items;
    }

    public async static Task WriteJson<T>(T item)
    {
        string path = $"Data/{typeof(T).Name}s.json";
        List<T> items = await ReadJson<T>();
        StreamWriter writer = new(path);

        items.Add(item);
        writer.Write(JsonConvert.SerializeObject(items, formatting: Formatting.Indented));

        writer.Close();
        writer.Dispose();
    }

    public static void WriteJsonList<T>(List<T> items)
    {
        string path = $"Data/{typeof(T).Name}s.json";
        StreamWriter writer = new(path);
        writer.Write(JsonConvert.SerializeObject(items, formatting: Formatting.Indented));
        writer.Close();
        writer.Dispose();
    }
}