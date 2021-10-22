using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretProjectBack.Models
{
    public class CartGetProductsModel
    {
        public List<CartItem> CartItems  { get; set; }
    }

    public class CartConfirmModel
    {
        public List<CartConfirmItemModel> CartItems { get; set; }
    }
    public class CartConfirmItemModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
    }

    public class CartItem
    {
        public int Id { get; set; }
        public int Amount { get; set; }
    }
}
