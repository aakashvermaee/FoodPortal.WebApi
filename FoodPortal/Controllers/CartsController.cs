using FoodPortal.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace FoodPortal.Controllers {
    //[EnableCors(origins: "*", methods: "*", headers: "*")]
    public class CartsController : ApiController
    {
        private FoodOrderingDbEntities db;
        public CartsController()
        {
            this.db = new FoodOrderingDbEntities();
        }
        public CartsController(FoodOrderingDbEntities _db) {
            db = _db;
        }

        [HttpPost]
        public JsonResult<List<Product>> GetCarts(Cart cart) {
            List<Cart> clientidincart = (from item in db.Carts
                                         where item.ClientId.Equals(cart.ClientId)
                                         select item).ToList();
            List<Product> products = (from item in db.Products select item).ToList();
            List<Product> result = new List<Product>(10);
            IEnumerator<Product> ie = products.GetEnumerator();
            IEnumerator<Cart> ie2 = clientidincart.GetEnumerator();
            ie2.Reset();
            while (ie2.MoveNext()) {
                while (ie.MoveNext()) {
                    if (ie.Current.ProductId == ie2.Current.ProductId) {
                        result.Add(ie.Current);
                    }
                }
                ie.Reset();
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult<string> PostCart(Cart cart) {
            List<Cart> records = (from p in db.Carts select p).ToList();
            bool check = this.checkExists(records, cart);
            //if true then don't '!' add
            if (!check) {
                db.Carts.Add(cart);
                db.SaveChanges();
                return Json("Added to the Cart!");
            }
            return Json("Product already in the cart!");
        }  

        //fun to check whether a product with current clientid and productid 
        //already exists in the cart table
        //return true if exists or false
        private bool checkExists(ICollection<Cart> collection, Cart cart) {
            IEnumerator<Cart> ie = collection.GetEnumerator();
            while (ie.MoveNext()) {
                if (cart.ProductId == ie.Current.ProductId.Value && cart.ClientId.Equals(ie.Current.ClientId)) {
                    return true;
                }
            }
            return false;
        }

        [HttpPost]
        public JsonResult<string> PostQuantity(Cart cart) {
            var v = (from c in db.Carts
                    where c.ProductId == cart.ProductId && c.ClientId.Equals(cart.ClientId)
                    select c).ToList();
            v.First().Quantity = cart.Quantity;
            db.SaveChanges();
            return Json("success");
        }

        [HttpDelete]
        public JsonResult<string> DeleteProductFromCart(Cart cart) {
            try {
                List<Cart> delete = (from c in db.Carts
                               where c.ProductId == cart.ProductId
                               select c).ToList();
                db.Carts.Remove(delete.First());
                db.SaveChanges();
                return Json("Product Deleted!");
            } catch (System.Exception e) { }
            return Json("Product Not Deleted!");
        }
    }
}
