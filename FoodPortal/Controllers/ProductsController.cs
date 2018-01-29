using FoodPortal.Data.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace FoodPortal.Controllers {
    //[EnableCors(origins:"*", methods:"*", headers:"*")]
    public class ProductsController : ApiController
    {
        //DbContext Class
        private FoodOrderingDbEntities db;

        public ProductsController()
        {
            this.db = new FoodOrderingDbEntities();
        }

        public ProductsController(FoodOrderingDbEntities _db) {
            db = _db;
        }

        [HttpGet]
        [ResponseType(typeof(JsonResult<DbSet<Product>>))]
        public JsonResult<DbSet<Product>> GetProducts() {
            return Json(db.Products);
        }

        [HttpPost]
        public JsonResult<List<Product>> GetCategoryProduct(Product productCategory) {
            List<Product> productsUnderCategory = (from eachproduct in db.Products
                                                   where eachproduct.Category.Equals(productCategory.Category)
                                                   select eachproduct).ToList();
            return Json(productsUnderCategory);
        }

        [HttpPost]
        public JsonResult<List<Product>> SearchProduct(Product product) {
            if (product.Name.Equals("")) {
                return Json(new List<Product>(0));
            }
            List<Product> searchProductDb = db.Products
                                              .Where(P => P.Name.Contains(product.Name))
                                              .ToList();
            return Json(searchProductDb);
        }

        [HttpPost]
        public JsonResult<List<Product>> GetOffers(Product product) {
            List<Product> eachOffer = (from offers in db.Products
                                       where offers.Offer == true
                                       select offers).ToList();
            return Json(eachOffer);
        }

        [HttpPost]
        public JsonResult<string> PostProduct(Product product) {
            try {
                db.Products.Add(product);
                db.SaveChanges();
                return Json(product.Name + " Added Successfully!");
            } catch (System.Exception e) { }
            return Json(product.Name + "Failed!");
        }

        [HttpDelete]
        public JsonResult<string> DeleteProduct(Product product) {
            try {
                List<Product> p = (from P in db.Products
                                   where P.ProductId == product.ProductId
                                   select P).ToList();
                db.Products.Remove(p.First());
                db.SaveChanges();
                return Json(product.Name + " Removed Successfully!");
            } catch (System.Exception e) { }
            return Json(product.Name + "Failed!");
        }

        [HttpPost]
        public JsonResult<List<Product>> CheckQuantity(Product product) {
            List<Product> p = (from P in db.Products
                                where P.Quantity <= product.Quantity
                                select P).ToList();
            return Json(p);
        }
    }
}