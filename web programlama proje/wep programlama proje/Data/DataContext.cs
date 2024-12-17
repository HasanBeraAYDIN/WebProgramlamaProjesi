using Microsoft.EntityFrameworkCore;

namespace wep_programlama_proje.Data
{
    public class DataContext :DbContext
    {
        public DbSet<Calisan> Calisanlar { get; set; }

        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }


    }
}
