using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedServices.Models
{
    public class User
    {
        [Key]
        public int UserRecId { get; set; }

        [MaxLength(100)]
        public string UserId { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public bool IsAdminUser { get; set; }

        [NotMapped]
        public string Token { get; set; }
    }
}
