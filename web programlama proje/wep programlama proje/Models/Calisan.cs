namespace wep_programlama_proje.Models
{
    public class Calisan
    {
        public int CalisanId { get; set; }
        public string CalisanAd { get; set; }
        public string CalisanUzmanlik {  get; set; }
        public List<Randevu> Randevular { get; set; }


    }
}
