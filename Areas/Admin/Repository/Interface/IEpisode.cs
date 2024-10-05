using eproject.Areas.Admin.Models;
using eproject.Areas.Admin.Repository.Interface;
using eproject.Data;
using eproject.Repository;

public interface IEpisode : IRepository<Episode>
{
    Task AddAsync(Episode episode);
    Task<IEnumerable<Episode>> GetEpisodesByAnimeIdAsync(int animeId);
    // Add any Anime-specific methods here
}

