using System;
using System.Collections.Generic;

namespace Business
{
    public class Instructor
    {
        public Guid InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public DateTime? CreatedAt { get; set; }
        public ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}