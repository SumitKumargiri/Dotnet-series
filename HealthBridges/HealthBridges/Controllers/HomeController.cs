using HealthBridges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace HealthBridges.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HealthBridgeDbContext _context;

        public HomeController(ILogger<HomeController> logger, HealthBridgeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult Signup()
        {
            var model = new SignupModel();
            return View(model);
        }


        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = HashPassword(model.Password);
                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username && u.Password == hashedPassword && u.Category == model.Category);

                if (user != null)
                {
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Category", user.Category);

                    string profileImage = null;

                    if (model.Category == "Patient")
                    {
                        var patient = _context.Patients.FirstOrDefault(p => p.UserId == user.Id);
                        profileImage = patient?.ProfileImage;
                    }
                    else if (model.Category == "Doctor" || model.Category == "Nurse")
                    {
                        var doctor = _context.Doctors.FirstOrDefault(d => d.UserId == user.Id);
                        profileImage = doctor?.ProfileImage;
                    }
                    else if (model.Category == "Admin")
                    {
                        var admin = _context.Admins.FirstOrDefault(a => a.UserId == user.Id);
                        profileImage = admin?.ProfileImage;
                    }

                    TempData["ToastrMessage"] = "Login successful!";
                    TempData["ToastrType"] = "success";

                    return Json(new
                    {
                        success = true,
                        userId = user.Id,
                        fullName = user.FullName,
                        username = user.Username,
                        category = user.Category,
                        profileImage,
                        toastrMessage = TempData["ToastrMessage"],
                        toastrType = TempData["ToastrType"]
                    });
                }
                else
                {
                    TempData["ToastrMessage"] = "Invalid username or password.";
                    TempData["ToastrType"] = "error";

                    return Json(new
                    {
                        success = false,
                        message = "Invalid username or password",
                        toastrMessage = TempData["ToastrMessage"],
                        toastrType = TempData["ToastrType"]
                    });
                }
            }

            TempData["ToastrMessage"] = "Please fill out all fields.";
            TempData["ToastrType"] = "warning";

            return Json(new
            {
                success = false,
                message = "Invalid form data",
                toastrMessage = TempData["ToastrMessage"],
                toastrType = TempData["ToastrType"]
            });
        }





        [HttpPost]
        public IActionResult Signup(SignupModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FullName = model.FullName,
                    Username = model.Username,
                    Email = model.Email,
                    Password = HashPassword(model.Password),
                    Category = model.Category
                };

                _context.Users.Add(user);

                try
                {
                    _context.SaveChanges();

                    switch (model.Category)
                    {
                        case "Admin":
                            var admin = new Admin
                            {
                                UserId = user.Id,
                                ProfileImage = null,
                                MobileNumber = null
                            };
                            _context.Admins.Add(admin);
                            break;

                        case "Doctor":
                            var doctor = new Doctor
                            {
                                UserId = user.Id,
                                ProfileImage = null,
                                MobileNumber = null,
                                Status = true
                            };
                            _context.Doctors.Add(doctor);
                            break;

                        case "Nurse":
                            var nurse = new Nurse
                            {
                                UserId = user.Id,
                                ProfileImage = null,
                                MobileNumber = null,
                                Status = true
                            };
                            _context.Nurses.Add(nurse);
                            break;

                        case "Patient":
                            var patient = new Patient
                            {
                                UserId = user.Id,
                                MedicalRecordNumber = GenerateUniqueMRN(),  
                                ProfileImage = null,
                                MobileNumber = null,
                                Status = true,
                            };
                            _context.Patients.Add(patient);
                            break;

                        default:
                            ModelState.AddModelError("", "Invalid category.");
                            return View(model);
                    }

                    _context.SaveChanges();

                    return RedirectToAction("Login");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred during registration. Please try again later.");
                    return View(model);
                }
            }

            return View(model);
        }




        private string GenerateUniqueMRN()
        {
            var guid = Guid.NewGuid().ToString();
            var mrnPrefix = guid.Substring(0, 8);
            var randomNumber = Random.Shared.Next(1, 1001);
            var mrn = $"{mrnPrefix}-{randomNumber}";

            if (_context.Patients.Any(p => p.MedicalRecordNumber == mrn))
            {
                return GenerateUniqueMRN();
            }
            return mrn;
        }


        private string HashPassword(string password)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }



    }
}
