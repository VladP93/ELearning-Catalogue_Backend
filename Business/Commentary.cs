namespace Business
{
    public class Commentary
    {
        public int CommentaryId { get; set; }
        public string Student { get; set; }
        public int Score { get; set; }
        public string CommentaryText { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}