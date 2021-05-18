using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DapperConnection.Instructor
{
    public interface IInstructor
    {
         Task<IEnumerable<InstructorModel>> GetList();
         Task<InstructorModel> GetById(Guid instructorId);

         Task<int> Create(string firstName, string lastName, string degree);
         Task<int> Update(Guid instructorId, string firstName, string lastName, string degree);
         Task<int> Delete(Guid id);
    }
}