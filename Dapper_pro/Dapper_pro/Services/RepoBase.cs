
using Dapper_pro.Code;
using Microsoft.Extensions.Options;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using Dapper_pro.Models;
using Dapper_pro.Interfaces;

namespace Dapper_pro.Services
{
    public class RepoBase : IRepoBase
    {
       
            private readonly IConfiguration _config;

            private string _connectionString;

            public RepoBase(IConfiguration config)
            {
                _config = config;
                _connectionString = _config.GetConnectionString("ConnectionString1");
            }



            public List<Student> GetAll()
            {
                using IDbConnection db = new MySqlConnection(_connectionString);
                string query = "SELECT * FROM md_student";
                return db.Query<Student>(query).ToList();
            }

            public Student GetById(int id)
            {
                using IDbConnection db = new MySqlConnection(_connectionString);
                string query = "SELECT * FROM students WHERE id = @Id";
                return db.QueryFirstOrDefault<Student>(query, new { Id = id });
            }

            public async Task<int> AddStudent(Student _student)
            {
                using IDbConnection db = new MySqlConnection(_connectionString);
                string query = "insert into students(id,name,email,District_Name,mobile,gender)values(@Id,@name,@email,@districtname,@mobile,@gender)";
                return await db.ExecuteAsync(query, _student);
            }


            public async Task<int> UpdateStudent(Student _student)
            {
                using IDbConnection db = new MySqlConnection(_connectionString);
                string query = "UPDATE students SET name = @name, email = @email, districtname = @districtname, mobile = @mobile, gender = @gender WHERE id = @Id";
                return await db.ExecuteAsync(query, new { _student.id, _student.username, _student.email, _student.districtname, _student.mobile, _student.gender });

            }

            public async Task<int> DeleteStudent(int id)
            {
                using IDbConnection db = new MySqlConnection(_connectionString);
                string query = "DELETE FROM students WHERE id = @Id";
                return await db.ExecuteAsync(query, new { Id = id });
            }

        }
    }
