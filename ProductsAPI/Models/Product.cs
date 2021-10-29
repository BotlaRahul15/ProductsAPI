using System.ComponentModel.DataAnnotations;

namespace ProductsAPI
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductFeatures { get; set; }
        public int ProductPrice { get; set; }
        public bool Status { get; set; }
    }
}
