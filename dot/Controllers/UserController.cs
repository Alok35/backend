using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string query = @"
                SELECT DISTINCT UserId as ""UserId"",
                                FirstName as ""FirstName"",
                                LastName as ""LastName"",
                                Age as ""Age"",
                                Gender as ""Gender""
                FROM Users";
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                var users = await connection.QueryAsync<User>(query);
                return Ok(users);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            string query = @"
                INSERT INTO Users (FirstName, LastName, Age, Gender)
                VALUES (@FirstName, @LastName, @Age, @Gender)";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                var rowsAffected = await connection.ExecuteAsync(query, user);
                if (rowsAffected > 0)
                {
                    return Ok("Added Successfully");
                }
                return BadRequest("Failed to add user");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            string query = @"
                UPDATE Users
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Age = @Age,
                    Gender = @Gender
                WHERE UserId = @UserId";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                var parameters = new { UserId = id, user.FirstName, user.LastName, user.Age, user.Gender };
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                if (rowsAffected > 0)
                {
                    return Ok("Updated Successfully");
                }
                return NotFound("Update Failed: User not found");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string query = @"
                DELETE FROM Users
                WHERE UserId = @UserId";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            using (var connection = new NpgsqlConnection(sqlDataSource))
            {
                var rowsAffected = await connection.ExecuteAsync(query, new { UserId = id });
                if (rowsAffected > 0)
                {
                    return Ok("Deleted Successfully");
                }
                return NotFound("Delete Failed: User not found");
            }
        }
    }
}
