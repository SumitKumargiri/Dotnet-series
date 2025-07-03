

using Dapper;
using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Dapper_pro.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_pro.Services
{
    public class AdminService : IAdmin
    {
        private readonly DBGateway _DBGateway;

        public AdminService(string connection)
        {
            _DBGateway = new DBGateway(connection);
        }

        public async Task<ResultModel<List<Admin>>> GetDistrictCountsAsync()
        {
            ResultModel<List<Admin>> result = new ResultModel<List<Admin>>();
            try
            {
                var districtCounts = await _DBGateway.ExeQueryList<Admin>("sp_districtcount", null);

                if (districtCounts != null && districtCounts.Count > 0)
                {
                    result.Message = "District counts retrieved successfully";
                    result.Model = districtCounts;
                }
                else
                {
                    result.Message = "No district counts found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while retrieving district counts: {ex.Message}";
            }
            return result;
        }



        public async Task<ResultModel<object>> GetAllAdminData(int pageIndex, int pageSize, string searchValue)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@take", pageSize);

                if (pageIndex > 0)
                {
                    par.Add("@skip2", (pageIndex - 1) * pageSize);
                }
                else
                {
                    par.Add("@skip2", 0);
                }

                par.Add("@searchValue", searchValue);

                var result = await _DBGateway.ExeQueryList<dynamic>("sp_admin", par);


                if (result != null && result.Count > 0)
                {
                    Result.TotalRecords = Convert.ToInt32(result[0].TotalRecords);
                    // No need to remove the first row data
                }

                Result.LstModel = result;
            }
            catch (Exception ex)
            {
                Result.Message = $"Error: {ex.Message}";
            }
            return Result;
        }

        /// <summary>
        /// //Course Operation
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<ResultModel<object>> InsertCourse(Getcourse course)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@name", course.name);
                par.Add("@duration", course.duration);
                par.Add("@type", course.type);
                par.Add("@description", course.description);
                par.Add("@isactive", course.isactive);
                par.Add("@createby", course.createby);

                var insertQuery = @"INSERT INTO md_course (name,duration,type, description, isactive, createby) 
                            VALUES (@name, @duration, @type, @description, @isactive, @createby);
                            SELECT LAST_INSERT_ID();";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Success = true;
                    result.Message = "Course Inserted Successfully";
                    result.Model = insertedId;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Failed to Insert Course";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error while inserting the course: {ex.Message}";
                Console.WriteLine($"Error: {ex.ToString()}");
            }
            return result;
        }

      
        public async Task<ResultModel<object>> GetCoursesAsync()
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var subjects = await _DBGateway.ExeQueryList<Course>("SELECT * FROM md_course", null);

                if (subjects != null && subjects.Count > 0)
                {
                    result.Message = "course retrieved successfully";
                    result.Model = subjects;
                }
                else
                {
                    result.Message = "No course found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while retrieving subject: {ex.Message}";
            }
            return result;
        }



        public async Task<ResultModel<object>> UpdateCourseAsync(Updatecourse course)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var query = @"UPDATE md_course SET name = @CourseName,duration = @Duration,type = @Type,description = @Description,updateby = @UpdateBy WHERE id = @CourseID";

                var parameters = new DynamicParameters();
                parameters.Add("@CourseID", course.id);
                parameters.Add("@CourseName", course.name);
                parameters.Add("@Duration", course.duration);
                parameters.Add("@Type", course.type);
                parameters.Add("@Description", course.description);
                parameters.Add("@UpdateBy", course.updateby);

                var updateResult = await _DBGateway.ExecuteAsync(query, parameters);

                if (updateResult > 0)
                {
                    result.Success = true;
                    result.Message = "Course updated successfully";
                    result.Model = updateResult;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Failed to update course";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error while updating the course: {ex.Message}";
            }
            return result;
        }



        public async Task<ResultModel<object>> InsertCourse(Course course)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@name", course.name);
                par.Add("@duration", course.Duration);
                par.Add("@type", course.Type);
                par.Add("@description", course.Description);
                par.Add("@isactive", course.IsActive);
                par.Add("@createby", course.createby);
                par.Add("@subjectname", course.subjectname);
                par.Add("@semester", course.Semester);

                var insertQuery = "CALL sp_course_subject(@name, @duration, @type, @description, @isactive, @createby, @subjectname, @semester)";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Success = true;
                    result.Message = "Failed to insert course";
                    result.Model = insertedId;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Course Inserted Successfully";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error while inserting the course: {ex.Message}";
            }
            return result;
        }




        /// <summary>
        /// //Subject Operation
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        public async Task<ResultModel<object>> getsubject()
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var subjects = await _DBGateway.ExeQueryList<Subject>("SELECT * FROM md_subject", null);

                if (subjects != null && subjects.Count > 0)
                {
                    result.Message = "Subject retrieved successfully";
                    result.Model = subjects;
                }
                else
                {
                    result.Message = "No Subject found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while retrieving subject: {ex.Message}";
            }
            return result;
        }
        public async Task<ResultModel<object>> InsertSubject(Subject subject)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@subjectname", subject.subjectname);

                var insertQuery = @"INSERT INTO md_subject (subjectname) VALUES (@subjectname); SELECT LAST_INSERT_ID();";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Success = true;
                    result.Message = "Subject Inserted Successfully";
                    result.Model = insertedId;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Failed to Insert Subject";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error while inserting the Subject: {ex.Message}";
                Console.WriteLine($"Error: {ex.ToString()}");
            }
            return result;
        }



        /// <summary>
        /// //Upload profile image
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<bool> adminupdateprofileimage(string username, string profileImage)
        {
            var query = "CALL sp_admin_update_profile_image(@Username, @ProfileImage)";
            var parameters = new DynamicParameters();
            parameters.Add("Username", username);
            parameters.Add("ProfileImage", profileImage);

            var result = await _DBGateway.ExecuteAsync(query, parameters);
            return result > 0;
        }


        public async Task<bool> adminupdateprofiledetail(string username, string email, string profileImage)
        {
            var query = "Update md_admin SET username= @Username, email=@Email, profileImage=@ProfileImage WHERE username=@Username";
            var par = new DynamicParameters();
            par.Add("Username", username);
            par.Add("Email", email);
            par.Add("ProfileImage", profileImage);

            var result = await _DBGateway.Executeimagedetails(query, par);
            return result > 0;

        }


        /// <summary>
        /// //Admin Reset password
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<ResultModel<string>> UpdatePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var query = "UPDATE mp_login SET password = @NewPassword WHERE username = @Username AND password = @OldPassword";
            var parameters = new DynamicParameters();
            parameters.Add("NewPassword", newPassword);
            parameters.Add("OldPassword", oldPassword);
            parameters.Add("Username", username);

            var result = await _DBGateway.ExecuteAsync(query, parameters);
            if (result > 0)
            {
                return new ResultModel<string>
                {
                    Success = true,
                    Message = "Password updated successfully",
                    Model = null
                };
            }
            else
            {
                return new ResultModel<string>
                {
                    Success = false,
                    Message = "Error updating password or invalid old password",
                    Model = null
                };
            }

        }



    }
}







