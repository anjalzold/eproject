using eproject.Areas.Admin.Models;

namespace eproject.ViewModel;
public class AnimeDetailsViewModel
{
    public Anime Anime { get; set; }
    public UserAnimeTracking? UserTracking { get; set; }
    public List<AnimeComment> Comments { get; set; }

    public decimal AverageRating { get; set; }
    public int NumberOfRatings { get; set; }


    
}