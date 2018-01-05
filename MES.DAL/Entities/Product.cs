using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public  class Product : IdProvider
    {
        public string Name { get; set; }
        

        public virtual ICollection<StructureOfTheProduct> StructureOfTheProducts { get; set; } = new HashSet<StructureOfTheProduct>();
        public virtual ICollection<Soldering> Solderings { get; set; } = new HashSet<Soldering>();
    }
}