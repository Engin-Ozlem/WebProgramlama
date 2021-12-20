using OrnekSite.DataAccess.Data;
using OrnekSite.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrnekSite.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }
        public ISP_Call SP_Call { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            SP_Call = new SP_Call(_db);
            Product = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
        }
        public void Dispose()
        {
            _db.Dispose();
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
