using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public sealed class Product : IdProvider
    {
        public string Name { get; set; }
        

        public ICollection<StructureOfTheProduct> StructureOfTheProducts { get; set; }

        public Product()
        {
            StructureOfTheProducts = new List<StructureOfTheProduct>();
        }
    }
}