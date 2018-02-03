using System;
using System.ComponentModel.DataAnnotations;

namespace MES.WEB.Models
{
    public class DateVm
    {
        [Required]
        [Display(Name = "От")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "До")]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}