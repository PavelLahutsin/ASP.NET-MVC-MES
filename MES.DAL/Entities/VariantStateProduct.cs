using System.Collections.Generic;

namespace MES.DAL.Entities
{
    //варианты состояния продукта
    public class VariantStateProduct : IdProvider
    {
        public string Name { get; set; }

        public virtual ICollection<ProductState> ProductStates { get; set; } = new HashSet<ProductState>();

    }
}