using eproject.Areas.Admin.Models;
using System.Collections.Generic;

namespace eproject.ViewModels
{
    public class EpisodeListViewModel
    {
        public Anime Anime { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }
    }
}