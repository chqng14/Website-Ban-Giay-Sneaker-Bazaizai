using System.Text.Json;

namespace App_Api.Helpers.CustomJson
{
    public class CustomJsonNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        }
    }
}
