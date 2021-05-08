using System;
using System.Collections.Generic;

namespace Business
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; } 
        public string CourseDescription { get; set; }
        public DateTime PublicationDate { get; set; }
        public byte[] CoverPhoto { get; set; }
        public Price Price { get; set; }
        public ICollection<Commentary> Commentaries { get; set; }
        public ICollection<CourseInstructor> CourseInstructors { get; set; }
    }
}