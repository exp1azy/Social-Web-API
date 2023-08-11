using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Models
{
    public class LikeModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public bool IsLiked { get; set; }

        public DateTime Date { get; set; }
    }
}
