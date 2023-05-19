namespace chatService.Data.Entities
{
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class GroupUsers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupUsersId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int UserId { get; set; }
        public Boolean IsAdmin { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        
    }
}
