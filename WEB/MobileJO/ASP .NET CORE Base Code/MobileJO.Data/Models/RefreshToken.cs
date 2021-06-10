using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MobileJO.Data.Models
{
    public class RefreshToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("IssuedUtc")]
        public DateTime IssuedUtc { get; set; }

        [Column("ExpiresUtc")]
        public DateTime ExpiresUtc { get; set; }

        [Column("Token")]
        public string Token { get; set; }

        [Column("Username")]
        public string Username { get; set; }
    }
}
