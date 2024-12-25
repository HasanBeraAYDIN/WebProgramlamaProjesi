using Microsoft.EntityFrameworkCore;

namespace web_programlama_proje.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options){ }
        public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
        public DbSet<Admin> Adminler => Set<Admin>();
        public DbSet<Calisan> Calisanlar => Set<Calisan>();
        public DbSet<Hizmet> Hizmetler => Set<Hizmet>();
        public DbSet<Randevu> Randevular => Set<Randevu>();
    }
}
