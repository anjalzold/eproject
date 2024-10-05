

using eproject.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace eproject.Data;
public class AppDbContext : IdentityDbContext<User>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){
        
    }
    public DbSet<Anime> Animes { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<AnimeGenre> AnimeGenres { get; set; }

    public DbSet<UserAnimeTracking> UserAnimeTrackings { get; set; }

    public DbSet<AnimeComment> AnimeComments { get; set; }


    public int SaveChanges(string userId)
    {
        UpdateLastActivityDate(userId);
        return base.SaveChanges();
    }

    public Task<int> SaveChangesAsync(string userId, CancellationToken cancellationToken = default)
    {
        UpdateLastActivityDate(userId);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateLastActivityDate(string userId)
    {
        if (!string.IsNullOrEmpty(userId))
        {
            var currentUser = Users.FirstOrDefault(u => u.Id == userId);
            if (currentUser != null)
            {
                currentUser.LastActivityDate = DateTime.Now;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnimeGenre>()
            .HasKey(ag => new { ag.AnimeId, ag.GenreId });

        modelBuilder.Entity<AnimeGenre>()
            .HasOne(ag => ag.Anime)
            .WithMany(a => a.AnimeGenres)
            .HasForeignKey(ag => ag.AnimeId);

        modelBuilder.Entity<AnimeGenre>()
            .HasOne(ag => ag.Genre)
            .WithMany(g => g.AnimeGenres)
            .HasForeignKey(ag => ag.GenreId);




        modelBuilder.Entity<Episode>()


     .HasOne(e => e.Anime)

     .WithMany(a => a.Episodes)

     .HasForeignKey(e => e.AnimeId);


        modelBuilder.Entity<Episode>()

            .Property(e => e.Id)

            .UseIdentityColumn();


        modelBuilder.Entity<IdentityUserLogin<string>>(entity =>

        {

            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey
            });

        });


// Configure IdentityUserRole<string>

        modelBuilder.Entity<IdentityUserRole<string>>(entity =>

        {

            entity.HasKey(e => new { e.UserId, e.RoleId });

        });


// Configure IdentityUserToken<string>

        modelBuilder.Entity<IdentityUserToken<string>>(entity =>

        {

            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

        });
    }

    // Configure IdentityUserLogin<string>

   

}