using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eproject.Areas.Admin.Models
{
    public class Anime
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Episodes must be at least 1")]
        public int TotalEpisodes { get; set; }

        [Required]
        public string Synopsis { get; set; }

        public string? CoverImagePath { get; set; }

        public List<AnimeGenre> AnimeGenres { get; set; } = new List<AnimeGenre>();

        public List<Episode> Episodes { get; set; } = new List<Episode>();

        public DateTime UpdatedAt { get; set; }

        public int RecentlyAddedCount { get; set; }


        public string AnimeStatus { get; set; }

        public string Studio { get; set; }

        public string? Views { get; set; }


        // ... existing properties ...

        public List<UserAnimeTracking> UserTrackings { get; set; } = new List<UserAnimeTracking>();
            public List<AnimeComment> Comments { get; set; } = new List<AnimeComment>();
        
    }

  


}