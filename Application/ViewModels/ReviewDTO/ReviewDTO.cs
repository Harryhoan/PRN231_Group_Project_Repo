using Domain.Entities;

namespace Application.ViewModels.ReviewDTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int OrderDetailId { get; set; }
        public string KoiName { get; set; }
        public string KoiImage { get; set; }
    }
}
