using System;

namespace Application.DTO
{
    public class CommentaryDTO
    {
        public Guid CommentaryId { get; set; }
        public string Student { get; set; }
        public int Score { get; set; }
        public string CommentaryText { get; set; }
        public Guid CourseId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}