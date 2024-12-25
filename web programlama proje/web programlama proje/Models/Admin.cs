using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Admin
    {
        [Key]

        public int AdminId { get; set; }

        public string AdminAd { get; set; }
        public string AdminPassword { get; set; }


    }
}
