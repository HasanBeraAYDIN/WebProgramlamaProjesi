namespace web_programlama_proje.Models
{
    public class YapayZeka
    {
        public IFormFile Img { get; set; } // Kullanıcının yüklediği dosya
        public string Acıklama { get; set; }  // Kullanıcının tarif girdiği prompt
        public string ApiResimUrl { get; set; } // API'den dönen düzenlenmiş resmin URL'si
    }
}
