namespace JobSeekingApplication.Model
{
    public class Admin
    {
    }

    public class employeedata
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username {  get; set; }
        public string email {  get; set; }
        public string profileimg {  get; set; }
        public int mobilenumber {  get; set; }
        public string address {  get; set; }
    }

    public class state_district
    {
        public int id { get; set; }
        public string StateCode {  get; set; }
        public string StateName { get; set; }
        public int DistrictCode {  get; set; }
        public string DistrictName { get; set;}
    }

    public class statedistrictdata
    {
        public string StateName { get; set; }
        public int DistrictCode { get; set; }
        public string DistrictName { get; set; }
    }
}
