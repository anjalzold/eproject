using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eproject.Models;
using eproject.Data;
using Microsoft.EntityFrameworkCore;
using eproject.Areas.Admin.Models;
using eproject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using eproject.ViewModels;

namespace eproject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;
    private readonly IAnimeRepository _animeRepository;
    private readonly IUserAnimeTracking _userAnimeTrackingRepository;
    private readonly UserManager<User> _userManager;

    private readonly IEpisode _episodeRepository;

    public HomeController(
        ILogger<HomeController> logger,
        IAnimeRepository animeRepository,
        AppDbContext context,
        IUserAnimeTracking userAnimeTrackingRepository,
        UserManager<User> userManager,
        IEpisode episodeRepository
        
    )
    {
        _logger = logger;
        _animeRepository = animeRepository;
        _context = context;
        _userAnimeTrackingRepository = userAnimeTrackingRepository;
        _userManager = userManager;
        _episodeRepository = episodeRepository;
    }



    public async Task<IActionResult> Index(){

        var animes = await _animeRepository.GetAllAsync();

        return View(animes.ToList());

    }

    // ... other methods ...
    public async Task<IActionResult> Details(int id)
    {
        var anime = await _context.Animes
            .Include(a => a.AnimeGenres)
            .ThenInclude(ag => ag.Genre)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (anime == null)
        {
            return NotFound();
        }

        var userId = GetCurrentUserId();

        var ratings = _context.UserAnimeTrackings
       .Where(ut => ut.AnimeId == id)
       .ToList();
        var averageRating = ratings.Any() ? ratings.Average(r => r.UserRating) : 0;
        var numberOfRatings = ratings.Count;

        var userTracking = await _userAnimeTrackingRepository
            .FirstOrDefaultAsync(ut => ut.AnimeId == id && ut.UserId == userId);

  

   

        var comments = await _context.AnimeComments
            .Include(c => c.User)
            .Where(c => c.AnimeId == id)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();

        var viewModel = new AnimeDetailsViewModel
        {
            Anime = anime,
            UserTracking = userTracking,
            Comments = comments,
            AverageRating = averageRating,
            NumberOfRatings = numberOfRatings,
        };

        return View(viewModel);
    }


    [Authorize]

    [HttpPost]

    public async Task<IActionResult> UpdateTracking(int animeId, int episodesWatched, decimal userRating, AnimeWatchStatus watchStatus)

    {

        var userId = GetCurrentUserId();

        var anime = await _context.Animes.FindAsync(animeId);



        var tracking = await _userAnimeTrackingRepository

            .FirstOrDefaultAsync(ut => ut.AnimeId == animeId && ut.UserId == userId);


        if (tracking == null)

        {

            tracking = new UserAnimeTracking

            {

                AnimeId = animeId,

                UserId = userId

            };

            _userAnimeTrackingRepository.Add(tracking);

        }



        tracking.EpisodesWatched = episodesWatched;

        tracking.UserRating = userRating;

        tracking.Status = episodesWatched >= anime.TotalEpisodes
        ? AnimeWatchStatus.Completed
        : watchStatus;






        await _userAnimeTrackingRepository.SaveChangesAsync();


        return RedirectToAction(nameof(Details), new { id = animeId });

    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddComment(int animeId, string commentText)
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var comment = new AnimeComment
        {
            UserId = userId,
            AnimeId = animeId,
            CommentText = commentText,
            CreatedAt = DateTime.Now
        };

        _context.AnimeComments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = animeId });
    }


    private string GetCurrentUserId()

    {

        return User.FindFirstValue(ClaimTypes.NameIdentifier);

    }

    public async Task<IActionResult> EpisodeList(int id)

    {

        var anime = await _animeRepository.GetByIdAsync(id);

        // Assuming anime.Views is a string that holds an integer value
        if (int.TryParse(anime.Views, out int currentViews))
        {
            // Increment the value
            currentViews += 1;

            // Convert it back to string
            anime.Views = currentViews.ToString();
        }
        else
        {
            // Handle the case where the string could not be parsed
            // For example, you might set a default value or log an error
            anime.Views = "1"; // or any other default value
        }




        await _context.SaveChangesAsync();

        if (anime == null)

        {

            return NotFound();

        }


        var episodes = await _episodeRepository.GetEpisodesByAnimeIdAsync(id);

        var viewModel = new EpisodeListViewModel

        {

            Anime = anime,

            Episodes = episodes

        };


        return View(viewModel);

    }

    [Authorize]
    public async Task<IActionResult> Profile(string id = null, string searchQuery = null)
    {
        User user;
        if (string.IsNullOrEmpty(id))
        {
            // If no id is provided, show the current user's profile
            var userId = GetCurrentUserId();
            user = await _userManager.FindByIdAsync(userId);
        }
        else
        {
            // If an id is provided, show that user's profile
            user = await _userManager.FindByIdAsync(id);
        }

        if (user == null)
        {
            return NotFound();
        }

        var animeTrackings = await _context.UserAnimeTrackings
         .Include(ut => ut.Anime)
         .Where(ut => ut.UserId == user.Id)
         .ToListAsync(); // Retrieve all data first


        if (!string.IsNullOrEmpty(searchQuery))
        {
            animeTrackings = animeTrackings
                .Where(ut => ut.Anime.Title.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                            .ToList(); // Apply filtering on the client side
        }

        // var animeTrackings = await animeTrackings.ToListAsync();

        var viewModel = new ProfileViewModel
        {
            User = user,
            AnimeTrackings = animeTrackings
        };

        ViewData["SearchQuery"] = searchQuery; // Pass search query to the view for retaining input

        return View(viewModel);
    }

    public async Task<IActionResult> AnimeList()
    {
        var animes = await _animeRepository.GetAllAsync();
        var genres = await _context.Genres.Select(g => g.Name).Distinct().ToListAsync();

        var viewModel = new AnimeListViewModel
        {
            Genres = genres,
            Animes = animes.Select(a => new AnimeViewModel
            {
                Id = a.Id,
                Title = a.Title,
                CoverImagePath = a.CoverImagePath,
                Genres = a.AnimeGenres.Select(ag => ag.Genre.Name).ToList(),
                TotalEpisodes = a.TotalEpisodes,
                AverageRating = a.UserTrackings.Any()
                    ? (double)a.UserTrackings.Average(ut => ut.UserRating)
                    : 0
            }).ToList()
        };

        return View(viewModel);
    }


    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return View(new List<Anime>());
        }

        var lowerCaseQuery = query.ToLower();

        var searchResults = await _context.Animes
            .Where(a => a.Title.ToLower().Contains(lowerCaseQuery) || a.Synopsis.ToLower().Contains(lowerCaseQuery))
            .ToListAsync();

        return View(searchResults);
    }








    // ... other methods ...
}