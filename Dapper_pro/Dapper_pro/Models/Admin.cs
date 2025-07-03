using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dapper_pro.Models
{
    public class Admin
    {
        public string districtname { get; set; }
        public int District_count { get; set; }
    }

    public class searchmodel2

    {
        public int pageIndex { get; set; }

        public int pageSize { get; set; }

        public string searchText { get; set; }


    }


    public class adminProfileImageUpdateRequest
    {
        public string username { get; set; }
        public string profileImage { get; set; }
    }

    public class adminProfileDetailUpdateRequest
    {
        public string username { get; set; }
        public string email { get; set; }
        public string profileImage { get; set; }
    }




   public class ResetPasswordModel
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }



    public class Course
{
    public int id {  get; set; }
    public string name { get; set; }
    public int Duration { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int IsActive { get; set; }
    public DateTime createby { get; set; }
    public string subjectname { get; set; }
    public int Semester { get; set; }
}




    public class Getcourse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int duration {  get; set; }
        public string type { get; set; }
        public DateTime createby {  get; set; }
        public string description {  get; set; }
        public int isactive { get; set; }
    }

    public class Updatecourse
    {
        public int id { get; set; }
        public string name { get; set; }
        public int duration { get; set; }
        public string type { get; set; }
        public DateTime updateby { get; set; }
        public string description { get; set; }
    }

    public class Subject
    {
        public int subjectid { get; set; }
        public string subjectname { get; set; }
    }



}
