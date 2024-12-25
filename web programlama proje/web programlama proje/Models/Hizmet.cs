namespace web_programlama_proje.Models
{
    public class Hizmet
    {
        public int HizmetId { get; set; }
        public string HizmetAd {  get; set; }
        public int HizmetFiyat  { get; set; }
        public ICollection<Calisan> Calisanlar { get; set; }

    }
}
