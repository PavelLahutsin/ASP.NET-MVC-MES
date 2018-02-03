using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MES.DAL.Entities
{
    public class User : IdProvider
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public virtual Role Role { get; set; }
        public int RoleId { get; set; }

        [ScaffoldColumn(false)]
        public byte[] Image { get; set; } // данные изображения

        [ScaffoldColumn(false)]
        public string MimeType { get; set; } // Mime - тип данных изображения

        public virtual ICollection<Assembly> Assemblys { get; set; } = new HashSet<Assembly>();
        public virtual ICollection<ArrivalOfDetail> ArrivalOfDetails { get; set; } = new HashSet<ArrivalOfDetail>();
        public virtual ICollection<Boxing> Boxings { get; set; } = new HashSet<Boxing>();
        public virtual ICollection<CheckJmt> CheckJmts { get; set; } = new HashSet<CheckJmt>();
        public virtual ICollection<DefectDetail> DefectDetails { get; set; } = new HashSet<DefectDetail>();
        public virtual ICollection<Soldering> Solderings { get; set; } = new HashSet<Soldering>();
        public virtual ICollection<Repair> Repairs { get; set; } = new HashSet<Repair>();

    }
}