using System.Collections.Generic;

namespace MES.DAL.Entities
{
    //Комплектующие
    public sealed class Detail : IdProvider
    {
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public int Quantity { get; set; }
        public int GroupProductId { get; set; }
        public GroupProduct GroupProduct { get; set; }
        

        public ICollection<StructureOfTheProduct> StructureOfTheProducts { get; set; }
        public ICollection<ArrivalOfDetail> ArrivalOfDetailses { get; set; }
        

        public Detail()
        {
            StructureOfTheProducts = new List<StructureOfTheProduct>();
            ArrivalOfDetailses = new List<ArrivalOfDetail>();
        }
    }
}
