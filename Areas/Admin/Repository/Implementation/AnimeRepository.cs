using eproject.Areas.Admin.Models;
using eproject.Data;
using eproject.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AnimeRepository : Repository<Anime>, IAnimeRepository
{
    private readonly AppDbContext _context;

    public AnimeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Anime>> GetAllAsync()

    {

        return await _context.Animes

            .Include(a => a.AnimeGenres)

                .ThenInclude(ag => ag.Genre)

            .Include(a => a.UserTrackings)

            .ToListAsync();

    }

    public async Task<Anime> GetByIdAsync(int id)
    {
        return await _context.Animes.FindAsync(id);
    }

    public async Task AddAsync(Anime anime)
    {
        await _context.Animes.AddAsync(anime);
    }

    public async Task UpdateAsync(Anime anime)
    {
        _context.Entry(anime).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }


        public async Task RemoveAsync(Anime anime)
    {
        _context.Animes.Remove(anime);
        await Task.CompletedTask;
    }

    // You can add any additional Anime-specific methods here
}