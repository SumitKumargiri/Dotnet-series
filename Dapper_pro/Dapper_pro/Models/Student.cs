using System.ComponentModel.DataAnnotations.Schema;
namespace Dapper_pro.Models
{
    public class Student
    
        {
        public int id { get; set; }
        public string username { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string statename {  get; set; }
        public int statecode {  get; set; }
        public string districtname { get; set; }
        public int districtcode {  get; set; }
        public int mobile { get; set; }
        public string status { get; set; }
        public string gender { get; set; }
    }



    public class searchmodel

    {
        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public string searchText { get; set; }


    }


    public class ProfileImageUpdateRequest
    {
        public string username { get; set; }
        public string profileImage { get; set; }
    }

    public class ProfileDetailUpdateRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string profileImage { get; set; }
    }

    public class StdResetPasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }


    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        //public string Type { get; set; }
    }



    public class AttendanceStatus
    {
        public int StudentId { get; set; }
        public string Username { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Type { get; set; }
        public string Status { get; set; }
    }

    public class AttendanceSummary
    {
        public List<AttendanceStatus> AttendanceStatuses { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
    }


    public class AttendanceResult
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }

    public class StudentSubjectMarks
    {
        public string subjectname { get; set; }
        public int marks { get; set; }
        public int A1marks {  get; set; }
        public int A2marks { get; set;}
        public int A3marks { get; set;}
    }


    public class StudentDetails
    {
        public int id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string departmentname { get; set; }
        public int departmentid { get; set; }
        public string subjectname { get; set; }
        public int subjectid { get; set; }
    }


}

