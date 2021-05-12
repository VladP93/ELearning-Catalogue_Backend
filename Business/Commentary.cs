using System;

namespace Business
{
    public class Commentary
    {
        public Guid CommentaryId { get; set; }
        public string Student { get; set; }
        public int Score { get; set; }
        public string CommentaryText { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}