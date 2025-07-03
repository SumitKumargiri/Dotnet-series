namespace Dapper_pro.Models
{
    public class TeacherInter
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string email {  get; set; }
        public string gender {  get; set; }
        public string statename {  get; set; }
        public int statecode {  get; set; }
        public string districtname { get; set; }
        public int districtcode { get; set; }
        public int mobile {  get; set; }
        public string qualifications {  get; set; }
        public int subjectid {  get; set; }
    }
    public class teachersearchmodel

    {
        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public string searchText { get; set; }
    }



    public class AddTeacherAttendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }



    public class TeacherAttendanceStatus
    {
        public int TeacherId { get; set; }
        public string Username { get; set; }
        public string Status { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int TeacherType { get; set; }

    }

    public class TeacherAttendanceSummary
    {
        public List<TeacherAttendanceStatus> AttendanceTeacherStatuses { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
    }


    public class TeacherAttendanceResult
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }


    /// <summary>
    /// //Teacher subject course Name
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>


    public class TeacherCourseSubjectDto
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string name { get; set; }
        public string subjectname { get; set; }
    }

    public class TeacherSubjectAssignmentDto
    {
        public string Firstname { get; set; }
        public string Subjectname { get; set; }
        public string Assignment { get; set; }
    }


    public class TeacherSubjectAssignment
    {
        public int teacherid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int subjectid { get; set; }
        public string subjectname { get; set; }
        public string departmentname { get; set; }
        public int semester { get; set; }
        public string assignment { get; set; }
    }

    //public class AddTeacherMarks
    //{

    //}
    public class StudentMarksSubject
    {
        public string firstname { get; set; }
        public string departmentname { get; set; }
        public string subjectname { get; set; }
        public int marks { get; set; }
        public int A1marks { get; set; }
        public int A2marks { get; set; }
        public int A3marks { get; set; }
    }





}
