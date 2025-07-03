using Dapper_pro.Models;

namespace Dapper_pro.Interfaces
{
    public interface IRepoBase
    {
        List<Student> GetAll();
        Student GetById(int id);
        Task<int> AddStudent(Student _student);
        Task<int> UpdateStudent(Student _student);
        Task<int> DeleteStudent(int id);
    }
}

