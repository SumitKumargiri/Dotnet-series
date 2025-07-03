using JobSeekingApplication.Model;

namespace JobSeekingApplication.Interface
{
    public interface Ijobseeker
    {
        Task<ResultModel<Object>>GetJobSeekerAsync(int id);
        Task<ResultModel<Object>> AppliedJobSeeker(int id, jsapplier jobseekerapplied);
        Task<ResultModel<Object>> GelAllJobData();
        Task<ResultModel<Object>> GelAllapplieddata(int userid);
    }
}
