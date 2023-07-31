using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Data
{
    [Table("Message")]
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Column("Sender_id")] public int SenderId { get; set; }
        [Column("Receiver_id")] public int ReceiverId { get; set; }
        public string Text { get; set; }
        [Column("Sending_date")] public DateTime Date { get; set; }
    }
}