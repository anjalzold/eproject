
namespace eproject.Areas.Admin.Models;
public class AnimeComment

{

    public int Id { get; set; }

    public string UserId { get; set; }
    public int AnimeId { get; set; }

    public string CommentText { get; set; }

    public DateTime CreatedAt { get; set; }


    public User User { get; set; }

    public Anime Anime { get; set; }

}