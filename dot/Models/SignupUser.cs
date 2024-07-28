namespace WebApplication2.Models
{
    public class SignupUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Message { get; set; }
        public string Role { get; set; } 
    }
}
