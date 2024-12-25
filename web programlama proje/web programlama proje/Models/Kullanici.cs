using System.ComponentModel.DataAnnotations;

namespace web_programlama_proje.Models
{
    public class Kullanici
    {
        [Key]


        public int KullaniciId {  get; set; }

        [Required(ErrorMessage = "Ad ve soyad zorunludur.")]
        [Display(Name = "Ad ve Soyad")]
        public string KullaniciAd { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta Adresi")]
        public string KullaniciEmail { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifreyi onaylamanız gerekiyor.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifreyi Onayla")]
        public required string SifreOnay { get; set; }

        public List<Randevu> RandevuListesi { get; set; }


    }
}
