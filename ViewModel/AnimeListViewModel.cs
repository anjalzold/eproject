namespace eproject.ViewModels;

public class AnimeListViewModel
{
    public List<string> Genres { get; set; }
    public List<AnimeViewModel> Animes { get; set; }
}

public class AnimeViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CoverImagePath { get; set; }
    public List<string> Genres { get; set; }
    public int TotalEpisodes { get; set; }
    public double AverageRating { get; set; }
}