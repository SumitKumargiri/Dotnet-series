namespace JobSeekingApplication.Model
{
    public class Employee
    {
        public int id {  get; set; }
        public int userid {  get; set; }
        public string company_name {  get; set; }
        public string role {  get; set; }
        public string description {  get; set; }
        public string location {  get; set; }
        public string package {  get; set; }
        public string mode {  get; set; }
        public string skill {  get; set; }
        public int noofhiring {  get; set; }
        public string image { get; set; }
    }


    public class count
    {
        public int role { get; set; }
        public int role_count { get; set; }
    }
}
