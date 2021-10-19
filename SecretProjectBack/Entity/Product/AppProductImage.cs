using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SecretProjectBack.Entity.Product
{
    [Table("AppProductImage")]
    public class AppProductImage
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(255)]
        public string Name { get; set; }
        public int Priority { get; set; }

        [ForeignKey("AppProduct")]
        public int? ProductId { get; set; }
        public virtual AppProduct Product { get; set; }
    }
}
