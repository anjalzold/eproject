using eproject.Areas.Admin.Models;
using eproject.Data;
using eproject.Repository;
using Microsoft.EntityFrameworkCore;

public class EpisodeRepository : Repository<Episode>, IEpisode
{
    private readonly AppDbContext _context;
    public EpisodeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task AddAsync(Episode episode)

    {

        await _context.Episodes.AddAsync(episode);

    }

    public async Task<IEnumerable<Episode>> GetEpisodesByAnimeIdAsync(int animeId)

    {

        return await _context.Episodes

            .Where(e => e.AnimeId == animeId)

            .OrderBy(e => e.EpisodeNumber)

            .ToListAsync();

    }

    // Implement any Anime-specific methods here
}