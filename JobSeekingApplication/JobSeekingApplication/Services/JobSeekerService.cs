using Dapper;
using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using JobSeekingApplication.utility;

namespace JobSeekingApplication.Services
{
    public class JobSeekerService : Ijobseeker
    {
        private readonly DBGateway _DBGateway;
        public JobSeekerService(DBGateway dBGateway)
        {
            _DBGateway = dBGateway;
        }


        /// <summary>
        /// JOBSEEKER Process
        /// </summary>
        /// <param JobSeeker Process="id"></param>
        /// <returns></returns>
        
        public async Task<ResultModel<Object>> GetJobSeekerAsync(int id)
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = @"select pl.firstname,pl.lastname,pl.username,pl.email,js.mobilenumber,js.image,js.address from mp_login pl join md_jobseeker js on pl.id = js.userid where pl.id = @id";
                var par = new DynamicParameters();
                par.Add("@id", id);

                var jobseekerdata = await _DBGateway.ExeQueryList<Jobseeker>(query, par);
                if (jobseekerdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = jobseekerdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data received failed";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<Object>> AppliedJobSeeker(int id, jsapplier jobseekerapplied)
        {
            var result = new ResultModel<Object>();
            try
            {
                var queryJseekerId = @"SELECT id FROM mp_login WHERE firstname = @firstname AND lastname = @lastname AND email = @email";
                var paramJseeker = new DynamicParameters();
                paramJseeker.Add("@firstname", jobseekerapplied.firstname);
                paramJseeker.Add("@lastname", jobseekerapplied.lastname);
                paramJseeker.Add("@email", jobseekerapplied.email);

                var jseekerId = await _DBGateway.ExecuteScalarQueryAsync<int?>(queryJseekerId, paramJseeker);
                if (jseekerId == null)
                {
                    result.Success = false;
                    result.Message = "Job seeker not found";
                    return result;
                }

                var queryPostId = @"SELECT id FROM md_postjob WHERE role = @role";
                var paramPostId = new DynamicParameters();
                paramPostId.Add("@role", jobseekerapplied.role);

                var postId = await _DBGateway.ExecuteScalarQueryAsync<int?>(queryPostId, paramPostId);
                if (postId == null)
                {
                    result.Success = false;
                    result.Message = "Post not found";
                    return result;
                }

                var queryStateCode = @"SELECT StateCode FROM md_state_district WHERE StateName = @StateName";
                var paramState = new DynamicParameters();
                paramState.Add("@StateName", jobseekerapplied.StateName);

                var stateCode = await _DBGateway.ExecuteScalarQueryAsync<int?>(queryStateCode, paramState);
                if (stateCode == null)
                {
                    result.Success = false;
                    result.Message = "State not found";
                    return result;
                }

                var queryDistrictCode = @"SELECT DistrictCode FROM md_state_district WHERE StateName = @StateName AND DistrictName = @DistrictName";
                var paramDistrict = new DynamicParameters();
                paramDistrict.Add("@StateName", jobseekerapplied.StateName);
                paramDistrict.Add("@DistrictName", jobseekerapplied.DistrictName);

                var districtCode = await _DBGateway.ExecuteScalarQueryAsync<int?>(queryDistrictCode, paramDistrict);
                if (districtCode == null)
                {
                    result.Success = false;
                    result.Message = "District not found";
                    return result;
                }

                var queryInsert = @"INSERT INTO mp_jsapplication (jseekerid, postid, mobilenumber, resume, StateCode, DistrictCode, appliedby) 
                            VALUES (@jseekerid, @postid, @mobilenumber, @resume, @StateCode, @DistrictCode, NOW())";
                var paramInsert = new DynamicParameters();
                paramInsert.Add("@jseekerid", jseekerId);
                paramInsert.Add("@postid", postId);
                paramInsert.Add("@mobilenumber", jobseekerapplied.mobilenumber);
                paramInsert.Add("@resume", jobseekerapplied.resume);
                paramInsert.Add("@StateCode", stateCode);
                paramInsert.Add("@DistrictCode", districtCode);

                var rowsAffected = await _DBGateway.ExeQuery(queryInsert, paramInsert);
                if (rowsAffected > 0)
                {
                    result.Success = true;
                    result.Message = "Job application submitted successfully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Failed to submit job application";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }



        public async Task<ResultModel<Object>> GelAllJobData()
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = @"select * from md_postjob";
                var jobdata = await _DBGateway.ExeQueryList<Jobdata>(query);
                if (jobdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = jobdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data received failed";
                }
            }catch(Exception ex) 
            {
                result.Success=false;
                result.Message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// JOBSEEKER viewapplieddata
        /// </summary>
        /// <param JobSeeker viewapplieddata="id"></param>
        /// <returns></returns>
       
        public async Task<ResultModel<Object>> GelAllapplieddata(int userid)
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = @"SELECT p.role,p.company_name,p.location,p.package,p.mode,p.skill,p.noofhiring,p.image,p.description, a.appliedby FROM md_postjob p JOIN mp_jsapplication a ON p.id = a.postid
                WHERE a.jseekerid = @userid;";

                var parapplieddata = new DynamicParameters();
                parapplieddata.Add("@userid", userid);

                var exitdata = await _DBGateway.ExeQueryList<viewapplieddata>(query, parapplieddata);
                if (exitdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = exitdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data received Failed";
                }
            }
            catch(Exception ex)
            {
                result.Success=false;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
