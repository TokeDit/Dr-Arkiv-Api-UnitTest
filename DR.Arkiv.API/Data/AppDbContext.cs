using DR.Arkiv.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DR.Arkiv.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<MusicRecord> MusicRecords => Set<MusicRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MusicRecord>().HasData(
            new MusicRecord { Id = 1, Title = "Bohemian Rhapsody", Artist = "Queen", Duration = 354, PublicationYear = 1975 },
            new MusicRecord { Id = 2, Title = "Hotel California", Artist = "Eagles", Duration = 391, PublicationYear = 1977 },
            new MusicRecord { Id = 3, Title = "Stairway to Heaven", Artist = "Led Zeppelin", Duration = 482, PublicationYear = 1971 },
            new MusicRecord { Id = 4, Title = "Smells Like Teen Spirit", Artist = "Nirvana", Duration = 301, PublicationYear = 1991 },
            new MusicRecord { Id = 5, Title = "Billie Jean", Artist = "Michael Jackson", Duration = 294, PublicationYear = 1983 }
        );
    }
}
