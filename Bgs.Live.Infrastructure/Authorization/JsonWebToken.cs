namespace Bgs.Infrastructure.Api.Authorization
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }

        public int ExpiresInMinutes { get; set; }
    }
}