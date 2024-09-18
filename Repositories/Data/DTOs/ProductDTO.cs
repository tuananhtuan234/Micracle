using Repositories.Data.Entity;

namespace Micracle.DTOs
{
    public class ProductDTO
    {      
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string SubCategoryId { get; set; }
    }
}
