using OrnekSite.DataAccess.Data;
using OrnekSite.DataAccess.Repository.IRepository;
using OrnekSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrnekSite.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetails nesne)
        {
            _db.Update(nesne);
        }
    }
}
