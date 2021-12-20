using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrnekSite.DataAccess.Repository.IRepository;
using OrnekSite.Diger;
using OrnekSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrnekSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork; // veritabanına bağlanıyoruz
        private readonly UserManager<IdentityUser> _userManager;
        public ShoppingCartVM ShoppingCartVM { get; set; }


        public CartController(UserManager<IdentityUser> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value);
            foreach (var item in ShoppingCartVM.ListCart)
            {

                ShoppingCartVM.OrderHeader.OrderTotal += (item.Count * item.Product.Price);

            }
            return View(ShoppingCartVM);
        }
        public IActionResult Plus(int cartId) // sepetteki ürünleri arttırmak için
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId, includeProperties: "Product");
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)// sepetteki ürünler 1 adetten  küçük olduğunda silmek için 
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId, includeProperties: "Product");
            if (cart.Count==1)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count - 1);
            }
            else
            {
                cart.Count -= 1;
                _unitOfWork.Save();
            }
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)// sepetteki ürünleri Azaltmak için 
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(i => i.Id == cartId, includeProperties: "Product");
            
                var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                _unitOfWork.ShoppingCart.Remove(cart);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count - 1);
            
            

            return RedirectToAction(nameof(Index));
        }

    }
}
