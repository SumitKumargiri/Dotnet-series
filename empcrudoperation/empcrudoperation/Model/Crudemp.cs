namespace empcrudoperation.Model
{
    public class Crudemp
    {
        public int empid { get; set; } 
        public string firstname { get; set; }
        public string lastname { get; set; }
        public DateTime dob { get; set; }
        public string gender { get; set; }
        public string qualification { get; set; }
        public string email {  get; set; }
        public int phonenumber {  get; set; }
        public bool isactive {  get; set; }
    }

    public class attendance
    {
        public int empid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int isactive { get; set; }
        public DateTime timein { get; set; }
        public DateTime timeout { get; set; }
        public int total { get; set; }
        public string status { get; set; }
    }
}
