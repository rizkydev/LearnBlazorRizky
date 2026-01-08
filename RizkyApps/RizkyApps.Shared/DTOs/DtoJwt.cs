
namespace RizkyApps.Shared.DTOs
{
    public record JwtConfiguration
    {
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpireDays { get; set; } = 7;
    }

    public record LoginRequest(string Username, string Password);

}
