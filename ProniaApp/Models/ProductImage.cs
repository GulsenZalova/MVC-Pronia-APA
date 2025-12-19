namespace ProniaApp.Models
{
    public class ProductImage:BaseEntity
    {
        
        public string Image { get; set; }
        //true falsa null
        public bool? IsPrimary { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }



    }
}