namespace chatService.Data.Entities
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Messages
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? Text { get; set; }
        public int? Reactions { get; set; }
        public DateTime? createdOn { get; set; } = DateTime.Now;
        public int? createdBy { get; set; }
    }
}
