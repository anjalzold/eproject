using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using eproject.Models;
using eproject.Areas.Admin.Repository.Interface;
using eproject.Areas.Admin.Models;
using eproject.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace eproject.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class HomeController : Controller
{
    private readonly IAnimeRepository _animeRepository;
    private readonly AppDbContext _context;
    private readonly IEpisode _episodeRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IAnimeRepository animeRepository, AppDbContext context, IEpisode episodeRepository, ILogger<HomeController> logger)
    {
        _animeRepository = animeRepository;
        _context = context;
        _episodeRepository = episodeRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var thirtyMinutesAgo = DateTime.Now.AddMinutes(-30);
        var viewModel = new DashboardViewModel
        {
            TotalAnime = await _context.Animes.CountAsync(),
            TotalUsers = await _context.Users.CountAsync(),
            RecentlyAddedAnime = await _context.Animes.OrderByDescending(a => a.Id).Take(5).CountAsync(),
            ActiveUsers = await _context.Users.CountAsync(u => u.LastActivityDate >= thirtyMinutesAgo),
                        AnimePopularity = await _context.Animes
                .OrderByDescending(a => (a.UserTrackings.Count * 0.3m) + (a.UserTrackings.Sum(ut => ut.UserRating) * 0.7m))
                .Take(5)
                .Select(a => new AnimePopularityDto
                {
                    Title = a.Title,
                    Popularity = (a.UserTrackings.Count * 0.3m) + (a.UserTrackings.Sum(ut => ut.UserRating) * 0.7m)
                })
                .ToListAsync(),
            RecentUpdates = await _context.Animes
                .OrderByDescending(a => a.UpdatedAt)
                .Take(5)
                .Select(a => new RecentUpdateDto { Title = a.Title, UpdateType = "Updated", Date = a.UpdatedAt })
                .ToListAsync()
        };

        return View(viewModel);
    }
    public async Task<IActionResult> ListAnime()
    {
        var animes = await _animeRepository.GetAllAsync();
        return View(animes);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Genres = _context.Genres.ToList();
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Anime anime, List<int>? GenreIds, IFormFile? coverImage)
    {

        
        if (ModelState.IsValid)
        {
            try
            {
                if (coverImage != null && coverImage.Length > 0)
                {
                    var fileName = Path.GetFileName(coverImage.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "animes");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(fileStream);
                    }

                    anime.CoverImagePath = "/images/animes/" + uniqueFileName;
                }
                else
                {
                    anime.CoverImagePath = "/images/animes/default.jpg";
                }

                anime.CreatedAt = DateTime.Now;
                anime.UpdatedAt = DateTime.Now;
            


                if (GenreIds != null && GenreIds.Any())
                {
                    anime.AnimeGenres = GenreIds.Select(genreId => new AnimeGenre { GenreId = genreId }).ToList();
                }

                await _animeRepository.AddAsync(anime);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListAnime));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the anime");
                ModelState.AddModelError("", "An error occurred while creating the anime. Please try again.");
            }
        }

        // If we got this far, something failed; redisplay form
        ViewBag.Genres = await _context.Genres.ToListAsync();
        return View(anime);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var anime = await _animeRepository.GetByIdAsync(id);
        if (anime == null)
        {
            return NotFound();
        }

        ViewBag.Genres = await _context.Genres.ToListAsync();
        return View(anime);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,TotalEpisodes,Synopsis,CoverImagePath,AnimeStatus,Studio")] Anime animeUpdate, List<int> GenreIds, List<int> CurrentGenreIds, IFormFile? coverImage)    {
        if (id != animeUpdate.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var anime = await _animeRepository.GetByIdAsync(id);
                if (anime == null)
                {
                    return NotFound();
                }

                // Update the properties
                anime.Title = animeUpdate.Title;
                anime.ReleaseDate = animeUpdate.ReleaseDate;
                anime.TotalEpisodes = animeUpdate.TotalEpisodes;
                anime.Synopsis = animeUpdate.Synopsis;
                anime.UpdatedAt = DateTime.Now;
                anime.AnimeStatus = animeUpdate.AnimeStatus;
                anime.Studio = animeUpdate.Studio;


                if (coverImage != null && coverImage.Length > 0)
                {
                    var fileName = Path.GetFileName(coverImage.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "animes");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    Directory.CreateDirectory(uploadsFolder);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await coverImage.CopyToAsync(fileStream);
                    }

                    // Delete the old image if it exists and is not the default image
                    if (!string.IsNullOrEmpty(anime.CoverImagePath) && anime.CoverImagePath != "/images/animes/goku.jpg")
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", anime.CoverImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    anime.CoverImagePath = "/images/animes/" + uniqueFileName;
                }


                // Update AnimeGenres


                if (!GenreIds.SequenceEqual(CurrentGenreIds))
                {
                    var existingAnimeGenres = await _context.AnimeGenres.Where(ag => ag.AnimeId == anime.Id).ToListAsync();
                    _context.AnimeGenres.RemoveRange(existingAnimeGenres);

                    foreach (var genreId in GenreIds)
                    {
                        anime.AnimeGenres.Add(new AnimeGenre { GenreId = genreId });
                    }
                }

                anime.UpdatedAt = DateTime.Now;

                await _animeRepository.UpdateAsync(anime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the anime");
                ModelState.AddModelError("", "An error occurred while updating the anime. Please try again.");
            }
        }

        ViewBag.Genres = await _context.Genres.ToListAsync();
        return View(animeUpdate);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var anime = await _animeRepository.GetByIdAsync(id);
        if (anime == null)
        {
            return NotFound();
        }

        try
        {
            await _animeRepository.RemoveAsync(anime);
            anime.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the anime");
            return Json(new { success = false, message = "An error occurred while deleting the anime. Please try again." });
        }
    }

    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     var anime = await _animeRepository.GetByIdAsync(id);
    //     if (anime == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     try
    //     {
    //         await _animeRepository.RemoveAsync(anime);
    //         return RedirectToAction(nameof(Index));
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError(ex, "An error occurred while deleting the anime");
    //         ModelState.AddModelError("", "An error occurred while deleting the anime. Please try again.");
    //         return View(anime);
    //     }
    // }
    public IActionResult EpisodeList(int id)
    {
        var anime = _animeRepository.GetById(id);
        if (anime == null)
        {
            return NotFound();
        }

        var episodes = _episodeRepository.Find(e => e.AnimeId == id);
        ViewBag.AnimeId = id;
        ViewBag.AnimeTitle = anime.Title;
        return View(episodes);
    }


    public IActionResult UploadEpisode(int id)
    {
        var anime = _animeRepository.GetById(id);
        if (anime == null)
        {
            return NotFound();
        }

        ViewBag.AnimeId = id;
        ViewBag.AnimeTitle = anime.Title;
        return View();
    }






    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadEpisode(int id, Episode episode, IFormFile videoFile)
    {
        if (!ModelState.IsValid)
        {
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    _logger.LogError($"Model Error: {error.ErrorMessage}");
                }
            }
            ViewBag.AnimeId = id;
            ViewBag.AnimeTitle = _animeRepository.GetById(id)?.Title;
            return View(episode);
        }

        try
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(videoFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                var filePath = Path.Combine(uploadsFolder, fileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                }

                episode.VideoPath = "/videos/" + fileName;
            }
            else
            {
                ModelState.AddModelError("", "Please upload a video file.");
                ViewBag.AnimeId = id;
                ViewBag.AnimeTitle = _animeRepository.GetById(id)?.Title;
                return View(episode);
            }

            episode.AnimeId = id;
            episode.UploadDate = DateTime.Now;

            // Ensure that the Id is not set
            episode.Id = 0;

            await _episodeRepository.AddAsync(episode);
            await _context.SaveChangesAsync();

            return RedirectToAction("EpisodeList", new { id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading episode");
            ModelState.AddModelError("", "An error occurred while uploading the episode. Please try again.");
            ViewBag.AnimeId = id;
            ViewBag.AnimeTitle = _animeRepository.GetById(id)?.Title;
            return View(episode);
        }
    }


    public async Task<IActionResult> EditEpisode(int id)
    {
        var episode = await _episodeRepository.FirstOrDefaultAsync(e => e.Id == id);
        if (episode == null)
        {
            return NotFound();
        }
        return View(episode);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEpisode(Episode episode, IFormFile? videoFile)
    {
        if (!ModelState.IsValid)
        {
            return View(episode);
        }

        try
        {
            // Handle new video file upload
            if (videoFile != null && videoFile.Length > 0)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(videoFile.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos");
                var filePath = Path.Combine(uploadsFolder, fileName);

                Directory.CreateDirectory(uploadsFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(fileStream);
                }

                // Delete the old video file if it exists
                if (!string.IsNullOrEmpty(episode.VideoPath))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", episode.VideoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Update the VideoPath with the new file path
                episode.VideoPath = "/videos/" + fileName;
            }

            episode.UploadDate = DateTime.Now;

            _episodeRepository.Update(episode);
            await _episodeRepository.SaveChangesAsync();

            return RedirectToAction("EpisodeList", new { id = episode.AnimeId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error editing episode");
            ModelState.AddModelError("", "An error occurred while editing the episode. Please try again.");
            return View(episode);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEpisode(int id)
    {
        var episode = await _episodeRepository.FirstOrDefaultAsync(e => e.Id == id);
        if (episode == null)
        {
            return Json(new { success = false, message = "Episode not found." });
        }

        _episodeRepository.Remove(episode);
        await _episodeRepository.SaveChangesAsync();
        return Json(new { success = true });
    }

}