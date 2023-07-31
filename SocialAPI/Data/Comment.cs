using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Data
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Column("Post_id")] public int PostId { get; set; }
        [Column("Commentator_id")] public int CommentatorId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}