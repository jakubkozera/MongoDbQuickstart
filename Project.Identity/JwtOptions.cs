namespace Project.Identity
{
    public class JwtOptions
    {
        public const string SectionName = "jwt";
        public string JwtKey { get; set; }

        public string JwtIssuer { get; set; }

        public long JwtExpireDays { get; set; }

    }
}
