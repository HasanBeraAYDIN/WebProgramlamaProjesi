using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Data
{
    public class Musteri
    {
        [Key]
        public int MusteiId { get; set; }
        public string MusteriAd { get; set; }
        public string MusteriEmail { get; set; }
        public string Password{ get; set; }



    }
}
