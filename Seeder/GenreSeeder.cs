using eproject.Data;


namespace eproject.Seeder;


public class GenreSeeder
{
    public static void SeedGenres(AppDbContext context)
    {
        if (!context.Genres.Any())
        {
            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Slice of Life" }
            };

            context.Genres.AddRange(genres);
            context.SaveChanges();
        }
    }
}