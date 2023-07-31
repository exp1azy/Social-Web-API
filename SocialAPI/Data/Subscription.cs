using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialAPI.Data
{
    [Table("Subscription")]
    public class Subscription
    {
        [Key]
        public int Id { get; set; }
        [Column("Responder_id")] public int ResponderId { get; set; }
        [Column("Follower_id")] public int FollowerId { get; set; }
        public DateTime Date { get; set; }

        public virtual User Responder { get; set; }
        public virtual User Follower { get; set; }
    }
}