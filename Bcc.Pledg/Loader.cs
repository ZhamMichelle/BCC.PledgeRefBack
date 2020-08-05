using System;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace Bcc.Pledg
{
    public class Loader
    {
        public static async Task<TElem[]> Load<TElem>(string jsonFile)
        {
            using var jsonFileStream = File.OpenRead(jsonFile);
            var value = await JsonSerializer.DeserializeAsync<TElem[]>(jsonFileStream);
            return value;
        }

        public static async Task<object> Load(Type type, string jsonFile)
        {
            using var jsonFileStream = File.OpenRead(jsonFile);
            var value = await JsonSerializer.DeserializeAsync(jsonFileStream, type.MakeArrayType(), new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            return value;
        }
    }
}
