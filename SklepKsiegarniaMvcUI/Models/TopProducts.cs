using System.ComponentModel.DataAnnotations.Schema;

namespace SklepKsiegarniaMvcUI.Models
{
    [Table("TopProducts")]
    public class TopProducts
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public Book Product { get; set; }
    }
}
