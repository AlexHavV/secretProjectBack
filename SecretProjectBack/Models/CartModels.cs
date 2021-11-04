using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretProjectBack.Models
{
    public class CartGetProductsModel
    {
        public int UserId  { get; set; }
    }
    public class CartConfirmOrderModel
    {
        public int UserId { get; set; }
    }

    public class CartViewModel : ProductViewModel
    {
        public decimal Amount { get; set; }
    }

    public class CartGeneralModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    //public class CartItem
    //{
    //    public int Id { get; set; }
    //    public int Amount { get; set; }
    //}
}
