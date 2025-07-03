using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApplication.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobSeekerController : Controller
    {

        private readonly Ijobseeker _jobSeekerService;
        public JobSeekerController(Ijobseeker jobSeekerService)
        {
            _jobSeekerService = jobSeekerService;
        }

        [HttpGet("jobseekerdata/{id}")]
        public async Task<IActionResult> GetJobSeekerdata(int id)
        {
            var result = await _jobSeekerService.GetJobSeekerAsync(id);
            return Ok(result);
        }

        [HttpPost("jobseekerapplieddata/{id}")]
        public async Task <IActionResult> AppliedbyJobSeeker(int id, jsapplier jobseekerapplied)
        {
            var result = await _jobSeekerService.AppliedJobSeeker(id, jobseekerapplied);
            return Ok(result);
        }

        [HttpGet("GetalljobData")]
        public async Task<IActionResult> GelAlldata()
        {
            var result = await _jobSeekerService.GelAllJobData();
            return Ok(result);  
        }

        [HttpGet("viewapplieddata/{userid}")]
        public async Task<IActionResult> GelAllapplied(int userid)
        {
            var result = await _jobSeekerService.GelAllapplieddata(userid);
            return Ok(result);
        }
    }
}
