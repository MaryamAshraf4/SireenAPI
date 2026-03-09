namespace Sireen.Infrastructure.Configurations
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpirationInMinutes { get; set; }
        public int RememberMeTokenExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; }
        public int RememberMeRefreshTokenExpirationInDays { get; set; }
    }
}
