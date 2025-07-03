using Dapper;
using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Dapper_pro.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Dapper_pro.Services
{
    public class StudentService : IStudent
    {
        private readonly DBGateway _DBGateway;

        public StudentService(string connection)
        {
            _DBGateway = new DBGateway(connection);
        }

        public async Task<ResultModel<object>> GetById(int id)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@id", id);

                result.Model = await _DBGateway.ExeScalarQuery<object>("SELECT id, username, email, districtname, districtcode, statename, statecode, mobile, gender FROM md_students WHERE id = @id", par);

                if (result.Model != null)
                    result.Message = "Record Found Successfully";
                else
                    result.Message = "Record Not Found";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public async Task<ResultModel<object>> Insert(Student student)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                //par.Add("@id", student.id);
                par.Add("@username", student.username);
                par.Add("@firstname",student.firstname);
                par.Add("@lastname",student.lastname);
                par.Add("@email", student.email);
                par.Add("@districtname", student.districtname);
                par.Add("@districtcode", student.districtcode);
                par.Add("@statename", student.statename);
                par.Add("@statecode", student.statecode);
                par.Add("@mobile", student.mobile);
                par.Add("@gender", student.gender);

                var insertQuery = @"INSERT INTO md_students (username,firstname,lastname, email, districtname, districtcode, statename, statecode, mobile, gender) 
                            VALUES (@username,@firstname,@lastname, @email, @districtname, @districtcode, @statename, @statecode, @mobile, @gender);
                            SELECT LAST_INSERT_ID();";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Message = "Record Inserted Successfully";
                    result.Model = insertedId;
                }
                else
                {
                    result.Message = "Failed to Insert Record";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while inserting the record: {ex.Message}";
            }
            return result;
        }

        public async Task<ResultModel<object>> GetAllCategory(int pageIndex, int pageSize, string searchValue)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@take", pageSize);

                if (pageIndex > 0)
                {
                    par.Add("@skip1", (pageIndex - 1) * pageSize);
                }
                else
                {
                    par.Add("@skip1", 0);
                }

                par.Add("@searchValue", searchValue);

                var result = await _DBGateway.ExeQueryList<dynamic>("sp_student", par);


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
        public async Task<ResultModel<object>> Update(Student student)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@id", student.id);
                par.Add("@username", student.username);
                par.Add("@firstname",student.firstname);
                par.Add("@lastname",student.lastname);
                par.Add("@email", student.email);
                par.Add("@districtname", student.districtname);
                par.Add("@districtcode", student.districtcode); 
                par.Add("@statename", student.statename);
                par.Add("@statecode", student.statecode);
                par.Add("@mobile", student.mobile);
                par.Add("@gender", student.gender);

                var existingRecord = await _DBGateway.ExeScalarQuery<object>("SELECT 1 FROM md_students WHERE id = @id", par);

                if (existingRecord != null)
                {
                    var updateResult = await _DBGateway.ExeScalarQuery<int>("UPDATE md_students SET username = @username,firstname=@firstname, lastname=@lastname, email = @email, " +
                                                                            "districtname = @districtname, districtcode=@districtcode, statename=@statename, statecode=@statecode, mobile = @mobile, gender = @gender " +
                                                                            "WHERE id = @id;" +
                                                                            "SELECT 1", par);
                    result.Message = "Record Updated Successfully";
                    result.Model = null;
                }
                else
                {
                    result.Message = "Record not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while updating the record {ex.Message}";
            }
            return result;
        }
        public async Task<ResultModel<object>> Delete(int id)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@id", id);

                var existingRecord = await _DBGateway.ExeScalarQuery<object>("SELECT Id FROM md_students WHERE id = @id", par);

                if (existingRecord != null)
                {
                    var deleteResult = await _DBGateway.ExeQuery("DELETE FROM md_students WHERE id = @id", par) == 1 ? 1 : 2;
                    result.Message = "Record Deleted Successfully";
                    result.Model = null;
                }
                else
                {
                    result.Message = "Record not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while Deleting the record {ex.Message}";
            }
            return result;
        }

        /// <summary>
        /// //Upload profile image
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<bool> UpdateProfileImage(string username, string profileImage)
        {
            var query = "CALL sp_update_profile_image(@Username, @ProfileImage)";
            var parameters = new DynamicParameters();
            parameters.Add("Username", username);
            parameters.Add("ProfileImage", profileImage);

            var result = await _DBGateway.ExecuteAsync(query, parameters);
            return result > 0;
        }



        public async Task<bool> UpdateProfileDetail(string username, string email, string profileImage)
            {
            var query = "Update md_students SET username= @Username, email=@Email, profileImage=@ProfileImage WHERE username=@Username";
            var par= new DynamicParameters();
            par.Add("Username", username);
            par.Add("Email", email);
            par.Add("ProfileImage", profileImage);

            var result = await _DBGateway.Executeimagedetails(query,par);
            return result > 0;
        }


        /// <summary>
        /// //Student Reset password
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
         
        public async Task<ResultModel<string>> StdUpdatePasswordAsync(string username, string oldPassword, string newPassword)
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


        /// <summary>
        /// //Student Attendance
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>   
        public async Task<ResultModel<object>> AddAttendanceAsync(Attendance attendance)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@Id", attendance.Id);
                par.Add("@Date", attendance.Date);
                par.Add("@Status", attendance.Status);

                par.Add("@Type", Constants.studentrole);

                var insertQuery = @"INSERT INTO mp_attendance (Id, Type, Date, Status) VALUES (@Id, @Type, @Date, @Status);
                            SELECT LAST_INSERT_ID();";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Message = "Attendance Inserted Successfully";
                    result.Model = insertedId;
                }
                else
                {
                    result.Message = "Failed to Insert Attendance";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while inserting the record: {ex.Message}";
            }
            return result;
        }



        public async Task<AttendanceSummary> GetStudentAttendanceSummary(DateTime date)
        {
            var Studentsummary = new AttendanceSummary();
            try
            {
                var stdattendanceQuery = @"SELECT s.id AS StudentId,s.username AS Username,s.departmentid AS DepartmentId,c.name AS DepartmentName,s.type AS Type,a.status AS Status
            FROM md_students s
            INNER JOIN 
                md_course c ON s.departmentid = c.id
            INNER JOIN 
                mp_attendance a ON s.id = a.id AND s.type = a.type
            WHERE 
                CAST(a.date AS DATE) = @Date";

                var par = new DynamicParameters();
                par.Add("@Date", date.Date, DbType.Date);

                var studentattendanceStatuses = await _DBGateway.ExeQueryList<AttendanceStatus>(stdattendanceQuery, par);

                Studentsummary.AttendanceStatuses = studentattendanceStatuses.Distinct().ToList();
                Studentsummary.TotalPresent = studentattendanceStatuses.Count(a => a.Status == "Present");
                Studentsummary.TotalAbsent = studentattendanceStatuses.Count(a => a.Status == "Absent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return Studentsummary;
        }



        public async Task<List<AttendanceResult>> GetAttendanceByUsernameAsync(string username)
        {
            var query = @"SELECT a.Date, a.Status FROM md_students s JOIN mp_attendance a ON s.Id = a.Id AND s.Type = a.Type WHERE s.Username = @Username";

            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            var results = await _DBGateway.ExeQueryList<AttendanceResult>(query, parameters);
            return results;
        }



        public async Task<List<StudentSubjectMarks>> GetStudentSubjectMarks(string username)
        {
            var query = @"SELECT md_subject.subjectname,mp_studentmarks_subject.marks,mp_studentmarks_subject.A1marks,mp_studentmarks_subject.A2marks,mp_studentmarks_subject.A3marks
                  FROM md_students
                  JOIN 
                    mp_studentmarks_subject ON md_students.id = mp_studentmarks_subject.studentid
                  JOIN 
                    md_subject ON mp_studentmarks_subject.subjectid = md_subject.subjectid
                  WHERE md_students.Username = @Username AND md_students.departmentid = mp_studentmarks_subject.departmentid";

            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            var results = await _DBGateway.ExeQueryList<StudentSubjectMarks>(query, parameters);
            return results;
        }





        public async Task<List<StudentDetails>> GetStudentDetails()
        {
            string query = @"SELECT st.id, st.firstname, st.lastname,c.id AS departmentid, c.name AS departmentname,s.subjectid, sub.subjectname FROM md_students st
        INNER JOIN md_course c ON st.departmentid = c.id
        LEFT JOIN mp_studentmarks_subject s ON st.id = s.studentid
        LEFT JOIN md_subject sub ON s.subjectid = sub.subjectid";

            var result = await _DBGateway.ExeQueryList<StudentDetails>(query);
            return result;
        }


    }

}




