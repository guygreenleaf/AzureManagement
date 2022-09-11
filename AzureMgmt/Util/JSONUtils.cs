using Newtonsoft.Json;
using AzureMgmt.DTO;

namespace AzureMgmt.Util
{
    public class JSONUtils : IJSONUtils
    {
        private readonly JSONItems jsonItems;

        public JSONUtils()
        {
            using StreamReader r = new StreamReader("Secrets.json");
            string json = r.ReadToEnd();
            jsonItems = JsonConvert.DeserializeObject<JSONItems>(json) ?? new JSONItems();
        }
        public JSONItems GetJSONItems() => jsonItems;
    }
}
