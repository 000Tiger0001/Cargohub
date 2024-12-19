using System.Text;
using Newtonsoft.Json;

public class JsonToDb
{
    public static async Task TransferJsonToDb(WebApplication app)
    {
        List<string> dataTypes = new List<string>()
        {
            "Client",
            "Supplier",
            "Warehouse",
            "Shipment",
            "ItemGroup",
            "ItemLine",
            "ItemType",
            "Item",
            "Inventorie",
            "Location",
            "Order",
            "Transfer"
        };

        List<Type> classes = new List<Type>()
        {
            typeof(Client),
            typeof(Supplier),
            typeof(Warehouse),
            typeof(Shipment),
            typeof(ItemGroup),
            typeof(ItemLine),
            typeof(ItemType),
            typeof(Item),
            typeof(Inventory),
            typeof(Location),
            typeof(Order),
            typeof(Transfer)
        };

        using var scope = app.Services.CreateScope();
        // Folder path for data files
        string folderPath = "data";
        // Ensure the folder exists
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        // Loop through all data types
        for (int i = 0; i < dataTypes.Count; i++)
        {
            string dataType = dataTypes[i];

            // Dynamically construct the file path
            string path = $"{folderPath}/{dataType.ToLower()}s.json";

            Type? accessType;
            if (dataType == "Inventorie") accessType = Type.GetType($"InventoryAccess");
            // Use reflection to get the service for each data type
            else accessType = Type.GetType($"{dataType}Access");

            if (accessType == null) continue;

            dynamic access = scope.ServiceProvider.GetRequiredService(accessType);

            // Check if the file exists and deserialize if it does
            if (File.Exists(path))
            {
                using StreamReader reader = new(path);
                string content = await reader.ReadToEndAsync();
                content = DecodeUnicodeEscapeSequences(content);

                // Dynamically determine the item type to deserialize based on the dataType
                Type itemType = classes[i];
                Type listType = typeof(List<>).MakeGenericType(itemType);
                var items = JsonConvert.DeserializeObject(content, listType);
                var data = (IEnumerable<object>)items!;

                // make sure id is not negative and 0
                // let EF update values for you by using [DatabaseGenerated(DatabaseGeneratedOption.Identity)] in models
                if (data.Any())
                {
                    var firstItem = data.ElementAt(0);
                    foreach (var item in data)
                    {
                        var itemProperty = item.GetType().GetProperty("Id");
                        if (itemProperty != null && itemProperty.CanWrite) itemProperty.SetValue(item, null);
                    }
                }

                if (!await access.IsTableEmpty()) Console.WriteLine($"Table for {dataType} is not empty, skipping add operation.");
                else
                {
                    try
                    {
                        // If the Add method expects a specific type, pass the typedItem
                        var addMethod = access.GetType().GetMethod("AddMany");
                        if (addMethod != null)
                        {
                            var result = await (Task<bool>)addMethod.Invoke(access, new object[] { data });
                            // Console.WriteLine($"Add result: {result}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding item: {ex.Message}");
                        break;
                    }
                }
            }
        }
    }

    static string DecodeUnicodeEscapeSequences(string input)
    {
        byte[] byteArray = Encoding.Default.GetBytes(input);
        return Encoding.UTF8.GetString(byteArray);
    }
}