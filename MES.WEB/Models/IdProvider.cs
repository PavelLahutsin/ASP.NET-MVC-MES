using System.Web.Mvc;

namespace MES.WEB.Models
{
    public class IdProvider
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}