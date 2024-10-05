using System;
using System.Collections.Generic;

namespace eproject.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int TotalAnime { get; set; }
        public int TotalUsers { get; set; }
        public int RecentlyAddedAnime { get; set; }
        public int ActiveUsers { get; set; }
        public List<AnimePopularityDto> AnimePopularity { get; set; }
        public List<RecentUpdateDto> RecentUpdates { get; set; }
    }

    public class AnimePopularityDto
    {
        public string Title { get; set; }

        public decimal Popularity { get; set; }
    }

    public class RecentUpdateDto
    {
        public string Title { get; set; }
        public string UpdateType { get; set; }
        public DateTime Date { get; set; }
    }
}