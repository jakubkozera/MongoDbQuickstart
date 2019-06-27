namespace Project.API.Models
{
    public class UserSignedIn
    {
        public string Id { get; set; }
        public string AccessToken { get; set; }
        public string[] Roles { get; set; }
        public string Email { get; set; }
    }
}
