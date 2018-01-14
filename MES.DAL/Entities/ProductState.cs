using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MES.DAL.Enums;


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
        public VariantStateProduct StateProduct { get; set; }
        
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        
    }
}