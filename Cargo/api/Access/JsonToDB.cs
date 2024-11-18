using System.Text;
using Newtonsoft.Json;

public class JsonToDb
{
    public static async void TransferJsonToDb(WebApplication app)
    {
        List<string> dataTypes = new List<string>()
        {
            "Client",
            "Inventorie",
            "Item",
            "ItemGroup",
            "ItemLine",
            "ItemType",
            "Location",
            "Order",
            "Shipment",
            "Supplier",
            "Transfer",
            "Warehouse"
        };

        List<Type> classes = new List<Type>()
        {
            typeof(Client),
            typeof(Inventory),
            typeof(Item),
            typeof(ItemGroup),
            typeof(ItemLine),
            typeof(ItemType),
            typeof(Location),
            typeof(Order),
            typeof(Shipment),
            typeof(Supplier),
            typeof(Transfer),
            typeof(Warehouse)
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
            if (dataType == "Inventorie")
            {
                accessType = Type.GetType($"InventoryAccess");
            }
            else
            {
                // Use reflection to get the service for each data type
                accessType = Type.GetType($"{dataType}Access");
            }
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

                // Convert uid to id
                if (dataType == "Item")
                {
                    foreach (var item in data)
                    {
                        var itemProperty = item.GetType().GetProperty("uid");
                        if (itemProperty != null)
                        {
                            // Get the value of the "uid" property
                            var uidValue = itemProperty.GetValue(item) as string;

                            if (!string.IsNullOrEmpty(uidValue))
                            {
                                // Extract the numeric part of the "uid" (ignoring the "P" prefix)
                                string numericPart = uidValue.Substring(1);

                                // Convert the numeric part to an integer
                                if (int.TryParse(numericPart, out int numericId))
                                {
                                    Console.WriteLine($"Converted uid to integer: {numericId}");
                                    itemProperty.SetValue(item, numericId);
                                }
                                else
                                {
                                    Console.WriteLine($"Failed to convert uid '{uidValue}' to an integer.");
                                }
                            }
                        }
                    }
                }


                // make sure ID doesn`t start with 0
                if (data.Any())
                {
                    var firstItem = data.ElementAt(0);
                    // Map and increment the id for each item if the first item's index is 0
                    int id = 1;
                    foreach (var item in data)
                    {
                        // Here we are assuming each item has an 'Id' property, and it's of type 'int'
                        var itemProperty = item.GetType().GetProperty("Id");
                        if (itemProperty != null && itemProperty.CanWrite)
                        {
                            itemProperty.SetValue(item, id);
                            id++;
                        }
                    }
                }

                if (!await access.IsTableEmpty())
                {
                    Console.WriteLine($"Table for {dataType} is not empty, skipping add operation.");
                }
                else
                {
                    try
                    {
                        // If the Add method expects a specific type, pass the typedItem
                        var addMethod = access.GetType().GetMethod("AddMany");
                        if (addMethod != null)
                        {
                            var result = await (Task<bool>)addMethod.Invoke(access, new object[] { data });
                            Console.WriteLine($"Add result: {result}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error adding item: {ex.Message}");
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