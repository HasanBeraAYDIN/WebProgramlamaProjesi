using Microsoft.EntityFrameworkCore;

namespace web_programlama_proje.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :base(options){ }
        public DbSet<Kullanici> Kullanicilar => Set<Kullanici>();
    }
}
