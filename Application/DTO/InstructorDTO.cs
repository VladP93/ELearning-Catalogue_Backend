using System;

namespace Application.DTO
{
    public class InstructorDTO
    {
        public Guid InstructorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public byte[] ProfilePhoto { get; set; }
    }
}