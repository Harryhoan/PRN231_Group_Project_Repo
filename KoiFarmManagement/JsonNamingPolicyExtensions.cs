using System.Text.Json;

namespace KoiFarmManagement
{
    public static class JsonNamingPolicyExtensions
    {
        public static JsonNamingPolicy KebabCaseLower(this JsonNamingPolicy _) => new KebabCaseNamingPolicy();

        private class KebabCaseNamingPolicy : JsonNamingPolicy
        {
            public override string ConvertName(string name)
            {
                return string.Join('-', name.Split(new[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()));
            }
        }
    }
}
