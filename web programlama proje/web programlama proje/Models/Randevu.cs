using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Randevu
    {
        [Key]
        public int RandevuId { get; set; }
        public int MusteriId { get; set; }
        public Kullanici Musteri { get; set; }

        public int CalisanId { get; set; }
        public Calisan Calisan { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        public DateTime RandevuTarih {  get; set; }
        public string Durum { get; set; }

    }
}
