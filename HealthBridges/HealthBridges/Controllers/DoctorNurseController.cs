using HealthBridges.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace HealthBridges.Controllers
{
    public class DoctorNurseController : Controller
    {
        private readonly HealthBridgeDbContext _context;

        public DoctorNurseController(HealthBridgeDbContext context)
        {
            _context = context;
        }
               
        [HttpGet]
        public IActionResult DoctorNurseDashboard()
        {
            ViewData["Title"] = "Doctor/Nurse Dashboard";
            return View();
        }

        /// <summary>
        /// Change Image
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> DoctorUploadProfileImage(IFormFile doctorprofileImage, int userId)
        {
            if (userId == 0)
            {
                return Json(new { success = false, message = "User ID not found." });
            }
            if (doctorprofileImage != null && doctorprofileImage.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Images", doctorprofileImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await doctorprofileImage.CopyToAsync(stream);
                }
                var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == userId);
                if (doctor != null)
                {
                    doctor.ProfileImage = "/Images/" + doctorprofileImage.FileName;
                    _context.Doctors.Update(doctor);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, imageUrl = Url.Content("~/Images/" + doctorprofileImage.FileName) });
                }
                else
                {
                    return Json(new { success = false, message = "Doctor Data not found." });
                }
            }
            return Json(new { success = false, message = "Invalid image file." });
        }

        /// <summary>
        /// Manage Patient Details
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult ManagePatients()
        {
            var category = HttpContext.Session.GetString("Category");

            if (string.IsNullOrEmpty(category) || (category != "Doctor" && category != "Nurse"))
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
                UserId = patient.User.Id,
                FullName = patient.User.FullName,
                Email = patient.User.Email,
                MedicalRecordNumber = patient.MedicalRecordNumber,
                DateOfBirth = patient.DateOfBirth,
                Address = patient.Address,
                MobileNumber = patient.MobileNumber,
                Status = patient.Status,
                Progress = patient.Progress
            });
        }


        public IActionResult EditPatient(string medicalRecordNumber)
        {
            var patient = _context.Patients
                .Include(p => p.User)
                .FirstOrDefault(p => p.MedicalRecordNumber == medicalRecordNumber);

            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }




        [HttpPost]
        public IActionResult UpdatePatientDetails(Patient patient)
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



        public IActionResult DeletePatient(int id)
        {
            var patient = _context.Patients.FirstOrDefault(p => p.MedicalRecordNumber == id.ToString()); 

            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            _context.SaveChanges();

            return RedirectToAction("ManagePatients");
        }

        /// <summary>
        /// Chat Process
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult Chatdoctornurse()
        {
            return View();
        }

        [HttpGet("GetDoctorNurseChatMessages")]
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


        [HttpPost("SaveDoctorNurseMessage")]
        public async Task<IActionResult> SaveMessage([FromBody] ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid message data");

            chatMessage.Date = DateTime.Today;
            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        /// <summary>
        /// Event Calendar
        /// </summary>
        /// <returns></returns>

        public IActionResult DoctorNurseEvent()
        {
            return View();
        }

        [HttpGet]
        [Route("EventDoctorNurse")]
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


        public async Task<IActionResult> ViewAppointments()
        {
            var appointments = await _context.Appointments
                .Include(a => a.User) // Include the User data
                .Select(a => new
                {
                    FullName = a.User.FullName,
                    Email = a.User.Email,
                    DateOfBirth = a.DateOfBirth,
                    AppointmentDate = a.AppointmentDate,
                    Description = a.Description,
                    MobileNumber = a.MobileNumber
                })
                .ToListAsync();

            return View(appointments);
        }


    }
}


