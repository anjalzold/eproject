public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();
}