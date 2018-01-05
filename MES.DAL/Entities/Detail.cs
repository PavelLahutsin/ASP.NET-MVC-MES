using System.Collections.Generic;

namespace MES.DAL.Entities
{
    //Комплектующие
    public class Detail : IdProvider
    {
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public int Quantityq { get; set; }
        public int GroupProductId { get; set; }
        public virtual GroupProduct GroupProduct { get; set; }
        

        public virtual ICollection<StructureOfTheProduct> StructureOfTheProducts { get; set; } = new HashSet<StructureOfTheProduct>();
        public virtual ICollection<ArrivalOfDetail> ArrivalOfDetailses { get; set; } = new HashSet<ArrivalOfDetail>();
    }
}
