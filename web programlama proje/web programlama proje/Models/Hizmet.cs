using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Hizmet
    {
        [Key]
        public int HizmetId { get; set; }
        public string HizmetAd { get; set; }
        public int HimzetUcret {  get; set; }
        public List<CalisanHizmet> Calisanlar { get; set; } = new List<CalisanHizmet>();

    }
}
