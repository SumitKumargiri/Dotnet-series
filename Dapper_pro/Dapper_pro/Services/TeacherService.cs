using Dapper;
using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Dapper_pro.Utility;
using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
//using static Dapper_pro.Models.AddTeacherMarks;


namespace Dapper_pro.Services
{
    public class TeacherService:ITeacher
    {
        private readonly DBGateway _DBGateway;
        public TeacherService(string connection)
        {
            _DBGateway = new DBGateway(connection);
        }


        public async Task<ResultModel<object>> GetAllTeacher(int pageIndex, int pageSize, string searchValue)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@take", pageSize);

                if (pageIndex > 0)
                {
                    par.Add("@skip", (pageIndex - 1) * pageSize);
                }
                else
                {
                    par.Add("@skip", 0);
                }

                par.Add("@searchValue", searchValue);

                var result = await _DBGateway.ExeQueryList<dynamic>("sp_teacher", par);

                if (result != null && result.Count > 0)
                {
                    Result.TotalRecords = Convert.ToInt32(result[0].TotalRecords);
                }
                else
                {
                    Result.TotalRecords = 0;
                }

                Result.LstModel = result;
            }
            catch (Exception ex)
            {
                Result.Message = $"Error: {ex.Message}";
            }
            return Result;
        }



        public async Task<ResultModel<object>> Insert(TeacherInter teacher)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                //par.Add("@id", teacher.id);
                par.Add("@username", teacher.username);
                par.Add("@firstname", teacher.firstname);
                par.Add("@lastname", teacher.lastname);
                par.Add("@email", teacher.email);
                par.Add("@districtname", teacher.districtname);
                par.Add("@districtcode", teacher.districtcode);
                par.Add("@statename", teacher.statename);
                par.Add("@statecode", teacher.statecode);
                par.Add("@mobile", teacher.mobile);
                par.Add("@gender", teacher.gender);

                var insertQuery = @"INSERT INTO md_teacher (username,firstname,lastname, email, districtname, districtcode, statename, statecode, mobile, gender) 
                            VALUES (@username,@firstname,@lastname, @email, @districtname, @districtcode, @statename, @statecode, @mobile, @gender);
                            SELECT LAST_INSERT_ID();";

                var insertedId = await _DBGateway.ExeScalarQuery<int>(insertQuery, par);

                if (insertedId > 0)
                {
                    result.Message = "Teacher Record Inserted Successfully";
                    result.Model = insertedId;
                }
                else
                {
                    result.Message = "Failed to Insert Teacher Record";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while inserting the record: {ex.Message}";
            }
            return result;
        }


        public async Task<ResultModel<object>> Update(TeacherInter teacher)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@id", teacher.id);
                par.Add("@username", teacher.username);
                par.Add("@firstname", teacher.firstname);
                par.Add("@lastname", teacher.lastname);
                par.Add("@email", teacher.email);
                par.Add("@districtname", teacher.districtname);
                par.Add("@districtcode", teacher.districtcode);
                par.Add("@statename", teacher.statename);
                par.Add("@statecode", teacher.statecode);
                par.Add("@mobile", teacher.mobile);
                par.Add("@gender", teacher.gender);

                var existingRecord = await _DBGateway.ExeScalarQuery<object>("SELECT 1 FROM md_teacher WHERE id = @id", par);

                if (existingRecord != null)
                {
                    var updateResult = await _DBGateway.ExeScalarQuery<int>("UPDATE md_teacher SET username = @username,firstname=@firstname, lastname=@lastname, email = @email, " +
                                                                            "districtname = @districtname, districtcode=@districtcode, statename=@statename, statecode=@statecode, mobile = @mobile, gender = @gender " +
                                                                            "WHERE id = @id;" +
                                                                            "SELECT 1", par);
                    result.Message = "Teacher Record Updated Successfully";
                    result.Model = null;
                }
                else
                {
                    result.Message = "Teacher Record not Found";
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

                var existingRecord = await _DBGateway.ExeScalarQuery<object>("SELECT Id FROM md_teacher WHERE id = @id", par);

                if (existingRecord != null)
                {
                    var deleteResult = await _DBGateway.ExeQuery("DELETE FROM md_teacher WHERE id = @id", par) == 1 ? 1 : 2;
                    result.Message = "Teacher Record Deleted Successfully";
                    result.Model = null;
                }
                else
                {
                    result.Message = "Teacher Record not Found";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error while Deleting the record {ex.Message}";
            }
            return result;
        }




        /// <summary>
        /// //Teacher Attendance
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        public async Task<ResultModel<object>> AddTeacherAttendance(AddTeacherAttendance addteacherattendance)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@Id", addteacherattendance.Id);
                par.Add("@Date", addteacherattendance.Date);
                par.Add("@Status", addteacherattendance.Status);

                par.Add("@Type", Constants.teacherrole);

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


        public async Task<TeacherAttendanceSummary> GetTeacherAttendanceSummary(DateTime date)
        {
            var summary = new TeacherAttendanceSummary();
            try
            {
                var attendanceQuery = @"SELECT t.id AS TeacherId,t.username AS Username,t.departmentid AS DepartmentId,t.type AS TeacherType,c.name AS DepartmentName,
                        a.status AS Status FROM md_teacher t 
                    INNER JOIN 
                        md_course c ON t.departmentid = c.id
                    INNER JOIN
                        mp_attendance a ON t.id = a.id AND t.type = a.type
                    WHERE 
                        CAST(a.date AS DATE) = @Date";

                var par = new DynamicParameters();
                par.Add("@Date", date.Date, DbType.Date);

                var attendanceStatuses = await _DBGateway.ExeQueryList<TeacherAttendanceStatus>(attendanceQuery, par);

                summary.AttendanceTeacherStatuses = attendanceStatuses.ToList();
                summary.TotalPresent = attendanceStatuses.Count(a => a.Status == "Present");
                summary.TotalAbsent = attendanceStatuses.Count(a => a.Status == "Absent");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            return summary;
        }


        public async Task<List<TeacherAttendanceResult>> GetTeacherAttendanceByUsernameAsync(string username)
        {
            var query = @"SELECT a.Date, a.Status FROM md_teacher s JOIN mp_attendance a ON s.Id = a.Id AND s.Type = a.Type WHERE s.Username = @Username";

            var parameters = new DynamicParameters();
            parameters.Add("@Username", username);

            var results = await _DBGateway.ExeQueryList<TeacherAttendanceResult>(query, parameters);
            return results;
        }


        /// <summary>
        /// //Subject Related work
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<IEnumerable<TeacherCourseSubjectDto>> GetTeacherCourseSubjectDataAsync()
        {
            string query = @"SELECT t.firstname, t.lastname, c.name AS name, s.subjectname AS subjectname FROM md_teacher t INNER JOIN md_course c ON t.departmentid = c.id
            INNER JOIN mp_teacher_subject ts ON t.id = ts.teacherid
            INNER JOIN md_subject s ON ts.subjectid = s.subjectid";

            var result = await _DBGateway.ExeQueryList<TeacherCourseSubjectDto>(query);
            return result.ToList();
        }



        public async Task<bool> AssignSubjectToTeacher(TeacherSubjectAssignmentDto dto)
        {
            using (var connection = _DBGateway.Connection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var teacherId = await connection.ExecuteScalarAsync<int>(
                            "SELECT id FROM md_teacher WHERE firstname = @Firstname",
                            new { dto.Firstname }, transaction);

                        var subjectId = await connection.ExecuteScalarAsync<int>(
                            "SELECT subjectid FROM md_subject WHERE subjectname = @Subjectname",
                            new { dto.Subjectname }, transaction);

                        var result = await connection.ExecuteAsync(
                            "UPDATE mp_teacher_subject SET assignment = @Assignment WHERE teacherid = @TeacherId AND subjectid = @SubjectId",
                            new { dto.Assignment, TeacherId = teacherId, SubjectId = subjectId }, transaction);

                        transaction.Commit();
                        return result > 0;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }




        public async Task<List<TeacherSubjectAssignment>> GetTeacherSubjectAssignments()
        {
            string query = @"SELECT 
                    t.id AS teacherid,
                    t.firstname AS firstname,
                    t.lastname AS lastname,
                    s.subjectid AS subjectid,
                    s.subjectname AS subjectname,
                    d.name AS departmentname, 
                    cs.semester AS semester,
                    ts.assignment AS assignment
                FROM 
                    mp_teacher_subject ts
                JOIN 
                    md_teacher t ON ts.teacherid = t.id
                JOIN 
                    md_subject s ON ts.subjectid = s.subjectid
                JOIN
                    md_course d ON t.departmentid = d.id
                JOIN
                    mp_course_subject cs ON ts.subjectid = cs.subjectid";

            var assignments = await _DBGateway.ExeQueryList<TeacherSubjectAssignment>(query);
            return assignments.ToList();
        }




        public async Task UpdateStudentMarks(StudentMarksSubject studentMarks)
        {
            string studentQuery = @"SELECT id FROM md_students WHERE firstname = @firstname";

            string departmentQuery = @"SELECT Id FROM md_course WHERE name = @departmentname";

            string subjectQuery = @"SELECT subjectid FROM md_subject WHERE subjectname = @subjectname";

            string updateMarksQuery = @"UPDATE mp_studentmarks_subject SET marks = @marks, A1marks = @A1marks, A2marks = @A2marks, A3marks = @A3marks WHERE studentid = @studentid AND subjectid = @subjectid AND departmentid = @departmentid";

            string insertMarksQuery = @"INSERT INTO mp_studentmarks_subject (studentid, subjectid, departmentid, marks, A1marks, A2marks, A3marks) VALUES (@studentid, @subjectid, @departmentid, @marks, @A1marks, @A2marks, @A3marks)";

            var parameters = new DynamicParameters();
            parameters.Add("firstname", studentMarks.firstname);
            parameters.Add("departmentname", studentMarks.departmentname);
            parameters.Add("subjectname", studentMarks.subjectname);

            using (var connection = _DBGateway.Connection())
            {
                connection.Open();

                int studentid = await connection.QueryFirstOrDefaultAsync<int>(studentQuery, parameters);

                int departmentid = await connection.QueryFirstOrDefaultAsync<int>(departmentQuery, parameters);

                int subjectid = await connection.QueryFirstOrDefaultAsync<int>(subjectQuery, parameters);

                if (studentid == 0 || departmentid == 0 || subjectid == 0)
                {
                    throw new Exception("Invalid student, department, or subject details.");
                }

                parameters.Add("studentid", studentid);
                parameters.Add("departmentid", departmentid);
                parameters.Add("subjectid", subjectid);
                parameters.Add("marks", studentMarks.marks);
                parameters.Add("A1marks", studentMarks.A1marks);
                parameters.Add("A2marks", studentMarks.A2marks);
                parameters.Add("A3marks", studentMarks.A3marks);

                int rowsAffected = await connection.ExecuteAsync(updateMarksQuery, parameters);

                if (rowsAffected == 0)
                {
                    await connection.ExecuteAsync(insertMarksQuery, parameters);
                }
            }
        }





    }
}
