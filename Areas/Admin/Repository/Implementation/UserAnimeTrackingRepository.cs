using eproject.Areas.Admin.Models;
using eproject.Data;
using eproject.Repository;

public class UserAnimeTrackingRepository : Repository<UserAnimeTracking>, IUserAnimeTracking
{
    public UserAnimeTrackingRepository(AppDbContext context) : base(context)
    {
    }

    // Implement any Anime-specific methods here
}