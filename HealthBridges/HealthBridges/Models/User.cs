using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HealthBridges.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Category { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual Admin Admin { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Nurse Nurse { get; set; }
        //public virtual ChatMessage ChatMessage { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual PatientDocument PatientDocument { get; set; }

    }


    public class Patient
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string MedicalRecordNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }

        public string? ProfileImage { get; set; }
        public int? MobileNumber { get; set; }
        public bool? Status {  get; set; }
        public string? Progress {  get; set; }
        public virtual User User { get; set; }
    }

    public class Admin
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? ProfileImage { get; set; } 
        public int? MobileNumber { get; set; }
        public virtual User User { get; set; }
    }

    public class Doctor
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? Specialization { get; set; }
        public string? ProfileImage { get; set; }
        public int? MobileNumber { get; set; }
        public bool? Status { get; set; }
        public virtual User User { get; set; }
    }

    public class Nurse
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public string? Department { get; set; }

        public string? ProfileImage { get; set; }
        public int? MobileNumber { get; set; }
        public bool? Status { get; set; }
        public virtual User User { get; set; }
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string DoctorName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Specialization { get; set; }
        public string Description { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public int? MobileNumber { get; set; }
        public virtual User User { get; set; }
    }


    public class PatientDocument
    {
        public int PatientDocumentId { get; set; }
        public int UserId { get; set; } 
        public string Title { get; set; }
        public string Type { get; set; }
        public string DoctorName { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public virtual User User { get; set; }
    }



    public class Event
    {
        public int EventId { get; set; }
        public DateOnly EventDate { get; set; }
        public string EventName { get; set; }
    }

    public class AppointmentViewModel
    {
        public User User { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
    }


    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string FullName { get; set; }
    }



    public class EditProfileViewModel
    {
        public int UserId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ProfileImage { get; set; }
        public int? MobileNumber { get; set; }
    }


}
