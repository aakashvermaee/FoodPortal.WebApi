using FoodPortal.Data.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace FoodPortal.Controllers {
    public class OrdersController : ApiController
    {
        private FoodOrderingDbEntities db;
        public OrdersController() {
            this.db = new FoodOrderingDbEntities();
        }

        [HttpPost]
        public JsonResult<string> PostOrder(Order order) {
            //var existsUserName = (from o in db.Orders
            //                     where o.username.Equals(order.username)
            //                     select o).ToList();
            //var obj = existsUserName.First();

            //if (existsUserName != null) {
            //    if (obj.username.Equals(order.username)) {
            //        obj.Payment = order.Payment;
            //        obj.Payment_Mode = order.Payment_Mode;
            //        //db.Orders.Add(obj);
            //        db.SaveChanges();
            //        return Json("Order Changed!");
            //    }
            //}
            db.Orders.Add(order);
            db.SaveChanges();
            return Json("Order Placed!");
        }

        [HttpPost]
        public JsonResult<string> ReduceQuantity(Cart cart) {
            Product product = new Product();
            var x = db.Carts.Where(n => n.ProductId == cart.ProductId && n.ClientId.Equals(cart.ClientId));
            var changeqty = db.Products.Where(n => n.ProductId == x.FirstOrDefault().ProductId);
            changeqty.FirstOrDefault().Quantity -= cart.Quantity;
            //Product updatedQty = db.Products.Find(x.FirstOrDefault().ProductId);
            //if (updatedQty.Quantity == 0)
            //{
            //    db.Products.Remove(updatedQty);
            //}
            db.SaveChanges();
            return Json("changed qty!");
        }
    }
}
