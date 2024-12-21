using Microsoft.EntityFrameworkCore;

namespace wep_programlama_proje.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 
        
        }
        public DbSet<Calisan> Calisanlar => Set<Calisan>();

        public DbSet<Hizmet> Hizmetler => Set<Hizmet>();

        public DbSet<Musteri> Musteriler => Set<Musteri>();
    }
}
