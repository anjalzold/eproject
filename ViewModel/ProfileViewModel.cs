using eproject.Areas.Admin.Models;

namespace eproject.ViewModels
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<UserAnimeTracking> AnimeTrackings { get; set; }
    }
}