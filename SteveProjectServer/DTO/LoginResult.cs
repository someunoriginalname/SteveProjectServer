namespace SteveProjectServer.DTO
{
    public class LoginResult
    {
        public bool success { get; set; }
        public string? message { get; set; }
        public string? token { get; set; }
    }
}