using System.Text.Json.Serialization;

namespace LibraryManagement.API.Models.Common;

public class JwtSettings
{
    [JsonPropertyName("SecretKey")]
    public string Key { get; set; } = "";

    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    [JsonPropertyName("AccessTokenExpiryMinutes")]
    public int ExpiryMinutes { get; set; }
}