using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eproject.Areas.Admin.Models
{
    public class Episode
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int AnimeId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Episode number must be at least 1")]
        public int EpisodeNumber { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string? VideoPath { get; set; }

        public DateTime UploadDate { get; set; }

        public Anime? Anime { get; set; }
    }
}