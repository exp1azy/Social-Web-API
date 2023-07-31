using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Data
{
    [Table("Post")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        [Column("Author_id")] public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}