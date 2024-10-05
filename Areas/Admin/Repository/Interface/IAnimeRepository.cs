using eproject.Areas.Admin.Models;
using eproject.Areas.Admin.Repository.Interface;
using eproject.Data;
using eproject.Repository;

public interface IAnimeRepository : IRepository<Anime>
{

    Task<IEnumerable<Anime>> GetAllAsync();

    Task<Anime> GetByIdAsync(int id);

    Task AddAsync(Anime anime);

    Task UpdateAsync(Anime anime);

    Task RemoveAsync(Anime anime);
    // Add any Anime-specific methods here
}

