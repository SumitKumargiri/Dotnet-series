namespace Dapper_pro.Models
{
    static public class Constants
    {

        public const int adminrole = 1;

        public const int studentrole = 2;

        public const int teacherrole = 3;   


        public const int COUNTRYCODE = 1;

        public const int SUCCESS = 1;

        public const int VALIDATION_ERROR = 10;

        public const int INVALID = 11;

        public const int EXCEPTION = 12;

        public const int NOTDELETED = 13;

        public const int NOTCREATED = 14;

        public const int NOTUPDATED = 15;

        public const int MULTIINSERTDELETE = 16;

        public const int USERNOTEXIST = 17;

        public const int RECORDNOTEXIST = 18;

        public const int INVALIDCAPTCHA = 19;

        public const int USERNAME_PASSWORD_NOTMATCHED_CODE = 20;

        public const int UNMAPPEDSUCCESS = 21;

        public const int EMAILNOTEXIST = 22;

        public const int MOBILENOTEXIST = 23;

        public const int INVALIDMOBILEOTP = 24;

        public const int INVALIDEMAILOTP = 25;

        public const int CANNOT_DELETED = 26;

        public const int PROFILE_INCOMPLETED = 27;

        public const int INVALIDFILE = 28;

        public const int MOBILEEMAILREQUIRED = 29;

        public const int MOBILEALREADYEXIST = 30;

        public const int EMAILALREADYEXIST = 31;

        public const int MCINUMNERALREADYEXIST = 32;

        public const int MOBILENOREQUIRED = 33;

        public const int USERBLOCKED = 34;

        public const int DOB_VALIDATION = 35;

        public const int ROASTERTIME_VALIDATION = 36;

        public const int FOLLOWUP_MISSING = 37;

        public const int CANNOT_DEACTIVATED = 38;

        public const int DELETE_MESSAGE_CODE = 39;

        public const int NEGATIVE_VALUE_CODE = 40;

        public const int ALREADY_MAPPED = 41;

        public const int TRY_AFTERSOMETIME_CODE = 42;

        public const int ABHA_RECORD_EXIST_CODE = 43;

        public const int PRACTITIONER_STATUS_CODE = 44;

        public const string ROASTERTIME_VALIDATION_MESSAGE = "RoasterTime format is not valid.";

        public const string DOB_VALIDATION_MEASSAGE = "DOB can't be same year as current year.";

        public const string INVALID_ATTEMPT = "Invalid Attempt.";

        public const string INVALID_CREDENTIALS = "Invalid Credentials.";

        public const string INVALID_CAPTCHA = "Invalid CAPTCHA.";

        public const string NOTMATCHED_OPT = "Invalid OTP.";

        public const string EXCEPTION_MESSAGE = "Exception";

        public const string NOTUSEREXISTS_MESSAGE = "User not exists";

        public const string DELETE_MESSAGE = "Record Deleted Successfully";

        public const string NOTDELETE_MESSAGE = "There Is No Record to Delete";

        public const string ALREADY_ASSOCIATED_MESSAGE = "Record already linked with other services";

        public const string ALREADY_EXISTS_MESSAGE = "Record already exist";

        public const string CREATED_MESSAGE = "Record saved successfully";

        public const string NOTCREATED_MESSAGE = "Unable to create the record";

        public const string UNMAPPED_SUCCESS = "Record unmapped successfully";

        public const string NOTUPDATED_MESSAGE = "There Is No Record to Update";

        public const string UPDATED_MESSAGE = "Record updated successfully";

        public const string MULTIINSERTDELETE_MESSAGE = "Multi Insert Delete Executed Successfully";

        public const string NORECORDFOUND_MESSAGE = "Record not found";

        public const string USERNAMEPASSWORD_NOTMATCHED = "User/Password not macthed";

        public const string ALREADY_EXISTS_AND_Archived_MESSAGE = "Record already exist and Archived";

        public const string EMAIL_NOTEXIST = "User email not exist";

        public const string MOBILE_NOTEXIST = "User mobile not exist";

        public const string INVALIDMOBILEOTP_MESSAGE = "Invalid mobile OTP";

        public const string INVALIDEMAILOTP_MESSAGE = "Invalid email OTP";

        public const string CANNOT_DELETED_MESSAGE = "Record can't be deleted as it has associated information.";

        public const string PROFILE_INCOMPLETED_MESSAGE = "Profile not completed.";

        public const string INVALIDFILE_MESSAGE = "Invalid file.";

        public const string MOBILEEMAILREQUIRED_MESSAGE = "Mobile/Email required.";

        public const string MOBILEALREADYEXIST_MESSAGE = "Mobile already exist.";

        public const string EMAILALREADYEXIST_MESSAGE = "Email already exist.";

        public const string MCINUMNERALREADYEXIST_MESSAGE = "MCI Number already exist.";

        public const string MOBILENOREQUIRED_MESSAGE = "Mobile number is required.";

        public const string USERBLOCKED_MESSAGE = "User blocked not able to login.";

        public const string FOLLOWUP_MISSING_MESSAGE = "Followup prescription required.";

        public const string CANNOT_DEACTIVATED_MESSAGE = "Record can't be deactivated as it has associated information.";

        public const string CANNOT_UPDATE_NEGATIVE_MESSAGE = "Record can't be Update as it having negative values.";

        public const string ALREADY_MAPPED_MESSAGE = "Record already mapped with another health facility.";

        public const string TRY_AFTERSOMETIME_MESSAGE = "Please try after some time.";

        public const string ABHA_RECORD_EXIST_MESSAGE = "ABHA details already linked with other patient.";

        public const string PRACTITIONER_STATUS_MESSAGE = "Already in consultation.";
    }

}
