using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrnekSite.DataAccess.Repository.IRepository;
using OrnekSite.Diger;
using OrnekSite.Models;
using OrnekSite.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrnekSite.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            var claimsIdentity = (ClaimsIdentity)User.Identity; 
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim!=null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(i=>i.Id==id,includeProperties:"Category"); // sadece bir ürün bilgisi gelicektir
            ShoppingCart cart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id
            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart Scart)
        {
            Scart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity; // giriş yapan kullanıcıyı buluyoruz
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                Scart.ApplicationUserId = claim.Value;
                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                    u => u.ApplicationUserId == Scart.ApplicationUserId &&
                    u.ProductId == Scart.ProductId,
                    includeProperties: "Product"
                    );
                if (cartFromDb==null)// bu kart boş ise bu kart veritabanına eklenicek
                {
                    _unitOfWork.ShoppingCart.Add(Scart);
                }
                else
                {
                    cartFromDb.Count += Scart.Count; // var olan sepete ürün eklenicektir
                }
                _unitOfWork.Save();
                var count = _unitOfWork.ShoppingCart.GetAll(i => i.ApplicationUserId == Scart.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == Scart.Id, includeProperties: "Category"); // sadece bir ürün bilgisi gelicektir
                ShoppingCart cart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };
            }
            
            return View(Scart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
