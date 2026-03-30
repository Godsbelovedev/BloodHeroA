namespace BloodHeroA.Models.Entities
{
    public class Notification
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public string Message { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public DateTime DateSent { get; set; } = DateTime.UtcNow;
    }
}
