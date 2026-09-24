using System.Text.Json.Serialization;

namespace LibraryManagement.API.Models.Common;

public class JwtSettings
{
    public string Secret { get; set; } = "";

    public string Issuer { get; set; } = "";

    public string Audience { get; set; } = "";

    public int AccessTokenExpiryMinutes { get; set; }
}