using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;
using Dapper; // Added Dapper using directive
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            string query = @"
            SELECT Password, Role
            FROM SignupUsers
            WHERE Email = @Email";

            var parameters = new { Email = loginRequest.Email }; // Using anonymous object for parameters

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                // Using Dapper to execute the query and map the result
                var result = await connection.QuerySingleOrDefaultAsync<SignupUser>(query, parameters);

                if (result != null)
                {
                    if (loginRequest.Password == result.Password)
                    {
                        return Ok(new LoginResponse
                        {
                            Message = "Login successful",
                            Role = result.Role
                        });
                    }
                    else
                    {
                        return Unauthorized(new { message = "Invalid email or password" });
                    }
                }
                else
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }
            }
        }
    }
}
