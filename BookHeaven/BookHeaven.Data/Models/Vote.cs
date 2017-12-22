using BookHeaven.Data.Models.Enums;

namespace BookHeaven.Data.Models
{
    public class Vote
    {
        public int BookId { get; set; }

        public Book Book { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public VoteValue? VoteValue { get; set; }
    }
}