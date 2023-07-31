using SocialAPI.Data;

namespace SocialAPI.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public static PostModel? Map(Post post) => post == null ? null : new PostModel
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            Text = post.Text,
            Date = post.Date
        };
    }
}
