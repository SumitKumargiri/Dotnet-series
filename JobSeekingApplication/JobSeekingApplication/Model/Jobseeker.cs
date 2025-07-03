namespace JobSeekingApplication.Model
{
    public class Jobseeker
    {
        public int id {  get; set; }
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string username {  get; set; }
        public string email {  get; set; }
        public string image {  get; set; }
        public int mobilenumber {  get; set; }
        public string address {  get; set; }
    }

    public class jsapplier
    {
        public int jseekerid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string StateName { get; set; }
        public string DistrictName { get; set; }
        public string role {  get; set; }
        public int mobilenumber { get; set; }
        public string resume {  get; set; }
    }

    public class Jobdata
    {
        public int id { get; set; }
        public string role { get;set; }
        public string company_name { get; set; }
        public string location { get; set; }
        public string package {  get; set; }
        public string mode {  get; set; }
        public string skill {  get; set; }
        public int noofhiring {  get; set; }
        public string image { get; set; }

        public string description {  get; set; }
    }

    public class viewapplieddata
    {
        public int id { get; set; }
        public string company_name { get; set; }
        public string role { get; set; }
        public string location { get; set; }
        public string package { get; set; }
        public string mode { get; set; }
        public string skill { get; set; }
        public int noofhiring { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public DateTime appliedby { get; set; }
    }
}
