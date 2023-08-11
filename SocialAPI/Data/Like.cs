using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Data
{
    [Table("Like")]
    public class Like
    {
        public int Id { get; set; }

        [Column("User_id")] public int UserId { get; set; }

        [Column("Post_id")] public int PostId { get; set; }

        [Column("Is_liked")] public bool IsLiked { get; set; }

        public DateTime Date { get; set; }
    }
}