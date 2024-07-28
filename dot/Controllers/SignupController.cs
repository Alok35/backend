using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using Dapper;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SignupController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupUser signupUser)
        {
            string query = @"
            INSERT INTO SignupUsers (Username, Email, Password, Role)
            VALUES (@Username, @Email, @Password, @Role)";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                await connection.OpenAsync();
                var parameters = new
                {
                    signupUser.Username,
                    signupUser.Email,
                    signupUser.Password,
                    signupUser.Role
                };

                await connection.ExecuteAsync(query, parameters);
                return Ok(new { message = "Signup successful" });
            }
        }
    }
}
