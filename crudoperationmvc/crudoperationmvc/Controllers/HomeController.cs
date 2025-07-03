using crudoperationmvc.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace crudoperationmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Add(Home home)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO home (FirstName, LastName, Email, MobileNumber) VALUES (@FirstName, @LastName, @Email, @MobileNumber)";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", home.FirstName);
                command.Parameters.AddWithValue("@LastName", home.LastName);
                command.Parameters.AddWithValue("@Email", home.Email);
                command.Parameters.AddWithValue("@MobileNumber", home.MobileNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }


        // +++++++++++++++++++++++ get data ++++++++++++++++++++++++++++++

        [HttpGet]
        public IActionResult Index()
        {
            List<Home> homeList = new List<Home>();
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM home";
                var command = new MySqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Home home = new Home
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNumber = Convert.ToInt32(reader["MobileNumber"])
                        };
                        homeList.Add(home);
                    }
                }
            }

            return View(homeList);
        }

        // Default Index Action
        //public IActionResult Index()
        //{
        //    return View();
        //}


        //+++++++++++++ Update data +++++++++++++++++++++++++++++++++++

        public IActionResult Edit(int id)
        {
            Home home = new Home();
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Home WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        home.Id = Convert.ToInt32(reader["Id"]);
                        home.FirstName = reader["FirstName"].ToString();
                        home.LastName = reader["LastName"].ToString();
                        home.Email = reader["Email"].ToString();
                        home.MobileNumber = Convert.ToInt32(reader["MobileNumber"]);
                    }
                }
            }

            return View(home);
        }


        [HttpPost]
        public IActionResult Edit(Home home)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "UPDATE Home SET FirstName = @FirstName, LastName = @LastName, Email = @Email, MobileNumber = @MobileNumber WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", home.Id);
                command.Parameters.AddWithValue("@FirstName", home.FirstName);
                command.Parameters.AddWithValue("@LastName", home.LastName);
                command.Parameters.AddWithValue("@Email", home.Email);
                command.Parameters.AddWithValue("@MobileNumber", home.MobileNumber);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        //+++++++++++++++++++ Delete Data +++++++++++++++++++++++++++++++++++

        public IActionResult Delete(int id)
        {
            Home home = new Home();
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Home WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        home.Id = Convert.ToInt32(reader["Id"]);
                        home.FirstName = reader["FirstName"].ToString();
                        home.LastName = reader["LastName"].ToString();
                        home.Email = reader["Email"].ToString();
                        home.MobileNumber = Convert.ToInt32(reader["MobileNumber"]);
                    }
                }
            }

            return View(home);
        }

       
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionString1");
            using (var connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM Home WHERE Id = @Id";
                var command = new MySqlCommand(query, connection);

                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
