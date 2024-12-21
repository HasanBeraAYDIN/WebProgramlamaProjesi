using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Data
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }
        public required string CalisanAd {  get; set; }
        public string CalisanUzmanlik {  get; set; }

    }
}
