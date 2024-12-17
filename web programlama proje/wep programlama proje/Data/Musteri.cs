using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Data
{
    public class Musteri
    {
       [Key]
        public int MusteriID {  get; set; }

        public string MusteriName { get; set; }

        
    }
}
