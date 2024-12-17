using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Data
{
    public class Hizmet
    {
        [Key]
        public int HizmetId { get; set; }

        public string HizmetName { get; set; }

        public int Ucret {  get; set; }

    }
}
