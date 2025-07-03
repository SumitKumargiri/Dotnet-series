using HealthBridges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using System.Linq;

namespace HealthBridges.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HealthBridgeDbContext _context;

        public PatientController(ILogger<HomeController> logger, HealthBridgeDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Image Change
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage, int userId)
        {
            if (userId == 0)
            {
                return Json(new { success = false, message = "User ID not found." });
            }
            if (profileImage != null && profileImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Images", profileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }
                var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
                if (patient != null)
                {
                    patient.ProfileImage = "/Images/" + profileImage.FileName;
                    _context.Patients.Update(patient);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, imageUrl = Url.Content("~/Images/" + profileImage.FileName) });
                }
                else
                {
                    return Json(new { success = false, message = "Patient not found." });
                }
            }
            return Json(new { success = false, message = "Invalid image file." });
        }


        public IActionResult PatientDashboard()
        {
            ViewData["Title"] = "Patient Dashboard";
            return View();
        }


        /// <summary>
        /// Appointment
        /// </summary>
        /// <returns></returns>
        
        public IActionResult Appointment()
        {
            return View(new Appointment());
        }


        [HttpPost]
        public async Task<IActionResult> Appointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var query = "INSERT INTO Appointments (UserId,AppointmentId,DoctorName,DateOfBirth,Specialization,Description, AppointmentDate, MobileNumber) VALUES (@UserId,@AppointmentId,@DoctorName,@DateOfBirth,@Specialization,@Description, @AppointmentDate, @MobileNumber) ON DUPLICATE KEY UPDATE UserId = UserId;";

                    var parameters = new[]
                    {
                new MySqlParameter("@UserId", appointment.UserId),
                new MySqlParameter("@AppointmentId", appointment.AppointmentId),
                new MySqlParameter("@DoctorName", appointment.DoctorName),
                new MySqlParameter("@DateOfBirth", appointment.DateOfBirth),
                new MySqlParameter("@Specialization", appointment.Specialization),
                new MySqlParameter("@Description", appointment.Description),
                new MySqlParameter("@AppointmentDate", appointment.AppointmentDate),
                new MySqlParameter("@MobileNumber", appointment.MobileNumber),
            };

                    await _context.Database.ExecuteSqlRawAsync(query, parameters);
                    TempData["SuccessMessage"] = "Appointment added successfully!"; 
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the appointment. Please try again.");
                }

                return RedirectToAction("Appointment");
            }

            return View(appointment); 
        }



        //public ActionResult ViewAppointment()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("Patient/ViewAppointment")]
        public ActionResult ViewAppointment()
        {
            int UserId = Convert.ToInt32(TempData["UserId"]);
            var user = _context.Users.FirstOrDefault(u => u.Id == UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var appointments = _context.Appointments.Where(a => a.UserId == UserId).ToList();
            var viewModel = new AppointmentViewModel { User = user, Appointments = appointments };

            return View(viewModel); 
        }




        /// <summary>
        /// Upload Document
        /// </summary>
        /// <returns></returns>


        public IActionResult UploadDocument()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(PatientDocument model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        model.File = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                _context.PatientDocuments.Add(model);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        /// <summary>
        /// View Event
        /// </summary>
        /// <returns></returns>

        public IActionResult PatientEvent()
        {
            return View();
        }

        [HttpGet]
        [Route("EventPatient")]
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


        /// <summary>
        /// Chat Process
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult ChatPatient()
        {
            return View();
        }

        //[HttpGet("GetPatientChatMessages")]
        //public IActionResult GetChatMessages()
        //{
        //    var today = DateTime.UtcNow.Date;
        //    var messages = _context.ChatMessages
        //        .Where(m => m.Date == today)
        //        .OrderBy(m => m.Date)
        //        .Select(m => new { m.FullName, m.Message, m.Date })
        //        .ToList();

        //    return Json(messages);
        //}

        [HttpGet("GetPatientChatMessages")]
        public IActionResult GetChatMessages()
        {
            var messages = _context.ChatMessages
                .OrderBy(m => m.Date)
                .Select(m => new { m.FullName, m.Message, m.Date })
                .ToList();

            return Json(messages);
        }



        [HttpPost("SavePatientMessage")]
        public async Task<IActionResult> SaveMessage([FromBody] ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid message data");

            chatMessage.Date = DateTime.Today;
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

    }
}
