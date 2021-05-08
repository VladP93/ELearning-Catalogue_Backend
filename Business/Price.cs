namespace Business
{
    public class Price
    {
        public int PriceId { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}