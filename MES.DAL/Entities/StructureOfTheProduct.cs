using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MES.DAL.Entities
{
    //показывает из каких деталей состоит продукт
    public class StructureOfTheProduct : IdProvider
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DetailId { get; set; }
        public int Quantity { get; set; }

        public virtual Detail Detail { get; set; }
        public virtual Product Product { get; set; }
    }
}
