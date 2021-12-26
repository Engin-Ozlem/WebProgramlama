using Microsoft.AspNetCore.Mvc;
using OrnekSite.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrnekSite.Areas.Customer.Views.Home.Components
{
    public class ProductList:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductList(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int? kategoriId)
        {
            if (kategoriId.HasValue)
            {
                return View(_unitOfWork.Product.GetProductByCategory((int)kategoriId));
            }
            var product = _unitOfWork.Product.GetAll();
            return View(product);
        }
    }
}
