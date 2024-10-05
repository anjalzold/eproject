

namespace eproject.Areas.Admin.Models;

public class UserAnimeTracking

{

    public int Id { get; set; }

    public string UserId { get; set; }
    public int AnimeId { get; set; }

    public int EpisodesWatched { get; set; }

    public decimal UserRating { get; set; }




    public User User { get; set; }

    public AnimeWatchStatus Status { get; set; } // New field

    public Anime Anime { get; set; }

}

public enum AnimeWatchStatus

{

    Watching,

    Completed,

    Planned

}