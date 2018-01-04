using System;

namespace MES.DAL.Entities
{
    public class DefectDetail : IdProvider
    {
        public int DetailId { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }

        public virtual Detail Detail { get; set; }
    }
}