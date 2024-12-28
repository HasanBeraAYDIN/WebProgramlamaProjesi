using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class CalisanHizmet
    {
        [Key]
        public int Id { get; set; }
        public int CalisanId { get; set; }
        public int HizmetId { get; set; }
        public Calisan Calisan { get; set; }
        public Hizmet Hizmet { get; set; }
    }
}
