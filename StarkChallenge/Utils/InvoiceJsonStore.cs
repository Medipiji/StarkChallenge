using StarkChallenge.Interfaces;
using System.Text.Json;

namespace StarkChallenge.Utils
{
    public class InvoiceJsonStore : IInvoiceJsonStore
    {
        private const string FilePath = "invoiceJson.json";

        public bool AlreadyTransferred(string invoiceId)
        {
            var ids = Load();
            return ids.Contains(invoiceId);
        }

        public void MarkAsTransferred(string invoiceId)
        {
            var ids = Load();
            if (!ids.Contains(invoiceId))
            {
                ids.Add(invoiceId);
                Save(ids);
            }
        }

        private List<string> Load()
        {
            if (!File.Exists(FilePath)) return new List<string>();

            var json = File.ReadAllText(FilePath);

            return JsonSerializer.Deserialize<List<string>>(json)
                   ?? new List<string>();
        }

        private void Save(List<string> ids)
        {
            var json = JsonSerializer.Serialize(ids, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(FilePath, json);
        }
    }
}
