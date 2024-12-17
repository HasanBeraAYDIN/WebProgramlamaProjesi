using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Data
{
    public class Calisan
    {
        [Key]
        public int Id { get; set; }
        public required string CalisanAd {  get; set; }
        public int Maas { get; set; }    
        public string Uzmanlik {  get; set; }
    }
}
