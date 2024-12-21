using System.ComponentModel.DataAnnotations;

namespace wep_programlama_proje.Models
{
    public class Kayit
    {
        [Required(ErrorMessage = "Ad ve soyad zorunludur.")]
        [Display(Name = "Ad ve Soyad")]
        public string MusteriAd { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        [Display(Name = "E-posta Adresi")]
        public string MusteriEmail { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifreyi onaylamanız gerekiyor.")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Şifreyi Onayla")]
        public string SifreOnay { get; set; }
    }
}
