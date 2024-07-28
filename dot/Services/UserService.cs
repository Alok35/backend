using System.Linq;
using BCrypt.Net;
using WebApplication2.Models; // Your actual namespace for User model
using WebApplication2.Data;   // Your actual namespace for DbContext

namespace WebApplication2.Services // Your actual namespace for services
{
    public class UserService
    {
        private readonly AppDbContext _context; // Your actual DbContext class

        public UserService(AppDbContext context) // Inject DbContext in constructor
        {
            _context = context;
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.SingleOrDefault(u => u.Username == username);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public void CreateUser(User user)
        {
            // Hash the password before storing it
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool ValidateUser(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
