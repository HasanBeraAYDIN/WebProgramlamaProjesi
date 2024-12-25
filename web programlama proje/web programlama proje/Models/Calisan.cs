using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }
        public string CalisanAd { get; set; }
       // public string CalisanUzmanlik { get; set; }
        public string UygunSaatler { get; set; }
        public ICollection<Hizmet> Hizmetler { get; set; }
        public List<Randevu> Randevular {  get; set; }

    }
}
