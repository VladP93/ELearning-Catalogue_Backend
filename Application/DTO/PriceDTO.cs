using System;

namespace Application.DTO
{
    public class PriceDTO
    {
        public Guid PriceId { get; set; }
        public decimal ActualPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public Guid CourseId { get; set; }
    }
}