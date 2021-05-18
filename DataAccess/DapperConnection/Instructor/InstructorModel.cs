using System;

namespace DataAccess.DapperConnection.Instructor
{
    public class InstructorModel
    {
        public Guid InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}