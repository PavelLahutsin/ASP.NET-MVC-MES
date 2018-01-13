using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MES.DAL.Entities
{
    //состояние продукта
    public class ProductState : IdProvider
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int VariantStateProductId { get; set; }

        [Range(0, 100000, ErrorMessage = "Min = 0, Max = 1000000")]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual VariantStateProduct VariantStateProduct { get; set; }
    }
}