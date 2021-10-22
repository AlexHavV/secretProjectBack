using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using SecretProjectBack.Entity.Product;
using SecretProjectBack.Entity.User;

namespace SecretProjectBack.Entity.Cart
{
    [Table("AppCart")]
    public class AppCart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        [Required]
        public Int64 UserId { get; set; }
        [ForeignKey("AppProduct")]
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public bool IsPayed { get; set; }
        public virtual AppUser User { get; set; }
        public virtual AppProduct Product { get; set; }
    }
}
