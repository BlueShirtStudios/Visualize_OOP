using ClassExtractor;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassExtractor
{
    public class ClassToJSONConvertor
    {
        private string FilePath { get; set; }
        private JsonSerializerOptions JSONOptions { get; set; }
        public ClassToJSONConvertor()
        {
            JSONOptions = new JsonSerializerOptions { WriteIndented = true };
            FilePath = Path.Combine(AppContext.BaseDirectory, "classes.json");
        }

        public async Task WriteToJSONFile(List<ClassNode> classList)
        {
            using FileStream createStream = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(createStream, classList, JSONOptions);
        }
    }
}