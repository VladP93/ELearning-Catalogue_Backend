using System;
using System.Collections.Generic;

namespace Application.DTO
{
    public class CourseDTO
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; } 
        public string CourseDescription { get; set; }
        public DateTime? PublicationDate { get; set; }
        public byte[] CoverPhoto { get; set; }
        public ICollection<InstructorDTO> Instructors { get; set; }
        public PriceDTO Price { get; set; }
        public ICollection<CommentaryDTO> Commentaries { get; set; }
    }
}