// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using eproject.Models;
// using eproject.Areas.Admin.Repository.Interface;
// using eproject.Areas.Admin.Models;
// using eproject.Data;

// namespace eproject.Areas.Admin.Controllers;

// [Area("Admin")]
// public class EpisodeController : Controller
// {


//     private readonly IAnimeRepository _animeRepository;
//     private readonly AppDbContext _context;

//     private readonly IEpisode _episodeRepository;


//     public EpisodeController(IAnimeRepository animeRepository, AppDbContext context, IEpisode episodeRepository)

//     {

//         _animeRepository = animeRepository;
//         _context = context;
//         _episodeRepository = episodeRepository;

//     }


//     public IActionResult Index()
//     {

//         return View();
//     }
//     public async Task<IActionResult> Create()

//     {

//         ViewData["AnimeId"] = new SelectList(await _context.Animes.ToListAsync(), "Id", "Title");

//         return View();

//     }






//     // GET: Anime/Details/5

//     // public IActionResult Details(int id)
//     //
//     //     {
//     //
//     //         var anime = _animeRepository.GetById(id);
//     //
//     //         if (anime == null)
//     //
//     //         {
//     //
//     //             return NotFound();
//     //
//     //         }
//     //
//     //         return View(anime);
//     //
//     //
//     //     }


//     // GET: Anime/Create

//     [HttpGet]
//     public IActionResult Create()
//     {
//         ViewBag.Genres = _context.Genres.ToList();
//         return View();
//     }

//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> Create(Anime anime, List<int> GenreIds, IFormFile coverImage)
//     {
//         try
//         {
//             if (ModelState.IsValid)
//             {
//                 if (coverImage != null && coverImage.Length > 0)
//                 {
//                     var fileName = Path.GetFileName(coverImage.FileName);
//                     var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
//                     var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "animes");
//                     var filePath = Path.Combine(uploadsFolder, uniqueFileName);

//                     Directory.CreateDirectory(uploadsFolder);

//                     using (var fileStream = new FileStream(filePath, FileMode.Create))
//                     {
//                         await coverImage.CopyToAsync(fileStream);
//                     }

//                     anime.CoverImagePath = "/images/animes/" + uniqueFileName;
//                 }
//                 else
//                 {
//                     anime.CoverImagePath = "/images/animes/goku.jpg";
//                 }

//                 foreach (var genreId in GenreIds)
//                 {
//                     anime.AnimeGenres.Add(new AnimeGenre { GenreId = genreId });
//                 }

//                 _animeRepository.Add(anime);
//                 await _context.SaveChangesAsync();
//                 return RedirectToAction(nameof(Index));
//             }
//             else
//             {
//                 // Log validation errors
//                 foreach (var modelState in ModelState.Values)
//                 {
//                     foreach (var error in modelState.Errors)
//                     {
//                         // Log the error.ErrorMessage
//                         Console.WriteLine(error.ErrorMessage); // Replace with proper logging
//                     }
//                 }
//             }
//         }
//         catch (Exception ex)
//         {
//             // Log the exception
//             Console.WriteLine(ex.Message); // Replace with proper logging
//             ModelState.AddModelError("", "An error occurred while saving the anime. Please try again.");
//         }

//         ViewBag.Genres = _context.Genres.ToList();
//         return View(anime);
//     }
//     // GET: Anime/Edit/5

//     public IActionResult Edit(int id)

//     {

//         var anime = _animeRepository.GetById(id);

//         if (anime == null)

//         {

//             return NotFound();

//         }

//         return View(anime);

//     }


//     // POST: Anime/Edit/5

//     [HttpPost]

//     [ValidateAntiForgeryToken]

//     public IActionResult Edit(int id, Anime anime)

//     {

//         if (id != anime.Id)

//         {

//             return NotFound();

//         }


//         if (ModelState.IsValid)

//         {

//             _animeRepository.Update(anime);

//             return RedirectToAction(nameof(Index));

//         }

//         return View(anime);

//     }


//     // GET: Anime/Delete/5

//     // public IActionResult Delete(int id)
//     //
//     // {
//     //
//     //     var anime = _animeRepository.GetById(id);
//     //
//     //     if (anime == null)
//     //
//     //     {
//     //
//     //         return NotFound();
//     //
//     //     }
//     //
//     //     return View(anime);
//     //
//     // }


//     // POST: Anime/Delete/5

//     [HttpPost, ActionName("Delete")]

//     [ValidateAntiForgeryToken]

//     public IActionResult DeleteConfirmed(int id)

//     {

//         var anime = _animeRepository.GetById(id);

//         _animeRepository.Remove(anime);

//         return RedirectToAction(nameof(Index));

//     }

//     // public IActionResult Privacy()
//     // {
//     //     return View();
//     // }

//     // public IActionResult AddAnime(){

//     //     return View();
//     // }

//     // public IActionResult UpdateAnime(){

//     //     return View();  
//     // }

//     // public IActionResult ListAnime(){

//     //     return View();

//     // }



//     public IActionResult UploadEpisode(int id)

//     {

//         var anime = _animeRepository.GetById(id);

//         if (anime == null)

//         {

//             return NotFound();

//         }

//         ViewBag.AnimeId = id;

//         ViewBag.AnimeTitle = anime.Title;

//         return View();

//     }


//     // POST: Anime/UploadEpisode/5

//     [HttpPost]

//     [ValidateAntiForgeryToken]

//     public async Task<IActionResult> UploadEpisode(int id, Episode episode, IFormFile videoFile)

//     {

//         if (ModelState.IsValid)

//         {

//             if (videoFile != null && videoFile.Length > 0)

//             {

//                 var fileName = Path.GetFileName(videoFile.FileName);

//                 var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "videos", fileName);

//                 using (var fileStream = new FileStream(filePath, FileMode.Create))

//                 {

//                     await videoFile.CopyToAsync(fileStream);

//                 }

//                 episode.VideoPath = "/videos/" + fileName;

//             }


//             episode.AnimeId = id;

//             episode.UploadDate = DateTime.Now;

//             _episodeRepository.Add(episode);

//             // return RedirectToAction(nameof(Details), new { id = id });

//             return RedirectToAction(nameof(ListAnime));

//         }

//         ViewBag.AnimeId = id;

//         return View(episode);

//     }

// }


