using BloodHeroA.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BloodHeroA.DTOs
{
    public class NotificationDTO
    {
        [Required]
        public string ReceiverEmail { get; set; } = default!;
        public string Subject { get; set; } = default!;
        [Required]
        public string Message { get; set; } = default!;

    }

    public class NotificationResponseDto
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = default!;
        public string? SenderName { get; set; }
        public string Message { get; set; } = default!;
        public DateTime SentDate { get; set; }
        public string? SenderEmail { get; set; }
        public bool IsRead { get; set; }
        public Role Role { get; set; }
    }
}
