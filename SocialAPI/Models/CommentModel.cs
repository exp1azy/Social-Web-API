namespace SocialAPI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int AuthorId { get; set; }
        public int CommentatorId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
