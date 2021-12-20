using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrnekSite.Models
{
   public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        [Key]
        public int Id { get; set; } // kart numarası
        public string ApplicationUserId { get; set; } // sipariş veren kullanıcı Id si
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
