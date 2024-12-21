namespace wep_programlama_proje.Models
{
    public class Musteri
    {
        public int MusteriId { get; set; }

        public string MusteriAd { get; set; }
        public string MusteriEmail { get; set; }
        public List<Randevu> Randevular { get; set; }


    }
}
