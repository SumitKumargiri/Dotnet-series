using HealthBridges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace HealthBridges.Controllers
{
    public class AdminController : Controller
    {
        private readonly HealthBridgeDbContext _context;
        private readonly string _connectionString;

        public AdminController(HealthBridgeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("ConnectionString1");
        }

        [HttpGet]
        public IActionResult AdminDashboard()
        {
            ViewData["Title"] = "Admin Dashboard";
            return View();
        }


        /// <summary>
        /// Admin Profile image change.
        /// </summary>
        /// <returns></returns>


        [HttpPost]
        public async Task<IActionResult> AdminUploadProfileImage(IFormFile adminprofileImage, int userId)
        {
            if (userId == 0)
            {
                return Json(new { success = false, message = "User ID not found." });
            }
            if (adminprofileImage != null && adminprofileImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Images", adminprofileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await adminprofileImage.CopyToAsync(stream);
                }
                var admin = await _context.Admins.FirstOrDefaultAsync(a => a.UserId == userId);
                if (admin != null)
                {
                    admin.ProfileImage = "/Images/" + adminprofileImage.FileName;
                    _context.Admins.Update(admin);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, imageUrl = Url.Content("~/Images/" + adminprofileImage.FileName) });
                }
                else
                {
                    return Json(new { success = false, message = "Admin Data not found." });
                }
            }
            return Json(new { success = false, message = "Invalid image file." });
        }


        /// <summary>
        /// Patients Managements.
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult AdminManagePatients()
        {
            var category = HttpContext.Session.GetString("Category");

            if (string.IsNullOrEmpty(category) || (category != "Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var patients = _context.Patients
                .Include(p => p.User)
                .Select(p => new
                {
                    UserId = p.User.Id,
                    FullName = p.User.FullName,
                    Email = p.User.Email,
                    MedicalRecordNumber = p.MedicalRecordNumber,
                    DateOfBirth = p.DateOfBirth,
                    Address = p.Address,
                    MobileNumber = p.MobileNumber,
                    Status = p.Status,
                    Progress = p.Progress
                })
                .ToList();

            return View(patients);
        }


        public IActionResult ViewPatient(int id)
        {
            var patient = _context.Patients
                .Include(p => p.User)
                .FirstOrDefault(p => p.MedicalRecordNumber == id.ToString());

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        [HttpGet("GetPatientDetails")]
        public IActionResult GetPatientDetails(string medicalRecordNumber)
        {
            var patient = _context.Patients
                .Include(p => p.User)
                .FirstOrDefault(p => p.MedicalRecordNumber == medicalRecordNumber);

            if (patient == null)
            {
                return NotFound();
            }

            return Json(new
            {
                userId = patient.User.Id,
                fullName = patient.User.FullName,
                email = patient.User.Email,
                medicalRecordNumber = patient.MedicalRecordNumber,
                dateOfBirth = patient.DateOfBirth,
                address = patient.Address,
                mobileNumber = patient.MobileNumber,
                status = patient.Status,
                progress = patient.Progress
            });
        }




        [HttpGet("EditPatient")]
        public IActionResult EditPatient(string medicalRecordNumber)
        {
            var patient = _context.Patients
                .Include(p => p.User)
                .FirstOrDefault(p => p.MedicalRecordNumber == medicalRecordNumber);

            if (patient == null)
            {
                return NotFound();
            }
            return Json(new
            {
                user = new
                {
                    fullName = patient.User.FullName,
                    email = patient.User.Email,
                    id = patient.User.Id
                },
                dateOfBirth = patient.DateOfBirth,
                address = patient.Address,
                mobileNumber = patient.MobileNumber,
                status = patient.Status,
                progress = patient.Progress,
                medicalRecordNumber = patient.MedicalRecordNumber
            });
        }



        [HttpPost("UpdatePatientDetails")]
        public IActionResult UpdatePatientDetails([FromBody] Patient patient)
        {
            var existingPatient = _context.Patients
                .FirstOrDefault(p => p.MedicalRecordNumber == patient.MedicalRecordNumber);

            if (existingPatient == null)
            {
                return Json(new { success = false });
            }
            existingPatient.DateOfBirth = patient.DateOfBirth;
            existingPatient.Address = patient.Address;
            existingPatient.MobileNumber = patient.MobileNumber;
            existingPatient.Status = patient.Status;
            existingPatient.Progress = patient.Progress;

            _context.SaveChanges();

            return Json(new { success = true });
        }


        /// <summary>
        /// [param]
        /// Staff management
        /// [param]
        /// </summary>
        /// <returns></returns>

        public IActionResult StaffManagement()
        {
            // Retrieve doctors with category "Doctor"
            var doctors = _context.Users
                .Where(u => u.Category == "Doctor")
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    DoctorInfo = u.Doctor != null ? new
                    {
                        u.Doctor.Specialization,
                        u.Doctor.MobileNumber,
                        u.Doctor.Status
                    } : null
                })
                .ToList();

            // Retrieve nurses with category "Nurse"
            var nurses = _context.Users
                .Where(u => u.Category == "Nurse")
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email,
                    NurseInfo = u.Nurse != null ? new
                    {
                        u.Nurse.Department,
                        u.Nurse.MobileNumber,
                        u.Nurse.Status
                    } : null
                })
                .ToList();

            // Combine the results for display
            var staff = new
            {
                Doctors = doctors,
                Nurses = nurses
            };

            return View(staff);
        }


        [HttpGet]
        [Route("GetStaffDetails")]
        public IActionResult GetStaffDetails(int Id)
        {
            //var doctor = _context.Doctors.Include(d => d.User).FirstOrDefault(d => d.Id == id);
            //var nurse = _context.Nurses.Include(n => n.User).FirstOrDefault(n => n.Id == id);

            //if (doctor != null)
            //{
            //    return Ok(new
            //    {
            //        FullName = doctor.User.FullName,
            //        Email = doctor.User.Email,
            //        Role = "Doctor",
            //        MobileNumber = doctor.MobileNumber,
            //        Status = doctor.Status ,
            //        Specialization = doctor.Specialization
            //    });
            //}
            //else if (nurse != null)
            //{
            //    return Ok(new
            //    {
            //        FullName = nurse.User.FullName,
            //        Email = nurse.User.Email,
            //        Role = "Nurse",
            //        MobileNumber = nurse.MobileNumber,
            //        Status = nurse.Status,
            //        Department = nurse.Department
            //    });
            //}

            //return NotFound(new { message = "Staff member not found." });

            var doctor = _context.Doctors.Include(d => d.User).FirstOrDefault(d => d.Id == Id);

            if (doctor == null)
            {
                return NotFound();
            }

            return Json(new
            {
                userId = doctor.User.Id,
                fullName = doctor.User.FullName,
                email = doctor.User.Email,
                mobileNumber = doctor.MobileNumber,
                status = doctor.Status,
                specialization = doctor.Specialization
            });
        }


        /// <summary>
        /// [param]
        /// Event details
        /// [param]
        /// </summary>
        /// <returns></returns>

        public IActionResult Event()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminDashboard");
            }
            return View("AdminDashboard", await _context.Events.ToListAsync());
        }

        [HttpGet]
        [Route("Event/GetEvents")]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Select(e => new
                {
                    title = e.EventName,
                    start = e.EventDate.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            return Json(events);
        }


        [HttpGet]
        [Route("Event/GetEventByDate")]
        public async Task<IActionResult> GetEventByDate(DateOnly date)
        {
            var eventItem = await _context.Events
                .Where(e => e.EventDate == date) 
                .Select(e => new { e.EventName })
                .FirstOrDefaultAsync();

            if (eventItem == null)
                return NotFound();

            return Json(new { eventName = eventItem.EventName });
        }

        [HttpPost]
        [Route("Event/UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] Event updatedEvent)
        {
            var eventItem = await _context.Events
                .FirstOrDefaultAsync(e => e.EventDate == updatedEvent.EventDate);

            if (eventItem != null)
            {
                eventItem.EventName = updatedEvent.EventName; 
                await _context.SaveChangesAsync(); 
                return Ok(); 
            }

            return NotFound(); 
        }


        /// <summary>
        /// [param]
        /// Chat Process
        /// [param]
        /// </summary>
        /// <returns></returns>


        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }

        [HttpGet("GetChatMessages")]
        public IActionResult GetChatMessages()
        {
            var today = DateTime.UtcNow.Date; 
            var messages = _context.ChatMessages
                .Where(m => m.Date == today) 
                .OrderBy(m => m.Date)
                .Select(m => new { m.FullName, m.Message, m.Date })
                .ToList();

            return Json(messages);
        }


        [HttpPost("SaveMessage")]
        public async Task<IActionResult> SaveMessage([FromBody] ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid message data");

            chatMessage.Date = DateTime.Today;
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }


        // Admin Profile Edit Page

        //[HttpGet]
        //public IActionResult GetUserProfile(int userId)
        //{
        //    var user = _context.Users.Include(u => u.Admin).FirstOrDefault(u => u.Id == userId);

        //    if (user == null)
        //    {
        //        return NotFound(new { message = "User not found." });
        //    }

        //    return Json(new
        //    {
        //        fullName = user.FullName,
        //        email = user.Email,
        //        username = user.Username,
        //        mobileNumber = user.Admin?.MobileNumber,
        //        profileImage = user.Admin?.ProfileImage
        //    });
        //}


        //[HttpPost]
        //public IActionResult UpdateUserProfile(User user)
        //{
        //    var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        //    if (existingUser == null) return Json(new { success = false });

        //    existingUser.FullName = user.FullName;
        //    existingUser.Email = user.Email;
        //    existingUser.Username = user.Username;

        //    existingUser.Admin.ProfileImage = user.Admin.ProfileImage;

        //    _context.SaveChanges();
        //    return Json(new { success = true });
        //}




        // GET: EditProfile
        public ActionResult GetUserProfile(int userId)
        {
            // Retrieve user details from the Users table
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Retrieve admin details if the user is an admin
            var admin = _context.Admins.FirstOrDefault(a => a.UserId == userId);

            // Prepare the ViewModel
            var viewModel = new EditProfileViewModel
            {
                UserId = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                ProfileImage = admin?.ProfileImage,
                MobileNumber = admin?.MobileNumber
            };

            return View(viewModel);
        }

        // POST: EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUserProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Update user details
            var user = _context.Users.FirstOrDefault(u => u.Id == model.UserId);
            if (user == null)
            {
                return NotFound("User not found.");

            }

            user.FullName = model.FullName;
            user.Username = model.Username;
            user.Email = model.Email;

            // Update admin details if present
            var admin = _context.Admins.FirstOrDefault(a => a.UserId == model.UserId);
            if (admin != null)
            {
                admin.ProfileImage = model.ProfileImage;
                admin.MobileNumber = model.MobileNumber;
            }

            _context.SaveChanges();
            TempData["Message"] = "Profile updated successfully.";
            return RedirectToAction("GetUserProfile", new { userId = model.UserId });
        }
    


    /// <summary>
    /// Reset Password
    /// </summary>
    /// <returns></returns>

    [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string OldPassword, string NewPassword, string ConfirmNewPassword, string FullName)
        {
            // Validate if FullName is provided
            if (string.IsNullOrEmpty(FullName))
            {
                TempData["ErrorMessage"] = "User not logged in.";
                return RedirectToAction("AdminDashboard", "Admin");
            }

            // Hash the old password for comparison
            string hashedOldPassword = HashPassword(OldPassword);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Check if the user's old password matches the database password
                string query = "SELECT Password FROM Users WHERE FullName = @FullName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FullName", FullName);
                    var dbPassword = command.ExecuteScalar()?.ToString();

                    // If the old password does not match
                    if (dbPassword != hashedOldPassword)
                    {
                        ModelState.AddModelError("", "The old password is incorrect.");
                        return View();
                    }
                }

                // Ensure new password and confirmation match
                if (NewPassword != ConfirmNewPassword)
                {
                    ModelState.AddModelError("", "New password and confirmation do not match.");
                    return View();
                }

                // Ensure the new password meets security criteria
                if (NewPassword.Length < 8)
                {
                    ModelState.AddModelError("", "The new password must be at least 8 characters long.");
                    return View();
                }

                // Hash the new password
                string hashedNewPassword = HashPassword(NewPassword);

                // Update the password in the database
                query = "UPDATE Users SET Password = @NewPassword WHERE FullName = @FullName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
                    command.Parameters.AddWithValue("@FullName", FullName);

                    int rowsAffected = command.ExecuteNonQuery();

                    // Check if the update was successful
                    if (rowsAffected == 0)
                    {
                        TempData["ErrorMessage"] = "Password update failed. Please try again.";
                        return RedirectToAction("ResetPassword", "Admin");
                    }
                }
            }

            TempData["SuccessMessage"] = "Password reset successfully!";
            return RedirectToAction("Index", "Home");
        }

        // Helper function to hash the password using SHA-512
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
