using System.Text.Json;
using System.Text;

namespace Administrator.Helper
{
    public static class JwtHelper
    {
        public static string[]? GetRolesFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var parts = token.Split('.');
            if (parts.Length < 2)
                return null;

            string base64 = parts[1];
            base64 = base64.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            try
            {
                var bytes = Convert.FromBase64String(base64);
                var json = Encoding.UTF8.GetString(bytes);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("role", out var roleProp))
                {
                    return ExtractRoles(roleProp);
                }

                if (root.TryGetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", out var schemaRoleProp))
                {
                    return ExtractRoles(schemaRoleProp);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private static string[] ExtractRoles(JsonElement roleElement)
        {
            switch (roleElement.ValueKind)
            {
                case JsonValueKind.String:
                    return new[] { roleElement.GetString()! };

                case JsonValueKind.Array:
                    var roles = new string[roleElement.GetArrayLength()];
                    int i = 0;
                    foreach (var item in roleElement.EnumerateArray())
                    {
                        roles[i++] = item.GetString()!;
                    }
                    return roles;

                default:
                    return Array.Empty<string>();
            }
        }
    }
}
