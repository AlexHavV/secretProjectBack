using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecretProjectBack.Entity.Product
{
    [Table("AppProduct")]
    public class AppProduct
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string ProductName { get; set; }
        [Required, StringLength(255)]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<AppProductImage> ProductImages { get; set; }
    }
}
