using System.Collections.Generic;

namespace MES.DAL.Entities
{
    public class GroupProduct : IdProvider
    {
        public string Name { get; set; }

        public ICollection<Detail> Details { get; set; }

        public GroupProduct()
        {
            Details = new List<Detail>();
        }
    }
}
