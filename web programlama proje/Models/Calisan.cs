using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Calisan
    {
        [Key]
        public int CalisanId { get; set; }
        public string CalisanAd {  get; set; }
        public string CalisanEmail { get; set; }
        public List<CalisanHizmet> Hizmetler { get; set; } = new List<CalisanHizmet>();
        public List<Randevu> Randevular { get; set; } = new List<Randevu>();

    }
}
